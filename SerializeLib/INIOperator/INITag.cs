using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SerializeLib.INIOperator
{
    public class Section
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = string.IsNullOrWhiteSpace(value) ? throw new ArgumentNullException() : value; }
        }
        public Dictionary<string, string> KeyValuePairs { get; private set; } = new Dictionary<string, string>();
        public Section(string name)
        {
            Name = name;
        }

        public void AddNewKeyValue(string key,string value)
        {
            KeyValuePairs[key] = value;
        }
    }

    public class INIFile
    {
        public string FileName { get; set; }
        public string FileDirectory { get; set; }
        public string FilePath { get; set; }
        public List<Section> Sections { get; private set; } = new List<Section>();


        public INIFile()
        {

        }

        public INIFile(string filePath)
        {
            FilePath = filePath;
            string[] strs = filePath.Split('\\');
            FileName = strs.Last();
            FileDirectory = filePath.Remove(filePath.Length - FileName.Length);
        }

        public INIFile(string fileDirectory, string fileName)
        {
            FilePath = fileDirectory + FileName;
            FileDirectory = fileDirectory;
            FileName = fileName;
        }

        public void Save(string filePath)
        {
            FilePath = filePath;
            string[] strs = filePath.Split('\\');
            FileName = strs.Last();
            FileDirectory = filePath.Remove(filePath.Length - FileName.Length);
            if (string.IsNullOrWhiteSpace(FilePath))
            {
                return;
            }
            if (!Directory.Exists(FileDirectory))
            {
                Directory.CreateDirectory(FileDirectory);
            }
            InternalSave();
        }

        public void Save()
        {
            if (string.IsNullOrWhiteSpace(FilePath))
            {
                return;
            }
            if (!Directory.Exists(FileDirectory))
            {
                Directory.CreateDirectory(FileDirectory);
            }
            InternalSave();
        }

        public void Save(string fileDirectory, string fileName)
        {
            FilePath = fileDirectory + FileName;
            FileDirectory = fileDirectory;
            FileName = fileName;
            if (string.IsNullOrWhiteSpace(FilePath))
            {
                return;
            }
            if (!Directory.Exists(FileDirectory))
            {
                Directory.CreateDirectory(FileDirectory);
            }
            InternalSave();
        }

        private void InternalSave()
        {
            foreach (Section section in Sections)
            {
                foreach (var keyValue in section.KeyValuePairs)
                {
                    INIReadWriteFunction.Write(FilePath, section.Name, keyValue.Key, keyValue.Value);
                }
            }
        }

        public void Load()
        {
            if (INIReadWriteFunction.GetAllSections(FilePath, out string[] sections))
            {
                foreach (var sectionName in sections)
                {
                    Section section = new Section(sectionName);
                    if (INIReadWriteFunction.GetAllKeyValues(FileName, sectionName, out string[] keys, out string[] values))
                    {
                        for (int i = 0; i < keys.Length && i < values.Length; i++)
                        {
                            section.AddNewKeyValue(keys[i], values[i]);
                        }
                    }
                    Sections.Add(section);
                }
            }
        }

        public bool FindValue(string sectionName, string key, out string value)
        {
            bool result = false;
            value = null;
            foreach (var section in Sections)
            {
                if (section.Name.Equals(sectionName))
                {
                    result = section.KeyValuePairs.TryGetValue(key, out value);
                    break;
                }
            }
            return result;
        }
    }


    public static class INIReadWriteFunction
    {
        // 读写INI文件相关。
        [DllImport("kernel32.dll")]
        public static extern int GetPrivateProfileInt(
            string lpAppName,
            string lpKeyName,
            int nDefault,
            string lpFileName
            );

        [DllImport("kernel32.dll")]
        public static extern int GetPrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpDefault,
            StringBuilder lpReturnedString,
            int nSize,
            string lpFileName
            );

        [DllImport("kernel32.dll")]
        public static extern int WritePrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpString,
            string lpFileName
            );

        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileSectionNames", CharSet = CharSet.Ansi)]
        public static extern int GetPrivateProfileSectionNames(
            IntPtr lpszReturnBuffer,
            int nSize,
            string filePath);

        [DllImport("KERNEL32.DLL ", EntryPoint = "GetPrivateProfileSection", CharSet = CharSet.Ansi)]
        public static extern int GetPrivateProfileSection(
            string lpAppName,
            byte[] lpReturnedString,
            int nSize,
            string filePath);

        /// <summary>
        /// read an integer from the section accroding to the key
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ReadInt(string fileName, string section, string key, int defaultValue = 0)
        {
            return GetPrivateProfileInt(section, key, defaultValue, fileName);
        }
        /// <summary>
        /// read a string from the section accroding to the key
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string ReadString(string fileName, string section, string key, string defaultValue)
        {
            StringBuilder vRetSb = new StringBuilder(2048);
            GetPrivateProfileString(section, key, defaultValue, vRetSb, 2048, fileName);
            return vRetSb.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Write(string fileName, string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, fileName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="section"></param>
        public static void DeleteSection(string fileName, string section)
        {
            WritePrivateProfileString(section, null, null, fileName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public static void DeleteAllSection(string fileName)
        {
            WritePrivateProfileString(null, null, null, fileName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static bool GetAllSections(string fileName,out string[] sections)
        {
            int MAX_BUFFER = 32767;
            IntPtr pReturnedString = Marshal.AllocCoTaskMem(MAX_BUFFER);
            int bytesReturned = GetPrivateProfileSectionNames(pReturnedString, MAX_BUFFER, fileName);
            if (bytesReturned == 0)
            {
                sections = null;
                return false;
            }
            string local = Marshal.PtrToStringAnsi(pReturnedString, (int)bytesReturned).ToString();
            Marshal.FreeCoTaskMem(pReturnedString);
            //use of Substring below removes terminating null for split
            sections = local.Substring(0, local.Length - 1).Split('\0');
            return true;
        }

        /// <summary>
        /// 得到某个节点下面所有的key和value组合
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="section"></param>
        /// <param name="keys"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool GetAllKeyValues(string fileName, string section, out string[] keys, out string[] values)
        {
            byte[] b = new byte[65535];
            GetPrivateProfileSection(section, b, b.Length, fileName);
            string s = System.Text.Encoding.Default.GetString(b);
            string[] tmp = s.Split((char)0);
            List<string> result = new List<string>();
            foreach (string r in tmp)
            {
                if (r != string.Empty)
                    result.Add(r);
            }
            keys = new string[result.Count];
            values = new string[result.Count];
            for (int i = 0; i < result.Count; i++)
            {
                string[] item = result[i].ToString().Split('=');
                if (item.Length == 2)
                {
                    keys[i] = item[0].Trim();
                    values[i] = item[1].Trim();
                }
                else if (item.Length == 1)
                {
                    keys[i] = item[0].Trim();
                    values[i] = "";
                }
                else if (item.Length == 0)
                {
                    keys[i] = "";
                    values[i] = "";
                }
            }
            return true;
        }
    }

}
