using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace ArchanistTower
{
    public static class Globals
    {
        public static SpriteBatch spriteBatch;
        public static GraphicsDeviceManager graphics;
        public static ContentManager content;
        public static ResolutionManager Resolution;
        public static SpriteFont spriteFont;
        public static SpriteFont menuFont;
        public static Camera camera;

        public static GraphicsDevice GraphicsDevice
        {
            get { return graphics.GraphicsDevice; }
        }

        public static void Initialize(Game game)
        {
            graphics = new GraphicsDeviceManager(game);
            content = new ContentManager(game.Services);
            content.RootDirectory = "Content";
            camera = new Camera();

            Resolution = new ResolutionManager(ResolutionManager.Res800x600);
            SetResolution(Resolution.ObtainResolution(), false);
        }

        public static void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = content.Load<SpriteFont>("Fonts/Arial");
            menuFont = content.Load<SpriteFont>("Fonts/menufont");
        }

        public static void SetResolution(Vector2 res, bool fullScreen)
        {
            graphics.PreferredBackBufferWidth = (int)res.X;
            graphics.PreferredBackBufferHeight = (int)res.Y;

            graphics.IsFullScreen = fullScreen;

            graphics.ApplyChanges();
        }

        public static int ScreenWidth
        {
            get { return Resolution.GetWidth(); }
        }

        public static int ScreenHeight
        {
            get { return Resolution.GetHeight(); }
        }

        public static int ScreenMiddleX
        {
            get { return Resolution.GetWidth() / 2; }
        }

        public static int ScreenMiddleY
        {
            get { return Resolution.GetHeight() / 2; }
        }

    }
}
