using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;
using System.ComponentModel;

namespace BIMPropotype_Lib.ViewModel
{
    public class PrefixDirectory : INotifyPropertyChanged
    {
        const string addModelDirectory = "RCP_Data\\Prototype";
        const string extention = ".xml";
        public string typeFile = "prot_";

        private string _pathThisModel;

        public string PathThisModel
        {
            get { return _pathThisModel; }
            set { _pathThisModel = value;
                this.OnPropertyChanged("PathThisModel");
            }
        }

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
        private string _fieldName;

        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value;
                this.OnPropertyChanged("FieldName");
            }
        }
        private string _prefix;

        public string Prefix
        {
            get { return _prefix; }
            set { _prefix = value;
                this.OnPropertyChanged("Prefix");
            }
        }
        public TSM.ProjectInfo ProjectlInfo { get; set; }
        public TSM.ModelInfo ModelInfo { get; set; }

        public PrefixDirectory() { }
        public PrefixDirectory(TSM.Model model) 
        {
            GetPath(model);
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
            ProjectlInfo.SetUserProperty("PROJECT_USERFIELD_1",_modelDirectory);
        }
        public string GetFile() 
        {
            if (!Directory.Exists(this.GetDirectory())) Directory.CreateDirectory(this.GetDirectory());
            return Path.Combine(ModelDirectory, addModelDirectory, FieldName,$"{typeFile}{Prefix}{extention}" );
        }

        public string GetDirectory()
        {
            return Path.Combine(ModelDirectory, addModelDirectory, FieldName);
        }
        public string GetDataDirectory()
        {
            return Path.Combine(ModelDirectory, addModelDirectory);
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
