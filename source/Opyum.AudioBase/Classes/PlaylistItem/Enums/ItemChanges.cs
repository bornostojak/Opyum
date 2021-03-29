using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opyum.AudioBase
{
    public enum ItemChanges
    {
        None = 0,
        Content = 1,
        Tags = 2,
        Time = 4,
        Duration = 8,
        Extra = 16
    }
}
