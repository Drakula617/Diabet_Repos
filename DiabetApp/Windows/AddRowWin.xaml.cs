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

namespace DiabetApp.Window
{
    /// <summary>
    /// Логика взаимодействия для AddRowWin.xaml
    /// </summary>
    public partial class AddRowWin
    {
        public AddRowWin()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (App.diary_View.Selected_GeneralDiary_Person == null)
            {
                App.diary_View.Selected_GeneralDiary_Person = App.db.GeneralDiary_Person.Add(new GeneralDiary_Person()
                {
                    ID_Person = App.diary_View.Selected_Person.ID,
                    Date = App.diary_View.Selected_DateTime.Date

                });
                App.db.SaveChanges();
            }

            else
            {
                var addRow = App.db.Diary_Line.Add(new Diary_Line()
                {
                    ID_GeneralDiary_Person = App.diary_View.Selected_GeneralDiary_Person.ID,
                    Time = new TimeSpan(timepicker.DateTime.Hour, timepicker.DateTime.Minute, timepicker.DateTime.Second),
                    Glucose = (float?)Convert.ToDouble(glucText.Text),
                    IsDoseLower = false
                });
                App.db.SaveChanges();

                App.diary_View.Gen_D_P.Add(new GeneralDiary_Person()
                {
                    ID = App.diary_View.Selected_GeneralDiary_Person.ID,
                    Person = App.diary_View.Selected_GeneralDiary_Person.Person,
                    Date = App.diary_View.Selected_GeneralDiary_Person.Date

                });
                App.diary_View.Diary_Lines.Add(addRow);
                
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
