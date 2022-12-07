using DiabetApp.Classes;
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
    /// Логика взаимодействия для Authorized.xaml
    /// </summary>
    public partial class Authorized : Page
    {
        public Authorized()
        {
            InitializeComponent();
            App.diary_View = new Diary_View();

        }

        private void Entrance_Click(object sender, RoutedEventArgs e)
        {
            Person person;
            person = App.db.Person.ToList().Find(c => c.Login == login.Text && c.Password == Md5Hesh.HeshCode(password.Password));
            if (person != null)
            {
                App.diary_View = new Diary_View(person);
                //NavigationService.Navigate(new Pages.GraphLine());
                NavigationService.Navigate(new Pages.CaseProfilePage());
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }

        private void Registr_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrPage());
        }
    }
}
