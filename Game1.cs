﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
        Texture2D clamClosedTexture, pearlTexture, clamTexture, starTexture, startPointTexture, circle, blowFishRight, blowFishLeft;
        Texture2D backgroundLayer1, bubblesParralax, fishesParallax, backgroundLayer2;

        public Map map;
        CandyVpt candy;
        VElement elements;
        Clam clam;
        public Camera cameraMono;
        private float fishSpeed = 1f;
        private float bubbleSpeed = 1f;
        int parallaxHeight = 1000;
        int parallaxWidth = 1400;
        public int w, h, currentLevel = 1;
        KeyboardState keyboardState;

        Rectangle pantallaRect;

        private SpriteFont font;

        // Mouse
        Vector2 fishPosition, fishPosition2, bubblesPosition, bubblesPosition2;
        Vector2 startMousePosition; // Almacena la posición del mouse cuando se presiona el botón izquierdo
        private bool isMousePressed;

        bool levelfinished;
        bool gameOver, gameWon;

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

            map = new Map();
            map.Draw(pantallaRect, ref candy, ref elements, ref clam, pearlTexture, starTexture, clamTexture, currentLevel);
            SetupRopes();
        }

        private void SetupRopes()
        {
            if (elements.cndPts.Count > 0 && elements.strtPts.Count > 0)
            {
                foreach (var startPoint in elements.strtPts)
                {
                    if (startPoint.Level == candy.Level)
                    {
                        VRope rope = new VRope(startPoint, candy, 6, currentLevel);
                        elements.AddRope(rope);
                    }
                }
            }
        }

        protected override void Initialize()
        {
            Init();

            // Configura MediaPlayer para repetir la canción
            MediaPlayer.IsRepeating = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            SpriteBatchExtensions.Initialize(GraphicsDevice);

            // Elements
            pearlTexture = Content.Load<Texture2D>("perla");
            clamTexture = Content.Load<Texture2D>("almeja");
            clamClosedTexture = Content.Load<Texture2D>("almejaTite");
            starTexture = Content.Load<Texture2D>("estrella");
            startPointTexture = Content.Load<Texture2D>("startVpt");
            circle = Content.Load<Texture2D>("radio");
            blowFishLeft = Content.Load<Texture2D>("pezGlobo2");
            blowFishRight = Content.Load<Texture2D>("pezGlobo");

            // Background
            backgroundLayer1 = Content.Load<Texture2D>("fondo0");
            backgroundLayer2 = Content.Load<Texture2D>("rocaArriba");
            fishesParallax = Content.Load<Texture2D>("pecesParallax");
            bubblesParralax = Content.Load<Texture2D>("burbujasParallax");

            //Sounds
            SoundManager.song = Content.Load<Song>("Burbujas");

            SoundManager.eatSound = Content.Load<SoundEffect>("eat");
            SoundManager.cutSound = Content.Load<SoundEffect>("cut");
            SoundManager.starSound = Content.Load<SoundEffect>("star");
            SoundManager.grabPointSound = Content.Load<SoundEffect>("grabPoint");

            SoundManager.instEat = SoundManager.eatSound.CreateInstance();
            SoundManager.instCut = SoundManager.cutSound.CreateInstance();
            SoundManager.instStar = SoundManager.starSound.CreateInstance();
            SoundManager.instGrabPoint = SoundManager.grabPointSound.CreateInstance();

            fishPosition = new Vector2(0, 0);
            fishPosition2 = new Vector2(parallaxWidth, 0);
            bubblesPosition = new Vector2(0, 0);
            bubblesPosition2 = new Vector2(0, parallaxHeight);
            
            //Fonts
            font = Content.Load<SpriteFont>("MyFont");

        }

        protected override void Update(GameTime gameTime)
        {
            // Default in Monogame
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Play and stop sound
            keyboardState = Keyboard.GetState();

            if( keyboardState.IsKeyDown(Keys.Up) )
            {
                SoundManager.PlaySong();
            }
            if(keyboardState.IsKeyDown(Keys.Down))
            {
                SoundManager.StopSong();
            }

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

            elements.Update(pantallaRect, gameTime);
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

            levelfinished = LevelChangeDetections(clam);
            CheckLevelCompletion(levelfinished);
            // The else statement is in another part

            // Check if the star is collected
            ObtainStar();

            CheckGameState();

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
                        // Play sound
                        SoundManager.instCut.Pan = 1;
                        SoundManager.instCut.Volume = 1f;
                        SoundManager.instCut.Play();
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

            elements.Render(_spriteBatch, pantallaRect, currentLevel, pearlTexture, starTexture, clamTexture, startPointTexture, clamClosedTexture, circle, blowFishLeft, blowFishRight,cameraMono);

            // Draw a line in the cut
            if (isMousePressed)
            {
                MouseState mouseState = Mouse.GetState();
                Vector2 currentMousePosition = new Vector2(mouseState.X, mouseState.Y);
                _spriteBatch.DrawLine(startMousePosition, currentMousePosition, Color.White);
            }

            //Check for intersection between CandyVpt and PinnedVpt radius
            RadiusIntersectionDetection(elements.pndPts);

            //Draw "Game Over" message
            if (gameOver)
            {
                string message = "Game Over";
                Vector2 fontSize = font.MeasureString(message);
                Vector2 position = new Vector2((GraphicsDevice.Viewport.Width - fontSize.X * 2) / 2, (GraphicsDevice.Viewport.Height - fontSize.Y * 2) / 2);  // Center the text
                _spriteBatch.DrawString(font, message, position, Color.Red, 0, Vector2.Zero, 2.0f, SpriteEffects.None, 0);
            }

            // Draw "Game Won" message
            if (gameWon)
            {
                string gameWonMessage = "Congratulations! You Won!";
                Vector2 gameWonSize = font.MeasureString(gameWonMessage) * 2.0f; // Scale the text size by 2
                Vector2 gameWonPosition = new Vector2((GraphicsDevice.Viewport.Width - gameWonSize.X) / 2, (GraphicsDevice.Viewport.Height - gameWonSize.Y) / 2);
                _spriteBatch.DrawString(font, gameWonMessage, gameWonPosition, Color.Green, 0, Vector2.Zero, 2.0f, SpriteEffects.None, 0);
            }

            //Draw score
            string scoreText = $"Score: {map.score}";
            _spriteBatch.DrawString(font, scoreText, new Vector2(100, 100), Color.Black);
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void RadiusIntersectionDetection(List<PinnedVpt> pinnedVpts)
        {
            foreach (var pinnedPt in pinnedVpts)
            {
                    if (candy.Pos.Distance(pinnedPt.Pos) <= pinnedPt.Radius)
                    {
                        if (pinnedPt.Available)
                        {
                            // Play sound
                            SoundManager.instGrabPoint.Pan = 1;
                            SoundManager.instGrabPoint.Volume = 1f;
                            SoundManager.instGrabPoint.Play();
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
                        // Play sound
                        SoundManager.instStar.Pan = 1;
                        SoundManager.instStar.Volume = 1f;
                        SoundManager.instStar.Play();

                        UpdateScore(10);
                        Console.WriteLine("Star collected. Score : " + map.score);
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

        public bool LevelChangeDetections(Clam clam)
        {
            for (int i = 0; i < elements.cndPts.Count; i++)
            {
                if (clam.AteCandy(elements.cndPts[i]))
                {
                    // Play sound
                    SoundManager.instEat.Pan = 1;
                    SoundManager.instEat.Volume = 1f;
                    SoundManager.instEat.Play();
                    
                    UpdateScore(100);
                    Console.WriteLine("Candy collected. Score : " + map.score);
                    if (currentLevel == 3)
                    {
                        GameWon();
                    }
                    else
                    {
                        elements.cndPts.Remove(elements.cndPts[i]);
                        elements.DeleteClam();
                    }
                    return true;
                }
            }
            return false;
        }

        private void UpdateScore(int pointsToAdd)
        {
            map.score += pointsToAdd;
            Console.WriteLine($"Score updated: {map.score}");
        }
        
        public bool IsCandyLost(Camera cameraMono)
        {
            // Check if any candy has fallen below the camera's position.
            foreach (var candy in elements.cndPts)
            {
                // Check if the Y coordinate of the candy is greater than the Y coordinate of the camera.
                if (candy.Pos.Y > cameraMono.position.Y +  GraphicsDevice.Viewport.Height - 20) 
                {
                    return true;
                }
            }
            return false;
        }
        
        public void CheckGameState()
        {
            if (IsCandyLost(cameraMono))
            {
                GameOver();
            }
        }

        public void CheckLevelCompletion(bool levelfinished)
        {
            if (levelfinished)
            {
                currentLevel++;
                if (currentLevel > 3)
                {
                    currentLevel = 1;  // Loop back to the first level or handle the game completion 
                }
                Init();  // Re-initialize game state for the new level
            }
        }
        
        public void GameOver()
        {
            gameOver = true;
            Console.WriteLine(gameOver);
        }

        public void GameWon()
        {
            gameWon = true;
            Console.WriteLine(gameWon);
        }
    }
}
