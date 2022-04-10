using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP200OK01.Entities
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public string ReviewDesc { get; set; }
        public User user { get; set; }
        public Movie movie { get; set; }


        public Review(int movieId, int userId, string reviewDesc)
        {
            MovieId = movieId;
            ReviewDesc = reviewDesc;
        }
        public Review() { }
    }
}
