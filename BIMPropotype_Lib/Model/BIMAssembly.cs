using System;
using System.Collections.Generic;
using Tekla.Structures.Model;
using System.Linq;


namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class BIMAssembly : iBIMModelObject
    {
        public string Name { get; set; }
        public string Prefix { get; set; }
        public List<BIMBeam> Elements { get; set; }
        public List<BIMPlate> Plates { get; set; }
        public List<BIMPolyBeam> PolyBeams { get; set; }
        public MainTypa Type { get; set; }
        public BIMAssembly() { }

        public BIMAssembly(Assembly assembly)
        {
            var parts = GetPartsToAssembly(assembly);
            this.Elements = new List<BIMBeam>();
            this.Plates = new List<BIMPlate>();
            this.PolyBeams = new List<BIMPolyBeam>();

            for (int i = 0; i < parts.Count; i++)
            {
                if (i == 0)
                {
                    Name = parts[i].Name;
                    Prefix = parts[i].AssemblyNumber.Prefix;
                    if (parts[i] is Beam beam)
                    {
                        Type = MainTypa.beam;
                        Elements.Add(new BIMBeam(beam));
                        continue;
                    }
                    if (parts[i] is ContourPlate plate)
                    {
                        Type = MainTypa.plate;
                        Plates.Add(new BIMPlate(plate));
                        continue;
                    }
                    if (parts[i] is PolyBeam polyBeam)
                    {
                        Type = MainTypa.polyBeam;
                        PolyBeams.Add(new BIMPolyBeam(polyBeam));
                        continue;
                    }
                }
                else
                {
                    if (parts[i] is Beam beam)
                    {
                        Elements.Add(new BIMBeam(beam));
                        continue;
                    }
                    if (parts[i] is ContourPlate plate)
                    {
                        Plates.Add(new BIMPlate(plate));
                        continue;
                    }
                    if (parts[i] is PolyBeam polyBeam)
                    {
                        PolyBeams.Add(new BIMPolyBeam(polyBeam));
                        continue;
                    }

                }
            }
        }

        public void InsertMirror() 
        {

            foreach (var element in Elements)
            {
                element.InsertMirror();
            }

            foreach (var plate in Plates)
            {
                plate.InsertMirror();
            }

            foreach (var polyBeam in PolyBeams)
            {
                polyBeam.InsertMirror();
            }

            BuildAssembly();
        }

        public void Insert()
        {
            foreach (var element in Elements)
            {
                element.Insert();
            }

            foreach (var plate in Plates)
            {
                plate.Insert();
            }

            foreach (var polyBeams in PolyBeams)
            {
                polyBeams.Insert();
            }

            BuildAssembly();
        }
        /// <summary>
        /// Сборка отдельных деталей в сборку.
        /// </summary>
        private void BuildAssembly()
        {
            Assembly assembly = null;

            if ((int)Type == 0)
            {
                assembly = Elements[0].InPart.GetAssembly();
                if (Elements.Count > 1)
                {
                    for (int i = 1; i < Elements.Count; i++)
                    {
                        assembly.Add(Elements[i].InPart);
                    }
                }
                if (Plates.Count > 0)
                {
                    for (int i = 0; i < Plates.Count; i++)
                    {
                        assembly.Add(Plates[i].ContourPlate);
                    }
                }
                if (PolyBeams.Count > 0)
                {
                    for (int i = 0; i < PolyBeams.Count; i++)
                    {
                        assembly.Add(PolyBeams[i].PolyBeam.PolyBeam);
                    }
                }
            }
            else if ((int)Type == 1)
            {
                assembly = Plates[0].ContourPlate.GetAssembly();
                if (Plates.Count > 1)
                {
                    for (int i = 1; i < Plates.Count; i++)
                    {
                        assembly.Add(Plates[i].ContourPlate);
                    }
                }

                if (Elements.Count > 0)
                {
                    for (int i = 0; i < Elements.Count; i++)
                    {
                        assembly.Add(Elements[i].InPart);
                    }
                }
                if (PolyBeams.Count > 0)
                {
                    for (int i = 0; i < PolyBeams.Count; i++)
                    {
                        assembly.Add(PolyBeams[i].PolyBeam.PolyBeam);
                    }
                }
            }
            else if ((int)Type == 2)
            {
                assembly = PolyBeams[0].PolyBeam.PolyBeam.GetAssembly();
                if (PolyBeams.Count > 1)
                {
                    for (int i = 1; i < PolyBeams.Count; i++)
                    {
                        assembly.Add(PolyBeams[i].PolyBeam.PolyBeam);
                    }
                }

                if (Elements.Count > 0)
                {
                    for (int i = 0; i < Elements.Count; i++)
                    {
                        assembly.Add(Elements[i].InPart);
                    }
                }
                if (Plates.Count > 0)
                {
                    for (int i = 0; i < Plates.Count; i++)
                    {
                        assembly.Add(Plates[i].ContourPlate);
                    }
                }
            }
            assembly.Modify();
        }

        internal Assembly GetAssembly()
        {
            if ((int)Type == 0)
            {
                return Elements[0].InPart.GetAssembly();              
            }
            else
            {
                return Plates[0].ContourPlate.GetAssembly();               
            }
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

        public enum MainTypa
        {
            beam = 0,
            plate = 1,
            polyBeam = 2,
        }
    }
    
}
