using BIMPropotype_Lib.Model.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;
using Tekla.Structures.Geometry3d;

namespace BIMPropotype_Lib.Model.Custom
{
    public interface IFormObject
    {
        /// <summary>
        /// Перенос supportElement в TSM.ModelObject
        /// </summary>
        void FormObject();
        TSM.ModelObject GetModelObject();
    }
}
