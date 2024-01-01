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

        int Check(List<int> g, int x, int y)
        {
            if (x < 0 || y < 0 || x >= width || y >= height)
                return 1000000000; // way too high
            return g[y * width + x];
        }
        void Set(List<int> g, int x, int y, int value)
        {
            if (x < 0 || y < 0 || x >= width || y >= height)
                return;
            g[y * width + x] = value;
        }

        public void Part1()
        {
            string[] lines = File.ReadAllLines("testinput11.txt");

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
                    for (int x = 0; x < width; x++)
                    {
                        grid[y * width + x] *= 2;
                    }
                }
            }

            for (int x = 0; x < width; x++)
            {
                bool found = false;
                for (int y = 0; x < height; y++)
                {
                    if (galaxies.Contains(y * width + x))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    for (int y = 0; x < height; y++)
                    {
                        grid[y * width + x] *= 2;
                    }
                }
            }

            /*
            foreach (int g1 in galaxies)
            {
                foreach (int g2 in galaxies)
                {
                    if (g1 == g2)
                        continue;

                    int cost = Search(g2 % width, g2 / width, g1 % width, g1 % height);
                    Console.WriteLine($"G1 {g1} G2 {g2} Cost {cost}");
                }
            }
            */

            foreach (int g1 in galaxies)
            {
                foreach (int g2 in galaxies)
                {
                    if (g1 == g2)
                        continue;

                    int g1x = g1 % width, g1y = g1 / width;
                    int g2x = g2 % width, g2y = g2 / width;

                    int cost = Search(g2 % width, g2 / width, g1 % width, g1 % height);
                    Console.WriteLine($"G1 {g1} G2 {g2} Cost {cost}");
                }
            }

        }

        /*
        int Search(int destx, int desty, int startx, int starty)
        {
            List<int> unsolved = new List<int>();
            for (int i = 0; i < grid.Count; i++)
            {
                unsolved.Add(-1);
            }

            int[] neightbors = new int[4];

            Set(unsolved, destx, desty, 0);
            Set(unsolved, destx - 1, desty, Check(grid, destx - 1, desty));
            Set(unsolved, destx + 1, desty, Check(grid, destx + 1, desty));
            Set(unsolved, destx, desty - 1, Check(grid, destx, desty - 1));
            Set(unsolved, destx, desty + 1, Check(grid, destx, desty + 1));


            while (unsolved.Count(x => x < 0) > 0)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (Check(unsolved, x, y) >= 0)
                            continue;

                        neightbors[0] = Check(unsolved, x - 1, y);
                        neightbors[1] = Check(unsolved, x + 1, y);
                        neightbors[2] = Check(unsolved, x, y - 1);
                        neightbors[3] = Check(unsolved, x, y + 1);

                        if (neightbors.Count(x => x < 0) == 0)
                        {
                            Set(unsolved, x, y, neightbors.Min());
                        }
                    }
                }
            }

            return Check(unsolved, startx, starty);
        }
        */

        public void Part2()
        {
            string[] lines = File.ReadAllLines("testinput11.txt");

        }
    }
}
