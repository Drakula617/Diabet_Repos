using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для CreateCoefficient.xaml
    /// </summary>
    public partial class CreateCoefficient
    {
        public static List<Coefficient> carbCoef = new List<Coefficient>();
        private static int typeint;
        private static CheckInput checkcoef = new CheckInput();
        public CreateCoefficient(int type)
        {
            typeint = type;
            InitializeComponent();
            DataContext = App.diary_View;
            carbCoef.Clear();
            ListViewCoef.ItemsSource = carbCoef.ToList();
            App.diary_View.Selected_Coefficient = new Coefficient()
            {
                Coef = 0,
                TimeBegin = new TimeSpan(0, 0, 0),
                TimeEnd = new TimeSpan(0, 15, 0)
            };
            CheckBut();
            coeftext.Text = 0.ToString();
            downMinute.IsEnabled = false;
        }

        public void LoadBasal()
        {
            if (AddProfileWin.addprofwinopen == false)
            {
                var remove_prof_basal = App.db.Dose_Profile.RemoveRange(App.db.Dose_Profile.ToList().Where(c => c.Profile.ID == App.diary_View.Selected_Profile.ID && c.ID_Type_Coefficient == 1));
                App.db.SaveChanges();
            }
            else
            {
                foreach (var item in AddProfileWin.profile.Dose_Profile.ToList())
                {
                    if (item.ID_Type_Coefficient == 1)
                        AddProfileWin.profile.Dose_Profile.Remove(item);
                }
            }
            foreach (var item in carbCoef.ToList())
            {
                if (carbCoef.First() == item && carbCoef.Last() != item)
                {
                    item.TimeBegin = new TimeSpan(0, 0, 0);
                    item.TimeEnd = item.TimeEnd.Add(-new TimeSpan(0, 0, 1));
                }
                else if (carbCoef.First() != item && carbCoef.Last() == item)
                {
                    item.TimeEnd = new TimeSpan(23, 59, 59);
                }
                else
                {
                    item.TimeEnd = item.TimeEnd.Add(-new TimeSpan(0, 0, 1));
                }
                if (item.TimeEnd >= new TimeSpan(1, 0, 0, 0))
                {
                    item.TimeEnd = new TimeSpan(23, 59, 59);
                }
                if (AddProfileWin.addprofwinopen == true)
                {
                    AddProfileWin.profile.Dose_Profile.Add(new Dose_Profile()
                    {
                        ID_Type_Coefficient = 1,
                        Time_Begin = item.TimeBegin,
                        Time_End = item.TimeEnd,
                        Coefficient = (float?)item.Coef,
                    });
                    
                }
                else
                { 

                    App.db.Dose_Profile.Add(new Dose_Profile()
                    {
                        ID_Profile = App.diary_View.Selected_Profile.ID,
                        ID_Type_Coefficient = 1,
                        Time_Begin= item.TimeBegin,
                        Time_End= item.TimeEnd,
                        Coefficient= (float?)item.Coef,
                    });
                    App.db.SaveChanges();
                    
                }
            }
            
        }
        public void LoadCoef()
        {

            if (AddProfileWin.addprofwinopen == false)
            {
                var removecoef = App.db.Dose_Profile.RemoveRange(App.db.Dose_Profile.ToList().Where(c => c.Profile == App.diary_View.Selected_Profile && c.ID_Type_Coefficient == 2));
                App.db.SaveChanges();
                App.diary_View.CarbCoef.Clear();
            }
            else
            {
                foreach (var item in AddProfileWin.profile.Dose_Profile.ToList())
                {
                    if (item.ID_Type_Coefficient == 2)
                        AddProfileWin.profile.Dose_Profile.Remove(item);
                }
            }
            foreach (var item in carbCoef.ToList())
            {
                if (carbCoef.First() == item && carbCoef.Last() != item)
                {
                    item.TimeBegin = new TimeSpan(0, 0, 0);
                    item.TimeEnd = item.TimeEnd.Add(-new TimeSpan(0, 0, 1));
                }
                else if (carbCoef.First() != item && carbCoef.Last() == item)
                {
                    item.TimeEnd = new TimeSpan(23, 59, 59);
                }
                else
                {
                    item.TimeEnd = item.TimeEnd.Add(-new TimeSpan(0, 0, 1));
                }
                if (item.TimeEnd >= new TimeSpan(1, 0, 0, 0))
                {
                    item.TimeEnd = new TimeSpan(23, 59, 59);
                }
                if (AddProfileWin.addprofwinopen == true)
                {
                    AddProfileWin.profile.Dose_Profile.Add(new Dose_Profile()
                    {
                        ID_Type_Coefficient = 2,
                        Time_Begin = item.TimeBegin,
                        Time_End = item.TimeEnd,
                        Coefficient = (float?)item.Coef,
                    });
                }
                else
                {
                    var carbnew = App.db.Dose_Profile.Add(new Dose_Profile()
                    {
                        ID_Type_Coefficient = 2,
                        ID_Profile = App.diary_View.Selected_Profile.ID,
                        Time_Begin = item.TimeBegin,
                        Time_End = item.TimeEnd,
                        Coefficient = (float?)item.Coef
                    });
                    App.db.SaveChanges();
                }
            
            }
           
        }

        public bool LoadCoefInDatabase()
        {

            if (App.diary_View.Selected_Coefficient.TimeEnd >= new TimeSpan(1, 0, 0, 0))
            {
                nextbut.IsEnabled = false;
            }
            else
            {
                nextbut.IsEnabled = true;
            }
            if (carbCoef.Last().TimeEnd - carbCoef.First().TimeBegin >= new TimeSpan(1, 0, 0, 0) || App.diary_View.Selected_Coefficient.TimeEnd >= new TimeSpan(1, 0, 0, 0))
            {
                var delete = MessageBox.Show("Задействовать выставленные коэффициенты?", "Предупреждение", MessageBoxButton.YesNo);
                if (delete == MessageBoxResult.Yes)
                {
                    if (typeint == 1)
                    {
                        LoadCoef();
                    }
                    else
                    {
                        LoadBasal();
                    }
                    MessageBox.Show("Готово");
                    if (AddProfileWin.addprofwinopen == false)
                    {
                        App.diary_View.UpdateGraph();
                        App.diary_View.collectionDiary_Line.Refresh();
                        App.diary_View.CalculationSummDose();
                    }
                    carbCoef.Clear();
                    nextbut.IsEnabled = false;
                    Close();
                    return true;
                }
                else return true;
            }
            else
            {
                return false;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CheckBut();
            if (carbCoef.Count == 0)
            {
                carbCoef.Add(new Coefficient()
                {
                    TimeBegin = new TimeSpan(0, 0, 0),
                    TimeEnd = App.diary_View.Selected_Coefficient.TimeEnd,
                    Coef = checkcoef.Previous_number
                });
                ListViewCoef.ItemsSource = carbCoef.ToList();
                if (LoadCoefInDatabase() == true)
                {
                    CheckBut();
                    return;
                }
                App.diary_View.Selected_Coefficient.TimeBegin = App.diary_View.Selected_Coefficient.TimeEnd;

                App.diary_View.Selected_Coefficient.TimeEnd = App.diary_View.Selected_Coefficient.TimeEnd.Add(TimeSpan.FromMinutes(15));

                CheckBut();
                return;
            }
            carbCoef.Add(new Coefficient()
            {
                TimeBegin = App.diary_View.Selected_Coefficient.TimeBegin,
                TimeEnd = App.diary_View.Selected_Coefficient.TimeEnd,
                Coef = checkcoef.Previous_number
            });
            ListViewCoef.ItemsSource = carbCoef.ToList();

            if (LoadCoefInDatabase() == true)
            {
                CheckBut();
                return;
            }
            App.diary_View.Selected_Coefficient.TimeBegin = App.diary_View.Selected_Coefficient.TimeEnd;
            App.diary_View.Selected_Coefficient.TimeEnd = App.diary_View.Selected_Coefficient.TimeEnd.Add(TimeSpan.FromMinutes(15));
            CheckBut();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
        public void BlockBut()
        {
            upHour.IsEnabled = false;
            upMinute.IsEnabled = false;
            downHour.IsEnabled = false;
            downMinute.IsEnabled = false;
        }
        public void CheckBut()
        {
            BlockBut();
            TimeSpan timeSpan = App.diary_View.Selected_Coefficient.TimeEnd.Subtract(App.diary_View.Selected_Coefficient.TimeBegin);
            if (timeSpan == new TimeSpan(0, 0, 0) || App.diary_View.Selected_Coefficient.TimeEnd == new TimeSpan(0, 0, 0))
            {
                BlockBut();
                return;
            }
            upHour.IsEnabled = true;
            upMinute.IsEnabled = true;
            if (timeSpan > new TimeSpan(0, 15, 0))
            {
                downMinute.IsEnabled = true;
                if (timeSpan > new TimeSpan(1, 0, 0))
                {
                    downHour.IsEnabled = true;
                }
            }
            if (App.diary_View.Selected_Coefficient.TimeEnd > new TimeSpan(23, 0, 0))
            {
                upHour.IsEnabled = false;
                if (App.diary_View.Selected_Coefficient.TimeEnd == new TimeSpan(1, 0, 0, 0))
                {
                    upMinute.IsEnabled = false;
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            App.diary_View.Selected_Coefficient.TimeEnd = App.diary_View.Selected_Coefficient.TimeEnd.Add(new TimeSpan(1, 0, 0));
            CheckBut();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            App.diary_View.Selected_Coefficient.TimeEnd = App.diary_View.Selected_Coefficient.TimeEnd.Add(-new TimeSpan(1, 0, 0));
            CheckBut();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            App.diary_View.Selected_Coefficient.TimeEnd = App.diary_View.Selected_Coefficient.TimeEnd.Add(new TimeSpan(0, 15, 0));
            CheckBut();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            App.diary_View.Selected_Coefficient.TimeEnd = App.diary_View.Selected_Coefficient.TimeEnd.Add(-new TimeSpan(0, 15, 0));
            CheckBut();
        }

        private void coeftext_LostFocus(object sender, RoutedEventArgs e)
        {
            coeftext.Text = checkcoef.CheckNumber(coeftext.Text).ToString();
        }

        private void coeftext_GotFocus(object sender, RoutedEventArgs e)
        {
            checkcoef.Previous_number = (float)Convert.ToDouble(coeftext.Text);
        }
    }
}
