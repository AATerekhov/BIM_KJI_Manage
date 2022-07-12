using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RCProjectObject.Model;

namespace BIM_KJI_Manage.TreeViewMain
{
    public class AlbumViewModel: TreeViewItemViewModel
    {
        readonly Album _album;

        public AlbumViewModel(Album album )
            : base(null)
        {
            _album = album;
        }

        public string AlbumName
        {
            get { return _album.Cipher; }
        }
    }
}
