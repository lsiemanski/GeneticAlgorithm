using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    class GeneticAlgorithmParameters
    {
        public int PopulationSize;
        public int Generations;
        public double CrossoverProb;
        public double MutationProb;
        public int TournamentSize;

        public override string ToString()
        {
            return string.Format("[{0}, {1}, {2}, {3}, {4}]", PopulationSize, Generations, CrossoverProb, MutationProb, TournamentSize);
        }
    }
}
