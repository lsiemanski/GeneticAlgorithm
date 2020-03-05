using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace GeneticAlgorithm
{
    class TSPReader
    {
        private const string EDGE_WEIGHT_TYPE = "EDGE_WEIGHT_TYPE";
        private const string NODE_COORD_SECTION = "NODE_COORD_SECTION";
        private const string EOF = "EOF";

        public TSPProblem ReadFile(string filename, string problemName)
        {
            ICountingEdge countingEdgeStrategy = new EuclideanEdgeWeight();
            IList<City> cities = new List<City>();

            using (StreamReader sr = new StreamReader(filename))
            {
                string line;
                bool citiesLoading = false;
                while ((line = sr.ReadLine()) != null)
                {
                    //Console.WriteLine("[{0}]", string.Join(", ", line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries)));
                    if (line.StartsWith(EDGE_WEIGHT_TYPE))
                    {
                        countingEdgeStrategy = selectCountingEdgeStrategy(line.Split(':')[1].Substring(1));
                    }

                    if (line.StartsWith(EOF))
                    {
                        citiesLoading = false;
                    }

                    if (citiesLoading)
                    {
                        cities.Add(
                            new City
                            {
                                Coord1 = Double.Parse(line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries)[1], CultureInfo.InvariantCulture),
                                Coord2 = Double.Parse(line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries)[2], CultureInfo.InvariantCulture)
                            });

                    }

                    if (line.StartsWith(NODE_COORD_SECTION))
                    {
                        citiesLoading = true;
                    }
                }
            }

            return new TSPProblem(cities, countingEdgeStrategy, problemName);
        }

        private ICountingEdge selectCountingEdgeStrategy(string edgeWeightType)
        {
            switch (edgeWeightType)
            {
                case "EUC_2D":
                    return new EuclideanEdgeWeight();
                case "GEO":
                    return new GeoEdgeWeight();
                default:
                    return null;
            }
        }
    }
}
