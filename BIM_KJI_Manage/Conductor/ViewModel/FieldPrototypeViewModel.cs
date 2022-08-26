using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Propotype_Manage.Conductor.Model;
using Propotype_Manage.Conductor.Controller;

namespace Propotype_Manage.Conductor.ViewModel
{
    public class FieldPrototypeViewModel : TreeViewItemViewModel
    {
        readonly FieldPrototype _field;
        public Database Database { get; set; }
        public FieldPrototypeViewModel(FieldPrototype field,ModelDirectoryViewModel parentDirectory , Database database)
            : base(parentDirectory, true)
        {
            Database = database;
            _field = field;
        }

        public string Name
        {
            get { return _field.Name; }
        }

        protected override void LoadChildren()
        {
            foreach (PrototypeName prototypeName in Database.GetPrototype(_field))
                base.Children.Add(new PrototypeNameViewModel(prototypeName, this, Database));
        }
    }
}
