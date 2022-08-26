using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Propotype_Manage.Conductor.Model;
using Propotype_Manage.Conductor.Controller;

namespace Propotype_Manage.Conductor.ViewModel
{
    public class ModelDirectoryViewModel : TreeViewItemViewModel
    {
        readonly ModelDirectory _directory;
        public ModelDirectoryViewModel(ModelDirectory directory)
            : base(null, false)
        {
            _directory = directory;
        }

        public string ModelName
        {
            get { return _directory.ModelName; }
        }

        protected override void LoadChildren()
        {
            foreach (FieldPrototype fieldPrototyfe in Database.GetFieldPrototypes(_directory))
                base.Children.Add(new FieldPrototypeViewModel(fieldPrototyfe, this));
        }
    }
}
