using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Tekla.Structures.Model;
using BIMPropotype_Lib.ExtentionAPI.PartChildren;
using BIMPropotype_Lib.ExtentionAPI.Mirror;
using BIMPropotype_Lib.Model.Custom;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    //Класс контейнер для обобщенного хранения BIMPart.
    public class BIMPart: IUDAContainer
    {
        public List<BIMPartChildren> Children { get; set; }
        #region BoxObject
        public CustomBeam Beam { get; set; }
        public CustomPlate Plate { get; set; }
        public CustomPolyBeam PolyBeam { get; set; }
        public PartType Type { get; set; }
        public UDACollection UDAList { get; set; }

        public Part GetPart()
        {
            if ((int)Type == 1)
            {
                return Beam.Beam;
            }
            if ((int)Type == 2)
            {
                return Plate.ContourPlate;
            }
            if ((int)Type == 3)
            {
                return PolyBeam.PolyBeam;
            }
            return null;
        }
        public IFormObject GetCustomPart()
        {
            if ((int)Type == 1)
            {
                return Beam;
            }
            if ((int)Type == 2)
            {
                return Plate;
            }
            if ((int)Type == 3)
            {
                return PolyBeam;
            }
            return null;
        }
        #endregion

        public BIMPart() { }
        public BIMPart(Part part)
        {
            UDAList = new UDACollection(part);
            if (part is Beam beam)
            {
                Type = PartType.beam;
                Beam = new CustomBeam(beam);
            }
            if (part is ContourPlate plate)
            {
                Type = PartType.plate;
                Plate = new CustomPlate(plate);
            }
            if (part is PolyBeam polyBeam)
            {
                Type = PartType.polyBeam;
                PolyBeam =new CustomPolyBeam(polyBeam);
            }

            Children = this.GetChildren();
        }
        public  void Insert(BIMAssembly father)
        {
            var customPart = GetCustomPart();
            if (customPart != null)
            {
                var modeelPart = customPart.GetModelObject() as Part;
                modeelPart.Insert();
                UDAList.GetUDAToPart(modeelPart);
            }
        }

        public  void InsertMirror(BIMAssembly father)
        {
            var customPart = GetCustomPart();
            if (customPart != null)
            {
                var modeelPart = customPart.GetModelObject() as Part;
                modeelPart.InsertMirror(true);
                UDAList.GetUDAToPart(modeelPart);
            }
        }
    }
    public enum PartType
    {
        no = 0,
        beam = 1,
        plate = 2,
        polyBeam = 3,
    }
}
