// Status: Completed. Check the Render function
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Project1
{
    public class PinnedVpt : VptBase
    {
        public PinnedVpt(float x, float y, int id, int radius, bool available, int level) : base(x, y, id, true, Color.CornflowerBlue, 10f, level)
        {
            Radius = radius;
            Available = available;
            Level = level;
        }

        public int Radius { get; set; }
        public bool Available { get; set; }


        public void RenderRadius(SpriteBatch spriteBatch)
        {
            // Only draw the radius if Available = true
            if (Available)
            {
                Color color = Color.CornflowerBlue; // Adjust alpha to control transparency
                int segments = 20; // Number of segments for the dashed line
                float segmentAngle = MathHelper.TwoPi / segments;

                for (int i = 0; i < segments; i += 2)
                {
                    // Calculate the angle for this segment
                    float startAngle = i * segmentAngle;
                    float endAngle = (i + 1) * segmentAngle;

                    // Calculate the start and end points for this segment
                    Vector2 startPoint = new Vector2(Pos.X + Radius * (float)Math.Cos(startAngle), Pos.Y + Radius * (float)Math.Sin(startAngle));
                    Vector2 endPoint = new Vector2(Pos.X + Radius * (float)Math.Cos(endAngle), Pos.Y + Radius * (float)Math.Sin(endAngle));

                    // Calculate the bounding rectangle for this segment
                    Rectangle boundingRect = new Rectangle((int)Math.Min(startPoint.X, endPoint.X), (int)Math.Min(startPoint.Y, endPoint.Y), (int)Math.Abs(startPoint.X - endPoint.X), (int)Math.Abs(startPoint.Y - endPoint.Y));

                    // Draw the ellipse
                    spriteBatch.FillEllipse(color, boundingRect);
                }
            }
        }


    }
}
