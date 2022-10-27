using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;
using BIMPropotype_Lib.Model.Support;
using Tekla.Structures.Model;
using Tekla.Structures.Geometry3d;

namespace BIMPropotype_Lib.Model.Custom
{
    [Serializable]
    public class CustomSingleRebar : IFormObject
    {
        public TSM.SingleRebar SingleRebar { get; set; }
        public SupportPolygon Support { get; set; }
        public CustomSingleRebar() { }
        public CustomSingleRebar(SingleRebar singleRebar)
        {
            SingleRebar = singleRebar;
            Support = new SupportPolygon(singleRebar.Polygon.Points);
            SingleRebar.Polygon.Points.Clear();
        }


        public void FormObject()
        {
            SingleRebar.Polygon.Points.Clear();
            SingleRebar.Polygon.Points.AddRange(Support.Points);
        }

        public ModelObject GetModelObject( )
        {
            FormObject();
            return SingleRebar;
        }
    }
}
