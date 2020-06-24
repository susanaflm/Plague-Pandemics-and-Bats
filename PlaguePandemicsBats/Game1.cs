using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace PlaguePandemicsBats
{
    #region Enumerators
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

    /// <summary>
    /// Used to know in Which State the game is in
    /// </summary>
    public enum GameState
    {
        MainMenu, Options, Highscores, Playing, Paused
    }
    #endregion

    public class Game1 : Game
    {
        #region  private variables
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;
        private Texture2D _pausedTexture;
        private Rectangle _pausedRect;
        private Camera _camera;
        private SpriteManager _spriteManager;
        private CollisionManager _collisionManager;
        private Player _player;
        private Bat _bat;
        private Cat _cat;
        private Button _buttonPlay, _buttonQuit, _guyButton, _girlButton;
        private UI _ui;
        private List<Projectile> _projectiles;
        private List<Enemy> _enemies;
        private List<Cat> _friendlies;
        private List<Button> _buttons;
        private bool _paused = false;
        private GameState _gameState = GameState.MainMenu;
        #endregion

        #region Public variables
        public TilingBackground background;
        #endregion

        #region Constructor
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        #endregion

        #region Properties
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
        /// Get game's player 
        /// </summary>
        public Player Player => _player;

        /// <summary>
        /// Get game's UI
        /// </summary>
        public UI UI => _ui;

        /// <summary>
        /// Get game's Projectiles
        /// </summary>
        public List<Projectile> Projectiles => _projectiles;

        /// <summary>
        /// Get game's enemies
        /// </summary>
        public List<Enemy> Enemies => _enemies;

        /// <summary>
        /// Get game's player friendlies
        /// </summary>
        public List<Cat> Friendlies => _friendlies;

        /// <summary>
        /// Get Game's Button
        /// </summary>
        public List<Button> Buttons => _buttons;
        #endregion

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

            _camera = new Camera(this, worldWidth: 8f);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _spriteFont = Content.Load<SpriteFont>("minecraft");
            _collisionManager = new CollisionManager();

            SpriteManager.AddSpriteSheet("texture");
            SpriteManager.AddSpriteSheet("Fullgrass");

            /*PAUSE STUFF*/
            _pausedTexture = Content.Load<Texture2D>("pause");
            _pausedRect = new Rectangle(-1, 0, _pausedTexture.Width / 2, _pausedTexture.Height / 2);

            //buttons 
            _buttonPlay = new Button(this, Content.Load<Texture2D>("play"), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2));
            _buttonQuit = new Button(this, Content.Load<Texture2D>("quit"), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 1.5f));
            _guyButton = new Button(this, Content.Load<Texture2D>("guybutton"), new Vector2(3, 0));
            _girlButton = new Button(this, Content.Load<Texture2D>("girlbutton"), new Vector2(-1, 0));
         
            _ui = new UI(this);
            _player = new Player(this, 1);
            _bat = new Bat(this);
            _cat = new Cat(this);
            /*LISTS*/
            _enemies = new List<Enemy>();
            _friendlies = new List<Cat>();
            _buttons = new List<Button>();
            _projectiles = new List<Projectile>();
            /*ADDING TO LISTS*/
            _enemies.Add(_bat);
            _friendlies.Add(_cat);
            _buttons.Add(_buttonPlay);
            _buttons.Add(_buttonQuit);

            background = new TilingBackground(this, "Fullgrass", new Vector2(4, 3)); ;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            IsMouseVisible = true;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.F1))
                Exit();

            MouseState mouseState = Mouse.GetState();

            switch (_gameState)
            {
                case GameState.MainMenu:
                    if (_buttonPlay.isClicked)
                        _gameState = GameState.Playing;

                    _buttonPlay.Update(gameTime);

                    break;
                case GameState.Highscores:
                    break;
                case GameState.Options:
                    break;
                case GameState.Playing:
                    if (KeyboardManager.IsKeyGoingDown(Keys.Escape))
                    {
                        _gameState = GameState.Paused;

                        _buttonPlay.isClicked = false;
                    }

                    _ui.Update(gameTime);
                    _player.Update(gameTime);
                    _collisionManager.Update(gameTime);
                    _player.LateUpdate(gameTime);

                    foreach (Projectile p in Projectiles.ToArray())
                    {
                        p.Update(gameTime);
                    }

                    foreach (Enemy e in Enemies.ToArray())
                    {
                        e.Update(gameTime);
                        e.LateUpdate(gameTime);
                    }

                    foreach (Cat c in Friendlies.ToArray())
                    {
                        c.Update(gameTime);
                        c.LateUpdate(gameTime);
                    }
                    break;

                case GameState.Paused:

                    if (KeyboardManager.IsKeyGoingDown(Keys.Escape))
                        _gameState = GameState.Playing;
                    if (_buttonPlay.isClicked)
                        _gameState = GameState.Playing;
                    if (_buttonQuit.isClicked)
                        Exit();

                    _buttonPlay.Update(mouseState);
                    _buttonQuit.Update(mouseState);
                    break;
                default:
                    break;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            if (_gameState == GameState.Playing)
            {
                _ui.Draw(_spriteBatch, gameTime);

                background.Draw(gameTime);

                _player.Draw(_spriteBatch);

                foreach (Projectile p in Projectiles.ToArray())
                {
                    p.Draw(_spriteBatch);
                }

                foreach (Enemy e in Enemies.ToArray())
                {
                    e.Draw(_spriteBatch);
                }

                foreach (Cat c in Friendlies.ToArray())
                {
                    c.Draw(_spriteBatch);
                }
            }

            if (_gameState == GameState.Paused)
            {
                _spriteBatch.Draw(_pausedTexture, _pausedRect, Color.White);

                foreach (Button b in Buttons.ToArray())
                {
                    b.Draw(_spriteBatch);
                }
            }

            if (_gameState == GameState.MainMenu)
            {
                //main menu ratio screen size
                float widthRatio = GraphicsDevice.Viewport.Width / 900;
                float heightRatio = GraphicsDevice.Viewport.Height / 620;

                _spriteBatch.Draw(Content.Load<Texture2D>("mainmenu"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            }

            if (_gameState == GameState.Options)
            {
                _girlButton.Draw(gameTime);
                _guyButton.Draw(gameTime);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
