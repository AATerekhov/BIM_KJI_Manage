using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;
using TSG = Tekla.Structures.Geometry3d;

namespace BIMPropotype_Lib.ExtentionAPI.Mirror
{
    public static class PartExtentions
    {
        public static void InsertMirror(this Part part, bool swap) 
        {
            var CS = new TSG.CoordinateSystem();
            var plane = new TSG.GeometricPlane(CS.Origin, CS.AxisX);

            if (part is Beam beam)
            {
                var startPoint = beam.StartPoint;
                var endPoint = beam.EndPoint;
                if (swap)
                {
                    beam.StartPoint = MirrorPoint(plane, endPoint);
                    beam.EndPoint = MirrorPoint(plane, startPoint);                   
                }
                else
                {
                    beam.StartPoint = MirrorPoint(plane, startPoint);
                    beam.EndPoint = MirrorPoint(plane, endPoint);
                }
                beam.Insert();
            }
            else if (part is ContourPlate plate)
            {
                var points = plate.Contour.ContourPoints;
                List<ContourPoint> contourPoints = new List<ContourPoint>();
                foreach (var point in points)
                {
                    if (point is ContourPoint contourPoint)
                    {
                        contourPoint.SetPoint(MirrorPoint(plane, contourPoint));
                        contourPoints.Add(contourPoint);
                    }
                }

                if (swap) contourPoints.Reverse();

                plate.Contour.ContourPoints.Clear();
                foreach (var point in contourPoints)
                {
                    plate.Contour.AddContourPoint(point);
                }
                plate.Insert();
            }
        }

        private static TSG.Point MirrorPoint(TSG.GeometricPlane plane, TSG.Point point)
        {
            var mirrorPoint = TSG.Projection.PointToPlane(point, plane);
            var distance = TSG.Distance.PointToPoint(point, mirrorPoint);
            var vector = -1 * plane.Normal;
            vector.Normalize(2 * distance);
            return point + vector;
        }
    }
}
