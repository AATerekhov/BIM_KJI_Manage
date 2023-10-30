using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;
using BIMPropotype_Lib.Model;

namespace PrototypeStructure.Model
{
    public class Joint : IOrientation
    {
        public CoordinateSystem CS { get; set; }
    }
}
