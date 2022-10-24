using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIMPropotype_Lib.Model;
using BIMPropotype_Lib.Model.Custom;
using BIMPropotype_Lib.ExtentionAPI.PartChildren;

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
        }

        public override void Insert()
        {
            _bIMPartChildren.Insert((Parent as PartViewModel)._bIMPart);
        }

        public override void InsertMirror()
        {
            _bIMPartChildren.InsertMirror((Parent as PartViewModel)._bIMPart);
        }
    }
}
