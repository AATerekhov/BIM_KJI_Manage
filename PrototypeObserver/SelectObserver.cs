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
using PrototypeObserver.Extentions;

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

        public void CreatePreset() 
        {
            if (InContainerForSelected.IsDynamic)
            {
                var count = InContainerForSelected.PresetElements.Count;
                var value = InContainerForSelected.SelectedElement.DynamicProperties[0].BIMProperty;

                var copy = InContainerForSelected.PresetElements.Where(x => x.Distance == value).FirstOrDefault();
                if (copy == null)
                {
                    InContainerForSelected.PresetElements.Add(new PresetElement(count + 1, value));
                    InContainerForSelected.SelectPreset = InContainerForSelected.PresetElements.Last();
                }
            }
        }
        public void SavePresets()
        {
            if (InContainerForSelected.IsDynamic)
            {
                if (InContainerForSelected.SelectedElement is AssemblyViewModel assemblyViewModel)
                {
                    var meta = assemblyViewModel._bIMAssembly.Meta;
                    meta.SerializeXMLPresets(InContainerForSelected.PresetElements.ToList());
                }  
            }
        }
        


        public SelectObserver()
        {

        }
    }
}
