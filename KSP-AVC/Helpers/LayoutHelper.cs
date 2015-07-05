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

namespace KSP_AVC.Helpers
{
    using System;
    using UnityEngine;

    public static class LayoutHelper
    {
        public static void Horizontal(Action content, GUIStyle style = null)
        {
            if (style != null)
            {
                GUILayout.BeginHorizontal(style);
            }
            else
            {
                GUILayout.BeginHorizontal();
            }
            content?.Invoke();
            GUILayout.EndHorizontal();
        }

        public static void HorizontalScroll(Action content, ref Vector2 scrollPosition, GUISkin skin = null)
        {
            if (skin != null)
            {
                GUI.skin = skin;
            }

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, false);
            content?.Invoke();
            GUILayout.EndScrollView();

            if (skin != null)
            {
                GUI.skin = null;
            }
        }

        public static void Vertical(Action content, GUIStyle style = null)
        {
            if (style != null)
            {
                GUILayout.BeginVertical(style);
            }
            else
            {
                GUILayout.BeginVertical();
            }
            content?.Invoke();
            GUILayout.EndHorizontal();
        }

        public static void VerticalScroll(Action content, ref Vector2 scrollPosition, GUISkin skin = null)
        {
            if (skin != null)
            {
                GUI.skin = skin;
            }

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);
            content?.Invoke();
            GUILayout.EndScrollView();

            if (skin != null)
            {
                GUI.skin = null;
            }
        }
    }
}