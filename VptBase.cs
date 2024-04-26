// Status: Uncompleted
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace Project1
{
    public class VptBase
    {
        public V2 axis, normal, res;
        // public SolidBrush brush;
        public Color c;
        private float dif, dis;
        public float grndFrict = 0.5f;
        public int Id;
        public bool IsPinned;
        public float mass;
        public V2 old, vel, g;
        public VptBase p1, p2;
        public float r, d, mag, frict = 0.97f;

        public V2 Pos { get; set; }
        public int Level { get; set; }

        public VptBase(float x, float y, int id, bool isPinned, Color color, float radius, int level)
        {
            Pos = new V2(x, y);
            old = new V2(x, y);
            g = new V2(0, 1);
            //brush = new SolidBrush(color);
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

        public void Update(Size space)
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

        public void Constraints(Size space)
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

        // TODO: Fix Graphics and FillEllipse
        /*
        public void Render(Graphics g, Size space, List<VptBase> pts)
        {
            Update(space);
            DetectCollision(pts);
            Constraints(space);
            g.FillEllipse(brush, Pos.X - r, Pos.Y - r, d, d);
        }
        */
    }
}
