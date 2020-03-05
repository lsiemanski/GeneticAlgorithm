using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;

namespace GeneticAlgorithm
{
    interface ICountingEdge
    {
        double CountEdgeWeight(City city1, City city2);
    }

    class EuclideanEdgeWeight : ICountingEdge
    {
        public double CountEdgeWeight(City city1, City city2)
        {
            return Math.Floor(Math.Sqrt(Math.Pow(city1.Coord1 - city2.Coord1, 2) + Math.Pow(city1.Coord2 - city2.Coord2, 2)));
        }
    }

    class GeoEdgeWeight : ICountingEdge
    {
        public double CountEdgeWeight(City city1, City city2)
        {
            var firstGeoCoord = new GeoCoordinate(city1.Coord1, city1.Coord2);
            var secondGeoCoord = new GeoCoordinate(city2.Coord1, city2.Coord2);
            return Math.Floor(firstGeoCoord.GetDistanceTo(secondGeoCoord)/1000);
        }
    }

    class TSPProblem
    {
        public string ProblemName;
        public IList<City> Cities;
        public ICountingEdge CountingEdgeStrategy;
        public double[,] DistanceMatrix;

        public TSPProblem(IList<City> cities, ICountingEdge countingEdge, string problemName)
        {
            this.Cities = cities;
            this.CountingEdgeStrategy = countingEdge;
            this.ProblemName = problemName;
            InitDistanceMatrix();
        }

        public void InitDistanceMatrix()
        {
            DistanceMatrix = new double[Cities.Count, Cities.Count];
            for (int i = 0; i < Cities.Count; i++)
            {
                for (int j = 0; j < Cities.Count; j++)
                {
                    DistanceMatrix[i,j] = CountingEdgeStrategy.CountEdgeWeight(Cities[i], Cities[j]);
                }
            }
        }

        public Individual GenerateRandomIndividual()
        {
            Random randomGenerator = new Random();
            return new Individual
            {
                Order = Cities.OrderBy(x => randomGenerator.Next()).Select(x => Cities.IndexOf(x)).ToList()
            };
        }

        public double GetFitness(Individual individual)
        { 
            double fitnessValue = 0.0;
            int current = individual.Order[0];

            for (int i = 0; i < individual.Order.Count - 1; i++)
            {
                fitnessValue += CountingEdgeStrategy.CountEdgeWeight(Cities[individual.Order[i]], Cities[individual.Order[i + 1]]);
            }

            fitnessValue += CountingEdgeStrategy.CountEdgeWeight(Cities[individual.Order[individual.Order.Count - 1]], Cities[individual.Order[0]]);

            return fitnessValue;
        }

        public void PrintDistanceMatrix()
        {
            for (int i = 0; i < Cities.Count; i++)
            {
                for (int j = 0; j < Cities.Count; j++)
                {
                    Console.Write("{0, 4} | ", DistanceMatrix[i, j]);
                }
                Console.WriteLine();
            }
        }
    }
}

