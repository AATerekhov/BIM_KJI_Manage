using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIMPropotype_Lib.Model;

namespace PrototypeObserver.ViewModel
{
    public class UDALineViewModel : INotifyPropertyChanged
    {
        private BIMUda _bIMUdaView;

        public BIMUda BIMUdaView
        {
            get { return _bIMUdaView; }
            set
            {
                _bIMUdaView = value;
                this.OnPropertyChanged("BIMUdaView");
            }
        }


        public string Key
        {
            get { return BIMUdaView.Key; }
        }

        public string Value
        {
            get
            {
                if (BIMUdaView.Value != null) return BIMUdaView.Value;
                if (BIMUdaView.IntValue != 0) return BIMUdaView.IntValue.ToString();
                if (BIMUdaView.DoubleValue != 0) return BIMUdaView.DoubleValue.ToString();
                return string.Empty;
            }
        }

        public UDALineViewModel(BIMUda bIMUda)
        {
            BIMUdaView = bIMUda;
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
