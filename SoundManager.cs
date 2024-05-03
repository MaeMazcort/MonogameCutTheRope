using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public class SoundManager
    {
        public static Song song;

        public static SoundEffect cutSound;
        public static SoundEffect eatSound;
        public static SoundEffect starSound;
        public static SoundEffect grabPointSound;

        public static SoundEffectInstance instCut;
        public static SoundEffectInstance instEat;
        public static SoundEffectInstance instStar;
        public static SoundEffectInstance instGrabPoint;

        public static void PlaySong()
        {
            if(MediaPlayer.State != MediaState.Playing && MediaPlayer.GameHasControl)
            {
                MediaPlayer.Play(song);
            }
        }

        public static void StopSong()
        {
            if(MediaPlayer.State == MediaState.Playing && MediaPlayer.GameHasControl)
            {
                MediaPlayer.Stop();
            }
        }
    }
}
