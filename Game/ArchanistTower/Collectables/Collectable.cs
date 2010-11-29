using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ArchanistTower.Collectables
{
    public class Collectable
    {
        public bool Active { get; set; }
        public AnimatedSprite SpriteAnimation { get; set; }

        public Collectable() { }
        public virtual void Initialize(Vector2 position) { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw()
        {
            SpriteAnimation.Draw(Globals.spriteBatch);
        }
    }
}
