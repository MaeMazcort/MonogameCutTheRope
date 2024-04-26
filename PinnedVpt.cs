// Status: Completed. Check the Render function
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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


        public void RenderRadius(SpriteBatch spriteBatch, Texture2D pixelTexture)
        {
            // Only draw the radius if Available = true
            if (Available)
            {
                Color color = Color.CornflowerBlue * 0.5f; // Adjust alpha to control transparency

                // Calculate the top-left corner of the bounding rectangle for the circle
                Vector2 topLeft = new Vector2(Pos.X - Radius, Pos.Y - Radius);
                Rectangle boundingRect = new Rectangle((int)topLeft.X, (int)topLeft.Y, Radius * 2, Radius * 2);

                // Draw the dashed circle using a custom pixel texture
                spriteBatch.Draw(pixelTexture, boundingRect, null, color, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            }
        }
    }
}
