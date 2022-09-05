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
    public class BIMPolyBeam : BIMPart, iBIMModelObject
    {
        [XmlIgnore]
        public PolyBeam PolyBeam { get; set; }
        public CustomPolyBeam Plate { get; set; }

        public BIMPolyBeam() { }

        public BIMPolyBeam(PolyBeam inPolybeam)
        {
            //Plate = new CustomPlate(inPlate);

            //ContourPlate = inPlate;
            //UDAList = new UDACollection(ContourPlate);
            //GetRebar(ContourPlate.GetReinforcements());
            //Pruning = new BIMPruning(ContourPlate.GetBooleans());
            //GetBolts(ContourPlate.GetBolts());
            //if (CheckMainPart(ContourPlate)) GetPutInAssembly(ContourPlate);
        }
        public override void InsertMirror()
        {
            //    if (ContourPlate == null) GetCounterPlate();
            //    InsertMirror(this.ContourPlate);
        }
        public override void Insert()
        {
            //if (ContourPlate == null) GetCounterPlate();
            //Insert(this.ContourPlate);
        }
        public void GetCounterPlate()
        {
            //ContourPlate = Plate.GetContourPlate();
        }
    }
}
