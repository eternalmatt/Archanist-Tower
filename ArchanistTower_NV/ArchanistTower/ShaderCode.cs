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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ShaderCode : Microsoft.Xna.Framework.Game
    {
        float width, height;

        Vector2 blendTexturePos = new Vector2(0);
        Vector2 blendTexturePos2 = new Vector2(0);

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Our effect object, this is where our shader will be loaded and compiled
        Effect effect;
        Effect effectPost;

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
            //blendTexturePosition = Globals.content.;
            effectPost.Parameters["powerGained"].SetValue(Globals.shading);
        }
        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update()
        {
            effectPost.Parameters["powerGained"].SetValue(Globals.shading);
        }
        public void DrawSetup()
        {
            graphics.GraphicsDevice.SetRenderTarget(0, renderTarget);
        }
        public void Draw()
        {
            graphics.GraphicsDevice.SetRenderTarget(0, null);
            SceneTexture = renderTarget.GetTexture();


            // Render the scene with Edge Detection, using the render target from last frame.
            graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.DarkSlateBlue, 1.0f, 0);
            //spriteBatch.End();
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