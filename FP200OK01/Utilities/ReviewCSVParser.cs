using FP200OK01.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP200OK01.Utilities
{
    class ReviewCSVParser
    {
        public static List<Review> parseRoster(String fileContents)
        {
            IEnumerable<Review> reviews = Enumerable.Empty<Review>();
            // get rows by spliting '\n' syntax
            string[] lines = fileContents.Split('\n');
            try
            {
                // avoid title, and create Review by using csv data
                reviews = lines.Select(line => line.Split(','))
                    .Where(values => values[0] != "")
                    .Where(values => values[0] != "MovieId")
                       .Select(values =>
                       new Review(
                        Convert.ToInt32(values[0]),
                        Convert.ToInt32(values[1]),
                        values[2]
                        )
                       );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return reviews.ToList<Review>();
        }
    }
}
