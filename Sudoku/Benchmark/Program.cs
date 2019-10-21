using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using Sudoku.Core;

namespace Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Benchmark1();

            Console.ReadLine();
        }


        static void Benchmark1()
        {
	        

			var solvers = new List<ISudokuSolver>();


           
            foreach (var file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory))
			{
                if (file.EndsWith("dll") && !( Path.GetFileName(file).StartsWith("libz3")))
				{
					var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file);
					foreach (var type in assembly.GetTypes())
					{
						if (typeof(ISudokuSolver).IsAssignableFrom(type) && !(typeof(ISudokuSolver) == type))
						{
                            var solver = (ISudokuSolver)Activator.CreateInstance(type);
							solvers.Add(solver);
						}
					}
				}

			}

			var allSudokus = LoadEasy();
	        var premierSudoku = allSudokus[0];

			Console.WriteLine(premierSudoku.ToString());
            TimeSpan tempsTotal = new TimeSpan();

	        var chrono = new Stopwatch();

            foreach (var sudokuSolver in solvers)
			{
                foreach (var unsudoku in allSudokus)
                {
                    var aResoudre = new Sudoku.Core.Sudoku() { Cells = new List<int>(unsudoku.Cells) };
                    chrono.Restart();
                    var resolu = sudokuSolver.Solve(aResoudre);
                    var tempsPasse = chrono.Elapsed;
                    tempsTotal += tempsPasse;
                    Console.WriteLine(sudokuSolver.GetType().Name);
                    Console.WriteLine(resolu.ToString());
                    Console.WriteLine(tempsPasse.ToString());
                }
                Console.WriteLine(String.Concat("temps total de résolution pour ", sudokuSolver.GetType().Name, " :", tempsTotal.ToString()));
			}      
        }



		static void DisplayEasy()
        {
	        var allSudokus = LoadEasy();
	        for (int i = 0; i < allSudokus.Count; i++)
	        {
		        Sudoku.Core.Sudoku sudoku = (Sudoku.Core.Sudoku)allSudokus[i];
		        Console.WriteLine(sudoku.ToString());
	        }

		}


		static List<Sudoku.Core.Sudoku> LoadEasy()
        {
            string dataDirectory = @"../../../../../data";
            var sudokupath = Path.Combine(dataDirectory + @"/Sudoku_top95.txt");


            List<Sudoku.Core.Sudoku> allSudokus = Sudoku.Core.Sudoku.ParseFile(sudokupath);
	        return allSudokus;
        }

	}
}