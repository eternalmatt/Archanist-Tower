using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using TileEngine;
using ArchanistTower.GameObjects;

namespace ArchanistTower
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ArchanistTower : Microsoft.Xna.Framework.Game
    {
        public static TileMap tileMap = new TileMap();
        public static Camera camera = new Camera();

        List<AnimatedSprite> npcs = new List<AnimatedSprite>();
        SpriteFont font;


        Player p1 = new Player();
        Enemy e1 = new Enemy();
 
        HUD hud = new HUD();

        Screens.MenuScreen menuscreen;
        Screens.PauseScreen pausescreen;
        Texture2D menubg;
        Texture2D pausebg;

        public ArchanistTower()
        {
            Globals.Initialize(this);
        }

        protected override void Initialize()
        {
            base.Initialize();

            p1.Initialize();
            e1.Initialize();

            Random rand = new Random();

/*            foreach(AnimatedSprite s in npcs)
            {
                s.Animations.Add("Up", (FrameAnimation)up.Clone());
                s.Animations.Add("Down", (FrameAnimation)down.Clone());
                s.Animations.Add("Left", (FrameAnimation)left.Clone());
                s.Animations.Add("Right", (FrameAnimation)right.Clone());

                int animation = rand.Next(3);

                switch(animation)
                {
                    case 0:
                        s.CurrentAnimationName = "Up";
                        break;
                    case 1:
                        s.CurrentAnimationName = "Down";
                        break;
                    case 2:
                        s.CurrentAnimationName = "Left";
                        break;
                    case 3:
                        s.CurrentAnimationName = "Right";
                        break;
                }

                s.Position = new Vector2(
                    rand.Next(tileMap.GetWidthInPixels() - 32),
                    rand.Next(tileMap.GetHeightInPixels() - 32));
            }
*/
        }

        protected override void LoadContent()
        {
            Globals.LoadContent();
            hud.LoadContent();
            hud.LoadCamera(camera);
            tileMap.Layers.Add(TileLayer.FromFile(Globals.content, "Content/Layers/Layer1.layer"));
            tileMap.CollisionLayer = CollisionLayer.FromFile("Content/Layers/Collision.layer");

            p1.LoadContent();
            e1.LoadContent();
            
            /*
            npcs.Add(new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/man1")));
            npcs.Add(new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/knt4")));
            npcs.Add(new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/mst3")));
            npcs.Add(new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/thf4")));
            npcs.Add(new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/smr1")));
            npcs.Add(new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/nja2")));
            */
            font = Globals.content.Load<SpriteFont>("Fonts/Arial");

            menubg = Globals.content.Load<Texture2D>("Menuscreens/menu");
            menuscreen = new Screens.MenuScreen(this, menubg);
            pausebg = Globals.content.Load<Texture2D>("Menuscreens/pause");
            pausescreen = new Screens.PauseScreen(this, pausebg);
        }
        protected override void UnloadContent()
        { }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                pausescreen.IsActive = true;
            }

            if (menuscreen.IsActive)
            {
                menuscreen.Update(gameTime);
            }
            else if (pausescreen.IsActive)
            {
                pausescreen.Update(gameTime);
                if (pausescreen.BackToMenu)
                {
                    menuscreen.IsActive = true;
                    pausescreen.BackToMenu = false;
                }
            }
            else if (!menuscreen.IsActive && !pausescreen.IsActive)
            {
                // Allows the game to exit
                //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                //    this.Exit();                

                foreach (AnimatedSprite s in npcs)
                    s.Update(gameTime);


                p1.Update(gameTime);
                e1.Update(gameTime);

          
                //hud code
                hud.PlayerLifeBar++;
            }
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            if (menuscreen.IsActive)
            {
                menuscreen.Draw(gameTime);
            }
            else if (pausescreen.IsActive)
            {
                pausescreen.Draw(gameTime);
            }
            else if (!menuscreen.IsActive && !pausescreen.IsActive)
            {
                Globals.graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

                tileMap.Draw(Globals.spriteBatch, camera);


                Globals.spriteBatch.Begin(
                    SpriteBlendMode.AlphaBlend,
                    SpriteSortMode.Texture,
                    SaveStateMode.None,
                    ArchanistTower.camera.TransformMatrix);
                p1.Draw();
                e1.Draw();
            

     //           foreach (AnimatedSprite s in npcs)
     //               s.Draw(Globals.spriteBatch);
                hud.Draw(gameTime);                

                Globals.spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}
