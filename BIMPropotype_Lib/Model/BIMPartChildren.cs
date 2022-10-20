using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;
using BIMPropotype_Lib.Model.Custom;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class BIMPartChildren:ChildrenPart
    {
        public CustomFitting Fitting { get; set; }
        public CustomCutPlane CutPlane { get; set; }
        public CustomBooleanBeam BooleanBeam { get; set; }
        public CustomBooleanPlate BooleanPlate { get; set; }
        public BIMAssembly Assembly { get; set; }
        public PartChildrenType ChildrenType { get; set; }
        public BIMPartChildren() { }
        public BIMPartChildren(TSM.ModelObject modelObject)
        {

        }

        public BIMPartChildren(TSM.ModelObject modelObject, PartChildrenType childrenType)
        {
            ChildrenType = childrenType;
            if ((int)ChildrenType == 6)
            {
                Assembly = new BIMAssembly(modelObject as TSM.Assembly);
            }
            else if ((int)ChildrenType == 1)
            {
                BooleanBeam = new CustomBooleanBeam(modelObject as TSM.BooleanPart);
            }
            else if ((int)ChildrenType == 2)
            {
                CutPlane = new CustomCutPlane(modelObject as TSM.CutPlane);
            }
            else if ((int)ChildrenType == 3)
            {
                Fitting = new CustomFitting(modelObject as TSM.Fitting);
            }
            else if ((int)ChildrenType == 4)
            {
                BooleanPlate = new CustomBooleanPlate(modelObject as TSM.BooleanPart);
            }
        }

        public override void Insert()
        {
            if (Father != null)
            {
                if ((int)ChildrenType == 6)
                {

                }
                else if ((int)ChildrenType == 1)
                {

                }
                else if ((int)ChildrenType == 2)
                {

                }
                else if ((int)ChildrenType == 3)
                {

                }
                else if ((int)ChildrenType == 4)
                {
                    BooleanPlate.FormObject();
                    var operativePart = BooleanPlate.Plate.ContourPlate;
                    operativePart.Insert();
                    TSM.BooleanPart boolean = new TSM.BooleanPart() { Type = BooleanPlate.BooleanType, Father = this.Father.GetPart() };
                    boolean.SetOperativePart(operativePart);
                    if (boolean.Insert()) operativePart.Delete();
                }
            }
        }

        public override void InsertMirror()
        {
            base.InsertMirror();
        }
    }

    public enum PartChildrenType
    {
        no = 0,
        BooleanBeam = 1,
        CutPlane = 2,
        Fitting = 3,
        BooleanPlate = 4,
        BIMBolt = 5,
        BIMAssembly = 6,
        BIMReinforcement = 7,

    }
}
