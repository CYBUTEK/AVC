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

namespace CoreAVC.Avc
{
    using System.Collections.Generic;
    using General;
    using Helpers;

    public class AvcAddonInfo : AddonInfo
    {
        /// <summary>
        ///     Creates an AVC add-on information object using a supplied json string.
        /// </summary>
        public AvcAddonInfo(string json, string path) : base(path)
        {
            ParseJsonString(json);
        }

        /// <summary>
        ///     Gets the change log content.
        /// </summary>
        public string ChangeLog { get; private set; }

        /// <summary>
        ///     Gets the URL set to find the change log.
        /// </summary>
        public string ChangeLogUrl { get; private set; }

        /// <summary>
        ///     Gets the URL for downloading the add-on.
        /// </summary>
        public string Download { get; private set; }

        /// <summary>
        ///     Gets the URL for the remote AVC version file.
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        ///     Gets a tabbed pretty printable string representation of the object.
        /// </summary>
        public override string ToString(int firstLineTabs, int tabs)
        {
            return Utils.ConcatLine(base.ToString(firstLineTabs, tabs),
                                    Utils.ConcatTab(tabs, tabs,
                                                    "URL: " + Url,
                                                    "DOWNLOAD: " + Download,
                                                    "CHANGE_LOG_URL: " + ChangeLogUrl,
                                                    "CHANGE_LOG: " + ChangeLog));
        }

        /// <summary>
        ///     Parses deserialised json data.
        /// </summary>
        private void ParseJsonData(Dictionary<string, object> data)
        {
            // Check that the data supplied exists.
            if (data == null)
            {
                return;
            }

            // Iterate through each element in the json data.
            foreach (string key in data.Keys)
            {
                switch (key.ToLowerInvariant())
                {
                    // Add-on display name.
                    case "name":
                        Name = data[key] as string;
                        break;

                    // URL that is used to fetch the remote add-on information.
                    case "url":
                        Url = data[key] as string;
                        break;
                    
                    // Download URL where the player can obtain the latest version of the add-on.
                    case "download":
                        Download = data[key] as string;
                        break;

                    // Change log content that's stored in the add-on information.
                    case "change_log":
                        ChangeLog = data[key] as string;
                        break;
                    
                    // URL where a plain text change log can be fetched.
                    case "change_log_url":
                        ChangeLogUrl = data[key] as string;
                        break;

                    // The add-on version.
                    case "version":
                        Version = new VersionObject(data[key]);
                        break;

                    // Game version which the add-on was built for.
                    case "ksp_version":
                        Compatibility.Version = new VersionObject(data[key]);
                        break;
                    
                    // Minimum game version the add-on officially supports.
                    case "ksp_version_min":
                        Compatibility.VersionMin = new VersionObject(data[key]);
                        break;

                    // Maximum game version the add-on officially supports.
                    case "ksp_version_max":
                        Compatibility.VersionMax = new VersionObject(data[key]);
                        break;
                    
                    // Assembly name for dynamically fetching the add-on version.
                    case "assembly":
                        Version = Utils.GetAssemblyVersion(data[key] as string);
                        break;

                    case "github":
                        break;
                }
            }
        }

        /// <summary>
        ///     Deserialises and parses a json string.
        /// </summary>
        private void ParseJsonString(string json)
        {
            // Check that the json string exists.
            if (string.IsNullOrEmpty(json) == false)
            {
                // Deserialise and parse the json data.
                ParseJsonData(Json.Deserialize(json) as Dictionary<string, object>);
            }
        }
    }
}