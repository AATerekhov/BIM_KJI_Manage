using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;

namespace PrototypeConductor.Model
{
    /// <summary>
    /// Папка архива
    /// </summary>
    public class ModelDirectory
    {
        public string Path { get; private set; }
        public string ModelName { get; private set; }

        readonly List<FieldPrototype> _fields = new List<FieldPrototype>();
        /// <summary>
        /// Папки в архиве хранения
        /// </summary>
        public List<FieldPrototype> Fields
        {
            get { return _fields; }
        }

        public ModelDirectory(string path, string name) 
        {
            Path = path;
            ModelName = name;
        }
    }
}
