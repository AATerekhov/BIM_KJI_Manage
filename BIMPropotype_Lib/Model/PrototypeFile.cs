using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMPropotype_Lib.Model
{
    public class PrototypeFile
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public PrototypeFile(string Name)
        {
            this.Name = Name;
        }
        public override string ToString()
        {
            return this.Name;
        }

    }
}
