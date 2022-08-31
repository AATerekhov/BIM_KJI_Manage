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
        readonly PrototypeName _prototypeName;
        public Database Database { get; set; }

        public PrototypeNameViewModel(PrototypeName prototypeName, FieldPrototypeViewModel parentField, Database database)
           : base(parentField, false)
        {
            Database =  database;
            _prototypeName = prototypeName;
            this.PropertyChanged += PrototypeNameViewModel_PropertyChanged;
        }

        private void PrototypeNameViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                if ((sender as TreeViewItemViewModel).IsSelected)
                {
                    Database.PrefixDirectory.Prefix = Prefix;
                    Database.PrefixDirectory.FieldName = (this.Parent as FieldPrototypeViewModel).Name;
                }
            }
        }

        public string Prefix
        {
            get { return _prototypeName.Prefix; }
        }
    }
}
