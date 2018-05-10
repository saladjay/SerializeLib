using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VersionLib
{
    public class Serializer
    {
        /// <summary>
        /// save binary file.
        /// </summary>
        /// <param name="versionInformation">versioninfomation instance</param>
        /// <param name="filePath">path to save, has the default value "VersionInfo/Config.bin"</param>
        /// <returns>false: there is a bin file that has the same path or something goes wrong</returns>
        public static bool SaveToBinary(VersionInformation versionInformation, string filePath = null)
        {
            //try
            //{
                if(filePath==null)
                    filePath= MyDirectoryHelper.CreateDir("VersionInfo") + "Config.bin";
                IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                Stream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, versionInformation);
                stream.Close();
                return true;
            //}
            //catch (Exception)
            //{
            //    return false;
            //}
        }
        /// <summary>
        /// save binary file.
        /// </summary>
        /// <param name="versionInformation"></param>
        /// <param name="fileName">do not end with ".bin", suffix name will be added automatically</param>
        /// <param name="fileDirectory">if this is null, this function will write file in default directory</param>
        /// <returns></returns>
        public static bool SaveToBinary(VersionInformation versionInformation, string fileName, string fileDirectory)
        {
            string filePath = new StringBuilder(fileDirectory).Append(fileName).Append(".bin").ToString();
            return SaveToBinary(versionInformation, filePath);
        }

        /// <summary>
        /// read binary file.
        /// </summary>
        /// <param name="filePath">path to read, has the default value "VersionInfo/Config.bin"</param>
        /// <returns>null: the path is wrong or something goes wrong</returns>
        public static VersionInformation ReadFromBinary(string filePath = null)
        {
            if (filePath == null)
                filePath = MyDirectoryHelper.CreateDir("VersionInfo") + "Config.bin";
            if (File.Exists(filePath))
            {
                try
                {
                    Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
                    IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    VersionInformation versionInformation = (VersionInformation)formatter.Deserialize(stream);
                    stream.Close();
                    return versionInformation;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// read binary file.
        /// </summary>
        /// <param name="fileName">do not end with ".bin", suffix name will be added automatically</param>
        /// <param name="fileDirectory">if this is null, this function will read file from default directory</param>
        /// <returns></returns>
        public static VersionInformation ReadFromBinary(string fileName, string fileDirectory)
        {
            string filePath = new StringBuilder(fileDirectory).Append(fileName).Append(".bin").ToString();
            return ReadFromBinary(filePath);
        }
    }
}
