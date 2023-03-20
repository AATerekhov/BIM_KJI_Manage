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
        public JointViewModel(BIMJoint joint, TreeViewItemViewModel parentDirectory, ContainerForSelected containerForSelected)
            : base(parentDirectory, false)
        {
            _bIMJoint = joint;
            InContainerForSelected = containerForSelected;
            this.PropertyChanged += PrototypeNameViewModel_PropertyChanged;
            LoadChildren();
        }
        public string Name
        {
            get { return _bIMJoint.Name; }
            set { _bIMJoint.Name = value;}
        }

        public string Number
        {
            get { return _bIMJoint.Prefix; }
            set { _bIMJoint.Prefix = value; }
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
            return $"{Name}_{Number}";
        }
    }
}
