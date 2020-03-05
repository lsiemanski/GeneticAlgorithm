using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GeneticAlgorithm
{
    class GeneticAlgortihmLogger
    {
        private GeneticAlgorithmParameters parameters;
        private string problemName;
        private StringBuilder csv;

        public GeneticAlgortihmLogger(GeneticAlgorithmParameters parameters, string problemName)
        {
            this.parameters = parameters;
            this.problemName = problemName;
            csv = new StringBuilder();
        }

        public void AppendLine(int generation, IList<Individual> population)
        {
            Tuple<double, double, double> results = getBestWorstAndAverageFitness(population);
            string newLine = string.Format("{0},{1},{2},{3}", generation, (Int32)results.Item1, results.Item2, (Int32)results.Item3);
            csv.AppendLine(newLine);
        }

        public void WriteToFile()
        {
            File.WriteAllText(generateFilename(), csv.ToString());
        }

        private string generateFilename()
        {
            return string.Format("{0}_{1}_{2}_{3}_{4}_{5}.csv", 
                DateTime.Now.ToString("yyyyMMddhhmm"), 
                parameters.PopulationSize, 
                parameters.Generations, 
                parameters.CrossoverProb, 
                parameters.MutationProb, 
                parameters.TournamentSize);
        }

        private Tuple<double, double, double> getBestWorstAndAverageFitness(IList<Individual> population)
        {
            double bestFitness = population[0].Fitness;
            double worstFitness = population[0].Fitness;
            double fitnessSum = population[0].Fitness;

            for (int i = 1; i < population.Count; i++)
            {
                Individual current = population[i];
                if (current.Fitness > bestFitness)
                    bestFitness = current.Fitness;

                if (current.Fitness < worstFitness)
                    worstFitness = current.Fitness;

                fitnessSum += current.Fitness;
            }

            double averageFitness = fitnessSum / population.Count;

            return new Tuple<double, double, double>(bestFitness, averageFitness, worstFitness);
        }
    }
}
