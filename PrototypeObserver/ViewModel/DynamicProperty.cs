using BIMPropotype_Lib.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypeObserver.ViewModel
{
    public class DynamicProperty : INotifyPropertyChanged
    {
        public DynamicType PropertyType { get; set; }


        private double _bIMProperty;

        public double BIMProperty
        {
            get { return _bIMProperty; }
            set
            {
                _bIMProperty = value;
                this.OnPropertyChanged("BIMProperty");
            }
        }

        public DynamicProperty()
        {

        }
        public DynamicProperty(double bIMProperty)
        {
            PropertyType = DynamicType.Distance;
            BIMProperty = bIMProperty;
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion // INotifyPropertyChanged Members
    }

    public enum DynamicType 
    {
        Distance,
    }
}
