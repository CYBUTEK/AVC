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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class VersionObject : IComparable<VersionObject>, IComparable
    {
        /// <summary>
        ///     Creates a zeroed version object.
        /// </summary>
        public VersionObject()
        {
            SetSequences();
        }

        /// <summary>
        ///     Creates a version object with the major sequence set.
        /// </summary>
        public VersionObject(int major)
        {
            SetSequences(major);
        }

        /// <summary>
        ///     Creates a version object with the major and minor sequences set.
        /// </summary>
        public VersionObject(int major, int minor)
        {
            SetSequences(major, minor);
        }

        /// <summary>
        ///     Creates a version object with the major, minor and patch sequences set.
        /// </summary>
        public VersionObject(int major, int minor, int patch)
        {
            SetSequences(major, minor, patch);
        }

        /// <summary>
        ///     Creates a version object with the major, minor, patch and build sequences set.
        /// </summary>
        public VersionObject(int major, int minor, int patch, int build)
        {
            SetSequences(major, minor, patch, build);
        }

        /// <summary>
        ///     Creates a version object with sequences set to match the supplied string.
        /// </summary>
        public VersionObject(string version)
        {
            SetSequences(version);
        }

        /// <summary>
        ///     Creates a version object with sequences set to match a supplied json data object.
        /// </summary>
        public VersionObject(object obj)
        {
            if (obj == null)
            {
                return;
            }

            Dictionary<string, object> data = obj as Dictionary<string, object>;
            if (data != null)
            {
                SetSequences(data);
            }
            else
            {
                SetSequences(obj as string);
            }
        }

        /// <summary>
        ///     Creates a version object with the sequences set to match the supplied system type version object.
        /// </summary>
        public VersionObject(Version version)
        {
            SetSequences(version);
        }

        /// <summary>
        ///     Gets a new version object with all sequence values set to wildcards.
        /// </summary>
        public static VersionObject AnyValue => new VersionObject(-1, -1, -1, -1);

        /// <summary>
        ///     Gets the current version of the game.
        /// </summary>
        public static VersionObject KspVersion { get; } = new VersionObject(Versioning.version_major, Versioning.version_minor, Versioning.Revision);

        /// <summary>
        ///     Gets a new version object with maximum sequence values set.
        /// </summary>
        public static VersionObject MaxValue => new VersionObject(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);

        /// <summary>
        ///     Gets a new version object with zeroed sequence values set.
        /// </summary>
        public static VersionObject MinValue => new VersionObject();

        /// <summary>
        ///     Gets whether all the sequences are wildcards.
        /// </summary>
        public bool Any => Major == -1;

        /// <summary>
        ///     Gets and sets the build sequence number. (x.x.x.X)
        /// </summary>
        public int Build { get; set; }

        /// <summary>
        ///     Gets the GUID of the version object.
        /// </summary>
        public Guid Guid => new Guid();

        /// <summary>
        ///     Gets and sets the major sequence number. (X.x.x.x)
        /// </summary>
        public int Major { get; set; }

        /// <summary>
        ///     Gets and sets the minor sequence number. (x.X.x.x)
        /// </summary>
        public int Minor { get; set; }

        /// <summary>
        ///     Gets and sets the patch sequence number. (x.x.X.x)
        /// </summary>
        public int Patch { get; set; }

        /// <summary>
        ///     Compares this version object to another object with a cast.
        /// </summary>
        public int CompareTo(object obj)
        {
            return CompareTo(obj as VersionObject);
        }

        /// <summary>
        ///     Compares this version object to another version object.
        /// </summary>
        public int CompareTo(VersionObject other)
        {
            // Check whether the other object is null.
            if (other == null)
            {
                return 1;
            }

            // Compare major sequences.
            if (Major != -1 && other.Major != -1)
            {
                int major = Major.CompareTo(other.Major);
                if (major != 0)
                {
                    return major;
                }
            }
            else
            {
                return 0;
            }

            // Compare minor sequences.
            if (Minor != -1 && other.Minor != -1)
            {
                int minor = Minor.CompareTo(other.Minor);
                if (minor != 0)
                {
                    return minor;
                }
            }
            else
            {
                return 0;
            }

            // Compare patch sequences.
            if (Patch != -1 && other.Patch != -1)
            {
                int patch = Patch.CompareTo(other.Patch);
                if (patch != 0)
                {
                    return patch;
                }
            }
            else
            {
                return 0;
            }

            // Compare build sequences.
            if (Build != -1 && other.Build != -1)
            {
                int build = Build.CompareTo(other.Build);
                if (build != 0)
                {
                    return build;
                }
            }

            return 0;
        }

        /// <summary>
        ///     Equality operator.
        /// </summary>
        public static bool operator ==(VersionObject version1, VersionObject version2)
        {
            return Equals(version1, version2);
        }

        /// <summary>
        ///     Greater than operator.
        /// </summary>
        public static bool operator >(VersionObject version1, VersionObject version2)
        {
            return version1.Any || version2.Any || version1.CompareTo(version2) > 0;
        }

        /// <summary>
        ///     Greater than or equal to operator.
        /// </summary>
        public static bool operator >=(VersionObject version1, VersionObject version2)
        {
            return version1.Any || version2.Any || version1.CompareTo(version2) >= 0;
        }

        /// <summary>
        ///     Inequality operator.
        /// </summary>
        public static bool operator !=(VersionObject version1, VersionObject version2)
        {
            return Equals(version1, version2) == false;
        }

        /// <summary>
        ///     Less than operator.
        /// </summary>
        public static bool operator <(VersionObject version1, VersionObject version2)
        {
            return version1.Any || version2.Any || version1.CompareTo(version2) < 0;
        }

        /// <summary>
        ///     Less than or equal to operator.
        /// </summary>
        public static bool operator <=(VersionObject version1, VersionObject version2)
        {
            return version1.Any || version2.Any || version1.CompareTo(version2) <= 0;
        }

        /// <summary>
        ///     Compares equality between this version object and another.
        /// </summary>
        public override bool Equals(object obj)
        {
            // Check for null.
            if (obj == null)
            {
                return false;
            }

            // Check for reference equality.
            if (ReferenceEquals(obj, this))
            {
                return true;
            }

            // Check for value equality.
            return Equals(obj as VersionObject);
        }

        /// <summary>
        ///     Gets the hash code of the version object.
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                return Guid.GetHashCode();
            }
        }

        /// <summary>
        ///     Sets the version object sequences with the supplied values.
        /// </summary>
        public void SetSequences(int major = 0, int minor = 0, int patch = 0, int build = 0)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
            Build = build;
        }

        /// <summary>
        ///     Sets the version object sequences using a supplied string.
        /// </summary>
        public void SetSequences(string version)
        {
            if (string.IsNullOrEmpty(version))
            {
                return;
            }

            // Regex the supplied version string removing all but numbers and decimal points.
            // Then splitting at the decimals into an array of strings.
            string[] sequences = Regex.Replace(version, @"[^\d\.\*]", string.Empty).Split('.')
                                      .Select(sequence => sequence.Replace("*", "-1")).ToArray();

            // Switch depending on how many sequences were found in the supplied string.
            switch (sequences.Length)
            {
                // Set: Major
                case 1:
                    SetSequences(int.Parse(sequences[0]));
                    break;

                // Set: Major.Minor
                case 2:
                    SetSequences(int.Parse(sequences[0]), int.Parse(sequences[1]));
                    break;

                // Set: Major.Minor.Patch
                case 3:
                    SetSequences(int.Parse(sequences[0]), int.Parse(sequences[1]), int.Parse(sequences[2]));
                    break;

                // Set: Major.Minor.Patch.Build
                case 4:
                    SetSequences(int.Parse(sequences[0]), int.Parse(sequences[1]), int.Parse(sequences[2]), int.Parse(sequences[3]));
                    break;
            }
        }

        /// <summary>
        ///     Sets the version object sequences using a supplied system type version object.
        /// </summary>
        public void SetSequences(Version version)
        {
            if (version != null)
            {
                SetSequences(version.Major, version.Minor, version.Build, version.Revision);
            }
        }

        /// <summary>
        ///     Sets the version object sequences using supplied json data.
        /// </summary>
        public void SetSequences(Dictionary<string, object> data)
        {
            // Check that the data exista and is of the correct type.
            if (data == null)
            {
                return;
            }

            // Iterate through the keys in the json data.
            foreach (string key in data.Keys)
            {
                switch (key.ToLowerInvariant())
                {
                    // Sets the major version sequence.
                    case "major":
                        Major = (int)(long)data[key];
                        break;

                    // Sets the minor version sequence.
                    case "minor":
                        Minor = (int)(long)data[key];
                        break;

                    // Sets the patch version sequence.
                    case "patch":
                        Patch = (int)(long)data[key];
                        break;

                    // Sets the build version sequence.
                    case "build":
                        Build = (int)(long)data[key];
                        break;
                }
            }
        }

        /// <summary>
        ///     Returns a formatted string of the version object.
        /// </summary>
        public override string ToString()
        {
            // Check for 'Any' status.
            if (Any)
            {
                return "Any";
            }

            // Check for build sequence set.
            if (Build != 0)
            {
                // Return: Major.Minor.Patch.Build
                return string.Format("{0}.{1}.{2}.{3}", Major, Minor, Patch, Build).Replace("-1", "*");
            }

            // Check for patch sequence set.
            if (Patch != 0)
            {
                // Return: Major.Minor.Patch
                return string.Format("{0}.{1}.{2}", Major, Minor, Patch).Replace("-1", "*");
            }

            // Return: Major.Minor
            return string.Format("{0}.{1}", Major, Minor).Replace("-1", "*");
        }

        /// <summary>
        ///     Compares sequence equality between this version object and another.
        /// </summary>
        protected bool Equals(VersionObject other)
        {
            return other != null && other.CompareTo(this) == 0;
        }
    }
}