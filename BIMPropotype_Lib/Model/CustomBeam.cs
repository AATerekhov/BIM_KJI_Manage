using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class CustomBeam
    {
        public TSM.Beam Beam { get; set; }
        public SupportDistanse SupportDistanse { get; set; }
        public CustomBeam() { }
        public CustomBeam(TSM.Beam beam)
        {
            SupportDistanse = new SupportDistanse(beam.StartPoint, beam.EndPoint);
            Beam = beam;
        }
        public void GetBeam()
        {
            Beam.StartPoint = SupportDistanse.Start;
            Beam.EndPoint = SupportDistanse.End;            
        }

    }
}
