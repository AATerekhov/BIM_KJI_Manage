using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Tekla.Structures.Model;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public abstract class PartChildren : IUDAContainer, IModelOperations
    {
        [XmlIgnore]
        public ModelObject Father { get; set; }
        public UDACollection UDAList { get; set; }

        public virtual void Insert()
        {
        }
        public virtual void InsertMirror()
        {
        }
    }
}
