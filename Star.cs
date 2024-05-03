// Status: Completed
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Drawing;

namespace Project1
{
    public class Star
    {
        public PointF Position { get; set; }
        public int Points { get; set; }
        public bool IsCollected { get; set; }
        public int Level { get; set; }

        public Texture2D Texture { get; set; }

        public Star(float x, float y, int level, Texture2D texture)
        {
            Position = new PointF(x, y);
            Points = 10;
            IsCollected = false;
            Level = level;
            Texture = texture;
        }

        public void CheckCollision(CandyVpt candy)
        {
            float distance = Distance(Position, new PointF(candy.Pos.X, candy.Pos.Y));
            if (distance < 30)  // Assuming the radius for collision detection is 20
            {
                IsCollected = true;
            }
        }

        private float Distance(PointF point1, PointF point2)
        {
            return (float)Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }
    }
}
