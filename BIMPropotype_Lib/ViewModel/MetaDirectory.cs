using BIMPropotype_Lib.ExtentionAPI.Conductor;
using BIMPropotype_Lib.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class MetaDirectory:IEquatable<string>    
    {
        //BIMType.GetMetaSettings(5) - separator
        public string ShortGUID { get; set; }
        public BIMType Type { get; set; }
        public MetaDirectory(string shortGUID)
        {
            ShortGUID = shortGUID;
        }
        public MetaDirectory() { }

        public MetaDirectory(BIMType type, string name, string prefix)
        {
            Type = type;            
            Name = name;
            Prefix = prefix;
        }
        public MetaDirectory(BIMType type)
        {
            Type = type;
        }
        public MetaDirectory(BIMType type, string name, string prefix, int number)
        {
            Type = type;
            Name = name;
            Prefix = prefix;
            Number = number;
        }

        public MetaDirectory(BIMType type, string name, string prefix, string numberString, string shortGuid)
        {
            Type = type;
            Name = name;
            Prefix = prefix;

            int number = 0;
            int.TryParse(numberString, out number);
            Number = number;

            ShortGUID = shortGuid;
        }

        public string Prefix { get; set; }//UDA-0
        public int Number { get; set; }//резерв, UDA-1
        public string PostPrefix { get; set; }//резерв, UDA-2
        public string Name { get; set; }//UDA-3
        public string Сomment { get; set; }//резерв, UDA-4

        public bool Equals(string guid)
        {
            if (string.Compare(this.ShortGUID, guid, StringComparison.CurrentCulture) == 0)
                return true;
            else return false;
        }
        public void SetNewGuid() 
        {
            ShortGUID = FileExplorerExtentions.GetNewShortGUID();
        }
        public override string ToString()
        {
            //TODO: Префикс будет обязательно, поэтому если настройка пустая, то забираем из сборки.
            char separator = Type.GetMetaSepporate();
            if (Number != 0)
            {
                if (!string.IsNullOrEmpty(PostPrefix)) return $"{Name}{separator}{Prefix}{separator}{Number}{separator}{PostPrefix}";
                else return $"{Name}{separator}{Prefix}{separator}{Number}";
            }
            else
            {
                if (!string.IsNullOrEmpty(PostPrefix)) return $"{Name}{separator}{Prefix}{separator}{PostPrefix}";
                else return $"{Name}{separator}{Prefix}";
            }                
        }
        public string ToMark() 
        {
            if (!string.IsNullOrEmpty(PostPrefix)) return $"{Prefix}-{Number}.{PostPrefix}";//TODO:Обработка PostPrefix не реализована.
            else return $"{Prefix}-{Number}";
        }
    }
}
