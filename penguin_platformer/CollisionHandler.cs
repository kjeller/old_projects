using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer_Slutprojekt
{
    class CollisionHandler
    {
        // List of map where entity collides with map.
        List<Tile> drawCheckedTiles = new List<Tile>();

        // Entitys height and width in tilesize
        Vector2 sizeInTiles;

        // Array of tiles to check collision with
        Tile[,] tiles;

        // Coordinate of entity bounding box in tilemap
        Vector2 coordinateOfEntity;

        // Coordinate of the direction entity is heading towards, based on the edges of entity bounding box
        Vector2 coordinateOfDirection;

        #region Initialize Collision

        public CollisionHandler(Entity entity)
        {
            tiles = TileManager.tiles;

            // Creates a limit on how many tiles that will be checked around entity
            sizeInTiles = new Vector2((int)Math.Round((double)entity.width / TileManager.tileSize), (int)Math.Round((double)entity.height / TileManager.tileSize));
        }

        /// <summary>
        /// Makes sure entity spawns above ground. Bases on collision on start.
        /// Failsafe method used everytime entity respawn.
        /// </summary>
        public void SpawnOnGround(Entity entity)
        {
            foreach(Tile tile in tiles)
            {
                if(tile != null)
                {
                    // If players feet are in ground
                    if (Rectangle.Intersect(entity.rectangle, tile.rectangle).Height < Rectangle.Intersect(entity.rectangle, tile.rectangle).Width)
                        {
                        // If entity is colliding with a tile

                        entity.position.Y = tile.position.Y - entity.texture.Height;
                    }
                }
            }
        }

        #endregion

        #region Calculate Coordinates

        /// <summary>
        /// Calculate coordinate of entity in grid created by tilemanager.
        /// CalcCoordinate should be called each update if entity is moving.
        /// </summary>
        /// <param name="entity">Entity used to get coordinate of entity's position</param>
        public void CalcCoordinate(Entity entity)
        {
            // Coordinate of entity bounding box in tilemap
            coordinateOfEntity = new Vector2((float)Math.Round((float)entity.position.X / TileManager.tileSize), (float)Math.Round((float)entity.position.Y / TileManager.tileSize));

            // Resets direction before calculating so that old one doesn't persist
            coordinateOfDirection = Vector2.Zero;

            // Entity is moving towards the right
            if (entity.direction.X > 0)
            {
                // Get X coordinate of the right edges of entity bounding box
                coordinateOfDirection.X = (float)Math.Round((float)entity.rectangle.Right / TileManager.tileSize);
            }
            // Entity is moving towards the left
            else if (entity.direction.X < 0)
            {
                // Get X coordinate of the left edges of entity bounding box
                coordinateOfDirection.X = (float)Math.Round((float)entity.rectangle.Left / TileManager.tileSize - 1);
            }

            // Entity is falling
            if (entity.direction.Y > 0)
            {
                // Get Y coordinate of the bootom edge of entity bounding box
                coordinateOfDirection.Y = (float)Math.Round((float)entity.rectangle.Bottom / TileManager.tileSize);
            }
            // Entity is jumping
            else if (entity.direction.Y < 0)
            {
                // Get Y coordinate of the top edge of entity bounding box
                coordinateOfDirection.Y = (float)Math.Round((float)entity.rectangle.Top / TileManager.tileSize - 1);
            }

        }

        #endregion

        #region Detect Collision

        /// <summary>
        /// Determines where to check after bounding boxes in tilemap
        /// based on direction of where player is moving.
        /// </summary>
        /// <param name="entity"></param>
        public void DetectCollisionWhereDirection(Entity entity)
        {
            // Gets relevant coordinates of entity
            CalcCoordinate(entity);

            // Makes sure entity is in grid
            if(coordinateOfEntity.X > 0 && coordinateOfDirection.X >= 0  && coordinateOfEntity.Y > 0 && coordinateOfDirection.Y >= 0)
            {
                // Find nearest line of tiles near entity Y axis when entity is moving sideways
                if (coordinateOfDirection.X > -1)
                {
                    // Scan nearest line of tiles where entity might intersect
                    for (int i = -1; i <= sizeInTiles.Y; i++)
                    {
                        if (CheckCollisionX(tiles[(int)coordinateOfDirection.X, (int)coordinateOfEntity.Y + i], entity))
                        {
                            // Moves back player to previous position
                            // Keeps player from stuttering against wall
                            entity.position.X = entity.oldPosition.X;

                            // Entity movement(velocity, acceleration) is stopped

                            entity.velocity.X = 0;
                            entity.acceleration.X = 0;

                            // Will be able to draw tiles rectangle - Used for debugging
                            drawCheckedTiles.Add(tiles[(int)coordinateOfDirection.X, (int)coordinateOfEntity.Y + i]);
                        }
                    }
                }

                // Find nearest tile of entity in X axis when entity is falling or jumping
                if (coordinateOfDirection.Y > -1)
                {
                    // Scan ara based on size of rectangle
                    for (int i = -1; i <= sizeInTiles.X; i++)
                    {

                        // If entity collide with tile
                        if (CheckCollisionY(tiles[(int)coordinateOfEntity.X + i, (int)coordinateOfDirection.Y], entity))
                        {
                            // Entity movement(velocity, acceleration) is stopped
                            entity.velocity.Y = 0;
                            entity.acceleration.Y = 0;

                            // Moves back player to previous position
                            // Keeps player from stuttering against ground
                            entity.position.Y = entity.oldPosition.Y;

                            // Will be able to draw tiles rectangle - Used for debugging
                            drawCheckedTiles.Add(tiles[(int)coordinateOfEntity.X + i, (int)coordinateOfDirection.Y]);
                        }
                        
                    }
                }
            }
        }

        /// <summary>
        /// Changes direction based on collision. This will be used by an enemy.
        /// </summary>
        public void DetectCollisionChangeDirection(Enemy enemy)
        {
            // Get coordinate of enemy
            CalcCoordinate(enemy);

            // Find nearest line of tiles near entity Y axis when entity is moving sideways
            if (coordinateOfDirection.X > -1)
            {
                // Scan nearest line of tiles where enemy might intersect
                for (int i = -1; i <= 1; i++)
                {
                    // Check collision from sides and if tile exist
                    if(CheckCollisionX(tiles[(int)coordinateOfDirection.X, (int)coordinateOfEntity.Y + i], enemy))
                    {
                        // Moves back player to previous position
                        // Keeps player from stuttering against wall
                        enemy.position.X = enemy.oldPosition.X;

                        // Enemy movement(velocity, acceleration) is stopped
                        enemy.velocity.X = 0;
                        enemy.acceleration.X = 0;

                        // Change direction of enemy
                        enemy.ChangeDirection();

                        // Will be able to draw tiles rectangle - Used for debugging
                        drawCheckedTiles.Add(tiles[(int)coordinateOfDirection.X, (int)coordinateOfEntity.Y + i]);

                        // Breaks loop and makes sure that enemy changes direction once
                        break;  
                    } 
                }
            }
        }

        #endregion

        #region Check Collision

        /// <summary>
        /// Checks collision with tile and entity in X axis
        /// </summary>
        /// <param name="tile">Tile to check collision with</param>
        /// <param name="entity">Entity used to check collision with tile</param>
        /// <returns></returns>
        public bool CheckCollisionX(Tile tile, Entity entity)
        {
            // Makes sure tile is not null
            if (tile != null)
            {
                // Check if tile and entity intersects
                // Height must be greater than width, then there is collision from sides of rectangle
                if (Rectangle.Intersect(entity.rectangle, tile.rectangle).Height > Rectangle.Intersect(entity.rectangle, tile.rectangle).Width)
                {
                    // There is collision
                    return true;
                }
            }
            // There is no collision
            return false;
        }

        /// <summary>
        /// Checks collision with tile and entity in Y axis
        /// </summary>
        /// <param name="tile">Tile to check collision with</param>
        /// <param name="entity">Entity used to check collision with tile</param>
        /// <returns></returns>
        public bool CheckCollisionY(Tile tile, Entity entity)
        {
            // Makes sure the tile is there
            if (tile != null)
            {
                // And if the entity intersects with tile
                if (Rectangle.Intersect(entity.rectangle, tile.rectangle).Height < Rectangle.Intersect(entity.rectangle, tile.rectangle).Width)
                {
                    // When tile is above entity
                    if (tile.position.Y <= entity.position.Y)
                    {
                        entity.isGrounded = false;
                    }

                    // Otherwise tile is below entity
                    else
                    {
                        entity.isGrounded = true;
                    }
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Handles collision between player and enemy
        /// </summary>
        /// <param name="player"></param>
        /// <param name="enemy"></param>
        public void CollisionPlayerEnemy(Player player, Enemy enemy)
        {
            // If player collides with enemy's tile from above
            if (Rectangle.Intersect(player.rectangle, enemy.rectangle).Height < Rectangle.Intersect(player.rectangle, enemy.rectangle).Width && player.position.Y < enemy.position.Y)
            {
                // When player is above enemy
                if(player.position.Y < enemy.position.Y)
                {
                    player.velocity.Y = -15;
                    player.DealDamage(enemy);
                    if (!enemy.isAlive)
                        player.obtainedScore += enemy.score;
                }
                else if(player.position.Y > enemy.position.Y)
                {
                    enemy.DealDamage(player);
                }
            }

            // When player collides with enemy from side
            if (Rectangle.Intersect(player.rectangle, enemy.rectangle).Height > Rectangle.Intersect(player.rectangle, enemy.rectangle).Width)
            {
                // Player will be damaged
                enemy.DealDamage(player);

                // Player gets knocked back
                player.velocity.X = 6 * -player.direction.X;
            }

        }

        #endregion

        #region Get Coordinates

        /// <summary>
        /// Used for printing coordinate of entity's side in the direction entity is moving towards in grid. Used in debugging information
        /// </summary>
        /// <returns></returns>
        public Vector2 GetCoordinateDirection()
        {
            return coordinateOfDirection;
        }
        
        /// <summary>
        /// Used for debugging actual coordinate of entity in grid. Used in debugggin information.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetCoordinateEntity()
        {
            return coordinateOfEntity;
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draw every tile that is affected by collision.
        /// Used for debugging.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawTiles(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            foreach(Tile tile in drawCheckedTiles)
            {
                tile.DrawRectangle(spriteBatch, graphics);
            }
            drawCheckedTiles.Clear();
        }

        #endregion
    }
}
