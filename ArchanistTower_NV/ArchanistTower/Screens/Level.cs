using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace ArchanistTower.Screens
{
    class Level
    {
        List<Map> mapList;
        int currentMap;

        public int CurrentMap
        {
            get { return currentMap; }
            set
            {
                currentMap = (int)MathHelper.Clamp(value, 0, mapList.Count - 1);
            }
        }
    
        public Level()
        {
            mapList = new List<Map>();
        }

        public void AddMap(Map map)
        {
            mapList.Add(map);
        }

        

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Globals.spriteBatch.Begin();
            mapList[CurrentMap].Draw(spriteBatch);
            Globals.spriteBatch.End();
        }
    }
}
