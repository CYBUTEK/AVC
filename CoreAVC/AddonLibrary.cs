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

namespace CoreAVC
{
    using System.Collections.Generic;
    using System.IO;
    using Avc;
    using General;
    using UnityEngine;

    public class AddonLibrary
    {
        /// <summary>
        ///     Gets the current number of add-ons still to be loaded.
        /// </summary>
        public static int LoadingCount { get; private set; }

        /// <summary>
        ///     Gets whether the add-on library is currently in the process of loading add-ons.
        /// </summary>
        public static bool IsLoading => LoadingCount > 0;

        /// <summary>
        ///     Gets the list of loaded addons.
        /// </summary>
        public static List<Addon> LoadedAddons { get; } = new List<Addon>();

        /// <summary>
        ///     Gets the KSP GameData path.
        /// </summary>
        private static string GameDataPath { get; } = Path.GetFullPath(Path.Combine(GameDatabase.Instance.PluginDataFolder, "GameData"));

        /// <summary>
        ///     Loads or reloads all of the AVC add-ons.
        /// </summary>
        public static void LoadAddons()
        {
            // Check that loading is not currently in progress.
            if (IsLoading)
            {
                return;
            }

            // Clear the current list of loaded add-ons.
            LoadedAddons.Clear();
            LoadingCount = 0;

            // Get an array of all the .version file paths and a count of how many there are.
            string[] addonFilePaths = Directory.GetFiles(GameDataPath, "*.version", SearchOption.AllDirectories);
            int addonFileCount = addonFilePaths.Length;

            // Iterate over all the add-on file paths to start loading them.
            for (int i = 0; i < addonFileCount; ++i)
            {
                LoadAvcAddonFromFile(addonFilePaths[i]);
            }
        }

        /// <summary>
        ///     Loads an AVC add-on from file.
        /// </summary>
        public static AvcAddon LoadAvcAddonFromFile(string filePath)
        {
            LoadingCount = LoadingCount + 1;
            return new AvcAddon(filePath, OnAddonLoadComplete);
        }

        /// <summary>
        ///     Called when an addon has been loaded.
        /// </summary>
        private static void OnAddonLoadComplete(Addon addon)
        {
            if (addon != null)
            {
                Debug.Log(addon);
                LoadedAddons.Add(addon);
            }
            LoadingCount = LoadingCount - 1;
        }
    }
}