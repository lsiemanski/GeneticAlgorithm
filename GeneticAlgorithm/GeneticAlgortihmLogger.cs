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
            string newLine = string.Format("{0};{1};{2};{3}", 
                generation, 
                results.Item1.ToString("0.00"), 
                results.Item2.ToString("0.00"), 
                results.Item3.ToString("0.00"));
            csv.AppendLine(newLine);
        }

        public void WriteToFile()
        {
            string directoryName = "results";

            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            File.WriteAllText(generateFilename(directoryName), csv.ToString());
        }

        private string generateFilename(string directory)
        {
            return string.Format("{0}/{1}_{2}_{3}_{4}_{5}_{6}_{7}.csv",
                directory,
                problemName,
                DateTime.Now.ToString("yyyyMMddhhmmss.fff"),
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
