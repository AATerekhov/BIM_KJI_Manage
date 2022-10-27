using BIMPropotype_Lib.Model.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using TSM = Tekla.Structures.Model;

namespace BIMPropotype_Lib.Model.Custom
{
    [Serializable]
    public  class CustomBooleanBeam:IFormObject
    {
        public TSM.BooleanPart.BooleanTypeEnum BooleanType { get; set; }
        public CustomBeam Beam { get; set; }//Хранит OperativePart
        public CustomBooleanBeam() { }
        public CustomBooleanBeam(TSM.BooleanPart booleanBeam)
        {
            BooleanType = booleanBeam.Type;
            Beam = new CustomBeam(booleanBeam.OperativePart as TSM.Beam);
        }
        public void FormObject()
        {
            Beam.Beam.Class = TSM.BooleanPart.BooleanOperativeClassName;
        }

        public ModelObject GetModelObject()
        {
            FormObject();
            return Beam.GetModelObject();
        }
    }
}
