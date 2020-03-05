using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    class GeneticAlgorithm : Algorithm
    {
        private const int EXECUTIONS = 10;
        public ISelection Selection;
        public ICrossover Crossover;
        public IMutation Mutation;
        public GeneticAlgorithmParameters Parameters;

        public GeneticAlgorithm(TSPProblem problem) : base(problem) { }

        public override void PerformAlgorithm()
        {
            IList<Individual> bestIndividuals = new List<Individual>(EXECUTIONS);
            GeneticAlgorithmInstance geneticAlgorithmInstance = new GeneticAlgorithmInstance(Problem, Selection, Crossover, Mutation);

            for (int i = 0; i < EXECUTIONS; i++)
            {
                geneticAlgorithmInstance.PerformAlgorithm(Parameters);
                bestIndividuals.Add(geneticAlgorithmInstance.BestIndividual);
            }

            evaluateResults(bestIndividuals.Select(x => x.Fitness).ToArray());
        }
    }
}
