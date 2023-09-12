using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIMPropotype_Lib.Model;

namespace PrototypeConductor.Model
{
    public class StructureField
    {
        public string Name { get; private set; }

        public StructureField(string name)
        {
            Name = name;
        }

        readonly List<MetaDirectory> _prefix = new List<MetaDirectory>();
        /// <summary>
        /// Список файлов
        /// </summary>
        public List<MetaDirectory> Prefix
        {
            get { return _prefix; }
        }
    }
}
