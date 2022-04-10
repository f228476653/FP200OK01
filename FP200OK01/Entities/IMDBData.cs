using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FP200OK01.Entities
{
    // IMDBData Entity
    // To show the IMDBData of the movie
    public class IMDBData
    {
        [Key]
        public int MovieId { get; set; }
        public string imdbPath { get; set; }
        public string posterPath { get; set; }

        public IMDBData(int movieId, string imdbPath, string posterPath)
        {
            MovieId = movieId;
            this.imdbPath = imdbPath;
            this.posterPath = posterPath;
        }
    }
}
