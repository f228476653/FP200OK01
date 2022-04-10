using FP200OK01.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace FP200OK01.Utilities
{
    class DirectorParser
    {
        public static List<Director> DirectorsInfo = new List<Director>();
        public static List<Director> ParseDirector(string fileContents)
        {
            string[] lines = fileContents.Split('\n');

            foreach (string line in lines)
            {
                //Skip if line is empty
                if (string.IsNullOrEmpty(line)) continue;

                string[] fields = line.Trim().Split(',');
                //MessageBox.Show(fields.Length.ToString());
                if (fields.Length != 1)
                {
                    MessageBox.Show("Problem parsing file, check format in Director");
                    continue;
                }
                else
                {
                    try
                    {
                        Director newDirector = new Director();
                        newDirector.DirectorName = fields[0];
                        DirectorsInfo.Add(newDirector);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return DirectorsInfo;
        }

    }
}
