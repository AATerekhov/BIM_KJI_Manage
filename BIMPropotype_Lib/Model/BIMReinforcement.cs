using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;
using BIMPropotype_Lib.ExtentionAPI.Mirror;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class BIMReinforcement: ChildrenPart
    {
        public SingleRebar SingleRebar { get; set; }
        public BIMRebarGroup RebarGroup { get; set; }
        public BIMReinforcementType ReinforcementType { get; set; }
        public BIMReinforcement() { }
        internal BIMReinforcement(Reinforcement reinforcement)
        {
            reinforcement.Father = null;
            UDAList = new UDACollection(reinforcement);
            if (reinforcement is SingleRebar singleRebar)
            {
                ReinforcementType = BIMReinforcementType.single;
                SingleRebar = singleRebar;

            }
            if (reinforcement is RebarGroup group)
            {
                ReinforcementType = BIMReinforcementType.group;
                RebarGroup = new BIMRebarGroup(group);
            }
        }

        internal void Insert(ModelObject modelObject)
        {
            Father = modelObject;
            this.Insert();
        }
        internal void InsertMirror(ModelObject modelObject)
        {
            Father = modelObject;
            this.InsertMirror();
        }

        public override void Insert()
        {
            if ((int)ReinforcementType == 0)
            {
                SingleRebar.Father = Father;
                SingleRebar.Insert();
                UDAList.GetUDAToPart(SingleRebar);
            }
            if ((int)ReinforcementType == 1)
            {
                var rebar = RebarGroup.GetRebarGroup();
                rebar.Father = Father;
                rebar.Insert();
                UDAList.GetUDAToPart(rebar);
            }
        }
        public override void InsertMirror()
        {
            if ((int)ReinforcementType == 0)
            {
                SingleRebar.Father = Father;
                SingleRebar.InsertMirror();
                UDAList.GetUDAToPart(SingleRebar);
            }
            if ((int)ReinforcementType == 1)
            {
                var rebar = RebarGroup.GetRebarGroup();
                rebar.Father = Father;
                rebar.InsertMirror();
                UDAList.GetUDAToPart(rebar);
            }
        }

    }

    public enum BIMReinforcementType
    {
        single=0,
        group=1,
    }
}
