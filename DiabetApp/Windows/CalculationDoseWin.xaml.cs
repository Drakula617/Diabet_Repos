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

namespace DiabetApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для CalculationDoseWin.xaml
    /// </summary>
    public partial class CalculationDoseWin
    {
        public CalculationDoseWin()
        {
            InitializeComponent();
            DataContext = App.diary_View;
            //Sens.DataContext = App.diary_View.Selected_Profile;
            //MaxGK.DataContext = App.diary_View.Selected_Profile;
            //if(rowOfDiary.Diary_Products != null)
            //{
            //    ListProduct.DataContext = rowOfDiary.Diary_Products.ToList();
            //}
        }

        private void HeText_Initialized(object sender, EventArgs e)
        {

        }
    }
}
