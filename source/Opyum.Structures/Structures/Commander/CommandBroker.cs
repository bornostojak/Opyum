using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opyum.Structures.Playlist;

namespace Opyum.Structures
{
    public class CommandBroker
    {
        IPlaylistItem _item;


        public CommandBroker()
        {

        }


        public CommandBroker(IPlaylistItem item)
        {
            _item = item;
        }

        ~CommandBroker()
        {
            _item = null;
        }

        public void Command()
        {

        }
    }
}
