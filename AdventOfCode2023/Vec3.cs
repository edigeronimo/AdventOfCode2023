using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    struct Vec3
    {
        public int x, y, z;

        public Vec3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vec3(Vec3 v)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
        }

        public static bool operator ==(Vec3 a, Vec3 b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        public static bool operator !=(Vec3 a, Vec3 b)
        {
            return !(a == b);
        }

        public bool Equals(Vec3 v)
        {
            return this == v;
        }

        public override bool Equals(object? obj)
        {
            return obj != null && obj is Vec3 && Equals((Vec3)obj);
        }

        public override int GetHashCode()
        {
            return ((x * 1000) + y * 1000)  + z;
        }
    }
}
