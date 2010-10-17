using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TileEditor
{
    using Image = System.Drawing.Image;
    using TileEngine;

    public partial class Form1 : Form
    {
        //Array of acceptable file types
        string[] imageExtensions = new string[]
        {
            ".jpg", ".png", ".tga",
        };

        //Used for scroll bars
        int maxWidth = 0, maxHeight = 0;

        SpriteBatch spriteBatch;
        Texture2D tileTexture;
        Camera camera = new Camera();
        TileLayer currentLayer;

        int cellX, cellY;

        TileMap tileMap = new TileMap();

        Dictionary<string, Texture2D> textureDict = new Dictionary<string, Texture2D>();
        Dictionary<string, Image> previewDict = new Dictionary<string, Image>();
        Dictionary<string, TileLayer> layerDict = new Dictionary<string, TileLayer>();

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


            saveFileDialog1.Filter = "Layer File|*.layer";

            Mouse.WindowHandle = tileDisplay1.Handle;
        }

        void tileDisplay1_OnInitialize(object sender, EventArgs e)
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            tileTexture = Texture2D.FromFile(GraphicsDevice, "Content/tile.png");
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


            int mx = Mouse.GetState().X;
            int my = Mouse.GetState().Y;

            if (currentLayer != null)
            {
                if (mx >= 0 && mx < tileDisplay1.Width &&
                    my >= 0 && my < tileDisplay1.Height)
                {
                    cellX = mx / TileLayer.TileWidth;
                    cellY = my / TileLayer.TileHeight;

                    cellX = (int)MathHelper.Clamp(cellX, 0, currentLayer.Width - 1);
                    cellY = (int)MathHelper.Clamp(cellY, 0, currentLayer.Height - 1);

                    if (Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        if (drawRadioButton.Checked && textureListBox.SelectedItem != null)
                        {
                            Texture2D texture = textureDict[textureListBox.SelectedItem as string];

                            int index = currentLayer.IsUsingTexture(texture);

                            if (index == -1) //texture had not yet been used
                            {
                                currentLayer.AddTexture(texture);  //add new texture into the layer
                                index = currentLayer.IsUsingTexture(texture);  //get index that the layer assigned to the texture
                            }

                            currentLayer.SetCellIndex(cellX, cellY, index);  //draw index to the cell
                        }
                        else if (eraseRadioButton.Checked)
                        {
                            currentLayer.SetCellIndex(cellX, cellY, -1);
                        }
                    }
                }
                else
                {
                    cellX = cellY = -1;
                }


            }
        }

        private void Render()
        {
            GraphicsDevice.Clear(Color.Black);

            foreach (TileLayer layer in tileMap.Layers)
            {
                layer.Draw(spriteBatch, camera);

                spriteBatch.Begin();
                for (int y = 0; y < layer.Height; y++)
                {
                    for (int x = 0; x < layer.Width; x++)
                    {
                        if (layer.GetCellIndex(x, y) == -1)  //Find all empty tiles
                        {
                            //Draw tile box in empty tile positions
                            spriteBatch.Draw(
                            tileTexture,
                            new Rectangle(
                                x * TileLayer.TileWidth - (int)camera.Position.X,
                                y * TileLayer.TileHeight - (int)camera.Position.Y,
                                TileLayer.TileWidth,
                                TileLayer.TileHeight),
                            Color.White);
                        }
                    }
                }
                spriteBatch.End();
            }
            if (currentLayer != null)
            {
                if (cellX != -1 && cellY != -1)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(
                            tileTexture,
                            new Rectangle(
                                cellX * TileLayer.TileWidth - (int)camera.Position.X,
                                cellY * TileLayer.TileHeight - (int)camera.Position.Y,
                                TileLayer.TileWidth,
                                TileLayer.TileHeight),
                            Color.Red);
                    spriteBatch.End();
                }
            }
        }

        private void browseForContentButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                contentPathTextBox.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        #region Menu

        private void newTileMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Layer File|*.layer";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;

                string[] textureNames;
                TileLayer layer = TileLayer.FromFile(filename, out textureNames);

                if (layer.WidthInPixels > tileDisplay1.Width)
                {
                    maxWidth = (int)Math.Max(layer.Width, maxWidth);
                    hScrollBar1.Visible = true;
                    hScrollBar1.Minimum = 0;
                    hScrollBar1.Maximum = maxWidth;
                }

                if (layer.HeightInPixels > tileDisplay1.Height)
                {
                    maxHeight = (int)Math.Max(layer.Height, maxHeight);
                    vScrollBar1.Visible = true;
                    vScrollBar1.Minimum = 0;
                    vScrollBar1.Maximum = maxHeight;
                }

                layerDict.Add(Path.GetFileName(filename), layer);
                tileMap.Layers.Add(layer);
                layerListBox.Items.Add(Path.GetFileName(filename));

                foreach (string textureName in textureNames)
                {
                    string fullPath = contentPathTextBox.Text + "/" + textureName;

                    foreach (string ext in imageExtensions)
                    {
                        if (File.Exists(fullPath + ext))
                        {
                            fullPath += ext;
                            break;
                        }
                    }
                    Texture2D tex = Texture2D.FromFile(GraphicsDevice, fullPath);
                    Image image = Image.FromFile(fullPath);

                    textureDict.Add(textureName, tex);
                    previewDict.Add(textureName, image);

                    textureListBox.Items.Add(textureName);

                    layer.AddTexture(tex);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (layerListBox.SelectedItem != null)
            {
                string filename = layerListBox.SelectedItem as string;
                saveFileDialog1.FileName = filename;

                TileLayer tileLayer = layerDict[filename];

                Dictionary<int, string> utilizedTextures = new Dictionary<int, string>();

                foreach (string textureName in textureListBox.Items)
                {
                    int index = tileLayer.IsUsingTexture(textureDict[textureName]);

                    if (index != -1)
                    {
                        utilizedTextures.Add(index, textureName);
                    }
                }

                List<string> utilizedTextureList = new List<string>();

                for (int i = 0; i < utilizedTextures.Count; i++)
                    utilizedTextureList.Add(utilizedTextures[i]);

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    tileLayer.Save(saveFileDialog1.FileName, utilizedTextureList.ToArray());
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion //Menu

        private void textureListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (textureListBox.SelectedItem != null)
            {
                texturePreviewBox.Image = previewDict[textureListBox.SelectedItem as string];
            }
        }

        private void layerListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (layerListBox.SelectedItem != null)
                currentLayer = layerDict[layerListBox.SelectedItem as string];
        }

        private void addLayerButton_Click(object sender, EventArgs e)
        {
            NewLayerForm form = new NewLayerForm();

            form.ShowDialog();

            if (form.OKPressed)
            {
                TileLayer tileLayer = new TileLayer(
                    int.Parse(form.width.Text),
                    int.Parse(form.height.Text));

                layerDict.Add(form.name.Text, tileLayer);
                tileMap.Layers.Add(tileLayer);
                layerListBox.Items.Add(form.name.Text);
            }
        }

        private void removeLayerButton_Click(object sender, EventArgs e)
        {
            if (currentLayer != null)
            {
                string filename = layerListBox.SelectedItem as string;

                tileMap.Layers.Remove(currentLayer);
                layerDict.Remove(filename);
                layerListBox.Items.Remove(layerListBox.SelectedItem);
            }
        }

        private void addTextureButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "JPG Image|*.jpg|PNG Image|*.png|TGA Image|*.tga";
            openFileDialog1.InitialDirectory = contentPathTextBox.Text;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;

                Texture2D texture = Texture2D.FromFile(GraphicsDevice, filename);
                Image image = Image.FromFile(filename);

                filename = filename.Replace(contentPathTextBox.Text + "\\", "");
                filename = filename.Remove(filename.LastIndexOf("."));

                textureListBox.Items.Add(filename);
                textureDict.Add(filename, texture);
                previewDict.Add(filename, image);
            }
        }

        private void removeTextureButton_Click(object sender, EventArgs e)
        {

        }



    }
}