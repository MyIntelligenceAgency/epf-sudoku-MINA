using System;
using System.Collections.Generic;
using System.Text;

namespace Benchmark
{
    class TempsParSudoku
    {
        public String NomSolver { get; private set; }
        public TimeSpan TempsPasse { get; private set; }
        
        public TempsParSudoku(String nomSolver,TimeSpan tempsPasse)
        {
            NomSolver = nomSolver;
            TempsPasse = tempsPasse;
        }

        
    }
}
