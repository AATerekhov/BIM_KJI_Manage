using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMPropotype_Lib.Model
{
    public interface IReference
    {
         string Name { get; set; }//Папка хранения
         string Prefix { get; set; }//Имя файла хранения

        void Load();
        void Save();

    }
}
