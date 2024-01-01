using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day20 : Day
    {
        class Node
        {
            class PulseData
            {
                public string source;
                public string dest;
                public bool high;
            }

            public static void SendPulse(bool high, string source, string dest)
            {
                PulseData data = new PulseData();
                data.source = source;
                data.dest = dest;
                data.high = high;
                PulseQueue.Enqueue(data);

                if (high)
                    highPulses++;
                else
                    lowPulses++;
            }

            public static bool ProcessQueue()
            {
                if (PulseQueue.Count == 0)
                    return false;

                PulseData data = PulseQueue.Dequeue();

                nodes[data.dest].Pulse(data.high, data.source);

                return true;
            }

            static Queue<PulseData> PulseQueue = new Queue<PulseData>();
            public static int lowPulses = 0, highPulses = 0;
            public static Dictionary<string, Node> nodes;
            public string name;
            public string[] dests = new string[0];
            bool connectionsSet = false;
            public virtual void Pulse(bool highPulse, string source)
            {
            }

            public virtual void SetConnections(string input)
            {
                if (connectionsSet)
                    return;

                connectionsSet = true;

                foreach (string dest in dests)
                {
                    if (!nodes.ContainsKey(dest))
                    {
                        Node node = new Node();
                        node.name = input;
                        nodes[dest] = node;
                    }
                    nodes[dest].SetConnections(name);
                }
            }
        }

        class FlipFlop : Node
        {
            public bool on = false;

            public override void Pulse(bool highPulse, string source)
            {
                base.Pulse(highPulse, source);

                if (highPulse)
                    return;

                on = !on;

                foreach (string dest in dests)
                    SendPulse(on, name, dest);
            }
        }

        class Conjunction : Node
        {
            Dictionary<string, bool> inputs = new Dictionary<string, bool>();

            //public bool high = false;
            public override void Pulse(bool highPulse, string source)
            {
                base.Pulse(highPulse, source);

                inputs[source] = highPulse;

                bool high = true;
                foreach (string input in inputs.Keys)
                {
                    high &= inputs[input];
                }

                foreach (string dest in dests)
                    SendPulse(!high, name, dest);
            }

            public override void SetConnections(string input)
            {
                inputs[input] = false;
                base.SetConnections(input);
            }
        }

        class Broadcaster : Node
        {
            public override void Pulse(bool highPulse, string source)
            {
                base.Pulse(highPulse, source);
                foreach (string dest in dests)
                {
                    SendPulse(highPulse, name, dest);
                }
            }
        }

        class Button : Node
        {
            public override void Pulse(bool highPulse, string source)
            {
                base.Pulse(false, name);
            }
        }

        Dictionary<string, Node> nodes = new Dictionary<string, Node>();

        public void Part1()
        {
            string[] lines = File.ReadAllLines("input20.txt");

            foreach (string line in lines)
            {
                string name = line.Substring(0, line.IndexOf(' '));
                string destStr = line.Substring(line.IndexOf('>') + 1);
                string[] dests = destStr.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                Node.nodes = nodes;

                Node node = null;

                if (name == "broadcaster")
                {
                    node = new Broadcaster();
                    node.name = name;
                }
                else if (name[0] == '%')
                {
                    node = new FlipFlop();
                    node.name = name.Substring(1);
                }
                else if (name[0] == '&')
                {
                    node = new Conjunction();
                    node.name = name.Substring(1);
                }
                else
                {
                    continue;
                }
                node.dests = dests;

                nodes[node.name] = node;
            }

            nodes["broadcaster"].SetConnections("button");

            for (int i = 0; i < 1000; i++)
            {
                //nodes["broadcaster"].Pulse(false, "button");
                Node.SendPulse(false, "button", "broadcaster");
                while (Node.ProcessQueue()) { }
            }

            Console.WriteLine($"High pulses: {Node.highPulses} Low: {Node.lowPulses}");
            int product = Node.highPulses * Node.lowPulses;
            Console.WriteLine($"Product: {product}");

            // 675553410 too low
        }

        public void Part2()
        {
            string[] lines = File.ReadAllLines("testinput20.txt");
        }
    }
}
