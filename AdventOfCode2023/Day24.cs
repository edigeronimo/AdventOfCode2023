using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day24 : Day
    {
        struct Snow
        {
            public Vec3L pos;
            public Vec3L vel;
        }

        public void Part1()
        {
            const long boundsMin = 200000000000000;
            const long boundsMax = 400000000000000;
            string[] lines = File.ReadAllLines("input24.txt");

            // test values
            //const long boundsMin = 7;
            //const long boundsMax = 27;
            //string[] lines = File.ReadAllLines("testinput24.txt");

            List<Snow> particles = new List<Snow>();



            foreach (string line in lines)
            {
                string[] parts = line.Split('@');

                Snow snow = new Snow();
                snow.pos = Vec3L.Parse(parts[0].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
                snow.vel = Vec3L.Parse(parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
                particles.Add(snow);
            }

            /*
             * y1 = m1x1 + b1
             * y2 = m2x2 + b2
             * 
             * x1==x2, y1==y2
             * 
             * m1x + b1 = m2x + b2
             * m1x = m2x + b2 - b1
             * m1x - m2x = b2 - b1
             * x(m1 - m2) = b2 - b1
             * x = (b2 - b1) / (m1 - m2)
             * 
             * y = mx + b
             * b = y - mx
             */

            long intersections = 0;

            for (int i = 0; i < particles.Count - 1; i++)
            {
                for (int j = i + 1;  j < particles.Count; j++)
                {
                    Vec3L p1a = particles[i].pos, p1b = particles[i].pos + particles[i].vel;
                    Vec3L p2a = particles[j].pos, p2b = particles[j].pos + particles[j].vel;

                    if (p1a.x == p1b.x || p2a.x == p2b.x)
                        continue; // both vertical

                    double p1slope = (p1a.y - p1b.y) / (double)(p1a.x - p1b.x);
                    double p2slope = (p2a.y - p2b.y) / (double)(p2a.x - p2b.x);
                    double p1intersect = p1a.y - p1slope * p1a.x;
                    double p2intersect = p2a.y - p2slope * p2a.x;

                    if (p1slope == p2slope)
                        continue; // parallel

                    double intx = (p2intersect - p1intersect) / (p1slope - p2slope);
                    double inty = p1slope * intx + p1intersect;

                    // Skip intersections in the past
                    if (particles[i].vel.x > 0 && intx < particles[i].pos.x) continue;
                    if (particles[i].vel.x < 0 && intx > particles[i].pos.x) continue;
                    if (particles[i].vel.y > 0 && inty < particles[i].pos.y) continue;
                    if (particles[i].vel.y < 0 && inty > particles[i].pos.y) continue;

                    if (particles[j].vel.x > 0 && intx < particles[j].pos.x) continue;
                    if (particles[j].vel.x < 0 && intx > particles[j].pos.x) continue;
                    if (particles[j].vel.y > 0 && inty < particles[j].pos.y) continue;
                    if (particles[j].vel.y < 0 && inty > particles[j].pos.y) continue;

                    if (intx >= boundsMin && intx <= boundsMax &&
                        inty >= boundsMin && inty <= boundsMax)
                    {
                        Console.WriteLine($"Lines {i} and {j} intersect at {intx}, {inty}");

                        intersections++;
                    }
                }
            }

            Console.WriteLine($"Total intersections: {intersections}");
        }

        public void Part2()
        {
            string[] lines = File.ReadAllLines("testinput24.txt");
        }
    }
}
