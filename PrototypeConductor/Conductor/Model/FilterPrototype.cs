using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIMPropotype_Lib.Model;

namespace PrototypeConductor.Model
{
    /// <summary>
    /// Сортировка файлов в папке
    /// </summary>
    public class FilterPrototype
    {
        public string Prefix { get; private set; }

        public FilterPrototype(MetaDirectory meta)
        {
            Prefix = meta.Prefix;
            Prototypes.Add(meta);
        }

        public bool Add(MetaDirectory meta) 
        {
            if (Prefix == meta.Prefix)
            {
                Prototypes.Add(meta);
                return true;
            }
            else
            {
                return false;
            }
        }

        readonly List<MetaDirectory> _prototypes = new List<MetaDirectory>();
        public List<MetaDirectory> Prototypes
        {
            get { return _prototypes; }
        }

        /// <summary>
        /// Если один, то True
        /// </summary>
        /// <returns></returns>
        public bool CheckSingle() 
        {
            if (Prototypes.Count == 1) return true;
            else return false;
        }
    }


}
