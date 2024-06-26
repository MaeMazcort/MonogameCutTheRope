﻿// Status: Completed. Check Render function and the cirle
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Project1
{
    public class VElement
    {
        private int p;
        public List<VptBase> pts;
        public List<StartVpt> strtPts;
        public List<CandyVpt> cndPts;
        public List<PinnedVpt> pndPts;
        public List<VRope> rps;
        public List<VStk> stks;
        public List<Star> strs;

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

        public List<VRope> Rps => rps;

        public Clam clam { get; set; }
        public int clamState;

        public void SetClam(Clam clam)
        {
            this.clam = clam;
            clamState = 1;
            this.clam.openMouth = false;
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

        public void Update(Rectangle space, GameTime gameTime)
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
                rps[p].Update(space);
            }
            
            //Update influencers
            foreach (var pearl in cndPts)
            {
                foreach (var influencer in Influencers)
                {
                    V2 windForce = influencer.GetForce(pearl);
                    if (influencer.distance <= 200f)
                    {
                        pearl.ApplyForce(windForce);
                        pearl.Update(gameTime);
                    }
                }
            }
        }

        public void Render(SpriteBatch _spriteBatch, Rectangle pantallaRect, int currentLevel, Texture2D pearlTexture,
            Texture2D starTexture, Texture2D clamTexture, Texture2D startPointTexture, Texture2D clamClosedTexture,
            Texture2D circle, Texture2D blowerleft, Texture2D blowerright, Camera cameraMono)
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
                    _spriteBatch.Draw(startPointTexture,
                        new Rectangle((int)pts[p].Pos.X - 10, (int)(pts[p].Pos.Y - 10 - cameraMono.position.Y), 20, 20),
                        Color.White);
            }

            // Render pearl
            for (int p = 0; p < cndPts.Count; p++)
            {
                if (cndPts[p].Level == currentLevel)
                    _spriteBatch.Draw(pearlTexture,
                        new Rectangle((int)cndPts[p].Pos.X - 20, (int)(cndPts[p].Pos.Y - 20 - cameraMono.position.Y),
                            40, 40), Color.White);
            }

            // Render pinnedPoints
            for (int p = 0; p < pndPts.Count; p++)
            {
                if (pndPts[p].Level == currentLevel)
                {
                    _spriteBatch.Draw(startPointTexture,
                        new Rectangle((int)pndPts[p].Pos.X - 10, (int)(pndPts[p].Pos.Y - 10 - cameraMono.position.Y),
                            20, 20), Color.White);
                    _spriteBatch.Draw(circle,
                        new Rectangle((int)pndPts[p].Pos.X - 100, (int)(pndPts[p].Pos.Y - 100 - cameraMono.position.Y),
                            200, 200), Color.White);
                }
            }

            // Render stars
            for (int i = 0; i < strs.Count; i++)
            {
                if (strs[i].Level == currentLevel)
                    _spriteBatch.Draw(starTexture,
                        new Rectangle((int)(strs[i].Position.X - 15),
                            (int)(strs[i].Position.Y - 15 - cameraMono.position.Y), 30, 30), Color.White);
            }


            // Render clam
            if (clamState == 1)
            {

                if (clam.openMouth)
                    _spriteBatch.Draw(clamTexture, new Rectangle((int)(clam.Position.X - 35), (int)(clam.Position.Y - 30 - cameraMono.position.Y), 70, 70), Color.White);
                else
                    _spriteBatch.Draw(clamClosedTexture,
                        new Rectangle((int)(clam.Position.X - 35), (int)(clam.Position.Y - 30 - cameraMono.position.Y),
                            70, 40), Color.White);
            }

            //Render influencers
            for (int i = 0; i < Influencers.Count; i++)
            {
                if (Influencers[i].Direction.X == 1)
                {
                    _spriteBatch.Draw(blowerright, new Rectangle((int)(Influencers[i].Position.X - 40), (int)(Influencers[i].Position.Y - 40 - cameraMono.position.Y), 80, 80), Color.White);
                }
                else
                {
                    _spriteBatch.Draw(blowerleft, new Rectangle((int)(Influencers[i].Position.X - 40), (int)(Influencers[i].Position.Y - 40 - cameraMono.position.Y), 80, 80), Color.White);

                }
            }
            
        }
    }
    
}
