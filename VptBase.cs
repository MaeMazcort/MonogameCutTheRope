﻿// Status: Completed. Check the logic for FillEllipse
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Project1
{
    public class VptBase
    {
        public V2 axis, normal, res;
        public Color c;
        private float dif, dis;
        public float grndFrict = 0.5f;
        public int Id;
        public bool IsPinned;
        public float mass;
        public V2 old, vel, g;
        public VptBase p1, p2;
        public float r, d, mag, frict = 1f;

        public V2 Pos { get; set; }
        public int Level { get; set; }

        public VptBase(float x, float y, int id, bool isPinned, Color color, float radius, int level)
        {
            Pos = new V2(x, y);
            old = new V2(x, y);
            g = new V2(0, 1);
            Id = id;
            IsPinned = isPinned;
            r = radius;
            d = r * 2;
            Level = level;

            if (IsPinned)
            {
                mass = 1;
                d = r + r;
            }
            else
            {
                mass = 1;
                d = r + r;
            }
        }

        public void Update(Rectangle space)
        {
            if (!IsPinned)
            {
                vel = (Pos - old) * frict / 1f;

                if (Pos.Y >= space.Height - r && vel.MagSqr() > 0.000001)
                {
                    mag = vel.Length();
                    vel /= mag;
                    vel *= mag * grndFrict;
                }

                old = Pos;
                Pos += vel + g;
            }
        }

        public void Constraints(Rectangle space)
        {
            if (Pos.X > space.Width - r) Pos.X = space.Width - r;

            if (Pos.X < r) Pos.X = r;

            if (Pos.Y > space.Height - r) Pos.Y = space.Height - r;

            if (Pos.Y < r) Pos.Y = r;
        }

        public void DetectCollision(List<VptBase> pts)
        {
            var s = Id;
            for (var p = Id; p < pts.Count; p++)
            {
                p1 = pts[s];
                p2 = pts[p];
                if (p1.Id == p2.Id) // BY ID
                    continue;
                if (p1.IsPinned && p2.IsPinned)
                    continue;
                if (p1.IsPinned && !p2.IsPinned)
                    continue;
                if (!p1.IsPinned && p2.IsPinned)
                    continue;
                axis = p1.Pos - p2.Pos; // vector de direccion
                dis = axis.Length(); // magnitud
                if (dis < p1.r + p2.r) //COLLISION DETECTED
                {
                    // dividir la fuerza para repartir entre ambas colisiones
                    dif = (dis - (p1.r + p2.r)) * .5f;
                    normal = axis / dis; // normalizar la direccion para tener el vector unitario
                    res = dif * normal; // vector

                    if (!p1.IsPinned)
                        if (p2.IsPinned)
                            p1.Pos -= res * 1.5f;
                        else
                            p1.Pos -= res;
                    if (!p2.IsPinned)
                        if (p1.IsPinned)
                            p2.Pos += res * 1.5f;
                        else
                            p2.Pos += res;
                }
            }
        }
        public void Update2(Rectangle space, List<VptBase> pts) {
            Update(space);
            DetectCollision(pts);
            Constraints(space);
        }
        public void Render(SpriteBatch spriteBatch, Camera camera)
        {
            Rectangle destRect = new Rectangle((int)(Pos.X - r), (int)(Pos.Y - r - camera.position.Y), (int)d, (int)d);
            spriteBatch.FillEllipse(Color.White, destRect);
        }
    }
}
