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
    public class BIMBoxPart:IModelOperations
    {
        public BIMBeam Beam { get; set; }
        public BIMPlate Plate { get; set; }
        public BIMPolyBeam PolyBeam { get; set; }
        public PartType Type { get; set; }
        public BIMBoxPart() { }
        public BIMBoxPart(Part part)
        {
            if (part is Beam beam)
            {
                Type = PartType.beam;
                Beam = new BIMBeam(beam);
            }
            if (part is ContourPlate plate)
            {
                Type = PartType.plate;
                Plate = new BIMPlate(plate);
            }
            if (part is PolyBeam polyBeam)
            {
                Type = PartType.polyBeam;
                PolyBeam =new BIMPolyBeam(polyBeam);
            }
        }

        public void Insert()
        {
            if ((int)Type == 1)
            {
                Beam.Insert();
            }
            if ((int)Type == 2)
            {
                Plate.Insert();
            }
            if ((int)Type == 3)
            {
                PolyBeam.Insert();
            }
        }

        public void InsertMirror()
        {
            if ((int)Type == 1)
            {
                Beam.InsertMirror();
            }
            if ((int)Type == 2)
            {
                Plate.InsertMirror();
            }
            if ((int)Type == 3)
            {
                PolyBeam.InsertMirror();
            }
        }

        public Part GetPart() 
        {
            if ((int)Type == 1)
            {
                return Beam.Beam.Beam;
            }
            if ((int)Type == 2)
            {
               return Plate.Plate.ContourPlate;
            }
            if ((int)Type == 3)
            {
                return PolyBeam.PolyBeam.PolyBeam;
            }
            return null;
        }
        public BIMPart GetBIMPart()
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
    }
    public enum PartType
    {
        no = 0,
        beam = 1,
        plate = 2,
        polyBeam = 3,
    }
}
