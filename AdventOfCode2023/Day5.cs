using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day5 : Day
    {
        class Mapping
        {
            public long src;
            public long dst;
            public long range;
        }

        List<long> seeds = new List<long>();
        List<Mapping> SeedToSoil = new List<Mapping>();
        List<Mapping> SoilToFertilizer = new List<Mapping>();
        List<Mapping> FertilizerToWater = new List<Mapping>();
        List<Mapping> WaterToLight = new List<Mapping>();
        List<Mapping> LightToTemperature = new List<Mapping>();
        List<Mapping> TemperatureToHumidity = new List<Mapping>();
        List<Mapping> HumidityToLocation = new List<Mapping>();

        public void Part1()
        {
            string[] lines = File.ReadAllLines("input5.txt");

            List<Mapping> mapping = null;

            foreach (string line in lines)
            {
                if (line.StartsWith("seeds:"))
                {
                    string seednums = line.Split(':')[1];
                    string[] seedStrs = seednums.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    foreach (string seedStr in seedStrs)
                    {
                        long seed = long.Parse(seedStr);
                        seeds.Add(seed);
                    }

                    continue;
                }

                if (line.StartsWith("seed-to-soil"))
                    mapping = SeedToSoil;
                else if (line.StartsWith("soil-to-fertilizer"))
                    mapping = SoilToFertilizer;
                else if (line.StartsWith("fertilizer-to-water"))
                    mapping = FertilizerToWater;
                else if (line.StartsWith("water-to-light"))
                    mapping = WaterToLight;
                else if (line.StartsWith("light-to-temperature"))
                    mapping = LightToTemperature;
                else if (line.StartsWith("temperature-to-humidity"))
                    mapping = TemperatureToHumidity;
                else if (line.StartsWith("humidity-to-location"))
                    mapping = HumidityToLocation;
                else if (!string.IsNullOrEmpty(line))
                {
                    string[] nums = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    Mapping m = new Mapping();
                    m.dst = long.Parse(nums[0]);
                    m.src = long.Parse(nums[1]);
                    m.range = long.Parse(nums[2]);
                    mapping.Add(m);
                }
            }

            long nearest = 1000000000000;
            foreach (long seed in seeds)
            {
                long location = GetMapping(HumidityToLocation, GetMapping(TemperatureToHumidity, GetMapping(LightToTemperature, GetMapping(WaterToLight, GetMapping(FertilizerToWater, GetMapping(SoilToFertilizer, GetMapping(SeedToSoil, seed)))))));
                if (location < nearest)
                    nearest = location;
            }

            Console.Write("Nearest: " + nearest);
        }

        long GetMapping(List<Mapping> mapping, long seed)
        {
            foreach (Mapping m in mapping)
            {
                if (seed >= m.src && seed < m.src + m.range)
                {
                    return seed - m.src + m.dst;
                }
            }

            return seed;
        }

        public void Part2()
        {
            string[] lines = File.ReadAllLines("input5.txt");

            List<Mapping> mapping = null;

            foreach (string line in lines)
            {
                if (line.StartsWith("seeds:"))
                {
                    string seednums = line.Split(':')[1];
                    string[] seedStrs = seednums.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    foreach (string seedStr in seedStrs)
                    {
                        long seed = long.Parse(seedStr);
                        seeds.Add(seed);
                    }

                    continue;
                }

                if (line.StartsWith("seed-to-soil"))
                    mapping = SeedToSoil;
                else if (line.StartsWith("soil-to-fertilizer"))
                    mapping = SoilToFertilizer;
                else if (line.StartsWith("fertilizer-to-water"))
                    mapping = FertilizerToWater;
                else if (line.StartsWith("water-to-light"))
                    mapping = WaterToLight;
                else if (line.StartsWith("light-to-temperature"))
                    mapping = LightToTemperature;
                else if (line.StartsWith("temperature-to-humidity"))
                    mapping = TemperatureToHumidity;
                else if (line.StartsWith("humidity-to-location"))
                    mapping = HumidityToLocation;
                else if (!string.IsNullOrEmpty(line))
                {
                    string[] nums = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    Mapping m = new Mapping();
                    m.dst = long.Parse(nums[0]);
                    m.src = long.Parse(nums[1]);
                    m.range = long.Parse(nums[2]);
                    mapping.Add(m);
                }
            }

            long nearest = 1000000000000;

            bool[] finished = new bool[seeds.Count / 2];
            long[] result = new long[seeds.Count / 2];

            for (int i = 0; i < seeds.Count; i += 2)
            {
                long start = seeds[i];
                long range = seeds[i + 1];
                finished[i / 2] = false;
                result[i / 2] = 1000000000000;

                //for (long seed = start; seed < start + range; seed++)
                //{
                //    long location = GetMapping(HumidityToLocation, GetMapping(TemperatureToHumidity, GetMapping(LightToTemperature, GetMapping(WaterToLight, GetMapping(FertilizerToWater, GetMapping(SoilToFertilizer, GetMapping(SeedToSoil, seed)))))));
                //    if (location < nearest)
                //        nearest = location;
                //}
                int index = i / 2;
                Thread thread = new Thread(() => DoTest(start, range, finished, result, index));
                thread.Start();

                //Console.WriteLine("Finished seed " + i);
            }

            bool done;
            do
            {
                done = true;
                for (int i = 0; i < finished.Length; i++)
                    done &= finished[i];
            } while (!done);

            for (int i = 0; i < result.Length; i++)
                if (result[i] < nearest)
                    nearest = result[i];

            // 31161857

            Console.Write("Nearest: " + nearest);
        }

        void DoTest(long start, long range, bool[] done, long[] nearest, int index)
        {
            for (long seed = start; seed < start + range; seed++)
            {
                long location = GetMapping(HumidityToLocation, GetMapping(TemperatureToHumidity, GetMapping(LightToTemperature, GetMapping(WaterToLight, GetMapping(FertilizerToWater, GetMapping(SoilToFertilizer, GetMapping(SeedToSoil, seed)))))));
                if (location < nearest[index])
                    nearest[index] = location;
            }

            Console.WriteLine("Thread done " + index);
            done[index] = true;
        }
    }
}
