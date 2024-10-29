using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Linq;


namespace Tanky
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _texture;


        private Vector2 playerPosition;
        private float playerSpeed = 100f;
        private float playerRotation;
        
        private Texture2D bulletTexture;
        private List<Vector2> bulletPositions;
        private List<Vector2> bulletDirections;
        private float bulletSpeed = 200f;




        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            playerPosition = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            bulletPositions = new List<Vector2>(); 
            bulletDirections = new List<Vector2>();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            _texture = Content.Load<Texture2D>("tank");
            bulletTexture = Content.Load<Texture2D>("bullet");

            
            
        }

        protected override void Update(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var state = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            if (state.IsKeyDown(Keys.W))
                playerPosition.Y -= playerSpeed * deltaTime;

            
            if (state.IsKeyDown(Keys.S))
                playerPosition.Y += playerSpeed * deltaTime;

           
            if (state.IsKeyDown(Keys.A))
                playerPosition.X -= playerSpeed * deltaTime;

            
            if (state.IsKeyDown(Keys.D))
                playerPosition.X += playerSpeed * deltaTime;
            
            playerPosition.X = MathHelper.Clamp(playerPosition.X, 0, GraphicsDevice.Viewport.Width - _texture.Width);
            playerPosition.Y = MathHelper.Clamp(playerPosition.Y, 0, GraphicsDevice.Viewport.Height - _texture.Height);
            
            var directionToMouse = new Vector2(mouseState.X, mouseState.Y) - playerPosition;
            playerRotation = (float)Math.Atan2(directionToMouse.Y, directionToMouse.X);
            
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                var bulletDirection = Vector2.Normalize(directionToMouse);
                bulletPositions.Add(playerPosition);
                bulletDirections.Add(bulletDirection);
            }
            
            for (int i = 0; i < bulletPositions.Count; i++)
            {
                bulletPositions[i] += bulletDirections[i] * bulletSpeed * deltaTime;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);


            _spriteBatch.Draw(
                _texture,
                playerPosition,
                null,
                Color.White,
                playerRotation,
                new Vector2(_texture.Width / 2, _texture.Height / 2),
                1f,
                SpriteEffects.None,
                0f);
            
            foreach (var bulletPosition in bulletPositions)
            {
                _spriteBatch.Draw(
                    bulletTexture,
                    bulletPosition,
                    
                    null,
                    Color.White,
                    0f,
                    new Vector2(bulletTexture.Width / 2, bulletTexture.Height / 2),
                    1f,
                    SpriteEffects.None,
                    0f);
            }




            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
