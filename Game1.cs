using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Project1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D ballTexture; // Ball
        Texture2D pearlTexture, clamTexture,starTexture, startPointTexture, backgroundLayer1, bubblesParralax, fishesParallax, backgroundLayer2;
        Vector2 ballPosition; // Position
        float ballSpeed; // Speed

        public Map map;
        CandyVpt candy;
        VElement elements;
        Clam clam;
        public Camera cameraMono;

        private List<Vector2> slicePoints = new List<Vector2>();
        //private Point mouseStart, mouseEnd;
        private int checklevel;
        public int r;
        private bool allowRendering = true;

        Vector2 mouseEnd;

        Rectangle pantallaRect;

        // Mouse
        MouseState mouseState;
        Vector2 startMousePosition; // Almacena la posición del mouse cuando se presiona el botón izquierdo
        private bool isMousePressed;

        bool levelfinished;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = true;

            int w = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int h = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
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

            checklevel = 0;
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
            starTexture = Content.Load<Texture2D>("estrella");
            startPointTexture = Content.Load<Texture2D>("startVpt");
            backgroundLayer1 = Content.Load<Texture2D>("fondo0");
            backgroundLayer2 = Content.Load<Texture2D>("rocaArriba");
            fishesParallax = Content.Load<Texture2D>("pecesParallax");
            bubblesParralax = Content.Load<Texture2D>("burbujasParallax");
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

            levelfinished = false;

            elements.Update(pantallaRect);
            cameraMono.Follow(candy.Pos, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            cameraMono.ClampToArea(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height * 3, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            //Check for intersection between CandyVpt and PinnedVpt radius
            RadiusIntersectionDetection(elements.pndPts, elements.cndPts);

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

            // Slice rope
            // Handling Mouse Pressed
            if (mouseState.LeftButton == ButtonState.Pressed && !isMousePressed)
            {
                isMousePressed = true;
                Vector2 startPoint = new Vector2(mouseState.X, mouseState.Y);
                slicePoints.Add(startPoint); // Add the starting point
            }

            // Handling Mouse Movement
            if (isMousePressed)
            {
                Vector2 currentPoint = new Vector2(mouseState.X, mouseState.Y);
                if (!slicePoints.Contains(currentPoint)) // To avoid adding duplicate points
                {
                    slicePoints.Add(currentPoint);
                    if (slicePoints.Count > 1)
                    {
                        //IntersectionDetection(scene.Elements[0].stks, slicePoints);
                        //IntersectionDetection(scene.Elements[0].rps, slicePoints);
                    }

                    if (slicePoints.Count > 100) // Control line length
                    {
                        slicePoints.RemoveAt(0); // Always remove the first added
                    }
                }
            }

            // Handling Mouse Released
            if (mouseState.LeftButton == ButtonState.Released && isMousePressed)
            {
                isMousePressed = false;
                mouseEnd = new Vector2(mouseState.X, mouseState.Y);

                if (slicePoints.Count > 1)
                {
                    //IntersectionDetection(scene.Elements[0].rps, slicePoints);
                }

                slicePoints.Clear();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Show elements on the map
            _spriteBatch.Begin();

            // Parallax
            _spriteBatch.Draw(backgroundLayer1, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            _spriteBatch.Draw(bubblesParralax, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            _spriteBatch.Draw(fishesParallax, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            _spriteBatch.Draw(backgroundLayer2, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            elements.Render(_spriteBatch, pantallaRect, map.currentLevel, pearlTexture, starTexture, clamTexture, startPointTexture, cameraMono);

            // Draw a line in the cut
            if (isMousePressed)
            {
                MouseState mouseState = Mouse.GetState();
                Vector2 currentMousePosition = new Vector2(mouseState.X, mouseState.Y);
                _spriteBatch.DrawLine(startMousePosition, currentMousePosition, Color.White);
            }
            
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
        
        private Vector2 ConvertScreenToWorld(Vector2 screenPoint)
        {
            // Calculate the ratio of the screen coordinates to the control size
            float xRatio = screenPoint.X / (float)pantallaRect.X;
            float yRatio = screenPoint.Y / (float)pantallaRect.Y;

            // Use the ratio to calculate the world coordinates
            float worldX = map.fOffsetX + xRatio * (map.nTileWidth * map.nVisibleTilesX);
            float worldY = map.fOffsetY + yRatio * (map.nTileHeight * map.nVisibleTilesY);

            return new Vector2(worldX, worldY);
        }

        
        private void IntersectionDetection(List<VRope> ropes, List<Vector2> slicePts)
        {
            float intersectionRadius = 15;
            bool ropeCut = false;

            foreach (var rope in ropes)
            {
                foreach (var slicePoint in slicePts)
                {
                    foreach (var stick in rope.Sticks.ToList()) // Create a copy for safe modification
                    {
                        Vector2 worldPoint = ConvertScreenToWorld(slicePoint);
                        float distance = DistanceBetweenPoints(worldPoint, stick.GetMidpoint());
                        if (distance <= intersectionRadius)
                        {
                            rope.DeleteStick(stick); // Cut the rope
                            ropeCut = true;
                            break; // Exit the innermost loop
                        }
                    }

                    if (ropeCut)
                    {
                        rope.DeleteCutVRope(); // Remove all sticks if one is cut
                        return; // Exit the method after handling the cut
                    }
                }
            }
        }

        private void RadiusIntersectionDetection(List<PinnedVpt> pinnedVpts, List<CandyVpt> candyVpts)
        {
            foreach (var pinnedPt in pinnedVpts)
            {
                foreach (var candyPt in candyVpts)
                {
                    if (candyPt.Pos.Distance(pinnedPt.Pos) <= pinnedPt.Radius)
                    {
                        if (pinnedPt.Available)
                        {
                            VRope rope = new VRope(pinnedPt, candyPt, 6, pinnedPt.Level);
                            elements.AddRope(rope);
                            pinnedPt.Available = false;
                        }
                    }
                }
            }
        }

        private float DistanceBetweenPoints(Vector2 point1, Vector2 point2)
        {
            // Calculate the Euclidean distance between two points
            return (float)Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
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
