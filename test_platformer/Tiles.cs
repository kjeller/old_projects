using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Viking_Warrior___Slutprojekt
{
    /// <summary>
    /// Draw tile
    /// </summary>
    class Tiles
    {
        protected Texture2D texture;
        private Rectangle rectangle;
        public Rectangle Rectangle
        {
            get { return rectangle; }
            set { rectangle = value; }
        }
        private static ContentManager content;
        public static ContentManager Content //Tiles can reach textures from inside this class ex. Tiles.Content
        {
            protected get { return content; }
            set { content = value; }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White); //Draws the tile
        }
    }/// <summary>
    /// Gives tile texture
    /// </summary>
    class CollisionTiles : Tiles //Extends tiles class
    {
        public CollisionTiles(int i, Rectangle newRectangle) //Constructor
        {
            texture = Content.Load<Texture2D>("Sprites/Tiles/Tile" + i); //Loads different textures using the same construct   
            this.Rectangle = newRectangle;
        }
    }
    /// <summary>
    /// Gives tile texture
    /// </summary>
    class Coins : Tiles //List of all the coins
    {
        public Coins(int i, Rectangle coinRectangle ) //Constructor
        {
            texture = Content.Load<Texture2D>("Sprites/rendered_coin_2"); //Loads coin texture
            this.Rectangle = coinRectangle;
        }
    }
}
