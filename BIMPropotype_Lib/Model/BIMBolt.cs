using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;
using TSM = Tekla.Structures.Model;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class BIMBolt
    {
        public BIMBolt() { }

        internal BIMBolt(BoltGroup boltGroup)
        {

        }

        public UDACollection UDAList { get; set; }
        public BoltGroupType BoltType { get; set; }

    }
    public enum BoltGroupType 
    {
        array =0,
        xyList =1,
        Circle = 2,
    }

}
