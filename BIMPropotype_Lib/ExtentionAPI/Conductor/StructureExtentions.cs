using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIMPropotype_Lib.Model;
using BIMPropotype_Lib.ViewModel;
using TSM = Tekla.Structures.Model;
using System.Xml.Serialization;

namespace BIMPropotype_Lib.ExtentionAPI.Conductor
{
    public static class StructureExtentions
    {
        public static string[] GetUDAKeys(this BIMType type) 
        {
            if (type == BIMType.BIMStructure)
            {
                return new string[] { "SISStrName", "SISStrPrefix", "SISStrNumber", "SISStrMain" };
            }
            else
            {
                return new string[] {""};
            }
        }

        public static void SerializeXMLStructure(this BIMStructure bIMStructure)
        {
            string path = FileExplorerExtentions.GetDataDirectory();

            bIMStructure.Meta.CheckSavedGuid();

            string fullPath = Path.Combine(path, bIMStructure.Meta.GetNameFile());
            if (File.Exists(fullPath)) File.Delete(fullPath);

            XmlSerializer formatter = new XmlSerializer(typeof(BIMStructure));

            using (FileStream fs = new FileStream(fullPath, FileMode.CreateNew))
            {
                formatter.Serialize(fs, bIMStructure);
                fs.Close();
            }
        }

        public static BIMStructure DeserializeXMLStructure(this MetaDirectory meta)
        {
            if (meta.Type == BIMType.BIMStructure)
            {
                var path = meta.GetFilePath();
                if (File.Exists(path))
                {
                    var formatter = new XmlSerializer(typeof(BIMStructure));

                    using (var fs = new FileStream(path, FileMode.OpenOrCreate))
                    {
                        return (BIMStructure)formatter.Deserialize(fs);
                    }
                }
            }
            return null;
        }
    }
}
