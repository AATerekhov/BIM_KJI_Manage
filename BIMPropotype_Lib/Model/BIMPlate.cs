using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;
using System.Xml.Serialization;
using BIMPropotype_Lib.ExtentionAPI.PartChildren;
using BIMPropotype_Lib.Model.Custom;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class BIMPlate : BIMPart
    {
        public CustomPlate Plate { get; set; }
        public BIMPlate() { }

        #region Interface //Интрефейс нужно перенести во ViewModel!!!
        [XmlIgnore]
        public override string Class
        {
            get { return Plate.ContourPlate.Class; }
            set { if (Plate != null) Plate.ContourPlate.Class = value; }
        }
        [XmlIgnore]
        public override string Name
        {
            get { return Plate.ContourPlate.Name; }
            set { if (Plate != null) Plate.ContourPlate.Name = value; }
        }
        [XmlIgnore]
        public override string Profile
        {
            get { return Plate.ContourPlate.Profile.ProfileString; }
            set { if (Plate != null) Plate.ContourPlate.Profile.ProfileString = value; }
        }
        [XmlIgnore]
        public override string Material
        {
            get { return Plate.ContourPlate.Material.MaterialString; }
            set { if (Plate != null) Plate.ContourPlate.Material.MaterialString = value; }
        }
        #endregion //Интрефейс нужно перенести во ViewModel!!!



        public BIMPlate(ContourPlate inPlate)
        {
            Plate = new CustomPlate(inPlate);
            GetRebar(Plate.ContourPlate.GetReinforcements());
            Pruning = new BIMPruning(Plate.ContourPlate.GetBooleans());
            GetBolts(Plate.ContourPlate.GetBolts());
            if (Plate.ContourPlate.CheckMainPart()) GetPutInAssembly(Plate.ContourPlate);
        }
        public override void FormPart()
        {
            Plate.GetContourPlate();
        }
    }
}
