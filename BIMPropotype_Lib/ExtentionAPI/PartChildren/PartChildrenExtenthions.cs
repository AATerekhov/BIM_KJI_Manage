using BIMPropotype_Lib.Model;
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
        public static List<BIMPartChildren> GetPutInAssembly(this BIMBoxPart part) 
        {
            var partEnumChildren = part.GetPart().GetAssembly().GetSubAssemblies();

            var putInAssembly = new List<BIMPartChildren>();

            foreach (var assembly in partEnumChildren)
            {
                if (assembly is Assembly assemlyChild)
                {
                    var child = new BIMPartChildren(assemlyChild, PartChildrenType.BIMAssembly);
                    child.Father = part;
                    putInAssembly.Add(child);
                }
            }

            return putInAssembly;
        }
    }
}
