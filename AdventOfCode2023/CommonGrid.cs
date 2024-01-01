using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class CommonGrid
    {
        public int width { get; private set; }
        public int height { get; private set; }

        protected List<char> data = new List<char>();

        public CommonGrid()
        {

        }

        public CommonGrid(int width, int height, char value)
        {
            this.width = width;
            this.height = height;

            data = new List<char>(width * height);
            for (int i = 0; i < width * height; i++)
            {
                data.Add(value);
            }
        }

        public CommonGrid(string[] lines)
        {
            foreach (var line in lines)
            {
                data.AddRange(line.ToCharArray());
            }

            width = lines[0].Length;
            height = lines.Length;
        }

        public void AddData(string line)
        {
            if (width == 0)
                width = line.Length;

            data.AddRange(line.ToCharArray());

            height = data.Count / width;
        }

        public char Get(int x, int y)
        {
            return data[y * width + x];
        }

        public void Set(int x, int y, char value)
        {
            data[y * width + x] = value;
        }
    }
}
