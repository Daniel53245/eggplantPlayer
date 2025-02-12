using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eggplantPlayer
{
    using System;
    using System.Threading.Tasks;

    public class Animation
    {
        private string[] frames;
        private int frameIndex;
        private int frameDelay; // Delay in milliseconds

        public Animation(string[] frameSequence, int delay = 300)
        {
            frames = frameSequence;
            frameIndex = 0;
            frameDelay = delay;
        }

        public string NextFrame()
        {
            string frame = frames[frameIndex];
            frameIndex = (frameIndex + 1) % frames.Length; // Loop back when reaching the end
            return frame;
        }

        public int GetFrameDelay()
        {
            return frameDelay;
        }
    };

    public static class AnimationSequences
    {
        public static string[] SpinningVinyl = new string[]
        {
        "    .-~~~~-.\n" +
        "  /        \\\n" +
        " | (●)  (●) |\n" +
        "  \\   ()   /\n" +
        "    '-~~~~-'",

        "    .-~~~~-.\n" +
        "  /        \\\n" +
        " |  (●) (●)  |\n" +
        "  \\   ()   /\n" +
        "    '-~~~~-'",

        "    .-~~~~-.\n" +
        "  /        \\\n" +
        " |   (●) (●) |\n" +
        "  \\   ()   /\n" +
        "    '-~~~~-'",
        };

        public static string[] LoadingBar = new string[]
        {
        "[■□□□□□□□□□] Now Playing...",
        "[■■□□□□□□□□] Now Playing...",
        "[■■■□□□□□□□] Now Playing...",
        "[■■■■□□□□□□] Now Playing...",
        "[■■■■■□□□□□] Now Playing...",
        "[■■■■■■□□□□] Now Playing...",
        "[■■■■■■■□□□] Now Playing...",
        "[■■■■■■■■□□] Now Playing...",
        "[■■■■■■■■■□] Now Playing...",
        "[■■■■■■■■■■] Now Playing..."
        };

        public static string[] Waveform = new string[]
        {
        " ▄    ▄▄▄    ▄  ",
        " ▄▄   ▄  ▄   ▄▄ ",
        " ▄▄▄  ▄   ▄  ▄▄▄",
        "  ▄   ▄▄▄▄▄  ▄  ",
        " ▄▄   ▄   ▄  ▄▄ ",
        " ▄▄▄  ▄   ▄  ▄▄▄",
        };

        public static string[] MovieReel = new string[]
        {
        " 🎥  🎞️ |------| ",
        " 🎥  🎞️ /------\\ ",
        " 🎥  🎞️ |------| ",
        " 🎥  🎞️ \\------/ "
        };
    }



}
