using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class CustomPolygon
    {
        public List<Point> PolygonPoints { get; set; }

        public CustomPolygon() { }
        public CustomPolygon(ArrayList arrayList)
        {
            PolygonPoints = new List<Point>();
            foreach (var item in arrayList)
            {
                if (item is Point point) PolygonPoints.Add(point);
            }
        }

        private ArrayList GetArrayList() 
        {
            ArrayList arrayList = new ArrayList();
            foreach (var point in PolygonPoints) 
            {
                arrayList.Add(point);
            }           
            return arrayList;
        }

        internal Polygon GetPolygon() 
        {
            var polygon = new Polygon();
            polygon.Points = GetArrayList();
            return polygon;
        }
    }
}
