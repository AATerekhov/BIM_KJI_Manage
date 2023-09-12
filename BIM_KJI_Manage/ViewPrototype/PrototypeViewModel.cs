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
using POVM = PrototypeObserver.ViewModel;
using PrototypeObserver;
using BIMPropotype_Lib.ExtentionAPI.Conductor;

namespace Propotype_Manage.ViewPrototype
{
    public class PrototypeViewModel : Fusion.ViewModel
    {
        public event Action<BIMAssembly> ModifyBIMAssemblySelect;
        //public event Action<BIMAssembly> CreatePrototypeSelect;
        public event Action<BIMAssembly> UploadPrototypeSelect;
        public ObservableCollection<POVM.TreeViewItemViewModel> Propotypes { get; set; }
        public SelectObserver InSelectObserver { get; set; }

        private bool _isGlobal;
        public bool IsGlobal
        {
            get { return this._isGlobal; }
            set { this.SetValue(ref this._isGlobal, value); }
        }


        public void GetPropotypes(Reference bIMReferenceObjectSelect) 
        {
            if (bIMReferenceObjectSelect != null)
            {
                Propotypes.Clear();
                InSelectObserver.InContainerForSelected.UDAs.Clear();
                if (bIMReferenceObjectSelect is BIMAssembly bIMAssemblySelect)
                {
                    Propotypes.Add(new AssemblyViewModel(bIMAssemblySelect, InSelectObserver.InContainerForSelected));
                }
                else if (bIMReferenceObjectSelect is BIMStructure bIMStructureSelect)
                {
                    Propotypes.Add(new StructureViewModel(bIMStructureSelect, InSelectObserver.InContainerForSelected));
                }

                Propotypes[0].IsSelected = true;
            }
        }

        public PrototypeViewModel(SelectObserver selectObserver)
        {
            InSelectObserver = selectObserver;
            InSelectObserver.NewSelectPrototype += InSelectObserver_NewSelectPrototype;
            Propotypes = new ObservableCollection<POVM.TreeViewItemViewModel>();
            InSelectObserver.InContainerForSelected = new ContainerForSelected();
        }

        /// <summary>
        /// Получение свойств выбранного элемента.
        /// </summary>
        /// <param name="obj"></param>
        private void InSelectObserver_NewSelectPrototype(Reference obj)
        {
            GetPropotypes(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        [CommandHandler]
        public void CreatePrototype()
        {
            InSelectObserver.CreatePrototype();
           
        }
        /// <summary>
        /// Выбрать элемент в модели.
        /// </summary>
        [CommandHandler]
        public void Select() 
        {
            InSelectObserver.Select();
        }
        /// <summary>
        /// 
        /// </summary>
        [CommandHandler]
        public void GetReference()
        {
            if (InSelectObserver.InContainerForSelected.SelectedElement is AssemblyViewModel assemblyViewModel)
            {
                var meta = assemblyViewModel._bIMAssembly.Meta;
                var metaFile = meta.CheckSavedFile();
                if (metaFile != null)
                {
                    assemblyViewModel._bIMAssembly.Meta.ShortGUID = metaFile.ShortGUID;
                    assemblyViewModel.IsLinq = true;
                }
            }           
        }
        

        [CommandHandler]
        public void AddJoint()
        {
            //TODO: Алгоритм добавления узла не отлажен.

            //if (InContainerForSelected.SelectedElement != null)
            //{
            //    if (InContainerForSelected.SelectedElement is AssemblyViewModel assemblyViewModel)
            //    {
            //        assemblyViewModel.AddJoint();
            //    }
            //    if (InContainerForSelected.SelectedElement.GetOldFather() is AssemblyViewModel assemblyFatherViewModel)
            //    {
            //        ModifyBIMAssemblySelect?.Invoke(assemblyFatherViewModel._bIMAssembly);
            //    }
            //}
        }
    }
}
