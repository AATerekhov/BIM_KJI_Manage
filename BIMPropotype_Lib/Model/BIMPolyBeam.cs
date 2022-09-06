using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TSM = Tekla.Structures.Model;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class BIMPolyBeam : BIMPart, iBIMModelObject
    {
        public CustomPolyBeam PolyBeam { get; set; }

        public BIMPolyBeam() { }

        public BIMPolyBeam(TSM.PolyBeam inPolybeam)
        {
            PolyBeam = new CustomPolyBeam(inPolybeam);

            UDAList = new UDACollection(inPolybeam);
            GetRebar(inPolybeam.GetReinforcements());
            Pruning = new BIMPruning(inPolybeam.GetBooleans());
            GetBolts(inPolybeam.GetBolts());
            if (CheckMainPart(inPolybeam)) GetPutInAssembly(inPolybeam);
        }
        public override void InsertMirror()
        {
            PolyBeam.GetPolyBeam();
            InsertMirror(PolyBeam.PolyBeam);
        }
        public override void Insert()
        {
            PolyBeam.GetPolyBeam();
            Insert(PolyBeam.PolyBeam);
        }       
    }
}
