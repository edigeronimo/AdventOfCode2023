using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace AdventOfCode2023
{
    internal class Day23 : Day
    {
        CommonGrid grid, visitedGrid;
        int maxSteps = 0;
        List<Vec2> steps = new List<Vec2>();
        int startx, starty;
        int destx, desty;
        bool ignoreSlopes = false;

        public void Part1()
        {
            DoWork();
        }

        void DoWork()
        {
            string[] lines = File.ReadAllLines("input23.txt");

            grid = new CommonGrid(lines);
            visitedGrid = new CommonGrid(grid.width, grid.height, (char)0);

            startx = 1; starty = 0;
            destx = grid.width - 2; desty = grid.height - 1;

            AddStep(new Vec2(startx, starty));

            int stackSize = 50 * 1024 * 1024;
            Thread thread = new Thread(() => TryStep(new Vec2(startx, starty + 1)), stackSize);
            thread.Start();
            thread.Join();

            Console.WriteLine($"Max steps: {maxSteps}");
        }

        void AddStep(Vec2 step)
        {
            steps.Add(step);
            visitedGrid.Set(step.x, step.y, (char)1);
        }

        void RemoveStep()
        {
            visitedGrid.Set(steps[steps.Count - 1].x, steps[steps.Count - 1].y, (char)0);
            steps.RemoveAt(steps.Count - 1);
        }

        void TryNextStep()
        {
            Vec2 pos = steps[steps.Count - 1];

            TryStep(new Vec2(pos.x - 1, pos.y));
            TryStep(new Vec2(pos.x + 1, pos.y));
            TryStep(new Vec2(pos.x, pos.y - 1));
            TryStep(new Vec2(pos.x, pos.y + 1));
        }

        void TryStep(Vec2 next)
        {
            //if (next.x < 0 || next.y < 0)
            //    return;

            if (visitedGrid.Get(next.x, next.y) != (char)0)
                return;

            char c = grid.Get(next.x, next.y);

            if (c == '#')
                return;

            if (next.x == destx && next.y == desty)
            {
                if (steps.Count > maxSteps)
                {
                    maxSteps = steps.Count;
                    Console.WriteLine($"New longest path: {maxSteps}");
                }

                return;
            }

            if (c == '.' || ignoreSlopes)
            {
                AddStep(next);
                TryNextStep();
                RemoveStep();
                return;
            }

            if (c == 'v')
            {
                AddStep(next);
                TryStep(new Vec2(next.x, next.y + 1));
                RemoveStep();
                return;
            }

            if (c == '^')
            {
                AddStep(next);
                TryStep(new Vec2(next.x, next.y - 1));
                RemoveStep();
                return;
            }

            if (c == '<')
            {
                AddStep(next);
                TryStep(new Vec2(next.x - 1, next.y));
                RemoveStep();
                return;
            }

            if (c == '>')
            {
                AddStep(next);
                TryStep(new Vec2(next.x + 1, next.y));
                RemoveStep();
                return;
            }
        }

        public void Part2()
        {
            ignoreSlopes = true;
            DoWork();
        }
    }
}
