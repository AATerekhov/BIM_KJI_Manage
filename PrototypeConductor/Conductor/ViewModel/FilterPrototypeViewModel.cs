
using PrototypeConductor.Model;
using PrototypeConductor.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypeConductor.ViewModel
{
    public class FilterPrototypeViewModel : TreeViewItemViewModel
    {
        public Database Database { get; set; }
        public FilterPrototypeViewModel(PrototypeName prototypeName, FieldPrototypeViewModel parentField, Database database)
           : base(parentField, false)
        {
            Database = database;
        }
    }
}
