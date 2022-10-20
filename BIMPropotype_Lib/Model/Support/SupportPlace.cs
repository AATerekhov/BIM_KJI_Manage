using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;
using TSM = Tekla.Structures.Model;

namespace BIMPropotype_Lib.Model.Support
{
    public class SupportPlace:SupportElement
    {
        public SupportPlace(TSM.Plane plane)
        {
            Base = new CoordinateSystem(plane.Origin, plane.AxisX, plane.AxisY);
            base.Add(plane.Origin);
            base.Add(plane.Origin + plane.AxisX);
            base.Add(plane.Origin + plane.AxisY);
        }
        public SupportPlace() { }

        public TSM.Plane GetPlane()
        {
            var plane = new TSM.Plane();
            plane.Origin = Base.Origin;
            plane.AxisX = Base.AxisX;
            plane.AxisY = Base.AxisY;
            return plane;        
        }
    }
}
