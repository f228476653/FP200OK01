using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace FP200OK01.Entities
{
    class Genre
    {
        [Key]
        public int GenreId { get; set; }
        public string GenreName { get; set; }
        /* public List<Movie> MovieInThisGenre { get; set; }*/

        public virtual List<Movie> Movies { get; set; }
        public Genre()
        {

        }

        public override string ToString()
        {
            return GenreName;
        }
    }
}
