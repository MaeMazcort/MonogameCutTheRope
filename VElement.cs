// Status: Completed. Check Render function and the cirle
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework.Content;

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
        public List<VStk> stks;
        public List<Star> strs;

        public Map map { get; set; }

        public VElement()
        {
            pts = new List<VptBase>();
            stks = new List<VStk>();
            strtPts = new List<StartVpt>();
            cndPts = new List<CandyVpt>();
            pndPts = new List<PinnedVpt>();
            rps = new List<VRope>();
            stks = new List<VStk>();
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

        public void SetClam(Clam clam)
        {
            this.clam = clam;
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

        public void Update(Rectangle space)
        {
            // Update points
            for (p = 0; p < pts.Count; p++)
                pts[p].Update2(space, pts);

            // Perform iterations
            for (l = 0; l < 5; l++)
            {
                for (p = 0; p < stks.Count; p++)
                    stks[p].Update();

                for (p = 0; p < pts.Count; p++)
                    pts[p].DetectCollision(pts);

                for (p = 0; p < pts.Count; p++)
                    pts[p].Constraints(space);
            }
            //Update points
            for (p = 0; p < pts.Count; p++)
            {
                if (pts[p].Level == 1 && !(pts[p] is CandyVpt))
                {
                    pts[p].Update2(space, pts);
                }
            }
            // Update ropes
            for (p = 0; p < rps.Count; p++)
            {
                if (rps[p].Level == 1)
                {
                    rps[p].Update(space);
                }
            }
        }



        public void Render(SpriteBatch spriteBatch, Rectangle space, int currentLevel, Texture2D perlaTexture, Texture2D estrellaTexture, Texture2D almejaTexture)
        {
            // Render points
            for (p = 0; p < pts.Count; p++)
            {
                if (pts[p].Level == currentLevel && !(pts[p] is CandyVpt))
                {
                    pts[p].Render(spriteBatch);
                }
            }

            // Render pearls
            for (p = 0; p < cndPts.Count; p++)
            {
                if (cndPts[p].Level == currentLevel)
                {
                    spriteBatch.Draw(perlaTexture, new Vector2(cndPts[p].Pos.X - 10, cndPts[p].Pos.Y - 10), Color.White);
                }
            }

            // Render ropes
            for (p = 0; p < rps.Count; p++)
            {
                if (rps[p].Level == currentLevel)
                {
                    rps[p].Render(spriteBatch, space);
                }
            }

            // Render pinnedPoints
            for (p = 0; p < pndPts.Count; p++)
            {
                if (pndPts[p].Level == currentLevel)
                {
                    pndPts[p].RenderRadius(spriteBatch);

                }
            }

            // Render stars
            for (int i = 0; i < strs.Count; i++)
            {
                if (strs[i].Level == currentLevel)
                {
                    spriteBatch.Draw(estrellaTexture, new Rectangle((int)(strs[i].Position.X - 10), (int)(strs[i].Position.Y - 10), 40, 40), Color.White);
                }
            }

            // Render clam
            if (clamState == 1)
            {
                spriteBatch.Draw(almejaTexture, new Rectangle((int)(clam.Position.X - 20), (int)(clam.Position.Y - 30),40,40), Color.White);
            }
        }
    }
}
