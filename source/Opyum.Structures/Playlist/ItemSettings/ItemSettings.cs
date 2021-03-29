using System;

namespace Opyum.Structures.Playlist
{
    public class ItemSettings
    {
        /// <summary>
        /// The leeway amount for the item to find acceptable to overshoot or undershoot if the <see cref="TimeType"/> is set ot <see cref="TimeType.Set"/>.
        /// <para>This means that if the set time cannot be reached the item will try to arrange itself a leeway amount above or below the set time.</para>
        /// <para>If event that cannot be achieved, aditional variables are taken into consideration, but either the closest timeframe above and/or below the set time will be selected,
        /// or other items will have to change their duration or be changed completly.</para>
        /// </summary>
        public TimeSpan Leeway { get; set; }
        /// <summary>
        /// Determins what shood be done if the item cannot be inserted in the selected leeway.
        /// </summary>
        public LeewayOvershoot LeewayOvershoot { get; set; }
        /// <summary>
        /// Determines if the leeway is being lookd at before and/or after the designated time for the item.
        /// </summary>
        public LeewayOrientation LeewayOritentation { get; set; }
        
        
        
        /// <summary>
        /// The items the settings are linked to.
        /// </summary>
        public IPlaylistItem SettingsItem { get; private set; }



        /// <summary>
        /// The item that other settings may affect based on this items settings.
        /// </summary>
        public IPlaylistItem LinkedItem { get; set; }




    }
}
