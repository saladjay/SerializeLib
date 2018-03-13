using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VersionLib
{
    public class MyDirectoryHelper
    {
        public static readonly string _MainDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public static string CreateDir(string subdir)
        {
            StringBuilder stringBuilder = new StringBuilder(_MainDirectory);
            string path = stringBuilder.Append(subdir).ToString();
            if (Directory.Exists(path))
            {
                Console.WriteLine("此文件夹已经存在，无需创建！");
                return stringBuilder.Append('\\').ToString();
            }
            else
            {
                Directory.CreateDirectory(path);
                Console.WriteLine(path + " 创建成功!");
                return stringBuilder.Append('\\').ToString();
            }
        }

        public static void CreateNameDir(string name)
        {
            if (name.Length != 0)
            {
                CreateDir(name);
            }
            else
            {
                Console.WriteLine("必须指定文件夹名称，才能创建！");
            }
        }

        public static string[] GetAllFileName(string path, string fileType)
        {
            return Directory.GetFiles(path, fileType);
        }
    }
}
