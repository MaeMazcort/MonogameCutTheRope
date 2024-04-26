// Status: Uncompleted. Check the whole Draw function
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public class Map
    {
        int divs = 3;
        public float nTileWidth = 14.25f;
        public float nTileHeight = 15.5f;
        public int nLevelWidth, nLevelHeight;
        public float stepX, stepY;
        public int id;
        //public Bitmap bmp;
        public Rectangle s;
        private string sLevel;
        //public Graphics g;
        public int score;
        public int currentLevel;
        public float fOffsetX;
        public float fOffsetY;
        public float nVisibleTilesX;
        public float nVisibleTilesY;


        public Map(Rectangle size)
        {
            score = 0;
            s = size;

            sLevel = "";
            sLevel += "..........1..........";
            sLevel += "..........A..........";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += "..........S..........";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += "..........M..........";
            sLevel += "#####################";

            sLevel += ".....................";
            sLevel += "..............2....2.";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += "..................B..";
            sLevel += ".....................";
            sLevel += "..........K..........";
            sLevel += ".....................";
            sLevel += "...................T.";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += "...................M.";
            sLevel += ".....................";
            sLevel += "#####################";

            sLevel += ".....................";
            sLevel += "..3................3.";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += ".........C...........";
            sLevel += ".....................";
            sLevel += ".....U...............";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += "..M..................";
            sLevel += "#####################";

            nLevelWidth = 21;
            nLevelHeight = 42;

            bmp = new Bitmap(s.Width / divs, s.Height / divs);

            g = Graphics.FromImage(bmp);
            g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
            g.SmoothingMode = SmoothingMode.HighSpeed;
        }

        public void Draw(Vector2 cameraPos, Scene scene, Texture2D perlaTexture, Texture2D estrellaTexture, Texture2D almejaTexture)
        {
            // Draw Level based on the visible tiles on our picturebox (canvas)
            nVisibleTilesX = bmp.Width / nTileWidth;
            nVisibleTilesY = bmp.Height / nTileHeight;

            // Calculate Top-Leftmost visible tile
            fOffsetX = cameraPos.X - (float)nVisibleTilesX / 2.0f;
            fOffsetY = cameraPos.Y - (float)nVisibleTilesY / 2.0f;

            // Clamp camera to game boundaries
            if (fOffsetX < 0) fOffsetX = 0;
            if (fOffsetY < 0) fOffsetY = 0;
            if (fOffsetX > nLevelWidth - nVisibleTilesX) fOffsetX = nLevelWidth - nVisibleTilesX;
            if (fOffsetY > nLevelHeight - nVisibleTilesY) fOffsetY = nLevelHeight - nVisibleTilesY;

            float fTileOffsetX = (fOffsetX - (int)fOffsetX) * nTileWidth;
            float fTileOffsetY = (fOffsetY - (int)fOffsetY) * nTileHeight;

            //Draw visible tile map//*--------------------DRAW------------------------------
            char c;
            for (int x = -1; x < nVisibleTilesX + 2; x++)
            {
                for (int y = -1; y < nVisibleTilesY + 2; y++)
                {
                    c = GetTile(x + (int)fOffsetX, y + (int)fOffsetY);
                    stepX = x * nTileWidth - fTileOffsetX;
                    stepY = y * nTileHeight - fTileOffsetY;


                    if (currentLevel == 1)
                    {
                        switch (c)
                        {
                            case 'M': //Clam
                                SetTile(x + (int)fOffsetX, y + (int)fOffsetY, '.');
                                var clam = new Clam(stepX, stepY, almejaTexture);
                                scene.Elements[0].SetClam(new Vector2(stepX, stepY), almejaTexture);
                                break;
                            case '1': //Start Vpt from Level 1
                                SetTile(x + (int)fOffsetX, y + (int)fOffsetY, '.');
                                var startVpt1 = new StartVpt(stepX, stepY, id, level: 1);
                                scene.Elements[0].AddPoint(startVpt1);
                                scene.Elements[0].AddStartPoint(startVpt1);
                                id++;
                                break;
                            case 'S': //Star Level 1
                                SetTile(x + (int)fOffsetX, y + (int)fOffsetY, '.');
                                var star1 = new Star(stepX, stepY, level: 1, estrellaTexture);
                                scene.Elements[0].AddStar(star1);
                                //star1.DrawStar(canvas.g);
                                break;
                            case 'O': //Pinned Vpt Level 1
                                SetTile(x + (int)fOffsetX, y + (int)fOffsetY, '.');
                                var pinnedVpt1 = new PinnedVpt(stepX, stepY, id, 30, true, level: 1);
                                scene.Elements[0].AddPoint(pinnedVpt1);
                                scene.Elements[0].AddPinnedPoint(pinnedVpt1);
                                id++;
                                break;
                            case 'A': //Candy Vpt Level 1
                                SetTile(x + (int)fOffsetX, y + (int)fOffsetY, '.');
                                var candyVpt1 = new CandyVpt(stepX, stepY, id, level: 1, perlaTexture);
                                scene.Elements[0].AddPoint(candyVpt1);
                                scene.Elements[0].AddCandyPoint(candyVpt1);
                                //emitter.Particles.Add(candyVpt1);
                                id++;
                                break;
                                break;
                            case '#':
                                break;
                        }
                    }
                    else if (currentLevel == 2)
                    {
                        switch (c)
                        {
                            case 'M': //Clam
                                SetTile(x + (int)fOffsetX, y + (int)fOffsetY, '.');
                                scene.Elements[0].SetClam(new Vector2(stepX, stepY - 40), almejaTexture);
                                break;
                            case 'P': //Pinned Vpt Level 2
                                SetTile(x + (int)fOffsetX, y + (int)fOffsetY, '.');
                                var pinnedVpt2 = new PinnedVpt(stepX, stepY - 210, id, 30, true, level: 2);
                                scene.Elements[0].AddPoint(pinnedVpt2);
                                scene.Elements[0].AddPinnedPoint(pinnedVpt2);
                                id++;
                                break;
                            case '2': //Start Vpt from Level 2
                                SetTile(x + (int)fOffsetX, y + (int)fOffsetY, '.');
                                var startVpt2 = new StartVpt(stepX, stepY - 210, id, level: 2);
                                scene.Elements[0].AddPoint(startVpt2);
                                scene.Elements[0].AddStartPoint(startVpt2);
                                id++;
                                break;
                            case 'K': //Start Vpt from Level 2
                                SetTile(x + (int)fOffsetX, y + (int)fOffsetY, '.');
                                var startVpt21 = new StartVpt(stepX, stepY - 100, id, level: 2);
                                scene.Elements[0].AddPoint(startVpt21);
                                scene.Elements[0].AddStartPoint(startVpt21);
                                id++;
                                break;
                            case 'T': //Star Level 2
                                SetTile(x + (int)fOffsetX, y + (int)fOffsetY, '.');
                                var star2 = new Star(stepX, stepY - 100, level: 2, estrellaTexture);
                                scene.Elements[0].AddStar(star2);
                                //star2.DrawStar(canvas.g);
                                break;
                            case 'B': //Candy Vpt Level 2
                                SetTile(x + (int)fOffsetX, y + (int)fOffsetY, '.');
                                var candyVpt2 = new CandyVpt(stepX, stepY - 210, id, level: 2, perlaTexture);
                                scene.Elements[0].AddPoint(candyVpt2);
                                scene.Elements[0].AddCandyPoint(candyVpt2);
                                //emitter.Particles.Add(candyVpt2);
                                id++;
                                break;
                            case '#':
                                break;
                        }
                    }
                    else if (currentLevel == 3)
                    {
                        switch (c)
                        {
                            case '3': //Start Vpt from Level 3
                                SetTile(x + (int)fOffsetX, y + (int)fOffsetY, '.');
                                var startVpt3 = new StartVpt(stepX, stepY - 210, id, level: 3);
                                scene.Elements[0].AddPoint(startVpt3);
                                scene.Elements[0].AddStartPoint(startVpt3);
                                id++;
                                break;
                            case 'U': //Star Level 3
                                SetTile(x + (int)fOffsetX, y + (int)fOffsetY, '.');
                                var star3 = new Star(stepX, stepY - 100, level: 3, estrellaTexture);
                                scene.Elements[0].AddStar(star3);
                                //star3.DrawStar(canvas.g);
                                break;
                            case 'M': //Clam
                                SetTile(x + (int)fOffsetX, y + (int)fOffsetY, '.');
                                scene.Elements[0].SetClam(new Vector2(stepX, stepY - 40), almejaTexture);
                                break;
                            case 'Q': //Pinned Vpt Level 3
                                SetTile(x + (int)fOffsetX, y + (int)fOffsetY, '.');
                                var pinnedVpt3 = new PinnedVpt(stepX, stepY - 210, id, 30, true, level: 3);
                                scene.Elements[0].AddPoint(pinnedVpt3);
                                scene.Elements[0].AddPinnedPoint(pinnedVpt3);
                                id++;
                                break;
                            case 'C': //Candy Vpt Level 3
                                SetTile(x + (int)fOffsetX, y + (int)fOffsetY, '.');
                                var candyVpt3 = new CandyVpt(stepX, stepY - 210, id, level: 3, perlaTexture);
                                scene.Elements[0].AddPoint(candyVpt3);
                                scene.Elements[0].AddCandyPoint(candyVpt3);
                                //emitter.Particles.Add(candyVpt3);
                                id++;
                                break;
                            case '#':
                                break;
                        }
                    }
                }
            }

            g.DrawString("SCORE:: " + score, new Font("Consolas", 10, FontStyle.Italic), Brushes.White, 5, 5);
        }

        public void SetTile(float x, float y, char c)//changes the tile
        {
            if (x >= 0 && x < nLevelWidth && y >= 0 && y < nLevelHeight)
            {
                int index = (int)y * nLevelWidth + (int)x;
                sLevel = sLevel.Remove(index, 1).Insert(index, c.ToString());
            }
        }

        public char GetTile(float x, float y)
        {
            if (x >= 0 && x < nLevelWidth && y >= 0 && y < nLevelHeight)
                return sLevel[(int)y * nLevelWidth + (int)x];
            else
                return ' ';
        }
    }
}
