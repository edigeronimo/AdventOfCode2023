using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day13 : Day
    {
        public void Part1()
        {
            DoWork(0);
        }

        public void Part2()
        {
            DoWork(1);
        }

        void DoWork(int errors)
        {
            string[] lines = File.ReadAllLines("input13.txt");

            List<string> grid = new List<string>();
            int score = 0;

            foreach (string line in lines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    grid.Add(line);
                    continue;
                }

                score += TestGrid(grid, errors);
                grid.Clear();
            }

            if (grid.Count > 0)
            {
                score += TestGrid(grid, errors);
                grid.Clear();
            }

            Console.WriteLine(score);
        }

        int TestGrid(List<string> grid, int errors)
        {
            return TestGridH(grid, errors) * 100 + TestGridV(grid, errors);
        }

        int TestGridV(List<string> grid, int errors)
        {
            for (int x = 0; x < grid[0].Length - 1; x++)
            {
                int error = 0;
                for (int xx = x + 1, xxx = x; xx < grid[0].Length && xxx >= 0; xx++, xxx--)
                {
                    for (int y = 0; y < grid.Count; y++)
                    {
                        if (grid[y][xxx] != grid[y][xx])
                        {
                            error++;
                        }
                    }
                }

                if (error == errors)
                {
                    Console.WriteLine("MatchV: " + (x + 1));
                    return x + 1;
                }
            }

            //Console.WriteLine("No Mirror Found!");
            return 0;
        }

        int TestGridH(List<string> grid, int errors)
        {
            for (int y = 0; y < grid.Count - 1; y++)
            {
                int error = 0;
                for (int yy = y + 1, yyy = y; yy < grid.Count && yyy >= 0; yy++, yyy--)
                {
                    for (int x = 0; x < grid[0].Length; x++)
                    {
                        if (grid[yyy][x] != grid[yy][x])
                        {
                            error++;
                        }
                    }
                }

                if (error == errors)
                {
                    Console.WriteLine("MatchH: " + (y + 1));
                    return y + 1;
                }
            }

            //Console.WriteLine("No Mirror Found!");
            return 0;
        }
    }
}
