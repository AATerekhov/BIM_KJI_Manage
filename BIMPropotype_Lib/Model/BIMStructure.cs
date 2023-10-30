using BIMPropotype_Lib.Controller;
using BIMPropotype_Lib.ExtentionAPI.PartChildren;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;
using TSM = Tekla.Structures.Model;
using BIMPropotype_Lib.ViewModel;
using System.Xml.Serialization;
using System.IO;
using BIMPropotype_Lib.ExtentionAPI.Conductor;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class BIMStructure :Reference, IStructure, IBIMCollection
    {
        public List<BIMAssembly> Children { get; set; }
        public CoordinateSystem BaseStructure { get; set; }
        public BIMStructure() { }
        public BIMStructure(List<TSM.Assembly> assemblies, TSM.Model model)
        {
            WorkPlaneWorker workPlaneWorker = new WorkPlaneWorker(model);
            BaseStructure = assemblies[0].GetBaseStructure();

            if (BaseStructure != null)
            {
                workPlaneWorker.GetWorkPlace(BaseStructure);

                Children = new List<BIMAssembly>(
                      (from children in assemblies
                       select new BIMAssembly(children, workPlaneWorker.Model)));
                workPlaneWorker.ReturnWorkPlace();
            }
            var bimMainPart = Children[0].Children[0];
            string[] udaKeys = BIMType.BIMStructure.GetUDAKeys();
            Meta = new MetaDirectory(BIMType.BIMStructure, bimMainPart.GetUDASting(udaKeys[0]), bimMainPart.GetUDASting(udaKeys[1]), bimMainPart.GetUDANumber(udaKeys[2]));
        }
        public BIMStructure(MetaDirectory meta):base(meta)
        {
        }

        private TSM.Part GetMainPart(TSM.Assembly assembly) 
        {
            var part = assembly.GetMainPart();
            if (part is TSM.Part mainPart)
            {
                return mainPart;
            }
            else return null;
        }

        public void Insert(IStructure father)
        {
            throw new NotImplementedException();
        }

        public void InsertMirror(IStructure father)
        {
            throw new NotImplementedException();
        }
      
        private void CloneAsCurrentObject(BIMStructure clone)
        {
            Children = clone.Children;
            BaseStructure = clone.BaseStructure;
        }

        public override string ToString()
        {
            return Meta.ToString();
        }

        public void SelectInModel(TSM.Model model)
        {
            throw new NotImplementedException();
        }
    }
}
