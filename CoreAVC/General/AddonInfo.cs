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

    public class AddonInfo
    {
        /// <summary>
        ///     Creates an add-on information object with the supplied path set.
        /// </summary>
        public AddonInfo(string path)
        {
            Path = path;
        }

        /// <summary>
        ///     Gets and set the add-on's game compatibility object.
        /// </summary>
        public Compatibility Compatibility { get; set; } = new Compatibility();

        /// <summary>
        ///     Gets whether the add-on is compatible with the current game install.
        /// </summary>
        public bool IsCompatible => Compatibility.IsCompatible;

        /// <summary>
        ///     Gets and sets the name of the add-on.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        ///     Gets the path where the add-on information was fetched.
        /// </summary>
        public string Path { get; protected set; }

        /// <summary>
        ///     Gets and set the add-on version.
        /// </summary>
        public VersionObject Version { get; set; } = VersionObject.MinValue;

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
            return Utils.ConcatTab(firstLineTabs, tabs,
                "Path: " + Path,
                "NAME: " + Name,
                "VERSION: " + Version,
                "Compatibility", "{", Compatibility.ToString(1, tabs + 1), "}");
        }
    }
}