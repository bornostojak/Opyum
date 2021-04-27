using System;
using System.IO;
using System.Net;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Opyum.StandardPlayback
{
    public partial class StandardPlayer
    {

        public void Play(string path)
        {
            Dispose();

            _path = path;

            if (System.IO.Path.GetExtension(_path) == ".mp3")
            {
                pcm = WaveFormatConversionStream.CreatePcmStream(new Mp3FileReader(_path));
                blockAlignStream = new BlockAlignReductionStream(pcm);
                output.Init(blockAlignStream);
            }
            else if (System.IO.Path.GetExtension(_path) == ".wav")
            {
                wavereader = new WaveFileReader(_path);
                output.Init(new WaveChannel32(wavereader));

            }
            else if (System.IO.Path.GetExtension(_path) == ".aif")
            {
                aiffreader = new AiffFileReader(_path);
                output.Init(new WaveChannel32(aiffreader));
            }
            else
            {
                audioreader = new AudioFileReader(_path);
                output.Init(new WaveChannel32(audioreader));
            }

            output.Play();


        }

        public void PausePlay()
        {
            if (output != null)
            {
                if (output.PlaybackState == NAudio.Wave.PlaybackState.Playing)
                {
                    output.Pause();
                }
                else if (output.PlaybackState == NAudio.Wave.PlaybackState.Paused)
                {
                    output.Play();
                    //if (waveOut != null)
                    //{
                    //    this.Play();
                    //}
                }
            }
            //if (waveOut != null)
            //{
            //    if (playbackState == StreamingPlaybackState.Playing)
            //    {
            //        if (waveOut != null)
            //        {
            //            this.PauseStream();
            //        }
            //    }
            //    else if (playbackState == StreamingPlaybackState.Paused)
            //    {
            //        if (waveOut != null)
            //        {
            //            this.Play();
            //        }
            //    }
            //}
        }

        public void Pause()
        {
            output?.Pause();
            //if (waveOut != null)
            //{
            //    this.PauseStream();
            //}
        }

        public void Stop()
        {
            output?.Stop();
            //if (waveOut != null)
            //{
            //    this.StopPlayback();
            //}
        }
        
    }
}
