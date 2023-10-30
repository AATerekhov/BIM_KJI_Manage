using System;
using System.Collections.Generic;
using Tekla.Structures.Model;
using TSM = Tekla.Structures.Model;
using TSG = Tekla.Structures.Geometry3d;
using BIMPropotype_Lib.Model.Support;
using Tekla.Structures.Geometry3d;

namespace BIMPropotype_Lib.Model.Custom
{
    [Serializable]
    public class CustomPlate : IFormObject
    {
        public ContourPlate ContourPlate { get; set; }
        public SupportCountor Support { get; set; }
        public CustomPlate() { }
        public CustomPlate(TSM.ContourPlate contourPlate)
        {
            Support = new SupportCountor(Averaging(contourPlate));
            ContourPlate = contourPlate;
            this.Сleaning();
        }

        public void FormObject() 
        {
            ContourPlate.Contour = new Contour();
            foreach (var item in Support.GetContourPoints())
            {
                ContourPlate.AddContourPoint(item);
            }
        }

        private List<ContourPoint> GetPoints(TSM.Contour contour) 
        {
            var list = new List<ContourPoint>();

            foreach (var contourPoint in contour.ContourPoints) 
            {
                if (contourPoint is TSM.ContourPoint point) list.Add(point);
            }
            return list;
        }

        //Процесс приведения возможно вынести в Интрефейс как функцию.
        private List<TSM.ContourPoint> Averaging(TSM.ContourPlate contourPlate) 
        {            
          var listPoint = GetPoints(contourPlate.Contour);
            //TODO:Рассмотреть возможность адоптации отцентровки пластин под новый алгоритм вставки.
            //Определение направления толщины.
            //TSG.Vector vectorX = new TSG.Vector(listPoint[1]- listPoint[0]);
            //TSG.Vector vectorY = new TSG.Vector(listPoint[listPoint.Count-1] - listPoint[0]);
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
            //    vectorZ.Normalize(contourPlate.Position.DepthOffset + wightPlate/2);
            //    contourPlate.Position.Depth = Position.DepthEnum.MIDDLE;
            //    return PointShift(listPoint, vectorZ);
            //}
            //if (contourPlate.Position.Depth == Position.DepthEnum.BEHIND)
            //{
            //    vectorZ.Normalize(contourPlate.Position.DepthOffset + wightPlate / 2);
            //    contourPlate.Position.Depth = Position.DepthEnum.MIDDLE;
            //    return PointShift(listPoint, -1*vectorZ);
            //}

            //Закоменченный старый алгоритм перемещения по толщине.
            
            return listPoint;
        }
        private List<TSM.ContourPoint> PointShift(List<TSM.ContourPoint> listPoint, TSG.Vector vectorZ) 
        {
            for (int i = 0; i < listPoint.Count; i++)
            {
                listPoint[i].X = listPoint[i].X + vectorZ.X;
                listPoint[i].Y = listPoint[i].Y + vectorZ.Y;
                listPoint[i].Z = listPoint[i].Z + vectorZ.Z;
            }
            return listPoint;
        }
        private double GetWightFromProfile(string profile) 
        {
            string value = string.Empty;
            double rezult = 0.0;
            foreach (var item in Enum.GetNames(typeof(PlateProfileType)))
            {
                if (item == "ALTCODE")
                {
                    if (profile.StartsWith("—")) { value = profile.Substring(1); break; }
                }
                else if (profile.StartsWith(item)) { value = profile.Substring(item.Length); break; }
            }

            if (value == string.Empty) value = profile;

            if (double.TryParse(value, out rezult)) { return rezult; }
            else
            {
                return 0.0;
            }
           
        }

        public ModelObject GetModelObject()
        {
            FormObject();
            return ContourPlate;
        }

        public void Сleaning()
        {
            ContourPlate.Contour = null;
        }
    }
    enum PlateProfileType 
    {
        BL=1,
        BPL=2,
        FB=3,
        FL=4,
        FLT=5,
        FPL,
        PL=7,
        PLATE=8,
        PLT=9,
        TANKO= 10,
        ALTCODE=11,
        ПВ=12,
        Полоса=13,
        Риф=14,
        ЧРиф=15,
    }
}
