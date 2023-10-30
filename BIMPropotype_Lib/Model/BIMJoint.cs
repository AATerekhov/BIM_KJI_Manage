using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;
using TSM = Tekla.Structures.Model;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class BIMJoint :Reference, IStructure, IBIMCollection
    {
        public List<BIMPart> Children { get; set; }
        public List<BIMPartChildren> OwnChildren { get; set; }
        public CoordinateSystem BaseStructure { get; set; }
        public BIMJoint() { }
        public BIMJoint(MetaDirectory Meta, CoordinateSystem cs ) :base(Meta)
        {
            Children = new List<BIMPart>();
            OwnChildren = new List<BIMPartChildren>();
            BaseStructure = cs;
        }

        public void AddOwn(BIMPartChildren bIMPartChildren, Matrix matrix) 
        {
            bIMPartChildren.BaseStructure.Origin = matrix.Transform(bIMPartChildren.BaseStructure.Origin);
            OwnChildren.Add(bIMPartChildren);
        }
        public void AddPart(BIMPart bIMPart )
        {
            Children.Add(bIMPart);
        }

        public void Insert(IStructure father)
        { 
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

        public void SelectInModel(TSM.Model model)
        {
            throw new NotImplementedException();
        }
        public override string ToString()
        {
            return Meta.ToString();
        }
    }
}
