﻿using System;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindow mainWindow;
        string userName = string.Empty;
        public MainWindow()
        {
            // load PrePage to the Frame
            InitializeComponent();
            mainWindow = Window.GetWindow(this) as MainWindow;
            Frame frame = (Frame)mainWindow.FindName("frame");
            frame.Navigate(new PrePage());


        }

    }
}
