// Status: Completed. Check Texture instead of Image
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Project1
{
    public class CandyVpt : VptBase, IParticle
    {
        public Texture2D Texture { get; set; }

        public CandyVpt(float x, float y, int id, int level, Texture2D texture) : base(x, y, id, false, Color.CornflowerBlue, 12f, level)
        {
            Pos = new V2(x, y);
            Level = level;
            Texture = texture;
        }


        public void Update(GameTime gameTime)
        {
            
            Pos += vel * (float)gameTime.ElapsedGameTime.TotalSeconds;

            
        }

        public void ApplyForce(V2 force)
        {
            if (!IsPinned)
            {
                // Apply force adjusted by mass (F = ma, so a = F/m)
                V2 acceleration = force / mass;
                vel += acceleration;
            }
        }
    }
}