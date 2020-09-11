using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Viking_Warrior___Slutprojekt
{
    class Map
    {
        private List<CollisionTiles> collisionTiles = new List<CollisionTiles>(); // To get all the collisiontiles collide with the player
        /// <summary>
        /// List of Tiles with collision
        /// </summary>
        public List<CollisionTiles> CollisionTiles
        {
            get { return collisionTiles; }
        }
        private List<Coins> coins = new List<Coins>();
        /// <summary>
        /// List of "coins" with collision
        /// </summary>
        /// <remarks>
        /// Although I choose to write coins, 
        /// in this case I use the list to draw checkpoints
        /// </remarks>
        public List<Coins> Coins
        {
            get { return coins; }
        }

        private int width, height; // The size of the tile - will help later when setting up the camera
        /// <summary>
        /// Width of a tile
        /// </summary>
        public int Width
        {
            get { return width; }
        }
        /// <summary>
        /// Height of a tile
        /// </summary>
        public int Height
        {
            get { return height; }
        }
        /// <summary>
        /// Generates the map
        /// </summary>
        /// <param name="map">Array that decides which block to draw, from main()</param>
        /// <param name="size">Decides the size of the tile</param>
        public void Generate(int[,] map, int size) //Input of array and size of the tiles
        {
            for (int x = 0; x < map.GetLength(1); x++) //Loops through X-axis of the array
                for(int y = 0; y < map.GetLength(0); y++) //Loops through Y-axis of the array
                {
                    int number = map[y, x]; //Determine what texture we are going to load in(1, 2 or 3)
                    if(number > 0 && number < 4) //If number is 0 - add nothing
                    {
                        collisionTiles.Add(new CollisionTiles(number, new Rectangle(x * size, y * size, size, size))); //If (number > 0), then it's going to write a tile, if it isn't then it will leave a blank - adds to the list
                        width = (x + 1) * size; //X > 0
                        height = (y + 1) * size;//Y > 0
                    }
                    //Adds checkpoint to checkpoint list
                    else if(number == 4)
                    {
                        Coins.Add(new Coins(number, new Rectangle(x * size, y * size, size, size))); //Same size of the rectangle as before
                        width = (x + 1) * size; //X > 0
                        height = (y + 1) * size;//Y > 0
                    }
                }   
        }
        public void Draw(SpriteBatch spritebatch)
        {
            foreach (CollisionTiles tile in collisionTiles) //Applies collision to each tile
                tile.Draw(spritebatch); //Draws the tile
            foreach (Coins coin in Coins) //List of checkpoints with collision
                coin.Draw(spritebatch); //Draws the "coins" from the list
        }
    }
}
