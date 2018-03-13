using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VersionLib
{
    [Serializable]
    public class VersionInfomation
    {
        public object AppID { get; set; }
        public object DeviceID { get; set; }
        public object MachineType { get; set; }
        public object ReleaseDate { get; set; }
        public object ReleaseVersion { get; set; }
        public object HardwareVersion { get; set; }
        public object Key { get; set; }
        public object Notes { get; set; }
        public object Data { get; set; }

        private static object StaticReleaseDate;

        public VersionInfomation()
        {
            ReleaseDate = StaticReleaseDate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AutomaticallySetReleaseDate"></param>
        /// <param name="AutomaticallyCreateIniFile"></param>
        static VersionInfomation()
        {
            if (StaticConstructionController.AutomaticallyCreateIniFile&& StaticConstructionController.AutomaticallySetReleaseDate)
            {
                string filaPath = MyDirectoryHelper.CreateDir("VersionInfo") + "Config.ini";
                IniFileOperator iniFileOperator = new IniFileOperator(filaPath);
                string[] sections;
                if (iniFileOperator.GetAllSectionNames(out sections) == 0)
                {
                    if (sections.Contains("Release"))
                    {
                        StaticReleaseDate = iniFileOperator.ReadString("Release", "Date", "");
                    }
                    else
                    {
                        iniFileOperator.IniWriteValue("Release", "Date", DateTime.Now.ToLongDateString());
                        StaticReleaseDate = iniFileOperator.ReadString("Release", "Date", "");
                    }
                }
                else
                {
                    iniFileOperator.IniWriteValue("Config", "APP_ID", "0x0C");
                    iniFileOperator.IniWriteValue("Release", "Date", DateTime.Now.ToLongDateString());
                }
                Debug.WriteLine("the original struction function has been execute");
            }
            else if (StaticConstructionController.AutomaticallySetReleaseDate)
            {

                
            }
            else if (StaticConstructionController.AutomaticallyCreateIniFile)
            {

            }
            else
            {

            }
            if(StaticConstructionController.FunctionEnabled)
            {
                StaticConstructionController.CustomConstruction?.Invoke();
                Debug.WriteLine("the custom struction function has been execute");
            }
        }
    }

    public class StaticConstructionController
    {
        private static bool _FunctionEnabled = true;
        public static bool FunctionEnabled
        {
            get { return _FunctionEnabled; }
            set { _FunctionEnabled = value; }
        }
        public static bool AutomaticallySetReleaseDate { get; set; }
        public static bool AutomaticallyCreateIniFile { get; set; }

        public static Action CustomConstruction { get; set; }
        public static Action AnotherConstructionAction { get; set; }
    }

}
