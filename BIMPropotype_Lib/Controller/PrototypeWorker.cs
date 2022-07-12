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

namespace BIMPropotype_Lib.Controller
{
    public static class PrototypeWorker
    {
        public static void GetModelPrototype(TSM.Model Model, ObservableCollection<Model.PrototypeFile> listPrototype) 
        {
            TSM.ModelInfo Info = Model.GetInfo();
            string modelPath = Info.ModelPath;
            var path = $"{modelPath}\\RCP_Data";
            int indexStart = path.Length + 1;

            var listName = Directory.GetFiles(path);

            string subString = "Prototype_";

            for (int i = 0; i < listName.Length; i++)
            {
                var index = listName[i].IndexOf(subString);
                if (index != -1)
                {
                    string name = listName[i].Substring(indexStart, listName[i].Length-4 - indexStart);
                    listPrototype.Add(new Model.PrototypeFile($"{name}"));
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
