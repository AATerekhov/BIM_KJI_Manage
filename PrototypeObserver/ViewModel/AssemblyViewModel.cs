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

namespace PrototypeObserver.ViewModel
{
    public class AssemblyViewModel : TreeViewItemViewModel
    {
        public BIMAssembly _bIMAssembly;
        public ContainerForSelected InContainerForSelected { get; set; }

        public AssemblyViewModel(BIMAssembly assembly, TreeViewItemViewModel parentField, ContainerForSelected containerForSelected)
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
            get { return _bIMAssembly.Prefix; }
        }

        protected override void LoadChildren()
        {
            foreach (BIMPart partBox in _bIMAssembly.Children)
            {
                base.Children.Add(new PartViewModel(partBox, this, InContainerForSelected)); 
            }
        }

        public override void Insert()
        {
            foreach (var item in Children)
            {
                if (item is PartViewModel assemblyVM) assemblyVM.Insert();
            }   

            if (Parent == null)
            {
                _bIMAssembly.Insert(null);
            }
            else
            {
                _bIMAssembly.Insert((Parent as PartViewModel)._bIMPart);
            }
        }

        public void InsertNotFather() 
        {
            foreach (var item in Children)
            {
                if (item is PartViewModel assemblyVM) assemblyVM.Insert();
            }
            _bIMAssembly.Insert(null);
        }

        public override void InsertMirror()
        {
            foreach (var item in Children)
            {
                if (item is PartViewModel assemblyVM) assemblyVM.InsertMirror();
            }

            if (Parent == null)
            {
                _bIMAssembly.InsertMirror(null);
            }
            else
            {
                _bIMAssembly.InsertMirror((Parent as PartViewModel)._bIMPart);
            }
        }

    }
}
