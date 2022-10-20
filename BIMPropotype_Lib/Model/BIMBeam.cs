using BIMPropotype_Lib.ExtentionAPI.PartChildren;
using BIMPropotype_Lib.Model.Custom;
using System;
using System.Xml.Serialization;
using Tekla.Structures.Model;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class BIMBeam:BIMPart
    {
        public CustomBeam Beam { get; set; }
        public BIMBeam() { }

        #region Interface //Интрефейс нужно перенести во ViewModel!!!
        [XmlIgnore]
        public override string Name
        {
            get { return Beam.Beam.Name; }
            set { if (Beam != null) Beam.Beam.Name = value; }
        }

        [XmlIgnore]
        public override string Class
        {
            get { return Beam.Beam.Class; }
            set { if (Beam != null) Beam.Beam.Class = value; }
        }
        [XmlIgnore]
        public override string Profile
        {
            get { return Beam.Beam.Profile.ProfileString; }
            set { if (Beam != null) Beam.Beam.Profile.ProfileString = value; }
        }
        [XmlIgnore]
        public override string Material
        {
            get { return Beam.Beam.Material.MaterialString; }
            set { if (Beam != null) Beam.Beam.Material.MaterialString = value; }
        }
        #endregion//Интрефейс нужно перенести во ViewModel!!!

        public BIMBeam(Beam beam )
        {
            Beam = new CustomBeam(beam);

            GetRebar(Beam.Beam.GetReinforcements());
            Pruning = new BIMPruning(Beam.Beam.GetBooleans());
            GetBolts(Beam.Beam.GetBolts());
            if (Beam.Beam.CheckMainPart()) GetPutInAssembly(Beam.Beam);
        }
        public override void FormPart()
        {
            Beam.GetBeam();
        }
    }
}
