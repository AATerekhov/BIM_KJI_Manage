using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;
using TSG = Tekla.Structures.Geometry3d;


namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class CustomPolyBeam
    {
        public TSM.PolyBeam PolyBeam { get; set; }
        public List<TSM.ContourPoint> Contour { get; set; }

        public CustomPolyBeam() { }
        public CustomPolyBeam(TSM.PolyBeam polyBeam)
        {
            Contour = Averaging(polyBeam);
            polyBeam.Contour.ContourPoints.Clear();
            PolyBeam = polyBeam;
        }
        public void GetPolyBeam()
        {            
            foreach (var item in Contour)
            {
                PolyBeam.AddContourPoint(item);
            }
        }

        private List<TSM.ContourPoint> Averaging(TSM.PolyBeam polyBeam)
        {
            var listPoint = GetPoints(polyBeam.Contour);
            //TSG.Vector vectorX = new TSG.Vector(listPoint[1] - listPoint[0]);
            //TSG.Vector vectorY = new TSG.Vector(listPoint[listPoint.Count - 1] - listPoint[0]);

            //var vectorZ = vectorX.Cross(vectorY);

            //if (contourPlate.Position.Depth == Position.DepthEnum.MIDDLE)
            //{
            //    if (contourPlate.Position.DepthOffset == 0) { return listPoint; }
            //    else
            //    {
            //        vectorZ.Normalize(contourPlate.Position.DepthOffset);
            //        return PointShift(listPoint, vectorZ);
            //    }
            //}

            //double wightPlate = GetWightFromProfile(contourPlate.Profile.ProfileString);

            //if (contourPlate.Position.Depth == Position.DepthEnum.FRONT)
            //{
            //    vectorZ.Normalize(contourPlate.Position.DepthOffset + wightPlate / 2);
            //    return PointShift(listPoint, vectorZ);
            //}
            //if (contourPlate.Position.Depth == Position.DepthEnum.BEHIND)
            //{
            //    vectorZ.Normalize(contourPlate.Position.DepthOffset + wightPlate / 2);
            //    return PointShift(listPoint, -1 * vectorZ);
            //}

            return listPoint;
        }

        private List<TSM.ContourPoint> GetPoints(TSM.Contour contour)
        {
            var list = new List<TSM.ContourPoint>();

            foreach (var contourPoint in contour.ContourPoints)
            {
                if (contourPoint is TSM.ContourPoint point) list.Add(point);
            }
            return list;
        }
    }
}
