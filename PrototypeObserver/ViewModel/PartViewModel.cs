using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;
using BIMPropotype_Lib.Model;
using BIMPropotype_Lib.ExtentionAPI.PartChildren;
using BIMPropotype_Lib.Controller;
using Tekla.Structures.Geometry3d;

namespace PrototypeObserver.ViewModel
{
    public class PartViewModel : TreeViewItemViewModel
    {
        public BIMPart _bIMPart;
        public ContainerForSelected InContainerForSelected { get; set; }
        public PartViewModel(BIMPart part, TreeViewItemViewModel parentDirectory, ContainerForSelected containerForSelected)
            : base(parentDirectory, false)
        {
            _bIMPart = part;
            InContainerForSelected = containerForSelected;
            this.PropertyChanged += PrototypeNameViewModel_PropertyChanged;
            LoadChildren();
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
            get { return _bIMPart.GetPart().Name; }
        }

        protected override void LoadChildren()
        {
            foreach (BIMPartChildren assembly in _bIMPart.Children.GetAssembly())
                base.Children.Add(new AssemblyViewModel(assembly.Assembly, this, InContainerForSelected));
            foreach (BIMPartChildren boolean in _bIMPart.Children.GetBoolean())
                base.Children.Add(new ChildrenPartViewModel(boolean, this, InContainerForSelected));
            foreach (BIMPartChildren reinforcement in _bIMPart.Children.GetReinforcement())
                base.Children.Add(new ChildrenPartViewModel(reinforcement, this, InContainerForSelected));
            foreach (BIMPartChildren bolts in _bIMPart.Children.GetBolts())
                base.Children.Add(new ChildrenPartViewModel(bolts, this, InContainerForSelected));
            foreach (BIMPartChildren joints in _bIMPart.Children.GetJoints())
                base.Children.Add(new JointViewModel(joints, this, InContainerForSelected));
        }

        public override void Insert(Model model)
        {
            WorkPlaneWorker workPlaneWorker = new WorkPlaneWorker(model);
            workPlaneWorker.GetWorkPlace(_bIMPart.BaseStructure);

            _bIMPart.Insert((this.GetAssemblyFather())._bIMAssembly);

            foreach (var item in Children)
            {
                if (item is AssemblyViewModel assemblyVM) assemblyVM.Insert(model);
                else if (item is ChildrenPartViewModel childrenPartVM) childrenPartVM.Insert(model);
                else if (item is JointViewModel jointViewModel) jointViewModel.Insert(model);
            }
            workPlaneWorker.ReturnWorkPlace();
        }
        private bool _isMainPart;

        public bool IsMainPart
        {
            get { return _isMainPart; }
            set 
            {
                _isMainPart = value;
                this.OnPropertyChanged("IsMainPart");
            }
        }


        public override CoordinateSystem GetBase()
        {
            return _bIMPart.BaseStructure;
        }
        public override void SetBase(CoordinateSystem coordinateSystem)
        {
            _bIMPart.BaseStructure = coordinateSystem;
        }

        public override void InsertMirror()
        {
            _bIMPart.InsertMirror((Parent as AssemblyViewModel)._bIMAssembly);
            foreach (var item in Children)
            {
                if (item is AssemblyViewModel assemblyVM) assemblyVM.InsertMirror();
                else if (item is ChildrenPartViewModel childrenPartVM) childrenPartVM.InsertMirror();

            }
        }

        public void SelectInModel()
        {
            _bIMPart.SelectInModel();
        }
    }
}
