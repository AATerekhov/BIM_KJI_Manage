using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using BIMPropotype_Lib.Model;

namespace PrototypeObserver
{
    public class SelectObserver
    {
        public event Action<BIMAssembly> NewSelectPrototype;

        private BIMAssembly _selectedPrototype;
        public BIMAssembly SelectedPrototype
        {
            get { return _selectedPrototype; }
            set 
            {
                _selectedPrototype = value;
                NewSelectPrototype?.Invoke(_selectedPrototype);
            }
        }

        public SelectObserver()
        {

        }
    }
}
