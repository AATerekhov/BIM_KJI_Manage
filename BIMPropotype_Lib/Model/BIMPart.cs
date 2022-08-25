using System;
using Tekla.Structures.Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BIMPropotype_Lib.Model
{
    public class BIMPart : iBIMModelObject
    {
        public BIMPart() { }
        public BIMPart(Beam inPart)
        {
            InPart = inPart;
            GetRebar(InPart.GetReinforcements());
            Pruning = new BIMPruning(InPart.GetBooleans());
            if (CheckMainPart(inPart)) GetPutInAssembly(InPart);
        }
        public Beam InPart { get; set; }
        public List<SingleRebar> Rebars { get; set; }
        public List<BIMPart> PutInAssemblyBeam { get; set; }
        public List<BIMPlate> PutInAssemblyPlate { get; set; }
        public List<BooleanPart> Antidetails { get; set; }
        public List<BIMRebarGroup> RebarGroups { get; set; }
        public BIMPruning Pruning { get; set; }

        //TODO: Рассмотерть возможность работы с группами через разложение и объединение в простые стержни.

        public virtual void Insert() => Insert(this.InPart);
        public void Insert(Part  part) 
        {
            part.Insert();//При вставке деталь получает новый GUID.
            Pruning.Insert(part);

            foreach (var rebar in Rebars)//Вставка арматуры в деталь.
            {
                rebar.Father = part;
                rebar.Insert();
            }
            foreach (var rebarGroup in RebarGroups)//Вставка арматуры в деталь.
            {
                var rebar = rebarGroup.GetRebarGroup();
                rebar.Father = part;
                rebar.Insert();
            }

            if (PutInAssemblyBeam != null || PutInAssemblyPlate != null)
            {
                var mainAssembly = part.GetAssembly();

                if (PutInAssemblyBeam.Count != 0)//Если есть подсборки, то вставка.
                {
                    foreach (var item in PutInAssemblyBeam)
                    {
                        item.Insert();
                        var hisAssembly = item.InPart.GetAssembly();
                        mainAssembly.Add(hisAssembly);
                    }
                }

                if (PutInAssemblyPlate.Count != 0)//Если есть подсборки, то вставка.
                {
                    foreach (var item in PutInAssemblyPlate)
                    {
                        item.Insert();
                        var hisAssembly = item.ContourPlate.GetAssembly();
                        mainAssembly.Add(hisAssembly);
                    };
                }

                mainAssembly.Modify();
            }

            
        }

        #region internal method
        internal virtual void GetRebar(ModelObjectEnumerator modelObjectEnumerator)
        {
            Rebars = new List<SingleRebar>();
            RebarGroups = new List<BIMRebarGroup>();  
            while (modelObjectEnumerator.MoveNext())
            {
                if (modelObjectEnumerator.Current is SingleRebar reinforcement)
                {
                    reinforcement.Father = null;
                    Rebars.Add(reinforcement);
                    continue;
                }
                if (modelObjectEnumerator.Current is RebarGroup group)
                {
                    group.Father = null;
                    RebarGroups.Add(new BIMRebarGroup(group));
                    continue;
                }
            }
        }


        internal virtual void GetPutInAssembly(Part part)
        {
            var mainAssembly = part.GetAssembly();
            var partEnumChildren = mainAssembly.GetSubAssemblies();

            PutInAssemblyBeam = new List<BIMPart>();
            PutInAssemblyPlate = new List<BIMPlate>();

            foreach (var assembly in partEnumChildren)
            {
                if (assembly is Assembly assemlyChild)
                {
                    var mainPart = assemlyChild.GetMainPart();
                    if (mainPart is Beam beam)
                    {
                        PutInAssemblyBeam.Add(new BIMBeam(beam));
                        continue;
                    }
                    if (mainPart is ContourPlate plate)
                    {
                        PutInAssemblyPlate.Add(new BIMPlate(plate));
                    }
                }
            }
        }
        #endregion//internal method

        #region private method
        /// <summary>
        /// Проверка детали на гравность.
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        private bool CheckMainPart(Part part)
        {
            int main = 0;
            part.GetReportProperty("MAIN_PART", ref main);
            if (main != 0) return true;
            else return false;
        }
        #endregion//private method

    }
}
