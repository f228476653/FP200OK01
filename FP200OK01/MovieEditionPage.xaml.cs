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
using System.Windows.Shapes;
using FP200OK01.Entities;
using FP200OK01.Utilities;

namespace FP200OK01
{
    /// <summary>
    /// Interaction logic for MovieEditionPage.xaml
    /// </summary>
    public partial class MovieEditionPage : Window
    {
        public MovieEditionPage()
        {
            InitializeComponent();
            // show/hide CRUD button
            EditionMovieIdTextBlock.Visibility = Visibility.Hidden;
            EditionMovieIdTextBox.Visibility = Visibility.Hidden;
            EditionAddMovieButton.Visibility = Visibility.Visible;
            toggleEvent(true);
        }

        public MovieEditionPage(int MovieId)
        {
            InitializeComponent();
            EditionEditMovieButton.Visibility = Visibility.Visible;
            EditionMovieIdTextBox.Text = MovieId.ToString();
            toggleEvent(true);
            try
            {
                // when edit movie, we have to load previous data to the page
                using (var ctx = new MovieContext())
                {
                    var currentMovie = ctx.Movie.Where(x => x.MovieId.ToString() == EditionMovieIdTextBox.Text).First();
                    EditionMovieTitleTextBox.Text = currentMovie.MovieTitle;
                    EditionReleaseDatePicker.SelectedDate = currentMovie.ReleaseDate;
                    EditionDirectorTextBox.Text = currentMovie.DirectorId.ToString();
                    EditionGenreTextBox.Text = currentMovie.GenreId.ToString();
                    var imdb = ctx.IMDBData.Where(x => x.MovieId == currentMovie.MovieId).First();
                    if (imdb != null)
                    {
                        EditionImdbPathTextBox.Text = imdb.imdbPath;
                        EditionPosterPathTextBox.Text = imdb.posterPath;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong when get the data from database! " + ex.Message);
                ErrorLog.ErrorLogging(ex);
            }

        }

        private void EditionButtonClick(Object o, EventArgs e)
        {
            try
            {
                using (var ctx = new MovieContext())
                {
                    // update movie by using new data
                    var currentMovie = ctx.Movie.Where(x => x.MovieId.ToString() == EditionMovieIdTextBox.Text).First();
                    currentMovie.MovieTitle = EditionMovieTitleTextBox.Text;
                    currentMovie.DirectorId = Convert.ToInt32(EditionDirectorTextBox.Text);
                    currentMovie.GenreId = Convert.ToInt32(EditionGenreTextBox.Text);
                    var currentImdb = ctx.IMDBData.Where(i => i.MovieId == currentMovie.MovieId).First();
                    if (currentImdb == null)
                    {
                        IMDBData imdb = new IMDBData(currentMovie.MovieId, EditionImdbPathTextBox.Text, EditionPosterPathTextBox.Text);
                        ctx.IMDBData.Add(imdb);
                    }
                    else
                    {
                        currentImdb.imdbPath = EditionImdbPathTextBox.Text;
                        currentImdb.posterPath = EditionPosterPathTextBox.Text;
                    }
                    if (EditionReleaseDatePicker.SelectedDate != null)
                    {
                        currentMovie.ReleaseDate = EditionReleaseDatePicker.SelectedDate.Value;
                        ctx.SaveChanges();
                        this.Close();
                    }
                    else
                    {
                        EditionHintTextBox.Text = "Please select a date";
                    }



                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("No such element! " + ex.Message);
                ErrorLog.ErrorLogging(ex);
            }

        }

        // add movie button event
        private void AddButtonClick(Object o, EventArgs e)
        {
            if (EditionMovieTitleTextBox.Text.Length == 0 || EditionReleaseDatePicker.SelectedDate == null || EditionDirectorTextBox.Text.Length == 0
                || EditionGenreTextBox.Text.Length == 0)
            {
                MessageBox.Show("Please enter all fields");
            }
            try
            {
                using (var ctx = new MovieContext())
                {

                    Movie newMovie = new Movie();
                    newMovie.MovieTitle = EditionMovieTitleTextBox.Text;
                    newMovie.GenreId = Convert.ToInt32(EditionGenreTextBox.Text);
                    newMovie.DirectorId = Convert.ToInt32(EditionDirectorTextBox.Text);
                    if (EditionReleaseDatePicker.SelectedDate != null)
                    {
                        newMovie.ReleaseDate = EditionReleaseDatePicker.SelectedDate.Value;
                        ctx.Movie.Add(newMovie);
                        ctx.SaveChanges();
                        IMDBData imdb = new IMDBData(newMovie.MovieId, EditionImdbPathTextBox.Text, EditionPosterPathTextBox.Text);
                        ctx.IMDBData.Add(imdb);
                        ctx.SaveChanges();
                        this.Close();
                    }
                    else
                    {
                        EditionHintTextBox.Text = "Please select a date!";
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There are something wrong when data update to database! ");
                EditionHintTextBox.Text = "Some Input are wrong!";
                ErrorLog.ErrorLogging(ex);
            }
        }

        private void toggleEvent(bool toggle)
        {
            if (toggle)
            {
                EditionEditMovieButton.Click += EditionButtonClick;
                EditionAddMovieButton.Click += AddButtonClick;
            }
        }
    }
}
