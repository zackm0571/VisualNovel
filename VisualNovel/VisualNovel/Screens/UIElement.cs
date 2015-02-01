using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VisualNovel.Managers;
namespace VisualNovel.Screens
{
   public  class UIElement
    {
        public Vector2 position;
        public Texture2D texture;
        public bool drawWithScale;
        private float localScale;
        public UIElement(Vector2 position, Texture2D texture, bool drawWithScale)
        {
            this.position = position;
            this.texture = texture;
            this.drawWithScale = drawWithScale;

            localScale = (!drawWithScale) ? 1 :
                GameManager.aspectRatio;
        }

        public UIElement(Vector2 position, Texture2D texture, float Scale)
        {
            this.position = position;
            this.texture = texture;
            this.drawWithScale = true;

            localScale = Scale * GameManager.aspectRatio;
        }
        public void Draw()
        {
            SpriteBatch spriteBatch = GameManager.screenManager.spriteBatch;

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            
            spriteBatch.Draw(texture, position, new Rectangle(0, 
                0, texture.Width, texture.Height), Color.White, 
                0.0f, Vector2.Zero, localScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }
    }
}
