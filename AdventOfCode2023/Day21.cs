using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static AdventOfCode2023.Day10;

namespace AdventOfCode2023
{
    internal class Day21 : Day
    {
        CommonGrid grid;
        int[] ends;
        List<int> visited = new List<int>();


        public void Part1()
        {
            string[] lines = File.ReadAllLines("input21.txt");

            grid = new CommonGrid(lines);
            ends = new int[grid.width * grid.height];

            int stackSize = 50 * 1024 * 1024;
            Thread thread = new Thread(() => DoPart1(), stackSize);
            thread.Start();
            thread.Join();

            int count = 0;
            for (int y = 0; y < grid.height; y++)
            {
                for (int x = 0; x < grid.width; x++)
                {
                    //if (grid.Get(x, y) == 'O')
                    if (ends[y * grid.width + x] != 0)
                    {
                        count++;
                    }
                }
            }

            Console.Write($"Count = {count}");
            while (true) ;
        }

        void DoPart1()
        {
            for (int y = 0; y < grid.height; y++)
            {
                for (int x = 0; x < grid.width; x++)
                {
                    if (grid.Get(x, y) == 'S')
                    {
                        DoWalk(x, y, 0, 64);
                        return;
                    }
                }
            }
        }

        void DoWalk(int x, int y, int depth, int maxdepth)
        {
            if (depth > maxdepth)
                return;

            int hash = ((x * 1000) + y) * 1000 + depth;
            if (visited.Contains(hash))
                return;
            visited.Add(hash);

            if (x < 0 || y < 0) return;
            if (x >= grid.width || y >= grid.height) return;

            if (grid.Get(x, y) == '.' || grid.Get(x, y) == 'S')
            {
                if (depth == maxdepth)
                    ends[y * grid.width + x] = 1;
                DoWalk(x - 1, y, depth + 1, maxdepth);
                DoWalk(x + 1, y, depth + 1, maxdepth);
                DoWalk(x, y - 1, depth + 1, maxdepth);
                DoWalk(x, y + 1, depth + 1, maxdepth);
            }
        }

        // 63, 261, 644, 1196
        // 11 22 33 44
        // ax^2 + bx + c
        // a(11)^2 + b(11) + c = 63
        // a(22)^2 + b(22) + c = 261
        // a(33)^2 + b(33) + c = 644
        // 121a  + 11b + c = 63
        // 484a  + 22b + c = 261
        // 1089a + 33b + c = 644
        //
        // 363a + 11b = 198
        // 


        public void Part2()
        {
            string[] lines = File.ReadAllLines("testinput21.txt");

            grid = new CommonGrid(lines);
            ends = new int[grid.width * grid.height];

            int stackSize = 50 * 1024 * 1024;
            Thread thread = new Thread(() => DoPart2(11), stackSize);
            thread.Start();
            thread.Join();

            //int count = 0;
            //for (int y = 0; y < grid.height; y++)
            //{
            //    for (int x = 0; x < grid.width; x++)
            //    {
            //        if (ends[y * grid.width + x] != 0)
            //        {
            //            count++;
            //        }
            //    }
            //}
            int count = endSteps.Count;

            Console.Write($"Count = {count}");
            while (true) ;
        }

        struct Step : IEquatable<Step>
        {
            public int x, y, depth;

            public Step(int x, int y, int depth)
            {
                this.x = x;
                this.y = y;
                this.depth = depth;
            }

            public bool Equals(Step s)
            {
                return this == s;
            }

            public static bool operator ==(Step s1, Step s2)
            {
                return (s1.x == s2.x) && (s1.y == s2.y) && (s1.depth == s2.depth);
            }

            public static bool operator !=(Step s1, Step s2)
            {
                return (s1.x != s2.x) || (s1.y != s2.y) || (s1.depth != s2.depth);
            }
        }

        async void DoPart2(int count)
        {
            for (int y = 0; y < grid.height; y++)
            {
                for (int x = 0; x < grid.width; x++)
                {
                    if (grid.Get(x, y) == 'S')
                    {
                        grid.Set(x, y, '.');

                        DoWalkInfinite(x, y, 0, count);
                        Point p1 = new Point(count, endSteps.Count);

                        visitedSteps.Clear();
                        endSteps.Clear();
                        DoWalkInfinite(x, y, 0, count * 2);
                        Point p2 = new Point(count * 2, endSteps.Count);

                        visitedSteps.Clear();
                        endSteps.Clear();
                        DoWalkInfinite(x, y, 0, count * 3);
                        Point p3 = new Point(count * 3, endSteps.Count);

                        double a, b, c;
                        FindQuadraticEquation(p1, p2, p3, out a, out b, out c);

                        int steps1, steps2, steps3;
                        steps1 = (int)(a * 50 * 59 + b * 50 + c);
                        steps2 = (int)(a * 100 * 100 + b * 100 + c);
                        steps3 = (int)(a * 500 * 500 + b * 500 + c);

                        return;
                    }
                }
            }
        }

        List<Step> visitedSteps = new List<Step>(2000000000);
        List<Step> endSteps = new List<Step>(2000000000);

        void DoWalkInfinite(int rawx, int rawy, int depth, int maxdepth)
        {
            if (depth > maxdepth)
                return;

            Step step = new Step(rawx, rawy, depth);
            if (visitedSteps.Contains(step))
                return;
            visitedSteps.Add(step);

            int x, y;
            if (rawx >= 0)
                x = rawx % grid.width;
            else
                x = (grid.width - (-rawx % grid.width)) % grid.width;

            if (rawy >= 0)
                y = rawy % grid.height;
            else
                y = (grid.height - (-rawy % grid.height)) % grid.height;

            //int hash = ((x * 1000) + y) * 1000 + depth;
            //if (visited.Contains(hash))
            //    return;
            //visited.Add(hash);

            //if (x < 0 || y < 0) return;
            //if (x >= grid.width || y >= grid.height) return;

            if (grid.Get(x, y) == '.') // || grid.Get(x, y) == 'S')
            {
                if (depth == maxdepth)
                {
                    Step end = new Step(rawx, rawy, depth);
                    if (!endSteps.Contains(step))
                        endSteps.Add(step);
                }
                DoWalkInfinite(rawx - 1, rawy, depth + 1, maxdepth);
                DoWalkInfinite(rawx + 1, rawy, depth + 1, maxdepth);
                DoWalkInfinite(rawx, rawy - 1, depth + 1, maxdepth);
                DoWalkInfinite(rawx, rawy + 1, depth + 1, maxdepth);
            }
        }

        class Point
        {
            public double X, Y;

            public Point(double x, double y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        static void FindQuadraticEquation(Point p1, Point p2, Point p3, out double a, out double b, out double c)
        {
            double x1 = p1.X, y1 = p1.Y;
            double x2 = p2.X, y2 = p2.Y;
            double x3 = p3.X, y3 = p3.Y;

            double denom = (x1 - x2) * (x1 - x3) * (x2 - x3);
            a = (x3 * (y2 - y1) + x2 * (y1 - y3) + x1 * (y3 - y2)) / denom;
            b = (x3 * x3 * (y1 - y2) + x2 * x2 * (y3 - y1) + x1 * x1 * (y2 - y3)) / denom;
            c = (x2 * x3 * (x2 - x3) * y1 + x3 * x1 * (x3 - x1) * y2 + x1 * x2 * (x1 - x2) * y3) / denom;
        }
    }
}
