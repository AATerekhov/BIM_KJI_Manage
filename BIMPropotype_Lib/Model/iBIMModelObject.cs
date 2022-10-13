using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMPropotype_Lib.Model
{
    public interface iBIMModelObject
    {
        UDACollection UDAList { get; set; }
        void Insert();
    }
}
