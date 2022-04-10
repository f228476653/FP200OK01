using FP200OK01.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP200OK01.Utilities
{
    class MovieParser
    {
        public static List<Movie> movies;
        public static List<Movie> ParseMovie(string fileContents)
        {
            movies = new List<Movie>();
            string[] lines = fileContents.Split('\n');

            foreach (string line in lines)
            {
                //Skip if line is empty
                if (string.IsNullOrEmpty(line)) continue;

                string[] fields = line.Trim().Split(',');
                //MessageBox.Show(fields.Length.ToString());
                if (fields.Length != 5)
                {
                    System.Windows.MessageBox.Show("Problem parsing file, check format in Movie");
                    continue;
                }
                else
                {
                    try
                    {
                        Movie newMovie = new Movie(fields[0], DateTime.Parse(fields[1]), Convert.ToInt32(fields[2]), Convert.ToInt32(fields[3]), fields[4]);
                        movies.Add(newMovie);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return movies;
        }
    }
}
