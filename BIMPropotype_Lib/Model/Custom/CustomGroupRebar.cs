using BIMPropotype_Lib.Model.Support;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;

namespace BIMPropotype_Lib.Model.Custom
{
    public class CustomGroupRebar : IFormObject
    {
        public TSM.RebarGroup RebarGroup { get; set; }
        public List<SupportPolygon> Support { get; set; }
        public double[] OnPlaneOffsets { get; set; }
        public double[] RadiusValues { get; set; }
        public double[] Spacings { get; set; }
        public CustomGroupRebar() { }

        public CustomGroupRebar(TSM.RebarGroup rebarGroup)
        {
            Support = new List<SupportPolygon>(from item in GetPolygons(rebarGroup.Polygons) select new SupportPolygon(item.Points));
            rebarGroup.Polygons = null;
            OnPlaneOffsets = ContertArreyListToDouble(rebarGroup.OnPlaneOffsets);
            rebarGroup.OnPlaneOffsets = null;
            RadiusValues = ContertArreyListToDouble(rebarGroup.RadiusValues);
            rebarGroup.RadiusValues = null;
            Spacings = ContertArreyListToDouble(rebarGroup.Spacings);
            rebarGroup.Spacings = null;
            RebarGroup = rebarGroup;
        }

        public void FormObject()
        {
            RebarGroup.OnPlaneOffsets = ContertDoubleToArreyList(OnPlaneOffsets);
            RebarGroup.RadiusValues = ContertDoubleToArreyList(RadiusValues);
            RebarGroup.Spacings = ContertDoubleToArreyList(Spacings);
            RebarGroup.Polygons = GetPolygons();
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
            foreach (var item in Support)
            {
                arrayList.Add(item.GetPolygon());
            }
            return arrayList;
        }

        public TSM.ModelObject GetModelObject()
        {
            FormObject();
            return RebarGroup;
        }
    }
}
