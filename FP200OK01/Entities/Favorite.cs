using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace FP200OK01.Entities
{
    public class Favorite
    {
        [Key]
        [Required]
        public int UserId { get; set; }
        [Key]
        [Required]
        public int MovieId { get; set; }

        public Favorite(int memberId, int movieId)
        {
            UserId = memberId;
            MovieId = movieId;
        }
        public Favorite() { }
    }
}
