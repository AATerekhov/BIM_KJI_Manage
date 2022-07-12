using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM=Tekla.Structures.Model;
using UI = Tekla.Structures.Model.UI;
using TSMO = Tekla.Structures.Model.Operations;
using RCProjectObject.Controller;
using RCProjectObject.Model;
using Users_KM.Model;
using Users_KM.Controller;
using BIM_KJI_Manage.TreeViewMain;
using BIM_KJI_Manage.DataModel;
using Tekla.Structures;
using Tekla.Structures.Model.Operations;
using BIMPropotype_Lib.Controller;
using BIMPropotype_Lib.Model;



namespace BIM_KJI_Manage
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using Fusion;
    using TeklaStructures = Tekla.Structures.TeklaStructures;

    class MainWindowViewModel : WindowViewModel
    {
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
        private TSM.Model _inModel;

        public TSM.Model InModel
        {
            get { return _inModel; }
            set { _inModel = value; }
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

        #region Свойства Альбомов.

        private Tom inTom;
        private List<Album> inListAlbums;
        private Album inAlbum;

        public Tom InTom
        {
            get { return this.inTom; }
            private set { this.SetValue(ref this.inTom, value); }
        }

        public List<Album> InListAlbums
        {
            get { return this.inListAlbums; }
            private set { this.SetValue(ref this.inListAlbums, value); }
        }

        public Album InAlbum
        {
            get { return this.inAlbum; }
            set {this.SetValue(ref this.inAlbum, value);}
        }
        #endregion


        #region Property
        private string _prefixAssembly;

        public string PrefixAssembly
        {
            get { return this._prefixAssembly; }
            set { this.SetValue(ref this._prefixAssembly, value); }
        }

        private int _numberAssembly;

        public int NumberAssembly
        {
            get { return this._numberAssembly; }
            set { this.SetValue(ref this._numberAssembly, value); }
        }

        private bool _isMainAssembly;

        public bool IsMainAssembly
        {
            get { return this._isMainAssembly; }
            set { this.SetValue(ref this._isMainAssembly, value); }
        }

        private bool _isCopyAssembly;

        public bool IsCopyAssembly
        {
            get { return this._isCopyAssembly; }
            set { this.SetValue(ref this._isCopyAssembly, value); }
        }

        #endregion //Property

        //TODO: удалить после применения UserControl


        /// <summary>
        /// Инициализирует модель представления.
        /// </summary>
        protected override async void Initialize()
        {
            base.Initialize();

            this.IsConnected = await Task.Run(() => TeklaStructures.Connect());

            if (this.IsConnected)
            {
                InModel = new TSM.Model();
                this.ModelName = InModel.GetInfo().ModelName;

                InTom = new Tom();
                InTom.DeserializeXML();
                if (InTom.ListAlbums != null)
                {
                    InListAlbums = InTom.ListAlbums;
                    if (InListAlbums.Count != 0)
                    {
                        InAlbum = InListAlbums[0];
                    }
                }
                PrototypeList = new ObservableCollection<PrototypeFile>();

                PrototypeWorker.GetModelPrototype(InModel, PrototypeList);
            }
        }
        #region Command.

        /// <summary>
        /// Разрушение компонентов соединений 
        /// </summary>
        [CommandHandler]
        public void Exploding()
        {
            //TODO: пока не развиваем эту идею, прототипы перспективнее.
            if (InModel.GetConnectionStatus())
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
            //TODO: пока не развиваем эту идею, прототипы перспективнее.
            if (InModel.GetConnectionStatus())
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
        }

        /// <summary>
        /// Создание прототипа XML сборки.
        /// </summary>
        [CommandHandler]
        public void Serialize()
        {
            if (InModel.GetConnectionStatus())
            {
                TSM.ModelObjectEnumerator modelEnum = new UI.ModelObjectSelector().GetSelectedObjects();
                if (modelEnum.GetSize() == 1)
                {
                    ArrayList listConnection = new ArrayList();
                    while (modelEnum.MoveNext())
                    {
                        if (modelEnum.Current is TSM.Assembly assemblyModel)
                        {
                            new BeamLoader(assemblyModel);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Вызов вставки плагина, пока вставка в 0ль модели.
        /// </summary>
        [CommandHandler]
        public void InsertPartXML()
        {
            if (InModel.GetConnectionStatus())
            {
                if (SelectPrototype != null)
                {
                    var loader = new BeamLoader();
                    loader.InsertPartXML(SelectPrototype.Name);
                }
            }
        }

        /// <summary>
        /// Создание ревизии по заполненным данным. 
        /// </summary>
        [CommandHandler]
        public void RefreshList()
        {
            if (InModel.GetConnectionStatus())
            {
                PrototypeList.Clear();
                PrototypeWorker.GetModelPrototype(InModel, PrototypeList);
            }
        }
        // <summary>
        /// Создание ревизии по заполненным данным. 
        /// </summary>
        [CommandHandler]
        public void DelitedSelected()
        {
            if (InModel.GetConnectionStatus())
            {
                if (SelectPrototype != null)
                {
                    PrototypeWorker.DeliteFile(SelectPrototype.ToString());
                    RefreshList();
                }
            }
        }
        

        /// <summary>
        /// Создание ревизии по заполненным данным. 
        /// </summary>
        [CommandHandler]
        public void ModifySelectedPart()
        {
            if (InModel.GetConnectionStatus())
            {
                TSM.ModelObjectEnumerator modelEnum = new UI.ModelObjectSelector().GetSelectedObjects();
                if (modelEnum.GetSize() == 1)
                {
                    while (modelEnum.MoveNext())
                    {
                        if (modelEnum.Current is TSM.Assembly assemblyModel)
                        {
                            assemblyModel.AssemblyNumber.Prefix = string.Empty;
                            assemblyModel.AssemblyNumber.StartNumber = 0;
                            assemblyModel.Modify();
                            if (assemblyModel.GetMainPart() is TSM.Part mainPart)
                            {
                                mainPart.AssemblyNumber.StartNumber = 1;
                                mainPart.AssemblyNumber.Prefix = $"{PrefixAssembly}-{NumberAssembly}";
                                mainPart.SetUserProperty("BIM_MARK_KJI", $"{PrefixAssembly}-{NumberAssembly}");
                                mainPart.SetUserProperty("BIM_MAIN", ConvertBoolByUDA(IsMainAssembly));
                                mainPart.SetUserProperty("BIM_COPY", ConvertBoolByUDA(IsCopyAssembly));
                                mainPart.Modify();
                            }
                        }
                        else if (modelEnum.Current is TSM.Part partModel)
                        {
                            assemblyModel = partModel.GetAssembly();
                            assemblyModel.AssemblyNumber.Prefix = string.Empty;
                            assemblyModel.AssemblyNumber.StartNumber = 0;
                            assemblyModel.Modify();
                            if (assemblyModel.GetMainPart() is TSM.Part mainPart)
                            {
                                mainPart.AssemblyNumber.StartNumber = 1;
                                mainPart.AssemblyNumber.Prefix = $"{PrefixAssembly}-{NumberAssembly}";
                                mainPart.SetUserProperty("BIM_MARK_KJI", $"{PrefixAssembly}-{NumberAssembly}");
                                mainPart.SetUserProperty("BIM_MAIN", ConvertBoolByUDA(IsMainAssembly));
                                mainPart.SetUserProperty("BIM_COPY", ConvertBoolByUDA(IsCopyAssembly));
                                mainPart.Modify();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Создание ревизии по заполненным данным. 
        /// </summary>
        [CommandHandler]
        public void GetSelectedPartAttributes()
        {
            if (InModel.GetConnectionStatus())
            {
                TSM.ModelObjectEnumerator modelEnum = new UI.ModelObjectSelector().GetSelectedObjects();
                if (modelEnum.GetSize() == 1)
                {
                    while (modelEnum.MoveNext())
                    {
                        if (modelEnum.Current is TSM.Assembly assemblyModel)
                        {
                            if (assemblyModel.GetMainPart() is TSM.Part mainPart)
                            {
                                var prefix = mainPart.AssemblyNumber.Prefix;
                                string[] words = prefix.Split(new char[] {'-'});
                                PrefixAssembly = words[0];
                                var number = 0;
                                if (words.Length > 1)
                                {
                                    int.TryParse(words[1], out number);
                                    if (number != 0)
                                    {
                                        NumberAssembly = number;
                                    }
                                }

                                var bimMain = 0;
                                mainPart.GetUserProperty("BIM_MAIN", ref bimMain);
                                IsMainAssembly = ConvertUDAByBool(bimMain);
                                var bimCopy = 0;
                                mainPart.GetUserProperty("BIM_COPY",ref bimCopy);
                                IsCopyAssembly = ConvertUDAByBool(bimCopy);
                            }
                        }
                        else if (modelEnum.Current is TSM.Part partModel)
                        {
                            assemblyModel = partModel.GetAssembly();
                            if (assemblyModel.GetMainPart() is TSM.Part mainPart)
                            {
                                var prefix = mainPart.AssemblyNumber.Prefix;
                                string[] words = prefix.Split(new char[] { '-' });
                                PrefixAssembly = words[0];
                                var number = 0;
                                if (words.Length > 1)
                                {
                                    int.TryParse(words[1], out number);
                                    if (number != 0)
                                    {
                                        NumberAssembly = number;
                                    }
                                }

                                var bimMain = 0;
                                mainPart.GetUserProperty("BIM_MAIN", ref bimMain);
                                IsMainAssembly = ConvertUDAByBool(bimMain);
                                var bimCopy = 0;
                                mainPart.GetUserProperty("BIM_COPY", ref bimCopy);
                                IsCopyAssembly = ConvertUDAByBool(bimCopy);
                            }
                        }
                    }
                }
            }
        }

        #endregion

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
