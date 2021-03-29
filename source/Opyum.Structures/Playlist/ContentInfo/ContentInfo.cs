namespace Opyum.Structures.Playlist
{
    /// <summary>
    /// Audio information.
    /// <para>The standard things it contains should be simillar to a ID3 tag, incčluding an ID3 tag.</para>
    /// </summary>
    public class ContentInfo
    {
        /// <summary>
        /// The title of the song or content
        /// </summary>
        public string Title { get; set; }


        /// <summary>
        /// The name of the content, song artist or audio file
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A limited ammount of details for the content, taken from the server
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// The audio image that will be shown from the content
        /// </summary>
        public AudioPicture Image { get; set; }



        public ContentInfo(string path)
        {
            Image = new AudioPicture(path);
        }






    }
}
