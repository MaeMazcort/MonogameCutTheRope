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
        private Effect customEffect;
        private Effect inters;

        Texture2D ballTexture;

        EffectParameter colorTintParameter;
        VertexBuffer vertexBuffer;
        Vector4 colorTint; // Blanco inicial

        public Game1()
        {
            _graphics               = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen  = true;
            
            int w   = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int h   = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            int div = 1;

            _graphics.PreferredBackBufferWidth = w/div;
            _graphics.PreferredBackBufferHeight = h/div;//*/
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
            customEffect    = Content.Load<Effect>("Blue");
            inters          = Content.Load<Effect>("Inter");

            ballTexture = Content.Load<Texture2D>("ball");

            //// Obtener el parámetro del color del efecto
            colorTintParameter  = customEffect.Parameters["ColorTint"];
            colorTint           = new Vector4(1, 1, 1, 1);
            // Configuración inicial del color (puedes ajustar según tus necesidades)
            colorTintParameter.SetValue(colorTint);
            
            // Configurar el efecto
            customEffect.Parameters["World"].SetValue(Matrix.Identity);
            customEffect.Parameters["View"].SetValue(Matrix.Identity);
            customEffect.Parameters["Projection"].SetValue(Matrix.Identity);//*/


            // Crear una lista de vértices (clases)
            List<VertexPositionColor> verticesList = new List<VertexPositionColor>
            {
                new VertexPositionColor(new Vector3(0, 1, 0), Color.Red),
                new VertexPositionColor(new Vector3(1, -1, 0), Color.Green),
                new VertexPositionColor(new Vector3(-1, -1, 0), Color.Blue),
            };

            //// Convertir la lista a un array
            VertexPositionColor[] verticesArray = verticesList.ToArray();

            //// Crear un buffer de vértices
            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), verticesArray.Length, BufferUsage.None);

            if (verticesArray != null && verticesArray.Length > 0)
            {
                vertexBuffer.SetData(verticesArray);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Actualizar el color de manera interactiva (por ejemplo, cambiar con el tiempo)
            colorTint.X =   (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds);
            colorTint.Y =   (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds);            
            colorTint.Z = 1-(float)Math.Sin(gameTime.TotalGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Ball
            _spriteBatch.Begin();
            _spriteBatch.Draw(ballTexture, new Vector2(0, 0), Color.White);
            _spriteBatch.End();

            customEffect.Parameters["ColorTint"].SetValue(colorTint);

            // Configurar el efecto antes de dibujar           
            GraphicsDevice.SetVertexBuffer(vertexBuffer);
           
            for(int p=0;p<customEffect.CurrentTechnique.Passes.Count;p++)
            {
                customEffect.CurrentTechnique.Passes[p].Apply();
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
            }

            base.Draw(gameTime);
        }
    }
}
