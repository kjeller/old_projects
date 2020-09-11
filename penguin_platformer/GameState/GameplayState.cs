using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace Platformer_Slutprojekt
{
    class GameplayState : GameState
    {
        // Player is able to move with keyboard input
        Player player;

        // Used to create level
        TileManager tileManager;

        SpriteBatch spriteBatch;

        // List where enemies will be added
        List<Enemy> enemies;

        int currentLevel = 0;

        // Play level based on file path to txt file
        public string filePath;

        // When game is paused there is a retry button
        bool isPaused;

        // Will use to decide if game is loaded or not
        bool isMapLoaded;

        // Will enable debug information when true
        bool displayDebugInformation = false;

        public GameplayState(string filePath, int currentLevel)
        {
            this.filePath = filePath;
            this.currentLevel = currentLevel;
            isMapLoaded = false;
        }

        /// <summary>
        /// Will toggle the display of debug information
        /// </summary>
        void ToggleDebugInformation()
        {
            if (!displayDebugInformation)
                displayDebugInformation = true;
            else
                displayDebugInformation = false;
        }

        #region Initialize

        /// <summary>
        /// Resets every entity placed in map
        /// </summary>
        void ResetGame()
        {
            player.Reset();
            foreach (Enemy enemy in enemies)
                enemy.Reset();
            isPaused = false;
        }

        public override void Initialize()
        {
            gameStateManager.Game.IsMouseVisible = false;

            // Create new GUI manager that enables creating of buttons
            GUI = new GUIManager();

            // filePath defaults level if currentlevel is not -1
            string level = filePath;

            // This is used to decide if player wants to play campaign which changes current level
            if (File.Exists(string.Format(filePath + currentLevel.ToString() + ".txt")) && currentLevel > -1)
            {
                level = string.Format(filePath + currentLevel.ToString() + ".txt");
            }
            else if(currentLevel < 0) {  } // Do nothing since level path is assigned before
            // Returns to main menu
            else
            {
                gameStateManager.AddState(new MainMenuState());
                gameStateManager.ChangeState(typeof(MainMenuState));
                
                // Returns to gameStateManager
                return;
            }

            // Only load these if map wasn't already loaded
            if (!isMapLoaded)
            {
                // Initializes camera
                camera = new Camera2D(gameStateManager.GraphicsDevice.Viewport);

                // Tile manager uses screen size to determine where to place tiles
                tileManager = new TileManager(level);

                // New list of enemies
                enemies = new List<Enemy>();

                // Initializes new player and places player on spawn position
                player = new Player("Kjelle", TileManager.playerSpawnTile);

                // Adds enemy read from tilemanger based on type
                for (int i = 0; i < tileManager.enemiesFromRead.Count; i++)
                {
                    // Creates enemy with based on type from tilemanager
                    // And use constructor to initialize enemy.
                    object enemy = Activator.CreateInstance(
                        tileManager.enemiesFromRead[i].enemyType,
                        new Object[] { "Enemy", tileManager.enemiesFromRead[i].position});

                    enemies.Add((Enemy)enemy);
                }
                isMapLoaded = true;
            }

            // Creates new button and lets player retry which starts a new game
            GUI.CreateButton(
                "Retry",
                gameStateManager.ubuntuFont,
                gameStateManager.defaultButtonTexture,
                gameStateManager.defaultHoverButtonTexture,
                new Vector2(500, 50),
                new Vector2(
                    player.startPosition.X,
                    gameStateManager.Game.GraphicsDevice.DisplayMode.Height / 2 + 100),
                delegate {
                    this.ResetGame();
                    return;
                });

            isPaused = false;

            LoadContent();
        }

        public override void LoadContent()
        {
            ContentManager Content = gameStateManager.Game.Content;

            if(tileManager.endOfStageTile != null)
                // Loads tile that changes level
                tileManager.endOfStageTile.LoadContent(Content);

            tileManager.LoadContent(Content);

            player.LoadContent(gameStateManager.playerTexture);

            // Loads texture for every enemy
            foreach(Enemy enemy in enemies)
            {
                enemy.LoadContent(gameStateManager.enemyTexture);
            }

            // Apply Jumper enemy texture to enemies with type Jumper
            // this will override previous texture
            foreach(Jumper enemy in enemies.OfType<Jumper>())
            {
                enemy.LoadContent(gameStateManager.enemyJumperTexture);
            }

            // State is now ready to update
            isLoaded = true;
        }

        #endregion


        #region Update and Draw

        float timer = 2;
        float collisionTimer = 2;

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            collisionTimer -= elapsed;

            // Hides mouse
            gameStateManager.Game.IsMouseVisible = false;

            if (isPaused)
            {
                gameStateManager.Game.IsMouseVisible = true;
                
            }
            // Handle input
            HandleMouseInput();

            HandleKeyboardInput();

            // Pause game if player is dead
            if (!player.isAlive)
                isPaused = true;

            // Toggles debug information
            if (keyboardState.IsKeyDown(Keys.N) && oldKeyboardState.IsKeyUp(Keys.N))
                ToggleDebugInformation();

            // Changes state to mainmeny
            if (keyboardState.IsKeyDown(Keys.Escape) && oldKeyboardState.IsKeyUp(Keys.Escape))
            {
                gameStateManager.ChangeState(typeof(MainMenuState));
            }

            // Update game if game is not paused
            if(!isPaused)
            {
                // Checks collision between enemy and player every 2 millisecond
                if (collisionTimer < 0)
                {
                    // Update collision between player and enemies
                    foreach (Enemy enemy in enemies)
                    {
                        player.collisionHandler.CollisionPlayerEnemy(player, enemy);
                    }
                    collisionTimer = timer;
                }

                // Handles input
                player.UpdateInput(oldKeyboardState, keyboardState);

                // Updates player inputs, movement and collision
                player.Update(gameTime);

                // Update enemies
                foreach (Enemy enemy in enemies)
                {
                    enemy.Update(gameTime);
                }

                // Updates camera position to player position
                camera.Update(player);

                // Makes sure tile is there
                if (tileManager.endOfStageTile != null)

                    // If tile is there and player collides, end stage and change level
                    if (player.rectangle.Intersects(tileManager.endOfStageTile.rectangle))
                        gameStateManager.ChangeLevel();

            }
            base.Update(gameTime);
        }

        // Used for debugging game
        int frameCounter; // Incremental, counts frames
        int fps; // Displays current fps where 60 is max

        public override void Draw(GameTime gameTime)
        {
            double elapsedTime = gameTime.ElapsedGameTime.TotalSeconds;
            frameCounter++;

            // If time has moved I can know fps of drawed game
            if(elapsedTime > 0)
            {
                // Get frames per seconds
                fps = (int)(1 / elapsedTime);
            }
            
            // Get spritebatch from gamestatemanager no need to create new one
            SpriteBatch spriteBatch = gameStateManager.Game.Services.GetService<SpriteBatch>();

            string framesPerSecond = string.Format("FPS: {0}", fps);
            string score = string.Format("Score: {0}", player.obtainedScore.ToString());

            // Position where to place debug information
            Vector2 debugInformationPosition = camera.position - new Vector2((gameStateManager.GraphicsDevice.DisplayMode.Width / 2) - 10, gameStateManager.GraphicsDevice.DisplayMode.Height / 2);

            var viewMatrix = camera.GetViewMatrix();

            // Start spritebatch
            spriteBatch.Begin(transformMatrix: viewMatrix);

            if (tileManager.endOfStageTile != null)
            {
                tileManager.endOfStageTile.Draw(spriteBatch);
            }

            // Draws every tile
            tileManager.Draw(spriteBatch);
            
            // Draw player
            player.Draw(spriteBatch);

            // Draw every enemy in list of enemies
            foreach(Enemy enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }

            #region Debug Drawing

            // Enables display of debuginformation
            // Show if entity collides with tiles and draws rectangle
            if (displayDebugInformation)
            {
                // Draws only the tiles player collides with
                player.DrawTilesCollidedWithEntity(spriteBatch, gameStateManager.GraphicsDevice);

                // Draws collision on player. Debug only
                player.DrawRectangle(spriteBatch, gameStateManager.whiteRectangle);

                foreach(Enemy enemy in enemies)
                {
                    enemy.DrawRectangle(spriteBatch, gameStateManager.whiteRectangle);
                    enemy.DrawTilesCollidedWithEntity(spriteBatch, gameStateManager.GraphicsDevice);
                }

                // Displays debug information position, velocity and acceleration of player
                spriteBatch.DrawString(GameStateManager.defaultFont, player.ToString(), debugInformationPosition, Color.White);

                // Print fps below player information
                spriteBatch.DrawString(GameStateManager.defaultFont, framesPerSecond, debugInformationPosition + new Vector2(0, GameStateManager.defaultFont.MeasureString(player.ToString()).Y), Color.White);

                spriteBatch.DrawString(GameStateManager.defaultFont, string.Format("IsPaused: " + isPaused.ToString()), debugInformationPosition + new Vector2(0, GameStateManager.defaultFont.MeasureString(player.ToString()).Y + 20), Color.White);

                // Prints information about every enemy in list on next to their rectangle

                foreach (Enemy enemy in enemies)
                {
                    if(enemy.isAlive)
                        spriteBatch.DrawString(GameStateManager.defaultFont, enemy.ToString(), new Vector2(enemy.rectangle.Right + 10, enemy.rectangle.Y), Color.White);
                }
            }
            else
            {
                spriteBatch.DrawString(GameStateManager.defaultFont, score, debugInformationPosition + new Vector2(10, 10), Color.White);
            }

#endregion

            // Draw retry button i game is paused
            if (isPaused)
            {
                GUI.Draw(spriteBatch);
                if(displayDebugInformation)
                {
                    GUI.DrawArea(spriteBatch, gameStateManager.whiteRectangle);
                    spriteBatch.Draw(gameStateManager.whiteRectangle, new Rectangle((int)mousePosition.X, (int)mousePosition.Y, 10, 10), Color.White);
                }   
            }

            // End spritebatch
            spriteBatch.End();
        }

        #endregion
    }
}
