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
    using System.Linq;
    using General;

    public static class Utils
    {
        private const string TAB = "    ";

        public static string Concat(string separator, params object[] objects)
        {
            string concatString = string.Empty;
            int objCount = objects.Length;
            for (int i = 0; i < objCount; ++i)
            {
                concatString = concatString + objects[i];
                if (i < objCount - 1)
                {
                    concatString = concatString + separator;
                }
            }
            return concatString;
        }

        public static string ConcatLine(params object[] objects)
        {
            return Concat(Environment.NewLine, objects);
        }

        public static string ConcatTab(int firstLineTabs, int tabs, params object[] objects)
        {
            string concatString = string.Empty;
            string firstLineTabString = string.Empty;
            string tabString = string.Empty;

            for (int i = 0; i < firstLineTabs; ++i)
            {
                firstLineTabString = firstLineTabString + TAB;
            }

            for (int i = 0; i < tabs; ++i)
            {
                tabString = tabString + TAB;
            }

            int objCount = objects.Length;
            for (int i = 0; i < objCount; ++i)
            {
                if (i == 0)
                {
                    concatString = concatString + firstLineTabString + objects[i];
                }
                else
                {
                    concatString = concatString + tabString + objects[i];
                }

                if (i < objCount - 1)
                {
                    concatString = concatString + Environment.NewLine;
                }
            }
            return concatString;
        }

        public static VersionObject GetAssemblyVersion(string assemblyName)
        {
            Version version = AssemblyLoader.loadedAssemblies.First(assembly => assembly.name == assemblyName)?.assembly.GetName().Version;
            return version != null ? new VersionObject(version) : null;
        }
    }
}