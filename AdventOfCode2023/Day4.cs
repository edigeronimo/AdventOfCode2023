using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day4 : Day
    {
        class Card
        {
            public int game;
            public List<int> winners = new List<int>();
            public List<int> numbers = new List<int>();
            public int copies = 1;
        }

        public void Part1()
        {
            string[] lines = File.ReadAllLines("input4.txt");

            List<Card> cards = new List<Card>();

            int score = 0;

            foreach (string line in lines)
            {
                Card card = new Card();
                string[] gameline = line.Split(':');
                string[] gameid = gameline[0].Trim().Split(' ');
                card.game = int.Parse(gameid[gameid.Length-1]);
                string[] gamevalues = gameline[1].Trim().Split('|');
                string[] winners = gamevalues[0].Trim().Split(' ');
                foreach (string value in winners)
                {
                    if (string.IsNullOrWhiteSpace(value)) continue;
                    card.winners.Add(int.Parse(value));
                }
                string[] numbers = gamevalues[1].Trim().Split(' ');
                foreach (string value in numbers)
                {
                    if (string.IsNullOrWhiteSpace(value)) continue;
                    card.numbers.Add(int.Parse(value));
                }

                int gameScore = 0;
                foreach (int value in card.numbers)
                {
                    if (card.winners.Contains(value))
                    {
                        if (gameScore == 0)
                            gameScore = 1;
                        else
                            gameScore *= 2;
                    }
                }

                cards.Add(card);
                score += gameScore;

                Console.WriteLine($"Game {gameScore} Total {score}");
            }
        }

        public void Part2()
        {
            string[] lines = File.ReadAllLines("input4.txt");

            List<Card> cards = new List<Card>();

            int score = 0;

            foreach (string line in lines)
            {
                Card card = new Card();
                string[] gameline = line.Split(':');
                string[] gameid = gameline[0].Trim().Split(' ');
                card.game = int.Parse(gameid[gameid.Length - 1]);
                string[] gamevalues = gameline[1].Trim().Split('|');
                string[] winners = gamevalues[0].Trim().Split(' ');
                foreach (string value in winners)
                {
                    if (string.IsNullOrWhiteSpace(value)) continue;
                    card.winners.Add(int.Parse(value));
                }
                string[] numbers = gamevalues[1].Trim().Split(' ');
                foreach (string value in numbers)
                {
                    if (string.IsNullOrWhiteSpace(value)) continue;
                    card.numbers.Add(int.Parse(value));
                }

                cards.Add(card);
            }

            foreach (Card card in cards)
            {
                int gameScore = 0;
                foreach (int value in card.numbers)
                {
                    if (card.winners.Contains(value))
                        gameScore++;
                }

                for (int i = card.game; i < card.game + gameScore && i < cards.Count; i++)
                {
                    cards[i].copies += card.copies;
                }
            }

            foreach (Card card in cards)
            {
                score += card.copies;
            }

            Console.WriteLine($"Total {score}");
        }
    }
}
