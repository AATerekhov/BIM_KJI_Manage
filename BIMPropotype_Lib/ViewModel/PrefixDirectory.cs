using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;
using System.ComponentModel;
using BIMPropotype_Lib.ExtentionAPI.Conductor;
using BIMPropotype_Lib.Model;

namespace BIMPropotype_Lib.ViewModel
{
    /// <summary>
    /// Инструкция хранения типов файлов.
    /// </summary>
    public class PrefixDirectory : INotifyPropertyChanged
    {
        private BIMType _source;
        private IEnumerable<BIMType> _listSources;
        
        public BIMType Source
        {
            get { return _source; }
            set
            {
                _source = value;
                if (Meta != null) Meta.Type = value;
                this.OnPropertyChanged("Source");
            }
        }
        public IEnumerable<BIMType> ListSources
        {
            get { return this._listSources; }
            set
            {
                _listSources = value;
                this.OnPropertyChanged("ListSources");
            }
        }
        private int _number;
        private string _prefix;
        private string _postPrefix;
        private string _name;

        public int Number
        {
            get { return _number; }
            set
            {
                _number = value;
                if (Meta != null) Meta.Number = value;
                this.OnPropertyChanged("Number");
            }
        }
        public string Prefix
        {
            get { return _prefix; }
            set
            {
                _prefix = value;
                if (Meta != null) Meta.Prefix = value;
                this.OnPropertyChanged("Prefix");
             }
        }

        public string PostPrefix
        {
            get { return _postPrefix; }
            set
            {
                _postPrefix = value;
                if (Meta != null) Meta.PostPrefix = value;
                this.OnPropertyChanged("PostPrefix");
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                if (Meta != null) Meta.Name = value;
                this.OnPropertyChanged("Name");
            }
        }
        private string _comment;

        public string Сomment
        {
            get { return _comment; }
            set
            {
                _comment = value;
                if (Meta != null) Meta.Сomment = value;
                this.OnPropertyChanged("Сomment");
            }
        }

        public MetaDirectory Meta { get; set; }

        private string _modelDirectory;
        public string ModelDirectory
        {
            get { return _modelDirectory; }
            set
            {
                _modelDirectory = value;
                this.OnPropertyChanged("ModelDirectory");
                SevaModelDirectory();
            }
        }
        public TSM.Model Model { get; set; }
        public TSM.ProjectInfo ProjectlInfo { get; set; }
        public TSM.ModelInfo ModelInfo { get; set; }

        public PrefixDirectory() 
        {

            OpenModel(new TSM.Model());
        }
        
        public PrefixDirectory(TSM.Model model)
        {
            OpenModel(model);
        }

        private void OpenModel(TSM.Model model)
        {
            if (model.GetConnectionStatus())
            {
                Model = model;
                GetPath(model);
                Meta = new MetaDirectory();
                var listSources = Enum.GetValues(typeof(BIMType)).Cast<BIMType>().ToList();
                listSources.Remove(BIMType.Error);
                ListSources = listSources;
            }
        }

        public void GetPath(TSM.Model model)
        {
            ProjectlInfo = model.GetProjectInfo();
            string path = string.Empty;
            ProjectlInfo.GetUserProperty("PROJECT_USERFIELD_1", ref path);
            ModelDirectory = path;

            ModelInfo = model.GetInfo();
        }

        private void SevaModelDirectory()
        {
            ProjectlInfo.SetUserProperty("PROJECT_USERFIELD_1", _modelDirectory);
        }

        public string GetFile() => Meta.GetFilePath();

        public string[] GetFields(string directopy)
        {
            if (!Directory.Exists(directopy)) Directory.CreateDirectory(directopy);
            var vs = Directory.GetDirectories(directopy);
            return vs;
        }

        public string[] GetFiles(string directopy)
        {
            var vs = Directory.GetFiles(directopy);
            return vs;
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
    public enum BIMType
    {
        BIMAssembly = 0,
        BIMStructure = 1,
        BIMJoint = 2,
        Error = 10,
    }
}
