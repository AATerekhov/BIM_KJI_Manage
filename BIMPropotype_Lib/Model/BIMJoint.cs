using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class BIMJoint : IStructure, IBIMCollection, IReference
    {
        public List<BIMPartChildren> Children { get; set; }
        public CoordinateSystem BaseStructure { get; set; }
        public string Name { get; set; }
        public string Prefix { get; set; }
        public BIMJoint() { }
        public BIMJoint(BIMPartChildren bIMTemplate)
        {
            Children = new List<BIMPartChildren>() {bIMTemplate};
        }

        public void GetStarted() 
        {
            Children = new List<BIMPartChildren>();
            BaseStructure = new CoordinateSystem();
        }

        public void Insert(IStructure father)
        {
            throw new NotImplementedException();
        }

        public void InsertMirror(IStructure father)
        {
            throw new NotImplementedException();
        }

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
