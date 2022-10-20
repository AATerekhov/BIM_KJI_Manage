using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMPropotype_Lib.Model
{
    public abstract class ChildrenAssembly : IModelOperations, IUDAContainer
    {
        public BIMAssembly Father { get; set; }
        public UDACollection UDAList { get; set; }

        public virtual void Insert()
        {
        }
        public virtual void InsertMirror()
        {
        }
    }
}
