using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opyum.WindowsPlatform.Settings
{
    public class ColorContainer : ISettingsElement<ColorContainer>
    {
        public string Colormask { get; set; }
        public string ColorCode { get; set; }

        public ColorContainer Clone()
        {
            return new ColorContainer();
        }
    }
}
