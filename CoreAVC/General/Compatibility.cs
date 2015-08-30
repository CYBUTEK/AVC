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

    public class Compatibility
    {
        /// <summary>
        ///     Creates an uninitialised compatibility object.
        /// </summary>
        public Compatibility() { }

        /// <summary>
        ///     Creates a new compatibility object with the version set.
        /// </summary>
        public Compatibility(VersionObject version)
        {
            Version = version;
        }

        /// <summary>
        ///     Creates a new compatibility object with the version and min/max set.
        /// </summary>
        public Compatibility(VersionObject version, VersionObject versionMin, VersionObject versionMax)
        {
            Version = version;
            VersionMin = versionMin;
            VersionMax = versionMax;
        }

        /// <summary>
        ///     Gets whether this compatibility object is compatible with the current game install.
        /// </summary>
        public bool IsCompatible
        {
            get
            {
                // Check between minimum and version/any.
                if (VersionMin != null && VersionMax == null)
                {
                    return VersionMin <= VersionObject.KspVersion && (Version ?? VersionObject.AnyValue) >= VersionObject.KspVersion;
                }

                // Check between version/any and maximum.
                if (VersionMin == null && VersionMax != null)
                {
                    return (Version ?? VersionObject.AnyValue) <= VersionObject.KspVersion && VersionMax >= VersionObject.KspVersion;
                }

                // Check between minimum and maximum.
                if (VersionMin != null && VersionMax != null)
                {
                    return VersionMin <= VersionObject.KspVersion && VersionMax >= VersionObject.KspVersion;
                }

                // Check against version/any.
                return (Version ?? VersionObject.AnyValue) == VersionObject.KspVersion;
            }
        }

        /// <summary>
        ///     Gets and set the version to use for compatibility.
        /// </summary>
        public VersionObject Version { get; set; } = VersionObject.AnyValue;

        /// <summary>
        ///     Gets and sets the maximum version to use for compatibility.
        /// </summary>
        public VersionObject VersionMax { get; set; }

        /// <summary>
        ///     Gets and sets the minimum version to use for compatibility.
        /// </summary>
        public VersionObject VersionMin { get; set; }

        /// <summary>
        ///     Gets a zero tabbed pretty printable string representation of the object.
        /// </summary>
        public override string ToString()
        {
            return ToString(0, 0);
        }

        /// <summary>
        ///     Gets a tabbed pretty printable string respresentation of the object.
        /// </summary>
        public virtual string ToString(int firstLineTabs, int tabs)
        {
            return Utils.ConcatTab(firstLineTabs, tabs,
                "KSP_VERSION: " + Version,
                "KSP_VERSION_MIN: " + VersionMin,
                "KSP_VERSION_MAX: " + VersionMax,
                "IsCompatible: " + IsCompatible);
        }
    }
}