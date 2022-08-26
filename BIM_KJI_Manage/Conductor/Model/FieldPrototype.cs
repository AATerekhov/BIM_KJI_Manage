using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propotype_Manage.Conductor.Model
{
    public class FieldPrototype
    {
        public string Name { get; private set; }

        public FieldPrototype(string name)
        {
            Name = name;
        }

        readonly List<PrototypeName> _prefix = new List<PrototypeName>();
        public List<PrototypeName> Prefix
        {
            get { return _prefix; }
        }
    }
}
