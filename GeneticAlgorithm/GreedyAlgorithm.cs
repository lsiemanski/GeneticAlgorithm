using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm
{
    class GreedyAlgorithm : Algorithm
    {
        public GreedyAlgorithm(TSPProblem problem) : base(problem) { }

        public override void PerformAlgorithm()
        {
            IList<Individual> individuals = new List<Individual>();

            for (int i = 0; i < Problem.Cities.Count; i++)
            {
                Individual newIndividual = new Individual { Order = Solve(i) };
                newIndividual.Fitness = Problem.GetFitness(newIndividual);
                individuals.Add(newIndividual);
                //Console.WriteLine(newIndividual);
            }

            evaluateResults(individuals.Select(x => x.Fitness).ToArray());
        }

        private IList<int> Solve(int startingCityIndex)
        {
            IList<int> remainingCitiesIndexes = Enumerable.Range(0, Problem.Cities.Count - 1).ToList();
            IList<int> path = new List<int>();
           
            int currentCityIndex = startingCityIndex;
            path.Add(currentCityIndex);
            remainingCitiesIndexes.Remove(currentCityIndex);

            while (remainingCitiesIndexes.Count != 0)
            {
                currentCityIndex = findIndexOfClosestCity(currentCityIndex, remainingCitiesIndexes);
                path.Add(currentCityIndex);
                remainingCitiesIndexes.Remove(currentCityIndex);
            }

            return path;
        }

        private int findIndexOfClosestCity(int sourceIndex, IList<int> destinationIndexes)
        {
            double shortestDistance = Problem.DistanceMatrix[sourceIndex, destinationIndexes[0]];
            int foundCityIndex = destinationIndexes[0];
            foreach (int index in destinationIndexes)
            {
                double distance = Problem.DistanceMatrix[sourceIndex, index];
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    foundCityIndex = index;
                }
            }
            return foundCityIndex;
        }
    }
}
