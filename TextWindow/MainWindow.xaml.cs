using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VersionLib;
using SerializeLib;
using System.Reflection;
using System.Collections;
using TextWindow;

namespace TestWindow
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //string path1 = AppDomain.CurrentDomain.BaseDirectory + "original.bin";
            //string path2 = AppDomain.CurrentDomain.BaseDirectory + "new.bin";
            //VersionInfo1 versionInfo1 = new VersionInfo1() { note = "new", IntValue = 5, ListValue = new List<int>() { 0, 1, 5, 6 } };
            ////VersionInfo1 versionInfo1 = null;
            //VersionInfo versionInfo = new VersionInfo() { note = "original" };
            //Serializer.SaveBinFile(versionInfo1, path2);
            //Serializer.SaveBinFile(versionInfo, path1);
            //if (Serializer.ReadBinFile(path1) is VersionInfo info)
            //{
            //    Debug.WriteLine(info.note);
            //    Debug.WriteLine(info.ReleaseDate);
            //}
            //if (Serializer.ReadBinFile(path2) is VersionInfo1 info1)
            //{
            //    Debug.WriteLine(info1.note);
            //    Debug.WriteLine(info1.ReleaseDate);
            //    foreach (PropertyInfo property in ClassInfo.GetProperties(info1))
            //    {
            //        Debug.WriteLine(property.Name + " " + property.PropertyType.ToString() + " " + property.GetValue(info1));
            //        if (property.PropertyType.IsAssignableFrom(typeof(IEnumerable)) ||typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
            //        {
            //            foreach (var item in property.GetValue(info1) as IEnumerable)
            //            {
            //                Debug.WriteLine(item);
            //            }
            //        }
            //    }
            //    Debug.WriteLine("===============================================");
            //}

            string path3 = AppDomain.CurrentDomain.BaseDirectory + "class3.bin";
            Class3 class3 = new Class3() { str1 = "str1" };
            Serializer.SaveBinFile(class3, path3);
            if (Serializer.ReadBinFile(path3) is Class3 class32)
            {
                Debug.WriteLine(class32.str1);
            }
            else
            {
                Debug.WriteLine(Serializer.ReadBinFile(path3)?.GetType());
            }
        }
    }
    [Serializable]
    public class VersionInfo1 : ISerializable
    {
        public string ReleaseDate { get; private set; }
        public string note { get; set; }
        public int IntValue { get; set; }
        public List<int> ListValue { get; set; }
        public Dictionary<int,List<int>> DictionaryValue { get; set; }
        public int bb { get { return 5; } }
        public int cc { private get; set; }
        public int AA { get; set; }
        [NonSerialized]
        public int defe = 6;
        public int intfield = 5;
        public VersionInfo1()
        {
            if (Debugger.IsAttached)
            {
                ReleaseDate = DateTime.Now.ToShortDateString();
            }
            DictionaryValue = new Dictionary<int, List<int>>();
            for (int i = 0; i < 5; i++)
            {
                DictionaryValue.Add(i, new List<int>() { i, i, i, i, i });
            }
            defe = 100;
            cc = 10;
            AA = 20;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Debug.WriteLine("=========================serialize start======================");
            ClassInfo.SerializationPropertyHelper(info, this);
            ClassInfo.SerializationFieldHelper(info, this);
            Debug.WriteLine("=========================serialize end======================");
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected VersionInfo1(SerializationInfo info, StreamingContext context)
        {
            Debug.WriteLine("=========================deserialize start======================");
            ClassInfo.DeserializtionPropertyHelper(info, this);
            ClassInfo.DeSerializationFieldHelper(info, this);
            Debug.WriteLine("=========================deserialize end======================");
        }
    }



    public static class ClassInfo
    {
        public static string GetClassName(object _Object)
        {
            return _Object?.GetType().Name;
        }

        public static PropertyInfo[] GetProperties(object _Object)
        {
            return _Object?.GetType().GetProperties();
        }

        public static FieldInfo[] GetField(object _Object)
        {
            return _Object.GetType().GetFields();
        }

        public static void SerializationPropertyHelper(SerializationInfo info,object _Object)
        {
            foreach (PropertyInfo property in ClassInfo.GetProperties(_Object))
            {
                if (property.CanWrite && property.CanRead)
                {
                    Debug.WriteLine(property.Name);
                    info.AddValue(property.Name, property.GetValue(_Object));
                }
            }
        }

        public static void DeserializtionPropertyHelper(SerializationInfo info, object _Object)
        {
            foreach (PropertyInfo property in ClassInfo.GetProperties(_Object))
            {
                if (property.CanWrite&&property.CanRead)
                {
                    Debug.WriteLine(property.Name);
                    property.SetValue(_Object, info.GetValue(property.Name, property.PropertyType));
                }
            }
        }

        public static void SerializationFieldHelper(SerializationInfo info, object _Object)
        {
            foreach (FieldInfo field in ClassInfo.GetField(_Object))
            {
                if (field.IsPublic && !field.IsNotSerialized && !field.IsStatic)
                {
                    Debug.WriteLine(field.Name);
                    info.AddValue(field.Name, field.GetValue(_Object));
                }
            }
        }

        public static void DeSerializationFieldHelper(SerializationInfo info, object _Object)
        {
            foreach (FieldInfo field in ClassInfo.GetField(_Object))
            {
                if (field.IsPublic && !field.IsNotSerialized && !field.IsStatic)
                {
                    Debug.WriteLine(field.Name);
                    field.SetValue(_Object, info.GetValue(field.Name, field.FieldType));
                }
            }
        }
    }

}
