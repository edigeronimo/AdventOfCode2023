using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day15 : Day
    {
        public void Part1()
        {
            string[] lines = File.ReadAllLines("input15.txt");

            string[] entries = lines[0].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            int total = 0;
            foreach (string entry in entries)
            {
                Console.WriteLine(Hash(entry));
                total += Hash(entry);
            }
            Console.WriteLine(total);
        }

        public void Part2()
        {
            string[] lines = File.ReadAllLines("input15.txt");

            string[] steps = lines[0].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            List<List<string>> boxes = new List<List<string>>();
            for (int i = 0; i < 256; i++)
                boxes.Add(new List<string>());

            foreach (string step in steps)
            {
                char op;
                string[] labels;
                string name;
                if (step.Contains('-'))
                {
                    op = '-';
                    labels = step.Split('-');
                    name = labels[0];

                    int box = Hash(labels[0]);

                    for (int j = 0; j < boxes[box].Count; j++)
                    {
                        if (boxes[box][j].StartsWith(name))
                        {
                            boxes[box].RemoveAt(j);
                            break;
                        }
                    }
                }
                else
                {
                    op = '=';
                    labels = step.Split('=');

                    int box = Hash(labels[0]);
                    name = labels[0] + " " + labels[1];

                    bool removed = false;
                    for (int j = 0; j < boxes[box].Count; j++)
                    {
                        if (boxes[box][j].StartsWith(labels[0]))
                        {
                            boxes[box][j] = name;
                            removed = true;
                            break;
                        }
                    }

                    if (!removed)
                    {
                        boxes[box].Add(name);
                    }
                }
            }

            int total = 0;
            for (int boxnum = 0; boxnum < boxes.Count; boxnum++)
            {
                var box = boxes[boxnum];
                for (int i = 0; i < box.Count; i++)
                {
                    string[] parts = box[i].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    int f = int.Parse(parts[1]);
                    int boxtotal = (1 + boxnum) * (i + 1) * f;
                    total += boxtotal;
                }
            }

            Console.WriteLine(total);
        }

        int Hash(string str)
        {
            int hash = 0;
            foreach (char c in str)
            {
                hash += c;
                hash *= 17;
                hash %= 256;
            }

            return hash;
        }
    }
}
