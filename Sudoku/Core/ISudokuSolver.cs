using System;
namespace Sudoku.Core
{
    public interface ISudokuSolver
    {
		//A implementer pour chaque solver

		Sudoku Solve(Sudoku s);

    }
}
