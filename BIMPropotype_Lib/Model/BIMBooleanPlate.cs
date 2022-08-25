using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class BIMBooleanPlate
    {
        public TSM.BooleanPart.BooleanTypeEnum BooleanType { get; set; }
        public CustomPlate Plate { get; set; }
        public BIMBooleanPlate()
        {

        }

        public BIMBooleanPlate(TSM.BooleanPart booleanPart)
        {
            if(booleanPart.OperativePart is TSM.ContourPlate plate) Plate = new CustomPlate(plate);
            BooleanType = booleanPart.Type;
        }
    }
}
