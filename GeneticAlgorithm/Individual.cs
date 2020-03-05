using System;
using System.Collections.Generic;

namespace GeneticAlgorithm
{
    class Individual
    {
        public IList<int> Order;
        public double Fitness;

        public override string ToString()
        {
            string value = "[";
            foreach (int number in Order)
            {
                value += number;
                value += "|";
            }

            value = value.Remove(value.Length - 1);
            value += "], ";
            value += Fitness;

            return value;
        }
    }
}
