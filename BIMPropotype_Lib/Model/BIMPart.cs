using System;
using Tekla.Structures.Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BIMPropotype_Lib.Model
{
    public class BIMPart
    {
        public BIMPart() { }
        public BIMPart(Part inPart)
        {
            InPart = inPart;
            Rebars = GetRebar();
        }
        public Part InPart { get; set; }

        public List<Reinforcement> Rebars { get; set; }

        public IEnumerator GetChildren() 
        {
            return  InPart.GetChildren().GetEnumerator();
        }

        public List<Reinforcement> GetRebar() 
        {
            var partEnum = this.GetChildren();
            List<Reinforcement> rebars = new List<Reinforcement>();
            while (partEnum.MoveNext())
            {
                if (partEnum.Current is Reinforcement reinforcement) rebars.Add(reinforcement);
            }

            return rebars;
        }
        public void Insert() 
        {
            InPart.Insert();
        }


    }
}
