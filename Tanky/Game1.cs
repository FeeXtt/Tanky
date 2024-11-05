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
        private float playerRotation = 0f;
        private float playerRotationSpeed = 2f;
        private int playerHealth = 6;
        
        private Texture2D bulletTexture;
        private List<Vector2> bulletPositions;
        private List<Vector2> bulletDirections;
        private float bulletSpeed = 200f;
        
        
        private Texture2D enemyBulletTexture;
        private List<Vector2> enemyBulletPositions;
        private List<Vector2> enemyBulletDirections;
        private float enemyBulletSpeed = 170f;
        private float enemyCooldown = 3f;
        private float enemyShootTimer = 0f;
        
        private Texture2D enemyTankTexture;
        private Vector2 enemyTankPosition;
        private float enemyTankSpeed = 55f;
        private float enemyTankRotation = 0f;
        private int enemyHealth = 6;
        private bool enemyIsAlive = true;
        private Random random;




        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            playerPosition = new Vector2(100, 100);
            enemyTankPosition = new Vector2(300, 300);
            bulletPositions = new List<Vector2>();
            bulletDirections = new List<Vector2>();
            enemyBulletPositions = new List<Vector2>();
            enemyBulletDirections = new List<Vector2>();
            random = new Random();
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            

            _texture = Content.Load<Texture2D>("tank");
            bulletTexture = Content.Load<Texture2D>("bullet");
            enemyTankTexture = Content.Load<Texture2D>("enemyTank");
            enemyBulletTexture = Content.Load<Texture2D>("bullet");

            
            
        }

        protected override void Update(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var state = Keyboard.GetState();
            var mouseState = Mouse.GetState();
            var viewport = _graphics.GraphicsDevice.Viewport;

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
                Vector2 bulletPosition = playerPosition;
                Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
                Vector2 direction = Vector2.Normalize(mousePosition - bulletPosition);

                bulletPositions.Add(bulletPosition);
                bulletDirections.Add(direction);
                
                
            }
            
            for (int i = 0; i < bulletPositions.Count; i++)
            {
                bulletPositions[i] += bulletDirections[i] * bulletSpeed * deltaTime;
            }

            if (enemyIsAlive)
            {
                Vector2 directionToTank = playerPosition - enemyTankPosition;
                if (directionToTank.Length() > 10f) 
                {
                    directionToTank.Normalize();
                    enemyTankPosition += directionToTank * enemyTankSpeed * deltaTime;
                    enemyTankRotation = (float)Math.Atan2(directionToTank.Y, directionToTank.X);
                }

                
                enemyShootTimer += deltaTime;
                if (enemyShootTimer >= enemyCooldown)
                {
                    Vector2 enemyBulletDirection = Vector2.Normalize(playerPosition - enemyTankPosition);
                    enemyBulletPositions.Add(enemyTankPosition);
                    enemyBulletDirections.Add(enemyBulletDirection);
                    enemyShootTimer = 0f;
                }
            }
            
            for (int i = 0; i < enemyBulletPositions.Count; i++)
            {
                enemyBulletPositions[i] += enemyBulletDirections[i] * enemyBulletSpeed * deltaTime;
            }


            for (int i = 0; i < bulletPositions.Count; i++)
            {
                if (enemyIsAlive && Vector2.Distance(bulletPositions[i], enemyTankPosition) < 20f)
                {
                    bulletPositions.RemoveAt(i);
                    bulletDirections.RemoveAt(i);
                    enemyHealth--;
                    if (enemyHealth <= 0)
                    {
                        enemyIsAlive = false;
                    }
                    break;
                }
            }
            
            if (Vector2.Distance(playerPosition, enemyTankPosition) < 30f && enemyIsAlive)
            {
                playerHealth--;
                playerPosition = new Vector2(100, 100);
            }

            for (int i = 0; i < enemyBulletPositions.Count; i++)
            {
                if (Vector2.Distance(enemyBulletPositions[i], playerPosition) < 10f)
                {
                    enemyBulletPositions.RemoveAt(i);
                    enemyBulletDirections.RemoveAt(i);
                    playerHealth--;
                    break;
                }
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
            
            if (enemyIsAlive)
            {
                _spriteBatch.Draw(enemyTankTexture, enemyTankPosition, null, Color.White, enemyTankRotation,
                    new Vector2(enemyTankTexture.Width / 2, enemyTankTexture.Height / 2), 1f, SpriteEffects.None, 0f);
            }
            
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
            foreach (var enemyBullet in enemyBulletPositions )
            {
                _spriteBatch.Draw(bulletTexture, enemyBullet, Color.Yellow);
            }

            




            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
