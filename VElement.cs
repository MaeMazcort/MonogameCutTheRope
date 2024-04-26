// Status: Uncompleted. Check Render function
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Project1
{
    public class VElement
    {
        private int p, l;
        public List<VptBase> pts;
        public List<StartVpt> strtPts;
        public List<CandyVpt> cndPts;
        public List<PinnedVpt> pndPts;
        public List<VRope> rps;
        public List<Vstk> stks;
        public List<Star> strs;

        public Map map { get; set; }

        public VElement()
        {
            pts = new List<VptBase>();
            stks = new List<Vstk>();
            strtPts = new List<StartVpt>();
            cndPts = new List<CandyVpt>();
            pndPts = new List<PinnedVpt>();
            rps = new List<VRope>();
            stks = new List<Vstk>();
            strs = new List<Star>();
        }

        public List<VptBase> Pts => pts;
        public List<StartVpt> startVpts => strtPts;
        public List<CandyVpt> CandyVpts => cndPts;
        public List<PinnedVpt> PinnedVpts => pndPts;
        public List<Star> Stars => strs;
        public List<VRope> Rps => rps;

        public Clam clam { get; set; }
        public Texture2D Texture { get; set; } // Unused?
        public int clamState;

        public void SetClam(PointF position, Texture2D texture)
        {
            clam = new Clam(position.X, position.Y, texture);
            clamState = 1;

        }

        public void SetMap(Map map)
        {
            this.map = map;
        }

        public void DeleteClam()
        {
            clamState = 0;
        }
        public void AddPoint(VptBase vpt)
        {
            pts.Add(vpt);
        }

        public void AddStartPoint(StartVpt vpt)
        {
            strtPts.Add(vpt);
        }
        public void AddCandyPoint(CandyVpt vpt)
        {
            cndPts.Add(vpt);
        }
        public void AddPinnedPoint(PinnedVpt vpt)
        {
            pndPts.Add(vpt);
        }

        public void AddRope(VRope rp)
        {
            rps.Add(rp);
        }

        public void AddStar(Star str)
        {
            strs.Add(str);
        }


        public void Render(Graphics g, Size space, int currentLevel)
        {
            for (p = 0; p < pts.Count; p++)
                pts[p].Update(space);

            for (l = 0; l < 5; l++)
            {
                for (p = 0; p < stks.Count; p++)
                    stks[p].Update();

                for (p = 0; p < pts.Count; p++)
                    pts[p].DetectCollision(pts);

                for (p = 0; p < pts.Count; p++)
                    pts[p].Constraints(space);
            }

            for (p = 0; p < pts.Count; p++)
            {
                if (pts[p].Level == currentLevel && !(pts[p] is CandyVpt))
                {
                    pts[p].Render(g, space, pts);
                }
            }

            for (p = 0; p < cndPts.Count; p++)
            {
                if (cndPts[p].Level == currentLevel)
                {
                    g.DrawImage(Resources.perla, cndPts[p].Pos.X - 10, cndPts[p].Pos.Y - 10, map.nTileWidth, map.nTileHeight);
                }
            }


            for (p = 0; p < rps.Count; p++)
            {
                if (rps[p].Level == currentLevel)
                {
                    rps[p].Render(g, space);
                }
            }

            for (p = 0; p < pndPts.Count; p++)
            {
                if (pndPts[p].Level == currentLevel)
                {
                    pndPts[p].RenderRadius(g);

                }
            }


            for (int i = 0; i < strs.Count; i++)
            {
                if (strs[i].Level == currentLevel)
                {
                    g.DrawImage(Resources.estrella, strs[i].Position.X - 10, strs[i].Position.Y - 10, map.nTileWidth, map.nTileHeight);
                }
            }

            if (clamState == 1)
            {
                g.DrawImage(Resources.almeja1, clam.Position.X - 20, clam.Position.Y - 30, map.nTileWidth * 3, map.nTileHeight * 3);
            }
        }
    }
}
