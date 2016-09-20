
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WpfMusicPlayer.Helpers;
using WpfMusicPlayer.Model;

namespace WpfMusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private Controllers.Controller _controller;

        private string _lastSongHeaderClicked;
        private ListSortDirection _lastSongDirection = ListSortDirection.Ascending;
        private string _lastArtistHeaderClicked;
        private ListSortDirection _lastArtistDirection;
        private string _lastAlbumsHeaderClicked;
        private ListSortDirection _lastAlbumsDirection;

        /// <summary>
        /// Main application entry point.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();            

            try
            {
                _controller = new Controllers.Controller();
                DataContext = _controller;
            }
            catch (Exception ex)
            {
                // safety net to intersept unhandled exceptions.
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void LvArtistsGridViewColumnHeaderClick(object sender, RoutedEventArgs e)
        {
            _controller.SortArtistsView(sender, ref _lastArtistHeaderClicked, ref _lastArtistDirection);
        }

        private void LvArtistsSortOrderClick(object sender, MouseButtonEventArgs e)
        {
            _controller.SortArtistsView(LvArtists, ref _lastArtistHeaderClicked, ref _lastArtistDirection);
        }

        private void LvAlbumsSortByDateClick(object sender, MouseButtonEventArgs e)
        {
            _controller.AlbumsSortByDate(LvAlbums, ref _lastAlbumsHeaderClicked, ref _lastAlbumsDirection);
        }

        private void LvSongsGridViewColumnHeaderClick(object sender, RoutedEventArgs e)
        {
            _controller.SortListView(sender, ref _lastSongHeaderClicked, ref _lastSongDirection, ((GridViewColumnHeader)e.OriginalSource).Column.Header as string);
        }

        private void LvArtistsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _controller.ArtistsSelectionChanged(sender);
        }

        private void LvAlbumsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _controller.AlbumsSelectionChanged(sender);
        }
    }
}
