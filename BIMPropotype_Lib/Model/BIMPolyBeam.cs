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
    public class BIMPolyBeam : BIMPart
    {
        public CustomPolyBeam PolyBeam { get; set; }

        public BIMPolyBeam() { }

        [XmlIgnore]
        public override string Class
        {
            get { return PolyBeam.PolyBeam.Class; }
            set { if (PolyBeam != null) PolyBeam.PolyBeam.Class = value; }
        }

        [XmlIgnore]
        public override string Name
        {
            get { return PolyBeam.PolyBeam.Name; }
            set { if (PolyBeam != null) PolyBeam.PolyBeam.Name = value; }
        }
        [XmlIgnore]
        public override string Profile
        {
            get { return PolyBeam.PolyBeam.Profile.ProfileString; }
            set { if (PolyBeam != null) PolyBeam.PolyBeam.Profile.ProfileString = value; }
        }
        [XmlIgnore]
        public override string Material
        {
            get { return PolyBeam.PolyBeam.Material.MaterialString; }
            set { if (PolyBeam != null) PolyBeam.PolyBeam.Material.MaterialString = value; }
        }
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
