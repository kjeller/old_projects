using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;
using System.Reflection;

namespace Platformer_Slutprojekt
{
    class TileManager
    {
        // 2D array where tiles will be placed
        public static Tile[,] tiles { get; private set; }

        public Tile endOfStageTile;

        // Width/Height of every tile. Used when creating grid.
        public static int tileSize;

        // Position of the player spawn tile
        public static Vector2 playerSpawnTile { get; private set; }

        // Size of grid. This will limit the 2D array.
        public static Vector2 gridSize { get; private set; }

        public static Vector2 gridSizeInTiles { get; private set; }

        // List of objects with type and position of where enemies are allowed to spawn
        public List<EnemyRead> enemiesFromRead = new List<EnemyRead>();

        #region Initialize Grid

        public TileManager(string level)
        {
            gridSize = new Vector2(200, 200);
            
            tileSize = 60;

            gridSizeInTiles = new Vector2(gridSize.X / tileSize, gridSize.Y / tileSize);

            // Size of 2D array. Able to change 32 later
            tiles = new Tile[(int)gridSize.X, (int)gridSize.Y];

            // Creates a grid and fills it with tiles 
            // read from txt file
            MakeGrid(level);
        }

        #endregion

        #region Create Grid Fill With Tiles

        /// <summary>
        /// Creates a grid based on screensize (height, width).
        /// Reads every line in txt file and places tiles based
        /// on type( type = 1 => grass tile)
        /// </summary>
        /// <param name="level">name of text file</param>
        public void MakeGrid(string level)
        {
            // Current layer
            int layer = 0;
            List<string> lines = new List<string>();
            List<int> tileInRow = new List<int>();

            string path;

            // If file exist in solution
            if (!(level.Contains("C:\\")))
                // Gets filepath to file in solution
                path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), level);
            else
                path = level;

            // Reads txt file
            using (StreamReader reader = new StreamReader(path))
            {
                // Current line in txt file
                string line;

                // Will loop through every line read from txt file
                while((line = reader.ReadLine()) != null)
                {
                    // Gets Every value in one line
                    foreach (char type in line)
                    {
                        // Checks if char is int.
                        // Adds each character in line to a list 
                        // and converts char to int
                        if (Char.IsNumber(type))
                            tileInRow.Add((int)Char.GetNumericValue(type)); // Adds tile only if it's a number
                        else
                            tileInRow.Add(0); // Adds type 0 when char is not number. Error handler.
                    }

                    // Adds all the tiles per line
                    for (int i = 0; i < tileInRow.Count; i++)
                    {
                        // Uses AddTile method type, x index, y index
                        // Layer is currentLayer in txt file.
                        AddTile(tileInRow[i], i, layer);
                    }
                    layer++; // Changes to next layer
                    tileInRow.Clear(); // Clears list for next layer
                }
            }
        }

        /// <summary>
        /// Adds tile to list of tiles
        /// </summary>
        /// <param name="type">Specifies the type of tile</param>
        /// <param name="x">x index in list</param>
        /// <param name="y">y index in list</param>
        void AddTile(int type, int x, int y)
        {
            // PlayerSpawnPoint is assigned when reader finds type 3 in txt file
            if (type == 3)
            {
                playerSpawnTile = new Vector2(x * tileSize, y * tileSize);
            }

            // Adds position to enemy spawn list when type is 4
            else if (type == 4 || type == 5)
            {
                // Adds to enemy spawn list
                enemiesFromRead.Add(new EnemyRead(type, new Vector2((x * tileSize), (y * tileSize))));
            }

            // Adds tile if when type is not 0 and not spawntile.
            else if (type != 0)
            {
                if(type == 9)
                {
                    endOfStageTile = new Tile(new Vector2((x * tileSize), (y * tileSize)), type);
                }
                else
                {
                    // Adds tile based on the offset from x and y on each tile.
                    // Every tile has tileSize 60
                    // First tile on position (0,0) next tile (60,0) and so on
                    tiles[x, y] = new Tile(new Vector2((x * tileSize), (y * tileSize)), type);
                }
            }            
        }

        #endregion

        /// <summary>
        /// Loads every tile's texture
        /// </summary>
        /// <param name="Content">From main</param>
        public void LoadContent(ContentManager Content)
        {
            // Loads every tile texture for each tile in 2D array
            foreach(Tile tile in tiles)
            {
                // Some tiles in tile array aren't given any values
                // Hence why there is no reason to load them
                // If I don't want NullException
                if(tile != null)
                    tile.LoadContent(Content);
            }
        }

        /// <summary>
        /// Draws every tile
        /// </summary>
        /// <param name="spriteBatch">From main</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw every tile in array
            foreach(Tile tile in tiles)
            {
                // Only draw assigned tiles
                if(tile != null)
                    tile.Draw(spriteBatch);
            }
        }
    }

    // Will store type and Vector2 to use when creating new enemies in gameplaystate
    public class EnemyRead
    {
        int decideType;
        public Vector2 position { get; private set; }
        public Type enemyType { get; private set; }

        public EnemyRead(int type, Vector2 position)
        {
            this.decideType = type;
            this.position = position;

            // Decide what type enemy will be created
            // this type will be used in gameplaystate when creating enemies
            switch(decideType)
            {
                case 4:
                    enemyType = typeof(Enemy);
                    break;
                case 5:
                    enemyType = typeof(Jumper);
                    break;
                default:
                    enemyType = typeof(Enemy);
                    break;
            } 
        }
    }
}
