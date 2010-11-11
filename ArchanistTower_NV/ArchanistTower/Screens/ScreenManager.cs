using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ArchanistTower.Screens
{
    public class ScreenManager : DrawableGameComponent
    {
        List<Screen> ScreenList;
        Queue<Screen> ScreenToAdd;

        public ScreenManager(Game game)
            : base(game)
        {
            ScreenList = new List<Screen>();
            ScreenToAdd = new Queue<Screen>();
        }

        public void AddScreen(Screen newScreen)
        {
            ScreenToAdd.Enqueue(newScreen);
        }

        public Screen FindScreen(string name)
        {
            for(int i = 0; i < ScreenList.Count; i++)
            {
                if(ScreenList[i].GetScreenName().Equals(name))
                    return ScreenList[i];
            }
            return null;
        }

        public override void Update(GameTime gameTime)
        {
            while (ScreenToAdd.Count > 0)
            {
                ScreenList.Add(ScreenToAdd.Dequeue());
                ScreenList[ScreenList.Count - 1].OnAdd();
            }
            for (int i = 0; i < ScreenList.Count; i++)
            {
                Screen w = ScreenList[i];

                w.UpdateScreen(gameTime);

                if (w.GetScreenState() == ScreenState.Destroy)
                {
                    w.DestroyScreen();
                    ScreenList.Remove(w);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            for (int i = 0; i < ScreenList.Count; i++)
                ScreenList[i].DrawScreen();
        }
    }
}
