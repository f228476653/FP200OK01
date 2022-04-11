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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            // hide error msg block
            HintTextBlock.Visibility = Visibility.Hidden;
            toggleEvent();
        }
        //bind event
        private void toggleEvent()
        {
            Back.Click += navigateBackButton_Click;
            SubLoginButton.Click += checkPwd;
        }
        // Check Password
        private void checkPwd(object sender, RoutedEventArgs e)
        {
            // if there's inpit in userName text & PasswordText
            User user = null;
            if (UserNameTextBox.Text.Count() == 0 || PasswordTextBox.Text.Count() == 0)
            {
                HintTextBlock.Text = "Invalid Input, please input valid input";
                HintTextBlock.Visibility = Visibility.Visible;
            }
            using (var ctx = new MovieContext())
            {
                try
                {
                    // find user by user name
                    user = ctx.User.Where(x => x.UserName == UserNameTextBox.Text).First();
                    // validate password
                    if (PasswordTextBox.Text == user.getPassword().Trim())
                    {
                        HintTextBlock.Text = "";
                        UserNameTextBox.Text = "";
                        PasswordTextBox.Text = "";
                        this.NavigationService.Navigate(new MainPage(user));
                    }
                    else
                    {
                        HintTextBlock.Visibility = Visibility.Visible;
                        HintTextBlock.Text = "Wrong input information";
                    }
                    
                }
                catch (Exception ex)
                {
                    HintTextBlock.Text = "User name is wrong!";
                    HintTextBlock.Visibility = Visibility.Visible;
                    ErrorLog.ErrorLogging(ex);
                }

            }

        }

        void navigateBackButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back one page from this page, if there is an entry
            // in back navigation history
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.Navigate(new MainPage(new User()));
            }
            else
            {
                MessageBox.Show("No entries in back navigation history.");
            }
        }
    }
}
