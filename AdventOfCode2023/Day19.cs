using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day19 : Day
    {
        class Rule
        {
            public char op;
            public string variable;
            public int value;
            public string next;

            public void Test(Part part, out bool accepted, out bool rejected, out string nextRule)
            {
                bool pass = false;

                accepted = false;
                rejected = false;
                nextRule = null;

                if (variable != null)
                {
                    if (op == '<')
                    {
                        if (variable == "x")
                            pass = part.x < value;
                        if (variable == "m")
                            pass = part.m < value;
                        if (variable == "a")
                            pass = part.a < value;
                        if (variable == "s")
                            pass = part.s < value;
                    }
                    else
                    {
                        if (variable == "x")
                            pass = part.x > value;
                        if (variable == "m")
                            pass = part.m > value;
                        if (variable == "a")
                            pass = part.a > value;
                        if (variable == "s")
                            pass = part.s > value;
                    }
                }
                else
                {
                    pass = true;
                }

                if (pass)
                {
                    if (next == "A")
                        accepted = true;
                    else if (next == "R")
                        rejected = true;
                    else
                        nextRule = next;
                }
            }
        }

        class Part
        {
            public int x, m, a, s;
            public Part(string[] strs)
            {
                string[] split = strs[0].Split('=');
                x = int.Parse(split[1]);
                split = strs[1].Split('=');
                m = int.Parse(split[1]);
                split = strs[2].Split('=');
                a = int.Parse(split[1]);
                split = strs[3].Split('=');
                s = int.Parse(split[1]);
            }
        }

        Dictionary<string, List<Rule>> rules = new Dictionary<string, List<Rule>>();
        List<Part> parts = new List<Part>();

        void ParseData(string[] lines)
        {
            bool parsingRules = true;
            foreach (string line in lines)
            {
                if (parsingRules)
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        parsingRules = false;
                        continue;
                    }

                    int open = line.IndexOf('{');
                    string name = line.Substring(0, open);
                    string rulesStr = line.Substring(open + 1, line.Length - open - 2);

                    List<Rule> ruleList = new List<Rule>();

                    string[] ruleStrs = rulesStr.Split(',');
                    foreach (string ruleStr in ruleStrs)
                    {
                        Rule rule = new Rule();
                        if (ruleStr.IndexOf(':') < 0)
                        {
                            rule.next = ruleStr;
                        }
                        else if (ruleStr.Contains('<'))
                        {
                            rule.op = '<';
                            rule.variable = ruleStr.Substring(0, ruleStr.IndexOf('<'));
                            string ruleValueStr = ruleStr.Substring(ruleStr.IndexOf('<') + 1);
                            ruleValueStr = ruleValueStr.Substring(0, ruleValueStr.IndexOf(':'));
                            rule.value = int.Parse(ruleValueStr);
                            rule.next = ruleStr.Substring(ruleStr.IndexOf(':') + 1);
                        }
                        else if (ruleStr.Contains('>'))
                        {
                            rule.op = '>';
                            rule.variable = ruleStr.Substring(0, ruleStr.IndexOf('>'));
                            string ruleValueStr = ruleStr.Substring(ruleStr.IndexOf('>') + 1);
                            ruleValueStr = ruleValueStr.Substring(0, ruleValueStr.IndexOf(':'));
                            rule.value = int.Parse(ruleValueStr);
                            rule.next = ruleStr.Substring(ruleStr.IndexOf(':') + 1);
                        }
                        ruleList.Add(rule);
                    }

                    rules[name] = ruleList;
                }
                else
                {
                    string stripped = line.Substring(1, line.Length - 2);
                    string[] valueStrs = stripped.Split(',');
                    Part part = new Part(valueStrs);
                    parts.Add(part);
                }
            }
        }

        public void Part1()
        {
            string[] lines = File.ReadAllLines("input19.txt");

            ParseData(lines);

            List<Part> acceptedParts = new List<Part>();

            foreach (Part part in parts)
            {
                string nextRule = "in";
                bool accepted = false, rejected = false;
                while (!accepted && !rejected)
                {
                    List<Rule> ruleList = rules[nextRule];

                    foreach (Rule rule in ruleList)
                    {
                        string next;
                        rule.Test(part, out accepted, out rejected, out next);
                        if (accepted)
                            acceptedParts.Add(part);
                        if (accepted || rejected)
                            break;
                        if (next != null)
                        {
                            nextRule = next;
                            break;
                        }
                    }
                }
            }

            int sum = 0;
            foreach (Part part in acceptedParts)
            {
                sum += part.x + part.m + part.s + part.a;
            }

            Console.WriteLine(($"Sum: {sum}"));
        }

        class Node
        {
            public int[] lower = new int[4], upper = new int[4];

            public Node left, right;
            public Node parent = null;
            public bool accepted = false;

            public Node()
            {
                for (int i = 0; i < 4; i++)
                {
                    lower[i] = 1;
                    upper[i] = 4000;
                }
            }

            public Node(Node n)
            {
                for (int i = 0; i < 4; i++)
                {
                    lower[i] = n.lower[i];
                    upper[i] = n.upper[i];
                }
                parent = n;
            }
        }

        public static int GetIndex(string variable)
        {
            if (variable == "x")
                return 0;
            else if (variable == "m")
                return 1;
            else if (variable == "a")
                return 2;
            return 3;
        }

        /*
        Node WalkNode(Node parent, string ruleName, int index)
        {
            List<Rule> ruleList = rules[ruleName];

            if (ruleList[index].variable == null)
            {

            }

            Node left = new Node(parent);
            Node right = new Node(parent);
            int varindex = Node.GetIndex(ruleList[index].variable);
            if (ruleList[index].op == '<')
            {
                left.upper[varindex] = ruleList[index].value - 1;
                right.lower[varindex] = ruleList[index].value + 1;
            }
            else
            {
                left.lower[varindex] = ruleList[index].value + 1;
                right.upper[varindex] = ruleList[index].value - 1;
            }
            parent.left = left;
            parent.right = right;

            return node;
        }
        */

        struct Bounds
        {
            public int[] lower = new int[4], upper = new int[4];

            public Bounds()
            {
                for (int i = 0; i < 4; i++)
                {
                    lower[i] = 1;
                    upper[i] = 4000;
                }
            }

            public Bounds(Bounds b) {
                for (int i = 0; i < 4; i++)
                {
                    lower[i] = b.lower[i];
                    upper[i] = b.upper[i];
                }
            }

            public ulong GetTotal()
            {
                ulong total = 1;
                for (int i = 0; i < 4; i++)
                {
                    total *= (ulong)upper[i] - (ulong)lower[i] + 1;
                }
                return total;
            }
        }

        List<Bounds> results = new List<Bounds>();

        void WalkNode(string ruleName, Bounds bounds)
        {
            if (ruleName == "A")
            {
                //int total = 1;
                //for (int i = 0; i < 4; i++)
                //{
                //    total *= bounds.upper[i] - bounds.lower[i] + 1;
                //}
                //return total;
                results.Add(bounds);
                return;
            }

            if (ruleName == "R")
            {
                //return 0;
                return;
            }

            List<Rule> ruleList = rules[ruleName];
            foreach (Rule rule in ruleList)
            {
                if (rule.variable == null)
                {
                    WalkNode(rule.next, bounds);
                }
                else
                {
                    if (rule.op == '<')
                    {
                        Bounds newBounds = new Bounds(bounds);
                        newBounds.upper[GetIndex(rule.variable)] = Math.Min(rule.value - 1, newBounds.upper[GetIndex(rule.variable)]);
                        WalkNode(rule.next, newBounds);
                        bounds.lower[GetIndex(rule.variable)] = rule.value;
                    }
                    else
                    {
                        Bounds newBounds = new Bounds(bounds);
                        newBounds.lower[GetIndex(rule.variable)] = Math.Max(rule.value + 1, newBounds.lower[GetIndex(rule.variable)]);
                        WalkNode(rule.next, newBounds);
                        bounds.upper[GetIndex(rule.variable)] = rule.value;
                    }
                }
            }
        }

        public void Part2()
        {
            string[] lines = File.ReadAllLines("input19.txt");

            ParseData(lines);

            Bounds bounds = new Bounds();

            int stackSize = 50 * 1024 * 1024;
            Thread thread = new Thread(() => WalkNode("in", bounds), stackSize);
            thread.Start();
            thread.Join();

            //WalkNode("in", bounds);

            ulong total = 0;
            if (results.Count > 0)
            {
                total = results[0].GetTotal();
                for (int i = 1; i < results.Count; i++)
                {
                    ulong t = results[i].GetTotal();
                    total += t;
                }
            }

            Console.WriteLine($"Total: {total}");
        }
    }
}
