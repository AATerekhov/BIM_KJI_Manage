using TSM = Tekla.Structures.Model;
using System.Collections.Generic;
using System;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class BIMPruning
    {
        public List<TSM.BooleanPart> BooleanParts { get; set; }
        public List<TSM.CutPlane> CutPlanes { get; set; }
        public List<TSM.Fitting> Fittings { get; set; }
        public List<BIMBooleanPlate> BooleanPlates { get; set; }

        public BIMPruning() { }
        public BIMPruning(TSM.ModelObjectEnumerator modelObjectEnumerator)
        {
            BooleanParts = new List<TSM.BooleanPart>();
            CutPlanes = new List<TSM.CutPlane>();
            Fittings = new List<TSM.Fitting>();
            BooleanPlates = new List<BIMBooleanPlate>();
            foreach (var item in modelObjectEnumerator)
            {
                if (item is TSM.BooleanPart boolean)
                {
                    if (boolean.OperativePart is TSM.ContourPlate )
                    {
                        BooleanPlates.Add(new BIMBooleanPlate(boolean));
                        continue;
                    }
                    else
                    {
                        BooleanParts.Add(boolean);
                        continue;
                    }
                   
                }
                if (item is TSM.CutPlane cut)
                {
                    CutPlanes.Add(cut);
                    continue;
                }
                if (item is TSM.Fitting fit)
                {
                    Fittings.Add(fit);
                    continue;
                }
            }
        }

        public void Insert(TSM.Part part) 
        {
            if (BooleanParts.Count > 0) AddBooleanPart(part, BooleanParts);
            if (CutPlanes.Count > 0) AddBooleanCut(part, CutPlanes);
            if (Fittings.Count > 0) AddBooleanPlane(part, Fittings);
            if (BooleanPlates.Count > 0) AddBooleanPlate(part, BooleanPlates);
        }

        private void AddBooleanPart(TSM.Part part, List<TSM.BooleanPart> booleans)
        {
            foreach (var item in booleans)
            {
                var operativePart = item.OperativePart;
                operativePart.Insert();
                operativePart.Class = TSM.BooleanPart.BooleanOperativeClassName;
                TSM.BooleanPart boolean = new TSM.BooleanPart() { Type = item.Type, Father = part };
                boolean.SetOperativePart(operativePart);
                if (boolean.Insert()) operativePart.Delete();
            }
        }
        private void AddBooleanPlane(TSM.Part part, List<TSM.Fitting> fittings)
        {
            foreach (var item in fittings)
            {
                item.Father = part;
                item.Insert();
            }
        }
        private void AddBooleanCut(TSM.Part part, List<TSM.CutPlane> fittings)
        {
            foreach (var item in fittings)
            {
                item.Father = part;
                item.Insert();
            }
        }

        private void AddBooleanPlate(TSM.Part part, List<BIMBooleanPlate> fittings)
        {
            foreach (var item in fittings)
            {
                var operativePart = item.Plate.GetContourPlate();
                operativePart.Insert();
                operativePart.Class = TSM.BooleanPart.BooleanOperativeClassName;
                TSM.BooleanPart boolean = new TSM.BooleanPart() { Type = item.Boolean.Type, Father = part };
                boolean.SetOperativePart(operativePart);
                if (boolean.Insert()) operativePart.Delete();               
            }
        }
    }
}
