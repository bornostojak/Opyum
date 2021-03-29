using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;

namespace Opyum.Structures.Playlist
{
    [Opyum.Structures.Attributes.OpyumPlaylistItem]
    [Serializable]
    public class PlaylistItem : IPlaylistItem, IDisposable
    {
        /// <summary>
        /// The content to play from.
        /// </summary>
        public IContent Content { get => _content; protected internal set { _content = value; } }
        protected IContent _content;


        #region Time

        /// <summary>
        /// The time the audio is supposed to start playing.
        /// </summary>
        public DateTime SetTime { get; internal protected set; }
        /// <summary>
        /// The time the audio is going to be played to start playing.
        /// </summary>
        public DateTime PlayTime { get; set; }
        /// <summary>
        /// The type of item start time duration (SET or DYNAMIC)
        /// </summary>
        public TimeType TimeType { get; internal protected set; } = 0;

        #endregion


        /// <summary>
        /// Temporary tags that can be set in the running list.
        /// <para>These tags are applicable to only one item and are deleted the moment the item stops playling.</para>
        /// </summary>
        public ITags Tags { get; set; }

        /// <summary>
        /// The status of the item (wheather it can be used, if it's playing...).
        /// </summary>
        virtual public ItemStatus Status { get; internal protected set; }


        /// <summary>
        /// The type of the item.
        /// <para>Impropper assignment will throw an <see cref="InvalidOperationException"/></para>
        /// </summary>
        /// <exception cref="InvalidOperationException"/>
        virtual public PlaylistItemType ItemType
        {
            get
            {
                return _itemType;
            }
            protected set
            {
                if (typeof(PlaylistSlot) == this.GetType() && value != PlaylistItemType.Slot)
                {
                    throw new InvalidOperationException($"Impropper value assignment! {value.ToString()} cannot be assigned to a {this.GetType().ToString()} item type.");
                }
                if (typeof(PlaylistZone) == this.GetType() && (value != PlaylistItemType.Zone))//Start && value != PlaylistItemType.ZoneEnd))
                {
                    throw new InvalidOperationException($"Impropper value assignment! {value.ToString()} cannot be assigned to a {this.GetType().ToString()} item type.");
                }
                _itemType = value;
            }
        }
        protected PlaylistItemType _itemType = PlaylistItemType.None;



        protected PlaylistItem()
        {

        }

        public PlaylistItem(string data) : this()
        {
            Content = ContentCreator.CreateContent(data);
        }


        ~PlaylistItem()
        {
            Dispose();
        }

        #region Change Events

        public event PlaylistItemChangedEventHandler Changed;
        public event PlaylistItemChangedEventHandler NextItemUpdateRequest;
        public event PlaylistItemChangedEventHandler ItemLengthChanged;
        public event PlaylistItemActionCallEventHandler ItemActionCall;
        public event PlaylistItemChangedEventHandler Deletion;

        protected virtual void OnItemChange()
        {
            OnItemChange(new PlaylistItemChangedEventArgs() { Changes = ItemChanges.NotDefined });
        }

        protected virtual void OnItemChange(PlaylistItemChangedEventArgs changes)
        {

            Changed?.Invoke(this, changes);
        }

        protected virtual void OnActionCall(Action a)
        {
            ItemActionCall?.Invoke(this, a);
        }

        #endregion

        #region Methods

        protected virtual void UpdateSetup()
        {
            Content.ContentChanged += UpdateItem;
            Content.StateChanged += UpdateItem;
        }

        protected virtual void UpdateItem(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Run tihis to initialize the content.
        /// </summary>
        public void Initialize()
        {
            Content.Initialize();
        }

        /// <summary>
        /// Used to generate the content.
        /// <para>Depending on the <paramref name="data"/>, the method will call the <see cref="ContentCreator"/>, which will determin if the data is a querry, path or stream location.</para>
        /// </summary>
        /// <param name="data"></param>
        public void Generate(string data)
        {
            Content = ContentCreator.CreateContent(data);
        }

        #endregion

        #region Garbage Collection

        public void Dispose()
        {
            Deletion?.Invoke(this, new PlaylistItemChangedEventArgs());
            Dispose(true);
            GC.Collect();
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Content?.Dispose();
                this.Content = null;
                this.Tags = null;
                this.Content.VolumeCurve = null;
            }
        }

        #endregion

        #region >>>>>>>>>>>>>>>>>>>> UNFINISHED <<<<<<<<<<<<<<<<<<<<<<<<

        ///// <summary>
        ///// UNFINISHED
        ///// </summary>
        ///// <param name="location"></param>
        ///// <returns></returns>
        //public static PlaylistItem Create(string location)
        //{
        //    var temp = new PlaylistItem();

        //    return temp;
        //}

        ///// <summary>
        ///// Initialises item from a given content.
        ///// </summary>
        ///// <param name="content"></param>
        ///// <returns></returns>
        //public static PlaylistItem CreateFromContent(IContent content)
        //{
        //    var temp = new PlaylistItem();
        //    temp.Content = content;

        //    return temp;
        //}


        #endregion
    }
}
