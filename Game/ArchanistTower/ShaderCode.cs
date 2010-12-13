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
    /// <summary>
    /// The shader code for the game.  This takes the screen and first grayscales it, then restores color based on the crystals found in the map.
    /// </summary>
    public class ShaderCode : Microsoft.Xna.Framework.Game
    {
        float width, height, crysX, crysY;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Our effect object, this is where our shader will be loaded and compiled
        Effect effect;
        public static Effect effectPost;

        RenderTarget2D renderTarget;
        Texture2D SceneTexture;

        public ShaderCode()
        {
            // TODO: Construct any child components here
            graphics = Globals.graphics;
            //Initialize();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize()
        {
            // TODO: Add your initialization code here
            width = graphics.GraphicsDevice.Viewport.Width;
            height = graphics.GraphicsDevice.Viewport.Height;


            // Load and compile our Shader into our Effect instance.

            effect = Globals.content.Load<Effect>("Shaders/WaterTest");
            effectPost = Globals.content.Load<Effect>("Shaders/GrayingEffect");
        }
        public void LoadContent()
        {
            spriteBatch = Globals.spriteBatch;
            PresentationParameters pp = graphics.GraphicsDevice.PresentationParameters;
            renderTarget = new RenderTarget2D(graphics.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, 1,
            graphics.GraphicsDevice.DisplayMode.Format);
            effectPost.Parameters["powerGreen"].SetValue(Globals.green); 
            effectPost.Parameters["powerRed"].SetValue(Globals.red); 
            effectPost.Parameters["powerBlue"].SetValue(Globals.blue);
            crysX = Globals.crysLoc.X;
            crysY = Globals.crysLoc.Y;
            effectPost.Parameters["rectangleA"].SetValue(Globals.crysLoc);

        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update()
        {
            effectPost.Parameters["powerGreen"].SetValue(Globals.green);
            effectPost.Parameters["powerRed"].SetValue(Globals.red);
            effectPost.Parameters["powerBlue"].SetValue(Globals.blue);
            effectPost.Parameters["rectangleA"].SetValue(Globals.crysLoc);
            if (Globals.crysTex != null)
                effectPost.Parameters["textureA"].SetValue(Globals.crysTex);
        }
        //Place before draws
        public void DrawSetup()
        {
            graphics.GraphicsDevice.SetRenderTarget(0, renderTarget);
        }
        public void Draw()
        {
            //sets screen as texture for shader
            graphics.GraphicsDevice.SetRenderTarget(0, null);
            SceneTexture = renderTarget.GetTexture();


            // Render the scene with Edge Detection, using the render target from last frame.
            graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.DarkSlateBlue, 1.0f, 0);
            //Globals.spriteBatch.End();
            spriteBatch.Begin(SpriteBlendMode.None, SpriteSortMode.Immediate, SaveStateMode.SaveState);
            {
                // Apply the post process shader
                effectPost.Begin();
                {
                    effectPost.CurrentTechnique.Passes[0].Begin();
                    {
                        spriteBatch.Draw(SceneTexture, new Rectangle(0, 0, 800, 600), Color.White);
                        effectPost.CurrentTechnique.Passes[0].End();
                    }
                }
                effectPost.End();

            }
            spriteBatch.End();
        }
    }
}