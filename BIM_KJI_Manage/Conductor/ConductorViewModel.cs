using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion;
using System.Collections.ObjectModel;
using Propotype_Manage.Conductor.ViewModel;
using BIMPropotype_Lib.ViewModel;

namespace Propotype_Manage.Conductor
{
    public class ConductorViewModel : Fusion.ViewModel
    {

        private bool _isAllOpen;

        public bool IsAllOpen
        {
            get { return this._isAllOpen; }
            set { this.SetValue(ref this._isAllOpen, value); }
        }
        readonly ReadOnlyCollection<ModelDirectoryViewModel> _conductor;
        public ReadOnlyCollection<ModelDirectoryViewModel> Conductor
        {
            get { return _conductor; }
        }


        public ConductorViewModel(PrefixDirectory InPrefixDirectory)
        {
            //Get Conductor
        }
    }
}
