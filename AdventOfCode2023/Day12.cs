using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day12 : Day
    {
        class Record
        {
            public string data;
            public List<int> runs = new List<int>();
            Regex rx;
            string regex;
            public int numBad;

            void CreateRegex()
            {
                regex = @"^\.*";
                for (int j = 0; j < runs.Count; j++)
                {
                    if (j > 0)
                    {
                        regex += @"\.";
                    }

                    for (int k = 0; k < runs[j]; k++)
                        regex += @"#";

                    regex += @"\.*";
                }

                regex += "$";

                rx = new Regex(regex, RegexOptions.Compiled);
            }

            public Record(string dataStr, string numsStr)
            {
                data = dataStr;
                string[] nums = numsStr.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                foreach (string i in nums)
                {
                    runs.Add(int.Parse(i));
                }
                numBad = runs.Sum();

                CreateRegex();
            }

            public int IsMatch(string str)
            {
                int r = rx.IsMatch(str) ? 1 : 0;
                //Console.WriteLine($"IsMatch: {r} Regex: {regex} String: {str}");
                return r;
            }
        }

        List<Record> records = new List<Record>();

        public void Part1()
        {
            string[] lines = File.ReadAllLines("input12.txt");

            foreach (string line in lines)
            {
                string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                Record r = new Record(parts[0], parts[1]);
                records.Add(r);
            }

            int total = 0;
            foreach (Record r in records)
            {
                int count = CountMatches(r, r.data);
                total += count;
                Console.WriteLine($"Count {count} Total {total}");
            }
        }

        int CountMatches(Record r, string data)
        {
            int badCount = 0;
            for (int b = 0; b < data.Length; b++)
            {
                if (data[b] == '#')
                    badCount++;
            }
            if (badCount > r.numBad)
                return 0;
            
            int i = data.IndexOf('?');

            if (i == -1)
            {
                return r.IsMatch(data);
            }

            int matches = 0;

            char[] charStr = data.ToCharArray();
            charStr[i] = '.';
            matches += CountMatches(r, new string(charStr));
            charStr[i] = '#';
            matches += CountMatches(r, new string(charStr));

            return matches;
        }

        public void Part2()
        {
            string[] lines = File.ReadAllLines("testinput12.txt");

            foreach (string line in lines)
            {
                string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                string part1 = "";
                string part2 = "";
                for (int i = 0; i < 5; i++)
                {
                    if (i > 0)
                    {
                        part1 += "?";
                        part2 += ",";
                    }
                    part1 += parts[0];
                    part2 += parts[1];
                }
                Record r = new Record(part1, part2);
                records.Add(r);
            }

            int total = 0;
            foreach (Record r in records)
            {
                int count = CountMatches(r, r.data);
                total += count;
                Console.WriteLine($"Count {count} Total {total}");
            }
        }
    }
}
