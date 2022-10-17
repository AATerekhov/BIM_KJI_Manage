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
        public event Action ModifyAndSaveEvent;
        public IUDAContainer InBIMModelObject { get; set; }

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
            set 
            {
                if (Key == PropertyKey.Имя.ToString())
                {
                    (InBIMModelObject as BIMPart).Name = value;
                }
                else if (Key == PropertyKey.Профиль.ToString())
                {
                    (InBIMModelObject as BIMPart).Profile = value;
                }
                else if(Key == PropertyKey.Класс.ToString())
                {
                    (InBIMModelObject as BIMPart).Class = value;;
                }
                else if(Key == PropertyKey.Материал.ToString())
                {
                    (InBIMModelObject as BIMPart).Material = value;                  
                }

                BIMUdaView.SetValue(value);
                ModifyAndSaveEvent?.Invoke();
                this.OnPropertyChanged("Value");
            }
        }

        public UDALineViewModel(BIMUda bIMUda, IUDAContainer BIMModelObject)
        {
            BIMUdaView = bIMUda;
            InBIMModelObject = BIMModelObject;
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
