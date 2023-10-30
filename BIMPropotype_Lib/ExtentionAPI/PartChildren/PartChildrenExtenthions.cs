using BIMPropotype_Lib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;
using Tekla.Structures.Geometry3d;
using BIMPropotype_Lib.Controller;

namespace BIMPropotype_Lib.ExtentionAPI.PartChildren
{
    public static class PartChildrenExtenthions
    {
        /// <summary>
        /// Проверка детали на гравность.
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        public static bool CheckMainPart(this Part part) 
        {
            int main = 0;
            part.GetReportProperty("MAIN_PART", ref main);
            if (main != 0) return true;
            else return false;
        }

        public static List<BIMPartChildren> GetAssembly(this List<BIMPartChildren> children) 
        {
            List<BIMPartChildren> assembly = new List<BIMPartChildren>();
            foreach (var child in children) 
            {
                if (child.ChildrenType == PartChildrenType.Assembly) 
                {
                    assembly.Add(child);
                }
            }
            return assembly;
        }
        public static List<BIMPartChildren> GetBoolean(this List<BIMPartChildren> children)
        {
            List<BIMPartChildren> boolean = new List<BIMPartChildren>();
            foreach (var child in children)
            {
                if (child.ChildrenType == PartChildrenType.Fitting)
                {
                    boolean.Add(child);
                }
                else if (child.ChildrenType == PartChildrenType.CutPlane)
                {
                    boolean.Add(child);
                }
                else if (child.ChildrenType == PartChildrenType.BooleanBeam)
                {
                    boolean.Add(child);
                }
                else if (child.ChildrenType == PartChildrenType.BooleanPlate)
                {
                    boolean.Add(child);
                }
            }
            return boolean;
        }
        public static List<BIMPartChildren> GetReinforcement(this List<BIMPartChildren> children)
        {
            List<BIMPartChildren> reinforcement = new List<BIMPartChildren>();
            foreach (var child in children)
            {
                if (child.ChildrenType == PartChildrenType.singleRebar)
                {
                    reinforcement.Add(child);
                }
                else if (child.ChildrenType == PartChildrenType.groupRebar)
                {
                    reinforcement.Add(child);
                }
            }
            return reinforcement;
        }
        public static List<BIMPartChildren> GetBolts(this List<BIMPartChildren> children)
        {
            List<BIMPartChildren> bolts = new List<BIMPartChildren>();
            foreach (var child in children)
            {
                if (child.ChildrenType == PartChildrenType.Bolt)
                {
                    bolts.Add(child);
                }
            }
            return bolts;
        }
        public static List<BIMPartChildren> GetJoints(this List<BIMPartChildren> children)
        {
            List<BIMPartChildren> joints = new List<BIMPartChildren>();
            foreach (var child in children)
            {
                if (child.ChildrenType == PartChildrenType.joint)
                {
                    joints.Add(child);
                }
            }
            return joints;
        }

        public static CoordinateSystem GetBaseStructure(this Beam beam) 
        {
            CoordinateSystem cs = beam.GetCoordinateSystem();
            cs.Origin = beam.StartPoint;
            return cs;
        }
        public static CoordinateSystem GetBaseStructure(this PolyBeam polyBeam)
        {
            CoordinateSystem cs = polyBeam.GetCoordinateSystem();
            cs.Origin = polyBeam.Contour.ContourPoints[0] as Point;
            return cs;
        }
        public static CoordinateSystem GetBaseStructure(this ContourPlate contourPlate)
        {
            CoordinateSystem cs = contourPlate.GetCoordinateSystem();
            cs.Origin = contourPlate.Contour.ContourPoints[0] as Point;
            return cs;
        }
        public static CoordinateSystem GetBaseStructure(this SingleRebar singleRebar)
        {
            CoordinateSystem cs = singleRebar.GetCoordinateSystem();
            cs.Origin = singleRebar.Polygon.Points[0] as Point;
            return cs;
        }
        public static CoordinateSystem GetBaseStructure(this RebarGroup rebarGroup)
        {
            CoordinateSystem cs = rebarGroup.GetCoordinateSystem();
            cs.Origin = (rebarGroup.Polygons[0] as Polygon).Points[0] as Point;
            return cs;
        }
        public static CoordinateSystem GetBaseStructure(this Assembly assembly)
        {
            CoordinateSystem cs = null;
            ModelObject mainPart = assembly.GetMainPart();
            if (mainPart is Beam beam)
            {
                cs = beam.GetBaseStructure().Cloned();
            }
            else if (mainPart is PolyBeam polyBeam)
            {
                cs = polyBeam.GetBaseStructure().Cloned();
            }
            else if (mainPart is ContourPlate contourPlate)
            {
                cs = contourPlate.GetBaseStructure().Cloned();
            }
            return cs;
        }
        public static CoordinateSystem Cloned(this CoordinateSystem cs)
        {
            CoordinateSystem cloneCS = new CoordinateSystem();
            cloneCS.Origin = new Point(cs.Origin);
            cloneCS.AxisX = cs.AxisX;
            cloneCS.AxisY = cs.AxisY;
            return cloneCS;
        }
        public static List<Part> GetPartJoint(this BIMPart bIMPart)
        {
            List<Part> parts = new List<Part>();
            var joints = bIMPart.Children.Where(p => p.ChildrenType == PartChildrenType.joint).ToList();

            foreach (var joint in joints) 
            {
                foreach (var item in joint.Joint.Children)
                {
                    parts.Add(item.GetPart());
                }
            }
            return parts;
        }
    }
}
