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
        public BeamLoader(PrefixDirectory prefixDirectory)
        {
            WorkDirectory = prefixDirectory;
        }
        public BeamLoader(TSM.Assembly InAssembly, PrefixDirectory prefixDirectory)
        {
           WorkDirectory = prefixDirectory;
           InBIMAssembly = new BIMAssembly(InAssembly, WorkDirectory.Model);
           SerializeXML();
        }

        public void SerializeXMLStructure(List<TSM.Assembly> assemblies)
        {
            BIMStructure bIMStructure = new BIMStructure(assemblies, WorkDirectory.Model);

            if (Path != string.Empty)
            {
                WorkDirectory.FieldName = bIMStructure.Name;
                WorkDirectory.Prefix = bIMStructure.Prefix;
                string path = $"{WorkDirectory.GetFile()}{GetExtention(BIMType.BIMStructure)}";
                if (File.Exists(path)) File.Delete(path);

                XmlSerializer formatter = new XmlSerializer(typeof(BIMStructure));

                using (FileStream fs = new FileStream(path, FileMode.CreateNew))
                {
                    formatter.Serialize(fs, bIMStructure);
                    fs.Close();
                }
            }
        }

        public void SerializeXML()
        {
            if (Path != string.Empty)
            {
                WorkDirectory.FieldName = InBIMAssembly.Name;
                WorkDirectory.Prefix = InBIMAssembly.Prefix;
                string path = WorkDirectory.GetFile();
                if (File.Exists(path)) File.Delete(path);

                XmlSerializer formatter = new XmlSerializer(typeof(BIMAssembly));

                using (FileStream fs = new FileStream(path, FileMode.CreateNew))
                {
                    formatter.Serialize(fs, InBIMAssembly);
                    fs.Close();
                }
            }
        }

        public void DeserializeXML() 
        {
            var path = WorkDirectory.GetFile();
            if (File.Exists(path))
            {
                var formatter = new XmlSerializer(typeof(BIMAssembly));

                if (this.Path != string.Empty)
                {
                    using (var fs = new FileStream(path, FileMode.OpenOrCreate))
                    {
                        InBIMAssembly = (BIMAssembly)formatter.Deserialize(fs);
                    }
                }
            }
        }

        public string GetExtention(BIMType type) 
        {
            if ((int)type == 1)
            {
                return ".str";
            }
            else if ((int)type == 2)
            {
                return ".jnt";
            }
            else return string.Empty;                
        }
    }

    public enum BIMType 
    {
        BIMStructure = 1,
        BIMAssembly = 0,
        BIMJoint = 2
    }
}
