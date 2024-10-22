using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;


namespace Tanky
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _texture;


        private Vector2 playerPosition;
        private float playerSpeed = 100f;




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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            _texture = Content.Load<Texture2D>("tank");

            
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here
            KeyboardState state = Keyboard.GetState();
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (state.IsKeyDown(Keys.W))
                playerPosition.Y -= playerSpeed * deltaTime;

            
            if (state.IsKeyDown(Keys.S))
                playerPosition.Y += playerSpeed * deltaTime;

           
            if (state.IsKeyDown(Keys.A))
                playerPosition.X -= playerSpeed * deltaTime;

            
            if (state.IsKeyDown(Keys.D))
                playerPosition.X += playerSpeed * deltaTime;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);


            //_spriteBatch.Draw(sprite.texture, sprite.Rect, Color.White);
            _spriteBatch.Draw(_texture, playerPosition, Color.White);




            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
