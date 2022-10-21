using BIMPropotype_Lib.Model.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;
using TSM = Tekla.Structures.Model;

namespace BIMPropotype_Lib.Model.Custom
{
    [Serializable]
    public class CustomBeam: IFormObject
    {
        public TSM.Beam Beam { get; set; }
        public SupportDistanse Support { get; set; }
        public CustomBeam() { }
        public CustomBeam(TSM.Beam beam)
        {
            Support = new SupportDistanse(beam.StartPoint, beam.EndPoint);
            Beam = beam;
        }
        public void FormObject()
        {
            Beam.StartPoint = Support.Start;
            Beam.EndPoint = Support.End;            
        }

        public ModelObject GetModelObject()
        {
            FormObject();
            return Beam;
        }
    }
}
