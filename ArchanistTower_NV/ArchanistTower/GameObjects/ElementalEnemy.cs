using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ArchanistTower.GameObjects
{
    class ElementalEnemy : Enemy
    {
        private float enemyChaseVelocity { get { return 0.57f; } }

        public override void Update(GameTime gameTime)
        {
            if (enemyState == EnemySpriteState.Chase)
            {   //if enemyState == Chase, make sprite chase player, and adjust his speed
                Chase(SpriteAnimation.Position, PlayerPosition, ref enemyOrientation, enemyTurnSpeed);
                SpriteAnimation.Speed = enemyChaseVelocity; //set speed to equal ChaseVeclocity
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Chases the player
        /// </summary>
        public virtual void Chase(Vector2 position, Vector2 playerPosition, ref float orient, float turnSpeed)
        {   //change the oritenation to face player
            orient = TurnToFace(position, playerPosition, orient, turnSpeed);
        }

        public override void Draw()
        {
            //drawing JUST the lifebar above enemy's head
            Globals.spriteBatch.Draw(
                lifebar,
                new Rectangle(SpriteAnimation.Bounds.X + 3, SpriteAnimation.Bounds.Y - 5,   //x and y adjusted for sprite
                    SpriteAnimation.Bounds.Width * Health / 100 - 6, 2),    //width based on health / adjusted for sprite
                Microsoft.Xna.Framework.Graphics.Color.White);
            base.Draw();
        }
 
    }
}
