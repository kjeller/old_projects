
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Viking_Warrior___Slutprojekt
{
    /// <summary>
    /// Extension that converts Point to Vector2
    /// </summary>
    public static class PointExt
    {
        /// <summary>
        /// Converts point to Vector2
        /// </summary>
        /// <param name="point">input Position using point</param>
        /// <returns></returns>
        public static Vector2 ToVector2(this Point point)
        {
            return new Vector2(point.X, point.Y);
        }
    }
    /// <summary>
    /// Main class
    /// </summary>
    public class Game1 : Game
    {
        private KeyboardState previousKeyboardState;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player; //player object
        Map map; //map object
        private SpriteFont font;

        Texture2D backgroundSprite; //Blue background
        Texture2D backgroundMountainSprite; //Mountains
        Texture2D playerSpawn; //Checkpoint coin
        Texture2D titleScreen; //Title screen
        Texture2D gameOver; //Gameover screen
        Texture2D winScreen; //Win screen

        private double timeleft = 60; //Initialize timeleft
        private int score = 0; //Initialize score
        int nextLevel = 0; //Intitialize level 1
        int[,] mapLevel;
        /// <summary>
        /// Checkpoint position
        /// </summary>
        private Vector2 pivPosition = Vector2.Zero;
        enum gameState 
        {
            titleScreen,
            gameStarted,
            gameEnded,
        }
        gameState currentGameState = gameState.titleScreen; //Default titleScreen
        /// <summary>
        /// Main class constructor
        /// </summary>
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = Globals.screenWidth;
            graphics.PreferredBackBufferHeight = Globals.screenHeight;
            graphics.IsFullScreen = false; // Changes if fullscreen or not
        }
        /// <summary>
        /// Initializes and creates the objects tied to respective classes on game start.
        /// </summary>
        /// <remarks>Initializes before game starts</remarks>
        protected override void Initialize()
        {
            map = new Map(); //Intialize map object
            player = new Player(); //Initialize player object 
            pivPosition = player.startPosition; //First pivPosition assigned by startPosition
            base.Initialize();
        }
        /// <summary>
        /// Method that chooses levels, generate next level and clears the game from previous maps
        /// </summary>
        protected void LoadLevel()
        {
            
            //Clears map from the previous map
            map = new Map();
            if (nextLevel == -1)
            {
                map.Generate(mapLevel = new int[,]
          {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, // (1 - Stone), (2 - Grass), (3 - Dirt), (4 - Coins)
          }, 70);
            }
            
            if(nextLevel == 0)
            {
               map.Generate(mapLevel = new int[,]
          {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,2},
                {4,0,0,0,2,0,0,2,0,0,0,0,0,0,0,0,2,2,0,0,0,0},
                {2,2,2,0,0,0,0,0,0,0,0,2,2,2,0,0,0,0,0,0,0,2},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,2,2,2,2,0,0,0,0,0,0,0,2},
                {0,0,0,0,0,0,0,0,0,2,3,3,3,3,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,2,3,1,1,1,1,0,0,0,0,0,0,0,2},
                {2,2,2,2,2,2,2,2,3,1,1,1,1,1,0,0,0,0,0,0,0,0},
                {3,3,3,3,3,3,3,3,1,1,1,1,1,1,0,0,0,0,0,0,2,0}, // (1 - Stone), (2 - Grass), (3 - Dirt), (4 - Checkpoints)
          }, 70);
            }
            if(nextLevel == 1)
            {
                map.Generate(mapLevel = new int[,]
          {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,2,0,0,2},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,2},
                {0,0,0,0,0,0,0,0,0,2,3,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,2,3,1,0,0,0,0,0,0,0,0,0,0,2},
                {2,2,2,2,2,2,2,2,3,1,1,0,0,0,0,0,0,0,0,0,0,0},
                {3,3,3,3,3,3,3,3,1,1,1,0,0,0,0,0,0,0,2,2,2,2}, // (1 - Stone), (2 - Grass), (3 - Dirt), (4 - Checkpoints)
          }, 70);
            }
            if (nextLevel == 2)
            {
                map.Generate(mapLevel = new int[,]
          {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,4,0,0,0,0,0,0,0,2,2,0},
                {0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,0,0,0,0,0,0,2},
                {0,0,0,0,0,0,0,0,0,0,0,3,1,1,3,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,3,1,1,3,0,0,0,0,0,0,2},
                {0,0,0,0,0,0,0,0,0,0,0,3,1,1,3,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,1,3,0,0,0,0,0,0,2},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2}, // (1 - Stone), (2 - Grass), (3 - Dirt), (4 - Checkpoints)
          }, 70);
            }
        }
        /// <summary>
        /// Resets game by assigning same values as the start
        /// </summary>
        protected void Reset()
        {
            pivPosition = new Vector2(200, 900);
            nextLevel = 0;
            score = 0;
            timeleft = 60;
        }
        /// <summary>
        /// LoadContent loads all the content and is only called once per game
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            backgroundSprite = Content.Load<Texture2D>("Sprites/background_2");
            backgroundMountainSprite = Content.Load<Texture2D>("Sprites/mountainRender3");
            playerSpawn = Content.Load<Texture2D>("Sprites/rendered_coin_small");
            titleScreen = Content.Load<Texture2D>("Sprites/titleScreen");
            gameOver = Content.Load<Texture2D>("Sprites/gameEnded");
            winScreen = Content.Load<Texture2D>("Sprites/winScreen");
            font = Content.Load<SpriteFont>("Sprites/Vikingfont");
            Tiles.Content = Content;
            player.Load(Content); //Loads content from player class -IMPORTANT-
            LoadLevel(); //Loads level
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world, gamestate, 
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            KeyboardState currentKeyboardState = Keyboard.GetState();
            previousKeyboardState = currentKeyboardState;

            if (currentGameState == gameState.titleScreen)
            {
                if (currentKeyboardState.IsKeyDown(Keys.Enter))  // If the user presses "Enter", the user will be taken directly to the game
                {
                    currentGameState = gameState.gameStarted; //The gameState changes to gameStarted and the game will start
                }
            }
            if(currentGameState == gameState.gameStarted) //When player dies the game logic óf the player is not used
            {
                timeleft = (timeleft - gameTime.ElapsedGameTime.TotalSeconds);
                //Gives every tile collision
                for (int i = 0; i < map.CollisionTiles.Count; i++)
                {
                    player.UpdateCollisionTile(map.CollisionTiles[i].Rectangle, map.Width, map.Height);
                }
                //Gives the checkpoint collision and checking if player is ontop of it
                for (int i = 0; i < map.Coins.Count; i++)
                {
                    if (player.Rectangle.onTop(map.Coins[i].Rectangle))
                    {
                        pivPosition = map.Coins[i].Rectangle.Location.ToVector2(); //Assigns checkpoint position to pivPoint, from a converted Point => Vector2 position
                        player.startPosition = pivPosition; //If player falls down from map. Player will be teleported back to startposition.
                        map.Coins[i].Rectangle = new Rectangle(); //Removes checkpoint
                        nextLevel++; //change level value
                        LoadLevel(); //change level
                        score = score + 50; //gives player 50 points per level
                        timeleft = timeleft + 30; //gives player extra time
                        
                    }
                }
                player.Update(gameTime); //Method from the player class which updates all the methods involved with the player
                //When time is up
                if (timeleft <= 0)
                {
                    player.StateDead = true;//player dies
                }

                //Selection statements that ends the game
                if (player.StateDead == true || (player.StateDead == false && nextLevel >= 3))
                {
                    currentGameState = gameState.gameEnded;
                }
                
            }
            //Restart game
            if(currentGameState == gameState.gameEnded && currentKeyboardState.IsKeyDown(Keys.Enter))
            {
                player.Reset(); //Resets player positions and state
                Reset(); //Resets time and score
                LoadLevel(); //Reload level
                currentGameState = gameState.gameStarted;
            }
            base.Update(gameTime);
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            if (currentGameState == gameState.titleScreen)  // Decides if the game should be on titlescreen or not. If it's the latter, the game starts
            {
                spriteBatch.Begin();
                spriteBatch.Draw(titleScreen, new Vector2(0, 0));
                spriteBatch.End();
            }
            if (nextLevel >= 0 && currentGameState == gameState.gameStarted || currentGameState == gameState.gameEnded) 
            {
                spriteBatch.Begin();
                spriteBatch.Draw(backgroundSprite, new Vector2(0, 0)); //Draw the background image
                spriteBatch.Draw(backgroundMountainSprite, new Vector2(0, 0)); //Draw background mountains
                spriteBatch.Draw(playerSpawn, pivPosition, Color.White);
                map.Draw(spriteBatch); //Draw the map tiles
                player.Draw(spriteBatch);
                spriteBatch.DrawString(font, "Time: " + (int)timeleft + " s", new Vector2(Globals.screenWidth - 300, 0), Color.White); //Draws the gametime for the player
                spriteBatch.DrawString(font, "Score: " + (int)score + "", new Vector2(Globals.screenWidth - 600, 0), Color.White); //Draws the score
                spriteBatch.DrawString(font, "Level: " + (nextLevel + 1), new Vector2(Globals.screenWidth - 900, 0), Color.White); //Shows the player the current level
                if (currentGameState == gameState.gameEnded && player.StateDead == false) //Player wins
                {
                    spriteBatch.Draw(winScreen, Vector2.Zero);
                    spriteBatch.DrawString(font, "" + (int)(score + timeleft) + " (score + time)", new Vector2(Globals.screenWidth - 830, 750), Color.Yellow); //Draws the final score on game over screen
                    spriteBatch.DrawString(font, "0", new Vector2(Globals.screenWidth - 690, 640), Color.Yellow); // Will replace with actual enemy killed counter - TODO
                }
                else if(currentGameState == gameState.gameEnded && player.StateDead == true) //Player loses
                {
                    spriteBatch.Draw(gameOver, Vector2.Zero);
                    spriteBatch.DrawString(font, "" + score, new Vector2(Globals.screenWidth - 830, 750), Color.Red); //Draws the score on game over screen
                    spriteBatch.DrawString(font, "0", new Vector2(Globals.screenWidth - 690, 640), Color.Red); // Will replace with actual enemy killed counter - TODO
                }
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}
