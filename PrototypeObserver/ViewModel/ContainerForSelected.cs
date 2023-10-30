using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIMPropotype_Lib.Model;
using DrawingCopierLib.Transfer.Model;
using TeklaGridConverting.Model;
using PrototypeObserver.Extentions;

namespace PrototypeObserver.ViewModel
{
    public class ContainerForSelected : INotifyPropertyChanged
    {
        public ObservableCollection<PresetElement> PresetElements{ get; set; }

        private PresetElement _selectPreset;
        public PresetElement SelectPreset
        {
            get { return _selectPreset; }
            set
            {
                _selectPreset = value;

                if (_selectPreset != null)
                    if (IsDynamic)
                        if (SelectedElement != null) 
                            if (_selectPreset.Distance != 0)
                                if (SelectedElement.DynamicProperties[0].BIMProperty != _selectPreset.Distance)
                                    SelectedElement.DynamicProperties[0].BIMProperty = _selectPreset.Distance;

                this.OnPropertyChanged("SelectPreset");
            }
        }

        private bool _isDynamic = false;
        public bool IsDynamic
        {
            get
            {
                return _isDynamic;
            }
            set 
            { 
                _isDynamic = value;
                this.OnPropertyChanged("IsDynamic");
            }
        }

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
                    TransferBase.UCSFinish = _selectedElement.GetBase().CreateUniversalCoordinatSystem();

                    IsDynamic = CheckIsDynamic(_selectedElement);

                    this.OnPropertyChanged("DistanceX");
                    this.OnPropertyChanged("DistanceY");
                    this.OnPropertyChanged("DistanceZ");
                    this.OnPropertyChanged("AngleY");
                    this.OnPropertyChanged("AngleX");
                    this.OnPropertyChanged("AngleZ");
                }
                this.OnPropertyChanged("SelectedElement");
            }
        }

        private bool CheckIsDynamic(TreeViewItemViewModel treeViewItemViewModel)
        {
            PresetElements.Clear();
            if (treeViewItemViewModel.DynamicProperties != null)
            {
                if (treeViewItemViewModel.DynamicProperties.Count > 0) 
                {
                    if (treeViewItemViewModel is AssemblyViewModel assemblyViewModel)
                    {
                        var listPreset = assemblyViewModel._bIMAssembly.Meta.DeserializeXMLPresets();

                        foreach (var item in listPreset)
                        {
                            PresetElements.Add(item);
                        }

                        SelectPreset = PresetElements.Where(x => x.Distance == treeViewItemViewModel.DynamicProperties[0].BIMProperty).FirstOrDefault();
                    }


                    return true; 
                }
                else return false;
            }
            else
            {
                return false;
            }
        }

        private UDALineViewModel _selectedUDA;

        public UDALineViewModel SelectedUDA
        {
            get { return _selectedUDA; }
            set
            {               
                _selectedUDA = value;               
                this.OnPropertyChanged("SelectedUDA");
            }
        }
        public ObservableCollection<UDALineViewModel> UDAs { get; set; }
        public TransferBasis TransferBase { get; set; }
        public double DistanceX
        {
            get { return TransferBase.GetDistanceX(); }
            set
            {
                TransferBase.SetDistanceX(value);
                SelectedElement.SetBase(TransferBase.UCSFinish.GetTeklaCoordinatSystem());
                this.OnPropertyChanged("DistanceX");
                //this.OnPropertyChanged("DistanceTransfer");
                //this.OnPropertyChanged("ViewFinish");
            }
        }
        public double DistanceY
        {
            get { return TransferBase.GetDistanceY(); }
            set
            {
                TransferBase.SetDistanceY(value);
                SelectedElement.SetBase(TransferBase.UCSFinish.GetTeklaCoordinatSystem());
                this.OnPropertyChanged("DistanceY");
                //this.OnPropertyChanged("DistanceTransfer");
                //this.OnPropertyChanged("ViewFinish");
            }
        }
        public double DistanceZ
        {
            get { return TransferBase.GetDistanceZ(); }
            set
            {
                TransferBase.SetDistanceZ(value);
                SelectedElement.SetBase(TransferBase.UCSFinish.GetTeklaCoordinatSystem());
                this.OnPropertyChanged("DistanceZ");
                //this.OnPropertyChanged("DistanceTransfer");
                //this.OnPropertyChanged("ViewFinish");
            }
        }
        public double AngleX
        {
            get { return TransferBase.GetAngleX(); }
            set
            {
                TransferBase.SetAngleX(value);
                SelectedElement.SetBase(TransferBase.UCSFinish.GetTeklaCoordinatSystem());
                this.OnPropertyChanged("AngleX");
                this.OnPropertyChanged("AngleY");
                this.OnPropertyChanged("AngleZ");
                //this.OnPropertyChanged("Angle");
                //this.OnPropertyChanged("DistanceTransfer");
                //this.OnPropertyChanged("ViewFinish");
            }
        }
        public double AngleY
        {
            get { return TransferBase.GetAngleY(); }
            set
            {
                TransferBase.SetAngleY(value);
                SelectedElement.SetBase(TransferBase.UCSFinish.GetTeklaCoordinatSystem());
                this.OnPropertyChanged("AngleY");
                this.OnPropertyChanged("AngleX");
                this.OnPropertyChanged("AngleZ");
                //this.OnPropertyChanged("Angle");
                //this.OnPropertyChanged("DistanceTransfer");
                //this.OnPropertyChanged("ViewFinish");
            }
        }
        public double AngleZ
        {
            get { return TransferBase.GetAngleZ(); }
            set
            {
                TransferBase.SetAngleZ(value);
                SelectedElement.SetBase(TransferBase.UCSFinish.GetTeklaCoordinatSystem());
                this.OnPropertyChanged("AngleZ");
                this.OnPropertyChanged("AngleX");
                this.OnPropertyChanged("AngleY");
                //this.OnPropertyChanged("Angle");
                //this.OnPropertyChanged("DistanceTransfer");
                //this.OnPropertyChanged("ViewFinish");
            }
        }

        public ContainerForSelected()
        {
            UDAs = new ObservableCollection<UDALineViewModel>();
            TransferBase = new TransferBasis();
            PresetElements = new ObservableCollection<PresetElement>();
        }

        private void GetUDAs(TreeViewItemViewModel treeViewItemViewModel) 
        {
            if (treeViewItemViewModel is PartViewModel partObserver)
            {
                UDAs.Clear();
                UDAs.Add(new UDALineViewModel(new BIMUda(PropertyKey.Имя.ToString(), partObserver._bIMPart.GetPart().Name), partObserver._bIMPart));
                UDAs.Add(new UDALineViewModel(new BIMUda(PropertyKey.Класс.ToString(), partObserver._bIMPart.GetPart().Class), partObserver._bIMPart));
                UDAs.Add(new UDALineViewModel(new BIMUda(PropertyKey.Профиль.ToString(), partObserver._bIMPart.GetPart().Profile.ProfileString), partObserver._bIMPart));
                UDAs.Add(new UDALineViewModel(new BIMUda(PropertyKey.Материал.ToString(), partObserver._bIMPart.GetPart().Material.MaterialString), partObserver._bIMPart));
                foreach (BIMUda item in partObserver._bIMPart.UDAList.UDAList)
                {
                    UDAs.Add(new UDALineViewModel(item, null));
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
