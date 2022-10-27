using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;
using BIMPropotype_Lib.Model.Support;
using Tekla.Structures.Model;
using Tekla.Structures.Geometry3d;

namespace BIMPropotype_Lib.Model.Custom
{
    [Serializable]
    public  class CustomFitting: IFormObject
    {
        public TSM.Fitting Fitting { get; set; }
        public SupportPlace Support { get; set; }

        public CustomFitting() { }
        public CustomFitting(TSM.Fitting fitting)
        {
            Support = new SupportPlace(fitting.Plane);
            Fitting = fitting;
            Fitting.Plane = null;
        }

        public void FormObject()
        {
            Fitting.Plane = Support.GetPlane();
        }

        public ModelObject GetModelObject()
        {
            FormObject();
            return Fitting;
        }
    }
}
