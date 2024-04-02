
using UnityEngine;

namespace Puzzel
{
    public class Puzzle
    {
        public int number;
        public Color color;
        
        public Puzzle(int number, float r, float g, float b)
        {
            this.number = number;
            this.color = new Color(r, g, b);
        }
        public Puzzle(int number, Color color)
        {
            this.number = number;
            this.color = color;
        }
    }
}