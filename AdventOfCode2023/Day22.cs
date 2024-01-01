using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day22 : Day
    {
        class Brick
        {
            public Vec3 p1, p2;
            public int num;
            public bool set;

            Vec3 Parse(string[] s)
            {
                return new Vec3(int.Parse(s[0]), int.Parse(s[1]), int.Parse(s[2]));
            }

            public Brick(string[] p1s, string[] p2s, int num)
            {
                p1 = Parse(p1s);
                p2 = Parse(p2s);
                this.num = num;
                set = false;
            }

            public Brick(Brick brick)
            {
                this.p1 = new Vec3(brick.p1);
                this.p2 = new Vec3(brick.p2);
                this.num = brick.num;
                this.set = brick.set;
            }
        }

        class Cube
        {
            int[] cube;
            int size;
            public List<Brick> bricks;

            public Cube(int size, List<Brick> bricks)
            {
                this.size = size;
                this.cube = new int[size * size * size];
                this.bricks = bricks;

                foreach (Brick brick in bricks)
                {
                    AddBrick(brick);
                }
            }

            public Cube(Cube c)
            {
                cube = new int[c.cube.Length];
                c.cube.CopyTo(cube, 0);
                size = c.size;
                bricks = new List<Brick>();

                foreach (Brick b  in c.bricks)
                    bricks.Add(new Brick(b));
            }

            public static bool operator==(Cube a, Cube b)
            {
                if (a.size != b.size) return false;
                for (int i = 0; i < a.cube.Length; i++)
                    if (a.cube[i] != b.cube[i])
                        return false;

                return true;
            }

            public static bool operator !=(Cube a, Cube b)
            {
                return !(a == b);
            }

            void Set(int x, int y, int z, int value)
            {
                cube[z * size * size + y * size + x] = value;
            }
            int Get(int x, int y, int z)
            {
                return cube[z * size * size + y * size + x];
            }

            void AddBrick(Brick brick)
            {
                SetBrick(brick, 1);
            }

            public void ClearBrick(Brick brick)
            {
                SetBrick(brick, 0);
            }

            void SetBrick(Brick brick, int value)
            {
                brick.set = value != 0;
                for (int x = brick.p1.x; x <= brick.p2.x; x++)
                {
                    for (int y = brick.p1.y; y <= brick.p2.y; y++)
                    {
                        for (int z = brick.p1.z; z <= brick.p2.z; z++)
                        {
                            Set(x, y, z, value);
                        }
                    }
                }
            }

            bool CanSetBrick(Brick brick)
            {
                for (int x = brick.p1.x; x <= brick.p2.x; x++)
                {
                    for (int y = brick.p1.y; y <= brick.p2.y; y++)
                    {
                        for (int z = brick.p1.z; z <= brick.p2.z; z++)
                        {
                            if (Get(x, y, z) != 0)
                                return false;
                        }
                    }
                }

                return true;
            }

            public void SettleBricks(List<int> moved = null)
            {
                bool changed = false;
                do
                {
                    changed = false;
                    for (int i = 0; i < bricks.Count; i++)
                    {
                        Brick brick = bricks[i];
                        if (!brick.set)
                            continue;

                        ClearBrick(brick);
                        brick.p1.z--;
                        brick.p2.z--;
                        if (brick.p1.z > 0 && brick.p2.z > 0 && CanSetBrick(brick))
                        {
                            SetBrick(brick, 1);
                            changed = true;
                            //Console.WriteLine("Moved brick " + brick.num);

                            if (moved != null)
                            {
                                if (!moved.Contains(i))
                                    moved.Add(i);
                            }
                        }
                        else
                        {
                            brick.p1.z++;
                            brick.p2.z++;
                            SetBrick(brick, 1);
                        }
                    }
                } while (changed);
            }
        }

        public void Part1()
        {
            string[] lines = File.ReadAllLines("input22.txt");

            List<Brick> bricks = new List<Brick>();

            int count = 0;
            foreach (string line in lines)
            {
                string[] ends = line.Split('~');
                string[] p1 = ends[0].Split(',');
                string[] p2 = ends[1].Split(',');
                bricks.Add(new Brick(p1, p2, count++));
            }

            int max = 0;
            foreach (Brick brick in bricks)
            {
                max = Math.Max(max, brick.p1.x);
                max = Math.Max(max, brick.p1.y);
                max = Math.Max(max, brick.p1.z);
                max = Math.Max(max, brick.p2.x);
                max = Math.Max(max, brick.p2.y);
                max = Math.Max(max, brick.p2.z);
            }
            max++;

            Cube cube = new Cube(max, bricks);
            cube.SettleBricks();

            int canRemove = 0;
            for (int i = 0; i < bricks.Count; i++) {
                Cube c2 = new Cube(cube);
                c2.ClearBrick(c2.bricks[i]);
                Cube c3 = new Cube(c2);
                c2.SettleBricks();
                if (c3 == c2)
                {
                    Console.WriteLine("Can remove brick " + i);
                    canRemove++;
                }
            }

            Console.WriteLine($"Can remove: {canRemove}");
        }

        public void Part2()
        {
            string[] lines = File.ReadAllLines("input22.txt");

            List<Brick> bricks = new List<Brick>();

            int count = 0;
            foreach (string line in lines)
            {
                string[] ends = line.Split('~');
                string[] p1 = ends[0].Split(',');
                string[] p2 = ends[1].Split(',');
                bricks.Add(new Brick(p1, p2, count++));
            }

            int max = 0;
            foreach (Brick brick in bricks)
            {
                max = Math.Max(max, brick.p1.x);
                max = Math.Max(max, brick.p1.y);
                max = Math.Max(max, brick.p1.z);
                max = Math.Max(max, brick.p2.x);
                max = Math.Max(max, brick.p2.y);
                max = Math.Max(max, brick.p2.z);
            }
            max++;

            Cube cube = new Cube(max, bricks);
            cube.SettleBricks();

            List<int> willFall = new List<int>();
            int totalFall = 0;
            for (int i = 0; i < bricks.Count; i++)
            {
                Cube c2 = new Cube(cube);
                c2.ClearBrick(c2.bricks[i]);

                willFall.Clear();
                c2.SettleBricks(willFall);
                totalFall += willFall.Count;
            }

            Console.WriteLine($"totalFall: {totalFall}");
        }
    }
}
