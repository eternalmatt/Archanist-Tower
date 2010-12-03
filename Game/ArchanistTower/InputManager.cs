using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace ArchanistTower
{
    public enum KeyPressedState
    {
        Pressed,
        Released,
        JustPressed,
        JustReleased
    }

    public enum GamePadButtonState
    {
        Pressed,
        Released,
        JustPressed,
        JustReleased
    }

    public enum Thumbsticks
    {
        Left,
        Right
    }

    public enum ThumbstickDirection
    {
        Down,
        Left,
        Right,
        Up
    }

    public class InputManager
    {
#if WINDOWS
        private KeyboardState NewKeyState;
        private KeyboardState OldKeyState;

        private MouseState OldMouseState;
        private MouseState NewMouseState;
        private float mouse_move_x;
        private float mouse_move_y;

        private bool mouse_locked;
#endif

        private GamePadState[] NewGamePadState;
        private GamePadState[] OldGamePadState;

        public InputManager()
        {
#if WINDOWS
            NewKeyState = new KeyboardState();
            OldKeyState = new KeyboardState();

            mouse_locked = false;

            Mouse.SetPosition(Globals.ScreenMiddleX, Globals.ScreenMiddleY);
            OldMouseState = Mouse.GetState();
            NewMouseState = OldMouseState;

            mouse_move_x = 0f;
            mouse_move_y = 0f;

#endif

            NewGamePadState = new GamePadState[4];
            OldGamePadState = new GamePadState[4];

            for (int i = 0; i < 4; i++)
            {
                NewGamePadState[i] = new GamePadState();
                OldGamePadState[i] = new GamePadState();
            }
        }

        public KeyPressedState GetKeyPressedState(Keys input)
        {
            if (NewKeyState.IsKeyDown(input))
            {
                if (OldKeyState.IsKeyDown(input))
                    return KeyPressedState.Pressed;
                else
                    return KeyPressedState.JustPressed;
            }
            else
            {
                if (OldKeyState.IsKeyDown(input))
                    return KeyPressedState.JustReleased;
                else
                    return KeyPressedState.Released;
            }
        }

        public GamePadButtonState GetGamepadButtonState(PlayerIndex pi, Buttons input)
        {
            if (NewGamePadState[(int)pi].IsButtonDown(input))
            {
                if (OldGamePadState[(int)pi].IsButtonDown(input))
                    return GamePadButtonState.Pressed;
                else
                    return GamePadButtonState.JustPressed;
            }
            else
            {
                if (OldGamePadState[(int)pi].IsButtonDown(input))
                    return GamePadButtonState.JustReleased;
                else
                    return GamePadButtonState.Released;
            }
        }

        public GamePadButtonState ThumbstickState(PlayerIndex pi, Thumbsticks t, ThumbstickDirection d, float val)
        {
            if (t == Thumbsticks.Left)
            {
                if (d == ThumbstickDirection.Left)
                {
                    if (NewGamePadState[(int)pi].ThumbSticks.Left.X < -val)
                    {
                        if (OldGamePadState[(int)pi].ThumbSticks.Left.X < -val)
                            return GamePadButtonState.Pressed;
                        else
                            return GamePadButtonState.JustPressed;
                    }
                    else
                    {
                        if (OldGamePadState[(int)pi].ThumbSticks.Left.X < -val)
                            return GamePadButtonState.JustReleased;
                        else
                            return GamePadButtonState.Released;
                    }
                }
                else if (d == ThumbstickDirection.Right)
                {
                    if (NewGamePadState[(int)pi].ThumbSticks.Left.X > val)
                    {
                        if (OldGamePadState[(int)pi].ThumbSticks.Left.X > val)
                            return GamePadButtonState.Pressed;
                        else
                            return GamePadButtonState.JustPressed;
                    }
                    else
                    {
                        if (OldGamePadState[(int)pi].ThumbSticks.Left.X > val)
                            return GamePadButtonState.JustReleased;
                        else
                            return GamePadButtonState.Released;
                    }
                }
                else if (d == ThumbstickDirection.Down)
                {
                    if (NewGamePadState[(int)pi].ThumbSticks.Left.Y < -val)
                    {
                        if (OldGamePadState[(int)pi].ThumbSticks.Left.Y < -val)
                            return GamePadButtonState.Pressed;
                        else
                            return GamePadButtonState.JustPressed;
                    }
                    else
                    {
                        if (OldGamePadState[(int)pi].ThumbSticks.Left.Y < -val)
                            return GamePadButtonState.JustReleased;
                        else
                            return GamePadButtonState.Released;
                    }
                }
                else
                {
                    if (NewGamePadState[(int)pi].ThumbSticks.Left.Y > val)
                    {
                        if (OldGamePadState[(int)pi].ThumbSticks.Left.Y > val)
                            return GamePadButtonState.Pressed;
                        else
                            return GamePadButtonState.JustPressed;
                    }
                    else
                    {
                        if (OldGamePadState[(int)pi].ThumbSticks.Left.Y > val)
                            return GamePadButtonState.JustReleased;
                        else
                            return GamePadButtonState.Released;
                    }
                }
            }
            else
            {
                if (d == ThumbstickDirection.Left)
                {
                    if (NewGamePadState[(int)pi].ThumbSticks.Right.X < -val)
                    {
                        if (OldGamePadState[(int)pi].ThumbSticks.Right.X < -val)
                            return GamePadButtonState.Pressed;
                        else
                            return GamePadButtonState.JustPressed;
                    }
                    else
                    {
                        if (OldGamePadState[(int)pi].ThumbSticks.Right.X < -val)
                            return GamePadButtonState.JustReleased;
                        else
                            return GamePadButtonState.Released;
                    }
                }
                else if (d == ThumbstickDirection.Right)
                {
                    if (NewGamePadState[(int)pi].ThumbSticks.Right.X > val)
                    {
                        if (OldGamePadState[(int)pi].ThumbSticks.Right.X > val)
                            return GamePadButtonState.Pressed;
                        else
                            return GamePadButtonState.JustPressed;
                    }
                    else
                    {
                        if (OldGamePadState[(int)pi].ThumbSticks.Right.X > val)
                            return GamePadButtonState.JustReleased;
                        else
                            return GamePadButtonState.Released;
                    }
                }
                else if (d == ThumbstickDirection.Down)
                {
                    if (NewGamePadState[(int)pi].ThumbSticks.Right.Y < -val)
                    {
                        if (OldGamePadState[(int)pi].ThumbSticks.Right.Y < -val)
                            return GamePadButtonState.Pressed;
                        else
                            return GamePadButtonState.JustPressed;
                    }
                    else
                    {
                        if (OldGamePadState[(int)pi].ThumbSticks.Right.Y < -val)
                            return GamePadButtonState.JustReleased;
                        else
                            return GamePadButtonState.Released;
                    }
                }
                else
                {
                    if (NewGamePadState[(int)pi].ThumbSticks.Right.Y > val)
                    {
                        if (OldGamePadState[(int)pi].ThumbSticks.Right.Y > val)
                            return GamePadButtonState.Pressed;
                        else
                            return GamePadButtonState.JustPressed;
                    }
                    else
                    {
                        if (OldGamePadState[(int)pi].ThumbSticks.Right.Y > val)
                            return GamePadButtonState.JustReleased;
                        else
                            return GamePadButtonState.Released;
                    }
                }
            }
        }

#if WINDOWS
        public bool KeyJustPressed(Keys k)
        {
            if (GetKeyPressedState(k) == KeyPressedState.JustPressed)
                return true;
            else
                return false;
        }

        public bool KeyPressed(Keys k)
        {
            KeyPressedState state = GetKeyPressedState(k);

            if (state == KeyPressedState.Pressed || state == KeyPressedState.JustPressed)
                return true;
            else
                return false;
        }

        public bool KeyJustReleased(Keys k)
        {
            if (GetKeyPressedState(k) == KeyPressedState.JustReleased)
                return true;
            else
                return false;
        }

        public bool KeyReleased(Keys k)
        {
            KeyPressedState state = GetKeyPressedState(k);

            if (state == KeyPressedState.Released || state == KeyPressedState.JustReleased)
                return true;
            else
                return false;
        }

        public float MouseX
        {
            get { return NewMouseState.X; }
        }

        public float MouseY
        {
            get { return NewMouseState.Y; }
        }

        public float MouseMoveX
        {
            get { return mouse_move_x; }
        }

        public float MouseMoveY
        {
            get { return mouse_move_y; }
        }

        public bool MouseLocked
        {
            get { return mouse_locked; }
            set { mouse_locked = value; }
        }
#endif

        public bool ButtonJustPressed(PlayerIndex pi, Buttons b)
        {
            if (GetGamepadButtonState(pi, b) == GamePadButtonState.JustPressed)
                return true;
            else
                return false;
        }

        public bool ButtonPressed(PlayerIndex pi, Buttons b)
        {
            GamePadButtonState state = GetGamepadButtonState(pi, b);

            if (state == GamePadButtonState.Pressed || state == GamePadButtonState.JustPressed)
                return true;
            else
                return false;
        }

        public bool ButtonJustReleased(PlayerIndex pi, Buttons b)
        {
            if (GetGamepadButtonState(pi, b) == GamePadButtonState.JustReleased)
                return true;
            else
                return false;
        }

        public bool ButtonReleased(PlayerIndex pi, Buttons b)
        {
            GamePadButtonState state = GetGamepadButtonState(pi, b);

            if (state == GamePadButtonState.Released || state == GamePadButtonState.JustReleased)
                return true;
            else
                return false;
        }

        public void Update()
        {
#if WINDOWS
            OldKeyState = NewKeyState;
            NewKeyState = Keyboard.GetState();
            if (KeyJustPressed(Keys.I))
                Globals.I_AM_INVINCIBLE = !Globals.I_AM_INVINCIBLE;
            if (KeyJustPressed(Keys.U))
                Globals.UNLIMITED_MANA = !Globals.UNLIMITED_MANA;

            NewMouseState = Mouse.GetState();
            mouse_move_x = 0f;
            mouse_move_y = 0f;

            if (OldMouseState != NewMouseState)
            {
                mouse_move_x = NewMouseState.X - OldMouseState.X;
                mouse_move_y = NewMouseState.Y - OldMouseState.Y;
                if (mouse_locked)
                    Mouse.SetPosition(Globals.ScreenMiddleX, Globals.ScreenMiddleY);
            }
#endif

            for (int i = 0; i < 4; i++)
            {
                if (GamePad.GetState((PlayerIndex)i).IsConnected)
                {
                    OldGamePadState[i] = NewGamePadState[i];
                    NewGamePadState[i] = GamePad.GetState((PlayerIndex)i);
                }
            }
        }

        public void ClearStates()
        {
            OldKeyState = Keyboard.GetState();
            NewKeyState = Keyboard.GetState();

            NewMouseState = Mouse.GetState();
            OldMouseState = Mouse.GetState();

            for (int i = 0; i < 4; i++)
            {
                if (GamePad.GetState((PlayerIndex)i).IsConnected)
                {
                    OldGamePadState[i] = GamePad.GetState((PlayerIndex)i);
                    NewGamePadState[i] = GamePad.GetState((PlayerIndex)i);
                }
            }
        }

 
    }
}