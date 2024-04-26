// Status: Completed
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Project1
{
    public class Clam
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }


        public Clam(float x, float y, Texture2D texture)
        {
            Position = new Vector2(x, y);
            Texture = texture;
        }

        // Not used
        /*
        public void DrawClam(Graphics g)
        {
            // Drawing a simple star, you can replace this with a more complex shape or an image
            g.FillEllipse(Brushes.Ivory, Position.X - 10, Position.Y - 10, 20, 20);
        }
        */

        public bool AteCandy(CandyVpt candy)
        {
            float distance = Distance(Position, new Vector2(candy.Pos.X, candy.Pos.Y));
            if (distance < 25)
            {
                return true;
            }

            return false;
        }

        private float Distance(Vector2 point1, Vector2 point2)
        {
            return (float)Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }
    }
}
