using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;
using Sudoku.Core;

namespace NorvigSolver
{
    public class NorvigSolver : ISudokuSolver
    {

        public Sudoku.Core.Sudoku Solve(Sudoku.Core.Sudoku grid_sudoku)
        {
            var solution = search(parse_grid(grid_sudoku));
            Sudoku.Core.Sudoku solution_sudoku = Sudoku.Core.Sudoku.Parse(toLine(solution));

            return solution_sudoku;
        }

        public static String toLine(Dictionary<string, string> grid)
        {
            return string.Join("", grid.Select(x => x.Value).ToArray());
        }

        public static String toLine(Sudoku.Core.Sudoku grid)
        {
            return string.Join("", grid.Cells.Select(x => x.ToString()).ToArray());
        }

        public static void Main(string[] args)
        {
        }
   
        static string rows = "ABCDEFGHI";
        static string cols = "123456789";
        static string digits = "123456789";
        static string[] squares = cross(rows, cols);
        static List<string[]> unitlist= ((from c in cols select cross(rows, c.ToString()))
                .Concat(from r in rows select cross(r.ToString(), cols))
                .Concat(from rs in (new[] { "ABC", "DEF", "GHI" }) from cs in (new[] { "123", "456", "789" }) select cross(rs, cs))).ToList();
        static Dictionary<string, IGrouping<string, string[]>> units  = (from s in squares from u in unitlist where u.Contains(s) group u by s into g select g).ToDictionary(g => g.Key);
    
        static Dictionary<string, IEnumerable<string>> peers = (from s in squares from u in units[s] from s2 in u where s2 != s group s2 by s into g select g).ToDictionary(g => g.Key, g => g.Distinct());

        static string[] cross(string A, string B)
        {
            return (from a in A from b in B select "" + a + b).ToArray();
        }

        static string[][] zip(string[] A, string[] B)
        {
            var n = Math.Min(A.Length, B.Length);
            string[][] sd = new string[n][];
            for (var i = 0; i < n; i++)
            {
                sd[i] = new string[] { A[i].ToString(), B[i].ToString() };
            }
            return sd;
        }

        public static Dictionary<string, string> parse_grid(Sudoku.Core.Sudoku grid_sudoku)
        {
            string grid = toLine(grid_sudoku);

            var values = squares.ToDictionary(s => s, s => digits);

            foreach (var sd in zip(squares, (from s in grid select s.ToString()).ToArray()))
            {
                var s = sd[0];
                var d = sd[1];

                if (digits.Contains(d) && assign(values, s, d) == null)
                {
                    return null;
                }
            }
            return values;
        }


        public static Dictionary<string, string> search(Dictionary<string, string> values)
        {
            if (values == null)
            {
                return null;
            }
            if (all(from s in squares select values[s].Length == 1 ? "" : null))
            {
                return values;
            }


            var s2 = (from s in squares where values[s].Length > 1 orderby values[s].Length ascending select s).First();

            return some(from d in values[s2]
                        select search(assign(new Dictionary<string, string>(values), s2, d.ToString())));
        }


        static Dictionary<string, string> assign(Dictionary<string, string> values, string s, string d)
        {
            if (all(
                    from d2 in values[s]
                    where d2.ToString() != d
                    select eliminate(values, s, d2.ToString())))
            {
                return values;
            }
            return null;
        }


        static Dictionary<string, string> eliminate(Dictionary<string, string> values, string s, string d)
        {
            if (!values[s].Contains(d))
            {
                return values;
            }
            values[s] = values[s].Replace(d, "");
            if (values[s].Length == 0)
            {
                return null;
            }
            else if (values[s].Length == 1)
            {

                var d2 = values[s];
                if (!all(from s2 in peers[s] select eliminate(values, s2, d2)))
                {
                    return null;
                }
            }

            foreach (var u in units[s])
            {
                var dplaces = from s2 in u where values[s2].Contains(d) select s2;
                if (dplaces.Count() == 0)
                {
                    return null;
                }
                else if (dplaces.Count() == 1)
                {

                    if (assign(values, dplaces.First(), d) == null)
                    {
                        return null;
                    }
                }
            }
            return values;
        }

        static bool all<T>(IEnumerable<T> seq)
        {
            foreach (var e in seq)
            {
                if (e == null) return false;
            }
            return true;
        }

        static T some<T>(IEnumerable<T> seq)
        {
            foreach (var e in seq)
            {
                if (e != null) return e;
            }
            return default(T);
        }
    }
}
