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
            GetUDAList(InPart);
            GetRebar(InPart.GetReinforcements());
            Pruning = new BIMPruning(InPart.GetBooleans());
            if (CheckMainPart(inPart)) GetPutInAssembly(InPart);
        }
        public Beam InPart { get; set; }
        public List<BIMUda> UDAList { get; set; }
        public List<SingleRebar> Rebars { get; set; }
        public List<BIMAssembly> PutInAssembly { get; set; }
        public List<BooleanPart> Antidetails { get; set; }
        public List<BIMRebarGroup> RebarGroups { get; set; }
        public BIMPruning Pruning { get; set; }

        //TODO: Рассмотерть возможность работы с группами через разложение и объединение в простые стержни.

        public virtual void Insert() => Insert(this.InPart);
        public void Insert(Part  part) 
        {
            part.Insert();//При вставке деталь получает новый GUID.
            GetUDAToPart(part);

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

            if (PutInAssembly.Count != 0)//Если есть подсборки, то вставка.
            {
                var mainAssembly = part.GetAssembly();
                foreach (var item in PutInAssembly)
                {
                    item.Insert();
                    var hisAssembly = item.GetAssembly();
                    mainAssembly.Add(hisAssembly);
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
        internal virtual void GetUDAList(Part part)
        {
            UDAList = new List<BIMUda>();

            Hashtable hashtable = new Hashtable();
            part.GetAllUserProperties(ref hashtable);
            foreach (var item in hashtable) 
            {
                if (item is DictionaryEntry dictionary) UDAList.Add(new BIMUda(dictionary.Key, dictionary.Value));
            } 
        }


        internal virtual void GetPutInAssembly(Part part)
        {
            var partEnumChildren = part.GetAssembly().GetSubAssemblies();

            PutInAssembly = new List<BIMAssembly>();

            foreach (var assembly in partEnumChildren)
            {
                if (assembly is Assembly assemlyChild)
                {
                    PutInAssembly.Add(new BIMAssembly(assemlyChild));
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

        protected void GetUDAToPart(Part part) 
        {
            foreach (var uda in UDAList)
            {
                uda.WriteToDetail(part);
            }
        }
        #endregion//private method

    }
}
