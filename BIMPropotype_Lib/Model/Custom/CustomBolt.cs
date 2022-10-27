using BIMPropotype_Lib.Model.Support;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;
using TSG = Tekla.Structures.Geometry3d;
using System.Xml.Serialization;
using Tekla.Structures.Geometry3d;

namespace BIMPropotype_Lib.Model.Custom
{
    [Serializable]
    public class CustomBolt : IFormObject
    {
        [XmlIgnore]
        public TSM.BoltGroup BoltGroup { get; set; }
        public BoltSet BoltSet { get; set; }
        public SupportPolygon Splash { get; set; }
        public SupportDistanse Support { get; set; }
        public TSM.Position Position { get; set; }
        public string BoltStandard { get; set; }
        public double BoltSize { get; set; }
        public bool CreateBolt { get; set; }
        public CustomBolt() { }
        public CustomBolt(TSM.BoltGroup boltGroup)
        {
            Position = boltGroup.Position;
            Support = new SupportDistanse(boltGroup.FirstPosition, boltGroup.SecondPosition);
            Support.Base = boltGroup.GetCoordinateSystem();
            //EndPointOffset = boltGroup.EndPointOffset;
            //StartPointOffset = boltGroup.StartPointOffset;
            BoltSize = boltGroup.BoltSize;
            BoltStandard = boltGroup.BoltStandard;
            //CreateBolt = CheckBoltCreate(boltGroup);
            //BoltSet = new BIMBoltSet(boltGroup);
            Splash = new SupportPolygon(GetPositions(boltGroup));
            //TODO: доработать работу с отрисовкой болтов, если они нужны! 
            //Position нужно улучшить алгоритм передачи смещения.
            //if (CreateBolt && BoltSet.Length > 0)
            //{
            //    if (BoltStandard == "AnchorSet")
            //    {
            //        Position.RotationOffset = 0;
            //        Position.DepthOffset = 0;
            //        Position.PlaneOffset = 0;
            //    }
            //}
            //else
            //{
            //    Position.RotationOffset = 0;
            //    Position.DepthOffset = 0;
            //    Position.PlaneOffset = 0;
            //}
            Position.RotationOffset = 0;
            Position.DepthOffset = 0;
            Position.PlaneOffset = 0;
        }
        public void FormObject()
        {
            BoltGroup = new TSM.BoltXYList();
            BoltGroup.FirstPosition = Support.Start;
            BoltGroup.SecondPosition = Support.End;
            BoltGroup.Position = Position;
            //boltXYList.StartPointOffset = StartPointOffset;
            //boltXYList.EndPointOffset = EndPointOffset;
            BoltGroup.BoltSize = BoltSize;
            BoltGroup.BoltStandard = BoltStandard;
            BoltGroup.Bolt = false;
            //GetBoltSolid(boltXYList);
            AddPosition(BoltGroup as TSM.BoltXYList);
        }
        private void AddPosition(TSM.BoltXYList boltXYList)
        {
            var CSbolt = new TSG.CoordinateSystem(boltXYList.FirstPosition, Support.Base.AxisX, Support.Base.AxisY);
            var localMatrix = TSG.MatrixFactory.ToCoordinateSystem(CSbolt);

            foreach (var point in Splash.Points)
            {
                var localPont = localMatrix.Transform(point);

                boltXYList.AddBoltDistX(localPont.X);
                boltXYList.AddBoltDistY(localPont.Y);
            }
        }

        private List<TSG.Point> GetPositions(TSM.BoltGroup boltGroup)
        {
            var list = new List<TSG.Point>();

            foreach (var item in boltGroup.BoltPositions)
            {
                if (item is TSG.Point point) list.Add(point);
            }

            return list;
        }
        private bool CheckBoltCreate(TSM.BoltGroup boltGroup)
        {
            if (boltGroup.PartToBoltTo.Equals(boltGroup.PartToBoltTo)) return true;
            else return false;
        }
        private void GetBoltSolid(TSM.BoltXYList boltXYList)
        {
            if (CreateBolt) BoltSet.GetBoltSet(boltXYList);
        }

        public TSM.ModelObject GetModelObject( )
        {
            FormObject();
            return BoltGroup;
        }
    }
}
