using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day10 : Day
    {
        [Flags]
        public enum Connections
        {
            None = 0,
            Left = 1,
            Right = 2,
            Up = 4,
            Down = 8,
            Start = 16,
            Filled = 32
        }

        class Square
        {

            public Connections connections;
            public Square(char c)
            {
                switch (c)
                {
                    case '-':
                        connections = Connections.Left | Connections.Right; break;
                    case '|':
                        connections = Connections.Up | Connections.Down; break;
                    case 'L':
                        connections = Connections.Up | Connections.Right; break;
                    case 'J':
                        connections = Connections.Up | Connections.Left; break;
                    case '7':
                        connections = Connections.Down | Connections.Left; break;
                    case 'F':
                        connections = Connections.Down | Connections.Right; break;
                    case 'S':
                        connections = Connections.Start; break;
                    case '.':
                    default:
                        connections = Connections.None; break;
                }
            }

            public override string ToString()
            {
                if (connections == (Connections.Left | Connections.Right)) return "-";
                if (connections == (Connections.Up | Connections.Down)) return "|";
                if (connections == (Connections.Up | Connections.Right)) return "L";
                if (connections == (Connections.Up | Connections.Left)) return "J";
                if (connections == (Connections.Down | Connections.Left)) return "7";
                if (connections == (Connections.Down | Connections.Right)) return "F";
                if (connections == Connections.Filled) return "x";
                return ".";
            }
        }

        List<Square> grid = new List<Square>();
        List<Square> newgrid = new List<Square>();
        List<Square> doublegrid = new List<Square>();
        int width;
        int height;

        public void Part1()
        {
            string[] lines = File.ReadAllLines("input10.txt");

            width = lines[0].Length;
            height = lines.Length;

            foreach (string line in lines)
            {
                foreach (char c in line.ToCharArray())
                    grid.Add(new Square(c));
            }

            int start = grid.FindIndex(s => (s.connections & Connections.Start) != 0);

            int startx = start % width;
            int starty = start / width;

            Connections[] dirs = new Connections[] {
                Connections.Left, Connections.Right,
                Connections.Up, Connections.Down
            };
            foreach (Connections dir in dirs)
            {
                int result = TryPath(startx, starty, dir);
                int steps = result / 2;
                if (result > 0)
                {
                    Console.WriteLine($"Dir {dir} length {result} Steps {steps}");
                }
                else
                {
                    Console.WriteLine($"Dir {dir} not valid path");
                }
            }
        }

        int TryPath(int startx, int starty, Connections dir)
        {
            int x = startx;
            int y = starty;
            int prevx = startx;
            int prevy = starty;

            // This should be opposite the path we're about to walk, so we know not to backtrack
            Connections prevDir = Connections.None;

            if (dir == Connections.Left)
            {
                x--;
                prevDir = Connections.Right;
            }
            else if (dir == Connections.Right)
            {
                x++;
                prevDir = Connections.Left;
            }
            else if (dir == Connections.Up)
            {
                y--;
                prevDir = Connections.Down;
            }
            else if (dir == Connections.Down)
            {
                y++;
                prevDir = Connections.Up;
            }

            if ((grid[y * width + x].connections & prevDir) == 0)
                return -1; // doesn't connect back to start

            int count = 1;

            while (true)
            {
                if (x < 0 || x >= width || y < 0 || y >= height)
                    return -1;

                int pos = y * width + x;
                
                if ((grid[pos].connections & Connections.Left) != 0 && prevDir != Connections.Left)
                {
                    x--;
                    prevDir = Connections.Right;
                }
                else if ((grid[pos].connections & Connections.Right) != 0 && prevDir != Connections.Right)
                {
                    x++;
                    prevDir = Connections.Left;
                }
                else if ((grid[pos].connections & Connections.Up) != 0 && prevDir != Connections.Up)
                {
                    y--;
                    prevDir = Connections.Down;
                }
                else if ((grid[pos].connections & Connections.Down) != 0 && prevDir != Connections.Down)
                {
                    y++;
                    prevDir = Connections.Up;
                }
                else
                {
                    // invalid path
                    return -1;
                }

                count++;

                if (x == startx && y == starty)
                    return count;

                if (count > grid.Count)
                    return -1; // we've walked more squares than there are in the grid, so this can't be a valid loop
            }
        }

        public void Part2()
        {
            //string[] lines = File.ReadAllLines("testinput10.txt");

            string[] lines = File.ReadAllLines("input10.txt");

            width = lines[0].Length;
            height = lines.Length;

            foreach (string line in lines)
            {
                foreach (char c in line.ToCharArray())
                    grid.Add(new Square(c));
            }

            int start = grid.FindIndex(s => (s.connections & Connections.Start) != 0);

            int startx = start % width;
            int starty = start / width;

            Connections[] dirs = new Connections[] {
                Connections.Left, Connections.Right,
                Connections.Up, Connections.Down
            };
            foreach (Connections dir in dirs)
            {
                int result = TryPath(startx, starty, dir);
                int steps = result / 2;
                if (result > 0)
                {
                    Console.WriteLine($"Dir {dir} length {result} Steps {steps}");
                    MakeNewPath(startx, starty, dir);
                    //for (int y = 0; y < height; y++)
                    //{
                    //    for (int x = 0; x < width; x++)
                    //    {
                    //        Console.Write(newgrid[y * width + x]);
                    //    }
                    //    Console.WriteLine();
                    //}
                    DoublePath();
                    for (int y = 0; y < height * 2; y++)
                    {
                        for (int x = 0; x < width * 2; x++)
                        {
                            Console.Write(doublegrid[y * width * 2 + x]);
                        }
                        Console.WriteLine();
                    }
                    DoFloodFill();
                    Console.WriteLine();
                    for (int y = 0; y < height * 2; y++)
                    {
                        for (int x = 0; x < width * 2; x++)
                        {
                            Console.Write(doublegrid[y * width * 2 + x]);
                        }
                        Console.WriteLine();
                    }
                    int nests = CountEmpty();
                    Console.WriteLine($"Nests: {nests}");
                    break;
                }
            }
        }

        void MakeNewPath(int startx, int starty, Connections dir)
        {
            int x = startx;
            int y = starty;
            int prevx = startx;
            int prevy = starty;

            // This should be opposite the path we're about to walk, so we know not to backtrack
            Connections prevDir = Connections.None;

            if (dir == Connections.Left)
            {
                x--;
                prevDir = Connections.Right;
            }
            else if (dir == Connections.Right)
            {
                x++;
                prevDir = Connections.Left;
            }
            else if (dir == Connections.Up)
            {
                y--;
                prevDir = Connections.Down;
            }
            else if (dir == Connections.Down)
            {
                y++;
                prevDir = Connections.Up;
            }

            for (int i = 0; i < grid.Count; i++)
                newgrid.Add(new Square('.'));

            newgrid[starty * width + startx].connections = dir;

            while (true)
            {
                int pos = y * width + x;

                if ((grid[pos].connections & Connections.Left) != 0 && prevDir != Connections.Left)
                {
                    x--;
                    prevDir = Connections.Right;
                }
                else if ((grid[pos].connections & Connections.Right) != 0 && prevDir != Connections.Right)
                {
                    x++;
                    prevDir = Connections.Left;
                }
                else if ((grid[pos].connections & Connections.Up) != 0 && prevDir != Connections.Up)
                {
                    y--;
                    prevDir = Connections.Down;
                }
                else if ((grid[pos].connections & Connections.Down) != 0 && prevDir != Connections.Down)
                {
                    y++;
                    prevDir = Connections.Up;
                }
                else
                {
                    // invalid path
                    break;
                    // shouldn't happen
                }

                newgrid[pos] = grid[pos];

                if (x == startx && y == starty)
                {
                    newgrid[starty * width + startx].connections |= prevDir;
                    return;
                }
            }
        }

        void DoublePath()
        {
            for (int i = 0; i < grid.Count; i++)
            {
                for (int j = 0; j < 4; j++)
                    doublegrid.Add(new Square('.'));
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Square s = newgrid[y * width + x];
                    doublegrid[y * 2 * width * 2 + x * 2] = s;
                    if ((s.connections & Connections.Left) != 0)
                    {
                        doublegrid[y * 2 * width * 2 + x * 2 - 1].connections |= Connections.Left;
                    }
                    if ((s.connections & Connections.Right) != 0)
                    {
                        doublegrid[y * 2 * width * 2 + x * 2 + 1].connections |= Connections.Right;
                    }
                    if ((s.connections & Connections.Up) != 0)
                    {
                        doublegrid[(y * 2 - 1) * width * 2 + x * 2].connections |= Connections.Up;
                    }
                    if ((s.connections & Connections.Down) != 0)
                    {
                        doublegrid[(y * 2 + 1) * width * 2 + x * 2].connections |= Connections.Down;
                    }
                }
            }
        }

        void DoFloodFill()
        {
            // This will overflow the default stack size. Easiest way to increase the stack is to run on a new thread.
            // Also easier than coding this to not need a giant stack.
            int stackSize = 50 * 1024 * 1024;
            Thread thread = new Thread(() => DoFill(0, 0), stackSize);
            thread.Start();
            thread.Join();
        }

        void DoFill(int x, int y)
        {
            if (x < 0 || y < 0) return;
            if (x >= width * 2 || y >= height * 2) return;
            if (doublegrid[y * width * 2 + x].connections != Connections.None) return;

            doublegrid[y * width * 2 + x].connections |= Connections.Filled;

            DoFill(x - 1, y);
            DoFill(x + 1, y);
            DoFill(x, y + 1);
            DoFill(x, y - 1);
        }

        int CountEmpty()
        {
            int result = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (doublegrid[y * 2 * width * 2 + x * 2].connections == Connections.None)
                    {
                        result++;
                        //Console.WriteLine($"Nest {x},{y}");
                    }
                }
            }

            return result;
        }
    }
}
