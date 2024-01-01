#include "Day20.h"

#include <string>
#include <queue>
#include <map>
#include <vector>
#include <fstream>
#include <iostream>

namespace {
    struct PulseData
    {
        std::string source;
        std::string dest;
        bool high;

        PulseData(const std::string &_source, const std::string &_dest, bool _high)
            : source(_source), dest(_dest), high(_high)
        {
        }
    };

    std::queue<PulseData> pulseQueue;
    int highPulses = 0, lowPulses = 0;

    class Node;
    class Conjunction;

    // Part 2
    bool monitoring = false;
    Conjunction *junction = nullptr;
    std::map<std::string, bool> monitorFound;
    std::map<std::string, int> monitorCounts;
    int buttonCount = 0;


    static std::map<std::string, Node *> nodes;

    class Node
    {
    public:
        std::string name;
        std::vector<std::string> dests;

        Node(const std::string &_name)
            : name(_name)
        {
        }

        static void SendPulse(bool high, std::string source, std::string dest)
        {
            PulseData data(source, dest, high);
            pulseQueue.push(data);

            if (nodes.find(dest) == nodes.end())
            {
                std::cout << "error" << std::endl;
            }

            if (high)
                highPulses++;
            else
                lowPulses++;
        }

        virtual void Pulse(bool high, const std::string &source)
        {
        }

        virtual void SetConnections(const std::string &input)
        {
            if (connectionsSet)
                return;

            connectionsSet = true;

            for (auto &dest : dests) {
                if (nodes.count(dest) == 0) {
                    Node *node = new Node(input);
                    nodes[dest] = node;
                }
                nodes[dest]->SetConnections(name);
            }
        }

    protected:
        bool connectionsSet = false;
    };

    class FlipFlop : public Node
    {
    public:
        bool on = false;

        FlipFlop(const std::string &_name)
            : Node(_name) {}

        void Pulse(bool highPulse, const std::string &source)
        {
            if (highPulse)
                return;

            on = !on;

            for (auto &dest : dests)
                SendPulse(on, name, dest);
        }
    };

    class Conjunction : public Node
    {
    public:
        std::map<std::string, bool> inputs;

        Conjunction(const std::string &_name)
            : Node(_name) {}

        void Pulse(bool highPulse, const std::string &source)
        {
            if (inputs.find(source) == inputs.end())
            {
                std::cout << "error" << std::endl;
            }
            //inputs[source] = !inputs[source];
            inputs[source] = highPulse;

            bool high = true;
            for (auto &input : inputs)
                high &= input.second;

            for (auto &dest : dests)
                SendPulse(!high, name, dest);
        }

        void SetConnections(const std::string &input)
        {
            inputs[input] = false;
            Node::SetConnections(input);
        }
    };

    class Broadcaster : public Node
    {
    public:
        Broadcaster(const std::string &_name)
            : Node(_name) {}

        void Pulse(bool highPulse, const std::string &source)
        {
            for (auto &dest : dests)
            {
                SendPulse(highPulse, name, dest);
            }
        }
    };

    bool ProcessQueue()
    {
        if (pulseQueue.empty())
            return false;

        PulseData &data = pulseQueue.front();

        nodes[data.dest]->Pulse(data.high, data.source);
        pulseQueue.pop();

        return true;
    }

}

void Day20::Setup(const char *inputfile)
{
    std::ifstream file(inputfile, std::ifstream::in);
    std::string str;

    while (std::getline(file, str)) {
        std::string name = str.substr(0, str.find(' '));
        std::string destStr = str.substr(str.find('>') + 2);
        std::vector<std::string> dests;

        size_t i = 0;
        size_t ii;
        do {
            ii = destStr.find(',', i);
            dests.push_back(destStr.substr(i, ii - i));
            i = ii + 2;
        } while (i < destStr.size() && ii != destStr.npos);

        Node *node = nullptr;

        if (name == "broadcaster")
        {
            node = new Broadcaster(name);
        }
        else if (name[0] == '%')
        {
            node = new FlipFlop(name.substr(1));
        }
        else if (name[0] == '&')
        {
            node = new Conjunction(name.substr(1));
        }
        else
        {
            continue;
        }
        node->dests = dests;

        nodes[node->name] = node;

        //std::cout << "Data: " << entry->data << " Pattern: " << pattern << std::endl;
    }

    nodes["broadcaster"]->SetConnections("button");
}

void Day20::Part1()
{
    Setup("input20.txt");

    for (int i = 0; i < 1000; i++) {
        Node::SendPulse(false, "button", "broadcaster");

        while (ProcessQueue()) {
        }
    }

    // too low - Low: 15030 High: 44947 Product: 675553410

    std::cout << "Low: " << lowPulses << " High: " << highPulses << " Product: " << (lowPulses * highPulses) << std::endl;
}

void Day20::Part2()
{
    Setup("input20.txt");

    for (auto &i : nodes) {
        if (!i.second->dests.empty() && i.second->dests[0] == "rx") {
             junction = static_cast<Conjunction *>(i.second);
             break;
        }
    }

    if (junction == nullptr)
        return;

    for (auto &i : junction->inputs) {
        monitorFound[i.first] = false;
        monitorCounts[i.first] = 0;
    }

    bool done = true;
    buttonCount = 0;
    do {
        Node::SendPulse(false, "button", "broadcaster");
        buttonCount++;

        while (ProcessQueue()) {
            for (auto &input : junction->inputs) {
                if (monitorFound[input.first])
                    continue;

                if (input.second) {
                    monitorFound[input.first] = true;
                    monitorCounts[input.first] = buttonCount;
                }
            }
        }

        done = true;
        for (auto f : monitorFound)
            done &= f.second;
    } while (!done);

    long long product = 1;
    for (auto &count : monitorCounts) {
        std::cout << count.first << " repeats after " << count.second << std::endl;
        product *= count.second;
        //std::cout << "Product = " << product << std::endl;
    }

    std::cout << "LCM = " << product << std::endl;
}
