using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opyum.AudioBase
{
    public interface IAudioInfo
    {
        string Name { get; }

        string Album { get; }

        string Author { get; }

        string Lyrica { get; }

        double Duration { get; }

        string Format { get; }

        string Path { get; }

        AudioImage Image { get; }

        ItemType Type { get; }

        string AditionalInfo { get; }

        string Notes { get; }

    }
    
}
