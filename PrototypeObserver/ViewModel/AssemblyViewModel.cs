using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIMPropotype_Lib.Model;

namespace PrototypeObserver.ViewModel
{
    public class AssemblyViewModel : TreeViewItemViewModel
    {
        private BIMAssembly _bIMAssembly;
        public ContainerForSelected InContainerForSelected { get; set; }

        public AssemblyViewModel(BIMAssembly assembly, TreeViewItemViewModel parentField, ContainerForSelected containerForSelected)
           : base(parentField, true)
        {
            _bIMAssembly = assembly;
            InContainerForSelected = containerForSelected;
            this.PropertyChanged += PrototypeNameViewModel_PropertyChanged;
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
            get { return _bIMAssembly.Prefix; }
        }

        protected override void LoadChildren()
        {
            foreach (BIMPart part in _bIMAssembly.Elements)
            {
                base.Children.Add(new PartViewModel(part, this, InContainerForSelected)); 
            }

            foreach (BIMPart part in _bIMAssembly.Plates)
            {
                base.Children.Add(new PartViewModel(part, this, InContainerForSelected));
            }

            foreach (BIMPart part in _bIMAssembly.PolyBeams)
            {
                base.Children.Add(new PartViewModel(part, this, InContainerForSelected));
            }

        }
    }
}
