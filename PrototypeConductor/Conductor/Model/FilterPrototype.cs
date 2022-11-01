using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypeConductor.Model
{
    public class FilterPrototype
    {
        public char _separator = '-';
        public string Prefix { get; private set; }

        public FilterPrototype(PrototypeName prototypeName)
        {
            Prefix = Separation(prototypeName.Prefix);
            Prototypes.Add(prototypeName);
        }

        public bool Add(PrototypeName prototypeName) 
        {
            if (Prefix == Separation(prototypeName.Prefix))
            {
                Prototypes.Add(prototypeName);
                return true;
            }
            else
            {
                return false;
            }
        }

        readonly List<PrototypeName> _prototypes = new List<PrototypeName>();
        public List<PrototypeName> Prototypes
        {
            get { return _prototypes; }
        }

        private string Separation(string text) 
        {
            string rezult = string.Empty;
            rezult = text.Split(_separator)[0];
            return rezult;
        }

        public bool CheckSingle() 
        {
            if (Prototypes.Count == 1) return true;
            else return false;
        }
    }


}
