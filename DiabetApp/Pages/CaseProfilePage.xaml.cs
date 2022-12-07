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

namespace DiabetApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для CaseProfilePage.xaml
    /// </summary>
    public partial class CaseProfilePage : Page
    {
        public CaseProfilePage()
        {
            InitializeComponent();
            DataContext = App.diary_View;
            
        }

        private void caseProfileBut_Click(object sender, RoutedEventArgs e)
        {
            App.diary_View.Selected_Profile = (sender as Button).DataContext as Profile;
            App.diary_View.LoadDiaryView(); 
            NavigationService.Navigate(new CaseDiary());
        }

        private void updateProfileBut_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new Pages.CaseDiary());
            if (App.diary_View.Selected_Profile != null)
            {
                App.diary_View.Selected_Profile = (sender as Button).DataContext as Profile;
                App.diary_View.LoadDiaryView();
                Windows.UpdateProfile updateProfile = new Windows.UpdateProfile();
                updateProfile.Show();
            }
            else
            {
                App.diary_View.Selected_Profile = (sender as Button).DataContext as Profile;
                //App.diary_View = new Diary_View(App.diary_View.Selected_Person);
                Windows.UpdateProfile updateProfile = new Windows.UpdateProfile();
                updateProfile.Show();
            }

        }

        private void addProfile_Click(object sender, RoutedEventArgs e)
        {
            Windows.AddProfileWin addProfileWin = new Windows.AddProfileWin();
            addProfileWin.Show();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.Authorized());
        }
    }
}
