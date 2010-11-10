using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ArchanistTower.GameObjects
{
    public class GameObject
    {
        protected Vector2 position;
        protected Vector2 velocity;
        protected int health;
        protected AnimatedSprite animatedSprite;
        protected Vector2 origin;

        protected bool dead;
        protected bool collidable;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public bool Dead
        {
            get { return dead; }
            set { dead = value; }
        }

        public bool Colldable
        {
            get { return collidable; }
            set { collidable = value; }
        }

        public AnimatedSprite SpriteAnimation
        {
            get { return animatedSprite; }
        }

        public GameObject() { }
        public virtual void Collision(GameObject o) { }
        public virtual void Initialize() { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw() { }
    }
}
