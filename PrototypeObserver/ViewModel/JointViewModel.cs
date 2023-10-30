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
    public class JointViewModel : TreeViewItemViewModel
    {
        public BIMJoint _bIMJoint { get; set; }
        public ContainerForSelected InContainerForSelected { get; set; }
        public JointViewModel(BIMPartChildren joint, TreeViewItemViewModel parentDirectory, ContainerForSelected containerForSelected)
            : base(parentDirectory, false)
        {
            _bIMJoint = joint.Joint;
            InContainerForSelected = containerForSelected;
            this.PropertyChanged += PrototypeNameViewModel_PropertyChanged;
            LoadChildren();
        }
        public string Name
        {
            get { return _bIMJoint.ToString(); }
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

        protected override void LoadChildren()
        {
            foreach (BIMPartChildren assembly in _bIMJoint.OwnChildren.GetAssembly())
                base.Children.Add(new AssemblyViewModel(assembly.Assembly, this, InContainerForSelected));
            foreach (BIMPartChildren boolean in _bIMJoint.OwnChildren.GetBoolean())
                base.Children.Add(new ChildrenPartViewModel(boolean, this, InContainerForSelected));
            foreach (BIMPartChildren reinforcement in _bIMJoint.OwnChildren.GetReinforcement())
                base.Children.Add(new ChildrenPartViewModel(reinforcement, this, InContainerForSelected));
            foreach (BIMPartChildren bolts in _bIMJoint.OwnChildren.GetBolts())
                base.Children.Add(new ChildrenPartViewModel(bolts, this, InContainerForSelected));
            foreach (BIMPartChildren joints in _bIMJoint.OwnChildren.GetJoints())
                base.Children.Add(new JointViewModel(joints, this, InContainerForSelected));

            foreach (BIMPart partBox in _bIMJoint.Children)
            {
                base.Children.Add(new PartViewModel(partBox, this, InContainerForSelected));
            }

        }
        public override CoordinateSystem GetBase()
        {
            return _bIMJoint.BaseStructure;
        }
        public override void SetBase(CoordinateSystem coordinateSystem)
        {
            _bIMJoint.BaseStructure = coordinateSystem;
        }

        public override string ToString()
        {
            return _bIMJoint.ToString();
        }
        public override void Insert(Model model)
        {
            WorkPlaneWorker workPlaneWorker = new WorkPlaneWorker(model);
            workPlaneWorker.GetWorkPlace(_bIMJoint.BaseStructure);

            foreach (var item in Children)
            {
                if (item is ChildrenPartViewModel childrenPartVM) childrenPartVM.Insert(model);
                else if (item is PartViewModel partViewModel) partViewModel.Insert(model);
            }

            workPlaneWorker.ReturnWorkPlace();
        }
    }
}
