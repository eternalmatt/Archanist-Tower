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
using ArchanistTower.Screens;

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
        public static InputManager input;
        public static ScreenManager screenManager;
        public static Random random;
        public static float green;
        public static float red;
        public static float blue;
        public static bool I_AM_INVINCIBLE = false;
        public static bool UNLIMITED_MANA = false;
        public static int controlScheme = 0;


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
            screenManager = new ScreenManager(game);

            Resolution = new ResolutionManager(ResolutionManager.Res800x600);
            SetResolution(Resolution.ObtainResolution(), false);

            input = new InputManager();
            random = new Random();
            ResetColor();
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


        public static Rectangle ScreenRectangle
        {
            get { return new Rectangle((int)camera.Position.X, (int)camera.Position.Y, ScreenWidth, ScreenHeight); }
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

        public static void ResetColor()
        {
            blue = 0;
            green = 0;
            red = 0;
        }

    }
}
