using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIMPropotype_Lib.Model;
using Tekla.Structures.Model;
using BIMPropotype_Lib.ExtentionAPI.Mirror;
using BIMPropotype_Lib.ExtentionAPI.InserPlugin;
using BIMPropotype_Lib.ExtentionAPI.PartChildren;
using BIMPropotype_Lib.Controller;
using Tekla.Structures.Geometry3d;
using BIMPropotype_Lib.ViewModel;
using System.IO;
using System.Xml.Serialization;
using BIMPropotype_Lib.ExtentionAPI.Conductor;

namespace PrototypeObserver.ViewModel
{
    public class AssemblyViewModel : TreeViewItemViewModel, ILink
    {
        public BIMAssembly _bIMAssembly;
        public PrefixDirectory InPrefixDirectory { get; set; }
        public ContainerForSelected InContainerForSelected { get; set; }

        public AssemblyViewModel(BIMAssembly assembly, TreeViewItemViewModel parentField, ContainerForSelected containerForSelected )
           : base(parentField, false)
        {
            _bIMAssembly = assembly;
            InContainerForSelected = containerForSelected;
            this.PropertyChanged += PrototypeNameViewModel_PropertyChanged;
            LoadChildren();
        }

        public AssemblyViewModel(BIMAssembly assembly, ContainerForSelected containerForSelected) 
            : this(assembly, null, containerForSelected)
        {

        }
        private void PrototypeNameViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                if ((sender as TreeViewItemViewModel).IsSelected)
                {
                    InContainerForSelected.SelectedElement = sender as TreeViewItemViewModel;
                }
            }
        }

        public string Name
        {
            get { return _bIMAssembly.ToString(); }
        }

        public bool IsLinq
        {
            get { return _bIMAssembly.IsLink; }
            set
            {
                if (value)
                {
                    Clining();
                }

                _bIMAssembly.IsLink = value;                
                this.OnPropertyChanged("IsLinq");
            }
        }

        private void Clining()
        {
            _bIMAssembly.Children.Clear();
            base.Children.Clear();
        }

        protected override void LoadChildren()
        {
            foreach (BIMPart partBox in _bIMAssembly.Children)
            {
                base.Children.Add(new PartViewModel(partBox, this, InContainerForSelected)); 
            }
        }

        public override CoordinateSystem GetBase()
        {
            return _bIMAssembly.BaseStructure;
        }
        public override void SetBase(CoordinateSystem coordinateSystem)
        {
            _bIMAssembly.BaseStructure = coordinateSystem;
        }
        public override void Insert(Model model)
        {
            WorkPlaneWorker workPlaneWorker = new WorkPlaneWorker(model);
            workPlaneWorker.GetWorkPlace(_bIMAssembly.BaseStructure);
            if (IsLinq)
            {
                var copy = _bIMAssembly.Meta.DeserializeXML();
                _bIMAssembly.Children = copy.Children;
                this.LoadChildren();
            }

            foreach (var item in Children)
            {
                if (item is PartViewModel assemblyVM) assemblyVM.Insert(model);
            }
            if (Parent == null)
            {
                _bIMAssembly.Insert(null);
            }
            else if (Parent is StructureViewModel)
            {
                _bIMAssembly.Insert(null);
            }
            else
            {
                _bIMAssembly.Insert((Parent as PartViewModel)._bIMPart);
            }


            workPlaneWorker.ReturnWorkPlace();
            if (IsLinq) Clining();            
        }

        public void InsertNotFather(Model model)
        {
            if (IsLinq)
            {
                var copy = _bIMAssembly.Meta.DeserializeXML();
                _bIMAssembly.Children = copy.Children;
                this.LoadChildren();
            }

            foreach (var item in Children)
            {
                if (item is PartViewModel assemblyVM) assemblyVM.Insert(model);
            }

            _bIMAssembly.Insert(null);

            if (IsLinq) Clining();
        }

        public override void InsertMirror()
        {
            //foreach (var item in Children)
            //{
            //    if (item is PartViewModel assemblyVM) assemblyVM.InsertMirror();
            //}

            //if (Parent == null)
            //{
            //    _bIMAssembly.InsertMirror(null);
            //}
            //else
            //{
            //    _bIMAssembly.InsertMirror((Parent as PartViewModel)._bIMPart);
            //}
        }
        public void SelectInModel()
        {
            if(!IsLinq)
            _bIMAssembly.SelectInModel();
        }

        public void Swap(MetaDirectory meta)
        {
            //TODO: Отладить данный метод на Meta!

            //InPrefixDirectory.Meta = meta;

            //this.DeserializeXMLNotBase();
            //Children.Clear();
            //LoadChildren();
            //this.OnPropertyChanged("Name");
        }

        //TODO: Требуется отладка метода. Не реализован алгоритм работы.
        public void AddJoint()
        {
            IsExpanded = true;
            var joint = new BIMJoint();
            joint.GetStarted();
            base.Children.Add(new JointViewModel(joint, this, InContainerForSelected));
            base.Children.Last().IsSelected = true;
        }

        public void DeserializeXML()
        {
            throw new NotImplementedException();
        }
        public void SerializeXML()
        {
            _bIMAssembly.SerializeXML();
        }
        public void DeserializeXMLNotBase()
        {
            //UpdateDataDirectory(BIMType.BIMAssembly);
            //var path = InPrefixDirectory.GetFile();
            //if (File.Exists(path))
            //{
            //    var formatter = new XmlSerializer(typeof(BIMAssembly));

            //    using (var fs = new FileStream(path, FileMode.OpenOrCreate))
            //    {
            //        var rezult = (BIMAssembly)formatter.Deserialize(fs);
            //        this.CloneAsCurrentObjectNotBase(rezult);
            //    }
            //}
        }
        public void UpdateMetaDirectory(MetaDirectory meta)
        {
            InPrefixDirectory.Meta = meta;
        }

    }
}
