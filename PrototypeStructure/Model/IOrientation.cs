using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;
using Tekla.Structures.Geometry3d;

namespace PrototypeStructure.Model
{
    public interface IOrientation
    {
        CoordinateSystem CS { get; set; }

    }
}
