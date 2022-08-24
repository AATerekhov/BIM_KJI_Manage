using System;
using System.Collections.Generic;
using Tekla.Structures.Model;
using TSM = Tekla.Structures.Model;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class CustomPlate
    {
        public TSM.Profile Profile { get; set; }
        public TSM.Material Material { get; set; }
        //Деформация пропущена
        public TSM.NumberingSeries PartNumber { get; set; }
        public TSM.NumberingSeries AssemblyNumber { get; set; }

        public string Name { get; set; }
        public string nameClass { get; set; }
        public TSM.Position Position { get; set; }
        public List<TSM.ContourPoint> Contour { get; set; }
        public CustomPlate() { }
        public CustomPlate(TSM.ContourPlate contourPlate)
        {
            Profile = contourPlate.Profile;
            Material = contourPlate.Material;
            PartNumber = contourPlate.PartNumber;
            AssemblyNumber = contourPlate.AssemblyNumber;
            Name = contourPlate.Name;
            nameClass = contourPlate.Class;
            Position = contourPlate.Position;
            Contour = GetPoints(contourPlate.Contour) ;
        }

        public CustomPlate(Profile profile, Material material, NumberingSeries partNumber, NumberingSeries assemblyNumber, string name, string @class, Position position, Contour counter)
        {
           
        }

        public TSM.ContourPlate GetContourPlate() 
        {
            var plate = new TSM.ContourPlate() { Name = Name, Profile = Profile, Material = Material, Class = nameClass, Position = Position, PartNumber = PartNumber, AssemblyNumber = AssemblyNumber };
            foreach (var item in Contour)
            {
                plate.AddContourPoint(item);
            }
            return plate;
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
    }
}
