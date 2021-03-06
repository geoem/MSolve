﻿using ISAAR.MSolve.Analyzers.Optimization.Problem;
using System;
using System.Linq;

namespace ISAAR.MSolve.SamplesConsole.Optimization.BenchmarkFunctions
{
    /// <summary>
    /// Class for the Styblinski-Tang's optimization problem.
    /// <see href="https://en.wikipedia.org/wiki/Test_functions_for_optimization">Wikipedia: Test functions for optimization</see>
    /// </summary>
    public class StyblinskiTang : OptimizationProblem
    {
        public StyblinskiTang()
        {
            this.Dimension = 10;

            this.LowerBound = new double[Dimension];
            LowerBound = LowerBound.Select(i => -10.0).ToArray();

            this.UpperBound = new double[Dimension];
            UpperBound = UpperBound.Select(i => 10.0).ToArray();

            this.ObjectiveFunction = new Objective();
        }

        class Objective : IObjectiveFunction
        {
            public double Evaluate(double[] x)
            {
                double f = 0.0;

                for (int i = 0; i < x.Length; i++)
                {
                    f += Math.Pow(x[i], 4) - 16 * Math.Pow(x[i], 2) + 5 * x[i];
                }
                return f / 2.0;
            }
        }
    }
}