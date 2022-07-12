using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCProjectObject.Controller;
using RCProjectObject.Model;

namespace BIM_KJI_Manage.DataModel
{
    public class DataTom
    {
        private Tom InTom { get; set; }
        public DataTom()
        {
            InTom = new Tom();
            InTom.DeserializeXML();
        }

        public List<Album> GetAlbums() 
        {
            if (InTom.ListAlbums != null)
            {
                return InTom.ListAlbums;
            }
            else { return new List<Album>(); }
        }
    }
}
