using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Platformer_Slutprojekt
{
    class Jumper : Enemy
    {
        /// <summary>
        /// Uses Enemy construct when creating jumper
        /// </summary>
        /// <param name="name"></param>
        /// <param name="startPosition"></param>
        public Jumper(string name, Vector2 startPosition) : base(name, startPosition, 40, 70)
        {
            actionTimer = 2;
            timer = actionTimer;
            health = 30;
            jumpForce = -13;
            maxVelocity = 3;
        }

        public override void UpdateMovement(GameTime gameTime)
        {
            // Jump when times reaches zero and enemy is grounded
            if (actionTimer < 0 && isGrounded)
            {
                // Makes enemy jump by adding velocity
                Jump();

                // Resets timer
                actionTimer = timer;
            }

            // Makes enemy move sideways
            base.UpdateMovement(gameTime);
        }

        /// <summary>
        /// Makes enemy jump by adding to velocity y
        /// </summary>
        void Jump()
        {
            Debug.WriteLine("JUMP");
            velocity.Y = jumpForce;
            isGrounded = false;
        }
    }
}
