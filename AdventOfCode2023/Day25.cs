using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    class Wire : IEquatable<Wire>
    {
        public string n1, n2;
        public bool cut = false;

        public bool Equals(Wire? other)
        {
            return this == other;
        }

        public static bool operator==(Wire left, Wire right)
        {
            if (left.n1 == right.n1 && left.n2 == right.n2)
                return true;

            if (left.n1 == right.n2 && left.n2 == right.n1)
                return true;

            return false;
        }

        public static bool operator!=(Wire left, Wire right)
        {
            return !(left == right);
        }
    }

    internal class Day25 : Day
    {
        Dictionary<string, List<string>> connections = new Dictionary<string, List<string>>();
        List<string> allParts;
        bool foundResult = false;
        List<Wire> wires = new List<Wire>();

        public void Part1()
        {
            string[] lines = File.ReadAllLines("input25.txt");

            foreach (string line in lines)
            {
                string[] parts = line.Replace(':', ' ').Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                for (int i = 1; i < parts.Length; i++)
                {
                    if (!connections.ContainsKey(parts[0]))
                    {
                        connections[parts[0]] = new List<string>();
                    }
                    connections[parts[0]].Add(parts[i]);

                    if (!connections.ContainsKey(parts[i]))
                    {
                        connections[parts[i]] = new List<string>();
                    }
                    connections[parts[i]].Add(parts[0]);

                    Wire w = new Wire();
                    w.n1 = parts[0];
                    w.n2 = parts[i];
                    if (!wires.Contains(w))
                        wires.Add(w);
                }
            }

            allParts = connections.Keys.ToList();
            allParts.Sort();

            //foreach (string part in allParts)
            //{
            //    string str = part + " is connected to:";
            //    foreach (string key in connections[part])
            //        str += " " + key;
            //    Console.WriteLine(str);
            //}

            // Verification tests 
            /*
             * CutWire("hfx", "pzl");
             * CutWire("bvb", "cmg");
             * CutWire("nvd", "jqt");
             * 
             * //WalkNode(allParts[0]);
             * WalkNode("cmg");
             * WalkNode("bvb");
             */


            //CheckCut(1, 3);
            CheckCutViaWires();
        }

        void CheckCutViaWires()
        {
            for (int i = 0; i < wires.Count; i++)
            {
                CutWire(wires[i].n1, wires[i].n2);
                for (int j = i + 1; j < wires.Count; j++)
                {
                    CutWire(wires[j].n1, wires[j].n2);
                    for (int k = j + 1; k < wires.Count; k++)
                    {
                        CutWire(wires[k].n1, wires[k].n2);

                        List<string> remaining = allParts;

                        bool success = true;
                        int product = 1;
                        for (int check = 0; check < 3; check++)
                        {
                            if (remaining.Count == 0)
                            {
                                success = false;
                                break;
                            }
                            List<string> visited;
                            WalkNode(remaining[0], out visited);
                            if (check < 3)
                            {
                                remaining = remaining.Except(visited).ToList();
                            }
                            else
                            {
                                success = (remaining.Count == visited.Count);
                            }
                            product *= visited.Count;
                        }
                        foundResult = success;
                        if (foundResult)
                            Console.WriteLine($"Product: {product}");


                        AddWire(wires[k].n1, wires[k].n2);
                    }
                    AddWire(wires[j].n1, wires[j].n2);
                }
                AddWire(wires[i].n1, wires[i].n2);
            }
        }

        void CheckCut(int count, int maxDepth)
        {
            for (int i = 0; i < allParts.Count - 2; i++)
            {
                for (int j = i + 1; j < allParts.Count - 1; j++)
                {
                    if (!IsWire(allParts[i], allParts[j]))
                        continue;

                    CutWire(allParts[i], allParts[j]);

                    if (count < maxDepth)
                    {
                        CheckCut(count + 1, maxDepth);
                    }
                    else
                    {
                        List<string> remaining = allParts.ToList();

                        bool success = true;
                        int product = 1;
                        for (int check = 0; check < maxDepth; check++)
                        {
                            if (remaining.Count == 0)
                            {
                                success = false;
                                break;
                            }
                            List<string> visited;
                            WalkNode(remaining[0], out visited);
                            remaining = remaining.Except(visited).ToList();
                            product *= visited.Count;
                        }
                        foundResult = success && (remaining.Count == 0);
                        if (foundResult)
                            Console.WriteLine($"Product: {product}");
                    }

                    AddWire(allParts[i], allParts[j]);

                    if (foundResult)
                    {
                        Console.WriteLine($"Wire cut between {allParts[i]} and {allParts[j]}");
                        return;
                    }
                }
            }

        }

        bool IsWire(string n1, string n2)
        {
            // it's symmetrical
            return connections[n1].Contains(n2);
        }

        void CutWire(string n1, string n2)
        {
            connections[n1].Remove(n2);
            connections[n2].Remove(n1);
        }

        void AddWire(string n1, string n2)
        {
            connections[n1].Add(n2);
            connections[n2].Add(n1);
        }

        void WalkNode(string node, out List<string> visited)
        {
            visited = new List<string>();
            Queue<string> toCheck = new Queue<string>();

            toCheck.Enqueue(node);

            while (toCheck.Count > 0)
            {
                string next = toCheck.Dequeue();

                if (visited.Contains(next))
                    continue;

                visited.Add(next);

                foreach (string str in connections[next])
                {
                    if (!visited.Contains(str))
                        toCheck.Enqueue(str);
                }
            }

            string cycle = "";
            foreach (string str in visited)
            {
                cycle += str + " ";
            }

            //Console.WriteLine($"Cycle includes {visited.Count} nodes: {cycle}");
        }

        public void Part2()
        {
            string[] lines = File.ReadAllLines("testinput25.txt");
        }
    }
}
