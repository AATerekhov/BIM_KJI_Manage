using System;
using Tekla.Structures.Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BIMPropotype_Lib.Model
{
    public abstract class BIMPart
    {
        public BIMPart() { }
        public BIMPart(Beam inPart)
        {
            InPart = inPart;
            Rebars = GetRebar();
            if(CheckMainPart(inPart)) PutInAssembly = GetPutInAssembly();
        }
        public Beam InPart { get; set; }

        public List<SingleRebar> Rebars { get; set; }

        public List<BIMPart> PutInAssembly { get; set; }

        public IEnumerator GetChildren() 
        {
            return  InPart.GetChildren().GetEnumerator();
        }

        public virtual List<SingleRebar> GetRebar() 
        {
            var partEnum = this.GetChildren();
            List<SingleRebar> rebars = new List<SingleRebar>();
            while (partEnum.MoveNext())
            {
                if (partEnum.Current is SingleRebar reinforcement) rebars.Add(reinforcement);
            }

            return rebars;
        }
        public virtual List<BIMPart> GetPutInAssembly()
        {
            var mainAssembly = InPart.GetAssembly();
            var partEnumChildren = mainAssembly.GetSubAssemblies();

            List<BIMPart> components = new List<BIMPart>();

            foreach (var assembly in partEnumChildren) 
            {
                if (assembly is Assembly assemlyChild)
                {
                    if (assemlyChild.GetMainPart() is Beam beam)
                    {
                        components.Add(new BIMBeam(beam));
                    }
                }

            }

            return components;
        }

        private bool CheckMainPart(Part part) 
        {
            int main = 0;
            part.GetReportProperty("MAIN_PART", ref main);
            if (main!= 0) return true;
            else return false;
        }

        //TODO: Рассмотерть возможность работы с группами через разложение и обьединение в простые стержни.
        public virtual void Insert() 
        {
            InPart.Insert();//При вставле деталь получает новый GUID.
            
            foreach (var rebar in Rebars)
            {
                rebar.Father = InPart;
                rebar.Insert();
            }
            if (PutInAssembly != null)
            {
                if (PutInAssembly.Count != 0)
                {
                    var mainAssembly = InPart.GetAssembly();
                    foreach (var item in PutInAssembly)
                    {
                        item.Insert();
                        var hisAssembly = item.InPart.GetAssembly();
                        mainAssembly.Add(hisAssembly);
                    }
                    mainAssembly.Modify();
                }
                    
            }
             
        }
    }
}
