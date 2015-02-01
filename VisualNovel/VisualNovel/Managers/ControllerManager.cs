using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace VisualNovel.Managers
{
   public  class ControllerManager
    {
        public delegate void ControllerAction();

        public ControllerAction left;
        public ControllerAction right;
        public ControllerAction up;
        public ControllerAction down;

        public ControllerAction a;
        public ControllerAction b;
        public ControllerAction x;
        public ControllerAction y;

        protected GamePadState gamePad;
        protected GamePadState[] previousState = new GamePadState[4];
        protected int index;
        public ControllerManager()
        {
            
        }

        public void Update()
        {
            for (int i = 0; i < 4; i++ )
            {
                index = i;
                switch(index)
                {
                    case 0:
                        gamePad = GamePad.GetState(PlayerIndex.One);
                        break;
                        case 1:
                        gamePad = GamePad.GetState(PlayerIndex.Two);
                        break;
                        case 2:
                        gamePad = GamePad.GetState(PlayerIndex.Three);
                        break;
                        case 3:
                        gamePad = GamePad.GetState(PlayerIndex.Four);
                        break;
                }
                if (gamePad.ThumbSticks.Left.Y < -0.25f)
                {
                    if (up != null)
                    {
                        up();
                    }
                }

                if (gamePad.ThumbSticks.Left.Y > 0.25f)
                {
                    if (down != null)
                    {
                        down();
                    }
                }

                if (gamePad.ThumbSticks.Left.X > 0.25f)
                {
                    if (right != null)
                    {
                        right();
                    }
                }

                if (gamePad.ThumbSticks.Left.X < -0.25f)
                {
                    if (left != null)
                    {
                        left();
                    }
                }

                if (gamePad.IsButtonDown(Buttons.A) && previousState[index].IsButtonUp(Buttons.A))
                {
                    if (a != null)
                    {
                        a();
                    }
                }

                if (gamePad.IsButtonDown(Buttons.B) && previousState[index].IsButtonUp(Buttons.B))
                {
                    if (b != null)
                    {
                        b();
                    }
                }

                if (gamePad.IsButtonDown(Buttons.X) && previousState[index].IsButtonUp(Buttons.X))
                {
                    if (x != null)
                    {
                        x();
                    }
                }

                if (gamePad.IsButtonDown(Buttons.Y) && previousState[index].IsButtonUp(Buttons.Y))
                {
                    if (y != null)
                    {
                        y();
                    }
                }
                previousState[index] = gamePad;
            }
        }
    }
}
