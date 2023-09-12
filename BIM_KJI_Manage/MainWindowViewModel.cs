using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM=Tekla.Structures.Model;
using UI = Tekla.Structures.Model.UI;
using TSMO = Tekla.Structures.Model.Operations;
using Tekla.Structures;
using Tekla.Structures.Model.Operations;
using BIMPropotype_Lib.Controller;
using BIMPropotype_Lib.Model;
using BIMPropotype_Lib.ViewModel;
using Propotype_Manage.ViewPrototype;



namespace Propotype_Manage
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using BIMPropotype_Lib.ExtentionAPI.Conductor;
    using Fusion;
    using PrototypeObserver.ViewModel;
    using TeklaStructures = Tekla.Structures.TeklaStructures;

    class MainWindowViewModel : WindowViewModel
    {
        public PrefixDirectory InPrefixDirectory { get; set; }      
        public PrototypeViewModel InPrototypeViewModel { get; set; }
        public MainWindowViewModel(PrefixDirectory inPrefixDirectory, PrototypeViewModel prototypeViewModel)
        { 
            InPrefixDirectory = inPrefixDirectory;
            InPrototypeViewModel = prototypeViewModel;
            InPrototypeViewModel.ModifyBIMAssemblySelect += InPrototypeViewModel_ModifyBIMAssemblySelect;
            //InPrototypeViewModel.CreatePrototypeSelect += InPrototypeViewModel_CreatePrototypeSelect;
            InPrototypeViewModel.UploadPrototypeSelect += InPrototypeViewModel_UploadPrototypeSelect;

            this.Initialize();
        }

        private void InPrototypeViewModel_UploadPrototypeSelect(BIMAssembly obj)
        {
            //obj.DeserializeXML();
        }

        //private void InPrototypeViewModel_CreatePrototypeSelect(BIMAssembly obj)
        //{
        //    obj.SerializeXML();
        //}

        private void InPrototypeViewModel_ModifyBIMAssemblySelect(BIMAssembly obj)
        {
            //obj.SerializeXML();
        }

        /// <summary>
        /// Указывает, подключено ли приложение к Tekla Structures.
        /// </summary>
        private bool isConnected;

        /// <summary>
        /// Название модели.
        /// </summary>
        private string modelName;

        /// <summary>
        /// Получает логическое значение, указывающее, подключено ли приложение.
        /// </summary>
        /// <value> Истина, если приложение подключено, иначе ложь. </value>
        public bool IsConnected
        {
            get { return this.isConnected; }
            private set { this.SetValue(ref this.isConnected, value); }
        }

        /// <summary>
        /// Получает название модели.
        /// </summary>
        /// <value>Название модели.</value>
        public string ModelName
        {
            get { return this.modelName; }
            private set { this.SetValue(ref this.modelName, value); }
        }
       
        #region Prototype 

        private PrototypeFile _selectPrototype;

        public PrototypeFile SelectPrototype
        {
            get { return this._selectPrototype; }
            set { this.SetValue(ref this._selectPrototype, value); }
        }

        private ObservableCollection<PrototypeFile> _prototypeList;

        public ObservableCollection<PrototypeFile> PrototypeList
        {
            get { return this._prototypeList; }
            set { this.SetValue(ref this._prototypeList, value); }
        }


        #endregion //Prototype

        #region Property       


        #endregion //Property

        /// <summary>
        /// Инициализирует модель представления.
        /// </summary>
        protected override async void Initialize()
        {
            base.Initialize();

            this.IsConnected = await Task.Run(() => TeklaStructures.Connect());

            if (this.IsConnected)
            {
                
                this.ModelName = InPrefixDirectory.ModelInfo.ModelName;


                PrototypeList = new ObservableCollection<PrototypeFile>();

            }
        }
        #region Command.

        /// <summary>
        /// Вставка деталей в модель.
        /// </summary>
        [CommandHandler]
        public void InsertPartXML()
        {

            if (InPrototypeViewModel.InSelectObserver.InContainerForSelected.SelectedElement is AssemblyViewModel assembly)
            {
                if (InPrototypeViewModel.IsGlobal)
                {
                    assembly.Insert(InPrefixDirectory.Model);
                }
                else
                {
                    assembly.InsertNotFather(InPrefixDirectory.Model);
                }
            }
            else if (InPrototypeViewModel.InSelectObserver.InContainerForSelected.SelectedElement is StructureViewModel structure) 
            {
                if (InPrototypeViewModel.IsGlobal)
                {
                    structure.Insert(InPrefixDirectory.Model);
                }
                else
                {
                    structure.InsertNotFather(InPrefixDirectory.Model);
                }                
            }

            TSM.Model model = new TSM.Model();
            model.CommitChanges();
        }

        /// <summary>
        /// Сериализовать выбранные сборки.
        /// </summary>
        [CommandHandler]
        public void AddAssemblys()
        {
            TSM.ModelObjectEnumerator modelEnum = new UI.ModelObjectSelector().GetSelectedObjects();

            if (InPrefixDirectory.Source == BIMType.BIMAssembly)
            {
                while (modelEnum.MoveNext())
                {
                    if (modelEnum.Current is TSM.Assembly assemblyModel)
                    {
                        InPrototypeViewModel.InSelectObserver.SelectedPrototype = new BIMAssembly(assemblyModel, InPrefixDirectory.Model);
                        //TODO: временно убираем выбор.
                        //AddDirect();
                    }
                }
            }
            else if (InPrefixDirectory.Source == BIMType.BIMStructure)
            {
                List<TSM.Assembly> structure = new List<TSM.Assembly>();
                while (modelEnum.MoveNext())
                {
                    if (modelEnum.Current is TSM.Assembly assemblyModel)
                    {
                        structure.Add(assemblyModel);
                    }
                }

                BIMStructure bIMStructure = new BIMStructure(structure, InPrefixDirectory.Model);
                //bIMStructure.SerializeXML();
                InPrototypeViewModel.InSelectObserver.SelectedPrototype = bIMStructure;
                //TODO: временно убираем выбор.
                //AddDirect(); 

            }

        }

        #endregion //Command.

        #region ArchiveCommand

        /// <summary>
        /// Разрушение компонентов соединений 
        /// </summary>
        [CommandHandler]
        public void Exploding()
        {
            TSM.ModelObjectEnumerator modelEnum = new UI.ModelObjectSelector().GetSelectedObjects();
            if (modelEnum.GetSize() == 1)
            {
                ArrayList listConnection = new ArrayList();
                while (modelEnum.MoveNext())
                {
                    //if (modelEnum.Current is TSM.Assembly assemblyModel)
                    //{
                    //    var enumModel = assemblyModel.GetFatherComponent();

                    //}
                    if (modelEnum.Current is TSM.Part partModel)
                    {
                        var enumModel = partModel.GetComponents();
                        while (enumModel.MoveNext())
                        {
                            var connectionObject = enumModel.Current as TSM.BaseComponent;
                            listConnection.Add(connectionObject);
                        }
                    }
                }

                UI.ModelObjectSelector modelObjectSelector = new UI.ModelObjectSelector();
                modelObjectSelector.Select(listConnection, false);
                ExplodeConnection();
            }
        }


        private bool CraeteComponetnPartType(string markAssemby)
        {
            //DrawingHandler drawingHandler = new DrawingHandler();

            bool result = false;

            string Name = "BIM_PART_CREATE";

            Name += ".cs";

            string macrosPathFull = string.Empty;

            string MacrosPath = string.Empty;

            try
            {
                TSM.Model CurrentModel = new TSM.Model();
                //DrawingHandler DrawingHandler = new DrawingHandler();

                TeklaStructuresSettings.GetAdvancedOption("XS_MACRO_DIRECTORY", ref macrosPathFull);
                var macrosPaths = macrosPathFull.Split(';');
                macrosPathFull = macrosPaths[0];

                //if (DrawingHandler.GetActiveDrawing() != null)
                //    MacrosPath = "drawings";
                //else
                MacrosPath = "modeling";

                var pathFullCommon = Path.Combine(macrosPathFull, MacrosPath, Name);

                File.WriteAllText(pathFullCommon
                    ,
                    "#pragma warning disable 1633" + "\n" +
                    @"#pragma reference ""Tekla.Macros.Wpf.Runtime""" + "\n" +
                    @"#pragma reference ""Tekla.Macros.Akit""" + "\n" +
                    @"#pragma reference ""Tekla.Macros.Runtime""" + "\n" +
                    "#pragma warning restore 1633" + "\n" +
                    "namespace UserMacros {" +
                        " public sealed class Macro {" +
                        "[Tekla.Macros.Runtime.MacroEntryPointAttribute()]" +
                            "public static void Run(Tekla.Macros.Runtime.IMacroRuntime runtime) {" +
                                "Tekla.Macros.Akit.IAkitScriptHost akit = runtime.Get<Tekla.Macros.Akit.IAkitScriptHost>();" + "\n" +
                                @"Tekla.Macros.Wpf.Runtime.IWpfMacroHost wpf = runtime.Get<Tekla.Macros.Wpf.Runtime.IWpfMacroHost>();" + "\n" +
                                @"wpf.View(""CatalogTree.CatalogTreeView"").Find(""AdvancedMenu"", ""AdvancedMenu.Root"", ""AdvancedMenu.DefineCustomComponent"").As.Button.Invoke();" + "\n" +
                                @"akit.ValueChange(""diaCustomObjectCreate"", ""Optionmenu_120"", ""3"");" + "\n" +
                                @"akit.ValueChange(""diaCustomObjectCreate"", ""ConnectionName"",""" + markAssemby + @""");" + "\n" +
                                @"akit.PushButton(""NextButton"", ""diaCustomObjectCreate"");" + "\n" +
                                @"akit.CommandStart(""ailSelectCustomObjectPositions"", """", ""main_frame"");" + "\n" +
                                @"akit.PushButton(""NextButton"", ""diaCustomObjectCreate"");" + "\n" +
                                @"akit.CommandEnd();" + "\n" +
                                @"akit.PushButton(""FinishButton"", ""diaCustomObjectCreate"");" + "\n" +
                            "}" +
                        "}" +
                    "}"
                );


                //"Tekla.Macros.Akit.IAkitScriptHost akit =runtime.Get<Tekla.Macros.Akit.IAkitScriptHost>();" + "\n" +
                //                @"akit.Callback(""acmd_display_selected_drawing_object_dialog"", """", ""View_10 window_1"");" + "\n" +
                //                @"akit.PushButton(""NewButton"",""view_dial"");" + "\n" +
                //                @"akit.PushButton(""pushbutton_5154"",""view_dial"");" + "\n" +
                //                @"akit.TableSelect(""view_dial"", ""RuleTable"", new int[] {1});" + "\n" +
                //                @"akit.TableValueChange(""view_dial"",  ""RuleTable"",""Category"", ""co_object"");" + "\n" +
                //                @"akit.TableValueChange(""view_dial"",  ""RuleTable"",""Value"", """ + markAssemby + @""");" + "\n" +
                //                @"akit.PushButton(""view_modify"",""view_dial"");" + "\n" +
                //                @"akit.TableValueChange(""view_dial"",  ""RuleTable"",""Category"", ""co_assembly"");" + "\n" +
                //                @"akit.PushButton(""view_modify"",""view_dial"");" + "\n" +
                //                @"akit.PushButton(""view_ok"",""view_dial"");" + "\n" +

                string NameMacros = "..\\" + Path.Combine(MacrosPath, Name);

                if (CurrentModel.GetConnectionStatus())
                {

                    while (Operation.IsMacroRunning())
                        Thread.Sleep(100);
                    result = Operation.RunMacro(NameMacros);
                }
                // RunMacro("..\\" + Name);
            }
            catch (IOException Ex)
            {
                Debug.WriteLine(Ex);
            }
            finally
            {
                if (File.Exists(Path.Combine(macrosPathFull, MacrosPath, Name))) File.Delete(Path.Combine(macrosPathFull, MacrosPath, Name));
            }
            return result;
        }

        private void ExplodeConnection()
        {
            var macroBuilder = new MacroBuilder();

            //create view of all model objects
            macroBuilder.Callback("acmd_explode_selected_joints", "", "View_01 window_1");

            macroBuilder.Run();
        }

        /// <summary>
        /// Создание компонента типа деталь из выбранной сборки.
        /// </summary>
        [CommandHandler]
        public void CreatePart()
        {
            TSM.ModelObjectEnumerator modelEnum = new UI.ModelObjectSelector().GetSelectedObjects();
            if (modelEnum.GetSize() == 1)
            {
                ArrayList listConnection = new ArrayList();
                while (modelEnum.MoveNext())
                {
                    if (modelEnum.Current is TSM.Assembly assemblyModel)
                    {
                        listConnection = assemblyModel.GetSecondaries();
                        var mainPart = assemblyModel.GetMainPart();
                        listConnection.Add(mainPart as TSM.Part);
                    }
                }

                UI.ModelObjectSelector modelObjectSelector = new UI.ModelObjectSelector();
                modelObjectSelector.Select(listConnection, false);
                //ExplodeConnection();
                CraeteComponetnPartType("TestPart1");
            }
        }
        #endregion //ArchiveCommand`

        #region Приватные методы.
        private int ConvertBoolByUDA(bool propertyBool) 
        {
            if (propertyBool)
            {
                return 1;
            }
            else
            {
                return 0;      
            }
        }

        private bool ConvertUDAByBool(int propertyInt)
        {
            if (propertyInt == 0)
            {
                return false;
            }
            else 
            {
                return true;
            }
        }
        #endregion //Приватные методы.

    }
}
