using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Opyum.Structures.Playlist
{
    public class Playlist : IEnumerable, IDisposable, ICollection<IPlaylistItem>, IXmlSerializable
    {
        public LinkedList<IPlaylistItem> Items { get; protected set; } = null;
        PlaylistItemAccessor _accessor;
        PlaylistManager _manager;
        PlaylistRearranger _rearranger;


        Playlist()
        {
            Items = new LinkedList<IPlaylistItem>();
        }

        public Playlist(PlaylistManager manager, PlaylistItemAccessor accessor, PlaylistRearranger rearranger) : this()
        {
            _rearranger = rearranger;
            _manager = manager;
            _accessor = accessor;
        }




        #region Stack function

        public IPlaylistItem Next { get; protected internal set; }

        public IPlaylistItem Pop()
        {
            var item = Items.First.Value;
            Items.RemoveFirst();
            return item;
        }

        public void Insert(IPlaylistItem item) => Items.AddLast(item);
        public void Insert(IPlaylistItem item, int index)
        {
            if (index == 0)
            {
                Items.AddFirst(item);
                return;
            }
            var temp = Items.First;
            if (index > 0)
            {
                for (int i = 0; i < index; i++)
                {
                    temp = temp.Next;
                }
                Items.AddBefore(temp, item);
                return;
            }
            throw new IndexOutOfRangeException();

        }

        public IPlaylistItem First => throw new NotImplementedException();

        #endregion


        #region Garbage collection

        public void Dispose()
        {
            Dispose(true);
            GC.Collect(); 
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
        }

        #endregion


        

        #region IEnumerable

        public IEnumerator GetEnumerator() => (IEnumerator)Items;

        #endregion

        #region Collection

        public bool IsReadOnly => false;

        public int Count => Items.Count;

        public void Add(IPlaylistItem item)
        {
            Items.AddLast(item);
        }

        public void AddBefor(IPlaylistItem replacent, IPlaylistItem item)
        {
            Items.AddBefore(Items.Find(replacent), item);
        }

        public void AddAfter(IPlaylistItem replacent, IPlaylistItem item)
        {
            Items.AddAfter(Items.Find(replacent), item);
        }

        public bool Contains(IPlaylistItem item) => Items.Contains(item);

        public void CopyTo(IPlaylistItem[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(IPlaylistItem item)
        {
            if (Items.Contains(item))
            {
                Items.Remove(item);
                return true;
            }
            return false;
        }

        IEnumerator<IPlaylistItem> IEnumerable<IPlaylistItem>.GetEnumerator() => (IEnumerator<IPlaylistItem>)Items;


        public void Clear()
        {
            Items = new LinkedList<IPlaylistItem>();
            GC.Collect();
        }


        #endregion

        #region Conversion

        public Newtonsoft.Json.Schema.JsonSchema ToJSON()
        {
            throw new NotImplementedException();
        }

        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }






        #endregion

    }
}
