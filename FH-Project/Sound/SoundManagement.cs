using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FH_Project;

public static class SoundManagement
{
    public static SoundEffect SlimeHit {  get; set; }
    public static SoundEffect MainMusic { get; set; }
    public static SoundEffect Hit {  get; set; }
    public static Song HomeBaseMusic { get; set; }
    public static SoundEffect SwordSlash { get; set; }


    public static void PlaySound(SoundEffect Sound)
    {
        Sound.Play();
    }

    public static void PlayMusic(Song song)
    {
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = 1;
        MediaPlayer.Play(song);
    }
}