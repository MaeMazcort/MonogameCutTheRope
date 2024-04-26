using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Project1
{
        public class Game1 : Game
        {
            private GraphicsDeviceManager _graphics;
            private SpriteBatch _spriteBatch;

            Texture2D ballTexture; // Ball
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

            public Game1()
            {
                _graphics               = new GraphicsDeviceManager(this);
                _graphics.IsFullScreen  = true;
            
                int w   = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                int h   = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                int div = 1;

                _graphics.PreferredBackBufferWidth = w/div;
                _graphics.PreferredBackBufferHeight = h/div;
                _graphics.ApplyChanges();

                Content.RootDirectory = "Content";
                IsMouseVisible = true;

                Init();

                stopwatch.Start();
                //PCT_CANVAS.MouseMove += PCT_MouseMove;
                //PCT_CANVAS.MouseDown += PCT_MousePressed;
                //PCT_CANVAS.MouseUp += PCT_MouseReleased;
                //PCT_CANVAS.Paint += PCT_Paint;
            }

            public void Init()
            {

            }

            protected override void Initialize()
            {
                ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2,
                _graphics.PreferredBackBufferHeight / 2);
                ballSpeed = 100f;

                base.Initialize();
            }

            protected override void LoadContent()
            {
                _spriteBatch    = new SpriteBatch(GraphicsDevice);
                SpriteBatchExtensions.Initialize(GraphicsDevice);
            
                ballTexture = Content.Load<Texture2D>("perla");
            }

            protected override void Update(GameTime gameTime)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();

                MouseState mouseState = Mouse.GetState();

                if (mouseState.LeftButton == ButtonState.Pressed && !isMousePressed)
                {
                    // El botón izquierdo del mouse se ha presionado
                    isMousePressed = true;
                    startMousePosition = new Vector2(mouseState.X, mouseState.Y);
                }
                else if (mouseState.LeftButton == ButtonState.Released && isMousePressed)
                {
                    // El botón izquierdo del mouse se ha soltado
                    isMousePressed = false;
                }

                base.Update(gameTime);
            }

            protected override void Draw(GameTime gameTime)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);

                // Ball
                _spriteBatch.Begin();
                _spriteBatch.Draw(
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
                _spriteBatch.End();

                base.Draw(gameTime);
            }
        }
}
