using System;
using Tekla.Structures.Model;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class BIMBeam:BIMPart
    {
        public BIMBeam()
            :base()
        {

        }
        public BIMBeam(Beam beam )
           :base(beam)
        {

        }
    }
}
