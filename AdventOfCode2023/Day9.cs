using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day9 : Day
    {

        public void Part1()
        {
            string[] lines = File.ReadAllLines("input9.txt");

            int total = 0;
            foreach (string line in lines)
            {
                string[] numstr = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                List<int> num = new List<int>();
                foreach (string str in numstr)
                {
                    num.Add(int.Parse(str));
                }

                total += ProcessLine(num);
            }

            Console.WriteLine(total);
        }

        int ProcessLine(List<int> nums)
        {
            List<List<int>> allLines = new List<List<int>>();

            allLines.Add(nums);

            while (true) {
                List<int> line = new List<int>();
                for (int i = 0; i < allLines.Last().Count - 1; i++)
                {
                    line.Add(allLines.Last()[i + 1] - allLines.Last()[i]);
                }
                allLines.Add(line);
                if (line.Distinct().Count() == 1 && line[0] == 0)
                    break;
            }

            allLines.Last().Add(0);
            for (int i = allLines.Count - 2; i >= 0; i--)
            {
                int diff = allLines[i+1].Last();
                allLines[i].Add(allLines[i].Last() + diff);
            }

            return allLines[0].Last();
        }

        public void Part2()
        {
            string[] lines = File.ReadAllLines("input9.txt");

            int total = 0;
            foreach (string line in lines)
            {
                string[] numstr = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                List<int> num = new List<int>();
                foreach (string str in numstr)
                {
                    num.Add(int.Parse(str));
                }

                total += ProcessLineV2(num);
            }

            Console.WriteLine(total);
        }

        int ProcessLineV2(List<int> nums)
        {
            List<List<int>> allLines = new List<List<int>>();

            allLines.Add(nums);

            while (true)
            {
                List<int> line = new List<int>();
                for (int i = 0; i < allLines.Last().Count - 1; i++)
                {
                    line.Add(allLines.Last()[i + 1] - allLines.Last()[i]);
                }
                allLines.Add(line);
                if (line.Distinct().Count() == 1 && line[0] == 0)
                    break;
            }

            allLines.Last().Add(0);
            for (int i = allLines.Count - 2; i >= 0; i--)
            {
                int diff = allLines[i + 1].First();
                allLines[i].Insert(0, allLines[i].First() - diff);
            }

            return allLines[0].First();
        }
    }
}
