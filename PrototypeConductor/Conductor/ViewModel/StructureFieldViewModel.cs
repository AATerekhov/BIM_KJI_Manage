using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrototypeConductor.Model;
using PrototypeConductor.Controller;
using BIMPropotype_Lib.ViewModel;
using BIMPropotype_Lib.ExtentionAPI.Conductor;

namespace PrototypeConductor.ViewModel
{
    public class StructureFieldViewModel : TreeViewItemViewModel
    {

        readonly StructureField _structureField;
        public Database Database { get; set; }
        public StructureFieldViewModel(StructureField structureField, FieldPrototypeViewModel parentDirectory, Database database)
            : base(parentDirectory)
        {
            Database = database;
            _structureField = structureField;
            this.PropertyChanged += StructureViewModel_PropertyChanged;
        }

        private void StructureViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                if ((sender as TreeViewItemViewModel).IsSelected)
                {
                    Database.PrefixDirectory.Meta = _structureField.Prefix[0];
                }
            }
        }

        public string Name
        {
            get { return _structureField.Name.GetNameFiles(); }
        }

        public FieldPrototypeViewModel GetName()
        {
            if (this.Parent is FieldPrototypeViewModel parentField) return parentField;
            else return null;
        }

        public bool NameContainsText(string text)
        {
            if (String.IsNullOrEmpty(text) || String.IsNullOrEmpty(this.Name))
                return false;

            return this.Name.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) > -1;
        }

        //protected override void LoadChildren()
        //{
        //    foreach (FilterPrototype prototypeName in Database.GetFilterDirectories(_structureField))
        //        base.Children.Add(new PrototypeNameViewModel(prototypeName, this, Database));
        //}
    }
}
