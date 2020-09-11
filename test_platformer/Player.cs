using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Viking_Warrior___Slutprojekt
{
    /// <summary>
    /// Player main class
    /// </summary>
    public class Player 
    {
        private Texture2D texture;
        private Texture2D[] heartcontainer = new Texture2D[3]; //Hp array
        private Texture2D heart; //Temporary texture2D with no assigned value
        private Texture2D keyboard;
        private SpriteFont font;
        private bool stateDead;
        /// <summary>
        /// Returns playerstate
        /// </summary>
        public bool StateDead 
        {
            get
            {
                return stateDead; //Returns state to other classes
            }
            set
            {
                stateDead = value;
            }
        }
        /// <summary>
        /// Decides if the game should show tutorial on how to control the character
        /// </summary>
        public bool controlHelper = false;
        /// <summary>
        /// Initialize controlHelperValue
        /// </summary>
        public int controlHelperValue = 0;
        
        //Movement of the character
        private Vector2 velocity = Vector2.Zero;  //speed in certain direction.
        private Vector2 direction = Vector2.Zero; //the direction of the character (Left or Right)
        /// <summary>
        /// Startposition of character
        /// </summary>
        public Vector2 startPosition = new Vector2(200, 850); //Startposition
        private Vector2 position;//the position of the character
        /// <summary>
        /// Local player rectangle
        /// </summary>
        private Rectangle rectangle;
        /// <summary>
        /// Returns player rectangle to other classes
        /// </summary>
        public Rectangle Rectangle //Collision on the character
        {
            get
            {
                return rectangle;
            }
        }
       
        private int moveLeft = -1; //Moves character - 1x
        private int moveRight = 1; //Moves character + 1X
        private float rotation = 0.0f;
        const float speed = 300.0f; //The velocity of the movement
        const float gravity = 250.80f;

        //Keyboard
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        //State
        /// <summary>
        /// Bool that tells if player is grounded
        /// </summary>
        public bool onGround; //True if player is grounded
        private int lives = 3;
        /// <summary>
        /// Initializes values on reset
        /// </summary>
        public void Reset()
        {
            Vector2 startPosition = new Vector2(200, 900); //Startposition
            stateDead = false; //Character is alive by default
            position = new Vector2(200, 850);
            heart = heartcontainer[2]; //Keeps heart from returning null by initializing
            velocity.Y = 1;
            lives = 3;
        }
        /// <summary>
        /// Reset function that only resets players position to startposition
        /// </summary>
        public void ResetPosition()
        {
            position = startPosition;
        }
        /// <summary>
        /// Player constructor that initializes player object
        /// </summary>
        public Player()
        {
            stateDead = false; //Character is alive by default
            heart = heartcontainer[2]; //Keeps heart from returning null by initializing
            position = startPosition;
            velocity.Y = 1;
            
        
        }
        /// <summary>
        /// Loads all content for the player
        /// </summary>
        /// <param name="Content">Contentmanager that uses the monogame pipeline</param>
        public virtual void Load(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Sprites/character");
            heartcontainer[0] = Content.Load<Texture2D>("Sprites/hp/1hp");
            heartcontainer[1] = Content.Load<Texture2D>("Sprites/hp/2hp");
            heartcontainer[2] = Content.Load<Texture2D>("Sprites/hp/3hp");
            keyboard = Content.Load<Texture2D>("Sprites/keycaps_white_red");
            font = Content.Load<SpriteFont>("Sprites/Vikingfont");

        }
        /// <summary>
        /// Updates the input from the player.
        /// </summary>
        /// <remarks>It handles all the important input from the player,
        /// the value of direction differs</remarks>
        /// <param name="gametime">Provides a snapshot of timing values.</param>
        public void UpdateInput(GameTime gametime)
        {
                currentKeyboardState = Keyboard.GetState();
                if (currentKeyboardState.IsKeyDown(Keys.A)) //Moves the character right
                {
                    velocity.X = speed;
                    direction.X = moveLeft;
                    controlHelperValue++; //When the character moves, controlHelperValue adds up
                }
                if (currentKeyboardState.IsKeyDown(Keys.D)) //Moves the character left
                {
                    velocity.X = speed;
                    direction.X = moveRight;
                    controlHelperValue++; //Adds up to remove Controlhelper
                    
                }

                if (currentKeyboardState.IsKeyDown(Keys.W) && onGround) //Jump
                {
                velocity.Y = -3.5f;
                onGround = false; //Enable falling again
                    
                }
            if(controlHelperValue >= 40) //If the controlHelperValue has moved less than or 40 p
            {
                controlHelper = true; //Allows the controlhelper to be seen
            }
            previousKeyboardState = currentKeyboardState;
        }
        /// <summary>
        /// Updates the movement of the character in X - pos
        /// and Y - pos.
        /// </summary>
        /// <param name="gametime">Provides a snapshot of timing values.</param>
        public void UpdateMovement(GameTime gametime) //Updates the movement of the character
        {
            direction.X = 0; //Resets the direction
            velocity.X = 0; //Resets the velocity.X
            UpdateInput(gametime); //Updates Player input
            position.Y = position.Y + (gravity * velocity.Y * (float)gametime.ElapsedGameTime.TotalSeconds); //Updates the Y position for the character - By using elapsed time, consistent on every framerate
            position.X += speed * direction.X * (float)gametime.ElapsedGameTime.TotalSeconds;
            velocity.Y += 0.1f; //Makes the character not fall through floor by reseting the velocity
            if(onGround)
            {
                rotation = 0.0f; //No rotation onGround
            }
            else if(!(onGround) && direction.X == moveLeft)
            {
                rotation = 0.2f;
            }
            else if(!(onGround) && direction.X == moveRight)
            {
                rotation = -0.2f;
            }
        }
        /// <summary>
        /// Updates the collision related to the player
        /// </summary>
        /// <param name="newRectangle">Other rectangle, eg. maptiles</param>
        /// <param name="widthOffset">The offset of the width - Not used</param>
        /// <param name="heightOffset">The offset of the height - Not used</param>
        public void UpdateCollisionTile(Rectangle newRectangle, int widthOffset, int heightOffset)
        {
            if ((rectangle.onTop(newRectangle))) //If bool is true, player is on ground
            {
                velocity.Y = 0;
                onGround = true;
            }
            if(rectangle.onBottom(newRectangle))
            {
                velocity.Y = 1;
            }
        }
        /// <summary>
        /// Updates collision with coins
        /// </summary>
        /// <param name="newRectangle">Other rectangle, eg. coin</param>
        /// <param name="widthOffset">The offset of the width - Not used</param>
        /// <param name="heighOffset">The offset of the height - Not used</param>
        public void UpdateCollisionCoin(Rectangle newRectangle, int widthOffset, int heighOffset)
        {
            
        }
        /// <summary>
        /// Updates the state of the character (alive/dead)
        /// </summary>
        /// <param name="gametime"></param>
        public void UpdateState(GameTime gametime)
        {
            //Character takes damage
            if(lives == 3)
            {
                heart = heartcontainer[2];
            }
            else if(lives == 2)
            {
                heart = heartcontainer[1];
            }
            else if(lives == 1)
            {
                heart = heartcontainer[0];
            }
            else if(lives == 0)
            {
                stateDead = true; //Player is dead
            }

            if (position.Y > Globals.screenHeight && lives >= 1)
            {
                lives --; //Update method that makes the player dead
                position = startPosition; //Resets plaer position
                
            }
        }
        /// <summary>
        /// Updates all the player logic
        /// </summary>
        /// <param name="gametime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gametime)   //Updates all character traits in a logical order
        {
            UpdateState(gametime); //Updates the state from UpdateState method (IsWalking), and HP
            UpdateInput(gametime); //Updates Input from UpdateInput method
            UpdateMovement(gametime);  //Updates movement from UpdateMovement method
            if(stateDead == false)
            {
                rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            }
        }
        /// <summary>
        /// Draw character
        /// </summary>
        /// <param name="spritebatch">Spritebatch</param>
        public virtual void Draw(SpriteBatch spritebatch)
        {
            if(stateDead == false) //If character is alive
            {
                spritebatch.Draw(heart, new Vector2(0, 0), null); //Draw lives of the character
                if(controlHelper == false)
                {
                    spritebatch.Draw(keyboard, new Vector2(position.X - 30, (position.Y - (3 *texture.Height))), Color.White); //Places controlhelper above player - following the player
                }
            }
            if(stateDead == true)
            {
                spritebatch.DrawString(font, "DEAD", new Vector2(0, 0), Color.Red); //Returns dead statement
            }
            if(direction.X == moveRight)
            {
                spritebatch.Draw(texture, position, null, null, null, rotation, null, Color.White); //Draw player texture on the position
            }
            else if(direction.X < moveRight)
            {
                spritebatch.Draw(texture, position, null, null, null, rotation, null, Color.White, SpriteEffects.FlipHorizontally); //Draw player texture on the position
            }
            
        }

    }
}
