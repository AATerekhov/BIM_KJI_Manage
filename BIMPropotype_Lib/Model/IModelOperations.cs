using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;

namespace BIMPropotype_Lib.Model
{
    public interface IModelOperations
    {
        void Insert(TSM.Model model);
        void InsertMirror();
    }
}
