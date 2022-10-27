using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;
using Tekla.Structures.Geometry3d;

namespace BIMPropotype_Lib.Controller
{
    public class WorkPlaneWorker
    {
        public TSM.Model Model { get; set; }
        public TSM.WorkPlaneHandler WorkPlaneHandler { get; set; }
        public TSM.TransformationPlane OldTPlane { get; set; }

        //internal AssemblyType AssemblyType { get; set; }
        public WorkPlaneWorker(TSM.Model model)
        {
            Model = model;
            WorkPlaneHandler = Model.GetWorkPlaneHandler();            
            OldTPlane = WorkPlaneHandler.GetCurrentTransformationPlane();
        }

        public void GetWorkPlace(CoordinateSystem coordinateSystem)
        {
            WorkPlaneHandler.SetCurrentTransformationPlane(new TSM.TransformationPlane(coordinateSystem));
            //Model.CommitChanges();
        }

        public void ReturnWorkPlace()
        {
            WorkPlaneHandler.SetCurrentTransformationPlane(OldTPlane);
            //Model.CommitChanges();
        }

        /// <summary>
        /// Метод установки рабочей плоскости в начальную точку детали.
        /// </summary>
        /// <returns></returns>
        //private CoordinateSystem GetCoordinateSystem() 
        //{
        //var baseCS = new CoordinateSystem();
        //var matrixToLocal = OldTPlane.TransformationMatrixToLocal;

        //var pointX = matrixToLocal * baseCS.Origin + baseCS.AxisX;
        //var pointY = matrixToLocal * baseCS.Origin + baseCS.AxisY;

        //baseCS.Origin = matrixToLocal * baseCS.Origin;

        //baseCS.AxisX = new Vector(pointX - baseCS.Origin);
        //baseCS.AxisY = new Vector(pointY - baseCS.Origin);
        //var vecterZ = -1*baseCS.AxisX.Cross(baseCS.AxisY);
        //vecterZ.Normalize(1000);

        //var mainPart = WorkerAssembly.GetMainPart();
        //var partCS = mainPart.GetCoordinateSystem();
        //CheckType(partCS.AxisX, vecterZ);

        ////var vectorY = partCS.AxisX.Cross(new Vector(0, 0, -1));
        //CoordinateSystem newCS = null;
        //if (AssemblyType == AssemblyType.beam) 
        //{
        //    var vectorY = partCS.AxisX.Cross(vecterZ);
        //    vectorY.Normalize(1000);
        //    newCS = new CoordinateSystem(GetStartedPoint(mainPart), partCS.AxisX, vectorY);
        //}
        //else
        //{
        //    newCS = new CoordinateSystem(GetStartedPoint(mainPart), partCS.AxisX, partCS.AxisY);
        //}
        //return newCS;
        //}

        //private void CheckType(Vector mainVectorX, Vector vectorZ) 
        //{
        //   var angle = mainVectorX.GetAngleBetween(vectorZ);
        //    if (angle > 3.138 && angle < 3.145) AssemblyType = AssemblyType.column;
        //    else
        //    {
        //        AssemblyType = AssemblyType.beam;
        //    }
        //}

        /// <summary>
        /// Получение стартовой точки.
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        //private Point GetStartedPoint(TSM.ModelObject part) 
        //{
        //    if(part is TSM.Beam beam) return beam.StartPoint;
        //    else if (part is TSM.ContourPlate plate)
        //    {
        //        TSM.Polygon polygon = null;
        //        plate.Contour.CalculatePolygon(out polygon);

        //        if (polygon.Points[0] is Point point) return point;
        //    }
        //    else if (part is TSM.PolyBeam polyBeam)
        //    {
        //        TSM.Polygon polygon = null;
        //        polyBeam.Contour.CalculatePolygon(out polygon);

        //        if (polygon.Points[0] is Point point) return point;
        //    }

        //    return null;
        //}
    }

    //internal enum AssemblyType 
    //{
    //    beam,
    //    column,
    //}
}
