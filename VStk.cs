// Status: Pending changes for the DrawLine
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public class VStk
    {
        public VptBase a, b;
        public Color color;
        public V2 dxy, offset;
        public float stiff, damp, length, tot, m1, m2, dis, diff;

        public Vstk(VptBase a, VptBase b)
        {
            this.a = a;
            this.b = b;
            stiff = 20f;
            damp = 0.05f;
            length = Vector2.Distance(a.Pos.ToVector2(), b.Pos.ToVector2());
            color = Color.SaddleBrown;
            tot = a.mass + b.mass;
            m1 = b.mass / tot;
            m2 = a.mass / tot;
        }


        public void Update()
        {
            dxy = b.Pos - a.Pos;
            dis = dxy.Length();
            diff = stiff * (length - dis) / dis;
            offset = dxy * diff * damp;
            if (!a.IsPinned)
            {
                a.Pos -= offset * m1;
            }

            if (!b.IsPinned)
            {
                b.Pos += offset * m2;
            }
        }

        public void Render(SpriteBatch spriteBatch, Rectangle space)
        {
            Update();
            Vector2 startPoint = new Vector2(a.Pos.X, a.Pos.Y);
            Vector2 endPoint = new Vector2(b.Pos.X, b.Pos.Y);
            spriteBatch.DrawLine(startPoint, endPoint, color);
        }

        public Vector2 GetMidpoint()
        {
            float midX = (a.Pos.X + b.Pos.X) / 2;
            float midY = (a.Pos.Y + b.Pos.Y) / 2;
            return new Vector2(midX, midY);
        }

    }
}
