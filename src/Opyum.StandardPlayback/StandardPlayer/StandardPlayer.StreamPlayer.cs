using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Opyum.Structures.FileSupport;
using NAudio.Wave;
using NAudio;

namespace Opyum.StandardPlayback
{
    public partial class StandardPlayer
    {
        WaitCallback waitCallBack;

        enum Driver
        {
            Normal = 0,
            Asio = 1
        }

        public enum AudioFileType
        {
            Unknown = 0,
            Mp3 = 1,
            Wave = 2
        }

        private Driver DriverToUSer { get; set; } = 0;

        private IWavePlayer waveOut;

        public IFileFromMemoryStream sourceStream;

        enum StreamState
        {
            Playing = 1,
            Paused = 2,
            Stopped = 4
        }

        private StreamState _streamState { get ; set; }

        private void _InitializeTimer()
        {
            
        }

        /// <summary>
        /// Creates a new _wavePlayer if a current one doesn't already exist
        /// </summary>
        /// <returns></returns>
        private IWavePlayer CreateWaveOut() => _wavePlayer != null ? _wavePlayer : new WaveOut();


        private static IMp3FrameDecompressor CreateFrameDecompressor(Mp3Frame frame)
        {
            WaveFormat waveFormat = new Mp3WaveFormat(frame.SampleRate, frame.ChannelMode == ChannelMode.Mono ? 1 : 2,
                frame.FrameLength, frame.BitRate);
            return new AcmMp3FrameDecompressor(waveFormat);
        }



        public void StartStream(string url)
        {
            if (waitCallBack != null)
            {
                //wcb.
            }
            waitCallBack = StreamMp3;
            ThreadPool.QueueUserWorkItem(waitCallBack, url);
        }

        protected void StreamMp3(object state)
        {
            sourceStream = FileFromMemoryStream.Create((string)state);

            Thread.Sleep(300);
            if (sourceStream.FilePath.EndsWith(".mp3"))
            {
                try
                {
                    Thread.Sleep(1000);
                    //((FileFromMemoryStream)sourceStream).WaitForRead = true;
                    StartPlaying(new MediaFoundationReader(sourceStream.FilePath));// (Stream)sourceStream));
                }
                catch (Exception)
                {
                    //((FileFromMemoryStream)sourceStream).WaitForRead = false;
                    //sourceStream.Position = 0;
                    //StartPlaying(new StreamMediaFoundationReader((Stream)sourceStream));
                }
            }
            else if (sourceStream.FilePath.EndsWith(".wav"))
            {
                try
                {
                    StartPlaying(new WaveFileReader((Stream)sourceStream));
                }
                catch (EndOfStreamException)
                {
                }
            }
            else
            {
                throw new Exception("Unsupported file type.");
            }
        }

        /// <summary>
        /// Creates a new <see cref="WaveOut"></see> device and starts playing from the provided <see cref="IWaveProvider"/>  
        /// </summary>
        /// <param name="waveProvider"></param>
        private void StartPlaying(IWaveProvider waveProvider)
        {
            if (waveOut == null)
            {
                waveOut = CreateWaveOut(); 
            }
            waveOut.Init(waveProvider);
            waveOut.Play();
            _streamState = StreamState.Playing;
        }



        public void PlayPauseStream()
        {
            if (waveOut != null)
            {
                if (_streamState == StreamState.Playing)
                {
                    waveOut?.Pause();
                    _streamState = StreamState.Paused;
                }
                else if (_streamState == StreamState.Paused)
                {
                    waveOut?.Play();
                    _streamState = StreamState.Playing;
                }
            }
        }

        public void StopStream()
        {
            waveOut?.Stop();
            _streamState = StreamState.Stopped;
        }


        /// <summary>
        /// Returns a new <see cref="WaveFormat"/> depending on the file in the memory.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual WaveFormat GetWaveFormat(IFileFromMemoryStream sourceStream)
        {
            WaveFormat format;
            Mp3Frame frame = null; ;
            AudioFileType fileType = sourceStream.FilePath.EndsWith(".mp3") ?
                AudioFileType.Mp3 :
                sourceStream.FilePath.EndsWith(".wav") ?
                    AudioFileType.Wave :
                    AudioFileType.Unknown;

            if (fileType == AudioFileType.Mp3)
            {
                int counter = 0;
                while (frame == null)
                {
                    frame = Mp3Frame.LoadFromStream((Stream)sourceStream);
                    Thread.Sleep(50);
                    counter++;
                    if (counter > 100)
                    {
                        throw new ArgumentNullException();
                    }
                }

                format = new Mp3WaveFormat(frame.SampleRate, frame.ChannelMode == ChannelMode.Mono ? 1 : 2,
                    frame.FrameLength, frame.BitRate);
            }
            else if (fileType == AudioFileType.Wave)
            {
                format = (new WaveFileReader(sourceStream.FilePath)).WaveFormat;
            }
            else
            {
                throw new ArgumentException("The file type is currently unsupported");
            }

            return format;
        }
    }

        
}
