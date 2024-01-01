using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day1 : Day
    {
        public void Part1()
        {
            int total = 0;

            string[] lines = File.ReadAllLines("input1.txt");

            foreach (string line in lines)
            {
                int first = 0, last = 0;
                bool hasFirst = false;

                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] >= '0' && line[i] <= '9')
                    {
                        if (!hasFirst)
                        {
                            first = line[i] - '0';
                            hasFirst = true;
                        }
                        last = line[i] - '0';
                    }
                }

                int value = first * 10 + last;
                total += value;

                Console.WriteLine($"First {first} Last {last} Total {total} Line {line}");
            }

            Console.WriteLine("Total = " + total);

        }

        public void Part2()
        {
            int total = 0;

            string[] lines = File.ReadAllLines("input1.txt");

            foreach (string sline in lines)
            {
                string line = sline;
                int first = 0, last = 0;
                bool hasFirst = false;

                //line = line.Replace("one", "1");
                //line = line.Replace("two", "2");
                //line = line.Replace("three", "3");
                //line = line.Replace("four", "4");
                //line = line.Replace("five", "5");
                //line = line.Replace("six", "6");
                //line = line.Replace("seven", "7");
                //line = line.Replace("eight", "8");
                //line = line.Replace("nine", "9");

                for (int i = 0; i < line.Length; i++)
                {
                    int c = -1;
                    if (line[i] >= '0' && line[i] <= '9')
                    {
                        c = line[i] - '0';
                    }
                    else if (line.Substring(i).StartsWith("one"))
                    {
                        c = 1;
                    }
                    else if (line.Substring(i).StartsWith("two"))
                    {
                        c = 2;
                    }
                    else if (line.Substring(i).StartsWith("three"))
                    {
                        c = 3;
                    }
                    else if (line.Substring(i).StartsWith("four"))
                    {
                        c = 4;
                    }
                    else if (line.Substring(i).StartsWith("five"))
                    {
                        c = 5;
                    }
                    else if (line.Substring(i).StartsWith("six"))
                    {
                        c = 6;
                    }
                    else if (line.Substring(i).StartsWith("seven"))
                    {
                        c = 7;
                    }
                    else if (line.Substring(i).StartsWith("eight"))
                    {
                        c = 8;
                    }
                    else if (line.Substring(i).StartsWith("nine"))
                    {
                        c = 9;
                    }

                    //if (line[i] >= '0' && line[i] <= '9')
                    if (c >= 0)
                    {
                        if (!hasFirst)
                        {
                            //first = line[i] - '0';
                            first = c;
                            hasFirst = true;
                        }
                        //last = line[i] - '0';
                        last = c;
                    }
                }

                int value = first * 10 + last;
                total += value;

                Console.WriteLine($"First {first} Last {last} Total {total} Line {sline}");
            }

            Console.WriteLine("Total = " + total);

        }
    }
}
