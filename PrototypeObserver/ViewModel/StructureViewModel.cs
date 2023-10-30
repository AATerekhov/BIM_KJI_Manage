using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIMPropotype_Lib.Model;
using Tekla.Structures.Model;
using BIMPropotype_Lib.ExtentionAPI.Mirror;
using BIMPropotype_Lib.ExtentionAPI.InserPlugin;
using BIMPropotype_Lib.ExtentionAPI.PartChildren;
using BIMPropotype_Lib.Controller;
using Tekla.Structures.Geometry3d;
using BIMPropotype_Lib.ExtentionAPI.Conductor;

namespace PrototypeObserver.ViewModel
{
    public class StructureViewModel : TreeViewItemViewModel
    {
        public BIMStructure _bIMStructure;
        public ContainerForSelected InContainerForSelected { get; set; }

        public StructureViewModel(BIMStructure structure, TreeViewItemViewModel parentField, ContainerForSelected containerForSelected)
           : base(parentField, false)
        {
            _bIMStructure = structure;
            InContainerForSelected = containerForSelected;
            this.PropertyChanged += StructureViewModel_PropertyChanged;
            LoadChildren();
        }
        public StructureViewModel(BIMStructure structure, ContainerForSelected containerForSelected)
            : this(structure, null, containerForSelected)
        {

        }
        private void StructureViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                if ((sender as TreeViewItemViewModel).IsSelected)
                {
                    InContainerForSelected.SelectedElement = sender as TreeViewItemViewModel;
                }
            }
        }

        public string Name
        {
            get { return _bIMStructure.ToString(); }
        }

        protected override void LoadChildren()
        {
            foreach (BIMAssembly assemblyList in _bIMStructure.Children)
            {
                base.Children.Add(new AssemblyViewModel(assemblyList, this, InContainerForSelected));
            }
        }

        public override CoordinateSystem GetBase()
        {
            return _bIMStructure.BaseStructure;
        }

        public override void SetBase(CoordinateSystem coordinateSystem)
        {
            _bIMStructure.BaseStructure = coordinateSystem;
        }
        public override void Insert(Model model)
        {
            WorkPlaneWorker workPlaneWorker = new WorkPlaneWorker(model);
            workPlaneWorker.GetWorkPlace(_bIMStructure.BaseStructure);
            this.InsertNotFather(model);

            workPlaneWorker.ReturnWorkPlace();
        }
        public void InsertNotFather(Model model)
        {
            foreach (var item in Children)
            {
                if (item is AssemblyViewModel assemblyVM) assemblyVM.Insert(model);
            }            
        }

        public  void SerializeXML()
        {
            _bIMStructure.SerializeXMLStructure();
        }

        //TODO: InsertMirror() пропущено, отложенная задача?

        //TODO: AddJoint() пропущен, требуется доработка концепции.


    }
}
