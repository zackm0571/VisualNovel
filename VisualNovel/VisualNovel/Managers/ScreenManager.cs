using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VisualNovel.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace VisualNovel.Managers
{
  public  class ScreenManager
    {
        public List<GameScreen> screensToUpdate;
        public SpriteBatch spriteBatch;
        public SpriteFont gameFont12, headerFont;
        public ContentManager content;
        public ScreenManager()
        {
            screensToUpdate = new List<GameScreen>();
        }

        public void AddScreen(GameScreen screen)
        {
            screen.LoadContent();
            screensToUpdate.Add(screen);
        }

        public void RemoveScreen(GameScreen screen)
        {
            List<GameScreen> tempScreens = screensToUpdate;
            tempScreens.Remove(screen);
            screensToUpdate = tempScreens;
        }

        public void ClearScreens()
        {
            screensToUpdate = new List<GameScreen>();
        }
        public void Update(GameTime gameTime)
        {
            foreach (GameScreen screen in screensToUpdate)
            {
                screen.Update();
                
            }
        }
        public void Draw()
        {
            
            foreach (GameScreen screen in screensToUpdate)
            {
                screen.Draw();
            }
            
        }
    }
}
