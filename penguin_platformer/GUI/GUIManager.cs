using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer_Slutprojekt
{
    class GUIManager
    {
        // GUI, buttons that are clickable and tied to specific events
        public List<Button> buttons = new List<Button>(); // List where buttons are added

        // These variables are used on buttons when having buttons in a row
        Vector2 btnOffset; // Offset that will be used between buttons in row
        Vector2 btnRowPosition; // Position of button
        Vector2 btnRowSize; // Size of button
        Texture2D btnRowDefaultTexture;
        Texture2D btnRowHoverTexture;
        public SpriteFont btnRowFont;

        /// <summary>
        /// Establish default values to be used when creating buttons in row
        /// </summary>
        /// <param name="font">Font used for every button</param>
        /// <param name="defaultTexture">Texture used for every button</param>
        /// <param name="hoverTexture">Texture to be used when hovering</param>
        /// <param name="position">Position of row</param>
        /// <param name="size">Size of a button</param>
        /// <param name="offset">Offset between buttons</param>
        public void CreateRow(SpriteFont font, Texture2D defaultTexture, Texture2D hoverTexture, Vector2 position, Vector2 size, Vector2 offset)
        {
            this.btnRowFont = font;
            this.btnRowDefaultTexture = defaultTexture;
            this.btnRowHoverTexture = hoverTexture;
            this.btnRowPosition = position;
            this.btnRowSize = size;
            this.btnOffset = offset;
        }

        /// <summary>
        /// Creates a button in row of buttons. Once used, row is established. Used for menu.
        /// </summary>
        /// <param name="text">Text that will be used in button</param>
        /// <param name="font">Font that text will use when drawing button</param>
        /// <param name="defaultTexture">Texture on button</param>
        /// <param name="hoverTexture">Texture on button when hovering</param>
        /// <param name="size">Size of button</param>
        /// <param name="position">Position of cluster of buttons</param>
        /// <param name="offset">Offset between buttons in row</param>
        /// <param name="action">Method that will be used when button is clicked</param>
        public void CreateButtonInRow(string text, Texture2D defaultTexture, Texture2D hoverTexture, Action action)
        {
            if (defaultTexture == null && hoverTexture == null)
            {
                defaultTexture = btnRowDefaultTexture;
                hoverTexture = btnRowHoverTexture;
            }

            // Adds new button to list
            buttons.Add(
                new Button(
                    text, 
                    btnRowFont, 
                    defaultTexture,
                    hoverTexture, 
                    btnRowSize, 
                    btnRowPosition, 
                    action));

            // Creates offset between every button
            // Size makes sure that no overlapping exists from offset
            btnRowPosition.Y += btnOffset.Y + btnRowSize.Y;
        }

        /// <summary>
        /// Create a button and add to list
        /// </summary>
        /// <param name="text">Text that will be used in button</param>
        /// <param name="font">Font that text will use when drawing button</param>
        /// <param name="defaultTexture">Texture on button</param>
        /// <param name="hoverTexture">Texture on button when hovering</param>
        /// <param name="size">Size of button</param>
        /// <param name="position">Position where button will be placed</param>
        /// <param name="action">Method that will be used when button is clicked</param>
        public void CreateButton(string text, SpriteFont font, Texture2D defaultTexture, Texture2D hoverTexture, Vector2 size,  Vector2 position, Action action)
        {

            // Adds new button to list
            buttons.Add(
                new Button(
                    text,
                    font,
                    defaultTexture,
                    hoverTexture,
                    size,
                    position,
                    action));
        }

        /// <summary>
        /// Draw every button in list
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw every button in list
            foreach (Button button in buttons)
            {
                button.Draw(spriteBatch);
            }

        }

        /// <summary>
        /// Draws every rectangle of button
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="texture"></param>
        public void DrawArea(SpriteBatch spriteBatch, Texture2D texture)
        {
            foreach(Button button in buttons)
            {
                button.DrawArea(spriteBatch, texture);
            }
        }
    }
}
