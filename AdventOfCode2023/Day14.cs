using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day14 : Day
    {
        List<char> grid = new List<char>();
        int width, height;

        public void Part1()
        {
            string[] lines = File.ReadAllLines("input14.txt");

            width = lines[0].Length;
            height = lines.Count();

            foreach(string line in lines)
            {
                foreach (char c in line)
                {
                    grid.Add(c);
                }
            }

            bool changed = false;
            do
            {
                changed = false;
                for (int y = 1; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (grid[y * width + x] == 'O' && grid[(y - 1) * width + x] == '.')
                        {
                            grid[(y - 1) * width + x] = 'O';
                            grid[y * width + x] = '.';
                            changed = true;
                        }
                    }
                }
            } while (changed);

            int load = CalcLoad();

            Console.WriteLine(load);
        }

        int CalcLoad()
        {
            int load = 0;
            for (int i = 0; i < grid.Count; i++)
            {
                if (grid[i] == 'O')
                {
                    load += height - (i / width);
                }
            }
            return load;
        }

        void PrintGrid()
        {
            string output = "";
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    output += grid[y * width + x];
                }
                output += "\n";
            }
            Console.WriteLine(output);
        }

        public void Part2()
        {
            string[] lines = File.ReadAllLines("input14.txt");

            width = lines[0].Length;
            height = lines.Count();

            foreach (string line in lines)
            {
                foreach (char c in line)
                {
                    grid.Add(c);
                }
            }

            List<int> results = new List<int>();

            int totalreps = 1000000000;
            int reps = 200;
            for (int i = 0; i < reps; i++)
            {
                RotateNorth();
                RotateWest();
                RotateSouth();
                RotateEast();

                int load = CalcLoad();
                Console.WriteLine("Rep: " + i + " " + load);

                if (results.Contains(load))
                {
                    //if (results.IndexOf(results.Last()))
                    int first = results.IndexOf(load);
                    if (first == 80)
                        Console.WriteLine($"Found {load} at {first} and " + results.Count());
                }
                results.Add(load);

                if (i == 1000000000 / 100)
                {
                    Console.Write(".");
                }
            }

            bool foundPattern = false;
            int patternStart = 0;
            int pfirst = 0, prepeat = 0;
            for (int i = 0; i < 200 - 20 && !foundPattern; i++)
            {
                for (int j = i + 10; j < 200 - 10 && !foundPattern; j++)
                {
                    if (results[i] == results[j])
                    {
                        bool match = true;
                        for (int k = 0; k < 10; k++)
                        {
                            if (results[i + k] != results[j + k])
                            {
                                match = false;
                                break;
                            }
                        }
                        foundPattern = match;
                        if (foundPattern)
                        {
                            pfirst = i;
                            prepeat = j;
                        }
                    }
                }
            }

            Console.WriteLine($"Pattern first {pfirst} repeat {prepeat}");

            int offset = totalreps - pfirst;
            //int cycles = offset / prepeat;
            int index = pfirst + (offset % (prepeat - pfirst)) - 1;

            //Console.WriteLine(CalcLoad());
            Console.WriteLine("Load " + results[index]);
        }

        void RotateNorth()
        {
            bool changed = false;
            do
            {
                changed = false;
                for (int y = 1; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (grid[y * width + x] == 'O' && grid[(y - 1) * width + x] == '.')
                        {
                            grid[(y - 1) * width + x] = 'O';
                            grid[y * width + x] = '.';
                            changed = true;
                        }
                    }
                }
            } while (changed);
        }

        void RotateSouth()
        {
            bool changed = false;
            do
            {
                changed = false;
                for (int y = height - 2; y >= 0; y--)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (grid[(y + 1) * width + x] == '.' && grid[y * width + x] == 'O')
                        {
                            grid[(y + 1) * width + x] = 'O';
                            grid[y * width + x] = '.';
                            changed = true;
                        }
                    }
                }
            } while (changed);
        }

        void RotateEast()
        {
            bool changed = false;
            do
            {
                changed = false;
                for (int x = width - 2; x >= 0; x--)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (grid[y * width + x] == 'O' && grid[y * width + x + 1] == '.')
                        {
                            grid[y * width + x + 1] = 'O';
                            grid[y * width + x] = '.';
                            changed = true;
                        }
                    }
                }
            } while (changed);
        }

        void RotateWest()
        {
            bool changed = false;
            do
            {
                changed = false;
                for (int x = 1; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (grid[y * width + x - 1] == '.' && grid[y * width + x] == 'O')
                        {
                            grid[y * width + x - 1] = 'O';
                            grid[y * width + x] = '.';
                            changed = true;
                        }

                    }
                }
            } while (changed);
        }
    }
}
