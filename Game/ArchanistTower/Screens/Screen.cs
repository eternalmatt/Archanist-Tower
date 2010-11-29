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


namespace ArchanistTower.Screens
{
    public enum ScreenState
    {
        Active,
        Disabled,
        Destroy,
        Hidden
    }

    public abstract class Screen
    {
        protected ScreenState State;
        protected string Name;

        public void UpdateScreen(GameTime gameTime)
        {
            if (State == ScreenState.Active || State == ScreenState.Hidden)
                Update(gameTime);
        }

        public void DrawScreen()
        {
            if (State == ScreenState.Active || State == ScreenState.Disabled)
                Draw();
        }

        public void DestroyScreen()
        {
            if (State == ScreenState.Destroy)
                Unload();
        }

        public virtual void OnAdd() { }
        protected abstract void Initialize();
        protected abstract void Unload();
        protected abstract void Update(GameTime gameTime);
        protected abstract void Draw();

        public ScreenState GetScreenState() { return State; }
        public string GetScreenName() { return Name; }

        public virtual void Activate() { State = ScreenState.Active; }
        public virtual void Disable() { State = ScreenState.Disabled; }
        public virtual void Hide() { State = ScreenState.Hidden; }
        public virtual void Destroy() { State = ScreenState.Destroy; }

    }
}