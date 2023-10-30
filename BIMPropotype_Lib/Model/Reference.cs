using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using BIMPropotype_Lib.Model;
using BIMPropotype_Lib.ViewModel;
using TSM = Tekla.Structures.Model;
using BIMPropotype_Lib.ExtentionAPI.Conductor;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public abstract class Reference
    {
        public MetaDirectory Meta { get; set; }
        public int InstanceNumber { get; set; }//Номер экземпляра для нумерации копий с разными координатами.       
        public bool IsLink { get; set; }

        public Reference() { }

        public Reference(MetaDirectory meta)
        {
            Meta = meta;
        }       
    }
}
