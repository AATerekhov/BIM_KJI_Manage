using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;
using System.Xml.Serialization;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class BIMPlate : BIMPart
    {
        [XmlIgnore]
        public ContourPlate ContourPlate { get; set; }

        public CustomPlate Plate { get; set; }
        public BIMPlate()
        {

        }

        public BIMPlate(ContourPlate inPart) 
        {
            ContourPlate = inPart;
            Plate = new CustomPlate(inPart);
            Rebars = GetRebar(ContourPlate.GetReinforcements());
            Pruning = new BIMPruning(ContourPlate.GetBooleans());
        }

        public override void Insert()
        {
            if (ContourPlate == null) GetCounterPlate();
            Insert(this.ContourPlate);
        }
        public void GetCounterPlate() 
        {
            ContourPlate = Plate.GetContourPlate();
        }
    }
}
