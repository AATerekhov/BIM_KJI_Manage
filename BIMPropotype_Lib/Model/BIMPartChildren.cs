using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;
using BIMPropotype_Lib.Model.Custom;
using BIMPropotype_Lib.ExtentionAPI.Mirror;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class BIMPartChildren:ChildrenPart
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
        public BIMPartChildren() { }
        public BIMPartChildren(TSM.ModelObject modelObject)
        {
            if (modelObject is TSM.Assembly assembly)
            {
                ChildrenType = PartChildrenType.Assembly;
                Assembly = new BIMAssembly(assembly);
            }
            else if (modelObject is TSM.BooleanPart booleanPart)
            {
                if (booleanPart.OperativePart is TSM.ContourPlate)
                {
                    ChildrenType = PartChildrenType.BooleanPlate;
                    BooleanPlate = new CustomBooleanPlate(booleanPart);
                }
                else if (booleanPart.OperativePart is TSM.Beam)
                {
                    ChildrenType = PartChildrenType.BooleanBeam;
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
                CutPlane = new CustomCutPlane(cutPlane);
            }
            else if (modelObject is TSM.Fitting fitting)
            {
                ChildrenType = PartChildrenType.Fitting;
                Fitting = new CustomFitting(fitting);
            }
            else if (modelObject is TSM.SingleRebar singleRebar)
            {
                ChildrenType = PartChildrenType.singleRebar;
                SingleRebar = new CustomSingleRebar(singleRebar);
                UDAList = new UDACollection(singleRebar);
            }
            else if (modelObject is TSM.RebarGroup rebarGroup)
            {
                ChildrenType = PartChildrenType.groupRebar;
                GroupRebar = new CustomGroupRebar(rebarGroup);
                UDAList = new UDACollection(rebarGroup);
            }
            else if (modelObject is TSM.BoltGroup boltGroup)
            {
                ChildrenType = PartChildrenType.Bolt;
                Bolt = new CustomBolt(boltGroup);
                UDAList = new UDACollection(boltGroup);
            }
        }
        public override void Insert() => InsertAs(false);
        public override void InsertMirror() => InsertAs(true);
        public void InsertAs(bool mirror) 
        {
            if (Father != null)
            {
                if ((int)ChildrenType == 6)
                {
                    Assembly.Father = this.Father;
                    if (mirror) Assembly.InsertMirror();
                    else Assembly.Insert();
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
                    TSM.BooleanPart boolean = new TSM.BooleanPart() { Type = booleanType, Father = this.Father.GetPart() };
                    boolean.SetOperativePart(operativePart);
                    
                    if (boolean.Insert()) operativePart.Delete();
                }
                else if ((int)ChildrenType == 2)
                {
                    var boolean = CutPlane.GetModelObject() as TSM.CutPlane;
                    boolean.Father = this.Father.GetPart();
                    if (mirror) boolean.InsertMirror();
                    else boolean.Insert();
                }
                else if ((int)ChildrenType == 3)
                {
                    var boolean = Fitting.GetModelObject() as TSM.Fitting;
                    boolean.Father = this.Father.GetPart();
                    if (mirror) boolean.InsertMirror();
                    else boolean.Insert();
                }
                else if ((int)ChildrenType == 5)
                {
                    Bolt.FormObject();
                    Bolt.BoltGroup.PartToBoltTo = this.Father.GetPart();
                    Bolt.BoltGroup.PartToBeBolted = this.Father.GetPart();
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
                    SingleRebar.SingleRebar.Father = this.Father.GetPart();
                    if (mirror) SingleRebar.SingleRebar.InsertMirror();
                    else SingleRebar.SingleRebar.Insert();
                    UDAList.GetUDAToPart(SingleRebar.SingleRebar);
                }
                else if ((int)ChildrenType == 8)
                {
                    GroupRebar.FormObject();
                    GroupRebar.RebarGroup.Father = this.Father.GetPart(); 
                    if (mirror) GroupRebar.RebarGroup.InsertMirror();
                    else GroupRebar.RebarGroup.Insert();
                    UDAList.GetUDAToPart(GroupRebar.RebarGroup);
                }
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
