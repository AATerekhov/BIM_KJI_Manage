using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Fusion;
using System.Collections.ObjectModel;
using PrototypeConductor.ViewModel;
using BIMPropotype_Lib.ViewModel;
using PrototypeConductor.Controller;
using TSM = Tekla.Structures.Model;
using UI = Tekla.Structures.Model.UI;
using BIMPropotype_Lib.Model;
using PrototypeObserver.ViewModel;

namespace Propotype_Manage.ViewPrototype
{
    public class PrototypeViewModel : Fusion.ViewModel
    {
        public event Action<BIMAssembly> ModifyBIMAssemblySelect;
        public ObservableCollection<AssemblyViewModel> Propotypes { get; set; }
        public ContainerForSelected InContainerForSelected { get; set; }


        public void GetPropotypes(BIMAssembly bIMAssemblySelect) 
        {
            if (bIMAssemblySelect != null)
            {
                Propotypes.Clear();
                InContainerForSelected.UDAs.Clear();
                Propotypes.Add(new AssemblyViewModel(bIMAssemblySelect, InContainerForSelected));
            }
        }

        public PrototypeViewModel()
        {
            Propotypes = new ObservableCollection<AssemblyViewModel>();
            InContainerForSelected = new ContainerForSelected();
            InContainerForSelected.ModifyAndSaveEvent += InContainerForSelected_ModifyAndSaveEvent1;
        }

        private void InContainerForSelected_ModifyAndSaveEvent1(PrototypeObserver.ViewModel.TreeViewItemViewModel obj)
        {
            if (obj != null)
            {
                if (obj.GetOldFather() is AssemblyViewModel assemblyViewModel)
                {
                    ModifyBIMAssemblySelect?.Invoke(assemblyViewModel._bIMAssembly);
                }
            }
        }
    }
}
