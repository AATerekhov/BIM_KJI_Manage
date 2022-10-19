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
        public SupportCountor SupportCountor { get; set; }

        public CustomPolyBeam() { }
        public CustomPolyBeam(TSM.PolyBeam polyBeam)
        {
            SupportCountor =new SupportCountor(Averaging(polyBeam));
            polyBeam.Contour.ContourPoints.Clear();
            PolyBeam = polyBeam;
        }
        public void GetPolyBeam()
        {            
            foreach (var item in SupportCountor.GetContourPoints())
            {
                PolyBeam.AddContourPoint(item);
            }
        }

        private List<TSM.ContourPoint> Averaging(TSM.PolyBeam polyBeam)
        {
            var listPoint = GetPoints(polyBeam.Contour);         

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
