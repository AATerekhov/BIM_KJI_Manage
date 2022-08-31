using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;
using Tekla.Structures.Geometry3d;

namespace Propotype_Manage.Controller
{
    internal class WorkPlaneWorker
    {
        public TSM.Model Model { get; set; }
        public TSM.Assembly WorkerAssembly { get; set; }
        public TSM.WorkPlaneHandler WorkPlaneHandler { get; set; }
        public TSM.TransformationPlane OldTPlane { get; set; }
        public int MyProperty { get; set; }

        internal AssemblyType AssemblyType { get; set; }
        public WorkPlaneWorker(TSM.Model model, TSM.Assembly assembly)
        {
            Model = model;
            WorkPlaneHandler = Model.GetWorkPlaneHandler();            
            OldTPlane = WorkPlaneHandler.GetCurrentTransformationPlane();
            WorkerAssembly = assembly;
        }

        internal void GetWorkPlace() 
        {
            WorkPlaneHandler.SetCurrentTransformationPlane(new TSM.TransformationPlane(GetCoordinateSystem()));
            Model.CommitChanges();
        }

        internal void RerutnWorkPlace() 
        {
            WorkPlaneHandler.SetCurrentTransformationPlane(OldTPlane);
            Model.CommitChanges();
        }

        private CoordinateSystem GetCoordinateSystem() 
        {
            var baseCS = new CoordinateSystem();
            var matrixToLocal = OldTPlane.TransformationMatrixToLocal;
            
            var pointX = matrixToLocal * baseCS.Origin + baseCS.AxisX;
            var pointY = matrixToLocal * baseCS.Origin + baseCS.AxisY;

            baseCS.Origin = matrixToLocal * baseCS.Origin;
            
            baseCS.AxisX = new Vector(pointX - baseCS.Origin);
            baseCS.AxisY = new Vector(pointY - baseCS.Origin);
            var vecterZ = -1*baseCS.AxisX.Cross(baseCS.AxisY);

            var mainPart = WorkerAssembly.GetMainPart();
            var partCS = mainPart.GetCoordinateSystem();
            CheckType(partCS.AxisX, vecterZ);

            //var vectorY = partCS.AxisX.Cross(new Vector(0, 0, -1));
            CoordinateSystem newCS = null;
            if (AssemblyType == AssemblyType.beam) 
            {
                var vectorY = partCS.AxisX.Cross(vecterZ);
                newCS = new CoordinateSystem(GetStartedPoint(mainPart), partCS.AxisX, vectorY);
            }
            else
            {
                newCS = new CoordinateSystem(GetStartedPoint(mainPart), partCS.AxisX, partCS.AxisY);
            }
            return newCS;
        }

        private void CheckType(Vector mainVectorX, Vector vectorZ) 
        {
           var angle = mainVectorX.GetAngleBetween(vectorZ);
            if (angle > 3.138 && angle < 3.145) AssemblyType = AssemblyType.column;
            else
            {
                AssemblyType = AssemblyType.beam;
            }
        }

        private Point GetStartedPoint(TSM.ModelObject part) 
        {
            if(part is TSM.Beam beam) return beam.StartPoint;
            if (part is TSM.ContourPlate plate)
            {
                TSM.Polygon polygon = null;
                plate.Contour.CalculatePolygon(out polygon);

                if (polygon.Points[0] is Point point) return point;
            }

            return null;
        }
    }

    internal enum AssemblyType 
    {
        beam,
        column,
    }
}
