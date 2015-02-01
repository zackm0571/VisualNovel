using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using VisualNovel.Managers;
using Microsoft.Xna.Framework;
namespace VisualNovel.Screens
{
    public class GameScreen
    {
        public Texture2D background;
        protected List<UIElement> elements;
        public ControllerManager controllerManager;
        
        public GameScreen()
        {
            elements = new List<UIElement>();
        
            controllerManager = new ControllerManager();
        }

        public void AddUIElement(UIElement element)
        {
            elements.Add(element);
        }
        public virtual void LoadContent()
        {
            
        }

        public virtual void Update()
        {
            controllerManager.Update();
        }

        public virtual void Draw()
        {
            SpriteBatch spriteBatch = GameManager.screenManager.spriteBatch;

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            if(background != null)
            {
                spriteBatch.Draw(background, new Rectangle(0, 0, (int)GameManager.screenDimensions.X, (int)GameManager.screenDimensions.Y), Color.White);
            }

            spriteBatch.End();

            foreach (UIElement element in elements)
            {
                if (element != null)
                {
                    element.Draw();
                }
            }
        }
    }
}
