using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;
using BIMPropotype_Lib.Model;
using BIMPropotype_Lib.Model.Custom;
using BIMPropotype_Lib.ExtentionAPI.PartChildren;
using BIMPropotype_Lib.Controller;
using Tekla.Structures.Geometry3d;

namespace PrototypeObserver.ViewModel
{
    public class ChildrenPartViewModel : TreeViewItemViewModel
    {
        public BIMPartChildren _bIMPartChildren;
        public ContainerForSelected InContainerForSelected { get; set; }
        public ChildrenPartViewModel(BIMPartChildren bIMPartChildren, TreeViewItemViewModel parentDirectory, ContainerForSelected containerForSelected)
            : base(parentDirectory, false)
        {
            _bIMPartChildren = bIMPartChildren;
            InContainerForSelected = containerForSelected;
            this.PropertyChanged += PrototypeNameViewModel_PropertyChanged;
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

        public override void Insert(Model model)
        {
            WorkPlaneWorker workPlaneWorker = new WorkPlaneWorker(model);
            workPlaneWorker.GetWorkPlace(_bIMPartChildren.BaseStructure);

            _bIMPartChildren.Insert((Parent as PartViewModel)._bIMPart);

            workPlaneWorker.ReturnWorkPlace();
        }

        public override CoordinateSystem GetBase()
        {
            return _bIMPartChildren.BaseStructure;
        }

        public override void SetBase(CoordinateSystem coordinateSystem)
        {
            _bIMPartChildren.BaseStructure = coordinateSystem;
        }

        public override void InsertMirror()
        {
            _bIMPartChildren.InsertMirror((Parent as PartViewModel)._bIMPart);
        }

        public override string ToString()
        {
            return _bIMPartChildren.ToString();
        }
    }
}
