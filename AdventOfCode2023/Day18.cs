using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static AdventOfCode2023.Day10;

namespace AdventOfCode2023
{
    internal class Day18 : Day
    {
        class Cell
        {
            public int depth;
            public string color;

            public Cell(int depth = 0, string color = "")
            {
                this.depth = depth;
                this.color = color;
            }
        }

        class Grid
        {
            public int width, height;
            int x, y;

            public Cell[] data;

            public Grid()
            {
                width = height = 1;
                x = y = 0;
                data = new Cell[1];
                data[0] = new Cell(1);
            }

            public void MoveRight(string color)
            {
                if (x + 1 >= width)
                {
                    Cell[] newData = new Cell[height * (width + 1)];
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            newData[y * (width + 1) + x] = data[y * width + x];
                        }
                        newData[y * (width + 1) + width] = new Cell();
                    }
                    data = newData;
                    width++;
                }

                x++;
                data[y * width + x] = new Cell(1, color);
            }

            public void MoveLeft(string color)
            {
                if (x == 0)
                {
                    Cell[] newData = new Cell[height * (width + 1)];
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            newData[y * (width + 1) + x + 1] = data[y * width + x];
                        }
                        newData[y * (width + 1) + 0] = new Cell();
                    }
                    data = newData;
                    width++;
                    x++;
                }

                x--;
                data[y * width + x] = new Cell(1, color);
            }

            public void MoveDown(string color)
            {
                if (y + 1 >= height)
                {
                    Cell[] newData = new Cell[(height + 1) * width];
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            newData[y * width + x] = data[y * width + x];
                        }
                    }

                    for (int x = 0; x < width; x++)
                        newData[height * width + x] = new Cell();

                    height++;
                    data = newData;
                }

                y++;
                data[y * width + x] = new Cell(1, color);
            }

            public void MoveUp(string color)
            {
                if (y == 0)
                {
                    Cell[] newData = new Cell[(height + 1) * width];
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            newData[(y + 1) * width + x] = data[y * width + x];
                        }
                    }

                    for (int x = 0; x < width; x++)
                        newData[x] = new Cell();

                    height++;
                    data = newData;
                    y++;
                }

                y--;
                data[y * width + x] = new Cell(1, color);
            }

            public void DoFloodFill()
            {
                // This will overflow the default stack size. Easiest way to increase the stack is to run on a new thread.
                // Also easier than coding this to not need a giant stack.

                int startx = 1;
                bool hitWall = false;
                for (int x = 1; x < width - 1; x++)
                {
                    if (data[width + x].depth != 0)
                    {
                        if (hitWall)
                            continue;

                        hitWall = true;
                    }
                    else if (hitWall)
                    {
                        startx = x;
                        break;
                    }

                }
                int stackSize = 50 * 1024 * 1024;
                Thread thread = new Thread(() => DoFill(startx, 1), stackSize);
                thread.Start();
                thread.Join();
            }

            void DoFill(int x, int y)
            {
                if (x < 0 || y < 0) return;
                if (x >= width || y >= height) return;
                if (data[y * width + x].depth != 0) return;

                data[y * width + x].depth = 1;

                DoFill(x - 1, y);
                DoFill(x + 1, y);
                DoFill(x, y + 1);
                DoFill(x, y - 1);
            }

            public int Filled()
            {
                return data.Sum(x => (x.depth != 0) ? 1 : 0);
            }

            public void PrintGrid()
            {
                return;
                for (int y = 1; y < height; y++)
                {
                    string line = "";
                    for (int x = 0; x < width; x++)
                    {
                        line += (data[y * width + x].depth != 0) ? "#" : ".";
                    }
                    Console.WriteLine(line);
                    break;
                }
            }
        }

        Grid grid = new Grid();

        public void Part1()
        {
            string[] lines = File.ReadAllLines("input18.txt");

            foreach (string line in lines)
            {
                string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                string dir = parts[0];
                int reps = int.Parse(parts[1]);
                string color = parts[2];

                for (int i = 0; i < reps; i++)
                {
                    if (dir == "L")
                        grid.MoveLeft(color);
                    if (dir == "R")
                        grid.MoveRight(color);
                    if (dir == "U")
                        grid.MoveUp(color);
                    if (dir == "D")
                        grid.MoveDown(color);
                }
            }

            grid.PrintGrid();
            grid.DoFloodFill();

            Console.WriteLine("Area = " + grid.Filled());
        }

        public void Part2()
        {
            string[] lines = File.ReadAllLines("input18.txt");

            List<long> xPos = new List<long>(), yPos = new List<long>();

            long x = 0, y = 0, border = 0;
            foreach (string line in lines)
            {
                string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                //string dir = parts[0];
                string dirstr = parts[2];
                char dir = dirstr[dirstr.Length - 2];
                long reps;
                long.TryParse(parts[2].Substring(2, parts[2].Length -4), NumberStyles.HexNumber, CultureInfo.CurrentCulture, out reps);
                string color = parts[1];

                if (dir == '2')
                    x -= reps;
                if (dir == '0')
                    x += reps;
                if (dir == '3')
                    y -= reps;
                if (dir == '1')
                    y += reps;

                xPos.Add(x);
                yPos.Add(y);
                border += reps;
            }

            // shoelace
            long area = 0;
            long determinent;
            for (int i = 0; i < xPos.Count - 1; i++)
            {
                determinent = xPos[i] * yPos[i + 1] - yPos[i] * xPos[i + 1];
                area += determinent;
            }
            determinent = xPos[xPos.Count - 1] * yPos[0] - yPos[yPos.Count - 1] * xPos[0];
            area += determinent;
            area /= 2;

            // pick's
            // A = i + b/2 - 1
            // i = A - b/2 + 1
            long interior = area - border / 2 + 1;

            Console.WriteLine("Cells: " + (interior + border));
        }
    }
}
