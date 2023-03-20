using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Fusion;
using System.Collections.ObjectModel;
using PrototypeConductor.ViewModel;
using BIMPropotype_Lib.ViewModel;
using PrototypeConductor.Controller;
using TSM = Tekla.Structures.Model;
using UI = Tekla.Structures.Model.UI;
using BIMPropotype_Lib.Controller;
using BIMPropotype_Lib.ExtentionAPI.InserPlugin;
using PrototypeObserver;
using PrototypeConductor.Model;

namespace Propotype_Manage.ViewConductor
{
    public class ConductorViewModel : Fusion.ViewModel
    {

        private bool _isAllOpen;

        public bool IsAllOpen
        {
            get { return this._isAllOpen; }
            set { this.SetValue(ref this._isAllOpen, value); }
        }
        private ObservableCollection<ModelDirectoryViewModel> _conductor;
        public ObservableCollection<ModelDirectoryViewModel> Conductor
        {
            get { return this._conductor; }
            private set { this.SetValue(ref this._conductor, value); }
        }
        private Database _database;

        public Database Database
        {
            get { return this._database; }
            private set { this.SetValue(ref this._database, value); }
        }

        public ConductorViewModel(PrefixDirectory InPrefixDirectory, SelectObserver selectObserver)
        {
            Database = new Database(InPrefixDirectory, selectObserver);
            _conductor = new ObservableCollection<ModelDirectoryViewModel>(
                  (from directory in Database.GetModelDirectories()
                   select new ModelDirectoryViewModel(directory, Database))
                  .ToList());
        }
               

        /// <summary>
        /// Вставка деталей в модель.
        /// </summary>
        [CommandHandler]
        public void Refresh()
        {
            _conductor.Clear();
            foreach (var directory in Database.GetModelDirectories())
            {
                _conductor.Add(new ModelDirectoryViewModel(directory, Database));
            }
        }

        /// <summary>
        /// Сериализовать выбранные сборки.
        /// </summary>
        [CommandHandler]
        public void AddAssemblys()
        {
            TSM.ModelObjectEnumerator modelEnum = new UI.ModelObjectSelector().GetSelectedObjects();
            while (modelEnum.MoveNext())
            {
                if (modelEnum.Current is TSM.Assembly assemblyModel)
                {
                    new BeamLoader(assemblyModel, Database.PrefixDirectory);
                    AddDirect();
                }
            }
        }

        
        /// <summary>
        /// Сериализовать выбранные сборки.
        /// </summary>
        [CommandHandler]
        public void AddStructure()
        {
            TSM.ModelObjectEnumerator modelEnum = new UI.ModelObjectSelector().GetSelectedObjects();
            List<TSM.Assembly> structure = new List<TSM.Assembly>();
            while (modelEnum.MoveNext())
            {
                if (modelEnum.Current is TSM.Assembly assemblyModel)
                {
                    structure.Add(assemblyModel);
                }
            }
            new BeamLoader(Database.PrefixDirectory).SerializeXMLStructure(structure);
            AddDirect();
        }

        private void AddDirect()
        {
            Conductor[0].IsExpanded = true;
            var direct = SearcherDirect(Conductor.ToList<TreeViewItemViewModel>());

            if (direct == null)
            {
                Conductor[0].Children.Add(new FieldPrototypeViewModel(new FieldPrototype(Database.PrefixDirectory.FieldName, Database.PrefixDirectory.GetDirectory()), Conductor[0], Database));
                direct = Conductor[0].Children.Last();                  
            }

            direct.IsExpanded = true;
            var selectedpropotype = SearcherPrefix(Conductor.ToList<TreeViewItemViewModel>());
            if (selectedpropotype == null)
            {
                bool metka = false;
                var prototypeName = new PrototypeName(Database.PrefixDirectory.Prefix);

                foreach (var item in direct.Children)
                {
                    if (item is PrototypeNameViewModel prototypeNameViewModel)
                    {
                        metka = prototypeNameViewModel._prototypeName.Add(prototypeName);

                        if (metka)
                        {
                            prototypeNameViewModel.Children.Add(new PrototypeNameViewModel(new FilterPrototype(prototypeName), prototypeNameViewModel, Database));
                            prototypeNameViewModel.Children.Last().IsSelected = true;
                            break;
                        }
                    }
                }

                if (!metka)
                {
                    direct.Children.Add(new PrototypeNameViewModel(new FilterPrototype(prototypeName), direct, Database));
                    direct.Children.Last().IsSelected = true;
                }
            }
            else
            {
                selectedpropotype.IsSelected = true;
            }
        }

