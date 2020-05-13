using System;
using System.Collections.Generic;
using System.Globalization;

namespace GeneticAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            TSPFileLoader fileLoader = new TSPFileLoader();
            fileLoader.LoadAndListTSPFiles();
            TSPProblem problem = fileLoader.SelectAndLoadFile();
            //problem.PrintDistanceMatrix();

            Console.WriteLine("\nRandom Algorithm");
            Algorithm randomAlgorithm = new RandomAlgorithm(problem);
            randomAlgorithm.PerformAlgorithm();
            printAlgorithmResults(randomAlgorithm);

            Console.WriteLine("\nGreedy Algorithm");
            Algorithm greedyAlgorithm = new GreedyAlgorithm(problem);
            greedyAlgorithm.PerformAlgorithm();
            printAlgorithmResults(greedyAlgorithm);

            GeneticAlgorithmParameters parameters = readParameters();

            Algorithm geneticAlgorithm = new GeneticAlgorithm(problem) 
            { 
                Parameters = parameters, 
                Selection = new TournamentSelection(), 
                Crossover = new OrderedCrossover(), 
                Mutation = new InversionMutation() 
            };
            geneticAlgorithm.PerformAlgorithm();
            printAlgorithmResults(geneticAlgorithm);
        }

        private static GeneticAlgorithmParameters readParameters()
        {
            Console.WriteLine("\nGenetic algorithm parameters setup");
            Console.Write("Population size: ");
            string populationSize = Console.ReadLine();
            Console.Write("Generations: ");
            string generations = Console.ReadLine();
            Console.Write("Crossover probability: ");
            string crossoverProb = Console.ReadLine();
            Console.Write("Mutation probability: ");
            string mutationProb = Console.ReadLine();
            Console.Write("Tournament size: ");
            string tournamentSize = Console.ReadLine();

            return new GeneticAlgorithmParameters
            {
                PopulationSize = Int32.Parse(populationSize),
                Generations = Int32.Parse(generations),
                CrossoverProb = Double.Parse(crossoverProb, CultureInfo.InvariantCulture),
                MutationProb = Double.Parse(mutationProb, CultureInfo.InvariantCulture),
                TournamentSize = Int32.Parse(tournamentSize)
            };
        }

        private static void printAlgorithmResults(Algorithm algorithm)
        {
            Console.WriteLine("Best fitness: {0:.##}", algorithm.BestFitness);
            Console.WriteLine("Worst fitness: {0:.##}", algorithm.WorstFitness);
            Console.WriteLine("Average fitness: {0:.##}", algorithm.AverageFitness);
            Console.WriteLine("Standard deviation: {0:.##}", algorithm.StandardDeviation);
        }
    }
}
