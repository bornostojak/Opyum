using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace Opyum.Structures.Playlist
{
    [Opyum.Structures.Attributes.OpyumPlaylistItem]
    [Serializable]
    public class PlaylistBlock : IPlaylistItem, IEnumerable, IEnumerator, IDisposable, IXmlSerializable
    {
        /// <summary>
        /// The stack containing the items.
        /// </summary>
        protected BlockStack<PlaylistItem> ItemStack { get; set; } = null;

        /// <summary>
        /// The item stack in list form
        /// </summary>
        public List<PlaylistItem> Items { get { var list = ItemStack?.ToList(); /*list?.Reverse();*/ return list; } }

        public IPlaylistItem Next => ItemStack.Pop();



        /// <summary>
        /// Pops an item from the stack and gets the <see cref="PlaylistItem.Content"/> from it.
        /// </summary>
        public IContent Content { get => ItemStack?.Pop().Content; }



        /// <summary>
        /// The time the audio is supposed to start playing.
        /// </summary>
        public DateTime SetTime { get; protected internal set; }
        /// <summary>
        /// The time the audio is going to be played to start playing.
        /// </summary>
        public DateTime PlayTime { get; set; }
        /// <summary>
        /// The type of item start time duration (SET or DYNAMIC)
        /// </summary>
        public TimeType TimeType { get; set; }



        /// <summary>
        /// Temporary tags that can be set in the running list.
        /// <para>These tags are applicable to only one item and are deleted the moment the item stops playling.</para>
        /// </summary>
        public ITags Tags { get; set; }

        /// <summary>
        /// The status of the item (wheather it can be used, if it's playing...).
        /// </summary>
        public ItemStatus Status { get; protected internal set; }

        /// <summary>
        /// The type of the item.
        /// <para>Impropper assignment will throw an <see cref="InvalidOperationException"/></para>
        /// </summary>
        /// <exception cref="InvalidOperationException"/>
        public PlaylistItemType ItemType { get => PlaylistItemType.Block; }






        public event PlaylistItemChangedEventHandler Changed;
        public event PlaylistItemChangedEventHandler NextItemUpdateRequest;
        public event PlaylistItemChangedEventHandler Deletion;




        public void Generate(string data)
        {

        }

        public void Initialize()
        {

        }


        public IEnumerator GetEnumerator() => this;

        #region Garbage Collection

        public void Dispose()
        {
            Dispose(true);
            GC.Collect();
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Content.Dispose();
            }
        }

        #endregion

        #region IEnumerator

        public object Current => (_itemLocator < 0 || _itemLocator >= ItemStack.Count) ? Items[0] : Items[_itemLocator];
        private int _itemLocator = -1;

        public bool MoveNext() => ++_itemLocator >= ItemStack.Count ? false : true;

        public void Reset() => _itemLocator = -1;

        #endregion

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
    }
}
