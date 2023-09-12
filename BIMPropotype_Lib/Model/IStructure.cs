using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;
using TSM = Tekla.Structures.Model;

namespace BIMPropotype_Lib.Model
{
    //Интерфес вставки по базовой точки.
    public interface IStructure
    {
        CoordinateSystem BaseStructure { get; set; }
        void Insert(IStructure father);
        //void SelectInModel(TSM.Model model);
        void InsertMirror(IStructure father);

    }
}
