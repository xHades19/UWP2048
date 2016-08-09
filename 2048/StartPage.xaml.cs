using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238 

namespace _2048
{
    /// <summary>
    ///An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartPage : Page
    {
        private String LogWord;
        private String PassWord;
        private String RightLogWord = "Link";
        private String RightPassWord = "Link";

        public StartPage()
        {
            this.InitializeComponent();
        }

        private void Login()
        {
            if (LogWord==RightLogWord && PassWord == RightPassWord)
            {
                this.Frame.Navigate(typeof(MainPage));
            }else
            {
                WrongBlock.Visibility = Visibility.Visible;
            }
        }

        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Login();
            }else
            {
                WrongBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void appBarButton_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private void PassWordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PassWord = PassWordBox.Password;
        }

        private void LogBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LogWord = this.LogBox.Text;
        }

        private void appBarButton1_Click(object sender, RoutedEventArgs e)
        {

        }
        
    }
}
