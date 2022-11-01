using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrototypeConductor.Model;
using PrototypeConductor.Controller;

namespace PrototypeConductor.ViewModel
{
    public class PrototypeNameViewModel : TreeViewItemViewModel
    {
        public FilterPrototype _prototypeName;
        public Database Database { get; set; }

        public PrototypeNameViewModel(FilterPrototype prototypeName, TreeViewItemViewModel parentField, Database database)
           : base(parentField, false)
        {
            Database = database;
            _prototypeName = prototypeName;
            this.PropertyChanged += PrototypeNameViewModel_PropertyChanged;
            if (!_prototypeName.CheckSingle()) this.LoadChildren();
        }

        private void PrototypeNameViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                if ((sender as TreeViewItemViewModel).IsSelected)
                {
                    Database.PrefixDirectory.FieldName = GetField().Name;
                    Database.PrefixDirectory.Prefix = _prototypeName.Prototypes[0].Prefix;
                    Database.LoadingPrototype();
                }
            }
        }

        public FieldPrototypeViewModel GetField() 
        {
            if (this.Parent is FieldPrototypeViewModel parentField) return parentField;
            else if (this.Parent is PrototypeNameViewModel parentPrototype) return parentPrototype.GetField();
            else return null;
        }

        protected override void LoadChildren()
        {
            foreach (PrototypeName prototypeName in _prototypeName.Prototypes)
                base.Children.Add(new PrototypeNameViewModel(new FilterPrototype(prototypeName), this, Database));
        }

        public string Prefix
        {
            get
            {
                if (_prototypeName.CheckSingle()) return _prototypeName.Prototypes[0].Prefix;
                else return _prototypeName.Prefix;
            }

        }
    }
}
