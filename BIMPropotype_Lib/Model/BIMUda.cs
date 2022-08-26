using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class BIMUda
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public int IntValue { get; set; }
        public double DoubleValue { get; set; }
        public BIMUda() { }
        public BIMUda(object key, object value)
        {
           if (key is string stringKey) Key = stringKey;
           if (value is string stringValue) Value = stringValue;
           if (value is int intValue) IntValue = intValue;
           if (value is double doubleValue) DoubleValue = doubleValue;
        }

        internal void WriteToDetail(Part part) 
        {
            if (Key != null)
            {
                if (Value != null) part.SetUserProperty(Key, Value);
                if (IntValue != 0) part.SetUserProperty(Key, IntValue);
                if (DoubleValue != 0) part.SetUserProperty(Key, DoubleValue);
            }
        }
    }
}
