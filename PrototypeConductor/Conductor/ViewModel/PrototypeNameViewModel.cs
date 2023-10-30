using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrototypeConductor.Model;
using PrototypeConductor.Controller;
using BIMPropotype_Lib.ViewModel;
using BIMPropotype_Lib.ExtentionAPI.Conductor;
using BIMPropotype_Lib.Model;

namespace PrototypeConductor.ViewModel
{
    public class PrototypeNameViewModel : TreeViewItemViewModel
    {
        public FilterPrototype _prototypeName;//Сортировка по именам.
        public Database Database { get; set; }

        public PrototypeNameViewModel(FilterPrototype prototypeName, TreeViewItemViewModel parentField, Database database)
           : base(parentField)
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
                    Database.PrefixDirectory.Meta = _prototypeName.Prototypes[0];
                }
            }
        }

        public bool NameContainsText(string text)
        {
            if (String.IsNullOrEmpty(text) || String.IsNullOrEmpty(this.Prefix))
                return false;

            return this.Prefix.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) > -1;
        }

        /// <summary>
        /// Получить папку прототипа
        /// </summary>
        /// <returns></returns>
        public FieldPrototypeViewModel GetFoulderPropotype() 
        {
            if (this.Parent is FieldPrototypeViewModel parentField) return parentField;
            else if (this.Parent is PrototypeNameViewModel parentPrototype) return parentPrototype.GetFoulderPropotype();
            else return null;
        }

        protected override void LoadChildren()
        {
            foreach (MetaDirectory prototypeName in _prototypeName.Prototypes)
                base.Children.Add(new PrototypeNameViewModel(new FilterPrototype(prototypeName), this, Database));
        }

        /// <summary>
        /// Получение имени, если один элемент и если несколько...
        /// </summary>
        public string Prefix
        {
            get
            {
                if (_prototypeName.CheckSingle()) return _prototypeName.Prototypes[0].ToMark();
                else return _prototypeName.Prefix;
            }

        }
    }
}
