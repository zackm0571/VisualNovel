using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
namespace VisualNovel.Managers
{
   public  class SoundManager
    {
       private Dictionary<string, SoundEffect> sfx;
        public SoundManager()
        {
            sfx = new Dictionary<string, SoundEffect>();
        }

        public void Add(string key, SoundEffect effect)
        {
            sfx.Add(key, effect);
            
        }

        public SoundEffect GetSong(string key)
        {
            if(sfx.Keys.Contains(key))
            {
                return sfx[key];
            }

            return null;
        }
        
    }
}
