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

namespace FlightSimLibrary
{
    public class InputHandler
    {
        private struct KeysCheck
        {
            public bool Up;
            public bool Down;
            public bool Left;
            public bool Right;
            public bool Accelerate;
            public bool Deccelerate;
            public bool Boost;
            public KeysCheck(bool useDefault)
            {
                Up = false; Down = false; Left = false; Right = false; Accelerate = false; Deccelerate = false; Boost = false;
            }
        }
        private KeysCheck keysCurrentlyDown;
        private KeysCheck keysPressed;
        private KeyboardState prevKeyState;
        public KeyMapping CurrentKeyMapping { get; set; }
        public InputHandler()
        {
            keysCurrentlyDown = new KeysCheck(true);
            keysPressed = new KeysCheck(true);
            prevKeyState = Keyboard.GetState();
        }
        public void Update(GameTime gameTime)
        {
            KeyboardState curKeyState = Keyboard.GetState();
            if (curKeyState.IsKeyDown(CurrentKeyMapping.Up))
            {
                keysCurrentlyDown.Up = true;
                if (!prevKeyState.IsKeyDown(CurrentKeyMapping.Up))
                    keysPressed.Up = true;
            }
            else
            {
                keysCurrentlyDown.Up = false;
                keysPressed.Up = false;
            }
            if (curKeyState.IsKeyDown(CurrentKeyMapping.Down))
            {
                keysCurrentlyDown.Down = true;
                if (!prevKeyState.IsKeyDown(CurrentKeyMapping.Down))
                    keysPressed.Down = true;
            }
            else
            {
                keysCurrentlyDown.Down = false;
                keysPressed.Down = false;
            }
            if (curKeyState.IsKeyDown(CurrentKeyMapping.Left))
            {
                keysCurrentlyDown.Left = true;
                if (!prevKeyState.IsKeyDown(CurrentKeyMapping.Left))
                    keysPressed.Left = true;
            }
            else
            {
                keysCurrentlyDown.Left = false;
                keysPressed.Left = false;
            }
            if (curKeyState.IsKeyDown(CurrentKeyMapping.Right))
            {
                keysCurrentlyDown.Right = true;
                if (!prevKeyState.IsKeyDown(CurrentKeyMapping.Right))
                    keysPressed.Right = true;
            }
            else
            {
                keysCurrentlyDown.Right = false;
                keysPressed.Right = false;
            }
            if (curKeyState.IsKeyDown(CurrentKeyMapping.Accelerate))
            {
                keysCurrentlyDown.Accelerate = true;
                if (!prevKeyState.IsKeyDown(CurrentKeyMapping.Accelerate))
                    keysPressed.Accelerate = true;
            }
            else
            {
                keysCurrentlyDown.Accelerate = false;
                keysPressed.Accelerate = false;
            }
            if (curKeyState.IsKeyDown(CurrentKeyMapping.Deccelerate))
            {
                keysCurrentlyDown.Deccelerate = true;
                if (!prevKeyState.IsKeyDown(CurrentKeyMapping.Deccelerate))
                    keysPressed.Deccelerate = true;
            }
            else
            {
                keysCurrentlyDown.Deccelerate = false;
                keysPressed.Deccelerate = false;
            }
            if (curKeyState.IsKeyDown(CurrentKeyMapping.Boost))
            {
                keysCurrentlyDown.Boost = true;
                if (!prevKeyState.IsKeyDown(CurrentKeyMapping.Boost))
                    keysPressed.Boost = true;
            }
            else
            {
                keysCurrentlyDown.Boost = false;
                keysPressed.Boost = false;
            }

            prevKeyState = curKeyState;
        }
        public bool isKeyDown(Action key)
        {
            switch (key)
            {
                case Action.Up: return keysPressed.Up;
                case Action.Down: return keysPressed.Down;
                case Action.Left: return keysPressed.Left;
                case Action.Right: return keysPressed.Right;
                case Action.Accelerate: return keysPressed.Accelerate;
                case Action.Deccelerate: return keysPressed.Deccelerate;
                default: return keysPressed.Boost;
            }
        }
        public bool isKeyPressed(Action key)
        {
            switch (key)
            {
                case Action.Up: return keysCurrentlyDown.Up;
                case Action.Down: return keysCurrentlyDown.Down;
                case Action.Left: return keysCurrentlyDown.Left;
                case Action.Right: return keysCurrentlyDown.Right;
                case Action.Accelerate: return keysCurrentlyDown.Accelerate;
                case Action.Deccelerate: return keysCurrentlyDown.Deccelerate;
                default: return keysCurrentlyDown.Boost;
            }
        }
        public static KeyMapping getDefaultKeymapping()
        {
            return new KeyMapping() { Up = Keys.Up, Down = Keys.Down, Left = Keys.Left, Right = Keys.Right, Accelerate = Keys.LeftShift, Deccelerate = Keys.LeftControl, Boost = Keys.Space };
        }
    }
    public struct KeyMapping
    {
        public Keys Up;
        public Keys Down;
        public Keys Left;
        public Keys Right;
        public Keys Accelerate;
        public Keys Deccelerate;
        public Keys Boost;
    }
    public enum Action { Up, Down, Left, Right, Accelerate, Deccelerate, Boost };
    
}
