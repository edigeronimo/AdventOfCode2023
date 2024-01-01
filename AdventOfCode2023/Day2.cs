using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day2 : Day
    {
        class Game
        {
            public int num;
            public int blue, red, green;
        }

        List<Game> Games = new List<Game>();

        public void Part1()
        {
            Games = new List<Game>();
            string[] lines = File.ReadAllLines("input2.txt");

            int count = 1;
            foreach (string line in lines)
            {
                Game game = new Game();
                game.num = count++;

                string l = line.Substring(line.IndexOf(':') + 1);
                string[] inputs = l.Split(new char[] { ';' });

                foreach (string input in inputs)
                {
                    string[] colors = input.Split(new char[] { ',' });
                    foreach (string colordata in colors)
                    {
                        string[] color = colordata.Trim().Split(' ');
                        int value = int.Parse(color[0]);
                        //string color = colors[1].Trim();
                        if (color[1].Trim() == "blue")
                            game.blue = Math.Max(value, game.blue);
                        else if (color[1].Trim() == "red")
                            game.red = Math.Max(value, game.red);
                        else if (color[1].Trim() == "green")
                            game.green = Math.Max(value, game.green);
                    }
                }

                Games.Add(game);
            }

            int maxR = 12, maxG = 13, maxB = 14;
            int total = 0;

            foreach (Game game1 in Games)
            {
                if (game1.red <= maxR && game1.blue <= maxB && game1.green <= maxG)
                {
                    total += game1.num;
                }
            }

            Console.WriteLine("Total = " + total);
        }

        public void Part2()
        {
            Games = new List<Game>();
            string[] lines = File.ReadAllLines("input2.txt");

            int count = 1;
            foreach (string line in lines)
            {
                Game game = new Game();
                game.num = count++;

                string l = line.Substring(line.IndexOf(':') + 1);
                string[] inputs = l.Split(new char[] { ';' });

                foreach (string input in inputs)
                {
                    string[] colors = input.Split(new char[] { ',' });
                    foreach (string colordata in colors)
                    {
                        string[] color = colordata.Trim().Split(' ');
                        int value = int.Parse(color[0]);
                        //string color = colors[1].Trim();
                        if (color[1].Trim() == "blue")
                            game.blue = Math.Max(value, game.blue);
                        else if (color[1].Trim() == "red")
                            game.red = Math.Max(value, game.red);
                        else if (color[1].Trim() == "green")
                            game.green = Math.Max(value, game.green);
                    }
                }

                Games.Add(game);
            }

            int total = 0;

            foreach (Game game1 in Games)
            {
                total += game1.red * game1.blue * game1.green;
            }

            Console.WriteLine("Total = " + total);
        }
    }
}
