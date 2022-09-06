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
        /// <summary>
        /// Вставить деталь в модель зеркально.
        /// </summary>
        /// <param name="part"></param>
        /// <param name="swap">Сохранить систему координат детали</param>
        public static void InsertMirror(this Part part, bool swap)
        {
            var CS = new TSG.CoordinateSystem();
            var plane = new TSG.GeometricPlane(CS.Origin, CS.AxisX);

            if (part is Beam beam)
            {
                beam.Profile.ProfileMirrod();
                var startPoint = beam.StartPoint;
                var endPoint = beam.EndPoint;

                if (!TSG.Parallel.LineToPlane(new TSG.Line(startPoint, endPoint), plane))
                {
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
                }
                else
                {
                    beam.StartPoint = MirrorPoint(plane, endPoint);
                    beam.EndPoint = MirrorPoint(plane, startPoint);
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
            else if (part is PolyBeam polyBeam)
            {
                var points = polyBeam.Contour.ContourPoints;
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


                polyBeam.Contour.ContourPoints.Clear();
                foreach (var point in contourPoints)
                {
                    polyBeam.Contour.AddContourPoint(point);
                }

                if (polyBeam.Position.Depth == Position.DepthEnum.BEHIND) polyBeam.Position.Depth = Position.DepthEnum.FRONT;
                if (polyBeam.Position.Depth == Position.DepthEnum.FRONT) polyBeam.Position.Depth = Position.DepthEnum.BEHIND;
                if (polyBeam.Position.Plane == Position.PlaneEnum.LEFT) polyBeam.Position.Plane = Position.PlaneEnum.RIGHT;
                if (polyBeam.Position.Plane == Position.PlaneEnum.RIGHT) polyBeam.Position.Plane = Position.PlaneEnum.LEFT;
                polyBeam.Insert();
            }
        }

        public static void InsertMirror(this CutPlane cutPlane)
        {
            var CS = new TSG.CoordinateSystem();
            var gPlane = new TSG.GeometricPlane(CS.Origin, CS.AxisX);

            var plane = cutPlane.Plane;

            var pointOrigin = plane.Origin;
            var pointX = plane.Origin + plane.AxisX;
            var pointY = plane.Origin + plane.AxisY;

            cutPlane.Plane.Origin = MirrorPoint(gPlane, pointOrigin);
            cutPlane.Plane.AxisX = -1 * new TSG.Vector(MirrorPoint(gPlane, pointX) - cutPlane.Plane.Origin);
            cutPlane.Plane.AxisY = new TSG.Vector(MirrorPoint(gPlane, pointY) - cutPlane.Plane.Origin);
            cutPlane.Insert();
        }
        public static void InsertMirror(this Fitting fitting)
        {
            var CS = new TSG.CoordinateSystem();
            var gPlane = new TSG.GeometricPlane(CS.Origin, CS.AxisX);

            var plane = fitting.Plane;

            var pointOrigin = plane.Origin;
            var pointX = plane.Origin + plane.AxisX;
            var pointY = plane.Origin + plane.AxisY;

            fitting.Plane.Origin = MirrorPoint(gPlane, pointOrigin);
            fitting.Plane.AxisX = -1 * new TSG.Vector(MirrorPoint(gPlane, pointX) - fitting.Plane.Origin);
            fitting.Plane.AxisY = new TSG.Vector(MirrorPoint(gPlane, pointY) - fitting.Plane.Origin);
            fitting.Insert();
        }
        public static void InsertMirror(this SingleRebar singleRebar)
        {
            var CS = new TSG.CoordinateSystem();
            var gPlane = new TSG.GeometricPlane(CS.Origin, CS.AxisX);

            List<TSG.Point> points = new List<TSG.Point>();
            foreach (var item in singleRebar.Polygon.Points)
            {
                if (item is TSG.Point point)
                    points.Add(MirrorPoint(gPlane, point));
            }
            singleRebar.Polygon.Points.Clear();
            foreach (var point in points)
            {
                singleRebar.Polygon.Points.Add(point);
            }

            singleRebar.Insert();
        }
        public static void InsertMirror(this RebarGroup rebarGroup)
        {
            var CS = new TSG.CoordinateSystem();
            var gPlane = new TSG.GeometricPlane(CS.Origin, CS.AxisX);

            rebarGroup.StartPoint = MirrorPoint(gPlane, rebarGroup.StartPoint);
            rebarGroup.EndPoint = MirrorPoint(gPlane, rebarGroup.EndPoint);

            foreach (var itemGroup in rebarGroup.Polygons)
            {
                if (itemGroup is Polygon poligon)
                {
                    List<TSG.Point> points = new List<TSG.Point>();
                    foreach (var item in poligon.Points)
                    {
                        if (item is TSG.Point point)
                            points.Add(MirrorPoint(gPlane, point));
                    }
                    poligon.Points.Clear();
                    foreach (var point in points)
                    {
                        poligon.Points.Add(point);
                    }
                }

            }


            rebarGroup.Insert();
        }


        private static TSG.Point MirrorPoint(TSG.GeometricPlane plane, TSG.Point point)
        {
            var mirrorPoint = TSG.Projection.PointToPlane(point, plane);
            var distance = TSG.Distance.PointToPoint(point, mirrorPoint);
            var vector = new TSG.Vector(mirrorPoint - point);
            vector.Normalize(2 * distance);
            return point + vector;
        }

        private static void ProfileMirrod(this Profile profile)
        {
            string value = string.Empty;
            string key = string.Empty;

            foreach (var item in Enum.GetNames(typeof(ProfileMirrod)))
            {
                if (profile.ProfileString.StartsWith(item))
                {
                    key = item;
                    value = profile.ProfileString.Substring(item.Length); 
                    break;
                }
            }

            if (!string.IsNullOrEmpty(key)) 
            {
                if (key == "PRMD")
                {
                    var vs = value.Split('-');
                    profile.ProfileString = $"{key}{vs[1]}-{vs[0]}";
                }
            }
        }

    }
    public enum ProfileMirrod
    {
        PRMD
    }
}
