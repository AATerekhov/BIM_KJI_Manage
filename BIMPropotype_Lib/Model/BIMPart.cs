using System;
using Tekla.Structures.Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BIMPropotype_Lib.ExtentionAPI.Mirror;

namespace BIMPropotype_Lib.Model
{
    public abstract class BIMPart : IUDAContainer, IModelOperations
    {
        public List<BIMReinforcement> Rebars { get; set; }
        public List<BIMAssembly> PutInAssembly { get; set; }
        public List<BIMBolt> Bolts { get; set; }
        public BIMPruning Pruning { get; set; }

        public UDACollection UDAList { get; set; }

        #region Interface //Интрефейс нужно перенести во ViewModel!!!
        public virtual string Name { get; set; }
        public virtual string Class { get; set; }
        public virtual string Profile { get; set; }
        public virtual string Material { get; set; }
        #endregion //Интрефейс нужно перенести во ViewModel!!!

        public virtual void Insert() { }
        public void Insert(Part  part) 
        {
            part.Insert();//При вставке деталь получает новый GUID.
            UDAList.GetUDAToPart(part);

            Pruning.Insert(part);

            foreach (var rebar in Rebars)//Вставка арматуры в деталь.
            {
                rebar.Insert(part);
            }

            foreach (var bolt in Bolts)
            {
                bolt.Inser(part);
            }

            foreach (var item in PutInAssembly)
            {
                item.Father = part;
                item.Insert();
            }
        }
        public virtual void InsertMirror() { }
        public void InsertMirror(Part part)
        {
            part.InsertMirror(true);//При вставке деталь получает новый GUID.
            UDAList.GetUDAToPart(part);

            Pruning.InsertMirror(part);

            foreach (var rebar in Rebars)//Вставка арматуры в деталь.
            {
                rebar.InsertMirror(part);
            }

            foreach (var item in PutInAssembly)
            {
                item.Father = part;
                item.InsertMirror();
            }

            //foreach (var bolt in Bolts)
            //{
            //    bolt.Inser(part);
            //}
        }
        #region internal method
        internal virtual void GetBolts(ModelObjectEnumerator modelObjectEnumerator)
        {
            Bolts = new List<BIMBolt>();
            while (modelObjectEnumerator.MoveNext())
            {
                if (modelObjectEnumerator.Current is BoltGroup bolt)
                {
                    Bolts.Add(new BIMBolt(bolt));
                }
            }
        }

        internal virtual void GetRebar(ModelObjectEnumerator modelObjectEnumerator)
        {
            Rebars = new List<BIMReinforcement>();
            foreach (var item in modelObjectEnumerator)
            {
                if (item is Reinforcement reinforcement)
                {
                    Rebars.Add(new BIMReinforcement(reinforcement));
                }
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
        protected bool CheckMainPart(Part part)
        {
            int main = 0;
            part.GetReportProperty("MAIN_PART", ref main);
            if (main != 0) return true;
            else return false;
        }
        #endregion//private method

    }
}
