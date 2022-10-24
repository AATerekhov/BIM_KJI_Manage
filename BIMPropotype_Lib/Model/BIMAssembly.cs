using System;
using System.Collections.Generic;
using Tekla.Structures.Model;
using System.Linq;
using Tekla.Structures.Geometry3d;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class BIMAssembly : IUDAContainer
    {
        public string Name { get; set; }
        public string Prefix { get; set; }
        public List<BIMPart> Children { get; set; }
        public UDACollection UDAList { get; set; }
        public BIMAssembly() { }
        public BIMAssembly(Assembly assembly)
        {
            var parts = GetPartsToAssembly(assembly);
            this.Children = new List<BIMPart>();

            for (int i = 0; i < parts.Count; i++)
            {
                if (i == 0)
                {
                    Name = parts[0].Name;
                    Prefix = parts[0].AssemblyNumber.Prefix;                                      
                }

                var bimPart = new BIMPart(parts[i]);
                Children.Add(bimPart);
            }
        }

        public  void InsertMirror(BIMPart father)
        {           

            BuildAssembly();

            if (father != null)
            {
                var mainAssembly = father.GetPart().GetAssembly();
                var hisAssembly = this.GetAssembly();
                mainAssembly.Add(hisAssembly);
                mainAssembly.Modify();
            }
        }

        public  void Insert(BIMPart father)
        {         

            BuildAssembly();

            if (father != null)
            {
                var mainAssembly = father.GetPart().GetAssembly();
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

       
    }
    
}
