using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    struct Vec3L
    {
        public long x, y, z;

        public Vec3L(long x, long y, long z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vec3L(Vec3L v)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
        }

        public static bool operator ==(Vec3L a, Vec3L b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        public static bool operator !=(Vec3L a, Vec3L b)
        {
            return !(a == b);
        }

        public bool Equals(Vec3L v)
        {
            return this == v;
        }

        public override bool Equals(object? obj)
        {
            return obj != null && obj is Vec3L && Equals((Vec3L)obj);
        }

        public override int GetHashCode()
        {
            return (int)(((x * 1000) + y * 1000)  + z);
        }

        public static Vec3L Parse(string[] s)
        {
            return new Vec3L(long.Parse(s[0]), long.Parse(s[1]), long.Parse(s[2]));
        }

        public static Vec3L operator+(Vec3L a, Vec3L b)
        {
            return new Vec3L(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vec3L operator-(Vec3L a, Vec3L b)
        {
            return new Vec3L(a.x - b.x, a.y - b.y, a.z - b.z);
        }
    }
}
