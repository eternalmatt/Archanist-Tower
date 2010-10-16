using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TileEngine;

namespace TileEditor
{
    public partial class Form1 : Form
    {
        SpriteBatch spriteBatch;

        Camera camera = new Camera();
        TileLayer tileLayer = new TileLayer(new int[,]
        {
            {1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,},
            {1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,},
            {1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,},
            {1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,},
            {1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,},
            {1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,},
            {1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,},
            {1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,},
            {1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,},
            {1,1,1,1,1,4,1,1,1,1,1,0,0,0,0,4,0,0,0,0,0,0,0,0,0,4,0,0,0,0,},
            {0,0,0,0,0,4,0,0,0,0,0,0,0,0,0,4,0,0,0,0,0,1,1,1,1,4,1,1,1,1,},
            {1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,},
            {1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,},
            {1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,},
            {1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,},
            {1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,0,0,0,0,0,0,0,0,0,4,0,0,0,0,},
            {0,0,0,0,0,4,0,0,0,0,0,0,0,0,0,4,0,0,0,0,0,0,0,0,0,4,0,0,0,0,},
            {0,0,0,0,0,4,0,0,0,0,0,0,0,0,0,4,0,0,0,1,1,1,1,1,1,4,1,1,1,1,},
            {1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,},
            {1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,4,1,1,1,1,},
        });

        public GraphicsDevice GraphicsDevice
        {
            get { return tileDisplay1.GraphicsDevice; }
        }

        public Form1()
        {
            InitializeComponent();

            tileDisplay1.OnInitialize += new EventHandler(tileDisplay1_OnInitialize);
            tileDisplay1.OnDraw += new EventHandler(tileDisplay1_OnDraw);

            Application.Idle += delegate { tileDisplay1.Invalidate(); };
        }
        
        void tileDisplay1_OnInitialize(object sender, EventArgs e)
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            tileLayer.AddTexture(Texture2D.FromFile(GraphicsDevice, "Content/se_free_dirt_texture.jpg"));
            tileLayer.AddTexture(Texture2D.FromFile(GraphicsDevice, "Content/se_free_grass_texture.jpg"));
            tileLayer.AddTexture(Texture2D.FromFile(GraphicsDevice, "Content/se_free_ground_texture.jpg"));
            tileLayer.AddTexture(Texture2D.FromFile(GraphicsDevice, "Content/se_free_mud_texture.jpg"));
            tileLayer.AddTexture(Texture2D.FromFile(GraphicsDevice, "Content/se_free_road_texture.jpg"));
            tileLayer.AddTexture(Texture2D.FromFile(GraphicsDevice, "Content/se_free_rock_texture.jpg"));
            tileLayer.AddTexture(Texture2D.FromFile(GraphicsDevice, "Content/se_free_wood_texture.jpg"));

            if (tileLayer.WidthInPixels > tileDisplay1.Width)
            {
                hScrollBar1.Visible = true;
                hScrollBar1.Minimum = 0;
                hScrollBar1.Maximum = tileLayer.Width;
            }
            if (tileLayer.HeightInPixels > tileDisplay1.Height)
            {
                vScrollBar1.Visible = true;
                vScrollBar1.Minimum = 0;
                vScrollBar1.Maximum = tileLayer.Height;
            }
        }

        void tileDisplay1_OnDraw(object sender, EventArgs e)
        {
            Logic();
            Render();
        }

        private void Logic()
        {
            camera.Position.X = hScrollBar1.Value * TileLayer.TileWidth;
            camera.Position.Y = vScrollBar1.Value * TileLayer.TileHeight;
        }

        private void Render()
        {
            GraphicsDevice.Clear(Color.Black);
            tileLayer.Draw(spriteBatch, camera);
        }

        
    }
}
