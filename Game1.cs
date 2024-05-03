using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Formats.Asn1.AsnWriter;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Project1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D clamClosedTexture, pearlTexture, clamTexture, starTexture, startPointTexture, circle;
        Texture2D backgroundLayer1, bubblesParralax, fishesParallax, backgroundLayer2;

        public Map map;
        CandyVpt candy;
        VElement elements;
        Clam clam;
        public Camera cameraMono;

        private List<Vector2> slicePoints = new List<Vector2>();
        public int r, w, h;
        private float fishSpeed = 1f;
        private float bubbleSpeed = 1f;
        private bool allowRendering = true;
        int parallaxHeight = 1000;
        int parallaxWidth = 1400;

    Rectangle pantallaRect;

        // Mouse
        Vector2 fishPosition, fishPosition2, bubblesPosition, bubblesPosition2;
        Vector2 startMousePosition; // Almacena la posición del mouse cuando se presiona el botón izquierdo
        private bool isMousePressed;

        bool levelfinished;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = true;

            w = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            h = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            int div = 1;

            _graphics.PreferredBackBufferWidth = w / div;
            _graphics.PreferredBackBufferHeight = h / div;
            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        private void Init()
        {
            pantallaRect = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height * 3);
            cameraMono = new Camera(new V2(0, 0));
            elements = new VElement();
            elements.SetMap(map);

            map = new Map(pantallaRect, ref candy, ref elements, ref clam, pearlTexture, starTexture, clamTexture);
            map.currentLevel = 1;

            r = 0;
            levelfinished = false;
        }


        protected override void Initialize()
        {
            Init();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            SpriteBatchExtensions.Initialize(GraphicsDevice);

            pearlTexture = Content.Load<Texture2D>("perla");
            clamTexture = Content.Load<Texture2D>("almeja");
            clamClosedTexture = Content.Load<Texture2D>("almejaTite");
            starTexture = Content.Load<Texture2D>("estrella");
            startPointTexture = Content.Load<Texture2D>("startVpt");
            circle = Content.Load<Texture2D>("circulo");

            backgroundLayer1 = Content.Load<Texture2D>("fondo0");
            backgroundLayer2 = Content.Load<Texture2D>("rocaArriba");
            fishesParallax = Content.Load<Texture2D>("pecesParallax");
            bubblesParralax = Content.Load<Texture2D>("burbujasParallax");

            fishPosition = new Vector2(0, 0);
            fishPosition2 = new Vector2(parallaxWidth, 0);
            bubblesPosition = new Vector2(0, 0);
            bubblesPosition2 = new Vector2(0, parallaxHeight);

        }

        protected override void Update(GameTime gameTime)
        {
            // Default in Monogame
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Draw a line with the mouse
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed && !isMousePressed)
            {
                isMousePressed = true;
                startMousePosition = new Vector2(mouseState.X, mouseState.Y);
            }
            else if (mouseState.LeftButton == ButtonState.Released && isMousePressed)
            {
                isMousePressed = false;
            }

            // Cut Ropes
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
            Vector2 worldMousePosition = ConvertScreenToWorld(mousePosition);  // Asegúrate de que esta conversión toma en cuenta el desplazamiento de la cámara

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 currentMousePosition = ConvertScreenToWorld(new Vector2(mouseState.X, mouseState.Y));
                if (!isMousePressed)
                {
                    isMousePressed = true;
                }
                else
                {
                    CutRope(currentMousePosition);
                }
            }
            else
            {
                isMousePressed = false;
            }


            levelfinished = false;

            elements.Update(pantallaRect);
            cameraMono.Follow(candy.Pos, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            cameraMono.ClampToArea(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height * 3, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            //PARALAX
            bubblesPosition.Y -= bubbleSpeed;
            bubblesPosition2.Y -= bubbleSpeed;
            if (bubblesPosition.Y < -parallaxHeight)
            {
                bubblesPosition.Y = parallaxHeight; // Ajusta la posición utilizando el módulo
            }
            if (bubblesPosition2.Y < -parallaxHeight)
            {
                bubblesPosition2.Y = parallaxHeight;
            }


            fishPosition.X -= fishSpeed;
            fishPosition2.X -= fishSpeed;
            if(fishPosition.X < -parallaxWidth)
            {
                fishPosition.X = parallaxWidth;
            }
            if(fishPosition2.X < -parallaxWidth)
            {
                fishPosition2.X = parallaxWidth;
            }


            //levelfinished = LevelChangeDetections(scene.Elements[0].clam);
            if (levelfinished)
            {
                r = 0;
                allowRendering = false;  // Stop rendering until the timer elapses
            }
            // The else statement is in another part

            // Check if the star is collected
            ObtainStar();

            CheckGameState();

            LevelChangeDetections(clam);

            base.Update(gameTime);
        }

        private void CutRope(Vector2 currentMousePosition)
        {
            float tolerance = 15.0f;

            foreach (var rope in elements.Rps.ToList())
            {
                foreach (var stick in rope.Sticks.ToList())
                {
                    if (LineIntersects(stick.GetMidpoint(), startMousePosition, currentMousePosition, tolerance))
                    {
                        rope.DeleteEntireRope();
                        break;
                    }
                }
            }
        }


        public bool LineIntersects(Vector2 point, Vector2 lineStart, Vector2 lineEnd, float radius)
        {
            if (Vector2.Distance(lineStart, lineEnd) == 0) return false;  

            Vector2 lineVector = lineEnd - lineStart;
            Vector2 pointVector = point - lineStart;

            float projection = Vector2.Dot(pointVector, lineVector) / lineVector.LengthSquared();
            if (projection < 0 || projection > 1) return false;  

            Vector2 nearestPoint = lineStart + projection * lineVector;
            return Vector2.Distance(nearestPoint, point) <= radius;
        }




        public Vector2 ClosestPointOnLine(Vector2 lineStart, Vector2 lineEnd, Vector2 point)
        {
            Vector2 AP = point - lineStart;       // Vector from lineStart to point
            Vector2 AB = lineEnd - lineStart;     // Vector from lineStart to lineEnd
            float abSquared = AB.LengthSquared(); // Distance squared from lineStart to lineEnd
            if (abSquared == 0) return lineStart; // lineStart and lineEnd are the same point

            float abDotAp = Vector2.Dot(AP, AB);  // Dot product of AP and AB
            float t = abDotAp / abSquared;        // The normalized "distance" from lineStart to the closest point

            t = MathHelper.Clamp(t, 0, 1);        // Clamp t to the range [0,1]
            return lineStart + t * AB;            // Compute the closest point
        }


        private Vector2 ConvertScreenToWorld(Vector2 screenPosition)
        {
            return new Vector2(screenPosition.X + cameraMono.position.X, screenPosition.Y + cameraMono.position.Y);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Show elements on the map
            _spriteBatch.Begin();

            // Parallax
            _spriteBatch.Draw(backgroundLayer1, new Rectangle(0, -(int)cameraMono.position.Y / 10, GraphicsDevice.Viewport.Width, (int)(GraphicsDevice.Viewport.Height * 1.25f)), Color.White);
            _spriteBatch.Draw(bubblesParralax, new Rectangle((int)bubblesPosition.X, (int)bubblesPosition.Y, GraphicsDevice.Viewport.Width, (int)(GraphicsDevice.Viewport.Height * 1.25f)), Color.White);
            _spriteBatch.Draw(bubblesParralax, new Rectangle((int)bubblesPosition2.X, (int)bubblesPosition2.Y, GraphicsDevice.Viewport.Width, (int)(GraphicsDevice.Viewport.Height * 1.25f)), Color.White);
            _spriteBatch.Draw(fishesParallax, new Rectangle((int)fishPosition.X, (int)fishPosition.Y, GraphicsDevice.Viewport.Width, (int)(GraphicsDevice.Viewport.Height * 1.25f)), Color.White);
            _spriteBatch.Draw(fishesParallax, new Rectangle((int)fishPosition2.X, (int)fishPosition2.Y, GraphicsDevice.Viewport.Width, (int)(GraphicsDevice.Viewport.Height * 1.25f)), Color.White);
            _spriteBatch.Draw(backgroundLayer2, new Rectangle(0, -(int)cameraMono.position.Y / 10, GraphicsDevice.Viewport.Width, (int)(GraphicsDevice.Viewport.Height * 1.25f)), Color.White);

            elements.Render(_spriteBatch, pantallaRect, map.currentLevel, pearlTexture, starTexture, clamTexture, startPointTexture, clamClosedTexture, circle, cameraMono);

            // Draw a line in the cut
            if (isMousePressed)
            {
                MouseState mouseState = Mouse.GetState();
                Vector2 currentMousePosition = new Vector2(mouseState.X, mouseState.Y);
                _spriteBatch.DrawLine(startMousePosition, currentMousePosition, Color.White);
            }

            //Check for intersection between CandyVpt and PinnedVpt radius
            RadiusIntersectionDetection(elements.pndPts);

            // Render ropes
            if (!levelfinished && allowRendering)
            {
                if (r == 0)
                {
                    if (elements.cndPts.Count > 0)
                    {
                        for (int i = 0; i < elements.cndPts.Count; i++)
                        {
                            for (int j = 0; j < elements.strtPts.Count; j++)
                            {
                                if (elements.strtPts[j].Level == elements.cndPts[i].Level)
                                {
                                    VRope rope = new VRope(elements.strtPts[j], elements.cndPts[i], 6,
                                    elements.strtPts[j].Level);
                                    elements.AddRope(rope);
                                }
                            }
                        }
                    }
                    r++;
                }
            }
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private float DistanceBetweenPoints(Vector2 point1, Vector2 point2)
        {
            // Calculate the Euclidean distance between two points
            return (float)Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }

        private void RadiusIntersectionDetection(List<PinnedVpt> pinnedVpts)
        {
            foreach (var pinnedPt in pinnedVpts)
            {
                    if (candy.Pos.Distance(pinnedPt.Pos) <= pinnedPt.Radius)
                    {
                        if (pinnedPt.Available)
                        {
                            VRope rope = new VRope(pinnedPt, candy, 15, pinnedPt.Level);
                            elements.AddRope(rope);
                            pinnedPt.Available = false;
                        }
                    }
            }
        }

        public void ObtainStar()
        {
            List<Star> collectedStars = new List<Star>();

            foreach (var star in elements.strs)
            {
                foreach (var candy in elements.cndPts)
                {
                    star.CheckCollision(candy);
                    if (star.IsCollected)
                    {
                        map.score += 10; // Add points when the star is collected
                        collectedStars.Add(star);
                    }
                }

            }

            // Remove the collected stars from the main list after checking all stars
            foreach (var collectedStar in collectedStars)
            {
                elements.strs.Remove(collectedStar);
            }
        }

        public void LevelChangeDetections(Clam clam)
        {
            for (int i = 0; i < elements.cndPts.Count; i++)
            {
                if (clam.AteCandy(elements.cndPts[i]))
                {
                    map.score += 100;
                    if (map.currentLevel == 3)
                    {
                        //GameWon();
                    }
                    else
                    {
                        Console.WriteLine("Level " + map.currentLevel + " completed!");

                        elements.cndPts.Remove(elements.cndPts[i]);
                        elements.DeleteClam();
                        Console.WriteLine("\nLevel " + map.currentLevel + " started!");
                    }
                    map.currentLevel++;
                }
            }
        }


        public bool CheckCandyVptCollisionWithFloor()
        {
            // Loop through all elements to find CandyVpt
            for (int i = 0; i < elements.cndPts.Count; i++)
            {
                // Calculate the tile coordinates for the candy
                int tileX = (int)(elements.cndPts[i].Pos.X / map.nTileWidth);
                int tileY = (int)(elements.cndPts[i].Pos.Y / map.nTileHeight) + 1;  // Check the tile directly below the candy

                // Check if the tile below the candy is a floor ('#')
                if (tileY < map.nLevelHeight && map.GetTile(tileX, tileY) == '#')
                {
                    return true;  // Collision with floor detected
                }
            }

            return false;  // No collision detected
        }


        public void CheckGameState()
        {
            if (CheckCandyVptCollisionWithFloor())
            {
                //GameOver();
            }
        }

        /*
        public void GameOver()
        {
            TIMER.Stop(); // Stop the timer first
            MessageBox.Show("You lost! You've lost the candy.", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();  // Closes the form and ends the application
        }
        */

    }
}
