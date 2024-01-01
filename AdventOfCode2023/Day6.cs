using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day6 : Day
    {
        public void Part1()
        {
            string[] lines = File.ReadAllLines("input6.txt");

            string[] timesStr = lines[0].Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            string[] distStr = lines[1].Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            int[] times = new int[timesStr.Length];
            for (int i = 0; i < times.Length; i++)
            {
                times[i] = int.Parse(timesStr[i]);
            }
            int[] dist = new int[distStr.Length];
            for (int i = 0; i < dist.Length; i++)
            {
                dist[i] = int.Parse(distStr[i]);
            }

            int product = 1;
            for (int i = 0; i < times.Length; i++)
            {
                int wins = 0;
                for (int j = 0; j < times[i]; j++)
                {
                    bool canWin = CheckValue(times[i], dist[i], j);
                    if (canWin)
                    {
                        wins++;
                    }
                }
                product *= wins;
            }

            Console.WriteLine($"Wins {product}");
        }

        bool CheckValue(int time, int distance, int pressTime)
        {
            int moved = pressTime * (time - pressTime);
            return (moved > distance);
        }

        bool CheckValue(long time, long distance, long pressTime)
        {
            long moved = pressTime * (time - pressTime);
            return (moved > distance);
        }

        public void Part2()
        {
            string[] lines = File.ReadAllLines("input6.txt");

            string[] timesStr = lines[0].Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            string[] distStr = lines[1].Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            long[] times = new long[1];
            string _timeStr = "";
            for (int i = 0; i < timesStr.Length; i++)
            {
                _timeStr += timesStr[i];
            }
            long[] dist = new long[1];
            string _distStr = "";
            for (int i = 0; i < distStr.Length; i++)
            {
                _distStr += distStr[i];
            }

            times[0] = long.Parse(_timeStr);
            dist[0] = long.Parse(_distStr);

            int product = 1;
            for (int i = 0; i < times.Length; i++)
            {
                int wins = 0;
                for (int j = 0; j < times[i]; j++)
                {
                    bool canWin = CheckValue(times[i], dist[i], j);
                    if (canWin)
                    {
                        wins++;
                    }
                }
                product *= wins;
            }

            Console.WriteLine($"Wins {product}");
        }
    }
}
