// Status: Completed
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;

namespace Project1
{
    public class Clam
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public bool openMouth = false;


        public Clam(float x, float y, Texture2D texture)
        {
            Position = new Vector2(x, y);
            Texture = texture;
        }

        public bool AteCandy(CandyVpt candy)
        {
            float distance = Distance(Position, new Vector2(candy.Pos.X, candy.Pos.Y));
            if (distance < 25)
            {
                return true;
            }

            if (distance < 200)
            {
                openMouth = true;
            }

            return false;
        }

        private float Distance(Vector2 point1, Vector2 point2)
        {
            return (float)Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }
    }
}
