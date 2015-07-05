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
    using System.IO;
    using System.Reflection;
    using UnityEngine;

    public static class TextureHelper
    {
        public static string TextureDirectory { get; } = GetTextureDirectory();

        public static Texture2D GetTexture(string fileName, int width, int height)
        {
            Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);

            string texturePath = Path.Combine(TextureDirectory, fileName);
            if (string.IsNullOrEmpty(texturePath) == false && File.Exists(texturePath))
            {
                texture.LoadImage(File.ReadAllBytes(texturePath));
            }

            return texture;
        }

        private static string GetTextureDirectory()
        {
            string callingAssemblyLocation = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            if (string.IsNullOrEmpty(callingAssemblyLocation) == false)
            {
                return Path.Combine(callingAssemblyLocation, "Textures");
            }
            return null;
        }
    }
}