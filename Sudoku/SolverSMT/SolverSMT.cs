using System;
using System.Linq;
using System.Threading;
using Sudoku.Core;
using Z3.LinqBinding;

namespace solverSMTZ3
{
	public class SolverSMT : ISudokuSolver
	{

		private static readonly int[] Indices = Enumerable.Range(0, 9).ToArray();

		public Sudoku.Core.Sudoku Solve(Sudoku.Core.Sudoku s)
		{

			using (var ctx = new Z3Context())
			{

					var theorem = CreateTheorem(ctx, s);
					
					theorem.DefaultCollectionHandling = CollectionHandling.Constants;
					
					return theorem.Solve();

			}


		}




		


		/// <summary>
		/// Creates a Z3-capable theorem to solve a Sudoku
		/// </summary>
		/// <param name="context">The wrapping Z3 context used to interpret c# Lambda into Z3 constraints</param>
		/// <returns>A typed theorem to be further filtered with additional contraints</returns>
		public static Theorem<Sudoku.Core.Sudoku> CreateTheorem(Z3Context context, Sudoku.Core.Sudoku s )
		{

			var sudokuTheorem = context.NewTheorem<Sudoku.Core.Sudoku>();


			for (int i = 0; i < 81; i++)
			{
				if (s.Cells[i] != 0)
				{
					var idx = i;
					var cellValue = s.Cells[i];
					sudokuTheorem = sudokuTheorem.Where(sudoku => sudoku.Cells[idx] == cellValue);
				}
			}




			// Cells have values between 1 and 9
			for (int i = 0; i < 9; i++)
			{
				for (int j = 0; j < 9; j++)
				{
					//To avoid side effects with lambdas, we copy indices to local variables
					var i1 = i;
					var j1 = j;
					sudokuTheorem = sudokuTheorem.Where(sudoku => (sudoku.Cells[i1 * 9 + j1] > 0 && sudoku.Cells[i1 * 9 + j1] < 10));
				}
			}

			// Rows must have distinct digits
			for (int r = 0; r < 9; r++)
			{
				//Again we avoid Lambda closure side effects
				var r1 = r;
				sudokuTheorem = sudokuTheorem.Where(t => Z3Methods.Distinct(Indices.Select(j => t.Cells[r1 * 9 + j]).ToArray()));

			}

			// Columns must have distinct digits
			for (int c = 0; c < 9; c++)
			{
				//Preventing closure side effects
				var c1 = c;
				sudokuTheorem = sudokuTheorem.Where(t => Z3Methods.Distinct(Indices.Select(i => t.Cells[i * 9 + c1]).ToArray()));
			}

			// Boxes must have distinct digits
			for (int b = 0; b < 9; b++)
			{
				//On évite les effets de bords par closure
				var b1 = b;
				// We retrieve to top left cell for all boxes, using integer division and remainders.
				var iStart = b1 / 3;
				var jStart = b1 % 3;
				var indexStart = iStart * 3 * 9 + jStart * 3;
				sudokuTheorem = sudokuTheorem.Where(t => Z3Methods.Distinct(new int[]
					  {
					 t.Cells[indexStart ],
					 t.Cells[indexStart+1],
					 t.Cells[indexStart+2],
					 t.Cells[indexStart+9],
					 t.Cells[indexStart+10],
					 t.Cells[indexStart+11],
					 t.Cells[indexStart+18],
					 t.Cells[indexStart+19],
					 t.Cells[indexStart+20],
					  }
				   )
				);
			}

			return sudokuTheorem;
		}




	}
}
