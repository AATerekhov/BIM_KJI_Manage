using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;

namespace BIMPropotype_Lib.Model.Support
{
    [Serializable]
    public class SupportCountor:SupportPolygon
    {
        public List<Chamfer> Chamfers { get; set; }
        public SupportCountor() { }
        public SupportCountor(List<ContourPoint> contour)            
        {
            foreach (var item in contour)
            {
                var point = new Point(item.X, item.Y, item.Z);
                Add(point);
            }

            Chamfers = new List<Chamfer>();

            foreach (var item in contour)
            {
                Chamfers.Add(item.Chamfer);
            }
        }

        public List<ContourPoint> GetContourPoints() 
        {
            List<ContourPoint> contourPoints = new List<ContourPoint>();
            for (int i = 0; i < Points.Count; i++)
            {
                contourPoints.Add(new ContourPoint(Points[i], Chamfers[i]));
            }
            return contourPoints;
        }

    }
}
