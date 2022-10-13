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
        public UDAType InUDAType { get; set; }
        public BIMUda() { }
        public BIMUda(object key, object value)
        {
           if (key is string stringKey) Key = stringKey;
           if (value is string stringValue) Value = stringValue;
           if (value is int intValue) IntValue = intValue;
           if (value is double doubleValue) DoubleValue = doubleValue;
            CheckType();
        }

        internal void WriteToDetail(ModelObject modelObject) 
        {
            if (Key != null)
            {
                if (Value != null) modelObject.SetUserProperty(Key, Value);
                if (IntValue != 0) modelObject.SetUserProperty(Key, IntValue);
                if (DoubleValue != 0) modelObject.SetUserProperty(Key, DoubleValue);
            }
        }

        public void SetValue(string value) 
        {
            if (Key != null)
            {
                CheckType();

                if (InUDAType == UDAType.integerNumber) 
                {
                    int newValue = 0;
                    int.TryParse(value, out newValue);
                    IntValue = newValue;

                }
                else if (InUDAType == UDAType.doubleNumber)
                {
                    double newValue = 0.0;
                    double.TryParse(value, out newValue);
                    DoubleValue = newValue;
                }
                if (InUDAType == UDAType.text) Value = value;
            }
        }

        private void CheckType()
        {
            if (IntValue != 0) InUDAType = UDAType.integerNumber;
            else if (DoubleValue != 0) InUDAType = UDAType.doubleNumber;
            else InUDAType = UDAType.text;
        }
    }
    public enum UDAType
    {
        integerNumber,
        doubleNumber,
        text
    }
}
