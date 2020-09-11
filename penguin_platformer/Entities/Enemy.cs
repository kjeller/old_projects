using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Platformer_Slutprojekt
{
    class Enemy : Entity
    {
        int value;
        bool isPlayerNear;
        public int direction = 1;
        public int score { get; private set; }

        public float timer;
        public float actionTimer = 2;

        #region Initialize

        public Enemy(string name, Vector2 startPosition, int width, int height) : base(name, startPosition, width, height)
        {
            health = 30;
            startHealth = health;
            jumpForce = -13;
            maxVelocity = 5;
            damage = 10;
            score = 20;
            timer = actionTimer;
        }

        public Enemy(string name, Vector2 startPosition) : base(name, startPosition, 60, 120)
        {
            health = 30;
            startHealth = health;
            jumpForce = -13;
            maxVelocity = 5;
            damage = 10;
            score = 20;
            timer = actionTimer;
        }

        /// <summary>
        /// Loads texture for enemy
        /// </summary>
        /// <param name="Content"> Derived from main class </param>
        public override void LoadContent(Texture2D texture)
        {
            base.LoadContent(texture);
        }

        #endregion

        #region Update and Change Direction

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            actionTimer -= elapsed;

            // Reset timer if timer isn't used for anything
            if(actionTimer < -1)
            {
                actionTimer = timer;
            }

            if(isAlive)
                UpdateMovement(gameTime);

            // Apply acceleration to velocity and moves enemy 
            // Check collision
            base.Update(gameTime);
            
        }

        /// <summary>
        /// Updates movement on player
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void UpdateMovement(GameTime gameTime)
        {
            // Enemy moves towards designated direction
            acceleration.X = 2 * direction;

            // Check collision of where enemy is heading
            collisionHandler.DetectCollisionChangeDirection(this);
        }

        /// <summary>
        /// Change direction of enemy
        /// </summary>
        public void ChangeDirection()
        {
            // Direction is changed to opposite direction
            direction = -direction;
        }

        #endregion

        /// <summary>
        /// Prints information about enemy 
        /// Override from entity, adds action timer
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString() + 
                 string.Format("act. Timer: {0}", actionTimer);
        }
    }
}
