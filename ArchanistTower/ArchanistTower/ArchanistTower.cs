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

using TileEngine;

namespace ArchanistTower
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ArchanistTower : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        TileMap tileMap = new TileMap();

        Camera camera = new Camera();

        TileLayer tileLayer;

        public ArchanistTower()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize(); //must happen first to insure that each layer has been added

            tileMap.Layers.Add(tileLayer);            
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            tileLayer = TileLayer.FromFile(Content, "Content/Layers/Layer1.layer");
        }
        protected override void UnloadContent()
        { }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //update camera position
            camera.Update();            

            //get width and height of viewport
            int screenWidth = GraphicsDevice.Viewport.Width;
            int screenHeight = GraphicsDevice.Viewport.Height;

            //Restrict where the camera can go. (i.e. not off the map)
            if (camera.Position.X > tileLayer.WidthInPixels - screenWidth)
                camera.Position.X = tileLayer.WidthInPixels - screenWidth;
            if (camera.Position.Y > tileLayer.HeightInPixels - screenHeight)
                camera.Position.Y = tileLayer.HeightInPixels - screenHeight;
            
            if (camera.Position.X < 0)
                camera.Position.X = 0;
            if (camera.Position.Y < 0)
                camera.Position.Y = 0;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            tileMap.Draw(spriteBatch, camera);

            base.Draw(gameTime);
        }
    }
}
