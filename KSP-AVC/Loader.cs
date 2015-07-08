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

namespace KSP_AVC
{
    using CoreAVC;
    using CoreAVC.General;
    using UnityEngine;

    [KSPAddon(KSPAddon.Startup.Instantly, false)]
    public class Loader : MonoBehaviour
    {
        /// <summary>
        ///     Checks whether there are any add-on issues.
        /// </summary>
        private static bool CheckAddonIssues()
        {
            // Iterate over the loaded add-ons.
            for (int i = 0; i < AddonLibrary.LoadedAddons.Count; ++i)
            {
                // Check whether the add-on has an update available and game compatibility.
                Addon addon = AddonLibrary.LoadedAddons[i];
                if (addon.UpdateAvailable || addon.IsCompatible == false)
                {
                    // An add-on has an issue.
                    return true;
                }
            }

            // No add-ons were found with issues.
            return false;
        }

        private void Awake()
        {
            // Allow the loader object to persist through scene changes.
            DontDestroyOnLoad(gameObject);
        }

        private void OnLevelWasLoaded()
        {
            // Destroy the object after changing to a scene which AVC should not be running in.
            if (HighLogic.LoadedScene != GameScenes.LOADING && HighLogic.LoadedScene != GameScenes.MAINMENU)
            {
                Destroy(gameObject);
            }
        }

        private void ShowIssueMonitor()
        {
            // Checks whether there are any add-on issues that should be displayed.
            if (CheckAddonIssues())
            {
                // Create the issue monitor component.
                gameObject.AddComponent<IssueMonitor>();
            }
        }

        private void Start()
        {
            // Start loading all the AVC enabled add-ons.
            AddonLibrary.LoadAddons();

            // Create the overlay component.
            gameObject.AddComponent<Overlay>();
        }

        private void Update()
        {
            // Check if the add-on library has finished loading.
            if (AddonLibrary.IsLoading)
            {
                return;
            }

            // Try to show the issue monitor.
            ShowIssueMonitor();

            // Disable the loader component as the update loop is no longer required.
            enabled = false;
        }
    }
}