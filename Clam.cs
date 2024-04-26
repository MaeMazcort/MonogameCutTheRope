// Status: Completed
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Project1
{
    public class Clam
    {
        public PointF Position { get; set; }
        public Texture2D Texture { get; set; }


        public Clam(float x, float y, Texture2D texture)
        {
            Position = new PointF(x, y);
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
            float distance = Distance(Position, new PointF(candy.Pos.X, candy.Pos.Y));
            if (distance < 25)
            {
                return true;
            }

            return false;
        }

        private float Distance(PointF point1, PointF point2)
        {
            return (float)Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }
    }
}
