using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opyum.Playlist
{
    public class BaseAudioItem : IBaseAudioItem
    {
        protected string _ID = String.Empty;
        protected string _Tip = String.Empty;
        protected string _Color = String.Empty;
        protected string _NaKanalu = String.Empty;
        protected string _ItemType = String.Empty;
        protected string _StartCue = String.Empty;
        protected string _EndCue = String.Empty;
        protected string _Pocetak = String.Empty;
        protected string _Trajanje = String.Empty;
        protected string _Vrijeme = String.Empty;
        protected string _StvarnoVrijemePocetka = String.Empty;
        protected string _VrijemeMinTermin = String.Empty;
        protected string _VrijemeMaxTermin = String.Empty;
        protected string _PrviU_Bloku = String.Empty;
        protected string _ZadnjiU_Bloku = String.Empty;
        protected string _JediniU_Bloku = String.Empty;
        protected string _FiksniU_Terminu = String.Empty;
        protected string _Reklama = String.Empty;
        protected string _WaveIn = String.Empty;
        protected string _SoftIn = String.Empty;
        protected string _SoftOut = String.Empty;
        protected string _Volume = String.Empty;
        protected string _OriginalStartCue = String.Empty;
        protected string _OriginalEndCue = String.Empty;
        protected string _OriginalPocetak = String.Empty;
        protected string _OriginalTrajanje = String.Empty;


        /// <summary>
        /// 
        /// </summary>
        public virtual string ID { get { return _ID; } set { _ID = value; OnInformationChange(); } }
        /// <summary>
        /// The type of the content. If it comes from FirePlay, the "Tip" variable is connected to the color when loading from inside the program.
        /// </summary>
        public virtual string Tip { get { return _Tip; } set { _Tip = value; OnInformationChange(); } }
        /// <summary>
        /// Contains the information about the color in the program.
        /// </summary>
        public virtual string Color { get { return _Color; } set { _Color = value; OnInformationChange(); } }
        /// <summary>
        /// ???
        /// </summary>
        public virtual string NaKanalu { get { return _NaKanalu; } set { _NaKanalu = value; OnInformationChange(); } }
        /// <summary>
        /// Denotes the item type.
        /// </summary>
        public virtual string ItemType { get { return _ItemType; } set { _ItemType = value; OnInformationChange(); } }
        /// <summary>
        /// The time in the song where the song begins to play (in seconds, double). Everything before that is skipped.
        /// </summary>
        public virtual string StartCue { get { return _StartCue; } set { _StartCue = value; OnInformationChange(); } }
        /// <summary>
        /// The duration of the song playing (in seconds, double).
        /// </summary>
        public virtual string EndCue { get { return _EndCue; } set { _EndCue = value; OnInformationChange(); } }
        /// <summary>
        /// The duration at whitch the singing begins (in seconds, double). 
        /// </summary>
        public virtual string Pocetak { get { return _Pocetak; } set { _Pocetak = value; OnInformationChange(); } }
        /// <summary>
        /// The point in the song at whitch the next song is supposed to be playing (in seconds, double).
        /// </summary>
        public virtual string Trajanje { get { return _Trajanje; } set { _Trajanje = value; OnInformationChange(); } }
        /// <summary>
        /// The time when the song is supposed to start playing.
        /// </summary>
        public virtual string Vrijeme { get { return Vrijeme; } set { _Vrijeme = value; OnInformationChange(); } }
        /// <summary>
        /// ???
        /// </summary>
        public virtual string StvarnoVrijemePocetka { get { return _StvarnoVrijemePocetka; } set { _StvarnoVrijemePocetka = value; OnInformationChange(); } }
        /// <summary>
        /// ???
        /// </summary>
        public virtual string VrijemeMinTermin { get { return _VrijemeMinTermin; } set { _VrijemeMinTermin = value; OnInformationChange(); } }
        /// <summary>
        /// ???
        /// </summary>
        public virtual string VrijemeMaxTermin { get { return _VrijemeMaxTermin; } set { _VrijemeMaxTermin = value; OnInformationChange(); } }
        /// <summary>
        /// ???
        /// </summary>
        public virtual string PrviU_Bloku { get { return _PrviU_Bloku; } set { _PrviU_Bloku = value; OnInformationChange(); } }
        /// <summary>
        /// ???
        /// </summary>
        public virtual string ZadnjiU_Bloku { get { return _ZadnjiU_Bloku; } set { _ZadnjiU_Bloku = value; OnInformationChange(); } }
        /// <summary>
        /// ???
        /// </summary>
        public virtual string JediniU_Bloku { get { return _JediniU_Bloku; } set { _JediniU_Bloku = value; OnInformationChange(); } }
        /// <summary>
        /// ???
        /// </summary>
        public virtual string FiksniU_Terminu { get { return _FiksniU_Terminu; } set { _FiksniU_Terminu = value; OnInformationChange(); } }
        /// <summary>
        /// Reklama bool value (string).
        /// </summary>
        public virtual string Reklama { get { return _Reklama; } set { _Reklama = value; OnInformationChange(); } }
        /// <summary>
        /// WaveIn bool value (string).
        /// </summary>
        public virtual string WaveIn { get { return _WaveIn; } set { _WaveIn = value; OnInformationChange(); } }
        /// <summary>
        /// Contains the value of the fade-in.
        /// </summary>
        public virtual string SoftIn { get { return _SoftIn; } set { _SoftIn = value; OnInformationChange(); } }
        /// <summary>
        /// Contains the value of the fade-out.
        /// </summary>
        public virtual string SoftOut { get { return _SoftOut; } set { _SoftOut = value; OnInformationChange(); } }
        /// <summary>
        /// Contains the value of the volume.
        /// </summary>
        public virtual string Volume { get { return _Volume; } set { _Volume = value; OnInformationChange(); } }
        /// <summary>
        /// Statest the original (default) value of the "StartCue" field.
        /// </summary>
        public virtual string OriginalStartCue { get { return _OriginalStartCue; } set { _OriginalStartCue = value; OnInformationChange(); } }
        /// <summary>
        /// Statest the original (default) value of the "EndCue" field.
        /// </summary>
        public virtual string OriginalEndCue { get { return _OriginalEndCue; } set { _OriginalEndCue = value; OnInformationChange(); } }
        /// <summary>
        /// Statest the original (default) value of the "Pocetak" field.
        /// </summary>
        public virtual string OriginalPocetak { get { return _OriginalPocetak; } set { _OriginalPocetak = value; OnInformationChange(); } }
        /// <summary>
        /// Statest the original (default) value of the "Trajanje" field.
        /// </summary>
        public virtual string OriginalTrajanje { get { return _OriginalTrajanje; } set { _OriginalTrajanje = value; OnInformationChange(); } }
        /// <summary>
        /// Returns a bool for the WaveIn string variable.
        /// </summary>
        public virtual bool WaveInBool => WaveIn.ToLower() == "true" ? true : false;
        /// <summary>
        /// Returns a bool for the ReklamaBool string variable.
        /// </summary>
        public virtual bool ReklamaBool => Reklama.ToLower() == "true" ? true : false;

        protected virtual void OnInformationChange()
        {
            InformationChange?.Invoke(this, new EventArgs());
        }

        public event EventHandler InformationChange;
    }
}
