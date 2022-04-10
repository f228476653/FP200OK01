using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FP200OK01.Entities
{
    // Director Entity
    // To show the director of the movie
    public class Director
    {
        
            [Key]
            public int DirectorId { get; set; }

            public string DirectorName { get; set; }
            public List<Movie> Movies { get; set; }

            public Director()
            {

            }

            public override string ToString()
            {
                return DirectorName;
            }
        
    }
}
