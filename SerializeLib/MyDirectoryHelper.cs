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
        public static readonly string _MainDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public static string CreateDir(string subdir)
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
