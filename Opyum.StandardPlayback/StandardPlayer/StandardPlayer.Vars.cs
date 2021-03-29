using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Opyum.StandardPlayback
{
    public partial class StandardPlayer
    {
        private string _path { get; set; }

        public float Volume { get; private set; }

        private BlockAlignReductionStream blockAlignStream { get; set; } = null;

        private WaveStream pcm { get; set; } = null;

        private DirectSoundOut output { get; set; } = null;

        private WaveFileReader wavereader { get; set; } = null;

        private AiffFileReader aiffreader { get; set; } = null;

        private AudioFileReader audioreader { get; set; } = null;

        public PlaybackState PlaybackState
        {
            get
            {
                if (output.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                {
                    return PlaybackState.Playing;
                }
                else if (output.PlaybackState == NAudio.Wave.PlaybackState.Paused)
                {
                    return PlaybackState.Paused;
                }
                else return PlaybackState.Stopped;
            }
        }

        
    }
}
