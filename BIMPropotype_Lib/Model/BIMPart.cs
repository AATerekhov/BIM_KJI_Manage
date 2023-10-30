using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TSM = Tekla.Structures.Model;
using BIMPropotype_Lib.ExtentionAPI.PartChildren;
using BIMPropotype_Lib.ExtentionAPI.Mirror;
using BIMPropotype_Lib.Model.Custom;
using Tekla.Structures.Geometry3d;
using BIMPropotype_Lib.Controller;
using System.Collections;
using BIMPropotype_Lib.ViewModel;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    //Класс контейнер для обобщенного хранения BIMPart.
    public class BIMPart: IUDAContainer,IStructure, IBIMCollection
    {
        public List<BIMPartChildren> Children { get; set; }
        #region BoxObject
        public CustomBeam Beam { get; set; }
        public CustomPlate Plate { get; set; }
        public CustomPolyBeam PolyBeam { get; set; }
        public PartType Type { get; set; }
        public UDACollection UDAList { get; set; }
        public CoordinateSystem BaseStructure { get; set; }

        public TSM.Part GetPart()
        {
            if ((int)Type == 1)
            {
                return Beam.Beam;
            }
            if ((int)Type == 2)
            {
                return Plate.ContourPlate;
            }
            if ((int)Type == 3)
            {
                return PolyBeam.PolyBeam;
            }
            return null;
        }
        public IFormObject GetCustomPart()
        {
            if ((int)Type == 1)
            {
                return Beam;
            }
            if ((int)Type == 2)
            {
                return Plate;
            }
            if ((int)Type == 3)
            {
                return PolyBeam;
            }
            return null;
        }
        #endregion

        public BIMPart() { }
        public BIMPart(TSM.Part part, TSM.Model model)
        {
            var workPlaneWorker = new WorkPlaneWorker(model);

            UDAList = new UDACollection(part);
            if (part is TSM.Beam beam)
            {
                Type = PartType.beam;

                BaseStructure = beam.GetBaseStructure().Cloned();
                workPlaneWorker.GetWorkPlace(BaseStructure);
                beam.Select();

                Beam = new CustomBeam(beam);
            }
            if (part is TSM.ContourPlate plate)
            {
                Type = PartType.plate;
                //TODO:Место возможного переноса точек пластины.
                BaseStructure = plate.GetBaseStructure().Cloned();
                workPlaneWorker.GetWorkPlace(BaseStructure);
                plate.Select();
                Plate = new CustomPlate(plate);
            }
            if (part is TSM.PolyBeam polyBeam)
            {
                Type = PartType.polyBeam;

                BaseStructure = polyBeam.GetBaseStructure().Cloned();
                workPlaneWorker.GetWorkPlace(BaseStructure);
                polyBeam.Select();
                PolyBeam =new CustomPolyBeam(polyBeam);
            }
            Children = this.GetChildren(workPlaneWorker.Model);

            workPlaneWorker.ReturnWorkPlace();
        }

        public void TransformationОfТodes(MetaDirectory metaAssembly)
        {
            var startPoint = Beam.Support.GetFirst();
            var start = new BIMPartChildren(startPoint, new MetaDirectory(BIMType.BIMJoint, "StartPoint", metaAssembly.ToMark(), 1));
            var endPoint = Beam.Support.GetSecond();
            var end = new BIMPartChildren(Beam.Support.GetSecond(), new MetaDirectory(BIMType.BIMJoint, "EndPoint", metaAssembly.ToMark(), 2));
            var startMatrix = MatrixFactory.ByCoordinateSystems(this.BaseStructure, start.BaseStructure);
            var endMatrix = MatrixFactory.ByCoordinateSystems(this.BaseStructure, end.BaseStructure);
            var children = new List<BIMPartChildren>(Children);
            foreach (var сhild in children)
            {
                var point = сhild.BaseStructure.Origin;
                var startDistance = Distance.PointToPoint(startPoint, point);
                var endDistance = Distance.PointToPoint(endPoint, point);

                if (startDistance < endDistance)
                {
                    if (startDistance < 1000)
                    {
                        Children.Remove(сhild);

                        start.Joint.AddOwn(сhild, startMatrix);
                    }
                }
                else
                {
                    if (endDistance < 1000)
                    {
                        Children.Remove(сhild);
                        end.Joint.AddOwn(сhild, endMatrix);
                    }
                }
            }

            Children.Add(start);
            Children.Add(end);
        }

        public bool AddBIMPart(BIMPart bIMPart ) 
        {
            var point = bIMPart.BaseStructure.Origin;

            var start = Children.Where(p => p.ChildrenType == PartChildrenType.joint && p.Joint.Meta.Number == 1).FirstOrDefault();
            var end = Children.Where(p => p.ChildrenType == PartChildrenType.joint && p.Joint.Meta.Number == 2).FirstOrDefault();

            var startDistance = Distance.PointToPoint(start.BaseStructure.Origin, point);
            var endDistance = Distance.PointToPoint(end.BaseStructure.Origin, point);

            if (startDistance < endDistance)
            {
                if (startDistance < 1500)
                {
                    start.Joint.AddPart(bIMPart);
                    return true;
                }
            }
            else
            {
                if (endDistance < 1500)
                {
                    var matrix = MatrixFactory.ByCoordinateSystems(this.BaseStructure, end.BaseStructure);
                    bIMPart.BaseStructure.Origin = matrix.Transform(bIMPart.BaseStructure.Origin);
                    end.Joint.AddPart(bIMPart);
                    return true;
                }
            }
            return false;
        }



        public  void Insert(IStructure father)
        {
            var customPart = GetCustomPart();
            if (customPart != null)
            {
                var modelPart = customPart.GetModelObject() as TSM.Part;
                modelPart.Insert();
                UDAList.GetUDAToPart(modelPart);
            }
            customPart.Сleaning();
        }
        public  void InsertMirror(IStructure father)
        {
            var customPart = GetCustomPart();
            if (customPart != null)
            {
                var modelPart = customPart.GetModelObject() as TSM.Part;
                modelPart.InsertMirror(true);
                UDAList.GetUDAToPart(modelPart);
            }
        }

        public  List<BIMPartChildren> GetChildren(TSM.Model model)
        {
            var children = new List<BIMPartChildren>();
            var modelPart = GetPart();

            if (modelPart.CheckMainPart())
            {
                var partEnumChildren = modelPart.GetAssembly().GetSubAssemblies();
                foreach (var assembly in partEnumChildren)
                {
                    if (assembly is TSM.Assembly assemlyChild)
                    {
                        var child = new BIMPartChildren(assemlyChild, model);
                        //child.Father = part;
                        children.Add(child);
                    }
                }
            }

            foreach (TSM.ModelObject item in modelPart.GetReinforcements())
            {
                var child = new BIMPartChildren(item, model);
                //child.Father = part;
                children.Add(child);
            }

            foreach (TSM.ModelObject item in modelPart.GetBolts())
            {
                var child = new BIMPartChildren(item, model);
                //child.Father = part;
                children.Add(child);
            }

            foreach (TSM.ModelObject item in modelPart.GetBooleans())
            {
                var child = new BIMPartChildren(item, model);
                //child.Father = part;
                children.Add(child);
            }
            return children;
        }
        public override string ToString()
        {
            return $"{Type} {GetPart().Name}";
        }

        public void SelectInModel()
        {
            ArrayList ObjectsToSelect = new ArrayList();
            ObjectsToSelect.Add(this.GetPart());
            Tekla.Structures.Model.UI.ModelObjectSelector MS = new Tekla.Structures.Model.UI.ModelObjectSelector();
            MS.Select(ObjectsToSelect);
        }
        public string GetUDASting(string key) 
        {
            BIMUda select =  UDAList.UDAList.Where(p => p.Key == key).First();
            if (select != null) return select.Value;
            else return string.Empty;
        }
        public string GetMainPrefix()
        {
            return  GetPart().AssemblyNumber.Prefix;
        }
        public int GetUDANumber(string key)
        {
            BIMUda select = UDAList.UDAList.Where(p => p.Key == key).FirstOrDefault();
            if (select != null) return select.IntValue;
            else return 0;
        }
    }
    public enum PartType
    {
        no = 0,
        beam = 1,
        plate = 2,
        polyBeam = 3,
    }
}
