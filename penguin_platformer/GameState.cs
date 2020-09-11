using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Platformer_Slutprojekt
{
    /// <summary>
    /// A gamestate is a layer that will be updated and drawed. A gamestate only works through a GameStateManager.
    /// </summary>
    abstract class GameState
    {
        public GUIManager GUI = new GUIManager();
        public Camera2D camera;
        public KeyboardState keyboardState;
        public KeyboardState oldKeyboardState;

        public Vector2 mousePosition;
        public MouseState mouseState;
        public MouseState oldMouseState;

        // Will be used to decide when to load content from state
        public bool isLoaded = false;

        // Manages changes to game states. Remove, Change and Add states.
        public GameStateManager gameStateManager;

        #region Initialization

        /// <summary>
        /// Initializes state and will load content
        /// </summary>
        public virtual void Initialize()
        {
            LoadContent();
        }

        /// <summary>
        /// Loads graphics and spritefonts
        /// </summary>
        public virtual void LoadContent() { }

        #endregion

        #region Input


        /// <summary>
        /// Sets mouse input, updates mouse state and get position of mouse
        /// </summary>
        public void HandleMouseInput()
        {
            // Gets old input from mouse
            // Makes sure buttons will be pressed once
            oldMouseState = mouseState;

            // Gets input from mouse
            mouseState = Mouse.GetState();

            // Gets mouse position used for checking mouse presses on buttons
            mousePosition = new Vector2(mouseState.X, mouseState.Y);

            // When game uses camera it needs to translate the mouse position to world
            if(camera != null)
            {
                mousePosition = Vector2.Transform(mousePosition, Matrix.Invert(camera.GetViewMatrix()));
            }
        }

        /// <summary>
        /// Sets keyboard input and updates keyboard state
        /// </summary>
        public void HandleKeyboardInput()
        {
            // Gets old input from keyboard
            // Makes sure keys will be pressed once
            oldKeyboardState = keyboardState;

            // Gets input from keyboard
            keyboardState = Keyboard.GetState();
        }

        /// <summary>
        /// Handle mouse input on every assigned button in list
        /// </summary>
        public void UpdateButtons()
        {
            
            // Makes sure button is not null
            if (GUI.buttons != null)
            {
                // Updates every button in list
                foreach (Button button in GUI.buttons)
                {
                    button.Update(mousePosition, mouseState, oldMouseState);
                }
            }
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            // Updates buttons
            UpdateButtons();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Draw(GameTime gameTime) {}

        #endregion
    }
}
