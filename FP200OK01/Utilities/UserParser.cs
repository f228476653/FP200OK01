using FP200OK01.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP200OK01.Utilities
{
    class UserParser
    {
        public static List<User> Users;
        public static List<User> ParseUser(string fileContents)
        {
            Users = new List<User>();
            string[] lines = fileContents.Split('\n');

            foreach (string line in lines)
            {
                //Skip if line is empty
                if (string.IsNullOrEmpty(line)) continue;

                string[] fields = line.Trim().Split(',');
                //MessageBox.Show(fields.Length.ToString());
                if (fields.Length != 2)
                {
                    System.Windows.MessageBox.Show("Problem parsing file, check format");
                    continue;
                }
                else
                {
                    try
                    {
                        User newUser = new User();
                        newUser.UserName = fields[0];
                        newUser.setPassword(fields[1]);
                        Users.Add(newUser);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return Users;
        }
    }
}
