using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    abstract class Algorithm
    {
        public TSPProblem Problem;
        public double BestFitness { get; protected set; }
        public double WorstFitness { get; protected set; }
        public double AverageFitness { get; protected set; }
        public double StandardDeviation { get; protected set; }

        public Algorithm(TSPProblem problem)
        {
            this.Problem = problem;
        }

        public abstract IList<Individual> PerformAlgorithm();

        protected void evaluateResults(double[] fitnessArray)
        {
            BestFitness = fitnessArray.Min();
            WorstFitness = fitnessArray.Max();
            AverageFitness = fitnessArray.Average();
            StandardDeviation = fitnessArray.StdDev();
        }
    }
}
