#include "Day12.h"

#include <vector>
#include <iostream>
#include <fstream>
#include <string>
#include <sstream>
#include <thread>
#include <mutex>
#include <inttypes.h>

namespace {

struct Entry {
    std::string data;
    std::vector<int> pattern;
    int hashCount;
    Entry() : hashCount(0) {}
};

uint64_t TestString(Entry *entry, char *buffer, int index, int hashCount, int &failAt);
bool IsMatch(Entry *entry, char *data, int &failedAt);
void DoWork(int index, const std::vector<Entry *> &entries, uint64_t *results);

std::mutex workMutex;
int workIndex = 0;

}

void Day12::Part1()
{
    std::ifstream file("input12.txt", std::ifstream::in);
    std::string str;

    std::vector<Entry *> entries;

    while (std::getline(file, str)) {
        Entry *entry = new Entry();
        size_t split = str.find(' ');
        entry->data = str.substr(0, split);
        std::string pattern = str.substr(split + 1);

        std::stringstream ss(pattern);
        for (int i; ss >> i;) {
            entry->pattern.push_back(i);
            entry->hashCount += i;
            if (ss.peek() == ',')
                ss.ignore();
        }

        entries.push_back(entry);

        //std::cout << "Data: " << entry->data << " Pattern: " << pattern << std::endl;
    }

    char testbuffer[100000];

    uint64_t total = 0;
    for (Entry *entry : entries) {
        int fail;
        uint64_t matches = TestString(entry, testbuffer, 0, 0, fail);
        std::cout << "Matches: " << matches << std::endl;
        total += matches;
    }

    std::cout << "Total: " << total << std::endl;
}

void Day12::Part2()
{
    std::ifstream file("input12.txt", std::ifstream::in);
    std::string str;

    std::vector<Entry *> entries;

    while (std::getline(file, str)) {
        Entry *entry = new Entry();
        size_t split = str.find(' ');
        entry->data = str.substr(0, split);
        for (int i = 0; i < 4; i++) {
            entry->data += "?";
            entry->data += str.substr(0, split);
        }
        std::string pattern = str.substr(split + 1);

        for (int ii = 0; ii < 5; ii++) {
            std::stringstream ss(pattern);
            for (int i; ss >> i;) {
                entry->pattern.push_back(i);
                entry->hashCount += i;
                if (ss.peek() == ',')
                    ss.ignore();
            }
        }

        entries.push_back(entry);

        //std::cout << "Data: " << entry->data << " Pattern: " << pattern << std::endl;
    }

    uint64_t *results = new uint64_t[entries.size()];
    for (int i = 0; i < entries.size(); i++)
        results[i] = 0;

    std::vector<std::thread> threads;

    workIndex = std::thread::hardware_concurrency();
    for (unsigned int t = 0; t < std::thread::hardware_concurrency(); t++) {
        threads.emplace_back(std::thread(DoWork, t, entries, results));
    }

    for (auto &t : threads) {
        t.join();
    }

    uint64_t total = 0;
    for (int i = 0; i < entries.size(); i++)
        total += results[i];

    delete[] results;

    std::cout << "Total: " << total << std::endl;
    while (true) {}
}

namespace {

void DoWork(int index, const std::vector<Entry *> &entries, uint64_t *results)
{
    char testbuffer[100000];

    /*
    for (int i = index; i < entries.size(); i += std::thread::hardware_concurrency()) {
        Entry *entry = entries[i];
        int fail;
        uint64_t matches = TestString(entry, testbuffer, 0, 0, fail);
        std::cout << "Matches: " << matches << std::endl;
        results[i] = matches;
    }
    */

    Entry *entry = nullptr;
    uint64_t *result = nullptr;

    while (index < entries.size()) {
        entry = entries[index];
        result = &results[index];

        int fail;
        *result = TestString(entry, testbuffer, 0, 0, fail);

        workMutex.lock();
        index = workIndex;
        workIndex++;
        workMutex.unlock();

        std::cout << "Matches: " << *result << " Next index: " << index << std::endl;
    } while (entry != nullptr);
}

uint64_t TestString(Entry *entry, char *buffer, int index, int hashCount, int &failedAt)
{
    if (index == entry->data.size()) {
        buffer[index] = 0;
        bool match = IsMatch(entry, buffer, failedAt) ? 1 : 0;
        //if (match) {
        //    std::cout << "Match: " << buffer << std::endl;
        //}
        return match ? 1 : 0;
    }

    if (hashCount > entry->hashCount)
        return 0;

    if (entry->data[index] == '?') {
        uint64_t t1, t2;
        buffer[index] = '.';
        t1 = TestString(entry, buffer, index + 1, hashCount, failedAt);
        if (t1 == 0 && failedAt < index) {
            return 0;
        }

        buffer[index] = '#';
        t2 = TestString(entry, buffer, index + 1, hashCount + 1, failedAt);
        return t1 + t2;
    }
    else {
        buffer[index] = entry->data[index];
        int newHash = hashCount;
        if (buffer[index] == '#')
            newHash++;
        return TestString(entry, buffer, index + 1, newHash, failedAt);
    }
}

bool IsMatch(Entry *entry, char *data, int &failAt)
{
    int i = 0;

    int j = 0;
    for (; j < entry->pattern.size(); j++) {
        while (data[i] == '.' && data[i] != 0)
            i++;

        for (int ii = i; ii < i + entry->pattern[j]; ii++) {
            if (data[ii] == 0 || data[ii] != '#') {
                failAt = ii;
                return false;
            }
        }
        i += entry->pattern[j];
        if (data[i] == '#') {
            failAt = i;
            return false;
        }
        if (data[i] == 0) {
            if (j == entry->pattern.size() - 1) {
                failAt = -1;
                return true;
            }
            else {
                failAt = i;
                return false;
            }
        }
    }

    for (; data[i] == '.'; i++);

    if (data[i] == 0) {
        failAt = -1;
        return true;
    }

    return false;
}

}
