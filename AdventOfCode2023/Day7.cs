using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day7 : Day
    {
        enum Ranks
        {
            HighCard,
            OnePair,
            TwoPair,
            ThreeKind,
            FullHouse,
            FourKind,
            FiveKind
        }

        class Hand : IComparable
        {
            public int[] cards = new int[5];
            public int bet;
            public Ranks rank;
            public bool withJokers;
            public string input;

            int JCard = 11;

            public Hand(string input, bool withJokers = false)
            {
                string[] inputs = input.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                this.withJokers = withJokers;
                this.input = input;

                JCard = withJokers ? 1 : 11;

                for (int i = 0; i < 5; i++)
                {
                    cards[i] = inputs[0][i];

                    if (cards[i] >= '0' && cards[i] <= '9')
                        cards[i] = cards[i] - '0';
                    else if (cards[i] == 'T')
                        cards[i] = 10;
                    else if (cards[i] == 'J')
                        cards[i] = JCard;
                    else if (cards[i] == 'Q')
                        cards[i] = 12;
                    else if (cards[i] == 'K')
                        cards[i] = 13;
                    else if (cards[i] == 'A')
                        cards[i] = 14;
                }
                bet = int.Parse(inputs[1]);

                if (withJokers)
                    rank = GetRankWithJokers();
                else
                    rank = GetRank();
            }

            public int CompareTo(object? obj)
            {
                Hand hand = obj as Hand;

                if (rank < hand.rank)
                    return -1;
                if (rank > hand.rank)
                    return 1;

                for (int i = 0; i < 5; i++)
                {
                    if (cards[i] < hand.cards[i])
                        return -1;
                    else if (cards[i] > hand.cards[i])
                        return 1;
                }

                return 0;
            }

            Ranks GetRankWithJokers()
            {
                if (!cards.Contains(JCard))
                {
                    return GetRank();
                }

                Ranks rank = Ranks.HighCard;

                int[] newCards = new int[cards.Length];
                for (int i = 2; i <= 14; i++)
                {
                    if (i == 11) continue;
                    for (int j = 0; j < newCards.Length; j++)
                    {
                        newCards[j] = cards[j];
                        if (newCards[j] == JCard)
                            newCards[j] = i;
                    }
                    Ranks testRank = GetRank(newCards);
                    if (testRank > rank)
                        rank = testRank;
                }

                return rank;
            }

            Ranks GetRank()
            {
                return GetRank(cards);
            }

            Ranks GetRank(int[] cards)
            {
                var distinct = cards.Distinct();

                if (distinct.Count() == 1)
                {
                    return Ranks.FiveKind;
                }
                else if (distinct.Count() == 2)
                {
                    int max = 0;
                    for (int i = 0; i < distinct.Count(); i++)
                    {
                        int count = cards.Count(x => x == cards[i]);
                        if (count > max)
                        {
                            max = count;
                        }
                    }

                    if (max == 4)
                        return Ranks.FourKind;
                    else
                        return Ranks.FullHouse;
                }
                else if (distinct.Count() == 3)
                {
                    // can be 3 of a kind or full house
                    bool hasThree = false;
                    for (int i = 0; i < distinct.Count(); i++)
                    {
                        int count = cards.Count(x => x == cards[i]);
                        if (count == 3)
                            hasThree = true;
                    }

                    if (hasThree)
                        return Ranks.ThreeKind;
                    else
                        return Ranks.TwoPair;
                }
                else if (distinct.Count() == 4)
                {
                    return Ranks.OnePair;
                }
                else if (distinct.Count() == 5)
                {
                    return Ranks.HighCard;
                }

                return Ranks.HighCard;
            }
        }

        public void Part1()
        {
            string[] lines = File.ReadAllLines("input7.txt");

            List<Hand> hands = new List<Hand>();    

            foreach (string line in lines)
            {
                Hand hand = new Hand(line);
                hands.Add(hand);
            }

            hands.Sort();

            int total = 0;
            for (int i = 0; i < hands.Count; i++) {
                int win = hands[i].bet * (i + 1);
                Console.WriteLine($"Bet {hands[i].bet} Win {win}");
                total += win;
            }

            Console.WriteLine($"Total {total}");
        }

        public void Part2()
        {
            string[] lines = File.ReadAllLines("input7.txt");

            List<Hand> hands = new List<Hand>();

            foreach (string line in lines)
            {
                Hand hand = new Hand(line, true);
                hands.Add(hand);
            }

            hands.Sort();

            int total = 0;
            for (int i = 0; i < hands.Count; i++)
            {
                int win = hands[i].bet * (i + 1);
                Console.WriteLine($"Bet {hands[i].bet} Win {win} Rank {hands[i].rank} Input {hands[i].input}");
                total += win;
            }

            Console.WriteLine($"Total {total}");
        }
    }
}
