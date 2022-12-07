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
using System.Windows.Shapes;
using DiabetApp.Classes;

namespace DiabetApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddProfileWin.xaml
    /// </summary>
    public partial class AddProfileWin
    {
        public static bool addprofwinopen = false;
        public static Profile profile = new Profile();
        private static CheckInput check = new CheckInput();
        public AddProfileWin()
        {
            InitializeComponent();
            DataContext = App.diary_View;
            addprofwinopen = true;
            Closed += AddProfileWin_Closed;
        }

        private void AddProfileWin_Closed(object sender, EventArgs e)
        {
            addprofwinopen = false;
        }

        private void autoBasal_Click(object sender, RoutedEventArgs e)
        {
            basalList.DataContext = null;
            profile.Dose_Profile.ToList().Where(c => c.ID_Type_Coefficient == 1).ToList().Clear();
            foreach (var item in profile.Dose_Profile.ToList())
            {
                if(item.ID_Type_Coefficient == 1)
                profile.Dose_Profile.Remove(item);
            }
            
            float wei = (float)App.diary_View.Selected_Person.Weight;
            float OSD = wei / 2;
            float basalcoef = (float)(OSD /24/4);
            Dose_Profile basals = new Dose_Profile();
            basals.ID_Type_Coefficient = 1;
            basals.Coefficient = basalcoef;
            basals.Time_Begin = new TimeSpan(0, 0, 0);
            basals.Time_End = new TimeSpan(23, 59, 59);
            profile.Dose_Profile.Add(basals);
            basalList.DataContext = profile.Dose_Profile.ToList().Where(c=> c.ID_Type_Coefficient == 1);
            //App.db.Basal.RemoveRange(App.db.Basal.ToList().Where(c=> c.))
        }

        private void autoCarb_Coef_Click(object sender, RoutedEventArgs e)
        {
            carbList.DataContext = null;
            profile.Dose_Profile.ToList().Where(c => c.ID_Type_Coefficient == 2).ToList().Clear();
            foreach (var item in profile.Dose_Profile.ToList())
            {
                if (item.ID_Type_Coefficient == 2)
                    profile.Dose_Profile.Remove(item);
            }
            float wei = (float)App.diary_View.Selected_Person.Weight;
            float OSD = wei/2;
            float coef = 450/OSD/10;
            Dose_Profile basals = new Dose_Profile()
            {
                ID_Type_Coefficient = 2,
                Coefficient = coef,
                Time_Begin = new TimeSpan(0, 0, 0),
                Time_End = new TimeSpan(23, 59, 59)
            };
            profile.Dose_Profile.Add(basals);
            carbList.DataContext = profile.Dose_Profile.ToList().Where(c => c.ID_Type_Coefficient == 2);
        }

        private void newCoef_Click(object sender, RoutedEventArgs e)
        {
            CreateCoefficient createCoefficient = new CreateCoefficient(1);
            createCoefficient.Show();
            createCoefficient.Closed += CreateCoefficient_Closed;
        }

        private void newBasal_Click(object sender, RoutedEventArgs e)
        {
            CreateCoefficient createCoefficient = new CreateCoefficient(2);
            createCoefficient.Show();
            createCoefficient.Closed += CreateCoefficient_Closed;
        }

        private void CreateCoefficient_Closed(object sender, EventArgs e)
        {
            carbList.DataContext =  profile.Dose_Profile.ToList().Where(c => c.ID_Type_Coefficient == 2);
            basalList.DataContext = profile.Dose_Profile.ToList().Where(c => c.ID_Type_Coefficient == 1);
        }

        private void createProf_Click(object sender, RoutedEventArgs e)
        {
            if (
                string.IsNullOrWhiteSpace(nameProfile.Text) ||
                string.IsNullOrWhiteSpace(maxGK.Text) ||
                string.IsNullOrWhiteSpace(minGK.Text) ||
                string.IsNullOrWhiteSpace(SensText.Text) ||
                profile.Dose_Profile.ToList().Where(c => c.ID_Type_Coefficient == 1).Count() == 0 ||
                profile.Dose_Profile.ToList().Where(c => c.ID_Type_Coefficient == 2).Count() == 0
               )
            {
                MessageBox.Show("Недостаточно аднных для создания профиля");
                return;
            }
            if ((float?)Convert.ToDouble(maxGK.Text) < (float?)Convert.ToDouble(minGK.Text))
            {
                MessageBox.Show("Мин.ГК должно быть меньше или равно Макс. ГК!!!");
                return;
            }
            if ((float?)Convert.ToDouble(SensText.Text) <= 0)
            {
                MessageBox.Show("Чувсвительность не может равняться нулю или быть отрицательной");
                return;
            }
            var newprof = App.db.Profile.Add(new Profile()
            {
                Name = nameProfile.Text,
                MaxGlucose = (float?)Convert.ToDouble(maxGK.Text),
                MinGlucose = (float?)Convert.ToDouble(minGK.Text),
                Sensitivity = (float?)Convert.ToDouble(SensText.Text),
                ID_Person = App.diary_View.Selected_Person.ID,
            });
            App.db.SaveChanges();
            foreach (var item in profile.Dose_Profile.ToList())
            {
                var dose_prof = App.db.Dose_Profile.Add(new Dose_Profile()
                {
                    ID_Type_Coefficient = item.ID_Type_Coefficient,
                    Time_Begin = item.Time_Begin,
                    Time_End = item.Time_End,
                    Coefficient = item.Coefficient,
                    ID_Profile = newprof.ID
                });
                App.db.SaveChanges();
            }
            App.diary_View.ProfilesAdd();
        }

        private void coefText_GotFocus(object sender, RoutedEventArgs e)
        {
            check.Previous_number = (float)((sender as TextBox).DataContext as Dose_Profile).Coefficient;
        }

        private void coefText_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            Dose_Profile dose_Profile = ((sender as TextBox).DataContext as Dose_Profile);
            if (dose_Profile != null)
            {
                foreach (var item in profile.Dose_Profile.ToList())
                {
                    if (item == dose_Profile)
                    {
                        item.Coefficient = check.CheckNumber(textBox.Text);
                    }
                }
                carbList.DataContext = profile.Dose_Profile.ToList().Where(c => c.ID_Type_Coefficient == 2);
                basalList.DataContext = profile.Dose_Profile.ToList().Where(c => c.ID_Type_Coefficient == 1);
            }
            check = null;
        }
    }
}
