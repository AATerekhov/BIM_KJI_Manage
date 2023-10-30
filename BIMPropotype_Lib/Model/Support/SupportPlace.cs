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
            base.Add(plane.Origin);
            base.Add(plane.Origin + plane.AxisX);
            base.Add(plane.Origin + plane.AxisY);
        }
        public SupportPlace() { }

        public TSM.Plane GetPlane()
        {
            var plane = new TSM.Plane();
            plane.Origin = Points[0];
            plane.AxisX = new Vector(Points[1] - Points[0]); 
            plane.AxisY = new Vector(Points[2] - Points[0]);
            return plane;        
        }

        public CoordinateSystem GetCS()
        {
            return new CoordinateSystem(GetFirst(), new Vector(GetSecond() - GetFirst()), new Vector(Points[2] - GetFirst()));
        }
    }
}
