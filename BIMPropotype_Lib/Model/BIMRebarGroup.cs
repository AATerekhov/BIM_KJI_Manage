using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;
using Tekla.Structures.Geometry3d;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class BIMRebarGroup
    {

        public TSM.RebarGroup InRebarGroup { get; set; }
        public List<CustomPolygon> CustomPolygon { get; set; }
        public double[] OnPlaneOffsets { get; set; }
        public double[] RadiusValues { get; set; }
        public double[] Spacings { get; set; }
        public Point StarPoint { get; set; }
        public Point EndPoint { get; set; }
        public BIMRebarGroup() { }
        public BIMRebarGroup(TSM.RebarGroup rebarGroup)
        {
            CustomPolygon = new List<CustomPolygon>(from item in GetPolygons(rebarGroup.Polygons) select new CustomPolygon(item.Points));
            rebarGroup.Polygons = null;
            OnPlaneOffsets = ContertArreyListToDouble(rebarGroup.OnPlaneOffsets);
            rebarGroup.OnPlaneOffsets = null;
            RadiusValues = ContertArreyListToDouble(rebarGroup.RadiusValues);
            rebarGroup.RadiusValues = null;
            Spacings = ContertArreyListToDouble(rebarGroup.Spacings);
            rebarGroup.Spacings = null;

            InRebarGroup = rebarGroup;
        }

        private List<TSM.Polygon> GetPolygons(ArrayList arrayList)
        {
            var polygons = new List<TSM.Polygon>();
            foreach (var item in arrayList)
            {
                if (item is TSM.Polygon polygon) polygons.Add(polygon);
            }
            return polygons;
        }
        private double[] ContertArreyListToDouble(ArrayList arrayList) 
        {
            double[] vs = new double[arrayList.Count];
            for (int i = 0; i < vs.Length; i++)
            {
                if (arrayList[i] is double distance) vs[i] = distance;
            }
            return vs;
        }
        private ArrayList ContertDoubleToArreyList(double[] vs)
        {
          ArrayList arrayList = new ArrayList();
            for (int i = 0; i < vs.Length; i++)
            {
                arrayList.Add(vs[i]);
            }
            return arrayList;
        }

        private ArrayList GetPolygons() 
        {
            ArrayList arrayList = new ArrayList();
            foreach (var item in CustomPolygon)
            {
                arrayList.Add(item.GetPolygon());
            }
            return arrayList;
        }

        public TSM.RebarGroup GetRebarGroup()
        {
            InRebarGroup.OnPlaneOffsets = ContertDoubleToArreyList(OnPlaneOffsets);
            InRebarGroup.RadiusValues = ContertDoubleToArreyList(RadiusValues);
            InRebarGroup.Spacings = ContertDoubleToArreyList(Spacings);
            InRebarGroup.Polygons = GetPolygons();
            return InRebarGroup;
        }

    }
}
