using FP200OK01.Entities;
using FP200OK01.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FP200OK01
{
    /// <summary>
    /// Interaction logic for FavoritePage.xaml
    /// </summary>
    public partial class FavoritePage : Page
    {
        User user = new User();
        public FavoritePage()
        {
            InitializeComponent();
        }

        public FavoritePage(User u)
        {
            InitializeComponent();
            user = u;
            // bind event
            CloseBtn.Click += navigateBackButton_Click;
            // retrive favorite data
            popFavData();
        }

        private void popFavData()
        {
            wrapPanel.Children.Clear();
            using (var ctx = new MovieContext())
            {
                // get Favorite by userId
                var favList = ctx.Favorite.Where(x => x.UserId == user.UserId).ToList<Favorite>();
                // if there's no favorite moive in the list
                if (favList.Count == 0)
                {
                    TextBlock reviewContent = new TextBlock();
                    reviewContent.Text = "No favorite movie.";
                    wrapPanel.Children.Add(reviewContent);
                }
                else
                {
                    // loop Favorite List
                    foreach (Favorite f in favList)
                    {
                        var movie = ctx.Movie.Where(x => x.MovieId == f.MovieId).FirstOrDefault();
                        var imdb = ctx.IMDBData.Where(x => x.MovieId == f.MovieId).FirstOrDefault();
                        StackPanel favItem = new StackPanel();
                        TextBlock movieName = new TextBlock();
                        Image img = new Image();
                        movieName.Text = movie.MovieTitle;
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(@imdb.posterPath, UriKind.Absolute);
                        bitmap.EndInit();
                        img.Source = bitmap;
                        favItem.Children.Add(movieName);
                        favItem.Children.Add(img);
                        favItem.Orientation = Orientation.Vertical;
                        favItem.Width = 180;
                        favItem.Height = 200;
                        wrapPanel.Children.Add(favItem);
                    }

                }
            }

        }
        // back to main page
        void navigateBackButton_Click(object sender, RoutedEventArgs e)
        {

            this.NavigationService.Navigate(new MainPage(user));

        }
    }
}
