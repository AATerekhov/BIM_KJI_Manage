﻿using System;
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
    public class CustomCutPlane : IFormObject
    {
        public TSM.CutPlane CutPlane { get; set; }
        public SupportPlace Support { get; set; }

        public CustomCutPlane() { }
        public CustomCutPlane(TSM.CutPlane cutPlane)
        {
            CutPlane = cutPlane;
            Support = new SupportPlace(cutPlane.Plane);            
            this.Сleaning();
        }

        public void FormObject()
        {
            CutPlane.Plane = Support.GetPlane();
        }

        public ModelObject GetModelObject()
        {
            FormObject();
            return CutPlane;
        }

        public void Сleaning()
        {
            CutPlane.Father = null;
            CutPlane.Plane = null;
        }
    }
}
