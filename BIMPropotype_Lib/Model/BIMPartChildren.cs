using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;
using BIMPropotype_Lib.Model.Custom;
using BIMPropotype_Lib.ExtentionAPI.Mirror;
using BIMPropotype_Lib.ExtentionAPI.PartChildren;
using Tekla.Structures.Geometry3d;
using BIMPropotype_Lib.Controller;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class BIMPartChildren:IUDAContainer, IStructure
    {
        public CustomFitting Fitting { get; set; }//Pruning
        public CustomCutPlane CutPlane { get; set; }//Pruning
        public CustomBooleanBeam BooleanBeam { get; set; }//Pruning
        public CustomBooleanPlate BooleanPlate { get; set; }//Pruning
        public BIMAssembly Assembly { get; set; }
        public CustomBolt Bolt { get; set; }
        public CustomSingleRebar SingleRebar { get; set; }//Reinforcement
        public CustomGroupRebar GroupRebar { get; set; }//Reinforcement
        public PartChildrenType ChildrenType { get; set; }
        public CoordinateSystem BaseStructure { get; set; }
        public UDACollection UDAList { get; set; }
        public BIMPartChildren() { }
        public BIMPartChildren(TSM.ModelObject modelObject, TSM.Model model)
        {
            var workPlaneWorker = new WorkPlaneWorker(model);

            if (modelObject is TSM.Assembly assembly)
            {
                ChildrenType = PartChildrenType.Assembly;
                Assembly = new BIMAssembly(assembly, model);
                BaseStructure = Assembly.BaseStructure;
            }
            else if (modelObject is TSM.BooleanPart booleanPart)
            {
                if (booleanPart.OperativePart is TSM.ContourPlate operativePlate)
                {
                    ChildrenType = PartChildrenType.BooleanPlate;

                    BaseStructure = operativePlate.GetCoordinateSystem().Cloned();
                    workPlaneWorker.GetWorkPlace(BaseStructure);

                    booleanPart.Select();
                    BooleanPlate = new CustomBooleanPlate(booleanPart);
                }
                else if (booleanPart.OperativePart is TSM.Beam operativeBeam)
                {
                    ChildrenType = PartChildrenType.BooleanBeam;

                    BaseStructure = operativeBeam.GetCoordinateSystem().Cloned();
                    workPlaneWorker.GetWorkPlace(BaseStructure);

                    booleanPart.Select();
                    BooleanBeam = new CustomBooleanBeam(booleanPart);
                }
                else
                {
                    ChildrenType = PartChildrenType.no;
                }
            }
            else if (modelObject is TSM.CutPlane cutPlane) 
            {
                ChildrenType = PartChildrenType.CutPlane;

                BaseStructure = cutPlane.GetCoordinateSystem().Cloned();
                workPlaneWorker.GetWorkPlace(BaseStructure);

                cutPlane.Select();
                cutPlane.Father = null;
                CutPlane = new CustomCutPlane(cutPlane);
               
            }
            else if (modelObject is TSM.Fitting fitting)
            {
                ChildrenType = PartChildrenType.Fitting;

                BaseStructure = fitting.GetCoordinateSystem().Cloned();
                workPlaneWorker.GetWorkPlace(BaseStructure);

                fitting.Select();
                fitting.Father = null;
                Fitting = new CustomFitting(fitting);
            }
            else if (modelObject is TSM.SingleRebar singleRebar)
            {
                ChildrenType = PartChildrenType.singleRebar;

                BaseStructure = singleRebar.GetBaseStructure().Cloned();
                workPlaneWorker.GetWorkPlace(BaseStructure);

                singleRebar.Select();
                singleRebar.Father = null;
                SingleRebar = new CustomSingleRebar(singleRebar);
                UDAList = new UDACollection(singleRebar);
            }
            else if (modelObject is TSM.RebarGroup rebarGroup)
            {
                ChildrenType = PartChildrenType.groupRebar;

                BaseStructure = rebarGroup.GetBaseStructure().Cloned();
                workPlaneWorker.GetWorkPlace(BaseStructure);

                rebarGroup.Select();
                rebarGroup.Father = null;
                GroupRebar = new CustomGroupRebar(rebarGroup);
                UDAList = new UDACollection(rebarGroup);
            }
            else if (modelObject is TSM.BoltGroup boltGroup)
            {
                ChildrenType = PartChildrenType.Bolt;

                BaseStructure = boltGroup.GetCoordinateSystem().Cloned();
                workPlaneWorker.GetWorkPlace(BaseStructure);

                boltGroup.Select();
                Bolt = new CustomBolt(boltGroup);
                UDAList = new UDACollection(boltGroup);
            }

            if (ChildrenType != PartChildrenType.Assembly) workPlaneWorker.ReturnWorkPlace();
        }
        public  void Insert(IStructure father) => InsertAs(false, father as BIMPart);
        public  void InsertMirror(IStructure father) => InsertAs(true, father as BIMPart);
        public void InsertAs(bool mirror, BIMPart father)
        {
            if ((int)ChildrenType == 6)
            {
                if (mirror) Assembly.InsertMirror(father);
                else Assembly.Insert(father);
            }
            else if ((int)ChildrenType == 1 || (int)ChildrenType == 4)
            {
                TSM.Part operativePart = null;
                TSM.BooleanPart.BooleanTypeEnum booleanType = TSM.BooleanPart.BooleanTypeEnum.BOOLEAN_ADD;
                if ((int)ChildrenType == 1)
                {
                    operativePart = BooleanBeam.GetModelObject() as TSM.Part;
                    booleanType = BooleanBeam.BooleanType;
                }
                else
                {
                    operativePart = BooleanPlate.GetModelObject() as TSM.Part;
                    booleanType = BooleanPlate.BooleanType;
                }
                if (mirror) operativePart.InsertMirror(true);
                else operativePart.Insert();
                TSM.BooleanPart boolean = new TSM.BooleanPart() { Type = booleanType, Father = father.GetPart() };
                boolean.SetOperativePart(operativePart);

                if (boolean.Insert()) operativePart.Delete();
            }
            else if ((int)ChildrenType == 2)
            {
                var boolean = CutPlane.GetModelObject() as TSM.CutPlane;
                boolean.Father = father.GetPart();
                if (mirror) boolean.InsertMirror();
                else boolean.Insert();
            }
            else if ((int)ChildrenType == 3)
            {
                var boolean = Fitting.GetModelObject() as TSM.Fitting;
                boolean.Father = father.GetPart();
                if (mirror) boolean.InsertMirror();
                else boolean.Insert();
            }
            else if ((int)ChildrenType == 5)
            {
                Bolt.FormObject();
                Bolt.BoltGroup.PartToBoltTo = father.GetPart();
                Bolt.BoltGroup.PartToBeBolted = father.GetPart();
                if (mirror) { }//TODO: Зеркальной вставки болтов пока нет, нужно добавить.
                else
                {
                    Bolt.BoltGroup.Insert();
                    UDAList.GetUDAToPart(Bolt.BoltGroup);
                }
            }
            else if ((int)ChildrenType == 7)
            {
                SingleRebar.FormObject();
                SingleRebar.SingleRebar.Father = father.GetPart();
                if (mirror) SingleRebar.SingleRebar.InsertMirror();
                else SingleRebar.SingleRebar.Insert();
                UDAList.GetUDAToPart(SingleRebar.SingleRebar);
            }
            else if ((int)ChildrenType == 8)
            {
                GroupRebar.FormObject();
                GroupRebar.RebarGroup.Father = father.GetPart();
                if (mirror) GroupRebar.RebarGroup.InsertMirror();
                else GroupRebar.RebarGroup.Insert();
                UDAList.GetUDAToPart(GroupRebar.RebarGroup);
            }
        }
    }

    public enum PartChildrenType
    {
        no = 0,
        BooleanBeam = 1,
        CutPlane = 2,
        Fitting = 3,
        BooleanPlate = 4,
        Bolt = 5,
        Assembly = 6,
        singleRebar = 7,
        groupRebar = 8,
    }
}
