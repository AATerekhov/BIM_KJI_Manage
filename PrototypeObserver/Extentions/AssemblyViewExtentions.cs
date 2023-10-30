using BIMPropotype_Lib.Model;
using PrototypeObserver.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIMPropotype_Lib.ExtentionAPI.Conductor;
using BIMPropotype_Lib.ViewModel;
using System.IO;
using System.Xml.Serialization;

namespace PrototypeObserver.Extentions
{
    public static class AssemblyViewExtentions
    {
        public static void SerializeXMLPresets(this MetaDirectory meta, List<PresetElement> presetElements)
        {
            string path = FileExplorerExtentions.GetDataDirectory();
            if (meta.Type == BIMType.BIMAssembly && !string.IsNullOrEmpty(meta.ShortGUID))
            {

                string fullPath = Path.Combine(path, meta.GetNameFilePresets());
                if (File.Exists(fullPath)) File.Delete(fullPath);

                XmlSerializer formatter = new XmlSerializer(typeof(List<PresetElement>));

                using (FileStream fs = new FileStream(fullPath, FileMode.CreateNew))
                {
                    formatter.Serialize(fs, presetElements);
                    fs.Close();
                }
            }
           
        }
        public static List<PresetElement> DeserializeXMLPresets(this MetaDirectory meta)
        {
            if (meta.Type == BIMType.BIMAssembly)
            {
                string path = FileExplorerExtentions.GetDataDirectory();
                string fullPath = Path.Combine(path, meta.GetNameFilePresets());

                if (File.Exists(fullPath))
                {
                    var formatter = new XmlSerializer(typeof(List<PresetElement>));

                    using (var fs = new FileStream(fullPath, FileMode.OpenOrCreate))
                    {
                        return (List<PresetElement>)formatter.Deserialize(fs);
                    }
                }
            }
            return new List<PresetElement>();
        }
    }
}
