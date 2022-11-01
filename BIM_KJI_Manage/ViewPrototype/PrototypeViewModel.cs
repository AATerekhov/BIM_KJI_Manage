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
        public event Action<BIMAssembly> UploadPrototypeSelect;
        public ObservableCollection<AssemblyViewModel> Propotypes { get; set; }
        public ContainerForSelected InContainerForSelected { get; set; }
        public SelectObserver InSelectObserver { get; set; }

        private bool _isGlobal;
        public bool IsGlobal
        {
            get { return this._isGlobal; }
            set { this.SetValue(ref this._isGlobal, value); }
        }

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

        [CommandHandler]
        public void CreatePrototype()
        {
            if (InContainerForSelected.SelectedElement != null)
            {
                if (InContainerForSelected.SelectedElement is AssemblyViewModel assemblyViewModel)
                {
                    CreatePrototypeSelect?.Invoke(assemblyViewModel._bIMAssembly);
                }
            }
        }

        [CommandHandler]
        public void UpdatePrototype()
        {

            if (InContainerForSelected.SelectedElement != null)
            {
                if (InContainerForSelected.SelectedElement is AssemblyViewModel assemblyViewModel)
                {
                    UploadPrototypeSelect?.Invoke(assemblyViewModel._bIMAssembly); 
                   
                }
                if (InContainerForSelected.SelectedElement.GetOldFather() is AssemblyViewModel assemblyFatherViewModel)
                {
                    ModifyBIMAssemblySelect?.Invoke(assemblyFatherViewModel._bIMAssembly);
                }
            }
        }

    }
}
