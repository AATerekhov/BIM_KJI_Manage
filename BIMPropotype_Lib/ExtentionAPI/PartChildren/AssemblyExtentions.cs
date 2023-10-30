using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIMPropotype_Lib.Model;
using BIMPropotype_Lib.ViewModel;
using TSM = Tekla.Structures.Model;
using System.Xml.Serialization;

namespace BIMPropotype_Lib.ExtentionAPI.PartChildren
{
    public static class AssemblyExtentions
    {
        public static double CheckDistance(this BIMAssembly bIMAssembly)
        {
            double outcome = 0.0;
            if (!bIMAssembly.IsLink)
            {
                var endJoint = bIMAssembly.GetEndJoint();
                if (endJoint != null)
                {
                    outcome = endJoint.BaseStructure.Origin.X;
                }

            }          
            
            return outcome;
        }

        public static void ModifyDistance(this BIMAssembly bIMAssembly, double newDistance) 
        {
            bIMAssembly.Children[0].Beam.Support.Points[1].X = newDistance;
            var endJoint = bIMAssembly.GetEndJoint();
            endJoint.BaseStructure.Origin.X = newDistance;
            endJoint.Joint.BaseStructure.Origin.X = newDistance;
        }

        public static BIMPartChildren GetEndJoint(this BIMAssembly bIMAssembly)
        {
           return bIMAssembly.Children[0].Children.Where(p => p.ChildrenType == PartChildrenType.joint && p.Joint.Meta.Number == 2).FirstOrDefault(); 
        }


    }
}
