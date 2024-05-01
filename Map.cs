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
        public int currentLevel;
        public float fOffsetX;
        public float fOffsetY;
        public float nVisibleTilesX;
        public float nVisibleTilesY;

        // New implementation
        public int nLevelWidth = 21;
        public int nLevelHeight = 30;
        public int nTileWidth, nTileHeight;
        string sLevel;


        public Map(Rectangle size, ref CandyVpt candy, ref VElement elements, ref Clam clam, Scene scene, Texture2D perlaTexture, Texture2D estrellaTexture, Texture2D almejaTexture)
        {
            score = 0;

            sLevel  = ".....................";
            sLevel += "..........1......1...";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += "..........A..........";
            sLevel += ".....................";
            sLevel += "..................S..";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += "....................."; // 10
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += "..........S..........";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += "....................."; // 20
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += ".....................";
            sLevel += "..........M..........";
            sLevel += "....................."; // 20

            int nTileWidth = size.Width / nLevelWidth;
            int nTileHeight = size.Height / nLevelHeight;

            Size bmp = new Size(2 * size.Width, size.Height);

            for (int y = 0; y < nLevelHeight; y++)
            {
                for (int x = 0; x < nLevelWidth; x++)
                {
                    int index = y * nLevelWidth + x;
                    if (sLevel[index] == 'A')
                    {
                        Vector2 posicion = new Vector2(x * nTileWidth, y * nTileHeight);

                        candy = new CandyVpt(x * nTileWidth, y * nTileHeight, id, level: 1, perlaTexture);
                        scene.Elements[0].AddPoint(candy);
                        scene.Elements[0].AddCandyPoint(candy);
                        id++;
                    }
                    if (sLevel[index] == 'M')
                    {
                        clam = new Clam(x * nTileWidth, y * nTileHeight, almejaTexture);
                        scene.Elements[0].SetClam(new Vector2(stepX, stepY), almejaTexture);
                    }
                }
            }
            for (int y = 0; y < nLevelHeight; y++)
            {
                for (int x = 0; x < nLevelWidth; x++)
                {
                    int index = y * nLevelWidth + x;
                    switch (sLevel[index])
                    {
                        case '.':
                            break;

                        case 'S':
                            var star1 = new Star(x * nTileWidth, y * nTileHeight, level: 1, estrellaTexture);
                            scene.Elements[0].AddStar(star1);
                            break;
                        case '1':
                            var startVpt1 = new StartVpt(x * nTileWidth, y * nTileHeight, id, level: 1);
                            scene.Elements[0].AddPoint(startVpt1);
                            scene.Elements[0].AddStartPoint(startVpt1);
                            id++;
                            break;
                    }
                }
            }
        }

        public void SetTile(float x, float y, char c)
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
