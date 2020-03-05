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

        private void evaluatePopulation()
        {
            for (int i = 0; i < population.Count; i++)
            {
                if (population[i].Fitness == 0.0)
                    population[i].Fitness = problem.GetFitness(population[i]);
                
                if (population[i].Fitness > BestIndividual.Fitness)
                    BestIndividual = population[i];
            }
        }

        private IList<Individual> generateNewPopulation(int populationSize, double crossoverProb, int tournamentSize)
        {
            IList<Individual> newPopulation = new List<Individual>(populationSize);
            while (newPopulation.Count < populationSize)
            {
                Individual firstParent = selection.PerformSelection(population, tournamentSize);
                Individual secondParent = selection.PerformSelection(population, tournamentSize);

                if (randomGenerator.NextDouble() < crossoverProb)
                {
                    IList<Individual> children = crossover.DoCrossover(firstParent, secondParent);
                    newPopulation.Add(children[0]);
                    newPopulation.Add(children[1]);
                }
                else
                {
                    newPopulation.Add(firstParent);
                    newPopulation.Add(secondParent);
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