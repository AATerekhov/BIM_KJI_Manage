using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;

namespace BIMPropotype_Lib.Model.Custom
{
    [Serializable]
    public class BoltSet
    {
        public bool Bolt { get; set; }
        public bool Washer1 { get; set; }
        public bool Washer2 { get; set; }
        public bool Washer3 { get; set; }
        public bool Nut1 { get; set; }
        public bool Nut2 { get; set; }
        public double Length { get; set; }

        public BoltSet() { }
        public BoltSet(TSM.BoltGroup boltXYList)
        {
            Length = boltXYList.Length;
            Bolt = boltXYList.Bolt;
            Washer1 = boltXYList.Washer1;
            Washer2 = boltXYList.Washer2;
            Washer3 = boltXYList.Washer3;
            Nut1 = boltXYList.Nut1;
            Nut2 = boltXYList.Nut2;
        }

        public void GetBoltSet(TSM.BoltGroup boltXYList)
        {
            boltXYList.Bolt = Bolt;
            boltXYList.Washer1 = Washer1;
            boltXYList.Washer2 = Washer2;
            boltXYList.Washer3 = Washer3;
            boltXYList.Nut1 = Nut1;
            boltXYList.Nut2 = Nut2;
            boltXYList.Length = Length;
        }
    }
}
