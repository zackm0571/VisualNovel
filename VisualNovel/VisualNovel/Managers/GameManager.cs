using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using VisualNovel.Screens;

namespace VisualNovel.Managers
{
   public static class GameManager
    {
        public static ScreenManager screenManager;
        public static RenderManager renderManager;
        public static Vector2 screenDimensions;
        public static float aspectRatio = 1;
        public static float scale = 1.3f;
        public static SoundManager soundManager;
        public static Dictionary<string, GameScreen> screens;
        public static Dictionary<string, UIElement> speechBubbles, sideImages;
        public static Dictionary<string, Texture2D> backgrounds;
        public static int currentScene = 1, totalScenes = 8;
        public static Game1 game1;
        
    }
}
