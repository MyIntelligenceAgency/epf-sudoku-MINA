using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sudoku.Core
{
    public class Sudoku
    {
        private static readonly int[] Indices = Enumerable.Range(0, 9).ToArray();

        // The List property makes it easier to manipulate cells,
        public List<int> Cells { get; set; } = Enumerable.Repeat(0, 81).ToList();



        /// Displays a Sudoku in an easy-to-read format
        public override string ToString()
        {
            var lineSep = new string('-', 31);
            var blankSep = new string(' ', 8);

            var output = new StringBuilder();
            output.Append(lineSep);
            output.AppendLine();

            for (int row = 1; row <= 9; row++)
            {
                output.Append("| ");
                for (int column = 1; column <= 9; column++)
                {

                    var value = Cells[(row - 1) * 9 + (column - 1)];

                    output.Append(value);
                    if (column % 3 == 0)
                    {
                        output.Append(" | ");
                    }
                    else
                    {
                        output.Append("  ");
                    }
                }

                output.AppendLine();
                if (row % 3 == 0)
                {
                    output.Append(lineSep);
                }
                else
                {
                    output.Append("| ");
                    for (int i = 0; i < 3; i++)
                    {
                        output.Append(blankSep);
                        output.Append("| ");
                    }
                }
                output.AppendLine();
            }

            return output.ToString();
        }



        /// Parses a single Sudoku
        public static Sudoku Parse(string sudokuAsString)
        {
            return ParseMulti(new[] { sudokuAsString })[0];
        }



        /// Parses a file with one or several sudokus
        public static List<Sudoku> ParseFile(string fileName)
        {
            return ParseMulti(File.ReadAllLines(fileName));
        }



        /// Parses a list of lines into a list of sudoku, accounting for most cases usually encountered
        public static List<Sudoku> ParseMulti(string[] lines)
        {
            var toReturn = new List<Sudoku>();
            var cells = new List<int>(81);
            foreach (var line in lines)
            {
                if (line.Length > 0)
                {
                    if (char.IsDigit(line[0]) || line[0] == '.' || line[0] == 'X' || line[0] == '-')
                    {
                        foreach (char c in line)
                        {
                            int? cellToAdd = null;
                            if (char.IsDigit(c))
                            {
                                var cell = (int)Char.GetNumericValue(c);
                                cellToAdd = cell;
                            }
                            else
                            {
                                if (c == '.' || c == 'X' || c == '-')
                                {
                                    cellToAdd = 0;
                                }
                            }

                            if (cellToAdd.HasValue)
                            {
                                cells.Add(cellToAdd.Value);
                                if (cells.Count == 81)
                                {
                                    toReturn.Add(new Sudoku() { Cells = new List<int>(cells) });
                                    cells.Clear();
                                }
                            }
                        }
                    }
                }
            }

            return toReturn;
        }
    }
}
