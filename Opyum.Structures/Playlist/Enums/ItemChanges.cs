using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opyum.Structures.Playlist
{
    public enum ItemChanges
    {
        NotDefined          = 0,
        Content             = 1,
        Begining            = 2,
        Duration            = 4,
        PlayTime            = 8,
        DurationType        = 16,
        TimeType            = 32,
        Tags                = 64,
        State               = 128,
        AudioInformation    = 256,
        VolumeCurve         = 512,
        Settings            = 1024,
        History             = 2048,
        ItemType            = 4096
    }
}
