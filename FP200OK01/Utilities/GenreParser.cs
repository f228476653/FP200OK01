using FP200OK01.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FP200OK01.Utilities
{
    class GenreParser
    {
        public static List<Genre> GenresInfo = new List<Genre>();
        public static List<Genre> ParseGenre(string fileContents)
        {
            string[] lines = fileContents.Split('\n');

            foreach (string line in lines)
            {
                //Skip if line is empty
                if (string.IsNullOrEmpty(line)) continue;

                string[] fields = line.Trim().Split(',');
                if (fields.Length != 1)
                {
                    MessageBox.Show("Problem parsing file, check format in Genre");
                    continue;
                }
                else
                {
                    try
                    {
                        Genre newGenre = new Genre();
                        newGenre.GenreName = fields[0];
                        GenresInfo.Add(newGenre);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return GenresInfo;
        }
    }
}
