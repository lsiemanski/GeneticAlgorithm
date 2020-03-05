using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    class RandomAlgorithm : Algorithm
    {
        private const int EXECUTIONS = 10000;

        public RandomAlgorithm(TSPProblem problem) : base(problem) { }

        public override void PerformAlgorithm()
        {
            IList<Individual> individuals = new List<Individual>(EXECUTIONS);
            for(int i = 0; i < EXECUTIONS; i++)
            {
                Individual newIndividual = Problem.GenerateRandomIndividual();
                newIndividual.Fitness = Problem.GetFitness(newIndividual);
                individuals.Add(newIndividual);
            }

            evaluateResults(individuals.Select(x => x.Fitness).ToArray());
        }
    }
}
