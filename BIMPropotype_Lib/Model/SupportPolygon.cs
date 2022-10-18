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
    public class SupportPolygon : SupportElement
    {
        public SupportPolygon() { }
        public SupportPolygon(ArrayList arrayList)
        {
            foreach (var item in arrayList)
            {
                if (item is Point point) base.Add(point);
            }
        }
        public SupportPolygon(List<Point> splash)
        {
            foreach (var item in splash)
            {
                if (item is Point point) base.Add(point);
            }
        }

        private ArrayList GetArrayList() 
        {
            ArrayList arrayList = new ArrayList();
            foreach (var point in Points) 
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
