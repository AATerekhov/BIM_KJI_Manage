using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;
using TSG = Tekla.Structures.Geometry3d;
using System.Collections;
using BIMPropotype_Lib.Model.Support;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class BIMBolt
    {
        public UDACollection UDAList { get; set; }
        public SupportPolygon Splash { get; set; }
        public SupportDistanse SupportPoints { get; set; }
        public TSM.Position Position { get; set; }
        public string BoltStandard { get; set; }
        public double BoltSize { get; set; }
        public TSG.CoordinateSystem CS { get; set; }
        public bool CreateBolt { get; set; }
        public BIMBoltSet BoltSet { get; set; }
        public BIMBolt() { }


        internal BIMBolt(TSM.BoltGroup boltGroup)
        {
            CS = boltGroup.GetCoordinateSystem();
            UDAList = new UDACollection(boltGroup);            
            Position = boltGroup.Position;
            SupportPoints = new SupportDistanse(boltGroup.FirstPosition, boltGroup.SecondPosition);
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


        public void Inser(TSM.Part part)
        {
            TSM.BoltXYList boltXYList = new TSM.BoltXYList();
            boltXYList.PartToBoltTo = part;
            boltXYList.PartToBeBolted = part;
            boltXYList.FirstPosition = SupportPoints.Start;
            boltXYList.SecondPosition = SupportPoints.End;
            boltXYList.Position = Position;
            //boltXYList.StartPointOffset = StartPointOffset;
            //boltXYList.EndPointOffset = EndPointOffset;
            boltXYList.BoltSize = BoltSize;
            boltXYList.BoltStandard = BoltStandard;
            boltXYList.Bolt = false;
            //GetBoltSolid(boltXYList);
            AddPosition(boltXYList);
            UDAList.GetUDAToPart(boltXYList);
            boltXYList.Insert();
        }

        private void AddPosition(TSM.BoltXYList boltXYList) 
        {
            var CSbolt = new TSG.CoordinateSystem(boltXYList.FirstPosition, CS.AxisX, CS.AxisY);
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
            var list = new  List<TSG.Point>();

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
    }   

}
