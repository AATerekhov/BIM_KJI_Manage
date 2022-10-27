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
    public class CustomBooleanPlate :IFormObject
    {
        public TSM.BooleanPart.BooleanTypeEnum BooleanType { get; set; }
        public CustomPlate Plate { get; set; }//Хранит OperativePart
        public CustomBooleanPlate() { }

        public CustomBooleanPlate(TSM.BooleanPart booleanPlate)
        {

            BooleanType = booleanPlate.Type;
            Plate = new CustomPlate(booleanPlate.OperativePart as TSM.ContourPlate);
        }
        public void FormObject()
        {
            Plate.ContourPlate.Class = TSM.BooleanPart.BooleanOperativeClassName;
        }

        public ModelObject GetModelObject()
        {
            FormObject();
            return Plate.GetModelObject();
        }
    }
}
