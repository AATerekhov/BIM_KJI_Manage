using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;

namespace BIMPropotype_Lib.Model
{
    public interface IStructure
    {
        CoordinateSystem BaseStructure { get; set; }
        void Insert(IStructure father);
        void InsertMirror(IStructure father);

    }
}
