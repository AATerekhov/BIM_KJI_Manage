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
    public class BIMPlate : BIMPart, iBIMModelObject
    {
        [XmlIgnore]
        public ContourPlate ContourPlate { get; set; }
        public CustomPlate Plate { get; set; }

        [XmlIgnore]
        public override string Class
        {
            get { return Plate.nameClass; }
            set { Plate.nameClass = value; }
        }
        [XmlIgnore]
        public override string Name
        {
            get { return Plate.Name; }
            set { Plate.Name = value; }
        }
        [XmlIgnore]
        public override string Profile
        {
            get { return Plate.Profile.ProfileString; }
            set { Plate.Profile.ProfileString = value; }
        }
        [XmlIgnore]
        public override string Material
        {
            get { return Plate.Material.MaterialString; }
            set { Plate.Material.MaterialString = value; }
        }

        public BIMPlate()
        {
        }

        public BIMPlate(ContourPlate inPlate)
        {
            Plate = new CustomPlate(inPlate);
            ContourPlate = inPlate;
            UDAList = new UDACollection(ContourPlate);
            GetRebar(ContourPlate.GetReinforcements());
            Pruning = new BIMPruning(ContourPlate.GetBooleans());
            GetBolts(ContourPlate.GetBolts());
            if (CheckMainPart(ContourPlate)) GetPutInAssembly(ContourPlate);
        }
        public override void InsertMirror()
        {
            if (ContourPlate == null) GetCounterPlate();
            InsertMirror(this.ContourPlate);
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
