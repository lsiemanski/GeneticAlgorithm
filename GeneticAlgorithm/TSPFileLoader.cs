using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithm
{
    class TSPFileLoader
    {
        private const string TSP_PATH = "./TSP";
        private string[] TSPFiles;
        private TSPReader reader = new TSPReader();

        public void LoadAndListTSPFiles()
        {
            Console.WriteLine("*********TSP Problem Instances*********");
            TSPFiles = Directory.GetFiles(TSP_PATH).Where(x => x.EndsWith(".tsp")).ToArray();
            for (int i = 1; i <= TSPFiles.Length; i++)
            {
                Console.WriteLine("[{0}] {1}", i, getProblemNameFromPath(TSPFiles[i-1]));
            }
        }

        public TSPProblem SelectAndLoadFile()
        {
            Console.Write("Select problem to solve: ");
            string selectedIndex = Console.ReadLine();
            string selectedFile = TSPFiles[Int32.Parse(selectedIndex)-1];
            return reader.ReadFile(selectedFile, getProblemNameFromPath(selectedFile));
        }

        private string getProblemNameFromPath(string filepath)
        {
            return filepath.Substring(TSP_PATH.Length + 1, filepath.Length - TSP_PATH.Length - 5);
        }
      
    }
}
