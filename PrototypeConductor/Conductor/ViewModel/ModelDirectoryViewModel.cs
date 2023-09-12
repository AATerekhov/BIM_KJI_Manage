using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrototypeConductor.Model;
using PrototypeConductor.Controller;

namespace PrototypeConductor.ViewModel
{
    public class ModelDirectoryViewModel : TreeViewItemViewModel
    {
        readonly ModelDirectory _directory;
        public Database Database { get; set; }
        public ModelDirectoryViewModel(ModelDirectory directory, Database database)
            : base(null)
        {
            _directory = directory;
            Database = database;
            LoadChildren();
        }

        #region Search
        public void PerformSearch()
        {
            if (Database._matchingPeopleEnumerator == null || !Database._matchingPeopleEnumerator.MoveNext())
            { 
                VerifyMatchingPeopleEnumerator();
                Database._matchingPeopleEnumerator.MoveNext();
            } 

            var prototyView = Database._matchingPeopleEnumerator.Current;

            if (prototyView == null)
                return;

            // Ensure that this person is in view.
            if (prototyView.Parent != null)
                prototyView.Parent.IsExpanded = true;

            prototyView.IsSelected = true;
        }

        void VerifyMatchingPeopleEnumerator()
        {
            var matches = this.FindMatches(Database.SearchText);
            Database._matchingPeopleEnumerator = matches.GetEnumerator();
        }
        IEnumerable<TreeViewItemViewModel> FindMatches(string searchText)
        {
            //this.IsExpanded = true;
            foreach (TreeViewItemViewModel child in Children)
            {
                if (child is FieldPrototypeViewModel fieldPrototypeViewModel)
                {
                    //fieldPrototypeViewModel.IsExpanded = true;
                    foreach (var item in fieldPrototypeViewModel.Children)
                    {
                        if (item is StructureFieldViewModel structureFieldViewModel)
                        {
                            if (structureFieldViewModel.NameContainsText(searchText))
                                yield return structureFieldViewModel;
                        }
                        else if (item is PrototypeNameViewModel prototypeNameViewModel)
                        {
                            if (prototypeNameViewModel.NameContainsText(searchText))
                                yield return prototypeNameViewModel;
                        }
                    }
                }                
            }         
        }
        #endregion


        public string ModelName
        {
            get { return _directory.ModelName; }
        }

        protected override void LoadChildren()
        {
            foreach (FieldPrototype fieldPrototyfe in Database.GetFieldPrototypes(_directory))
                base.Children.Add(new FieldPrototypeViewModel(fieldPrototyfe, this,Database));
        }
    }
}
