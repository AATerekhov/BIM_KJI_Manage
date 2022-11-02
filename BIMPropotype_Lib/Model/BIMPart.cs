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

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    //Класс контейнер для обобщенного хранения BIMPart.
    public class BIMPart: IUDAContainer,IStructure
    {
        [XmlIgnore]
        public string Class
        {
            get { return GetPart().Class; }
        }
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
    }
    public enum PartType
    {
        no = 0,
        beam = 1,
        plate = 2,
        polyBeam = 3,
    }
}
