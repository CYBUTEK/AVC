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
    using UnityEngine;

    [KSPAddon(KSPAddon.Startup.Instantly, false)]
    public class Loader : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnLevelWasLoaded()
        {
            if (HighLogic.LoadedScene != GameScenes.LOADING && HighLogic.LoadedScene != GameScenes.MAINMENU)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            AddonLibrary.LoadAddons();
            gameObject.AddComponent<Overlay>();
        }

        private void Update()
        {
            if (AddonLibrary.IsLoading)
            {
                return;
            }
            enabled = false;
        }
    }
}