using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Platformer_Slutprojekt
{
    /// <summary>
    /// The GameStateManager is a component that will create new gamestates that handles their own update and draw
    /// methods. GameStateManager makes sure to switch between states when needed to.
    /// </summary>
    class GameStateManager : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        ContentManager Content;

        int currentState = 0;
        int currentLevel = 0;

        public static SpriteFont defaultFont;
        public SpriteFont ubuntuFont;
        public Texture2D defaultButtonTexture;
        public Texture2D defaultHoverButtonTexture;
        public Texture2D greenButtonTexture;
        public Texture2D greenHoverButtonTexture;
        public Texture2D playerTexture;
        public Texture2D enemyTexture;
        public Texture2D enemyJumperTexture;
        public Texture2D whiteRectangle;

        // List of states
        List<GameState> states = new List<GameState>();

        // Construcs a new gamstatemanager component
        public GameStateManager(Game game): base(game) {}

        public override void Initialize()
        {
            base.Initialize();

        }

        #region Alter States

        /// <summary>
        /// Will add states to list of states. Topmost list is active.
        /// </summary>
        /// <param name="state">Gamestate</param>
        public void AddState(GameState state)
        {
            // Adds state manager to state
            state.gameStateManager = this;

            // Adds to list of states
            states.Add(state);
        }

        /// <summary>
        /// Change between states
        /// </summary>
        /// <param name="state"></param>
        public void ChangeState(Type type)
        {
            // Searches for type in list of states
            int temp = states.FindIndex(x => x.GetType() == type);
            
            // if type exists change state
            if (!(temp == -1))
                currentState = temp;
        }
        
        public void ChangeLevel()
        {
            // Removes current state, which is gameplay state
            states.RemoveAt(states.FindIndex(x => x.GetType() == typeof(GameplayState)));
            currentLevel++;
            AddState(new GameplayState("Levels/level_", currentLevel));
        }

        /// <summary>
        /// Removes state from list based on type
        /// </summary>
        /// <typeparam name="T">type of GameState</typeparam>
        /// <param name="type"></param>
        public void Remove<T>(T type)
        {
            states.RemoveAll(x => x.GetType() == typeof(T));
        }

        #endregion

        protected override void LoadContent()
        {
            Content = Game.Content;
                        
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Loads textures and fonts that will be used by other classes
            // This makes sure textures load only once.
            defaultFont = Content.Load<SpriteFont>("GUI/Fonts/Debug");
            ubuntuFont = Content.Load<SpriteFont>("GUI/Fonts/Ubuntu");
            defaultButtonTexture = Content.Load<Texture2D>("GUI/Buttons/default_button");
            defaultHoverButtonTexture = Content.Load<Texture2D>("GUI/Buttons/hover_button");
            greenButtonTexture = Content.Load<Texture2D>("GUI/Buttons/default_green_button");
            greenHoverButtonTexture = Content.Load<Texture2D>("GUI/Buttons/hover_green_button");

            playerTexture = Content.Load<Texture2D>("Player/penguin_64x80");
            enemyTexture = Content.Load<Texture2D>("Enemy/60x120_default_enemy_sprite");
            enemyJumperTexture = Content.Load<Texture2D>("Enemy/red_penguin_64x80");

            // Creates new texture with graphicsdevice
            // Sets color of texture to white
            whiteRectangle = new Texture2D(this.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            whiteRectangle.SetData<Color>(new Color[] { Color.White });

            // Instead of passing down the spritebatch and graphicsdevice
            // I can add them to services and use them whenever I want
            Game.Services.AddService<SpriteBatch>(spriteBatch);
            Game.Services.AddService<GraphicsDevice>(GraphicsDevice);

            base.LoadContent();
        }
  
        /// <summary>
        /// Updates state first in position of list.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (states[currentState].isLoaded)
                states[currentState].Update(gameTime);     
        }

        /// <summary>
        /// Draws state first in position of list
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (states[currentState].isLoaded)
                states[currentState].Draw(gameTime);
            else
                states[currentState].Initialize();
        }
    }
}
