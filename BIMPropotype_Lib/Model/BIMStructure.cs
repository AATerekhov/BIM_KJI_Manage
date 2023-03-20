using BIMPropotype_Lib.Controller;
using BIMPropotype_Lib.ExtentionAPI.PartChildren;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;
using TSM = Tekla.Structures.Model;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class BIMStructure : IStructure, IBIMCollection, IReference
    {
        public List<BIMAssembly> Children { get; set; }
        public CoordinateSystem BaseStructure { get; set; }
        public string Name { get; set; }
        public string Prefix { get; set; }
        public BIMStructure() { }
        public BIMStructure(List<TSM.Assembly> assemblies, TSM.Model model)
        {
            WorkPlaneWorker workPlaneWorker = new WorkPlaneWorker(model);
            BaseStructure = assemblies[0].GetBaseStructure();
            TSM.Part mainPart = GetMainPart(assemblies[0]);
            this.Name = mainPart.Name;
            this.Prefix = mainPart.AssemblyNumber.Prefix;

            if (BaseStructure != null)
            {
                workPlaneWorker.GetWorkPlace(BaseStructure);

                Children = new List<BIMAssembly>(
                      (from children in assemblies
                       select new BIMAssembly(children, workPlaneWorker.Model)));

                workPlaneWorker.ReturnWorkPlace();
            }
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

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
