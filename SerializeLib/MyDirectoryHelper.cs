using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VersionLib
{
    internal class MyDirectoryHelper
    {
        internal static readonly string _MainDirectory = AppDomain.CurrentDomain.BaseDirectory;
        internal static string CreateDir(string subdir)
        {
            StringBuilder stringBuilder = new StringBuilder(_MainDirectory);
            string path = stringBuilder.Append(subdir).ToString();
            if (Directory.Exists(path))
            {
                return stringBuilder.Append('\\').ToString();
            }
            else
            {
                Directory.CreateDirectory(path);
                return stringBuilder.Append('\\').ToString();
            }
        }
    }
}
