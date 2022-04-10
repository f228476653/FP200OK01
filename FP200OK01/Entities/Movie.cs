using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP200OK01.Entities
{
    public class Movie : INotifyPropertyChanged
    {
        private int genreId;
        private int directorId;
        private string movieTitle;
        private DateTime releaseDate;
        [Key]
        public int MovieId { get; set; }
        [ForeignKey("Genre")]
        public int GenreId
        {
            get
            {
                return this.genreId;
            }
            set
            {
                this.genreId = value;
                NotifiyPropertyChanged();
            }
        }
        [ForeignKey("Director")]
        public int DirectorId { get; set; }
        public string MovieTitle { get; set; }
        public string MovieDescription { get; set; }
        public DateTime ReleaseDate { get; set; }


        public List<Review> Reviews { get; set; }

        public IMDBData imdbData { get; set; }

        public Movie(string movieTitle, DateTime releaseDate, int genreId, int directorId, string movieDescription)
        {
            GenreId = genreId;
            DirectorId = directorId;
            MovieTitle = movieTitle;
            ReleaseDate = releaseDate;
            MovieDescription = movieDescription;

        }

        public Movie() { }



        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifiyPropertyChanged()
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(""));
            }
        }
    }
}
