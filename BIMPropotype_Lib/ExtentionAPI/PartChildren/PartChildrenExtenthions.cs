﻿using BIMPropotype_Lib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;

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
        public static List<BIMPartChildren> GetChildren(this BIMPart part) 
        {
            var children = new List<BIMPartChildren>();
            var modelPart = part.GetPart();

            if (modelPart.CheckMainPart())
            {
               var partEnumChildren = modelPart.GetAssembly().GetSubAssemblies();
                foreach (var assembly in partEnumChildren)
                {
                    if (assembly is Assembly assemlyChild)
                    {
                        var child = new BIMPartChildren(assemlyChild);
                        child.Father = part;
                        children.Add(child);
                    }
                }
            }
           
            foreach (ModelObject item in modelPart.GetReinforcements())
            {
                var child = new BIMPartChildren(item);
                child.Father = part;
                children.Add(child);                
            }

            foreach (ModelObject item in modelPart.GetBolts())
            {
                var child = new BIMPartChildren(item);
                child.Father = part;
                children.Add(child);
            }

            foreach (ModelObject item in modelPart.GetBooleans())
            {
                var child = new BIMPartChildren(item);
                child.Father = part;
                children.Add(child);
            }
            return children;
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
    }
}
