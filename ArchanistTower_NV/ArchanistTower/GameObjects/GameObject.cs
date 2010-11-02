using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ArchanistTower.GameObjects
{
    /// <summary>
    /// This will be a Template class for all of our Game Objects
    /// Any method or attribute common among our objects should be
    /// in this class.
    /// </summary>
    
    public class GameObject
    {
        protected Vector2 position;
        protected Vector2 velocity;

        public GameObject()
        {
        }

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

        public virtual void Initialize() { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw() { }
    }
}
