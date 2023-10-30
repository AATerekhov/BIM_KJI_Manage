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
    public static class FileExplorerExtentions
    {
        /// <summary>
        /// Получение расширения из файла, что бы определить тип содержимого.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetЕxtensionFiles(this string value)
        {
            var rezult = string.Empty;

            if (value.Length > 4)
            {
                rezult = value.Substring(value.Length - 4);
            }

            return rezult;
        }
        /// <summary>
        /// Получить Наименование файла.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetNameFiles(this string value)
        {
            var rezult = string.Empty;

            if (value.Length > 4)
            {
                rezult = value.Substring(0, value.Length - 4);
            }

            return rezult;
        }
        /// <summary>
        /// Определить тип по расширению.
        /// </summary>
        /// <param name="Extention"></param>
        /// <returns></returns>
        public static BIMType GetTypeВyExtention(this string Extention)
        {
            var list = GetExtentionList();
            if (list[0] == Extention)
            {
                return BIMType.BIMAssembly;
            }
            if (list[1] == Extention)
            {
                return BIMType.BIMStructure;
            }
            if (list[2] == Extention)
            {
                return BIMType.BIMJoint;
            }
            return BIMType.Error;
        }


        /// <summary>
        /// Получить расширение для файла хранения шаблона.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetExtention(this BIMType type)
        {
            var list = GetExtentionList();
            return list[(int)type];
        }

        public static string[] GetExtentionList()
        {
            return new string[3] { ".ass", ".str", ".jnt"};
        }

        public static string GetNewShortGUID()
        {
            return (System.Guid.NewGuid().ToString()).Substring(0, 8);
        }

        public static char GetMetaSepporate(this BIMType type)
        {
            if (type == BIMType.BIMAssembly)
            {
                return '_';
            }
            else return '_';
            //Дать при вызове проверку на пустоту.
        }
        public static string GetModelDirectory(this TSM.Model model) 
        {
            if (model.GetConnectionStatus())
            {
                var path = model.GetInfo().ModelPath;
                if (path != null)
                {
                    return path;
                }
            }

            return string.Empty;
        }
        public static string GetDataDirectory()
        {
            string foulderName = "SISPrototipeTemplate";
            var model = new TSM.Model();
            var path = Path.Combine(model.GetModelDirectory(), foulderName);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return path;
        }
        public static void SerializeXML(this BIMAssembly bIMAssembly)
        {
            string path = GetDataDirectory();

            bIMAssembly.Meta.CheckSavedGuid();

            string fullPath = Path.Combine(path, bIMAssembly.GetNameFile());
            if (File.Exists(fullPath)) File.Delete(fullPath);

            XmlSerializer formatter = new XmlSerializer(typeof(BIMAssembly));

            using (FileStream fs = new FileStream(fullPath, FileMode.CreateNew))
            {
                formatter.Serialize(fs, bIMAssembly);
                fs.Close();
            }
        }
        public static BIMAssembly DeserializeXML(this MetaDirectory meta)
        {
            if (meta.Type == BIMType.BIMAssembly)
            {
                var path = meta.GetFilePath();
                if (File.Exists(path))
                {
                    var formatter = new XmlSerializer(typeof(BIMAssembly));

                    using (var fs = new FileStream(path, FileMode.OpenOrCreate))
                    {
                        return (BIMAssembly)formatter.Deserialize(fs);
                    }
                }
            }
            return null;
        }

        public static void CheckSavedGuid(this MetaDirectory meta)
        {
            var copy = meta.CheckSavedFile();
            if (copy != null)
            {
                meta.ShortGUID = copy.ShortGUID;
            }
            else
            {
                if (string.IsNullOrEmpty(meta.ShortGUID))
                {
                    meta.SetNewGuid();
                }                
            }
        }
        public static MetaDirectory CheckSavedFile(this MetaDirectory meta)
        {
            var metaList = meta.Type.GetMetaList(meta.Name);
            return metaList.Where(p => p.Prefix == meta.Prefix && p.Number == meta.Number && p.PostPrefix == meta.PostPrefix).FirstOrDefault();          
        }


        public static string GetFilePath(this MetaDirectory meta) => Path.Combine(GetDataDirectory(), meta.GetNameFile());
        public static string GetNameFile(this BIMAssembly bIMAssembly) => bIMAssembly.Meta.GetNameFile();        
        public static string GetNameFile(this MetaDirectory meta) => $"{meta}{meta.Type.GetMetaSepporate()}{meta.ShortGUID}{meta.Type.GetExtention()}";
        public static string GetNameFilePresets(this MetaDirectory meta) => $"{meta}{meta.Type.GetMetaSepporate()}{meta.ShortGUID}.aps";
        public static List<string> GetNameList(this BIMType type)
        {
            List<string> rezult = new List<string>();
            var vs = Directory.GetFiles(GetDataDirectory());
            foreach (var path in vs)
            {
                var item  = Path.GetFileName(path);
                if (item.CheckTypeFile(type))
                {
                    var nameItem = item.GetName(type);
                    if (!rezult.CheckIsContained(nameItem)) 
                    {
                        rezult.Add(nameItem);
                    }
                }
            }
            return rezult;
        }

        public static List<MetaDirectory> GetMetaList(this BIMType type, string name)
        {
            var metaList =  new List<MetaDirectory>(
                 from directory in Directory.GetFiles(GetDataDirectory())
                  select directory.GetMetaForPath());

            return metaList.Where(p => p.Type == type && p.Name.CheckName(name)).ToList();
        }

        static bool CheckIsContained(this List<string> vs, string name) 
        {
            foreach (var item in vs)
            {
                if (item.CheckName(name)) return true;
            }
            return false;
        }
        static bool CheckName(this string name, string over )
        {
            if (string.Compare(name, over, StringComparison.CurrentCulture) == 0) return true;
            return false;        
        }
        static bool CheckTypeFile(this string file, BIMType type) 
        {
            string extension = file.GetЕxtensionFiles();
            var typeFile = extension.GetTypeВyExtention();
            if (typeFile == type) return true;
            else return false;
        }

        static string GetName(this string file, BIMType type) 
        {
            var fullNameFile = file.GetNameFiles();
            string[] items = fullNameFile.Split(type.GetMetaSepporate());
            return items[0];
        }

        /// <summary>
        /// Получить Meta из полного пути.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static MetaDirectory GetMetaForPath(this string path)
        {
            var fullNameFile = Path.GetFileName(path);

            string extension = fullNameFile.GetЕxtensionFiles();
            var typeFile = extension.GetTypeВyExtention();

            var fullName = fullNameFile.GetNameFiles();

            string[] items = fullName.Split(typeFile.GetMetaSepporate());

            return new MetaDirectory(typeFile, items[0], items[1], items[2], items[3]);
        }
    }
}
