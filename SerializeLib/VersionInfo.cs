using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace SerializeLib
{
    [Serializable]
    public class VersionInfo : ISerializable
    {
        public string ReleaseDate { get; private set; }
        public string note { get; set; }
        public VersionInfo()
        {
            if (Debugger.IsAttached)
            {
                ReleaseDate = DateTime.Now.ToShortDateString();
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ReleaseDate", ReleaseDate);
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected VersionInfo(SerializationInfo info, StreamingContext context)
        {
            ReleaseDate = info.GetString("ReleaseDate");
        }
    }
}
