﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Troschuetz.Random;
using Troschuetz.Random.Generators;

namespace ISAAR.MSolve.Analyzers.Optimization.Commons
{
    static class RandomNumberGenerationUtilities
    {
        // TODO: make this Immutable and add a lock. 
        // See http://stackoverflow.com/questions/2706500/how-do-i-generate-a-random-int-number-in-c
        public static readonly Random sysRandom = new Random();
        public static readonly IGenerator troschuetzRandom = new StandardGenerator();

        /// <summary>
        /// Returns a random long from min (inclusive) to max (exclusive)
        /// </summary>
        /// <param name="random">The given random instance</param>
        /// <param name="min">The inclusive minimum bound</param>
        /// <param name="max">The exclusive maximum bound.  Must be greater than min</param>
        public static long NextLong(this Random random, long min, long max)
        {
            if (max <= min)
                throw new ArgumentOutOfRangeException("max", "max must be > min!");

            //Working with ulong so that modulo works correctly with values > long.MaxValue
            ulong uRange = (ulong)(max - min);

            //Prevent a modolo bias; see http://stackoverflow.com/a/10984975/238419
            //for more information.
            //In the worst case, the expected number of calls is 2 (though usually it's
            //much closer to 1) so this loop doesn't really hurt performance at all.
            ulong ulongRand;
            do
            {
                byte[] buf = new byte[8];
                random.NextBytes(buf);
                ulongRand = (ulong)BitConverter.ToInt64(buf, 0);
            } while (ulongRand > ulong.MaxValue - ((ulong.MaxValue % uRange) + 1) % uRange);

            return (long)(ulongRand % uRange) + min;
        }

        /// <summary>
        /// Returns a random long from 0 (inclusive) to max (exclusive)
        /// </summary>
        /// <param name="random">The given random instance</param>
        /// <param name="max">The exclusive maximum bound.  Must be greater than 0</param>
        public static long NextLong(this Random random, long max)
        {
            return random.NextLong(0, max);
        }

        /// <summary>
        /// Returns a random long over all possible values of long (except long.MaxValue, similar to
        /// random.Next())
        /// </summary>
        /// <param name="random">The given random instance</param>
        public static long NextLong(this Random random)
        {
            return random.NextLong(long.MinValue, long.MaxValue);
        }

        /// <summary>
        /// Shuffles an array using the Fisher-Yates algorithm. 
        /// It runs in O(n) time and shuffles in place, so no extra memory is required.
        /// </summary>
        /// <remarks> 
        /// Copied from <see cref="http://stackoverflow.com/questions/108819/best-way-to-randomize-an-array-with-net"/></remarks>
        /// <typeparam name="T">Can be anything</typeparam>
        /// <param name="array">The array to be shuffled. 
        ///                     If it is null a <see cref="NullReferenceException"/> will be thrown.</param>
        /// <param name="randomNumberGenerator">A random number generator. 
        ///                                     If none is provided, <see cref="troschuetzRandom"/> will be used.</param>
        public static void Shuffle<T>(T[] array, IGenerator randomNumberGenerator = null)
        {
            if (randomNumberGenerator == null) randomNumberGenerator = troschuetzRandom;
            int n = array.Length;
            while (n > 1)
            {
                int k = randomNumberGenerator.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
    }
}
