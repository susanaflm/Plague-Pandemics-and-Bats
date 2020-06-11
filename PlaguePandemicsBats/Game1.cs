using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlaguePandemicsBats
{
    /// <summary>
    /// Can be used to know the direction of the player or of the enemies
    /// </summary>
    public enum Direction
    {
        Up, Down, Left, Right
    }

    /// <summary>
    /// Type of Colliders, used when drawing a sprite
    /// </summary>
    public enum ColliderType
    { 
        OBB, AABB, Circle
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;

        private SpriteBatch _spriteBatch;

        private Camera _camera;

        private SpriteManager _spriteManager;

        private CollisionManager _collisionManager;

        private Player _player;

        public TilingBackground background;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Get game's sprite batch.
        /// </summary>
        public SpriteBatch SpriteBatch => _spriteBatch;

        /// <summary>
        /// Get the game's Camera
        /// </summary>
        public Camera Camera => _camera;

        /// <summary>
        /// Get game's sprite manager
        /// </summary>
        public SpriteManager SpriteManager => _spriteManager;

        /// <summary>
        /// Get game's collision manager
        /// </summary>
        public CollisionManager CollisionManager => _collisionManager;

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height / 2;
            
            _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width / 2;
            
            _graphics.ApplyChanges();

            Components.Add(new KeyboardManager(this));
            
            _spriteManager = new SpriteManager(this);
            
            _camera = new Camera(this, worldWidth: 10f);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _collisionManager = new CollisionManager();

            SpriteManager.AddSpriteSheet("texture");

            SpriteManager.AddSpriteSheet("Fullgrass");

            _player = new Player(this, 1);

            background = new TilingBackground(this, "Fullgrass", new Vector2(3, 3)); ;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _player.Update(gameTime);
            //_player.LateUpdate(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            background.Draw(gameTime);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
           
            _player.Draw(_spriteBatch);
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
