using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day11 : Day
    {
        List<int> galaxies = new List<int>();
        List<int> grid = new List<int>();
        int width, height;

        public void Part1()
        {
            string[] lines = File.ReadAllLines("input11.txt");

            width = lines[0].Length;
            height = lines.Length;

            foreach (string line in lines)
            {
                foreach (char c in line)
                {
                    int id = grid.Count;
                    if (c == '#')
                    {
                        galaxies.Add(id);
                    }
                    grid.Add(1);
                }
            }

            List<int> emptyRows = new List<int>(), emptyCols = new List<int>();

            for (int y = 0; y < height; y++)
            {
                bool found = false;
                for (int x = 0; x < width; x++)
                {
                    if (galaxies.Contains(y * width + x))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found) {
                    emptyRows.Add(y);
                }
            }

            for (int x = 0; x < width; x++)
            {
                bool found = false;
                for (int y = 0; y < height; y++)
                {
                    if (galaxies.Contains(y * width + x))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    emptyCols.Add(x);
                }
            }

            int sum = 0;
            int pairs = 0;
            foreach (int g1 in galaxies)
            {
                foreach (int g2 in galaxies)
                {
                    if (g1 >= g2)
                        continue;

                    int g1n = galaxies.IndexOf(g1) + 1;
                    int g2n = galaxies.IndexOf(g2) + 1;

                    int g1x = g1 % width, g1y = g1 / width;
                    int g2x = g2 % width, g2y = g2 / width;

                    int cost = 0;
                    for (int y = Math.Min(g1y, g2y) + 1; y <= Math.Max(g1y, g2y); y++)
                    {
                        if (emptyRows.Contains(y))
                            cost++;
                        cost++;
                    }
                    for (int x = Math.Min(g1x, g2x) + 1; x <= Math.Max(g1x, g2x); x++)
                    {
                        if (emptyCols.Contains(x))
                            cost++;
                        cost++;
                    }

                    sum += cost;
                    pairs++;

                    Console.WriteLine($"G1 {g1n} G2 {g2n} Cost {cost}");
                }
            }

            Console.WriteLine($"Total cost {sum} Total pairs {pairs}");
        }

        public void Part2()
        {
            string[] lines = File.ReadAllLines("input11.txt");

            width = lines[0].Length;
            height = lines.Length;

            foreach (string line in lines)
            {
                foreach (char c in line)
                {
                    int id = grid.Count;
                    if (c == '#')
                    {
                        galaxies.Add(id);
                    }
                    grid.Add(1);
                }
            }

            List<int> emptyRows = new List<int>(), emptyCols = new List<int>();

            for (int y = 0; y < height; y++)
            {
                bool found = false;
                for (int x = 0; x < width; x++)
                {
                    if (galaxies.Contains(y * width + x))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    emptyRows.Add(y);
                }
            }

            for (int x = 0; x < width; x++)
            {
                bool found = false;
                for (int y = 0; y < height; y++)
                {
                    if (galaxies.Contains(y * width + x))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    emptyCols.Add(x);
                }
            }

            int scale = 1000000;
            long sum = 0;
            int pairs = 0;
            foreach (int g1 in galaxies)
            {
                foreach (int g2 in galaxies)
                {
                    if (g1 >= g2)
                        continue;

                    int g1n = galaxies.IndexOf(g1) + 1;
                    int g2n = galaxies.IndexOf(g2) + 1;

                    int g1x = g1 % width, g1y = g1 / width;
                    int g2x = g2 % width, g2y = g2 / width;

                    long cost = 0;
                    for (int y = Math.Min(g1y, g2y) + 1; y <= Math.Max(g1y, g2y); y++)
                    {
                        if (emptyRows.Contains(y))
                            cost += scale - 1;
                        cost++;
                    }
                    for (int x = Math.Min(g1x, g2x) + 1; x <= Math.Max(g1x, g2x); x++)
                    {
                        if (emptyCols.Contains(x))
                            cost += scale - 1;
                        cost++;
                    }

                    sum += cost;
                    pairs++;

                    Console.WriteLine($"G1 {g1n} G2 {g2n} Cost {cost}");
                }
            }

            Console.WriteLine($"Total cost {sum} Total pairs {pairs}");
        }
    }
}
