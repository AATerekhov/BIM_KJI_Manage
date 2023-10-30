using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrototypeConductor.Model;
using PrototypeConductor.Controller;

namespace PrototypeConductor.ViewModel
{
    public class FieldPrototypeViewModel : TreeViewItemViewModel
    {
        readonly FieldPrototype _field;
        public Database Database { get; set; }
        public FieldPrototypeViewModel(FieldPrototype field,ModelDirectoryViewModel parentDirectory , Database database)
            : base(parentDirectory)
        {
            Database = database;
            _field = field; 
            LoadChildren();

            this.PropertyChanged += FieldPrototypeViewModel_PropertyChanged;            
        }

        private void FieldPrototypeViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                //if ((sender as TreeViewItemViewModel).IsSelected)
                //{
                //    Database.PrefixDirectory.Meta.Name = Name;
                //}
            }
        }

        public string Name
        {
            get { return _field.Name; }
        }

        protected override void LoadChildren()
        {
            //foreach (StructureField structureField in Database.GetStructure(_field))
            //    base.Children.Add(new StructureFieldViewModel(structureField, this, Database));

            foreach (FilterPrototype prototypeName in Database.GetFilterDirectories(_field))
                base.Children.Add(new PrototypeNameViewModel(prototypeName, this, Database));
        }
    }
}
