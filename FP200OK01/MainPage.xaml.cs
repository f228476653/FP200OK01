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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        Login loginPage;
        System.Windows.Controls.Button subLogInButton;
        CreateAccount createPage;
        List<MovieTempForList> tempMovieList;

        MovieEditionPage editionPage;


        List<Director> directors = new List<Director>();
        List<Genre> genres = new List<Genre>();
        List<Movie> movies;
        User u = new User();
        FileService fs = new FileService();



        public MainPage()
        {
            InitializeComponent();
            commonUsage();


        }

        public MainPage(User user)
        {
            InitializeComponent();
            u = user;
            if (user == null || user.UserId == 0)
            {
                LogoutButton.Visibility = Visibility.Hidden;
                LoginButton.Visibility = Visibility.Visible;
            }
            else
            {
                LogoutButton.Visibility = Visibility.Visible;
                LoginButton.Visibility = Visibility.Hidden;
                Great.Content = "Hi, " + u.UserName;

            }
            commonUsage();
        }

        public void commonUsage()
        {
            movies = new List<Movie>();
            //myUser = null;
            loginPage = new Login();
            subLogInButton = loginPage.SubLoginButton;
            createPage = new CreateAccount();
            ReadDataToDatabase();
            /*InitializeMovieGrid();*/
            PopulateMovie();
            InitializeDirectorsListBox();
            InitializeGenresListBox();
            PopulateMovie();
            toggleEvent(true);
        }
        private void SearchInput(Object o, EventArgs e)
        {

            string input = SearchTextBox.Text;
            using (var ctx = new MovieContext())
            {
                movies = ctx.Movie.ToList();
                List<MovieTempForList> tempMovieListFull = new List<MovieTempForList>();
                foreach (Movie m in movies)
                {
                    MovieTempForList currentMovie = new MovieTempForList();
                    currentMovie.MovieId = m.MovieId;
                    currentMovie.MovieTitle = m.MovieTitle;
                    currentMovie.ReleaseDate = m.ReleaseDate.ToShortDateString();
                    currentMovie.MovieDirector = ctx.Director.Where(x => x.DirectorId == m.DirectorId).FirstOrDefault().ToString();
                    currentMovie.MovieGenres = ctx.Genre.Where(x => x.GenreId == m.GenreId).FirstOrDefault().ToString();
                    tempMovieListFull.Add(currentMovie);
                }
                tempMovieList = tempMovieListFull.Where(x => x.MovieDirector.Contains(input) || x.MovieGenres.Contains(input) || x.MovieTitle.Contains(input)
                || x.MovieId.ToString().Contains(input) || x.ReleaseDate.ToString().Contains(input))
                    .ToList();
            }
            MovieDataGrid.ItemsSource = tempMovieList;
        }

        private void GoToFavList(object sender, RoutedEventArgs e)
        {
            if (u.UserId != 0)
            {
                this.NavigationService.Navigate(new FavoritePage(u));
            }
            else
            {
                MessageBox.Show("Please login");
            }
        }
        private void toggleVisible(bool toggle)
        {
            if (toggle)
            {
                LoginButton.Visibility = Visibility.Visible;
                LogoutButton.Visibility = Visibility.Visible;
                CreateButton.Visibility = Visibility.Visible;
            }
            else
            {
                LoginButton.Visibility = Visibility.Hidden;
                LogoutButton.Visibility = Visibility.Hidden;
                CreateButton.Visibility = Visibility.Hidden;
            }
        }



        private void CreateButtonClick(object sender, RoutedEventArgs e)
        {

            toggleVisible(false);
            this.NavigationService.Navigate(new CreateAccount());
        }

        private void LoginButtonClick(Object o, EventArgs e)
        {

            toggleVisible(false);
            this.NavigationService.Navigate(new LoginPage());
        }
        private void LogOutButtonClick(Object o, EventArgs e)
        {

            toggleVisible(true);
            this.NavigationService.Navigate(new MainPage());

        }
        private void toggleEvent(bool toggle)
        {
            if (toggle)
            {
                //subLogInButton.Click += SubLoginButtonClick;
                //CreateButton.Click += CreateButtonClick;
                //createPage.SubCreateButton.Click += SubCreateButtonClick;

                MovieDataGrid.MouseDoubleClick += DataGridCellMouseDoubleClick;
                AddMovieButton.Click += AddMovieButtonClick;
                EditMovieButton.Click += EditMovieButtonClick;
                DeleteMovieButton.Click += DeleteMovieButtonClick;
                SearchTextBox.TextChanged += SearchTextInput;

                LoginButton.Click += LoginButtonClick;
                LogoutButton.Click += LogOutButtonClick;
                CreateButton.Click += CreateButtonClick;
                FavoritesButton.Click += GoToFavList;

            }
        }



        private void WindowClosed(Object o, EventArgs e)
        {
            PopulateMovie();
        }

        private void SearchTextInput(Object o, EventArgs e)
        {
            string keyWord = SearchTextBox.Text;
            if (keyWord.Length == 0)
            {
                EditMovieButton.IsEnabled = true;
                DeleteMovieButton.IsEnabled = true;
                using (var ctx = new MovieContext())
                {
                    movies = ctx.Movie.ToList();
                }
                PopulateMovie();
            }
            else
            {
                EditMovieButton.IsEnabled = false;
                DeleteMovieButton.IsEnabled = false;
                using (var ctx = new MovieContext())
                {
                    movies = ctx.Movie.ToList();
                    List<MovieTempForList> tempMovieList = new List<MovieTempForList>();
                    foreach (Movie m in movies)
                    {
                        MovieTempForList currentMovie = new MovieTempForList();
                        currentMovie.MovieId = m.MovieId;
                        currentMovie.MovieTitle = m.MovieTitle;
                        currentMovie.ReleaseDate = m.ReleaseDate.ToShortDateString();
                        currentMovie.MovieDirector = ctx.Director.Where(x => x.DirectorId == m.DirectorId).First().ToString();
                        currentMovie.MovieGenres = ctx.Genre.Where(x => x.GenreId == m.GenreId).First().ToString();
                        tempMovieList.Add(currentMovie);
                    }
                    tempMovieList = tempMovieList.Where(x => x.MovieTitle.Contains(keyWord) || x.MovieId.ToString().Contains(keyWord) ||
                        x.ReleaseDate.Contains(keyWord) || x.MovieGenres.Contains(keyWord) || x.MovieDirector.Contains(keyWord)).ToList();
                    MovieDataGrid.ItemsSource = tempMovieList;

                }

            }
        }




        private void DataGridCellMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Movie temp = new Movie();
            var cellInfo = MovieDataGrid.SelectedCells[0];
            var id = (cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock).Text;
            temp.MovieId = Convert.ToInt32(id);
            cellInfo = MovieDataGrid.SelectedCells[1];
            var name = (cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock).Text;
            temp.MovieTitle = name;
            this.NavigationService.Navigate(new Detail(temp, u));

        }





        /* public void uploadGenres()
         {
             using (var ctx = new MovieContext())
             {
                 foreach (Genre g in genres)
                 {
                     ctx.Genre.Add(g);
                     ctx.SaveChanges();
                 }

             }
             InitializeGenresListBox();
         }

         public void uploadDirectors()
         {
             using (var ctx = new MovieContext())
             {
                 foreach (Director d in directors)
                 {
                     ctx.Director.Add(d);
                     ctx.SaveChanges();
                 }

             }
             InitializeDirectorsListBox();

         }*/
        public void InitializeDirectorsListBox()
        {
            DirectorListBox.Items.Clear();
            using (var ctx = new MovieContext())
            {
                var directorDB = ctx.Director.ToList().Distinct();
                foreach (var c in directorDB)
                {
                    try
                    {
                        ListViewItem l = new ListViewItem();
                        l.Content = c;
                        DirectorListBox.Items.Add(l);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }
        }

        public void InitializeGenresListBox()
        {
            GenreListBox.Items.Clear();
            using (var ctx = new MovieContext())
            {
                var genreDB = ctx.Genre.ToList().Distinct();
                foreach (var c in genreDB)
                {
                    try
                    {
                        ListViewItem l = new ListViewItem();
                        l.Content = c;
                        GenreListBox.Items.Add(l);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }

            }

        }

        /* private void SubLoginButtonClick(Object o, EventArgs e)
         {
             using (var ctx = new MovieContext())
             {
                 var user = ctx.User.Where(x => x.UserName == loginPage.UserNameTextBox.Text).FirstOrDefault ;

                 if (loginPage.PasswordTextBox.Text == user.getPassword().Trim())
                 {
                     myUser = user;*//*
                     LogoutButton.Visibility = Visibility.Visible;
                     LoginButton.Visibility = Visibility.Hidden;*//*
                     loginPage.HintTextBlock.Text = "";
                     loginPage.UserNameTextBox.Text = "";
                     loginPage.PasswordTextBox.Text = "";
                     loginPage.Hide();
                     CreateButton.Visibility = Visibility.Hidden;
                     AccountTextBlock.Text = "Hello, " + myUser.UserName;
                     AccountTextBlock.Visibility = Visibility.Visible;
                 }
                 else
                 {
                     loginPage.HintTextBlock.Visibility = Visibility.Visible;
                     loginPage.HintTextBlock.Text = "Wrong input information";
                 }
             }
         }*/



        private void PopulateMovie()
        {
            //MovieDataGrid.Items.Clear();
            using (var ctx = new MovieContext())
            {
                movies = ctx.Movie.ToList();
                List<MovieTempForList> tempMovieList = new List<MovieTempForList>();
                foreach (Movie m in movies)
                {
                    MovieTempForList currentMovie = new MovieTempForList();
                    currentMovie.MovieId = m.MovieId;
                    currentMovie.MovieTitle = m.MovieTitle;
                    currentMovie.ReleaseDate = m.ReleaseDate.ToShortDateString();
                    currentMovie.MovieDirector = ctx.Director.Where(x => x.DirectorId == m.DirectorId).FirstOrDefault().ToString();
                    currentMovie.MovieGenres = ctx.Genre.Where(x => x.GenreId == m.GenreId).FirstOrDefault().ToString();
                    tempMovieList.Add(currentMovie);
                }
                MovieDataGrid.ItemsSource = tempMovieList;
            }
        }

        /* private void InitializeMovieGrid()
         {
             DataGridTextColumn movieTitleColumn = new DataGridTextColumn();
             movieTitleColumn.Header = "Movie Title";
             movieTitleColumn.Binding = new Binding("MovieTitle");

             DataGridTextColumn movieReleaseDateColumn = new DataGridTextColumn();
             movieReleaseDateColumn.Header = "Release Date";
             movieReleaseDateColumn.Binding = new Binding("ReleaseDate");
             movieReleaseDateColumn.Binding.StringFormat = "dd/MM/yyyy";


             DataGridTextColumn genreColumn = new DataGridTextColumn();
             genreColumn.Header = "Genre";

             DataGridTextColumn directorColumn = new DataGridTextColumn();
             directorColumn.Header = "Director";

             MovieDataGrid.Columns.Add(movieTitleColumn);
             MovieDataGrid.Columns.Add(movieReleaseDateColumn);
             MovieDataGrid.Columns.Add(genreColumn);
             MovieDataGrid.Columns.Add(directorColumn);
         }*/

        private void AddMovieButtonClick(Object o, EventArgs e)
        {
            editionPage = new MovieEditionPage();

            editionPage.Show();
            editionPage.Closed += WindowClosed;
        }

        private void EditMovieButtonClick(Object o, EventArgs e)
        {
            using (var ctx = new MovieContext())
            {
                movies = ctx.Movie.ToList();
            }

            if (MovieDataGrid.SelectedItem != null)
            {
                editionPage = new MovieEditionPage(movies[MovieDataGrid.SelectedIndex].MovieId);

                editionPage.Show();
                editionPage.Closed += WindowClosed;
            }
            else
            {
                MessageBox.Show("Please select an item");
            }


        }

        private void DeleteMovieButtonClick(Object o, EventArgs e)
        {
            if (MovieDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please select one element for deleting!");
            }
            else
            {
                using (var ctx = new MovieContext())
                {
                    movies = ctx.Movie.ToList();

                    Movie deleteMovie = movies[MovieDataGrid.SelectedIndex];
                    ctx.Movie.Remove(ctx.Movie.Where(x => x.MovieId == deleteMovie.MovieId).First());
                    if (ctx.Favorite.Where(x => x.MovieId == deleteMovie.MovieId).FirstOrDefault() != null) 
                    {
                        ctx.Favorite.Remove(ctx.Favorite.Where(x => x.MovieId == deleteMovie.MovieId).FirstOrDefault());
                    }
                    ctx.SaveChanges();
                }

                PopulateMovie();
            }

        }

        private void ReadDataToDatabase()
        {

            using (var ctx = new MovieContext())
            {
                // First from director                
                var count = ctx.Director.Count();
                if (count <= 0)
                {
                    List<Director> directorList = new List<Director>();
                    directorList = DirectorParser.ParseDirector(fs.ReadFile(@"..\\..\\Data\\directors.csv"));
                    foreach (Director director in directorList)
                    {
                        ctx.Director.Add(director);
                    }
                }

                ctx.SaveChanges();
                // Second is Genre                
                count = ctx.Genre.Count();
                if (count <= 0)
                {
                    List<Genre> genreList = new List<Genre>();
                    genreList = GenreParser.ParseGenre(fs.ReadFile(@"..\\..\\Data\\genres.csv"));
                    foreach (Genre genre in genreList)
                    {
                        ctx.Genre.Add(genre);
                    }
                }

                ctx.SaveChanges();

                // Then add into Movie
                count = ctx.Movie.Count();
                if (count <= 0)
                {
                    List<Movie> movieList = new List<Movie>();
                    movieList = MovieParser.ParseMovie(fs.ReadFile(@"..\\..\\Data\\movies.csv"));
                    for (int i = 0; i < 5; i++)
                    {
                        List<Genre> myGenres = new List<Genre>();
                        myGenres.Add(ctx.Genre.ToList()[i]);
                        myGenres.Add(ctx.Genre.ToList()[(i + 2) % 5]);
                        // movieList[i].Genres = myGenres;                        
                        //movieList[i].MovieDirector = ctx.Director.ToList()[i];
                        ctx.Movie.Add(movieList[i]);

                    }
                }

                ctx.SaveChanges();

                count = ctx.IMDBData.Count();
                if (count <= 0)
                {
                    List<IMDBData> IMDBDataList = new List<IMDBData>();
                    IMDBDataList = IMDBParser.parseRoster(fs.ReadFile(@"..\\..\\Data\\IMDB.csv"));
                    foreach (IMDBData i in IMDBDataList)
                    {
                        ctx.IMDBData.Add(i);
                    }
                }

                ctx.SaveChanges();

                /*count = ctx.Review.Count();
                if (count <= 0)
                {
                    List<Review> review = new List<Review>();
                    review = ReviewCSVParser.parseRoster(fs.ReadFile(@"..\\..\\Data\\reviews.csv"));
                    foreach (Review i in review)
                    {
                        ctx.Review.Add(i);
                    }
                }

                ctx.SaveChanges();

                count = ctx.Favorite.Count();
                if (count <= 0)
                {
                    List<Favorite> favList = new List<Favorite>();
                    favList = FavoriteCSVParser.parseRoster(fs.ReadFile(@"..\\..\\Data\\favorites.csv"));
                    foreach (Favorite i in favList)
                    {
                        ctx.Favorite.Add(i);
                    }
                }

                ctx.SaveChanges();*/
            }
        }

        private class MovieTempForList
        {
            public int MovieId { get; set; }
            public string MovieTitle { get; set; }
            public string ReleaseDate { get; set; }
            public string MovieDirector { get; set; }
            public string MovieGenres { get; set; }


        }

    }
}
