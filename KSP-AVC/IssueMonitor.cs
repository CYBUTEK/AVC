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

    public class IssueMonitor : MonoBehaviour
    {
        private const float COLUMN_NAME_WIDTH = 275.0f;
        private const float COLUMN_VERSION_WIDTH = 100.0f;
        private GUIStyle boxStyle;
        private GUIStyle contentLabelCentredStyle;
        private GUIStyle contentLabelStyle;
        private bool hasCentred;
        private GUIStyle headingLabelCentredStyle;
        private GUIStyle headingLabelStyle;
        private Rect windowRect = new Rect(Screen.width, Screen.height, 0.0f, 0.0f);

        /// <summary>
        ///     Called by Unity on Awake to set the GUI styles.
        /// </summary>
        protected virtual void Awake()
        {
            boxStyle = new GUIStyle(HighLogic.Skin.box)
            {
                padding = new RectOffset(10, 10, 5, 5)
            };

            headingLabelStyle = new GUIStyle(HighLogic.Skin.label)
            {
                normal =
                {
                    textColor = Color.white
                },
                fontStyle = FontStyle.Bold
            };
            headingLabelCentredStyle = new GUIStyle(headingLabelStyle) { alignment = TextAnchor.MiddleCenter };

            contentLabelStyle = new GUIStyle(HighLogic.Skin.label);
            contentLabelCentredStyle = new GUIStyle(contentLabelStyle) { alignment = TextAnchor.MiddleCenter };
        }

        protected virtual void OnGUI()
        {
            // Draw the GUI window.
            windowRect = KSPUtil.ClampRectToScreen(GUILayout.Window(GetInstanceID(), windowRect, Window, "KSP-AVC - Issue Monitor", HighLogic.Skin.window));

            // Check if the window has been initially centred or should be centred.
            if (hasCentred == false)
            {
                hasCentred = CentreWindow();
            }
        }

        /// <summary>
        ///     Centres the window and returns whether the window was successfully centred.
        /// </summary>
        private bool CentreWindow()
        {
            // Check that the window has a width and height.
            if (windowRect.width > 0.0f && windowRect.height > 0.0f)
            {
                // Set the window to the centre of the screen.
                windowRect.center = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

                // Window was successfully centred.
                return true;
            }

            // Window was not centred.
            return false;
        }

        /// <summary>
        ///     Draws the updatable add-on list.
        /// </summary>
        private void DrawUpdateableList()
        {
            DrawUpdateableListHeadings();
            DrawUpdateableListAddons();
        }

        /// <summary>
        ///     Draws the updatable add-on list.
        /// </summary>
        private void DrawUpdateableListAddons()
        {
            // Iterate through all the loaded add-ons.
            for (int i = 0; i < AddonLibrary.LoadedAddons.Count; ++i)
            {
                // Get the current add-on from the library.
                Addon addon = AddonLibrary.LoadedAddons[i];

                // Check that the add-on exists and has an update available.
                if (addon != null && addon.UpdateAvailable)
                {
                    // Draw the add-on details inside a horizontal layout container.
                    LayoutHelper.Horizontal(() =>
                    {
                        GUILayout.Label(addon.Name, contentLabelStyle, GUILayout.Width(COLUMN_NAME_WIDTH));
                        GUILayout.Label(addon.LocalVersionString, contentLabelCentredStyle, GUILayout.Width(COLUMN_VERSION_WIDTH));
                        GUILayout.Label(addon.RemoteVersionString, contentLabelCentredStyle, GUILayout.Width(COLUMN_VERSION_WIDTH));
                    });
                }
            }
        }

        /// <summary>
        ///     Draws the headings for the updatable add-on list.
        /// </summary>
        private void DrawUpdateableListHeadings()
        {
            // Draw the add-on list headings inside a horizontal layout container.
            LayoutHelper.Horizontal(() =>
            {
                GUILayout.Label("ADD-ON NAME", headingLabelStyle, GUILayout.Width(COLUMN_NAME_WIDTH));
                GUILayout.Label("INSTALLED", headingLabelCentredStyle, GUILayout.Width(COLUMN_VERSION_WIDTH));
                GUILayout.Label("AVAILABLE", headingLabelCentredStyle, GUILayout.Width(COLUMN_VERSION_WIDTH));
            });
        }

        /// <summary>
        ///     Called to draw the GUI window.
        /// </summary>
        private void Window(int windowId)
        {
            // Draw a vertical container with the box style.
            LayoutHelper.Vertical(() => { DrawUpdateableList(); }, boxStyle);

            // Enable the window to be draggable.
            GUI.DragWindow();
        }
    }
}