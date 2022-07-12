using System;
using System.Collections.Generic;
using Tekla.Structures.Model;


namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class BIMAssembly
    {
        public List<Beam> Beams { get; set; }
        public List<BIMAssembly> InBeamAssemblies { get; set; }
        public BIMAssembly()
        {

        }
        public BIMAssembly(List<Beam> beams)
        {
            this.Beams = beams;
            InBeamAssemblies = new List<BIMAssembly>();
        }

        public BIMAssembly(List<Beam> beams, List<BIMAssembly> bIMAssemblies)
        {
            this.Beams = beams;
            InBeamAssemblies = bIMAssemblies;
        }

        public void Insert() 
        {
            foreach (var beam in Beams)
            {
                beam.Insert();
            }
        }
    }
}
