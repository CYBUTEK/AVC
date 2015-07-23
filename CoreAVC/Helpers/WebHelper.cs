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

namespace CoreAVC.Helpers
{
    using System;
    using System.Collections;
    using UnityEngine;

    public class WebHelper : MonoBehaviour
    {
        private static WebHelper instance;

        /// <summary>
        ///     Gets a current web helper instance.
        /// </summary>
        public static WebHelper Instance => instance ?? (instance = new GameObject("WebHelper").AddComponent<WebHelper>());

        /// <summary>
        ///     Gets content from the supplied url and returns it via an onSuccess callback.
        /// </summary>
        public static void GetWebContent(string url, Action<string> onSuccess, Action onFailure = null)
        {
            Instance.StartCoroutine(GetWebContent_Coroutine(url, onSuccess, onFailure));
        }

        /// <summary>
        ///     Coroutine used to fetch remote content.
        /// </summary>
        private static IEnumerator GetWebContent_Coroutine(string url, Action<string> onSuccess, Action onFailure)
        {
            WWW www = new WWW(url);
            yield return www;

            if (string.IsNullOrEmpty(www.error) && onSuccess != null)
            {
                onSuccess.Invoke(www.text);
            }
            else if (onFailure != null)
            {
                onFailure.Invoke();
            }
        }
    }
}