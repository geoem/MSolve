﻿using ISAAR.MSolve.Analyzers.Optimization.Algorithms.Metaheuristics.GeneticAlgorithms;
using ISAAR.MSolve.Analyzers.Optimization.Algorithms.Metaheuristics.GeneticAlgorithms.Encodings;
using ISAAR.MSolve.Analyzers.Optimization.Algorithms.Metaheuristics.GeneticAlgorithms.Mutations;
using ISAAR.MSolve.Analyzers.Optimization.Algorithms.Metaheuristics.GeneticAlgorithms.Recombinations;
using ISAAR.MSolve.Analyzers.Optimization.Convergence;
using ISAAR.MSolve.Analyzers.Optimization.Output;
using ISAAR.MSolve.Analyzers.Optimization.Problems;
using ISAAR.MSolve.SamplesConsole.Optimization.BenchmarkFunctions;
using System;
using System.Linq;

namespace ISAAR.MSolve.SamplesConsole.Optimization
{
    class OptimizationTestGA
    {
        public static void Run()
        {
            // Define optim problem
            OptimizationProblem problem = new Ackley();

            // Define optim algorithm and parameters
            var optimBuilder = new BinaryGA.Builder(problem);
            optimBuilder.Logger = new BestOfIterationLogger();
            //optimBuilder.Logger = new EmptyLogger();
            optimBuilder.ConvergenceCriterion = LogicCriteria.OR(new MaxIterations(200), new MaxFunctionEvaluations(10000));
            optimBuilder.Encoding = new GrayCodeEncoding(problem, 16, 8);
            optimBuilder.PopulationSize = 100;
            optimBuilder.Recombination = new SinglePointCrossover();
            optimBuilder.Mutation = new BitFlipMutation(0.05);

            // Start optimization
            const int repetitions = 100;
            var solutions = new double[repetitions];
            for (int rep = 0; rep < repetitions; ++rep)
            {
                var optimAlgorithm = optimBuilder.BuildAlgorithm();
                optimAlgorithm.Solve();
                solutions[rep] = optimAlgorithm.BestFitness;
                Console.WriteLine("Best objective value: " + optimAlgorithm.BestFitness);
            }
            Console.WriteLine("Average objective value: " + solutions.Average());

            //Print results
            //Console.WriteLine("----------- History -----------");
            //optimBuilder.Logger.PrintToConsole();

            //Console.WriteLine("----------- Results -----------");
            //Console.WriteLine("Best objective value: " + optimAlgorithm.BestFitness);
            //Console.WriteLine("For continuous design variables: ");
            //PrintLineArray(optimAlgorithm.BestVariables.Item1);
            //Console.WriteLine("and integer design variables: ");
            //PrintLineArray(optimAlgorithm.BestVariables.Item2);
        }
    }
}