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
    using System;
    using System.IO;
    using System.Threading;
    using General;
    using Helpers;

    public class AvcAddon : Addon
    {
        private Action<AvcAddon> onFetchComplete;

        /// <summary>
        ///     Creates a new AVC add-on given a version file path.
        /// </summary>
        public AvcAddon(string filePath, Action<AvcAddon> onComplete)
        {
            onFetchComplete = onComplete;

            State = AddonState.Fetching;

            // Start fetching the local add-on information.
            GetAddonInfoFromFile(filePath, OnLocalAddonInfoFetchSuccess, OnLocalAddonInfoFetchFailure);
        }

        /// <summary>
        ///     Gets an add-on information object from file.
        /// </summary>
        private static void GetAddonInfoFromFile(string filePath, Action<AvcAddonInfo> onSuccess, Action onFailure)
        {
            if (string.IsNullOrEmpty(filePath) == false && File.Exists(filePath) && onSuccess != null)
            {
                ThreadPool.QueueUserWorkItem(state => { onSuccess.Invoke(new AvcAddonInfo(File.ReadAllText(filePath), filePath)); });
            }
            else if (onFailure != null)
            {
                onFailure.Invoke();
            }
        }

        /// <summary>
        ///     Gets an add-on information object from the web and returning it via the onSuccess callback.
        /// </summary>
        private static void GetAddonInfoFromWeb(string url, Action<AvcAddonInfo> onSuccess, Action onFailure)
        {
            if (string.IsNullOrEmpty(url) == false && onSuccess != null)
            {
                WebHelper.GetWebContent(url, json => onSuccess.Invoke(new AvcAddonInfo(json, url)), onFailure);
            }
            else if (onFailure != null)
            {
                onFailure.Invoke();
            }
        }

        /// <summary>
        ///     Called when the add-on fails to fetch the local information.
        /// </summary>
        private void OnLocalAddonInfoFetchFailure()
        {
            State = AddonState.Failure;
            onFetchComplete?.Invoke(null);
        }

        /// <summary>
        ///     Called when the add-on succeeds in fetching the local information.
        /// </summary>
        private void OnLocalAddonInfoFetchSuccess(AvcAddonInfo localAddonInfo)
        {
            LocalAddonInfo = localAddonInfo;

            // Fetch the remote add-on information.
            GetAddonInfoFromWeb(localAddonInfo?.Url, OnRemoteAddonInfoFetchSuccess, OnRemoteAddonInfoFetchFailure);
        }

        /// <summary>
        ///     Called when fetching the remote add-on information failed.
        /// </summary>
        private void OnRemoteAddonInfoFetchFailure()
        {
            State = AddonState.Success;
            onFetchComplete?.Invoke(this);
        }

        /// <summary>
        ///     Called when fetching the remote add-on information succeeded.
        /// </summary>
        private void OnRemoteAddonInfoFetchSuccess(AvcAddonInfo remoteAddonInfo)
        {
            RemoteAddonInfo = remoteAddonInfo;
            State = AddonState.Success;
            onFetchComplete?.Invoke(this);
        }
    }
}