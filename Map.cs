// Status: Completed. Check the missing bmp and g
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Net.NetworkInformation;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using System.Security.Cryptography;
using static System.Formats.Asn1.AsnWriter;

namespace Project1
{
    public class Map
    {
        int divs = 3;
        public float stepX, stepY;
        public int id;
        public int score;
        public float fOffsetX;
        public float fOffsetY;
        public float nVisibleTilesX;
        public float nVisibleTilesY;

        // New implementation
        public int nLevelWidth = 21;
        public int nLevelHeight = 30;
        public int nTileWidth, nTileHeight;
        string sLevel, sLevel1, sLevel2, l;


        public Map()
        {
            score = 0;

            sLevel = ".....................";
            sLevel += "..........1......1...";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += "........I.A..........";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += "......1..............";
            sLevel += ".....................";
            sLevel += "..................S.."; // 10
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += "................S....";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += "................P....";
            sLevel += "..................I..";
            sLevel += ".....................";
            sLevel += "..............P......";
            sLevel += "....................."; // 20
            sLevel += ".....................";
            sLevel += "............P........";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += "..........P..........";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += "..........S..........";
            sLevel += "..........M.........."; // 30

            sLevel1 = ".....................";
            sLevel1 += "..........1..........";
            sLevel1 += ".....................";
            sLevel1 += ".....................";
            sLevel1 += "..........A..........";
            sLevel1 += ".....................";
            sLevel1 += ".....................";
            sLevel1 += ".....................";
            sLevel1 += ".....................";
            sLevel1 += "..................S.."; // 10
            sLevel1 += ".....................";
            sLevel1 += ".....................";
            sLevel1 += "................S....";
            sLevel1 += ".....................";
            sLevel1 += ".....................";
            sLevel1 += "................P....";
            sLevel1 += ".....................";
            sLevel1 += ".....................";
            sLevel1 += "..............P......";
            sLevel1 += "....................."; // 20
            sLevel1 += ".....................";
            sLevel1 += "............P........";
            sLevel1 += ".....................";
            sLevel1 += ".....................";
            sLevel1 += "..........P..........";
            sLevel1 += ".....................";
            sLevel1 += ".....................";
            sLevel1 += ".....................";
            sLevel1 += "..........S..........";
            sLevel1 += "..........M.........."; // 30

            sLevel2 = ".....................";
            sLevel2 += ".................1...";
            sLevel2 += ".....................";
            sLevel2 += ".....................";
            sLevel2 += "..........A..........";
            sLevel2 += ".....................";
            sLevel2 += ".....................";
            sLevel2 += ".....................";
            sLevel2 += ".....................";
            sLevel2 += "..................S.."; // 10
            sLevel2 += ".....................";
            sLevel2 += ".....................";
            sLevel2 += "................S....";
            sLevel2 += ".....................";
            sLevel2 += ".....................";
            sLevel2 += "................P....";
            sLevel2 += ".....................";
            sLevel2 += ".....................";
            sLevel2 += "..............P......";
            sLevel2 += "....................."; // 20
            sLevel2 += ".....................";
            sLevel2 += "............P........";
            sLevel2 += ".....................";
            sLevel2 += ".....................";
            sLevel2 += "..........P..........";
            sLevel2 += ".....................";
            sLevel2 += ".....................";
            sLevel2 += ".....................";
            sLevel2 += "..........S..........";
            sLevel2 += "..........M.........."; // 30
        }

        public void Draw(Rectangle size, ref CandyVpt candy, ref VElement elements, ref Clam clam, Texture2D perlaTexture, Texture2D estrellaTexture, Texture2D almejaTexture, int currentLevel)
        {
            switch (currentLevel)
            {
                case 1:
                    l = sLevel;
                    break;
                case 2:
                    l = sLevel1;
                    break;
                case 3:
                    l = sLevel2;
                    break;
            }

            int nTileWidth = size.Width / nLevelWidth;
            int nTileHeight = size.Height / nLevelHeight;

            for (int y = 0; y < nLevelHeight; y++)
            {
                for (int x = 0; x < nLevelWidth; x++)
                {
                    int index = y * nLevelWidth + x;
                    if (l[index] == 'A')
                    {
                        candy = new CandyVpt(x * nTileWidth, y * nTileHeight, id, currentLevel, perlaTexture);
                        elements.AddPoint(candy);
                        elements.AddCandyPoint(candy);
                        id++;
                    }
                    if (sLevel[index] == 'M')
                    {
                        clam = new Clam(x * nTileWidth, y * nTileHeight, almejaTexture);
                        elements.SetClam(clam);
                    }
                }
            }
            for (int y = 0; y < nLevelHeight; y++)
            {
                for (int x = 0; x < nLevelWidth; x++)
                {
                    int index = y * nLevelWidth + x;
                    switch (l[index])
                    {
                        case '.':
                            break;
                        case 'S':
                            var star1 = new Star(x * nTileWidth, y * nTileHeight, currentLevel, estrellaTexture);
                            elements.AddStar(star1);
                            break;
                        case '1':
                            var startVpt1 = new StartVpt(x * nTileWidth, y * nTileHeight, id, currentLevel);
                            elements.AddPoint(startVpt1);
                            elements.AddStartPoint(startVpt1);
                            id++;
                            break;
                        case 'P':
                            var pinnedpoint = new PinnedVpt(x * nTileWidth, y * nTileHeight, id, 140, true, currentLevel);
                            elements.AddPoint(pinnedpoint);
                            elements.AddPinnedPoint(pinnedpoint);
                            id++;
                            break;
                        case 'I': //Influencer
                            var influencerPosition = new V2(x * nTileWidth, y * nTileHeight);
                            float strength = 10;  
                            float velocity = 5;  
                            var windInfluencer = new WindInfluencer(influencerPosition, strength, velocity, size);
                            elements.AddInfluencer(windInfluencer);
                            break;
                    }
                }
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
