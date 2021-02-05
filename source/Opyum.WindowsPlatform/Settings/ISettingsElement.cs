using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opyum.WindowsPlatform.Settings
{
    public interface ISettingsElement<T>
    {
        T Clone();
    }
}
