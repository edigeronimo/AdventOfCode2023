using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day16 : Day
    {
        [Flags]
        enum Direction
        {
            None = 0,
            Up = 1,
            Down = 2,
            Left = 4,
            Right = 8
        }

        class Beam
        {
            public int x, y;
            public int vx, vy;
            public Direction Direction {
                get {
                    if (vx < 0) return Direction.Left;
                    if (vx > 0) return Direction.Right;
                    if (vy < 0) return Direction.Up;
                    return Direction.Down;
                }
            }

            public Beam()
            {

            }
            public Beam(int _x, int _y, int _vx, int _vy)
            {
                x = _x;
                y = _y;
                vx = _vx;
                vy = _vy;
            }
        }

        CommonGrid grid;

        public void Part1()
        {
            string[] lines = File.ReadAllLines("input16.txt");

            grid = new CommonGrid(lines);

            int total = CountActivated(new Beam(-1, 0, 1, 0));

            Console.WriteLine(total);
        }

        int CountActivated(Beam start)
        {
            Direction[] activated = new Direction[grid.width * grid.height];
            for (int i = 0; i < activated.Length; i++)
            {
                activated[i] = Direction.None;
            }

            List<Beam> beams = new List<Beam>();
            beams.Add(start);

            while (beams.Count > 0)
            {
                Beam beam = beams[0];

                while (true)
                {
                    int newx = beam.x + beam.vx;
                    int newy = beam.y + beam.vy;

                    if ((newx < 0) || (newx >= grid.width) ||
                            (newy < 0) || (newy >= grid.height))
                    {
                        beams.RemoveAt(0);
                        break;
                    }

                    if ((activated[newy * grid.width + newx] & beam.Direction) != 0)
                    {
                        beams.RemoveAt(0);
                        break;
                    }

                    activated[newy * grid.width + newx] |= beam.Direction;

                    char next = grid.Get(newx, newy);
                    if (next == '.')
                    {
                    }
                    else if (next == '/')
                    {
                        if (beam.vx == 1)
                        {
                            beam.vx = 0;
                            beam.vy = -1;
                        }
                        else if (beam.vx == -1)
                        {
                            beam.vx = 0;
                            beam.vy = 1;
                        }
                        else if (beam.vy == 1)
                        {
                            beam.vx = -1;
                            beam.vy = 0;
                        }
                        else if (beam.vy == -1)
                        {
                            beam.vx = 1;
                            beam.vy = 0;
                        }
                    }
                    else if (next == '\\')
                    {
                        if (beam.vx == 1)
                        {
                            beam.vx = 0;
                            beam.vy = 1;
                        }
                        else if (beam.vx == -1)
                        {
                            beam.vx = 0;
                            beam.vy = -1;
                        }
                        else if (beam.vy == 1)
                        {
                            beam.vx = 1;
                            beam.vy = 0;
                        }
                        else if (beam.vy == -1)
                        {
                            beam.vx = -1;
                            beam.vy = 0;
                        }
                    }
                    else if (next == '-')
                    {
                        if (beam.vx != 0)
                        {
                            // nothing
                        }
                        else
                        {
                            Beam beam1 = new Beam(newx, newy, -1, 0);
                            Beam beam2 = new Beam(newx, newy, 1, 0);

                            beams.RemoveAt(0);
                            beams.Add(beam1);
                            beams.Add(beam2);
                            break;
                        }
                    }
                    else if (next == '|')
                    {
                        if (beam.vy != 0)
                        {
                            // nothing
                        }
                        else
                        {
                            Beam beam1 = new Beam(newx, newy, 0, -1);
                            Beam beam2 = new Beam(newx, newy, 0, 1);

                            beams.RemoveAt(0);
                            beams.Add(beam1);
                            beams.Add(beam2);
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error!");
                    }
                    beam.x = newx;
                    beam.y = newy;
                }
            }

            return activated.Sum(x => (x > 0) ? 1 : 0);
        }

        public void Part2()
        {
            string[] lines = File.ReadAllLines("input16.txt");

            grid = new CommonGrid(lines);

            int best = 0;

            for (int x = 0; x < grid.width; x++)
            {
                int score;

                score = CountActivated(new Beam(x, -1, 0, 1));
                if (score > best)
                    best = score;

                score = CountActivated(new Beam(x, grid.height, 0, -1));
                if (score > best)
                    best = score;
            }

            for (int y = 0; y < grid.height; y++)
            {
                int score;

                score = CountActivated(new Beam(-1, y, 1, 0));
                if (score > best)
                    best = score;

                score = CountActivated(new Beam(grid.width, y, -1, 01));
                if (score > best)
                    best = score;
            }

            Console.WriteLine(best);
        }
    }
}
