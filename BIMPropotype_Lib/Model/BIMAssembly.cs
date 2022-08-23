﻿using System;
using System.Collections.Generic;
using Tekla.Structures.Model;
using System.Linq;


namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class BIMAssembly
    {
        public List<BIMBeam> Elements { get; set; }
        public BIMAssembly() { }

        public BIMAssembly(List<Beam> beams)
        {
            this.Elements = new List<BIMBeam>(from beam in beams select new BIMBeam(beam));
        }

        public void Insert()
        {
            foreach (var element in Elements)
            {
                element.Insert();
            }
        }
    }
}
