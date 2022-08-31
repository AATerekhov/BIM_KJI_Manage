using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class UDACollection
    {       
        public List<BIMUda> UDAList { get; set; }
        public UDACollection() { }
        internal UDACollection(ModelObject modelObject)
        {
            UDAList = new List<BIMUda>();

            Hashtable hashtable = new Hashtable();
            modelObject.GetAllUserProperties(ref hashtable);
            foreach (var item in hashtable)
            {
                if (item is DictionaryEntry dictionary) UDAList.Add(new BIMUda(dictionary.Key, dictionary.Value));
            }
        }
        internal void GetUDAToPart(ModelObject modelObject)
        {
            foreach (var uda in UDAList)
            {
                uda.WriteToDetail(modelObject);
            }
        }
    }
}
