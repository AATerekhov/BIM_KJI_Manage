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
        private ObservableCollection<AssemblyViewModel> _propotypes;
        public ObservableCollection<AssemblyViewModel> Propotypes
        {
            get { return this._propotypes; }
            private set { this.SetValue(ref this._propotypes, value); }
        }

        private ContainerForSelected _inContainerForSelected;

        public ContainerForSelected InContainerForSelected
        {
            get { return this._inContainerForSelected; }
            private set { this.SetValue(ref this._inContainerForSelected, value); }
        }


        private BIMAssembly _bIMAssemblySelect;

        public BIMAssembly BIMAssemblySelect
        {
            get { return this._bIMAssemblySelect; }
            set 
            {
                this.SetValue(ref this._bIMAssemblySelect, value); 
                if (this._bIMAssemblySelect != null)
                {
                    Propotypes.Clear();
                    InContainerForSelected.UDAs.Clear();
                    Propotypes.Add(new AssemblyViewModel(_bIMAssemblySelect, InContainerForSelected));
                }
            }
        }

        public PrototypeViewModel()
        {
            _propotypes = new ObservableCollection<AssemblyViewModel>();
            InContainerForSelected = new ContainerForSelected();
        }
    }
}
