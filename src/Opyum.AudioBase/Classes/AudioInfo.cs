using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opyum.AudioBase
{
    public class AudioInfo : IAudioInfo
    {
        public string Name => throw new NotImplementedException();

        public string Album => throw new NotImplementedException();

        public string Author => throw new NotImplementedException();

        public string Lyrica => throw new NotImplementedException();

        public double Duration => throw new NotImplementedException();

        public string Format => throw new NotImplementedException();

        public string Path => throw new NotImplementedException();

        public AudioImage Image => throw new NotImplementedException();

        public ItemType Type => ItemType.None;

        public string AditionalInfo => throw new NotImplementedException();

        public string Notes => throw new NotImplementedException();
    }
}
