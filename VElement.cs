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
using System.Xml.Linq;

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
        public List<WindInfluencer> Influencers { get; set; }

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
            Influencers = new List<WindInfluencer>();
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
            this.clam.openMouth = false;
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

        public void AddInfluencer(WindInfluencer influencer)
        {
            Influencers.Add(influencer);
        }

        public void Update(Rectangle space)
        {
            // Update points
            for (p = 0; p < pts.Count; p++)
                pts[p].Update2(space, pts);

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
            
            //Update influencers
            foreach (var pearl in cndPts)
            {
                foreach (var influencer in Influencers)
                {
                    V2 force = influencer.GetForce(pearl);
                    pearl.ApplyForce(force);
                }
                
                // Update particle state
                pearl.Update(space);
            }
        }

        public void Render(SpriteBatch _spriteBatch, Rectangle pantallaRect, int currentLevel, Texture2D pearlTexture, Texture2D starTexture, Texture2D clamTexture, Texture2D startPointTexture, Texture2D clamClosedTexture, Texture2D circle, Camera cameraMono)
        {
            // Render ropes

            for (int p = 0; p < rps.Count; p++)
            {
                if (rps[p].Level == currentLevel)
                    rps[p].Render(_spriteBatch, pantallaRect, cameraMono);
            }

            // Render points
            for (int p = 0; p < pts.Count; p++)
            {
                if (pts[p].Level == currentLevel && !(pts[p] is CandyVpt))
                    _spriteBatch.Draw(startPointTexture, new Rectangle((int)pts[p].Pos.X - 10, (int)(pts[p].Pos.Y - 10 - cameraMono.position.Y), 20, 20), Color.White);
            }

            // Render pearl
            for (int p = 0; p < cndPts.Count; p++)
            {
                if (cndPts[p].Level == currentLevel)
                    _spriteBatch.Draw(pearlTexture, new Rectangle((int)cndPts[p].Pos.X - 20, (int)(cndPts[p].Pos.Y - 20 - cameraMono.position.Y), 40, 40), Color.White);
            }

            // Render pinnedPoints
            for (int p = 0; p < pndPts.Count; p++)
            {
                if (pndPts[p].Level == currentLevel)
                {
                    _spriteBatch.Draw(startPointTexture, new Rectangle((int)pndPts[p].Pos.X - 10, (int)(pndPts[p].Pos.Y - 10 - cameraMono.position.Y), 20, 20), Color.White);
                    _spriteBatch.Draw(circle, new Rectangle((int)pndPts[p].Pos.X - 70, (int)(pndPts[p].Pos.Y - 70 - cameraMono.position.Y), 140, 140), Color.White);
                    pndPts[p].RenderRadius(_spriteBatch);
                }
            }

            // Render stars
            for (int i = 0; i < strs.Count; i++)
            {
                if (strs[i].Level == currentLevel)
                    _spriteBatch.Draw(starTexture, new Rectangle((int)(strs[i].Position.X - 15), (int)(strs[i].Position.Y - 15 - cameraMono.position.Y), 30, 30), Color.White);
            }
            

            // Render clam
            if (clamState == 1)
            {
                if (this.clam.openMouth)
                    _spriteBatch.Draw(clamTexture, new Rectangle((int)(clam.Position.X - 35), (int)(clam.Position.Y - 30 - cameraMono.position.Y), 70, 70), Color.White);
                else
                    _spriteBatch.Draw(clamClosedTexture, new Rectangle((int)(clam.Position.X - 35), (int)(clam.Position.Y - 30 - cameraMono.position.Y), 70, 40), Color.White);
            }
            
            //Render influencers
            for (int i = 0; i < Influencers.Count; i++)
            {
                _spriteBatch.Draw(starTexture, new Rectangle((int)(Influencers[i].Position.X-35), (int)(Influencers[i].Position.Y - 35 ), 70, 70), Color.White);
            }

        }
    }
}
