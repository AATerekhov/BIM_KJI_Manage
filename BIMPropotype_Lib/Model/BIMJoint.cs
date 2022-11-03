using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class BIMJoint : IStructure
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public List<BIMTemplate> Templates { get; set; }
        public CoordinateSystem BaseStructure { get; set; }
        public BIMJoint() { }
        public BIMJoint(BIMTemplate bIMTemplate)
        {
            Templates = new List<BIMTemplate>() {bIMTemplate};
        }

        public void GetStarted() 
        {
            Templates = new List<BIMTemplate>();
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
    }
}
