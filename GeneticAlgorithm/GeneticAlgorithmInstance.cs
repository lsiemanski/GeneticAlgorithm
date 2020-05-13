using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm
{
    interface ISelection
    {
        Individual PerformSelection(IList<Individual> individuals, int tournamentSize);
    }

    interface ICrossover
    {
        IList<Individual> DoCrossover(Individual firstParent, Individual secondParent);
    }

    interface IMutation
    {
        void Mutate(Individual individual, double mutationProb);
    }

    class TournamentSelection : ISelection
    {
        Random randomGenerator = new Random();
        public Individual PerformSelection(IList<Individual> individuals, int tournamentSize)
        {
            IList<Individual> tournamentParticipants = individuals.OrderBy(x => randomGenerator.Next()).Take(tournamentSize).ToList();
            return tournamentParticipants.OrderBy(x => x.Fitness).ToList()[0];
        }
    }

    class RouletteSelection : ISelection
    {
        Random random = new Random();
        public Individual PerformSelection(IList<Individual> individuals, int tournamentSize)
        {
            double rouletteDrawResult = random.NextDouble();
            double maximumFitness = individuals.Max(x => x.Fitness);
            double epsilon = maximumFitness/1000;

            double[] weights = individuals.Select(x => maximumFitness - x.Fitness + epsilon).ToArray();
            double weightsSum = weights.Sum();

            double sum = 0;

            int individualIndexToReturn = 0;
            double part;

            for(int i = 0; i < individuals.Count; i++)
            {
                sum += weights[i];
                part = sum / weightsSum;
                if (sum / weightsSum > rouletteDrawResult)
                {
                    individualIndexToReturn = i;
                    break;
                }
            }

            return individuals[individualIndexToReturn];
        }
    }

    class OrderedCrossover : ICrossover
    {
        Random random = new Random();
        public IList<Individual> DoCrossover(Individual firstParent, Individual secondParent)
        {
            int start = random.Next(0, firstParent.Order.Count);
            int end = random.Next(0, firstParent.Order.Count);
            
            if (end < start)
            {
                int tmp = end;
                end = start;
                start = tmp;
            }

            IList<int> newOrder = (new int[firstParent.Order.Count]).ToList();

            for (int i = start; i < end; i++)
            {
                newOrder[i] = firstParent.Order[i];
            }

            IList<int> leftCities = secondParent.Order.Where(x => newOrder.IndexOf(x) == -1).ToList();

            int j = 0;
            for (int i = 0; i < firstParent.Order.Count; i++)
            {
                if (newOrder[i] == 0)
                {
                    newOrder[i] = leftCities[j];
                    j++;
                }
            }

            return new List<Individual> { new Individual() { Order = newOrder } };
        }
    }

    class InversionMutation : IMutation
    {
        Random random = new Random();
        public void Mutate(Individual individual, double mutationProb)
        {
            for (int i = 0; i < individual.Order.Count; i++)
            {
                if (random.NextDouble() < mutationProb)
                {
                    int randomIndex = random.Next(0, individual.Order.Count);

                    int start = randomIndex > i ? i : randomIndex;
                    int end = randomIndex > i ? randomIndex : i;

                    List<int> newOrder = individual.Order.ToList();
                    newOrder.Reverse(start, end - start);
                    individual.Order = newOrder;
                }
            }
            
        }
    }

    class SwapMutation : IMutation
    {
        Random random = new Random();
        public void Mutate(Individual individual, double mutationProb)
        {
            for (int i = 0; i < individual.Order.Count; i++)
            {
                if (random.NextDouble() < mutationProb)
                {
                    int randomIndex = random.Next(0, individual.Order.Count);

                    int tmp = individual.Order[i];
                    individual.Order[i] = individual.Order[randomIndex];
                    individual.Order[randomIndex] = tmp;
                }
            }

        }
    }

    class GeneticAlgorithmInstance
    {
        private ISelection selection;
        private ICrossover crossover;
        private IMutation mutation;
        private TSPProblem problem;
        private IList<Individual> population;
        private Random randomGenerator;
        public Individual BestIndividual { get; private set; }

        public GeneticAlgorithmInstance(TSPProblem problem, ISelection selection, ICrossover crossover, IMutation mutation)
        {
            this.problem = problem;
            this.selection = selection;
            this.crossover = crossover;
            this.mutation = mutation;
            randomGenerator = new Random();
        }

        public void PerformAlgorithm(GeneticAlgorithmParameters parameters)
        {
            GeneticAlgortihmLogger logger = new GeneticAlgortihmLogger(parameters, problem.ProblemName);
            population = generateRandomPopulation(parameters.PopulationSize);
            //population = generateGreedyPopulation(parameters.PopulationSize);
            BestIndividual = population[0];

            evaluatePopulation(); 
            logger.AppendLine(0, population);

            for (int i = 1; i <= parameters.Generations; i++)
            {
                population = generateNewPopulation(parameters.PopulationSize, parameters.CrossoverProb, parameters.TournamentSize);
                mutatePopulation(parameters.MutationProb);
                evaluatePopulation();
                logger.AppendLine(i, population);
            }

            logger.WriteToFile();
        }

        private IList<Individual> generateRandomPopulation(int populationSize)
        {
            IList<Individual> individuals = new List<Individual>(populationSize);

            for (int i = 0; i < populationSize; i++)
            {
                individuals.Add(problem.GenerateRandomIndividual());
            }

            return individuals;
        }

        private IList<Individual> generateGreedyPopulation(int populationSize)
        {
            IList<Individual> individuals = new List<Individual>(populationSize);
            GreedyAlgorithm greedyAlgorithm = new GreedyAlgorithm(problem);

            IList<Individual> greedyIndividuals = greedyAlgorithm.PerformAlgorithm();

            individuals = individuals.Concat(greedyIndividuals.OrderBy(x => x.Fitness).Take(populationSize/2)).ToList();

            for (int i = greedyIndividuals.Count; i < populationSize; i++)
            {
                individuals.Add(problem.GenerateRandomIndividual());
            }

            return individuals;
        }

        private void evaluatePopulation()
        {
            for (int i = 0; i < population.Count; i++)
            {
                population[i].Fitness = problem.GetFitness(population[i]);
                
                if (population[i].Fitness < BestIndividual.Fitness)
                    BestIndividual = population[i];
            }
        }

        private IList<Individual> generateNewPopulation(int populationSize, double crossoverProb, int tournamentSize)
        {
            IList<Individual> newPopulation = new List<Individual>(populationSize);
            //newPopulation.Add(BestIndividual);
            while (newPopulation.Count < populationSize)
            {
                Individual firstParent = selection.PerformSelection(population, tournamentSize);
                Individual secondParent = selection.PerformSelection(population, tournamentSize);

                if (randomGenerator.NextDouble() < crossoverProb)
                {
                    IList<Individual> children = crossover.DoCrossover(firstParent, secondParent);
                    newPopulation = newPopulation.Concat(children).ToList();
                }
                else
                {
                    newPopulation.Add(new Individual { Order = firstParent.Order });
                    newPopulation.Add(new Individual { Order = secondParent.Order });
                }
            }

            return newPopulation;
        }

        private void mutatePopulation(double mutationProb)
        {
            for (int i = 0; i < population.Count; i++)
            {
                mutation.Mutate(population[i], mutationProb);
            }
        }

    }
}