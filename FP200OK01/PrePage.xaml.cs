using FP200OK01.Entities;
using FP200OK01.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Windows;

namespace FP200OK01
{
    /// <summary>
    /// Interaction logic for PrePage.xaml
    /// </summary>
    public partial class PrePage : Page
    {
        string directorFileName = "";
        string genreFileName = "";

        FileService fs = new FileService();
        DirectorParser dp = new DirectorParser();
        GenreParser gp = new GenreParser();

        public PrePage()
        {
            InitializeComponent();
            // bind event
            AddGenreBtn.Click += selectGenreFile;
            AddDirectorBtn.Click += selectDirectorFile;
            Next.Click += navigateBackButton_Click;
            Skip.Click += navigateBackButton_Click;
            AddMovieBtn.Click += selectMovieFile;
        }
        // go back to page
        void navigateBackButton_Click(object sender, RoutedEventArgs e)
        {

            this.NavigationService.Navigate(new MainPage());

        }
        // to get the csv path, and parse Genre data to DB
        private void selectGenreFile(object o, EventArgs e)
        {
            if (o.Equals(AddGenreBtn))
            {
                Microsoft.Win32.OpenFileDialog openFileDialogue = new Microsoft.Win32.OpenFileDialog();
                // Only show user the Data folder with only "directors.csv" file in it
                string CombinedPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\Data");
                openFileDialogue.InitialDirectory = System.IO.Path.GetFullPath(CombinedPath);
                openFileDialogue.Filter = "CSV Files(genres.csv)|genres.csv";
                openFileDialogue.RestoreDirectory = true;

                Nullable<bool> result = openFileDialogue.ShowDialog();
                if (result == true)
                {
                    genreFileName = openFileDialogue.FileName;
                    status2.Text = "OK";

                    AddMovieBtn.IsEnabled = true;
                }
                else
                {
                    status2.Text = "Fail";
                    return;
                }
                List<Genre> loadGenreList = GenreParser.ParseGenre(fs.ReadFile(genreFileName));
                using (var ctx = new MovieContext())
                {
                    foreach (Genre g in loadGenreList)
                    {
                        ctx.Genre.Add(g);
                        ctx.SaveChanges();
                    }

                }

            }
        }
        // to get the csv path, and parse Movie data to DB
        private void selectMovieFile(object o, EventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialogue = new Microsoft.Win32.OpenFileDialog();
            string CombinedPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\Data");
            openFileDialogue.InitialDirectory = System.IO.Path.GetFullPath(CombinedPath);
            openFileDialogue.Filter = "CSV Files(movies.csv)|movies.csv";
            openFileDialogue.RestoreDirectory = true;

            Nullable<bool> result = openFileDialogue.ShowDialog();

            if (result == true)
            {
                directorFileName = openFileDialogue.FileName;
                status3.Text = "OK";
            }
            else
            {
                status2.Text = "Fail";
                return;
            }
            List<Movie> movieList = new List<Movie>();
            movieList = MovieParser.ParseMovie(fs.ReadFile(@"..\\..\\Data\\movies.csv"));
            for (int i = 0; i < 5; i++)
            {
                List<Movie> movie = new List<Movie>();
                using (var ctx = new MovieContext())
                {
                    ctx.Movie.Add(movieList[i]);

                }
            }

        }
        // to get the csv path, and parse Director data to DB
        private void selectDirectorFile(object o, EventArgs e)
        {
            if (o.Equals(AddDirectorBtn))
            {
                Microsoft.Win32.OpenFileDialog openFileDialogue = new Microsoft.Win32.OpenFileDialog();
                // Only show user the Data folder with only "directors.csv" file in it
                string CombinedPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\Data");
                openFileDialogue.InitialDirectory = System.IO.Path.GetFullPath(CombinedPath);
                openFileDialogue.Filter = "CSV Files(directors.csv)|directors.csv";
                openFileDialogue.RestoreDirectory = true;

                Nullable<bool> result = openFileDialogue.ShowDialog();

                if (result == true)
                {
                    directorFileName = openFileDialogue.FileName;
                    status1.Text = "OK";
                    AddGenreBtn.IsEnabled = true;
                }
                else
                {
                    status2.Text = "Fail";
                    return;
                }
                List<Director> loadDirectorsList = DirectorParser.ParseDirector(fs.ReadFile(directorFileName));
                using (var ctx = new MovieContext())
                {
                    foreach (Director d in loadDirectorsList)
                    {
                        ctx.Director.Add(d);
                        ctx.SaveChanges();
                    }

                }
            }
        }
    }
}
