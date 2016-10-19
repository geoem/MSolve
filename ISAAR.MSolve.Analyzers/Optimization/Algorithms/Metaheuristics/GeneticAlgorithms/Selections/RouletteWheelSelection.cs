﻿using ISAAR.MSolve.Analyzers.Optimization.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troschuetz.Random;
using ISAAR.MSolve.Analyzers.Optimization.Algorithms.Metaheuristics.GeneticAlgorithms.Selections.FitnessScaling;

namespace ISAAR.MSolve.Analyzers.Optimization.Algorithms.Metaheuristics.GeneticAlgorithms.Selections
{
    public class RouletteWheelSelection<T> : ISelectionStrategy<T>
    {
        private readonly IFitnessScalingStrategy<T> fitnessScaling;
        private readonly IGenerator rng;

        public RouletteWheelSelection(IFitnessScalingStrategy<T> expectationStrategy):
            this(expectationStrategy, RandomNumberGenerationUtilities.troschuetzRandom)
        {
        }

        public RouletteWheelSelection(IFitnessScalingStrategy<T> fitnessScaling, IGenerator randomNumberGenerator)
        {
            if (fitnessScaling == null) throw new ArgumentException("The expectation strategy must not be null");
            this.fitnessScaling = fitnessScaling;

            if (randomNumberGenerator == null) throw new ArgumentException("The random number generator must not be null");
            this.rng = randomNumberGenerator;
        }

        public Individual<T>[][] Apply(Individual<T>[] population, int parentGroupsCount,
                                       int parentsPerGroup, bool allowIdenticalParents)
        {
            double[] expectations = fitnessScaling.CalculateExpectations(population);
            Roulette roulette = Roulette.CreateFromPositive(expectations, rng);

            var parentGroups = new Individual<T>[parentGroupsCount][];
            for (int group = 0; group < parentGroupsCount; ++group)
            {
                parentGroups[group] = new Individual<T>[parentsPerGroup];
                for (int parent = 0; parent < parentsPerGroup; ++parent)
                {
                    Individual<T> individual = population[roulette.SpinWheelWithBall()];
                    if (!allowIdenticalParents)
                    {
                        while (parentGroups[group].Contains<Individual<T>>(individual))
                        {
                            individual = population[roulette.SpinWheelWithBall()];
                        }
                    }
                    parentGroups[group][parent] = individual;
                }
            }
            return parentGroups;
        }
    }
}
