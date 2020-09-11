using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Platformer_Slutprojekt
{
    class Button
    {
        
        public string text { get; private set; }
        Vector2 size;
        public Vector2 position;

        Texture2D backgroundTexture; // backgroundTexture changes based on mouse inputs, hover/no hover
        Texture2D defaultTexture; // default, when button is not touched
        Texture2D hoverTexture; // texture when hovering button

        SpriteFont font;

        Rectangle area;
        Action action;

        /// <summary>
        /// Creates a new button
        /// </summary>
        /// <param name="text">Text that will appear inside button</param>
        /// <param name="font">Font used for text</param>
        /// <param name="size">Size of button</param>
        /// <param name="position">Position of button</param>
        public Button(string text, SpriteFont font, Texture2D defaultTexture, Texture2D hoverTexture, Vector2 size, Vector2 position, Action action)
        {
            this.text = text;
            this.size = size;
            this.position = position;
            this.font = font;
            this.defaultTexture = defaultTexture;
            this.hoverTexture = hoverTexture;
            this.action = action;
            backgroundTexture = defaultTexture;
            

            // If size is not assigned default the size of the button to be size of spritefont string
            if(size == null)
                size = font.MeasureString(text);

            // Creates area where button can be pressed
            area = new Rectangle((int)this.position.X - (int)size.X / 2, (int)this.position.Y - (int)size.Y / 2, (int)size.X, (int)size.Y);
        }

        /// <summary>
        /// Sets event from action assigned from method.
        /// Used to assign unassigned buttons
        /// </summary>
        /// <param name="action"></param>
        public void SetEvent(Action action)
        {
            this.action = action;
        }

        /// <summary>
        /// Runs event specified by SetEvent()
        /// </summary>
        public void RunEvent()
        {
            if(action != null)
                action();
        }

        /// <summary>
        /// Checks for mouse input
        /// </summary>
        /// <param name="mousePosition"></param>
        public void Update(Vector2 mousePosition, MouseState mouseState, MouseState oldMouseState)
        {
           // Checks if mouse is hovering button area
           if(area.Contains(mousePosition))
            {
                // Change backgroundtexture to hovertexture
                backgroundTexture = hoverTexture;

                // If left mousebutton is clicked, run event tied to button
                if(mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
                {
                    // Run method 
                    RunEvent();
                }
            }
           else
            // Use default texture
            backgroundTexture = defaultTexture;
        }

        /// <summary>
        /// Draws background and text on button
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Draws background texture of button
            spriteBatch.Draw(
                backgroundTexture,
                new Rectangle((int)position.X - (int)size.X / 2, (int)this.position.Y - (int)size.Y / 2, (int)size.X,
                (int)size.Y),
                Color.White);

            // Draw text 
            spriteBatch.DrawString(
                font, 
                text, 
                new Vector2(
                    position.X - font.MeasureString(text).X / 2, 
                    position.Y - font.MeasureString(text).Y / 2), 
                Color.White);
        }

        /// <summary>
        /// Draws area of button where mouse clicks will be registered
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="texture"></param>
        public void DrawArea(SpriteBatch spriteBatch, Texture2D texture)
        {
            // Draw rectangle based on rectangle position
            spriteBatch.Draw(texture, area, Color.White);
        }
    }
}
