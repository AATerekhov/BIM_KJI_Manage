using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Propotype_Manage.Conductor.Model;
using Propotype_Manage.Conductor.Controller;

namespace Propotype_Manage.Conductor.ViewModel
{
    public class PrototypeNameViewModel : TreeViewItemViewModel
    {
        readonly PrototypeName _prototypeName;
        public Database Database { get; set; }

        public PrototypeNameViewModel(PrototypeName prototypeName, FieldPrototypeViewModel parentField, Database database)
           : base(parentField, false)
        {
            Database =  database;
            _prototypeName = prototypeName;
        }

        public string Prefix
        {
            get { return _prototypeName.Prefix; }
        }
    }
}
