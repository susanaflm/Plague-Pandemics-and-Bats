using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

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
        MainMenu, Highscores, Playing, Paused, ChooseName, ChooseCharacter, LoadingScreen, GameOver, WinGame
    }
    #endregion

    public class Game1 : Game
    {
        #region  Private variables
        private GraphicsDeviceManager _graphics;
        private SoundEffect _playSound;
        private Song _menuSong, _gameSong;
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont, _astronBoy;
        private Texture2D _pausedTexture, _hb;
        private Rectangle _pausedRect, _hbRec;
        private Camera _camera;
        private SpriteManager _spriteManager;
        private CollisionManager _collisionManager;
        private Player _player;
        private Cat _cat;
        private Dragon _dragon;
        private Scene _scene;
        private Scene _finalScene;
        private Button _buttonPlay, _buttonQuit, _guyButton, _girlButton, _highScoreButton, _optnButton, _creditsButton, _back2menuButton, _back4menuButton;
        private UI _ui;
        private List<Ammo> _ammoList;
        private List<Projectile> _projectiles;
        private List<Enemy> _enemies;
        private List<EnemyProjectile> _enemyProjectiles;
        private GameState _gameState = GameState.MainMenu;
        private int _highScore;
        private bool _wasGameLoaded;
        #endregion

        #region Public variables
        public TilingBackground background, finalscene;
        public List<int> fileHighscores = new List<int>();

        public bool hasPlayerTouchedBlueHouse = false;
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

        public SpriteFont SpriteFont => _astronBoy;

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
        /// Get game's cat 
        /// </summary>
        public Cat Cat => _cat;

        /// <summary>
        /// Get game's dragon 
        /// </summary>
        public Dragon Dragon => _dragon;

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
        /// Get Game's Enemy Projectiles
        /// </summary>
        public List<EnemyProjectile> EnemyProjectiles => _enemyProjectiles;

        /// <summary>
        /// Get Game's Ammo
        /// </summary>
        public List<Ammo> Ammo => _ammoList;
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

            _camera = new Camera(this, worldWidth: 9f);

            LoadHighScores();

            Player.SaveScore += () =>
            {
               SaveHighScore(_player.Highscore);
                _gameState = GameState.GameOver;
            };

            //LISTS
            _enemies = new List<Enemy>();
            _projectiles = new List<Projectile>();
            _enemyProjectiles = new List<EnemyProjectile>();
            _ammoList = new List<Ammo>();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _hb = Content.Load<Texture2D>("healthbar");
            
            //FONT
            _spriteFont = Content.Load<SpriteFont>("minecraft");
            _astronBoy = Content.Load<SpriteFont>("astronboy");

            //SOUNDS
            _playSound = Content.Load<SoundEffect>("playsound");
            _menuSong = Content.Load<Song>("menusong");
            _gameSong = Content.Load<Song>("gameSong");

            //PAUSE
            _pausedTexture = Content.Load<Texture2D>("pause");
            _pausedRect = new Rectangle(-1, 0, _pausedTexture.Width / 2, _pausedTexture.Height / 2);

            //COLLISION
            _collisionManager = new CollisionManager(this);

            #region SpriteSheets
            SpriteManager.AddSpriteSheet("NonTrimmedSheet");
            SpriteManager.AddSpriteSheet("TrimmedSheet");
            SpriteManager.AddSpriteSheet("Fullgrass");
            #endregion

            #region Buttons
            _buttonPlay = new Button(this, Content.Load<Texture2D>("play"), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 1.8f));
            _highScoreButton = new Button(this, Content.Load<Texture2D>("button"), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 1.46f));
            _buttonQuit = new Button(this, Content.Load<Texture2D>("quit"), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 1.23f));
            _optnButton = new Button(this, Content.Load<Texture2D>("optnButton"), new Vector2(GraphicsDevice.Viewport.Width / 1.05f, GraphicsDevice.Viewport.Height / 5.7f));
            _creditsButton = new Button(this, Content.Load<Texture2D>("creditsButton"), new Vector2(GraphicsDevice.Viewport.Width / 1.05f, GraphicsDevice.Viewport.Height / 3.3f));
            _back2menuButton = new Button(this, Content.Load<Texture2D>("mmButton"), new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 1.46f));
            _back4menuButton = new Button(this, Content.Load<Texture2D>("mmButton"), new Vector2(GraphicsDevice.Viewport.Width / 2, 480));
            _girlButton = new Button(this, Content.Load<Texture2D>("girlbutton"), new Vector2(190, 370));
            _guyButton = new Button(this, Content.Load<Texture2D>("guybutton"), new Vector2(770, 360));
            #endregion

            // GAME Songs
             MediaPlayer.Play(_menuSong);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.F1))
                Exit();

            MouseState mouseState = Mouse.GetState();

            switch (_gameState)
            {
                #region Main Menu
                case GameState.MainMenu:
                    IsMouseVisible = true;

                    if (_buttonPlay.isClicked || KeyboardManager.IsKeyGoingDown(Keys.Enter))
                    {
                        if (!_wasGameLoaded)
                        {
                            LoadLevel();
                            _wasGameLoaded = true;
                        }
                        else
                            ReloadLevel();

                        _gameState = GameState.ChooseCharacter;
                    }
                    if (_highScoreButton.isClicked)
                        _gameState = GameState.Highscores;
                    if (_buttonQuit.isClicked || KeyboardManager.IsKeyGoingDown(Keys.Escape))
                        Exit();

                    _buttonPlay.Update(mouseState, 0);
                    _highScoreButton.Update(mouseState, 0);
                    _buttonQuit.Update(mouseState, 0);
                    _optnButton.Update(mouseState, 1);
                    _creditsButton.Update(mouseState, 2);
                    break;
                #endregion

                #region Choose Character
                case GameState.ChooseCharacter:
                    IsMouseVisible = true;

                    if (_girlButton.isClicked)
                    {
                        _player.SetGender(0);
                        _gameState = GameState.ChooseName;
                    }

                    if (_guyButton.isClicked)
                    {
                        _player.SetGender(1);
                        _gameState = GameState.ChooseName;
                    }

                    _girlButton.Update(mouseState, 0);
                    _guyButton.Update(mouseState, 0);
                    break;
                #endregion

                #region Loading
                case GameState.LoadingScreen:

                    if (KeyboardManager.IsKeyGoingDown(Keys.Enter))
                    {
                        _gameState = GameState.Playing;
                        _playSound.Play();
                        MediaPlayer.Stop();
                        MediaPlayer.Play(_gameSong);
                    }

                    break;
                #endregion

                #region Name
                case GameState.ChooseName:
                    _ui.Update(gameTime);

                    if (KeyboardManager.IsKeyGoingDown(Keys.Enter))
                    {
                        _gameState = GameState.LoadingScreen;
                    }
                    break;
                #endregion

                #region Highscore
                case GameState.Highscores:
                    IsMouseVisible = true;

                    if (_back4menuButton.isClicked) _gameState = GameState.MainMenu;

                    _back4menuButton.Update(mouseState, 0);
                    break;
                #endregion

                #region Playing
                case GameState.Playing:
                    IsMouseVisible = false;

                    _hbRec = new Rectangle(790, 45, _player.Health, 20);

                    if (KeyboardManager.IsKeyGoingDown(Keys.Escape))
                    {
                        _gameState = GameState.Paused;

                        _buttonPlay.isClicked = false;
                    }

                    if (!hasPlayerTouchedBlueHouse)
                    {

                        foreach (Projectile p in Projectiles.ToArray())
                        {
                            p.Update(gameTime);
                        }

                        foreach (EnemyProjectile eP in EnemyProjectiles.ToArray())
                        {
                            eP.Update(gameTime);
                        }

                        foreach (Enemy e in Enemies.ToArray())
                        {
                            e.Update(gameTime);
                        }
                        foreach (Enemy e in Enemies.ToArray())
                        {
                            e.LateUpdate(gameTime);
                        }
                    }

                    foreach (Ammo a in Ammo.ToArray())
                    {
                        a.Update();
                    }

                    _player.Update(gameTime);
                    _cat.Update(gameTime);
                    _dragon.Update(gameTime);
                    _collisionManager.Update(gameTime);
                   
                    _player.LateUpdate(gameTime);
                    _cat.LateUpdate(gameTime);
                    _dragon.LateUpdate(gameTime);
                    break;
                #endregion

                #region Paused
                case GameState.Paused:
                    IsMouseVisible = true;
                    if (KeyboardManager.IsKeyGoingDown(Keys.Escape))
                        _gameState = GameState.Playing;
                    if (_buttonPlay.isClicked)
                        _gameState = GameState.Playing;
                    if (_buttonQuit.isClicked)
                    {
                        Exit();
                        _player.Die();
                    }
                       
                    if (_back2menuButton.isClicked)
                    {
                        _gameState = GameState.MainMenu;
                        SaveHighScore(_player.Highscore);
                        MediaPlayer.Stop();
                        MediaPlayer.Play(_menuSong);
                    }

                    //UPDATES BUTTONS
                    _buttonPlay.Update(mouseState, 0);
                    _buttonQuit.Update(mouseState, 0);
                    _back2menuButton.Update(mouseState, 0);
                    break;
                #endregion

                #region Game Over
                case GameState.GameOver:
                    IsMouseVisible = true;

                    if (_back4menuButton.isClicked)
                        _gameState = GameState.MainMenu;

                    _back4menuButton.Update(mouseState, 0);
                    break;
                #endregion

                #region Win Game
                case GameState.WinGame:
                    IsMouseVisible = false;

                    if (KeyboardManager.IsKeyGoingDown(Keys.Escape))
                        Exit();
                    break;
                default:
                    break;
                    #endregion
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

            #region Playing
            if (_gameState == GameState.Playing)
            {
                Texture2D texture = Content.Load<Texture2D>("icon");

                //CHARACTERS AND SCENE
                if (!hasPlayerTouchedBlueHouse)
                {
                    background.Draw(gameTime);
                    
                    _cat.Draw(_spriteBatch);
                    _dragon.Draw(_spriteBatch);

                    //DRAW ENEMIES & PROJECTILES
                    foreach (Ammo a in Ammo.ToArray())
                    {
                        a.Draw(_spriteBatch);
                    }

                    foreach (Projectile p in Projectiles.ToArray())
                    {
                        p.Draw(_spriteBatch);
                    }

                    foreach (EnemyProjectile eP in EnemyProjectiles.ToArray())
                    {
                        eP.Draw(_spriteBatch);
                    }

                    foreach (Enemy e in Enemies.ToArray())
                    {
                        e.Draw(_spriteBatch);
                    }

                }
                else
                {
                    LoadLevel2();
                    _finalScene.Draw(gameTime);
                }
                
                _scene.Draw(gameTime);
                _player.Draw(_spriteBatch);
                
                //DRAW THE TOP LEFT CORNER ICON
                _spriteBatch.Draw(texture, new Vector2(10, 45), Color.White);

                //DRAW TOP LEFT CORNER TEXTS
                _spriteBatch.DrawString(_astronBoy, $"X {Player.AmmoQuantity}", new Vector2(45, 45), Color.DarkSlateGray);
                _spriteBatch.DrawString(_astronBoy, $"LIVES {Player.Lives}", new Vector2(790, 10), Color.DarkSlateGray);
                _spriteBatch.DrawString(_astronBoy, $"SCORE {Player.Score}", new Vector2(10, 10), Color.DarkSlateGray);
                _spriteBatch.Draw(_hb, _hbRec, Color.White);
            }
            #endregion

            #region Paused
            if (_gameState == GameState.Paused)
            {
                //CHARACTERS AND SCENE
                if (!hasPlayerTouchedBlueHouse)
                {
                    background.Draw(gameTime);
                    _scene.Draw(gameTime);
                }
                else
                {
                    _finalScene.Draw(gameTime);
                }
                
                _player.Draw(_spriteBatch);
                _cat.Draw(_spriteBatch);

                //DRAWS ENEMIES
                foreach (Projectile p in Projectiles.ToArray())
                {
                    p.Draw(_spriteBatch);
                }

                foreach (EnemyProjectile eP in EnemyProjectiles.ToArray())
                {
                    eP.Draw(_spriteBatch);
                }

                foreach (Enemy e in Enemies.ToArray())
                {
                    e.Draw(_spriteBatch);
                }

                //PAUSE RECTANGLE
                Rectangle rec = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

                Pixel.DrawRectangle(rec, Color.Black * 0.5f);

                //DRAWS PAUSE BUTTON
                _spriteBatch.Draw(_pausedTexture, _pausedRect, Color.White);

                //DRAWS THE OTHER BUTTONS
                _buttonPlay.Draw(_spriteBatch, 0);
                _buttonQuit.Draw(_spriteBatch, 0);
                _back2menuButton.Draw(_spriteBatch, 0);
            }
            #endregion

            #region Main Menu
            if (_gameState == GameState.MainMenu)
            {
                Texture2D texture = Content.Load<Texture2D>("mainmenu");
                
                Rectangle rec = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                Color color = Color.White;

                //DRAWS MENU SCREEN
                _spriteBatch.Draw(texture, rec, color);

                //DRAWS THE BUTTONS WITHIN THE MENU
                _buttonPlay.Draw(_spriteBatch, 0);
                _buttonQuit.Draw(_spriteBatch, 0);
                _highScoreButton.Draw(_spriteBatch, 0);
            }
            #endregion

            #region Choose Character
            if (_gameState == GameState.ChooseCharacter)
            {
                Texture2D texture = Content.Load<Texture2D>("characterPickMenu");
              
                Rectangle rec = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                Color color = Color.White;

                //DRAWS CHARACTER PICK MENU
                _spriteBatch.Draw(texture, rec, color);

                //DRAWS THE BUTTONS FOR THE PLAYER TO CLICK
                _girlButton.Draw(_spriteBatch, 0);
                _guyButton.Draw(_spriteBatch, 0);

                //DRAWS TEXT
                _spriteBatch.DrawString(_spriteFont, "MARIA SOTO", new Vector2(100, 150), Color.LightBlue);
                _spriteBatch.DrawString(_spriteFont, "OLIVER BUCHANAN", new Vector2(630, 150), Color.LightBlue);
            }
            #endregion

            #region Loading
            if (_gameState == GameState.LoadingScreen)
            {
                Texture2D texture = Content.Load<Texture2D>("LoadingScreen");
                
                Rectangle rec = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                Color color = Color.White;

                //DRAWS LOADING SCREEN
                _spriteBatch.Draw(texture, rec, color);
            }
            #endregion

            #region Name
            if (_gameState == GameState.ChooseName)
            {
                Texture2D texture = Content.Load<Texture2D>("pickYourNameScreen");
                Rectangle rec = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

                //DRAWS PICK YOUR NAME SCREEN
                _spriteBatch.Draw(texture, rec, Color.White);

                //CALLS UI 
                _ui.Draw(_spriteBatch, gameTime);
            }

            #endregion

            #region Highscore
            if (_gameState == GameState.Highscores)
            {   
                Texture2D texture = Content.Load<Texture2D>("highscoreMenu");
                Vector2 vec;
                Rectangle rec = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                Color color = Color.White;
                
                string [] file = File.ReadAllLines($@"{Content.RootDirectory}/highscore.txt");
                int line = 20;
                
                _spriteBatch.Draw(texture, rec, color);
                _back4menuButton.Draw(_spriteBatch, 0);
                
                for (int i = 0; i < file.Length; i++)
                {
                    vec = new Vector2(i + GraphicsDevice.Viewport.Width/2.3f, i + GraphicsDevice.Viewport.Height / 2.56f);
                    string [] text = file [i].Split(';');

                    foreach( var t in text)
                    {
                        if(fileHighscores.Count < 5)
                        {
                            line += 25;
                            vec = new Vector2(i + GraphicsDevice.Viewport.Width / 2.3f, i + line + GraphicsDevice.Viewport.Height / 4f);
                        
                            _spriteBatch.DrawString(_spriteFont, $"{t}\n", vec, Color.White);           
                        }
                                   
                    }
                   
                }
            }
            #endregion

            #region Game Over
            if (_gameState == GameState.GameOver)
            {
                Texture2D go = Content.Load<Texture2D>("gameOver");
                Rectangle rec = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

                _spriteBatch.Draw(go, rec, Color.White);
                _back4menuButton.Draw(_spriteBatch, 0);
            }
            #endregion

            #region Win Game
            if (_gameState == GameState.WinGame)
            {
                Texture2D wg = Content.Load<Texture2D>("WinScreen");
                Rectangle rec = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

                _spriteBatch.Draw(wg, rec, Color.White);
            }
            #endregion

            _spriteBatch.End();

            base.Draw(gameTime);
            
        }

        /// <summary>
        /// Read high Scores from the file
        /// If file is not available HighScore is set to 0
        /// </summary>
        public void LoadHighScores()
        {
            try
            {
                string[] file = File.ReadAllLines($@"{Content.RootDirectory}/highscore.txt");
          
                if (file.Length != 0 && !int.TryParse(file[0], out _highScore))
                {
                    _highScore = 0;
                }
            }
            catch (FileNotFoundException)
            {
                _highScore = 0;
            }
        }

        /// <summary>
        /// When corona dies
        /// </summary>
        public void CoronaDied()
        {
            _gameState = GameState.WinGame;
        }

        /// <summary>
        /// Saves the New HighScore
        /// </summary>
        /// <param name="newHighScore">The new High Score to save</param>
        public void SaveHighScore(int newHighScore)
        {
            string path = Content.RootDirectory + "/highscore.txt";
            string text = _player.Name + ";" + newHighScore.ToString() + "\n";

            fileHighscores.Add(newHighScore);
                                 
            File.AppendAllText(path, text);
        }

        /// <summary>
        /// This Function loads the level sprites. Also it creates the player and some entities
        /// </summary>
        private void LoadLevel()
        {
            //CHARACTERS & SCENE
            _player = new Player(this);
            _cat = new Cat(this);
            _dragon = new Dragon(this);
            _scene = new Scene(this, "MainScene");            
            _ui = new UI(this);
            
            //BACKGROUND 
            background = new TilingBackground(this, "Fullgrass", new Vector2(4));
        }

        private void LoadLevel2()
        {
            _finalScene = new Scene(this, "FinalScene");
            background = new TilingBackground(this, "lab", new Vector2(4));
        }
        /// <summary>
        /// This method allows the game to reload level
        /// </summary>
        private void ReloadLevel()
        {
            if (_player.Score > _highScore)
            {
                _player.Highscore = _player.Score;
                SaveHighScore(_player.Highscore);
            }

            //Clear the lists
            _enemies.Clear();
            _projectiles.Clear();
            _enemyProjectiles.Clear();
            _scene.Sprites.Clear();
            _collisionManager.Clear();

            //Unload Components
            _player = null;
            _scene = null;
            _finalScene = null;
            _dragon = null;
            _ui = null;
            _cat = null;
            background = null;

            //Load the level Again
            LoadLevel();
        }
    }
}