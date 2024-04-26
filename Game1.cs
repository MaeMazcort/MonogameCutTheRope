using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Timers;
using static System.Formats.Asn1.AsnWriter;

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

        Map map;
        public float delta;
        public Scene scene;
        private List<Point> slicePoints = new List<Point>();
        private Stopwatch stopwatch = new Stopwatch();
        private Point mouseStart, mouseEnd;
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


        // Camera properties
        float fCameraPosX = 0.0f;
        float fCameraPosY = 0.0f;
        bool levelfinished, up;

        //Parallax
        int motion1 = 1;
        int motion2 = 4;
        int motion3 = 8;

        int width = 300;
        int height = 220;

        int l1_X1, l1_X2, l2_X1, l2_X2, l3_X1, l3_X2, l4_X1, l4_X2;
        int l2_Y1, l2_Y2;
        // static Graphics g; // Check for the parallax

        // Bitmap layer1, layer2, layer3, layer4; // Check for the parallax

        int VWWIDTH, VWHEIGHT;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = true;

            int w = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            VWWIDTH = w;
            int h = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            VWHEIGHT = h;
            int div = 1;

            pantallaRect = new Rectangle(0, 0, w, h);

            _graphics.PreferredBackBufferWidth = w / div;
            _graphics.PreferredBackBufferHeight = h / div;
            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            //Init();

            stopwatch.Start();
            //PCT_CANVAS.MouseMove += PCT_MouseMove;
            //PCT_CANVAS.MouseDown += PCT_MousePressed;
            //PCT_CANVAS.MouseUp += PCT_MouseReleased;
            //PCT_CANVAS.Paint += PCT_Paint;
        }

        private void Init()
        {
            scene = new Scene();
            map = new Map(pantallaRect); // Obtén el tamaño de la ventana del juego
            map.currentLevel = 3;
            scene.AddElement(new VElement());
            scene.Elements[0].SetMap(map);
            map.Draw(new Vector2(fCameraPosX, fCameraPosY), scene, pearlTexture, starTexture, clamTexture);

            /*var startVpt1 = new StartVpt(VWWIDTH/2, 50, 2, level: 1);
            scene.Elements[0].AddPoint(startVpt1);
            scene.Elements[0].AddStartPoint(startVpt1);
            var candytemp = new CandyVpt(VWWIDTH / 2, 200, 1,1,pearlTexture);
            scene.Elements[0].AddPoint(candytemp);*/
            delta = 0;
            checklevel = 0;
            r = 0;
            renderTimer = new System.Timers.Timer();
            renderTimer.Elapsed += OnRenderTimerElapsed;
            renderTimer.AutoReset = false;
            levelfinished = false;
            up = false;

            // Parallax
            //layer1 = Properties.Resources.fondo0;
            //layer2 = Properties.Resources.burbujasParallax2;
            //layer3 = Properties.Resources.pecesParallax;
            //layer4 = Properties.Resources.rocaArriba;

            l1_X1 = l1_X2 = 0;
            l2_X1 = l2_X2 = 0;
            l3_X1 = 0;
            //l3_X2 = layer1.Width; // Tercera imagen justo al final de la primera

            l2_Y1 = 0; // Inicia en la parte inferior de la pantalla
            l2_Y2 = GraphicsDevice.Viewport.Height; // Segunda imagen justo fuera de la vista arriba


        }

        private void BackgroudMove()
        {
            l2_Y1 -= motion2;
            l2_Y2 -= motion2;
            if (l2_Y1 < -height) { l2_Y1 = height; }
            if (l2_Y2 < -height) { l2_Y2 = height; }

            l3_X1 -= motion3;
            l3_X2 -= motion3;
            if (l3_X1 < -width) { l3_X1 = width; }
            if (l3_X2 < -width) { l3_X2 = width; }
        }


        protected override void Initialize()
        {
            ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2,
            _graphics.PreferredBackBufferHeight / 2);
            ballSpeed = 100f;
            Init();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            SpriteBatchExtensions.Initialize(GraphicsDevice);

            ballTexture = Content.Load<Texture2D>("perla");
            pearlTexture = Content.Load<Texture2D>("perla");
            clamTexture = Content.Load<Texture2D>("almeja");
            starTexture = Content.Load<Texture2D>("estrella");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            scene.Elements[0].Update(pantallaRect);

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

            // Replacement for the 3 mouse functions
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                // Obtener la posición actual del mouse
                Point currentPoint = new Point(mouseState.X, mouseState.Y);

                if (!isMousePressed)
                {
                    isMousePressed = true;
                    slicePoints.Add(currentPoint); // Agregar el punto de inicio
                }
                else
                {
                    // Si ya se ha presionado el botón del mouse, agregar el punto solo si no está duplicado
                    if (!slicePoints.Contains(currentPoint))
                    {
                        slicePoints.Add(currentPoint);
                        if (slicePoints.Count > 1)
                        {
                            // Lógica para detectar intersecciones
                            IntersectionDetection(scene.Elements[0].rps, slicePoints);
                        }

                        if (slicePoints.Count > 100) // Control de la longitud de la línea
                        {
                            slicePoints.RemoveAt(0); // Si excede el límite, eliminar el primer punto agregado
                        }
                    }
                }
            }
            else
            {
                // Si el botón del mouse se ha soltado, limpiar los puntos y restablecer el estado
                if (isMousePressed)
                {
                    isMousePressed = false;
                    slicePoints.Clear();
                }
            }

            // Left code

            UpdateEnv();
            levelfinished = false;

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
                ballTexture, new Rectangle((int)scene.Elements[0].pts[i].Pos.X, (int)scene.Elements[0].pts[i].Pos.Y, 40, 40), Color.Aqua
            );

            }

            _spriteBatch.Draw(
            clamTexture, new Rectangle((int)scene.Elements[0].clam.Position.X, (int)scene.Elements[0].clam.Position.Y, 40, 40), Color.Aqua
            );

            /*_spriteBatch.Draw(
                ballTexture, ballPosition, null, Color.White, 0f, new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
                Vector2.One, SpriteEffects.None, 0f
            );

            if (isMousePressed)
            {
                // Dibujar una línea desde startMousePosition hasta la posición actual del mouse
                MouseState mouseState = Mouse.GetState();
                Vector2 currentMousePosition = new Vector2(mouseState.X, mouseState.Y);

                // Dibujar la línea
                _spriteBatch.DrawLine(startMousePosition, currentMousePosition, Color.White);
            }

            if (allowRendering)
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
            }*/

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

        private void UpdateEnv()
        {
            if (levelfinished)
            {
                targetPosY += 21.0f; // Set this based on the trigger condition
                easingStartTime = GetCurrentTimeInSeconds();
            }

            if (up)
            {
                targetPosY -= 21.0f; // Set this based on the trigger condition
                easingStartTime = GetCurrentTimeInSeconds();
            }

            if (GetCurrentTimeInSeconds() - easingStartTime < easingDuration)
            {
                float timeFraction = (GetCurrentTimeInSeconds() - easingStartTime) / easingDuration;
                fCameraPosY += (easeInOutQuad(timeFraction) * (targetPosY - fCameraPosY));
            }

            fCameraPosY = Math.Min(fCameraPosY, 48);

           // map.Draw(new Vector2(fCameraPosX, fCameraPosY), scene, pearlTexture, starTexture, clamTexture);
        }

        private Vector2 ConvertScreenToWorld(Point screenPoint)
        {
            // Calculate the ratio of the screen coordinates to the control size
            float xRatio = screenPoint.X / (float)pantallaRect.X;
            float yRatio = screenPoint.Y / (float)pantallaRect.Y;

            // Use the ratio to calculate the world coordinates
            float worldX = map.fOffsetX + xRatio * (map.nTileWidth * map.nVisibleTilesX);
            float worldY = map.fOffsetY + yRatio * (map.nTileHeight * map.nVisibleTilesY);

            return new Vector2(worldX, worldY);
        }

        private void IntersectionDetection(List<VRope> ropes, List<Point> slicePts)
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
