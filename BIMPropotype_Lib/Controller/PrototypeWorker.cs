using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;
using Tekla.Structures;
using Tekla.Structures.Model;
using BIMPropotype_Lib.ViewModel;

namespace BIMPropotype_Lib.Controller
{
    public static class PrototypeWorker
    {
        public static void GetModelPrototype(PrefixDirectory prefixDirectory, ObservableCollection<Model.PrototypeFile> listPrototype) 
        {
            var path = prefixDirectory.GetDataDirectory();
            int indexStart = path.Length + 1;

            var listName = Directory.GetDirectories(path);

            string subString = prefixDirectory.typeFile;
            foreach (var direct in listName)
            {
                var listNameFile = Directory.GetFiles(direct);
                for (int i = 0; i < listNameFile.Length; i++)
                {
                    var index = listNameFile[i].IndexOf(subString);
                    if (index != -1)
                    {
                        string name =$"{listNameFile[i].Substring(indexStart, listNameFile[i].Length - 4 - indexStart)}" ;
                        listPrototype.Add(new Model.PrototypeFile($"{name}"));
                    }
                }
            }
            
        }

        public static void DeliteFile(string fileName)
        {
            string modelPath = string.Empty;
            TSM.Model model = new TSM.Model();
            if (model.GetConnectionStatus())
            {
                ModelInfo Info = model.GetInfo();
                modelPath = Info.ModelPath;
            }

            if (File.Exists($"{modelPath}\\RCP_Data\\{fileName}.xml"))
            {
                File.Delete($"{modelPath}\\RCP_Data\\{fileName}.xml");
            }
        }

    }
}
