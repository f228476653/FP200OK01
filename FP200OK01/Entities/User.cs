using FP200OK01.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FP200OK01.Entities
{
    public class User
    {
        private string password;
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password
        {
            get
            {
                return Encrypt.DecryptPassword(key, password);
            }
            set
            {

            }
        }

        public List<Favorite> Favorites { get; set; }
        public List<Review> Reviews { get; set; }

        private string key = "b14ca5898a4e4133bbce2ea2315a1916";

        public User()
        {

        }


        public void setPassword(string nPassword)
        {
            this.password = Encrypt.EncryptPassword(key, nPassword);
            this.Password = password;

        }

        public string getPassword()
        {
            return Encrypt.DecryptPassword(key, password);
        }
    }
}
