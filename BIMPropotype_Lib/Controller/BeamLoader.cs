using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures;
using Tekla.Structures.Model;
using TSM = Tekla.Structures.Model;
using Tekla.Structures.Geometry3d;
using System.Xml.Serialization;
using System.IO;
using BIMPropotype_Lib.Model;
using UI = Tekla.Structures.Model.UI;
using System.Collections;
using BIMPropotype_Lib.ViewModel;

namespace BIMPropotype_Lib.Controller
{
    public class BeamLoader
    {
        public PrefixDirectory WorkDirectory { get; set; }
        public BIMAssembly InBIMAssembly { get; set; }
        public string Path { get; set; }
        public BeamLoader() { }
        public BeamLoader(TSM.Assembly InAssembly, PrefixDirectory prefixDirectory)
        {
           WorkDirectory = prefixDirectory;
           InBIMAssembly = new BIMAssembly(InAssembly);
           SerializeXML();
        }    

        //public void GetPath()
        //{
        //    string modelPath = string.Empty;
        //    TSM.Model model = new TSM.Model();
        //    if (model.GetConnectionStatus())
        //    {
        //        ModelInfo Info = model.GetInfo();
        //        modelPath = Info.ModelPath;
        //    }
        //    Path = modelPath;
        //}

        public void SerializeXML()
        {
            //if (Path == string.Empty || Path == null)
            //{
            //    this.GetPath();
            //}

            if (Path != string.Empty)
            {
                WorkDirectory.FieldName = InBIMAssembly.Name;
                WorkDirectory.Prefix = InBIMAssembly.Prefix;
                string path = WorkDirectory.GetFile();
               if (File.Exists(path)) File.Delete(path);

                XmlSerializer formatter = new XmlSerializer(typeof(BIMAssembly));

                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, InBIMAssembly);
                    fs.Close();
                }
            }
        }
        /// <summary>
        /// Вставка балки из xml файла.
        /// </summary>
        /// <param name="fileName"></param>
        public void InsertPartXML(string fileName)
        {
            //if (Path == string.Empty || Path == null)
            //{
            //    this.GetPath();
            //}
            if (File.Exists($"{Path}\\RCP_Data\\{fileName}.xml"))
            {
                var formatter = new XmlSerializer(typeof(BIMAssembly));

                if (this.Path != string.Empty)
                {
                    using (var fs = new FileStream($"{Path}\\RCP_Data\\{fileName}.xml", FileMode.OpenOrCreate))
                    {
                        InBIMAssembly = (BIMAssembly)formatter.Deserialize(fs);
                        InBIMAssembly.Insert();
                        TSM.Model model = new TSM.Model();
                        model.CommitChanges();
                    }
                }
            }
        }

        /// <summary>
        /// Вставка балки из xml файла.
        /// </summary>
        /// <param name="fileName"></param>
        public BIMAssembly GetPartXML(string fileName)
        {
            //if (Path == string.Empty || Path == null)
            //{
            //    this.GetPath();
            //}
            if (File.Exists($"{Path}\\RCP_Data\\{fileName}.xml"))
            {
                var formatter = new XmlSerializer(typeof(BIMAssembly));

                if (this.Path != string.Empty)
                {
                    using (var fs = new FileStream($"{Path}\\RCP_Data\\{fileName}.xml", FileMode.OpenOrCreate))
                    {
                       return (BIMAssembly)formatter.Deserialize(fs);
                    }
                }
            }
            return null;
        }
        

        #region private

      
        #endregion // private
    }
}
