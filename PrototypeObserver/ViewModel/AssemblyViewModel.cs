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
using System.Collections.ObjectModel;

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

            DynamicProperties = new ObservableCollection<DynamicProperty>();

            LoadChildren();
        }


        public AssemblyViewModel(BIMAssembly assembly, ContainerForSelected containerForSelected) 
            : this(assembly, null, containerForSelected)
        {

        }
        protected override void LoadChildren()
        {
            foreach (BIMPart partBox in _bIMAssembly.Children)
            {
                base.Children.Add(new PartViewModel(partBox, this, InContainerForSelected));
            }

            if (Children.Count > 0)
                (Children[0] as PartViewModel).IsMainPart = true;

            DynamicProperties.Clear();
            var distance = _bIMAssembly.CheckDistance();
            if (distance != 0.0)
            {
                DynamicProperties.Add(new DynamicProperty(distance));
                DynamicProperties[0].PropertyChanged += AssemblyViewModel_PropertyChanged;
            }
        }

        private void AssemblyViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!IsLinq)
            {
                if (e.PropertyName == "BIMProperty")
                {
                    _bIMAssembly.ModifyDistance((sender as DynamicProperty).BIMProperty);
                }
            }            
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
        public bool IsBeam
        {
            get 
            {
                if (_bIMAssembly.Children.Count >0)
                {
                    if (_bIMAssembly.Children[0].Type == PartType.beam) return true;
                }
                return false;
            } 
        }

        private void Clining()
        {
            _bIMAssembly.Children.Clear();
            base.Children.Clear();
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

        public void SelectInModel()
        {
            if(!IsLinq)
            _bIMAssembly.SelectInModel();
        }
        public void SerializeXML()
        {
            _bIMAssembly.SerializeXML();
        }
        public void AddJoint()
        {
            if (IsBeam)
            {
                _bIMAssembly.TransformationОfТodes();
                base.Children.Clear();
                this.LoadChildren();
            }
        }

        public override void InsertMirror()
        {
        }

        public void Swap(MetaDirectory meta)
        {
        }


        public void DeserializeXML()
        {
        }
        public void DeserializeXMLNotBase()
        {
        }
        public void UpdateMetaDirectory(MetaDirectory meta)
        {

        }

    }
}
