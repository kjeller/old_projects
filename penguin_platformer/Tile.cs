using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Platformer_Slutprojekt
{
    class Tile
    {
        int height; // Height of tile derived from texture height
        int width; // Width of tile derived from texture width
        public int type { get; private set; }// Decides what type of texture tile will receive

        Texture2D texture;

        public Rectangle rectangle;

        // Used to place tile
        public Vector2 position;

        public Tile(Vector2 position, int type)
        {
            this.position = position;
            this.type = type;
            width = 60;
            height = 60;
            
        }

        public void LoadContent(ContentManager Content)
        {
            if (type == 1)
                texture = Content.Load<Texture2D>("Tile/1");
            else if (type == 2)
                texture = Content.Load<Texture2D>("Tile/2");
            else
                // Error texture. This texture will never be shown in-game.
                texture = Content.Load<Texture2D>("Tile/default_texture_tile");

            rectangle = new Rectangle((int)position.X, (int)position.Y, width, height);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.Wheat);
        }

        public void DrawRectangle(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            Texture2D texture = new Texture2D(graphics, 1, 1, false, SurfaceFormat.Color);
            texture.SetData<Color>(new Color[] { Color.Green });

            spriteBatch.Draw(texture, rectangle, Color.Green);
        }
    }
}
