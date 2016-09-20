using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WpfMusicPlayer.Helpers;
using WpfMusicPlayer.Model;

namespace WpfMusicPlayer.Controllers
{
    public class Controller : INotifyPropertyChanged
    {
        #region Private properties
        
        private const string MusicUriKey = "MusicUri";
        public enum AlbumSortOrder { ByDate, ByTitleAsc, ByTitleDesc }
        private AlbumSortOrder _lastAlbumSortOrder = AlbumSortOrder.ByDate;

        #endregion

        #region Public properties

        public string LastArtistDirectionText { get; set; }
        public string LastAlbumSortOrderText
        {
            get
            {
                return _lastAlbumSortOrder == AlbumSortOrder.ByDate
                    ? "BY DATE ADDED"
                    : _lastAlbumSortOrder == AlbumSortOrder.ByTitleAsc
                        ? "BY TITLE A-Z"
                        : _lastAlbumSortOrder == AlbumSortOrder.ByTitleDesc ? "BY TITLE Z-A" : "";
            }
        }

        public int ArtistsSelectedIndex { get; set; }
        public int AlbumsSelectedIndex { get; set; }

        public string ArtistCounterText { get { return string.Format("{0} ARTISTS", Artists.Count); } }
        public string AlbumCounterText { get { return string.Format("{0} ALBUMS", Albums.Count); } }
        public string SongCounterText { get { return string.Format("{0} SONGS", Songs.Count); } }

        public string StatusBarStatusText { get; set; }
        public string StatusBarErrorText { get; set; }

        public List<Artist> Artists { get; set; }
        public List<Album> Albums { get; set; }
        public List<Song> Songs { get; set; }

        public Visibility StatusBarVisible { get; set; }

        #endregion

        public Controller()
        {
            Artists = new List<Artist>();
            Albums = new List<Album>();
            Songs = new List<Song>();

            LastArtistDirectionText = "A-Z";
            StatusBarVisible = Visibility.Collapsed;

            LoadArtists();
        }

        #region INotifyPropertyChanged implementation

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        /// <summary>
        /// Downloads music data from the internet and saves the data locally. 
        /// </summary>
        public void LoadArtists()
        {
            StatusBarErrorText = string.Empty;
            StatusBarVisible = Visibility.Collapsed;

            try
            {
                var musicUrl = ConfigurationManager.AppSettings[MusicUriKey];
                if (string.IsNullOrWhiteSpace(musicUrl))
                    throw new Exception(
                        string.Format("Key {0} is missing in AppSettings section in the configuration file.",
                            MusicUriKey));

                var networkHelper = new NetworkHelper();

                if (!networkHelper.CheckForInternetConnection())
                    throw new Exception(
                        "Internet connection is not available. Check if computer is connected to the internet.");

                var translator = new Translator();
                Artists = translator.Translate(networkHelper.GetJsonData(musicUrl));
                OnPropertyChanged("ArtistCounterText");

                if (Artists != null && Artists.Count > 0)
                {
                    ArtistsSelectedIndex = 0;
                    OnPropertyChanged("ArtistsSelectedIndex");
                }

                StatusBarStatusText = String.Empty;
            }
            catch (Exception e)
            {
                StatusBarVisible = Visibility.Visible;
                StatusBarStatusText = String.Empty;
                StatusBarErrorText = string.Format("Error: {0}", e.Message);
                OnPropertyChanged("StatusBarStatusText");
                OnPropertyChanged("StatusBarErrorText");
            }
        }

        /// <summary>
        /// Shows albums for selected atrist.
        /// </summary>
        /// <param name="artist"></param>
        public void ShowAlbums(Artist artist)
        {
            Albums = artist != null ? artist.Albums : new List<Album>();
            OnPropertyChanged("Albums");
            OnPropertyChanged("AlbumCounterText");
            if (Albums == null || Albums.Count == 0) return;
            AlbumsSelectedIndex = 0;
            OnPropertyChanged("AlbumsSelectedIndex");
        }

        /// <summary>
        /// Shows songs for selected album.
        /// </summary>
        /// <param name="album"></param>
        public void ShowSongs(Album album)
        {
            Songs = album != null ? album.Songs : new List<Song>();
            OnPropertyChanged("Songs");
            OnPropertyChanged("SongCounterText");
        }

        /// <summary>
        /// Sorts ListView content items.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="lastHeaderClicked"></param>
        /// <param name="lastDirection"></param>
        /// <param name="header"></param>
        public void SortListView(object sender, ref string lastHeaderClicked, ref ListSortDirection lastDirection, string header = null)
        {
            ListSortDirection direction;

            if (!Equals(header, lastHeaderClicked))
            {
                direction = ListSortDirection.Ascending;
            }
            else
            {
                direction = lastDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
            }

            var dataView = CollectionViewSource.GetDefaultView(((ListView)sender).ItemsSource);

            dataView.SortDescriptions.Clear();
            var sd = new SortDescription(header, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();

            lastDirection = direction;
            lastHeaderClicked = header;
        }

        /// <summary>
        /// Changes Artists ListView sort order. 
        /// </summary>
        /// <param name="sender"></param>
        public void SortArtistsView(object sender, ref string lastArtistHeaderClicked, ref ListSortDirection lastArtistDirection)
        {
            SortListView(sender, ref lastArtistHeaderClicked, ref lastArtistDirection, "Name");
            LastArtistDirectionText = lastArtistDirection == ListSortDirection.Ascending ? "A-Z" : "Z-A";
            Artists = (lastArtistDirection == ListSortDirection.Ascending
                ? Artists.OrderBy(i => i.Name)
                : Artists.OrderByDescending(i => i.Name)).ToList();
            OnPropertyChanged("LastArtistDirectionText");
        }

        /// <summary>
        /// Changes Album ListView sort order. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AlbumsSortByDate(object sender, ref string lastAlbumsHeaderClicked, ref ListSortDirection lastAlbumsDirection)
        {
            if (_lastAlbumSortOrder == AlbumSortOrder.ByDate)
            {
                SortListView(sender, ref lastAlbumsHeaderClicked, ref lastAlbumsDirection, "Title");
                _lastAlbumSortOrder = AlbumSortOrder.ByTitleAsc;
                Albums = Albums.OrderBy(i => i.Title).ToList();

            }
            else if (_lastAlbumSortOrder == AlbumSortOrder.ByTitleAsc)
            {
                SortListView(sender, ref lastAlbumsHeaderClicked, ref lastAlbumsDirection, "Title");
                _lastAlbumSortOrder = AlbumSortOrder.ByTitleDesc;
                Albums = Albums.OrderByDescending(i => i.Title).ToList();
            }
            else
            {
                SortListView(sender, ref lastAlbumsHeaderClicked, ref lastAlbumsDirection, "Date");
                _lastAlbumSortOrder = AlbumSortOrder.ByDate;
                Albums = Albums.OrderBy(i => i.Date).ToList();
            }
            OnPropertyChanged("LastAlbumSortOrderText");
        }

        /// <summary>
        /// Invoked when Artist ListView selection has changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ArtistsSelectionChanged(object sender)
        {
            var index = (sender as ListView).SelectedIndex;
            var artist = index >= 0 ? Artists[index] : null;
            ShowAlbums(artist);
        }

        /// <summary>
        /// Invoked when Album ListView selection has changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AlbumsSelectionChanged(object sender)
        {
            var index = (sender as ListView).SelectedIndex;
            var album = index >= 0 ? Albums[index] : null;
            ShowSongs(album);
        }
    }
}
