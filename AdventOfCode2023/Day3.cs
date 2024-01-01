using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day3 : Day
    {
        class Grid
        {
            public int width;
            public int height;
            public char[] data;
            public Dictionary<int, List<int>> gearValues = new Dictionary<int, List<int>>();

            public Grid(string[] lines)
            {
                height = lines.Length;
                width = lines[0].Length;
                data = new char[width * height];

                int count = 0;
                foreach (var line in lines)
                {
                    for (int i = 0; i < line.Length; i++)
                    {
                        data[count++] = line[i];
                    }
                }
            }

            public bool IsSymbol(int x, int y)
            {
                if (x < 0 || y < 0) return false;
                if (x >= width || y >= height) return false;

                char c = data[y * width + x];

                if (c == '.') return false;
                if (c >= '0' && c <= '9') return false;

                return true;
            }

            public bool IsGear(int x, int y)
            {
                if (x < 0 || y < 0) return false;
                if (x >= width || y >= height) return false;

                char c = data[y * width + x];

                if (c == '*') return true;

                return false;
            }

            public bool IsDigit(int x, int y)
            {
                char c = data[y * width + x];
                return IsDigit(c);
            }

            public bool IsDigit(char c)
            {
                if (c >= '0' && c <= '9') return true;
                return false;
            }

            public bool IsTouchingSymbol(int x, int y)
            {
                for (int yy = y - 1; yy <= y + 1; yy++)
                {
                    for (int xx = x - 1; xx <= x + 1; xx++)
                    {
                        if (IsSymbol(xx, yy)) return true;
                    }
                }

                return false;
            }

            public bool IsTouchingGear(int x, int y)
            {
                for (int yy = y - 1; yy <= y + 1; yy++)
                {
                    for (int xx = x - 1; xx <= x + 1; xx++)
                    {
                        if (IsGear(xx, yy)) return true;
                    }
                }

                return false;
            }

            public void AddNumberToGear(int value, int x, int y)
            {
                int gear = y * width + x;
                AddNumberToGear(value, gear);
            }

            public void AddNumberToGear(int value, int gear)
            {
                if (!gearValues.ContainsKey(gear))
                {
                    gearValues[gear] = new List<int>();
                }
                gearValues[gear].Add(value);
            }

            public int GetDigit(int x, int y)
            {
                return data[y * width + x] - '0';
            }
        }

        public void Part1()
        {
            string[] lines = File.ReadAllLines("input3.txt");

            Grid grid = new Grid(lines);
            int sum = 0;

            for (int y = 0; y < grid.height; y++)
            {
                for (int x = 0; x < grid.width; x++)
                {
                    if (grid.IsDigit(x, y))
                    {
                        int number = 0;
                        bool touching = false;

                        while (grid.IsDigit(x, y) && x < grid.width)
                        {
                            number *= 10;
                            number += grid.GetDigit(x, y);
                            touching |= grid.IsTouchingSymbol(x, y);
                            x++;
                        }

                        Console.WriteLine($"{number} : {touching}");
                        if (touching)
                            sum += number;
                    }
                }
            }

            Console.WriteLine("Sum: " + sum);
        }
        public void Part2()
        {
            string[] lines = File.ReadAllLines("input3.txt");

            Grid grid = new Grid(lines);
            int sum = 0;

            for (int y = 0; y < grid.height; y++)
            {
                for (int x = 0; x < grid.width; x++)
                {
                    if (grid.IsDigit(x, y))
                    {
                        int number = 0;
                        bool touching = false;

                        List<int> gears = new List<int>();
                        while (grid.IsDigit(x, y) && x < grid.width)
                        {
                            number *= 10;
                            number += grid.GetDigit(x, y);
                            touching |= grid.IsTouchingSymbol(x, y);

                            for (int gy = y - 1; gy <= y + 1; gy++)
                            {
                                for (int gx = x - 1; gx <= x + 1; gx++)
                                {
                                    if (grid.IsGear(gx, gy))
                                    {
                                        if (!gears.Contains(gy * grid.width + gx))
                                            gears.Add(gy * grid.width + gx);
                                    }
                                }
                            }
                            x++;
                        }
                        foreach (int gear in gears)
                        {
                            grid.AddNumberToGear(number, gear);
                        }

                        //Console.WriteLine($"{number} : {touching}");
                        //if (touching)
                        //    sum += number;
                    }
                }
            }

            foreach (int gear in grid.gearValues.Keys)
            {
                if (grid.gearValues[gear].Count == 2)
                {
                    int a = grid.gearValues[gear][0];
                    int b = grid.gearValues[gear][1];
                    int product = a * b;
                    sum += product;
                    Console.WriteLine($"Gear {a} * {b} = {product}");
                }
            }

            Console.WriteLine("Sum: " + sum);
        }

    }
}
