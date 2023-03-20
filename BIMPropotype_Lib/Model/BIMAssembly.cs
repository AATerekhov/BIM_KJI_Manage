using System;
using System.Collections.Generic;
using Tekla.Structures.Model;
using System.Linq;
using TSM = Tekla.Structures.Model;
using Tekla.Structures.Geometry3d;
using BIMPropotype_Lib.Controller;
using BIMPropotype_Lib.ExtentionAPI.PartChildren;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class BIMAssembly : IUDAContainer, IStructure, IBIMCollection, IReference
    {
        public string Name { get; set; }
        public string Prefix { get; set; }
        public List<BIMPart> Children { get; set; }
        public UDACollection UDAList { get; set; }
        public CoordinateSystem BaseStructure { get; set; }

        public BIMAssembly() { }
        public BIMAssembly(Assembly assembly, TSM.Model model)
        {
            WorkPlaneWorker workPlaneWorker = new WorkPlaneWorker(model);
            BaseStructure = assembly.GetBaseStructure();

            if (BaseStructure != null)
            {
                workPlaneWorker.GetWorkPlace(BaseStructure);
                assembly.Select();
                var parts = GetPartsToAssembly(assembly);
                Name = parts[0].Name;
                Prefix = parts[0].AssemblyNumber.Prefix;

                this.Children = new List<BIMPart>(
                      (from children in parts
                       select new BIMPart(children, workPlaneWorker.Model)));
                workPlaneWorker.ReturnWorkPlace();
            }
        }

        public  void InsertMirror(IStructure father)
        {          
            BuildAssembly();
            if (father != null)
            {
                var mainAssembly = (father as BIMPart).GetPart().GetAssembly();
                var hisAssembly = this.GetAssembly();
                mainAssembly.Add(hisAssembly);
                mainAssembly.Modify();
            }
        }

        public  void Insert(IStructure father)
        {   
            BuildAssembly();
            if (father != null)
            {
                var mainAssembly = (father as BIMPart).GetPart().GetAssembly();
                var hisAssembly = this.GetAssembly();
                mainAssembly.Add(hisAssembly);
                mainAssembly.Modify();
            }
        }

        //<summary>
        //Сборка отдельных деталей в сборку.
        //</summary>
        private void BuildAssembly()
        {
            Assembly assembly = GetAssembly();

            for (int i = 1; i < Children.Count; i++)
            {
                assembly.Add(Children[i].GetPart());
            }
            assembly.Modify();
        }

        internal Assembly GetAssembly()
        {
            return Children[0].GetPart().GetAssembly();
        }

        #region Privet
        private List<Part> GetPartsToAssembly(Assembly assembly)
        {
            var SecondaryBeams = new List<Part>();
            Part MainPart = null;
            if (assembly.GetMainPart() is Part beam)
            {
                MainPart = beam;
            }

            var ArreyChildren = assembly.GetSecondaries();//Получение второстепенных деталей в сборке.

            foreach (var child in ArreyChildren)
            {
                if (child is Part beamChild)
                {
                    SecondaryBeams.Add(beamChild);
                }
            }

            return GetAllBeams(MainPart, SecondaryBeams);
        }
        private List<Part> GetAllBeams(Part MainPart, List<Part> SecondaryBeams)
        {
            var beams = new List<Part>();
            if (MainPart != null)
            {
                if(CheckPartForAvailability(MainPart)) beams.Add(MainPart);
            }

            if (SecondaryBeams.Count > 0)
            {
                foreach (var part in SecondaryBeams) 
                {
                    if (CheckPartForAvailability(part)) beams.Add(part);
                }
            }
            return beams;
        }

        /// <summary>
        /// Проверка на доступность.
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        internal bool CheckPartForAvailability(Part part) 
        {
            if (part is Beam) return true;
            if (part is ContourPlate) return true;
            if (part is PolyBeam) return true;
            return false;
        }
        #endregion//Privet

        public override string ToString()
        {
            return $"{Name} {Prefix}";
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
