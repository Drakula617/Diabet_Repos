using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using DiabetApp.Classes;

namespace DiabetApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegistrPage.xaml
    /// </summary>
    public partial class RegistrPage : Page
    {
        public RegistrPage()
        {
            InitializeComponent();
        }

        bool IsWeigth(string str)
        {
            try
            {
                if (Convert.ToInt32(str) > 0)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Возраст введен не верно");
                    return false;
                }
            }
            catch
            { 
                MessageBox.Show("Возраст введен не верно");
                return false;
            }
        }

        private void Registr_Click(object sender, RoutedEventArgs e)
        {
            login.Text.Trim();
            password.Password.Trim();
            if (App.db.Person.ToList().Find(c => c.Login == login.Text) != null)
            {
                MessageBox.Show("Пользователь с таким логином уже существует");
                return;
            }
            if (login.Text.Length == 0)
            {
                MessageBox.Show("Не введен логин");
                return;
            }
            if (password.Password.Length == 0)
            {
                MessageBox.Show("Не введен пароль");
                return;
            }
            if (IsWeigth(weigth.Text) == false)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(FName.Text) && string.IsNullOrWhiteSpace(LName.Text) && string.IsNullOrWhiteSpace(MName.Text))
            {
                MessageBox.Show("Не введено ФИО");
                return;
            }
            App.db.Person.Add(new Person()
            {
                FName = FName.Text,
                LName = LName.Text,
                MName = MName.Text,

                Weight = (float?)Convert.ToDouble(weigth.Text),
                Login = login.Text,
                Password = Md5Hesh.HeshCode(password.Password)
            });
            App.db.SaveChanges();
            MessageBox.Show("Регистрация прошла успешно");
            NavigationService.Navigate(new Authorized());
        }

        private void FName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (Regex.IsMatch(textBox.Text, "[^А-Я-а-я]"))
            {
                MessageBox.Show("Можно вводить только русские заглавные или строчные символы!!!", "Ошибка!!!");
                textBox.Text = textBox.Text.Remove(textBox.Text.Length - 1);
                textBox.SelectionStart = textBox.Text.Length;
            }
        }
    }
}
