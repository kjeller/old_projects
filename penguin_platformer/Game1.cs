using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Platformer_Slutprojekt
{

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        GameStateManager gameStateManager;

        public Game1()
        {
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);

            // Creates a new gamestate manager and adds to component to be able to use draw and update methods
            gameStateManager = new GameStateManager(this);
            Components.Add(gameStateManager);

            // Adds initial states into a list.
            AddInitialState();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Loads Content for game
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            graphics.PreferredBackBufferHeight = 1080; // Height of game window
            graphics.PreferredBackBufferWidth = 1920; // Width of game window
            graphics.ApplyChanges();
            graphics.ToggleFullScreen(); // Toggle fullscreen*/
            //Window.IsBorderless = true;

            //Mouse.WindowHandle = Window.Handle;
            
        }

        /// <summary>
        /// Adds intial game state. Should start with main menu.
        /// </summary>
        void AddInitialState()
        {
            gameStateManager.AddState(new MainMenuState());
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}
