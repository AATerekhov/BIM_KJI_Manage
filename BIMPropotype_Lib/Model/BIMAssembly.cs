using System;
using System.Collections.Generic;
using Tekla.Structures.Model;
using System.Linq;


namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class BIMAssembly: iBIMModelObject
    {
        public string Name { get; set; }
        public string Prefix { get; set; }
        public List<BIMBeam> Elements { get; set; }
        public List<BIMPlate> Plates { get; set; }
        public MainTypa Type { get; set; }
        public BIMAssembly() { }

        public BIMAssembly(List<Part> parts)
        {
            this.Elements = new List<BIMBeam>();
            this.Plates = new List<BIMPlate>();
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

                }
            }
        }

        public void Insert()
        {
            foreach (var element in Elements)
            {
                element.Insert();
            }

            foreach (var plate in Plates)
            {
                plate.Insert();//TODO: Требуетя разделить создание файла и считывание из файл, на каком этаме мы получает уже деталь? При считывании деталь нужно получать сразу и хванить ее. 
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
            }
            else
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
            }
            assembly.Modify();
        }
        public enum MainTypa
        {
            beam = 0,
            plate = 1,
        }
    }
    
}
