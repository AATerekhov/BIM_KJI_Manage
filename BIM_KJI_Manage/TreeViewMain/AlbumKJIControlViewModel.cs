using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using RCProjectObject.Model;
using BIM_KJI_Manage.DataModel;
using Fusion;

namespace BIM_KJI_Manage.TreeViewMain
{
    public class AlbumKJIControlViewModel:ViewModel
    {
        readonly ReadOnlyCollection<AlbumViewModel> _albums;

        public AlbumKJIControlViewModel()
        {
            DataTom dataTom = new DataTom();
            List<Album> albums = dataTom.GetAlbums();

            _albums = new ReadOnlyCollection<AlbumViewModel>(
                (from album in albums
                 select new AlbumViewModel(album))
                .ToList());
        }

        public ReadOnlyCollection<AlbumViewModel> Albums
        {
            get { return _albums; }
        }

        /// <summary>
        /// Создание ревизии по заполненным данным. 
        /// </summary>
        [CommandHandler]
        public void TestModule()
        {

        }
    }
}
