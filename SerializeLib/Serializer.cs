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
        /// <param name="versionInfomation">versioninfomation instance</param>
        /// <param name="filePath">path to save, has the default value "VersionInfo/Config.bin"</param>
        /// <returns>false: there is a bin file that has the same path or something goes wrong</returns>
        public static bool SaveToBinary(VersionInfomation versionInfomation, string filePath = null)
        {
            try
            {
                if(filePath==null)
                    filePath= MyDirectoryHelper.CreateDir("VersionInfo") + "Config.bin";
                IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                if (File.Exists(filePath))
                {
                    return false;
                }
                Stream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, versionInfomation);
                stream.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// read binary file.
        /// </summary>
        /// <param name="filePath">path to read, has the default value "VersionInfo/Config.bin"</param>
        /// <returns>null: the path is wrong or something goes wrong</returns>
        public static VersionInfomation ReadFromBinary(string filePath = null)
        {
            if (filePath == null)
                filePath = MyDirectoryHelper.CreateDir("VersionInfo") + "Config.bin";
            if (File.Exists(filePath))
            {
                try
                { 
                    Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
                    IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    VersionInfomation versionInfomation = (VersionInfomation)formatter.Deserialize(stream);
                    stream.Close();
                    return versionInfomation;
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
    }
}
