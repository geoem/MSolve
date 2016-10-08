﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISAAR.MSolve.Analyzers.Optimization.Algorithms.Metaheuristics.GeneticAlgorithms.Mutations;
using ISAAR.MSolve.Analyzers.Optimization.Algorithms.Metaheuristics.GeneticAlgorithms.Recombinations;
using ISAAR.MSolve.Analyzers.Optimization.Algorithms.Metaheuristics.GeneticAlgorithms.Selections;
using Troschuetz.Random;
using ISAAR.MSolve.Analyzers.Optimization.Commons;

namespace ISAAR.MSolve.Analyzers.Optimization.Algorithms.Metaheuristics.GeneticAlgorithms.PopulationStrategies
{
    class StandardPopulationStrategy<T> : IPopulationStrategy<T>
    {
        private readonly int populationSize;
        private readonly int elitesCount;
        private readonly IGenerator rng;
        
        // Need to find a better way to check elitesCount against populationSize
        public StandardPopulationStrategy(int populationSize, int elitesCount) : 
                            this(populationSize, elitesCount, RandomNumberGenerationUtilities.troschuetzRandom)
        {
        }

        public StandardPopulationStrategy(int populationSize, int elitesCount, IGenerator randomNumberGenerator)
        {
            if (populationSize < 1) throw new ArgumentException("There population size must be >= 1");
            this.populationSize = populationSize;

            if ((elitesCount < 0) || (elitesCount >= populationSize))
            {
                throw new ArgumentException("The number of elites must belong to the interval [0, populationSize-1), but was "
                    + elitesCount);
            }
            this.elitesCount = elitesCount;

            if (randomNumberGenerator == null) throw new ArgumentException("The random number generator must not be null");
            this.rng = randomNumberGenerator;
        }

        public Individual<T>[] CreateNextGeneration(Individual<T>[] originalPopulation, ISelectionStrategy<T> selection, 
                                             IRecombinationStrategy<T> recombination, IMutationStrategy<T> mutation)
        {
            Array.Sort(originalPopulation); // sorting may not always be mandatory (e.g. 1-3 elites, selection does not sort)
            int offspringsCount = populationSize - elitesCount;
            var parents = selection.Apply(originalPopulation, offspringsCount);
            // TODO: 1) Recombination strategies may require different selection strategies (e.g. 3 parents). 
            //          It would be better to pass the selection object to recombination.Apply()
            //       2) Redundant copying. A linked list would be better.
            Individual<T>[] offsprings = recombination.Apply(parents, offspringsCount);
            mutation.Apply(offsprings);

            Individual<T>[] nextPopulation = new Individual<T>[populationSize];
            Array.Copy(originalPopulation, nextPopulation, elitesCount);
            Array.Copy(offsprings, 0, nextPopulation, elitesCount, offsprings.Length);
            return nextPopulation;
        }
    }
}
