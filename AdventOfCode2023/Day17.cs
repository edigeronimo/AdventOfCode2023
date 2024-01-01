using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day17 : Day
    {
        CommonGrid grid;

        public void Part1()
        {
            string[] lines = File.ReadAllLines("testinput17.txt");

            grid = new CommonGrid(lines);

            //int[] = Dijkstra(0, 0);
        }

        public void Part2()
        {
            string[] lines = File.ReadAllLines("testinput17.txt");

        }

        void Dijkstra(int x, int y)
        {
            int[] dist = new int[y * grid.width + x];
            int[] prev = new int[y * grid.width + x];

            List<int> queue = new List<int>();

            for (int i = 0; i < dist.Length; i++)
            {
                dist[i] = 1000000000;
                prev[i] = -1;

                queue.Add(i);
            }
            dist[0] = 0;

            List<int> neighbors = new List<int>();

            while (queue.Count > 0)
            {
                int node = queue[0];
                int mindist = dist[node];
                for (int i = 1; i < queue.Count; i++)
                {
                    if (dist[queue[i]] < mindist)
                    {
                        node = queue[i];
                        mindist = dist[queue[i]];
                    }
                }
                queue.Remove(node);

                neighbors.Clear();
                int nodex = node % grid.width, nodey = node / grid.width;
                int checkx, checky;
                
                //checkx = nodex - 1, checky = nodey;
                //if (checkx >= 0 && checkx < grid.width && checky >= 0 && checky < grid.height)
                //{
                //    int check = checky * grid.width + checkx;
                //    if (queue.Contains(check))
                //    {
                //        int alt = dist[node] + grid[check];
                //        if (alt < dist[check] {
                //            dist[check] = alt;
                //            prev[check] = node;
                //        })
                //    }
                //}
            }
        }
    }
}
