using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace Platformer_Slutprojekt
{
    /// <summary>
    /// Entity is base of enemy and player. This class contains basic methods and variables tied to a living object/entity.
    /// </summary>
    abstract class Entity
    {
        // Amount of health points. When health reaches 0 entity dies. Health is reduced by damage.
        protected int health;

        // Used to reset healt. starthealth is assigned in initialize
        protected int startHealth;

        // Amount of damage entity deals to another entity.
        protected int damage;
        
        // Name of entity. Can only be assigned when creating object.
        public string name { get; private set; }

        // Width of texture assigned to entity
        public int width;

        // Height of texture assigned to entity
        public int height;

        // Level is based on the experience gained from killing other entitys.
        public int level { get; private set; }

        // Height of jump Y-position.
        protected float jumpForce;

        // Texture assigned to entity. This texture will represent the entity in-game.
        public Texture2D texture { get; private set; }

        // Decides the position where entity will spawn.
        public Vector2 startPosition;

        // Current position of entity.
        public Vector2 position;

        // Acceleration will be increasingly be assigned to velocity
        public Vector2 acceleration;

        // Velocity will be assigned to entitys position
        public Vector2 velocity;

        // Will be used to determine collision between entity and other objects/entities
        public Rectangle rectangle;

        Rectangle oldRectangle;

        // Makes sure the offset of the rectangle is based on middle of sprite
        Vector2 offsetRectangle;

        // Used to determine if acceleration in Y-axis will be applied to entity
        public bool isGrounded;

        // Entity is alive and will Update
        public bool isAlive;

        // Max velocity of entity
        //Velocity can not exceed maxVelocity
        public float maxVelocity;

        // Entitys previous position
        public Vector2 oldPosition;

        // Will be used to determine if player is heading left or right
        public Vector2 deltaPosition = Vector2.Zero;

        // Tied to deltaPosition. Direction will be -1 if deltaPosition is negative and +1 if deltaPosition is positive
        // Direction will be used when to determine which direction entity will slow down when changing direction
        public Vector2 direction = Vector2.Zero;

        // Used for flipping sprites
        SpriteEffects flipSprite;
        SpriteEffects oldFlipSprite;

        // Checks collision with tiles and other entitys
        public CollisionHandler collisionHandler { get; private set; }

        #region Initalize

        public Entity(string name, Vector2 startPosition, int width, int height)
        {
            // Name will be assigned upon creating entity.
            this.name = name;

            // Startposition is saved and used when resetting game
            this.startPosition = startPosition;

            // Height and width of rectangle
            this.width = width;
            this.height = height;

            // Loads values assigned
            Initialize();

        }

        /// <summary>
        /// Loads default values assigned in constructor
        /// </summary>
        void Initialize()
        {
            Debug.WriteLine(name + " was initialized");

            // Startposition will be assigned upon creating entity.
            position = startPosition;

            oldPosition = position;

            health = startHealth;

            velocity = Vector2.Zero;

            acceleration = Vector2.Zero;

            this.isAlive = true;

            rectangle = new Rectangle((int)(position.X + offsetRectangle.X - width / 2), (int)(position.Y + offsetRectangle.Y - height / 2), width, height);

            // Initializes new collisionhandling
            collisionHandler = new CollisionHandler(this);

            // Makes sure entity spawns above ground
            collisionHandler.SpawnOnGround(this);
        }

        /// <summary>
        /// Loads texture. This method should be placed in LoadContent in main.
        /// </summary>
        public virtual void LoadContent(Texture2D texture)
        {
            this.texture = texture;

            // Offset makes sure rectangle is centerd on texture and aligned with bottom of texture
            offsetRectangle = new Vector2(texture.Width / 2 - rectangle.Width / 2, texture.Height - height);
        }

        /// <summary>
        /// Resets player from being dead by running initialize again
        /// </summary>
        public virtual void Reset()
        {
            Initialize();
        }


        #endregion

        #region Handle Damage

        /// <summary>
        /// Target is damaged by this entity damage and its health will be reduced
        /// </summary>
        /// <param name="target">Target is another entity</param>
        public void DealDamage(Entity target)
        {
            // Deal damage to other entity
            target.TakeDamage(this.damage);
        }

        /// <summary>
        /// This entity will be damaged and health will be reduced.
        /// </summary>
        /// <param name="damage">Damage from other entity</param>
        private void TakeDamage(int damage)
        {
            // Health is reduced by damage
            health = health - damage;

            // When health reaches zero entity will die
            // and stops update and draw
            if (health <= 0)
                Die();
        }

        /// <summary>
        /// Prevents player from falling off stage
        /// </summary>
        public void OffStage()
        {
            // TODO: Hårdkodat, fixa senare
            if (this.position.X <= 1 || this.position.Y > 1080 - 20)
            {
                this.Die();
            }
        }

        /// <summary>
        /// Uses startposition of entity to move entity back to stage
        /// </summary>
        public void Die()
        {
            position = startPosition;
            oldRectangle = rectangle;
            rectangle = new Rectangle();
            isAlive = false;
        }

        #endregion

        #region Update and Draw

        public virtual void Update(GameTime gameTime)
        {

            // A constant used to make sure that game is updated redundant regerdless of framerate
            var delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;

            // Makes sure to kill entitys offstage
            OffStage();

            // Only update entity if alive
            if (isAlive)
            {
                // Gravity downforce
                acceleration.Y = 20;

                // Velocity X
                velocity.X += acceleration.X * delta;

                // Limits the velocity.X value. Velocity can not exceed maxVelocity
                velocity.X = MathHelper.Clamp(velocity.X, -maxVelocity, maxVelocity);

                // Velocity Y
                velocity.Y += acceleration.Y * delta;

                // Check collision in the direction player is moving
                collisionHandler.DetectCollisionWhereDirection(this);

                // Previous position is used to determine deltaPosition
                oldPosition = position;

               // Debug.WriteLine("Position: {0:0}, {1:0}\nVelocity: {2:0}, {3:0}", position.X, position.Y, velocity.X, velocity.Y);

                // Changes position of entity based on velocity
                position += velocity;

                // Rectangle is based on position and offset of sprite and centered on sprite
                rectangle = new Rectangle((int)(position.X + offsetRectangle.X - width / 2), (int)(position.Y + offsetRectangle.Y - height / 2), width, height);

                // Makes sure entity keep facing direction last assigned
                oldFlipSprite = flipSprite;

                // If entity is moving left face entity to the left
                if (direction.X < 0)
                    // Flips entity sprite facing left
                    flipSprite = SpriteEffects.FlipHorizontally;
                else if (direction.X > 0)
                    // Entity will face right
                    flipSprite = SpriteEffects.None;
                else
                    // Last known flipstate is used otherwise
                    flipSprite = oldFlipSprite;

                // If deltaPosition is negative then entity is heading left
                // If deltaPosition is positive then entity is heading right
                deltaPosition = oldPosition - position;

                // Direction is assigned based on deltaPosition negative/positive
                direction.X = (deltaPosition.X > 0) ? direction.X = -1 : (deltaPosition.X < 0) ? direction.X = 1 : direction.X = 0;
                direction.Y = (deltaPosition.Y > 0) ? direction.Y = -1 : (deltaPosition.Y < 0) ? direction.Y = 1 : direction.Y = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Only draw entity if alive
            if(isAlive)
            {
                spriteBatch.Draw(
                    texture,
                    position,
                    null, // Dest. rectangle
                    null, // SourceRectangle
                    new Vector2(width / 2, height / 2), // Origin
                    0f, // Rotation
                    new Vector2(1, 1), // Scale
                    Color.White,
                    flipSprite, // Spriteffects
                    0); // Layer
            }    
        }

        /// <summary>
        /// Draw entity's rectangle that is used for collision
        /// </summary>
        /// <param name="spriteBatch">to be able to draw</param>
        /// <param name="graphics">used for creating new color to be used when drawing rectangle</param>
        public void DrawRectangle(SpriteBatch spriteBatch, Texture2D texture)
        {
            // Draw rectangle based on rectangle position
            spriteBatch.Draw(texture, rectangle, Color.White);
        }

        /// <summary>
        /// Draws tiles that collide with entity's rectangle
        /// </summary>
        /// <param name="spriteBatch">Used to draw</param>
        /// <param name="graphicsDevice"></param>
        public void DrawTilesCollidedWithEntity(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            // Collisionhandler adds the tiles that the entity collides with into a list and then they are drawed here before getting removed from list. This ensures that game will never draw permanent tiles
            collisionHandler.DrawTiles(spriteBatch, graphicsDevice);
        }

        #endregion

        /// <summary>
        /// Returns information about entity
        /// </summary>
        /// <returns></returns>
        public virtual string ToString()
        {
            string information = string.Format(
                "Type: {0} \n" +
                "Health: {1} \n" +
                "Velocity: {2:0}, {3:0} \n" +
                "Acceleration: ({4:0}), ({5:0}) \n" +
                "Direction: {6}, {7} \n", 
                this.GetType().Name , 
                health, 
                velocity.X, velocity.Y, 
                acceleration.X, acceleration.Y, 
                direction.X, direction.Y
                );

            return information;
        }
    }
}
