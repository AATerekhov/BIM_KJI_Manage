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
using PrototypeObserver;

namespace Propotype_Manage.ViewPrototype
{
    public class PrototypeViewModel : Fusion.ViewModel
    {
        public event Action<BIMAssembly> ModifyBIMAssemblySelect;
        public event Action<BIMAssembly> CreatePrototypeSelect;
        public ObservableCollection<AssemblyViewModel> Propotypes { get; set; }
        public ContainerForSelected InContainerForSelected { get; set; }
        public SelectObserver InSelectObserver { get; set; }

        public void GetPropotypes(BIMAssembly bIMAssemblySelect) 
        {
            if (bIMAssemblySelect != null)
            {
                Propotypes.Clear();
                InContainerForSelected.UDAs.Clear();
                Propotypes.Add(new AssemblyViewModel(bIMAssemblySelect, InContainerForSelected));
            }
        }

        public PrototypeViewModel(SelectObserver selectObserver)
        {
            InSelectObserver = selectObserver;
            InSelectObserver.NewSelectPrototype += InSelectObserver_NewSelectPrototype;
            Propotypes = new ObservableCollection<AssemblyViewModel>();
            InContainerForSelected = new ContainerForSelected();
            InContainerForSelected.ModifyAndSaveEvent += InContainerForSelected_ModifyAndSaveEvent1;
            InContainerForSelected.CreatePrototype += InContainerForSelected_CreatePrototype;

        }

        private void InContainerForSelected_CreatePrototype(PrototypeObserver.ViewModel.TreeViewItemViewModel obj)
        {
            if (obj != null)
            {
                if (obj is AssemblyViewModel assemblyViewModel)
                {
                    CreatePrototypeSelect?.Invoke(assemblyViewModel._bIMAssembly);
                }
            }
        }

        private void InSelectObserver_NewSelectPrototype(BIMAssembly obj)
        {
            GetPropotypes(obj);
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
