using BIMPropotype_Lib.Model;
using BIMPropotype_Lib.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypeObserver.ViewModel
{
    public interface ILink
    {
        PrefixDirectory InPrefixDirectory { get; set; }
        void SerializeXML();
        void DeserializeXML();
        void DeserializeXMLNotBase();
        void UpdateMetaDirectory(MetaDirectory meta);
        void Swap(MetaDirectory meta);
    }
}
