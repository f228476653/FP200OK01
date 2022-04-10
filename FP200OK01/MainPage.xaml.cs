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
        int prevselectedDir = -1;
        int prevselectedGen = -1;
        int selectedDirInd = -1;
        int selectedGenreInd = -1;


        public MainPage()
        {
            InitializeComponent();
            commonUsage();
        }

        public MainPage(User user)
        {
            InitializeComponent();
            u = user;
            // if user has login, display greeting word and show logout btn
            // if not, show login
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
            // auto populate data if the user skip the file upload
            ReadDataToDatabase();
            PopulateMovie();
            // init select box
            InitializeDirectorsListBox();
            InitializeGenresListBox();
            // get movie data
            PopulateMovie();
            LoadStatistics();
            toggleEvent(true);
        }
        // search by key word
        private void SearchInput(Object o, EventArgs e)
        {

            
            string input = SearchTextBox.Text;
            using (var ctx = new MovieContext())
            {
                // get movie list from database, and display it on the list
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
        // go to favorite list event
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
        // button show/hide
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
        //go to create account page
        private void CreateButtonClick(object sender, RoutedEventArgs e)
        {

            toggleVisible(false);
            this.NavigationService.Navigate(new CreateAccount());
        }
        // go to login page
        private void LoginButtonClick(Object o, EventArgs e)
        {

            toggleVisible(false);
            this.NavigationService.Navigate(new LoginPage());
        }
        //logout and go to main page
        private void LogOutButtonClick(Object o, EventArgs e)
        {

            toggleVisible(true);
            this.NavigationService.Navigate(new MainPage());

        }
        // bind event
        private void toggleEvent(bool toggle)
        {
            if (toggle)
            {

                MovieDataGrid.MouseDoubleClick += DataGridCellMouseDoubleClick;
                AddMovieButton.Click += AddMovieButtonClick;
                EditMovieButton.Click += EditMovieButtonClick;
                DeleteMovieButton.Click += DeleteMovieButtonClick;
                SearchTextBox.TextChanged += SearchTextInput;
                DirectorListBox.SelectionChanged += filterByDirectorGenreMulti;
                GenreListBox.SelectionChanged += filterByDirectorGenreMulti;
                LoginButton.Click += LoginButtonClick;
                LogoutButton.Click += LogOutButtonClick;
                CreateButton.Click += CreateButtonClick;
                FavoritesButton.Click += GoToFavList;
                MovieDataGrid.AutoGeneratedColumns += LoadStatistics;

            }
        }
        // when finished edit movie, refresh the grid
        private void WindowClosed(Object o, EventArgs e)
        {
            PopulateMovie();
        }
        // search by key word, and update the grid byt the result
        private void SearchTextInput(Object o, EventArgs e)
        {
            if (DirectorListBox.SelectedItems.Count != 0 || GenreListBox.SelectedItems.Count != 0)
            {
                unSelectListBox();
            }             
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

        // filter movie by Genre
        private void filterByDirectorGenreMulti(Object o, EventArgs e)
        {
            
            // Get the string from listview to build list 
            List<string> directorsName = new List<string>();
            foreach (ListViewItem li in DirectorListBox.SelectedItems)
            {
                directorsName.Add(li.Content.ToString());
            }
            List<string> genresName = new List<string>();
            foreach (ListViewItem li in GenreListBox.SelectedItems)
            {
                genresName.Add(li.Content.ToString());
            }

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
                // Three possible condition
                if (directorsName.Count > 0 && genresName.Count > 0)
                {
                    tempMovieList = tempMovieList.Where(x => directorsName.Contains(x.MovieDirector) || genresName.Contains(x.MovieGenres)).ToList();
                }
                else if (genresName.Count > 0)
                {
                    tempMovieList = tempMovieList.Where(x => genresName.Contains(x.MovieGenres)).ToList();
                }
                else if (directorsName.Count > 0)
                {
                    tempMovieList = tempMovieList.Where(x => directorsName.Contains(x.MovieDirector)).ToList();
                }

                if ((DirectorListBox.SelectedItems.Count != 0 || GenreListBox.SelectedItems.Count != 0) && SearchTextBox.Text.Length != 0 )
                {
                    SearchTextBox.Text = "";
                }


                MovieDataGrid.ItemsSource = tempMovieList;
            }


        }


        // go to detail page when user double click the row in data grid
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

        // init director listBox
        public void InitializeDirectorsListBox()
        {
            DirectorListBox.SelectionMode = SelectionMode.Multiple;
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
                        ErrorLog.ErrorLogging(e);
                    }
                }
            }
        }
        // init Genres listBox
        public void InitializeGenresListBox()
        {
            GenreListBox.SelectionMode = SelectionMode.Multiple;
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
                        ErrorLog.ErrorLogging(e);
                    }
                }

            }

        }

        

        // loop through movielist and add event entry to dataGrid
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

       // when user click add movie
        private void AddMovieButtonClick(Object o, EventArgs e)
        {
            editionPage = new MovieEditionPage();

            editionPage.Show();
            editionPage.Closed += WindowClosed;
        }
        // when user click edit movie
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
        // when user click delete movie
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
                    ctx.SaveChanges();
                }

                PopulateMovie();
            }

        }
        // if the user doesn't update csv themself, we'll parse our own data
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
                // parse IMDB data
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

               
            }
        }

        // unSelectListBox to solve the conflict between functions
        private void unSelectListBox()
        {
            DirectorListBox.SelectionChanged -= filterByDirectorGenreMulti;
            GenreListBox.SelectionChanged -= filterByDirectorGenreMulti;
            foreach (ListViewItem ls in GenreListBox.Items)
            {
                ls.IsSelected = false;
            }
            foreach (ListViewItem ls in DirectorListBox.Items)
            {
                ls.IsSelected = false;
            }
            DirectorListBox.SelectionChanged += filterByDirectorGenreMulti;
            GenreListBox.SelectionChanged += filterByDirectorGenreMulti;
        }

        // Delegate for LoadStatistics
        private void LoadStatistics(Object o, EventArgs e)
        {
            LoadStatistics();
        }

        // Populate the data to Statistics Text box
        private void LoadStatistics()
        {
            StatisticTextBox.Text = "";
            using (var ctx = new MovieContext())
            {
                var moviesCount = ctx.Movie.Count();
                var directorsCount = ctx.Director.Count();
                var genresCount = ctx.Director.Count();
                StatisticTextBox.Text = "In database:\n\tMovies: " + moviesCount + "\n\tDirectors: " + directorsCount +
                    "\n\tGenres: " + genresCount;
            }

            StatisticTextBox.Text += "\n\n\nDisplaying: " + MovieDataGrid.Items.Count + " movie(s)";
            
        }
        // Temp class for formating
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
