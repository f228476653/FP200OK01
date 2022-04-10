using FP200OK01.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP200OK01.Utilities
{
    // parse Favorite data
    class FavoriteCSVParser
    {
        public static List<Favorite> parseRoster(String fileContents)
        {
            IEnumerable<Favorite> favorites = Enumerable.Empty<Favorite>();
            // get rows by spliting '\n' syntax
            string[] lines = fileContents.Split('\n');
            try
            {
                // avoid title, and create Favorite by using csv data
                favorites = lines.Select(line => line.Split(','))
                    .Where(values => values[0] != "")
                    .Where(values => values[0] != "UserId")
                       .Select(values =>
                       new Favorite(
                        Convert.ToInt32(values[0]),
                        Convert.ToInt32(values[1])
                        )
                       );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ErrorLog.ErrorLogging(ex);
            }

            return favorites.ToList<Favorite>();
        }
    }
}
