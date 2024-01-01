using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace AdventOfCode2023
{
    internal class Day8 : Day
    {
        class Node
        {
            public string name;
            public string left, right;

            public Node(string line)
            {
                name = line.Substring(0, 3);
                left = line.Substring(7, 3);
                right = line.Substring(12, 3);
            }
        }

        Dictionary<string, Node> nodes = new Dictionary<string, Node>();

        public void Part1()
        {
            string[] lines = File.ReadAllLines("input8.txt");

            string path = lines[0];

            foreach (string line in lines)
            {
                if (!line.Contains('='))
                    continue;

                Node node = new Node(line);
                nodes[node.name] = node;
            }

            int index = 0, count = 0;
            string pos = "AAA";
            while (pos != "ZZZ")
            {
                count++;
                char dir = path[index++];
                if (index >= path.Length)
                    index = 0;

                if (dir == 'L')
                    pos = nodes[pos].left;
                else
                    pos = nodes[pos].right;
            }

            Console.WriteLine($"Count {count}");
        }

        public void Part2()
        {
            string[] lines = File.ReadAllLines("input8.txt");

            string path = lines[0];
            List<string> paths = new List<string>();

            foreach (string line in lines)
            {
                if (!line.Contains('='))
                    continue;

                Node node = new Node(line);
                nodes[node.name] = node;

                if (node.name[2] == 'A')
                {
                    paths.Add(node.name);
                }
            }

            List<int> counts = new List<int>();
            foreach (string p in paths)
            {
                string pos = p;
                int index = 0, count = 0;
                while (pos[2] != 'Z')
                {
                    count++;
                    char dir = path[index++];
                    if (index >= path.Length)
                        index = 0;

                    if (dir == 'L')
                        pos = nodes[pos].left;
                    else
                        pos = nodes[pos].right;
                }
                counts.Add(count);
            }

            long lcmcount = 0;
            bool good = true;
            do
            {
                good = true;
                lcmcount++;
                long total = counts[0] * lcmcount;
                for (int i = 1; i < counts.Count; i++)
                {
                    if (total % counts[i] != 0)
                    {
                        good = false;
                        break;
                    }
                }
            } while (!good);

            Console.WriteLine("Count = " + lcmcount * counts[0]);


            // Brute force, this will take forever
            //int index = 0, count = 0;
            //while (!IsDone(paths))
            //{
            //    count++;
            //    char dir = path[index++];
            //    if (index >= path.Length)
            //        index = 0;

            //    for (int i = 0; i < paths.Count; i++)
            //    {
            //        if (dir == 'L')
            //            paths[i] = nodes[paths[i]].left;
            //        else
            //            paths[i] = nodes[paths[i]].right;
            //    }
            //}

            //Console.WriteLine($"Count {count}");
        }

        bool IsDone(List<string> paths)
        {
            foreach (string path in paths)
            {
                if (path[2] != 'Z')
                    return false;
            }

            return true;
        }
    }
}
