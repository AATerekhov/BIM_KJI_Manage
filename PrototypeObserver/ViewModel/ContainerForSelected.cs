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
        public event Action<TreeViewItemViewModel> ModifyAndSaveEvent;

        private TreeViewItemViewModel _selectedElement;

        public TreeViewItemViewModel SelectedElement
        {
            get { return _selectedElement; }
            set
            {
                if (value != _selectedElement)
                {
                    _selectedElement = value;
                    GetUDAs(_selectedElement);
                }
            }
        }

        private UDALineViewModel _selectedUDA;

        public UDALineViewModel SelectedUDA
        {
            get { return _selectedUDA; }
            set
            {
                if (_selectedUDA != null)
                {
                    _selectedUDA.ModifyAndSaveEvent -= Item_ModifyAndSaveEvent;
                }
                _selectedUDA = value;
                if (_selectedUDA != null)
                {
                    _selectedUDA.ModifyAndSaveEvent += Item_ModifyAndSaveEvent;
                }
                this.OnPropertyChanged("SelectedUDA");
            }
        }
        public ObservableCollection<UDALineViewModel> UDAs { get; set; }        

        public ContainerForSelected()
        {
            UDAs = new ObservableCollection<UDALineViewModel>();
        }

        private void GetUDAs(TreeViewItemViewModel treeViewItemViewModel) 
        {
            if (treeViewItemViewModel is PartViewModel partObserver)
            {
                UDAs.Clear();
                UDAs.Add(new UDALineViewModel(new BIMUda(PropertyKey.Имя.ToString(), partObserver._bIMPart.GetPart().Name), partObserver._bIMPart));
                UDAs.Add(new UDALineViewModel(new BIMUda(PropertyKey.Класс.ToString(), partObserver._bIMPart.GetPart().Class), partObserver._bIMPart));
                UDAs.Add(new UDALineViewModel(new BIMUda(PropertyKey.Профиль.ToString(), partObserver._bIMPart.GetPart().Profile), partObserver._bIMPart));
                UDAs.Add(new UDALineViewModel(new BIMUda(PropertyKey.Материал.ToString(), partObserver._bIMPart.GetPart().Material), partObserver._bIMPart));
                foreach (BIMUda item in partObserver._bIMPart.UDAList.UDAList)
                {
                    UDAs.Add(new UDALineViewModel(item, null));
                }
            }
        }
      
        private void Item_ModifyAndSaveEvent()
        {
            ModifyAndSaveEvent?.Invoke(SelectedElement);
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
