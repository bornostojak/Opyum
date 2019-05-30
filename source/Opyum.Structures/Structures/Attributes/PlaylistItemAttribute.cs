﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opyum.Playlist;

namespace Opyum.Structures.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class PlaylistItemAttribute : Attribute
    {
        public string Version { get; set; }
    }
}