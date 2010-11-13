using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ArchanistTower.GameObjects
{
    public class GameObject
    {
        public float Speed { get; set; }
        public bool Dead { get; set; }
        public bool Collidable {get; set;}
        public AnimatedSprite SpriteAnimation { get; set; }
        public int Health { get; set; }
        public Vector2 LastMovement { get; set; }
        
        public enum FacingDirection
        {
            Up,
            Down,
            Left,
            Right
        }
                
        public GameObject() { }
        public virtual void Collision(GameObject o) { }
        public virtual void Initialize() { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw() 
        {
            SpriteAnimation.Draw(Globals.spriteBatch);
        }

        public void Collision()
        {
            SpriteAnimation.Position -= LastMovement;
        }
    }
}
