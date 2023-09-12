using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propotype_Manage.Model
{
    [Serializable]
    public class ShortGUID
    {
        public string ShortGuid { get; set; }
        public ShortGUID()
        {

        }
        public ShortGUID(bool metkeNew)
        {
            if (metkeNew)
            {
                GetNewShoertGUID();
            }
        }
        private void GetNewShoertGUID()
        {
            ShortGuid = (System.Guid.NewGuid().ToString()).Substring(0, 8);
        }
    }
}
