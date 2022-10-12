﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIMPropotype_Lib.Model;

namespace PrototypeObserver.ViewModel
{
    public class PartViewModel : TreeViewItemViewModel
    {
        public BIMPart _bIMPart;
        public ContainerForSelected InContainerForSelected { get; set; }
        public PartViewModel(BIMPart part, TreeViewItemViewModel parentDirectory, ContainerForSelected containerForSelected)
            : base(parentDirectory, true)
        {
            _bIMPart = part;
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
        public string Name
        {
            get { return _bIMPart.Name; }
        }

        protected override void LoadChildren()
        {
            foreach (BIMAssembly assembly in _bIMPart.PutInAssembly)
                base.Children.Add(new AssemblyViewModel(assembly, this, InContainerForSelected));
        }

    }
}