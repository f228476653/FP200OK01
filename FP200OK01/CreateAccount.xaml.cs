using FP200OK01.Entities;
using FP200OK01.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FP200OK01
{
    /// <summary>
    /// Interaction logic for CreateAccount.xaml
    /// </summary>
    public partial class CreateAccount : Page
    {
        public CreateAccount()
        {
            InitializeComponent();
            // bind create button event
            SubCreateButton.Click += SubCreateButtonClick;
            // bind back button event
            back.Click += GoBack;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            User u = new User();
            // Navigate to MainPage
            this.NavigationService.Navigate(new MainPage(u));
        }

        private void SubCreateButtonClick(Object o, EventArgs e)
        {
            // empty check
            if (CreateUserNameTextBox.Text.Length == 0 || CreatePasswordTextBox.Text.Length == 0)
            {
                CreateHintTextBlock.Text = "Please input both user name and password";
            }
            else
            {
                using (var ctx = new MovieContext())
                {
                    //Save User
                    var count = ctx.User.Where(x => x.UserName == CreateUserNameTextBox.Text).Count();

                    if (count == 0)
                    {
                        User newUser = new User();
                        newUser.UserName = CreateUserNameTextBox.Text;
                        newUser.setPassword(CreatePasswordTextBox.Text);
                        ctx.User.Add(newUser);
                        ctx.SaveChanges();
                        this.NavigationService.Navigate(new MainPage(new User()));

                    }
                    else
                    {
                        // Multi User Check
                        CreateHintTextBlock.Text = "The user name is used. Please input another one!";
                    }
                }
            }

        }
    }
}
