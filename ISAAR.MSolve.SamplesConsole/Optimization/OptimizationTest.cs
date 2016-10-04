﻿using ISAAR.MSolve.Analyzers.Optimization;
using ISAAR.MSolve.Analyzers.Optimization.Algorithms.Metaheuristics.DifferentialEvolution;
using ISAAR.MSolve.Analyzers.Optimization.Convergence;
using ISAAR.MSolve.SamplesConsole.Optimization.BenchmarkFunctions;
using System;

namespace ISAAR.MSolve.SamplesConsole.Optimization
{
    public class OptimizationTest
    {
        public static void Main()
        {
            OptimizationProblem optimizationProblem = new Ackley();

            DifferentialEvolutionAlgorithm.Builder builder = new DifferentialEvolutionAlgorithm.Builder(optimizationProblem);
            builder.PopulationSize = 100;
            builder.MutationFactor = 0.4;
            builder.CrossoverProbability = 0.9;
            builder.ConvergenceCriterion = new MaxFunctionEvaluations(100000);

            //DifferentialEvolutionAlgorithm de = new DifferentialEvolutionAlgorithm(optimizationProblem);
            IOptimizationAlgorithm de = builder.Build();
            IOptimizationAnalyzer analyzer = new OptimizationAnalyzer(de);
            analyzer.Optimize();

            // Print results
            Console.WriteLine("\n Best Position:");
            for (int i = 0; i < optimizationProblem.Dimension; i++)
            {
                Console.WriteLine(String.Format(@"  x[{0}] = {1} ", i, de.BestPosition[i]));
            }
            Console.WriteLine(String.Format(@"Best Fitness: {0}", de.BestFitness));
        }
    }
}
