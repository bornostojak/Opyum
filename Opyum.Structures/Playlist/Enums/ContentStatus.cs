using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opyum.Structures.Playlist
{
    public enum ContentStatus
    {
        NoContentFound = 0,
        Loading = 1,
        Ready = 2,
        Playling = 4,
        Stopped = 8,
        Disposing = 16,
        Disposed = 32,
        UnsuccessfulPathResolution = 64
        //UnsupportedFormat = 128
    }
}
