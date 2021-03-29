using NAudio.Wave;

namespace Opyum.StandardPlayback
{
    public partial class StandardPlayer
    {
        private void InitializePlayer()
        {
            output = new DirectSoundOut();
        }
        
        public void InstantiatePlayerOutput(IWavePlayer provider)
        {
            _wavePlayer = provider;
        }

        //On the change of volume

        
    }


}
