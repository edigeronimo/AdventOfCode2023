using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    struct Vec2 : IEquatable<Vec2>
    {
        public int x, y;

        public Vec2() { x = y = 0; }
        public Vec2(int x, int y) { this.x = x; this.y = y; }

        public static bool operator ==(Vec2 a, Vec2 b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Vec2 a, Vec2 b)
        {
            return !(a == b);
        }

        public bool Equals(Vec2 v)
        {
            return this == v;
        }

        public override bool Equals(object? obj)
        {
            return obj != null && obj is Vec2 && Equals((Vec2)obj);
        }

        public override int GetHashCode()
        {
            return x << 16 | y;
        }
    }
}
