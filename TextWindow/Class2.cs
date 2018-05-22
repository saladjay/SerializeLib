using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using TestWindow;

namespace TextWindow
{
    [Serializable]
    public abstract class Class2 : ISerializable
    {
        [NonSerialized]
        Type _ChildType;
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Debug.WriteLine("=========================serialize start======================");
            ClassInfo.SerializationPropertyHelper(info, this);
            ClassInfo.SerializationFieldHelper(info, this);
            Debug.WriteLine("=========================serialize end======================");
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected Class2(SerializationInfo info, StreamingContext context)
        {
            Debug.WriteLine("=========================deserialize start======================");
            ClassInfo.DeserializtionPropertyHelper(info, this);
            ClassInfo.DeSerializationFieldHelper(info, this);
            Debug.WriteLine("=========================deserialize end======================");
        }

        public Class2(Type type)
        {
            _ChildType = type;
        }
    }

    [Serializable]
    public class Class3 : Class2
    {
        public string str1 { get; set; }
        public Class3() : base(typeof(Class3))
        {
            str1 = "11";
        }
    }
}
