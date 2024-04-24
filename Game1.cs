using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Project1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D ballTexture;

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
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch    = new SpriteBatch(GraphicsDevice);

            ballTexture = Content.Load<Texture2D>("ball");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Ball
            _spriteBatch.Begin();
            _spriteBatch.Draw(ballTexture, new Vector2(0, 0), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
