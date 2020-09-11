using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace Platformer_Slutprojekt
{
    class Player : Entity
    {
        public int obtainedScore; // Player obtain score by killing enemies

        #region Intialize

        public Player(string name, Vector2 startPosition) :base(name, startPosition, 40, 70)
        {
            // Assigning player values
            health = 30;
            startHealth = health;
            obtainedScore = 0;
            jumpForce = -13;
            maxVelocity = 8;
            damage = 10;
        }

        /// <summary>
        /// Loads texture for player
        /// </summary>
        /// <param name="Content"> Derived from main class </param>
        public override void LoadContent(Texture2D texture)
        {
            base.LoadContent(texture);
        }

        public override void Reset()
        {
            base.Reset();

            // Resets score tied to player
            obtainedScore = 0;
        }

        #endregion

        #region Update

        /// <summary>
        /// Handles all update functions tied to player.
        /// This function contains UpdateInput() and UpdateMovement(),
        /// To be used in main to update player.
        /// </summary>
        public override void Update(GameTime gameTime)
                {
                    base.Update(gameTime);
                }

                public void UpdateInput(KeyboardState oldKeyboardState, KeyboardState keyboardState)
                {
                    if(isAlive)
                    {
                        // Accelerates player to the left
                        if (keyboardState.IsKeyDown(Keys.A))
                        {
                            // Player is now facing left
                            // Changes acceleration force to negative making player move left
                            acceleration.X = -12;
                        }
                        // Accelerates player to the right
                        else if (keyboardState.IsKeyDown(Keys.D))
                        {
                            // Player is now facing right
                            // Changes acceleration force to positive making player move right
                            acceleration.X = 12;
                        }

                        // Slows down entity when entity is no longer accelerating
                        else if (velocity.X != 0)
                        {
                            // Slows down player based on direction
                            acceleration.X = 2 * -direction.X;

                            // Makes sure player will stop
                            if (velocity.X < 0.02 && velocity.X > -0.02)
                            {
                                velocity.X = 0;
                                acceleration.X = 0;
                                direction.X = 0;
                            }
                        }

                        // Player is able to jump when space is pressed while player is grounded
                        if (keyboardState.IsKeyDown(Keys.Space) && isGrounded)
                        {
                            // Adds velocity to player
                            velocity.Y = jumpForce;
                            isGrounded = false;
                        }
                    }
                }

        #endregion

        public override string ToString()
        {
            return base.ToString() + 
                string.Format(
                "cord.Direction: {0}, {1} \n" +
                "cord.Position: {2}, {3} \n" +
                "IsGrounded: {4} \n" +
                "IsAlive: {5} \n" +
                "Position: {6:0}, {7:0} \n", 
                this.collisionHandler.GetCoordinateDirection().X, this.collisionHandler.GetCoordinateDirection().Y,
                this.collisionHandler.GetCoordinateEntity().X, this.collisionHandler.GetCoordinateEntity().Y, 
                isGrounded.ToString(), 
                isAlive.ToString(), 
                rectangle.Location.X, rectangle.Location.Y
                );
        }
    }
 }

