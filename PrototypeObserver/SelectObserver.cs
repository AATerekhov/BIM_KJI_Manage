using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using BIMPropotype_Lib.Model;
using PrototypeObserver.ViewModel;
using BIMPropotype_Lib.ViewModel;

namespace PrototypeObserver
{
    public class SelectObserver
    {
        public event Action<Reference> NewSelectPrototype;

        private Reference _selectedPrototype;
        public ContainerForSelected InContainerForSelected { get; set; }
        public Reference SelectedPrototype
        {
            get { return _selectedPrototype; }
            set 
            {
                _selectedPrototype = value;
                NewSelectPrototype?.Invoke(_selectedPrototype);
            }
        }
        public void CreatePrototype() 
        {
            if (InContainerForSelected.SelectedElement != null)
            {
                if (InContainerForSelected.SelectedElement is AssemblyViewModel assemblyViewModel)
                {
                    if (!assemblyViewModel.IsLinq)
                    {
                        assemblyViewModel.SerializeXML();
                    }
                }
                else if (InContainerForSelected.SelectedElement is StructureViewModel structureViewModel)
                {
                    structureViewModel.SerializeXML();
                }
            }
        }
        public void Swap(BIMType bIMType)
        {
            if (InContainerForSelected.SelectedElement != null)
            {
                if (bIMType == BIMType.BIMAssembly)
                {
                    if (InContainerForSelected.SelectedElement is AssemblyViewModel selectedAssemblyViewModel)
                    {
                        selectedAssemblyViewModel.Swap(selectedAssemblyViewModel._bIMAssembly.Meta);
                    }
                }
            }
        }

        public void Select()
        {
            if (InContainerForSelected.SelectedElement != null)
            {
                if (InContainerForSelected.SelectedElement is PartViewModel partViewModel)
                {
                    partViewModel.SelectInModel();
                }
                else if (InContainerForSelected.SelectedElement is AssemblyViewModel assemblyViewModel)
                {
                    assemblyViewModel.SelectInModel();
                }
            }
        }

        public SelectObserver()
        {

        }
    }
}
