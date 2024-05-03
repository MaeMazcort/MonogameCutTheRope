using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Project1
{
    public static class SpriteBatchExtensions
    {
        private static Texture2D pixel;

        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            // Create a 1x1 white pixel texture
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.White });
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, int width = 1)
        {
            Vector2 edge = end - start;
            float angle = (float)Math.Atan2(edge.Y, edge.X);

            spriteBatch.Draw(pixel, start, null, color, angle, Vector2.Zero, new Vector2(edge.Length(), width), SpriteEffects.None, 0);
        }

        public static void FillEllipse(this SpriteBatch spriteBatch, Color color, Rectangle destRect)
        {
            spriteBatch.Draw(pixel, destRect, color);
        }
    }
}
