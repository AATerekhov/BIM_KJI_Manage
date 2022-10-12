using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIMPropotype_Lib.Model;

namespace PrototypeObserver.ViewModel
{
    public class ContainerForSelected : INotifyPropertyChanged
    {
        private TreeViewItemViewModel _selectedElement;

        public TreeViewItemViewModel SelectedElement
        {
            get { return _selectedElement; }
            set
            {
                if (value != _selectedElement)
                {
                    _selectedElement = value;
                    this.OnPropertyChanged("SelectedElement");
                    GetUDAs(_selectedElement);
                }
            }
        }

        private ObservableCollection<UDALineViewModel> _uDAs;

        public ObservableCollection<UDALineViewModel> UDAs
        {
            get { return _uDAs; }
            set
            {
                _uDAs = value;
                this.OnPropertyChanged("UDAs");
            }
        }

        public ContainerForSelected()
        {
            UDAs = new ObservableCollection<UDALineViewModel>();
        }

        private void GetUDAs(TreeViewItemViewModel treeViewItemViewModel) 
        {
            if (treeViewItemViewModel is PartViewModel partObserver)
            {
                UDAs.Clear();

                UDAs.Add(new UDALineViewModel(new BIMUda(PropertyKey.Имя.ToString(), partObserver._bIMPart.Name)));
                UDAs.Add(new UDALineViewModel(new BIMUda(PropertyKey.Класс.ToString(), partObserver._bIMPart.Class)));
                UDAs.Add(new UDALineViewModel(new BIMUda(PropertyKey.Профиль.ToString(), partObserver._bIMPart.Profile)));
                UDAs.Add(new UDALineViewModel(new BIMUda(PropertyKey.Материал.ToString(), partObserver._bIMPart.Material)));
                foreach (BIMUda item in partObserver._bIMPart.UDAList.UDAList)
                {
                    UDAs.Add(new UDALineViewModel(item));
                }               
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
    public enum PropertyKey 
    {
        Имя,
        Класс,
        Профиль,
        Материал
    }
}
