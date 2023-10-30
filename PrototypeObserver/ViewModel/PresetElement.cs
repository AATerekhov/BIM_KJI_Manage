using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypeObserver.ViewModel
{
    public class PresetElement : INotifyPropertyChanged
    {
        private int _number;
        private double _distance;

        public PresetElement()
        {

        }
        public PresetElement(int number, double distance)
        {
            Number = number;
            Distance = distance;
        }

        public int Number
        {
            get { return _number; }
            set
            {
                _number = value;
                this.OnPropertyChanged("Number");
            }
        }

        public double Distance
        {
            get { return _distance; }
            set 
            {
                _distance = value;
                this.OnPropertyChanged("Distance");
            }
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
}
