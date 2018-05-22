using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SerializeLib
{
    [Serializable]
    public class ExampleVersion : VersionInfo, ISerializable
    {
        public int IntData { get; set; }
        public string StringData { get; set; }
        public List<int> ListData { get; set; }

    }
}