        /// <summary>
        /// Вставка деталей в модель.
        /// </summary>
        [CommandHandler]
        public void Delete(object SelectedItem)
        {
            if (SelectedItem is PrototypeNameViewModel prototypeName)
            {
                var msg = this.Host.UI.ShowMessageDialog($"Вы хотите удалить {prototypeName.Prefix}?", "Message", icon: "Geometry.RecycleBin", new string[] { "Delete", "Сancel" });
                if (msg == "Delete")
                {
                    var path = Database.PrefixDirectory.GetFile();
                    if (File.Exists(path)) File.Delete(path);
                    var parect = prototypeName.Parent;
                    parect.Children.Remove(prototypeName);
                    if (parect.Children.Count > 0)
                    {
                        parect.Children.Last().IsSelected = true;
                    }
                }
            }
            else if (SelectedItem is FieldPrototypeViewModel fieldPrototype)
            {
                if (fieldPrototype.Children.Count == 0)
                {
                    var path = Database.PrefixDirectory.GetDirectory();
                    if (Directory.Exists(path)) Directory.Delete(path);
                    fieldPrototype.Parent.Children.Remove(fieldPrototype);
                }
            }

            var selected = Searcher(Conductor.ToList<TreeViewItemViewModel>());
            if (selected != null)
            {

                if (CheckPrototype(selected))
                {
                    
                }
                if (CheckFoulder(selected))//Пустая папка.
                {
                    var path = Database.PrefixDirectory.GetDirectory();
                    if (Directory.Exists(path)) Directory.Delete(path);
                    selected.Parent.Children.Remove(selected);
                }
            }
        }
        /// <summary>
        /// Вставка деталей в модель.
        /// </summary>
        [CommandHandler]
        public void InsertPlugin()
        {
            var selected = Searcher(Conductor.ToList<TreeViewItemViewModel>());
            if (selected != null)
            {
                Database.PrefixDirectory.InsetPlugin();
            }
        }

        private bool CheckFoulder(TreeViewItemViewModel treeViewItemViewModel) 
        {
            if (treeViewItemViewModel is FieldPrototypeViewModel field)
            {
                if (field.Children?.Count == 0)
                {
                    return true;
                }
            }
            return false;
        }
        private bool CheckPrototype(TreeViewItemViewModel treeViewItemViewModel)
        {
            if (treeViewItemViewModel is PrototypeNameViewModel prototype)
            {
                return true;
            }
            return false;
        }

        private TreeViewItemViewModel Searcher(List<TreeViewItemViewModel> conductor) 
        {
            foreach (var itemConductor in conductor)
            {
                if(itemConductor.IsSelected) return itemConductor;
                else
                {
                    if (itemConductor.Children?.Count > 0)
                    {
                        var item = Searcher(itemConductor.Children.ToList<TreeViewItemViewModel>());
                        if (item != null) return item;
                    }
                }
            }
            return null;
        }

        private TreeViewItemViewModel SearcherDirect(List<TreeViewItemViewModel> conductor)
        {
            foreach (var itemConductor in conductor)
            {
                if (itemConductor is FieldPrototypeViewModel field)
                {
                    if (field.Name == Database.PrefixDirectory.FieldName) return field;
                    
                }

                if (itemConductor.Children?.Count > 0)
                {
                    var item = SearcherDirect(itemConductor.Children.ToList<TreeViewItemViewModel>());
                    if (item != null) return item;
                }
            }
            return null;
        }

        private TreeViewItemViewModel SearcherPrefix(List<TreeViewItemViewModel> conductor)
        {
            foreach (var itemConductor in conductor)
            {
                if (itemConductor is PrototypeNameViewModel propotype)
                {
                    if (propotype.Prefix == Database.PrefixDirectory.Prefix) return propotype;
                }

                if (itemConductor.Children?.Count > 0)
                {
                    var item = SearcherPrefix(itemConductor.Children.ToList<TreeViewItemViewModel>());
                    if (item != null) 
                    {
                        itemConductor.IsExpanded = true;
                        return item;
                    }
                }
            }
            return null;
        }
    }
}
