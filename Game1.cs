using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using System.Timers;
using static System.Formats.Asn1.AsnWriter;
using System.Collections;

namespace Project1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D ballTexture; // Ball
        Texture2D pearlTexture;
        Texture2D clamTexture;
        Texture2D starTexture;
        Vector2 ballPosition; // Position
        float ballSpeed; // Speed

        public Map map;
        CandyVpt candy;
        VElement elements;
        Clam clam;

        public float delta;
        public Scene scene;
        private List<Vector2> slicePoints = new List<Vector2>();
        private Stopwatch stopwatch = new Stopwatch();
        //private Point mouseStart, mouseEnd;
        private int checklevel;
        public int r, tick_counter = 0;
        private float targetPosY;
        private float easingStartTime;
        private float easingDuration = 1.0f; // Duration in seconds
        private System.Timers.Timer renderTimer;
        private bool allowRendering = true;

        Rectangle pantallaRect;

        // Mouse
        MouseState mouseState;
        Vector2 startMousePosition; // Almacena la posición del mouse cuando se presiona el botón izquierdo
        private bool isMousePressed;

        bool levelfinished, up;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = true;

            int w = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int h = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            int div = 1;

            //pantallaRect = new Rectangle(0, 0, w, h);

            _graphics.PreferredBackBufferWidth = w / div;
            _graphics.PreferredBackBufferHeight = h / div;
            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            stopwatch.Start();
        }

        private void Init()
        {
            pantallaRect = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            scene = new Scene();
            scene.AddElement(new VElement());
            scene.Elements[0].SetMap(map);

            map = new Map(pantallaRect, ref candy, ref elements, ref clam, scene, pearlTexture, starTexture, clamTexture);
            map.currentLevel = 1;

            delta = 0;
            checklevel = 0;
            r = 0;
            renderTimer = new System.Timers.Timer();
            renderTimer.Elapsed += OnRenderTimerElapsed;
            renderTimer.AutoReset = false;
            levelfinished = false;
            up = false;
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

            // Code from last project
            // UpdateEnv(); // Check the logic for thi
            levelfinished = false;

            scene.Elements[0].Update(pantallaRect);

            //Check for intersection between CandyVpt and PinnedVpt radius
            RadiusIntersectionDetection(scene.Elements[0].pndPts, scene.Elements[0].cndPts);

            levelfinished = LevelChangeDetections(scene.Elements[0].clam);
            if (levelfinished)
            {
                r = 0;
                // Start the timer to delay rendering
                renderTimer.Interval = 2000;  // Delay rendering for 2 seconds, adjust as needed
                renderTimer.Start();
                allowRendering = false;  // Stop rendering until the timer elapses
            }
            // The else statement is in another part

            // Check if the star is collected
            ObtainStar();

            tick_counter++;

            delta += 0.001f;

            CheckGameState();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Ball
            _spriteBatch.Begin();

            for (int i = 0; i < scene.Elements[0].pts.Count; i++) {
                _spriteBatch.Draw(
                pearlTexture, new Rectangle((int)scene.Elements[0].pts[i].Pos.X, (int)scene.Elements[0].pts[i].Pos.Y, 40, 40), Color.White
            );
            }

            // Render stars
            for (int i = 0; i < scene.Elements[0].strs.Count; i++)
            {
                _spriteBatch.Draw(starTexture, new Rectangle((int)(scene.Elements[0].strs[i].Position.X - 10), (int)(scene.Elements[0].strs[i].Position.Y - 10), 40, 40), Color.White);
            }

            // Render ropes
            for (int p = 0; p < scene.Elements[0].rps.Count; p++)
            {
                scene.Elements[0].rps[p].Render(_spriteBatch, pantallaRect);
            }

            // Render clam
            _spriteBatch.Draw(clamTexture, new Rectangle((int)(clam.Position.X - 20), (int)(clam.Position.Y - 30), 40, 40), Color.White);
           

            //scene.Elements[0].Render(_spriteBatch, pantallaRect, checklevel, pearlTexture, starTexture, clamTexture);

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
                scene.Render(_spriteBatch, pantallaRect, checklevel, pearlTexture, starTexture, clamTexture);  // Render only if allowed
                if (r == 0)
                {
                    if (scene.Elements[0].cndPts.Count > 0)
                    {
                        for (int i = 0; i < scene.Elements[0].cndPts.Count; i++)
                        {
                            for (int j = 0; j < scene.Elements[0].strtPts.Count; j++)
                            {
                                if (scene.Elements[0].strtPts[j].Level == scene.Elements[0].cndPts[i].Level)
                                {
                                    VRope rope = new VRope(scene.Elements[0].strtPts[j], scene.Elements[0].cndPts[i], 6,
                                    scene.Elements[0].strtPts[j].Level);
                                    scene.Elements[0].AddRope(rope);
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
        private void OnRenderTimerElapsed(object sender, ElapsedEventArgs e)
        {
            allowRendering = true;
        }

        private float GetCurrentTimeInSeconds()
        {
            return (float)stopwatch.Elapsed.TotalSeconds;
        }

        private float easeInOutQuad(float t)
        {
            return t < 0.5 ? 2 * t * t : -1 + (4 - 2 * t) * t;
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
                            scene.Elements[0].AddRope(rope);
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

            foreach (var star in scene.Elements[0].strs)
            {
                foreach (var candy in scene.Elements[0].cndPts)
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
                scene.Elements[0].strs.Remove(collectedStar);
            }
        }

        public bool LevelChangeDetections(Clam clam)
        {
            for (int i = 0; i < scene.Elements[0].cndPts.Count; i++)
            {
                if (clam.AteCandy(scene.Elements[0].cndPts[i]))
                {
                    map.score += 100;
                    if (map.currentLevel == 3)
                    {
                        //GameWon();
                    }
                    else
                    {
                        Console.WriteLine("Level " + map.currentLevel + " completed!");

                        scene.Elements[0].cndPts.Remove(scene.Elements[0].cndPts[i]);
                        scene.Elements[0].DeleteClam();
                        Console.WriteLine("\nLevel " + map.currentLevel + " started!");
                    }
                    map.currentLevel++;
                    return true;
                }
            }
            return false;
        }


        public bool CheckCandyVptCollisionWithFloor()
        {
            // Loop through all elements to find CandyVpt
            for (int i = 0; i < scene.Elements[0].cndPts.Count; i++)
            {
                // Calculate the tile coordinates for the candy
                int tileX = (int)(scene.Elements[0].cndPts[i].Pos.X / map.nTileWidth);
                int tileY = (int)(scene.Elements[0].cndPts[i].Pos.Y / map.nTileHeight) + 1;  // Check the tile directly below the candy

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
