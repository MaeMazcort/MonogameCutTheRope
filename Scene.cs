// Status: Completed. Added parameters to the Render funtion
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public class Scene
    {
        public List<VElement> Elements { get; set; }

        public Scene()
        {
            Elements = new List<VElement>();
        }

        public void AddElement(VElement element)
        {
            Elements.Add(element);
        }

        public void Render(SpriteBatch spriteBatch, Rectangle space, int currentLevel, Texture2D perlaTexture, Texture2D estrellaTexture, Texture2D almejaTexture)
        {
            for (int i = 0; i < Elements.Count; i++)
            {
                Elements[i].Render(spriteBatch, space, currentLevel, perlaTexture, estrellaTexture, almejaTexture);
            }
        }

    }
}
