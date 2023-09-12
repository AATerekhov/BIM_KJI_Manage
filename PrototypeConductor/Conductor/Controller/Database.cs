using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using PrototypeConductor.Model;
using BIMPropotype_Lib.ViewModel;
using PrototypeObserver;
using BIMPropotype_Lib.Controller;
using BIMPropotype_Lib.ExtentionAPI.Conductor;
using BIMPropotype_Lib.Model;
using System.ComponentModel;
using PrototypeConductor.ViewModel;

namespace PrototypeConductor.Controller
{
    public class Database : INotifyPropertyChanged
    {
        public PrefixDirectory PrefixDirectory { get; set; }
        public SelectObserver InSelectObserver { get; set; }

        public IEnumerator<TreeViewItemViewModel> _matchingPeopleEnumerator;
        string _searchText = String.Empty;

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (value == _searchText)
                    return;
                _searchText  = value;

                this.OnPropertyChanged("SearchText");
                _matchingPeopleEnumerator = null;
            }
        }


        public Database(PrefixDirectory prefixDirectory, SelectObserver selectObserver)
        {
            InSelectObserver = selectObserver;
            PrefixDirectory = prefixDirectory;
        }

        public List<FieldPrototype> GetFieldPrototypes(ModelDirectory modelDirectory) => new List<FieldPrototype>(from item in PrefixDirectory.Meta.Type.GetNameList() select new FieldPrototype(item));
        
        public List<StructureField> GetStructure(FieldPrototype fieldPrototype)
        {
            List <StructureField> rezult = new List<StructureField>();
            foreach (var item in BIMType.BIMStructure.GetMetaList(fieldPrototype.Name))
            {
                rezult.Add(new StructureField(item.Name));
            }
            return rezult;
        }

        public List<ModelDirectory> GetModelDirectories()
        {
            return new List<ModelDirectory>() { new ModelDirectory(FileExplorerExtentions.GetDataDirectory(), PrefixDirectory.ModelInfo.ModelName) };
        }
        
        public List<FilterPrototype> GetFilterDirectories(FieldPrototype fieldPrototype)
        {
            List<MetaDirectory> filterNames = new List<MetaDirectory>(PrefixDirectory.Meta.Type.GetMetaList(fieldPrototype.Name));
            List<FilterPrototype> filterPrototypes = new List<FilterPrototype>();

            //Прогон имен файлов.
            for (int i = 0; i < filterNames.Count; i++)
            {
                bool metka = false; 
                foreach (var item in filterPrototypes)
                {
                    metka = item.Add(filterNames[i]);
                    if (metka) break;                    
                }

                if (!metka)
                {
                    filterPrototypes.Add(new FilterPrototype(filterNames[i]));
                }
            }
            return filterPrototypes;
        }
        
        public List<FilterPrototype> GetFilterDirectories(StructureField structureField)
        {
            List<MetaDirectory> filterNames = new List<MetaDirectory>(PrefixDirectory.Meta.Type.GetMetaList(structureField.Name));
            List<FilterPrototype> filterPrototypes = new List<FilterPrototype>();

            //Прогон имен файлов.
            for (int i = 0; i < filterNames.Count; i++)
            {
                bool metka = false;
                foreach (var item in filterPrototypes)
                {
                    metka = item.Add(filterNames[i]);
                    if (metka) break;
                }

                if (!metka)
                {
                    filterPrototypes.Add(new FilterPrototype(filterNames[i]));
                }
            }
            return filterPrototypes;
        }

        public void UploadPrototype() 
        {
            if (PrefixDirectory.Meta.Type == BIMType.BIMStructure)
            {
                InSelectObserver.SelectedPrototype = PrefixDirectory.Meta.DeserializeXMLStructure();
            }
            else if (PrefixDirectory.Meta.Type == BIMType.BIMAssembly) 
            {
                InSelectObserver.SelectedPrototype = PrefixDirectory.Meta.DeserializeXML();
            }
        }

        public void SwapSelectedElement() 
        {
            InSelectObserver.Swap(PrefixDirectory.Meta.Type);
            //TODO: Выполнить реализацию.
        }

        
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion // INotifyPropertyChanged Members
    }
}
