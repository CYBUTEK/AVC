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
    using Helpers;
    using UnityEngine;

    public class Overlay : MonoBehaviour
    {
        private bool compactList;
        private GUIStyle greenLabelStyle;
        private Vector2 scrollPosition;
        private bool showList;
        private Rect windowRect = new Rect(0.0f, 0.0f, 400.0f, 0.0f);
        private GUIStyle windowStyle;
        private GUIStyle yellowLabelStyle;

        /// <summary>
        ///     Draws a list of loaded add-ons.
        /// </summary>
        private void AddonList()
        {
            // Iterate over all the loaded add-ons.
            for (int i = 0; i < AddonLibrary.LoadedAddons.Count; ++i)
            {
                // Retrieve and store the add-on from the list for use.
                Addon addon = AddonLibrary.LoadedAddons[i];

                // Set a label style for use with the add-on text depending on whether it's compatible or requires updating.
                GUIStyle labelStyle = addon.UpdateAvailable || addon.IsCompatible == false ? yellowLabelStyle : greenLabelStyle;

                // Draw the add-on GUI components horizontally.
                LayoutHelper.Horizontal(() =>
                {
                    // Name
                    GUILayout.Label(addon.Name, labelStyle);

                    // Pad the space so the name is on the left and version is on the right.
                    GUILayout.FlexibleSpace();

                    // Check is an update is available.
                    if (addon.UpdateAvailable)
                    {
                        // Display the currently installed and latest add-on version if an update is available.
                        GUILayout.Label(addon.LocalVersionString + " / " + addon.RemoteVersionString, labelStyle);
                    }
                    else
                    {
                        // Display only the currently installed add-on version if an update is not available.
                        GUILayout.Label(addon.LocalVersionString, labelStyle);
                    }
                });
            }
        }

        /// <summary>
        ///     Draws the overlay's add-on list container.
        /// </summary>
        private void AddonListContainer()
        {
            if (showList == false)
            {
                return;
            }

            // Check if there are loaded add-ons.
            if (AddonLibrary.LoadedAddons.Count == 0)
            {
                // Display a message when there are no loaded add-ons.
                GUILayout.Label("No AVC enabled add-ons have been processed.", yellowLabelStyle);
            }
            else if (compactList)
            {
                // Draw the add-on list inside a scroll view when in compact mode.
                LayoutHelper.VerticalScroll(AddonList, ref scrollPosition);
            }
            else
            {
                // Draw the add-on list straight onto the GUI if not in compact mode.
                AddonList();
            }
        }

        /// <summary>
        ///     Called by Unity on Awake to set the GUI styles.
        /// </summary>
        private void Awake()
        {
            windowStyle = new GUIStyle
            {
                normal = { background = TextureHelper.GetTexture("list-bg.png", 400, 35) },
                border = new RectOffset(3, 3, 20, 3),
                padding = new RectOffset(10, 10, 23, 5)
            };

            greenLabelStyle = new GUIStyle
            {
                normal =
                {
                    textColor = Color.green
                }
            };

            yellowLabelStyle = new GUIStyle
            {
                normal =
                {
                    textColor = Color.yellow
                }
            };
        }

        /// <summary>
        ///     Checks whether the overlay's add-on list requires converting to compact mode.
        /// </summary>
        private void CheckCompactList()
        {
            if (showList == false)
            {
                return;
            }

            // Check if compact list mode requires enabling.
            if (compactList == false && windowRect.height > Screen.height)
            {
                // Enable compact list mode.
                compactList = true;
            }

            // Check if the window height requires adjusting.
            if (compactList && (windowRect.height < Screen.height || windowRect.height > Screen.height))
            {
                // Set the window height to the screen height.
                windowRect.height = Screen.height;
            }
        }

        /// <summary>
        ///     Called by Unity each frame to draw the GUI.
        /// </summary>
        private void OnGUI()
        {
            // Draw the window used to display the overlay.
            windowRect = GUILayout.Window(GetInstanceID(), windowRect, Window, string.Empty, windowStyle);

            // Check whether the add-on list needs to be in compact mode to keep the window height within the screen area.
            CheckCompactList();
        }

        /// <summary>
        ///     Called to draw a GUI window.
        /// </summary>
        private void Window(int windowId)
        {
            // Align GUI components horizontally.
            LayoutHelper.Horizontal(() =>
            {
                // Check if still loading add-ons.
                if (AddonLibrary.LoadingCount > 0)
                {
                    // Display the number of loaded add-ons and total.
                    GUILayout.Label("Processed Add-ons: " + AddonLibrary.LoadedAddons.Count + " of " + (AddonLibrary.LoadedAddons.Count + AddonLibrary.LoadingCount));
                }
                else
                {
                    // Display the number of loaded add-ons.
                    GUILayout.Label("AVC Enabled Add-ons: " + AddonLibrary.LoadedAddons.Count);
                }

                // Pad the space after the label to push the show list toggle to the right.
                GUILayout.FlexibleSpace();

                // Check if the show list toggle has been clicked.
                if (GUILayout.Toggle(showList, "Show List") != showList)
                {
                    // Toggle the show list boolean.
                    showList = !showList;

                    // Reset the height of the window so that it collapses when closed.
                    windowRect.height = 0.0f;
                }
            });

            AddonListContainer();
        }
    }
}