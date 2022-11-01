using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;
using TSG = Tekla.Structures.Geometry3d;
using BIMPropotype_Lib.Model.Support;
using Tekla.Structures.Geometry3d;

namespace BIMPropotype_Lib.Model.Custom
{
    [Serializable]
    public class CustomPolyBeam: IFormObject
    {
        public TSM.PolyBeam PolyBeam { get; set; }
        public SupportCountor Support { get; set; }

        public CustomPolyBeam() { }
        public CustomPolyBeam(TSM.PolyBeam polyBeam)
        {
            PolyBeam = polyBeam;
            Support = new SupportCountor(Averaging(polyBeam));
            this.Сleaning();
        }
        public void FormObject()
        {            
            foreach (var item in Support.GetContourPoints())
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

        public TSM.ModelObject GetModelObject( )
        {
            FormObject();
            return PolyBeam;
        }
        public void Сleaning()
        {
            PolyBeam.Contour.ContourPoints.Clear();
        }
    }
}
