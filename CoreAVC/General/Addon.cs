// 
//     Copyright (C) 2015 CYBUTEK
// 
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 

namespace CoreAVC.General
{
    using Helpers;

    public class Addon
    {
        /// <summary>
        ///     Gets whether the add-on is compatible with the current version of the game.
        /// </summary>
        public bool IsCompatible => UsingRemoteAmendments ? (RemoteAddonInfo?.IsCompatible ?? (LocalAddonInfo?.IsCompatible ?? true)) : (LocalAddonInfo?.IsCompatible ?? true);

        /// <summary>
        ///     Gets the local add-on info.
        /// </summary>
        public AddonInfo LocalAddonInfo { get; protected set; }

        /// <summary>
        ///     Gets the local add-on version string.
        /// </summary>
        public string LocalVersionString => (LocalAddonInfo?.Version.ToString() ?? "n/a");

        /// <summary>
        ///     Gets the name of the add-on.
        /// </summary>
        public string Name => UsingRemoteAmendments ? (RemoteAddonInfo?.Name ?? (LocalAddonInfo?.Name ?? "n/a")) : (LocalAddonInfo?.Name ?? "n/a");

        /// <summary>
        ///     Gets the remote add-on info.
        /// </summary>
        public AddonInfo RemoteAddonInfo { get; protected set; }

        /// <summary>
        ///     Gets the remote add-on version string.
        /// </summary>
        public string RemoteVersionString => (RemoteAddonInfo?.Version.ToString() ?? "n/a");

        /// <summary>
        ///     Gets the state of the add-on fetch cycle.
        /// </summary>
        public AddonState State { get; protected set; } = AddonState.Waiting;

        /// <summary>
        ///     Gets whether there is an update available for the add-on.
        /// </summary>
        public bool UpdateAvailable
        {
            get
            {
                // Both local and remote add-on information is require to check for update availablity.
                if (LocalAddonInfo == null || RemoteAddonInfo == null)
                {
                    return false;
                }

                // Check the versions and make sure that the update (if available) is also
                // compatible with the player's current version of the game.
                return LocalAddonInfo.Version < RemoteAddonInfo.Version && RemoteAddonInfo.IsCompatible;
            }
        }

        /// <summary>
        ///     Gets whether the add-on is using amended remote information for compatibility checks.
        /// </summary>
        public bool UsingRemoteAmendments => LocalAddonInfo != null && RemoteAddonInfo != null && LocalAddonInfo.Version == RemoteAddonInfo.Version;

        /// <summary>
        ///     Gets a zero tabbed pretty printable string representation of the object.
        /// </summary>
        public override string ToString()
        {
            return ToString(0, 0);
        }

        /// <summary>
        ///     Gets a tabbed pretty printable string representation of the object.
        /// </summary>
        public virtual string ToString(int firstLineTabs, int tabs)
        {
            return Utils.ConcatTab(firstLineTabs, tabs, Name, "{",
                Utils.ConcatTab(1, tabs + 1,
                    "UsingRemoteAmendments: " + UsingRemoteAmendments,
                    "IsCompatible: " + IsCompatible,
                    "UpdateAvailable: " + UpdateAvailable,
                    "Local", "{", LocalAddonInfo.ToString(1, tabs + 2), "}",
                    "Remote", "{", RemoteAddonInfo.ToString(1, tabs + 2), "}"),
                "}");
        }
    }
}