using System;
using Tekla.Structures.Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BIMPropotype_Lib.ExtentionAPI.Mirror;

namespace BIMPropotype_Lib.Model
{
    public class BIMPart : iBIMModelObject
    {
        public BIMPart() { }
        public BIMPart(Beam inPart)
        {
            InPart = inPart;
            UDAList = new UDACollection(InPart);
            GetRebar(InPart.GetReinforcements());
            Pruning = new BIMPruning(InPart.GetBooleans());
            GetBolts(InPart.GetBolts());
            if (CheckMainPart(inPart)) GetPutInAssembly(InPart);
        }
        public Beam InPart { get; set; }
        public UDACollection UDAList { get; set; }
        public List<BIMReinforcement> Rebars { get; set; }
        public List<BIMAssembly> PutInAssembly { get; set; }
        public List<BooleanPart> Antidetails { get; set; }
        public List<BIMBolt> Bolts { get; set; }
        public BIMPruning Pruning { get; set; }

        public virtual string  Name
        {
            get { return InPart.Name; }
            set { if (InPart != null) InPart.Name = value; }
        }

        public virtual string Class
        {
            get { return InPart.Class; }
            set { if (InPart != null) InPart.Class = value; }
        }
        public virtual string Profile
        {
            get { return InPart.Profile.ProfileString; }
            set { if (InPart != null) InPart.Profile.ProfileString = value; }
        }
        public virtual string Material
        {
            get { return InPart.Material.MaterialString; }
            set { if (InPart != null) InPart.Material.MaterialString = value; }
        }
        public virtual void InsertMirror() => InsertMirror(this.InPart);
        public void InsertMirror(Part part)
        {
            part.InsertMirror(true);//При вставке деталь получает новый GUID.
            UDAList.GetUDAToPart(part);

            Pruning.InsertMirror(part);

            foreach (var rebar in Rebars)//Вставка арматуры в деталь.
            {
                rebar.InsertMirror(part);
            }

            //foreach (var bolt in Bolts)
            //{
            //    bolt.Inser(part);
            //}

            if (PutInAssembly.Count != 0)//Если есть подсборки, то вставка.
            {
                var mainAssembly = part.GetAssembly();
                foreach (var item in PutInAssembly)
                {
                    item.InsertMirror();
                    var hisAssembly = item.GetAssembly();
                    mainAssembly.Add(hisAssembly);
                }
                mainAssembly.Modify();
            }
        }
        public virtual void Insert() => Insert(this.InPart);
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
