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
    /// Логика взаимодействия для GraphLine.xaml
    /// </summary>
    public partial class GraphLine : Page
    {
        public GraphLine()
        {
            InitializeComponent();
            //App.diary_View.TimeSpanControl();
            DataContext = App.diary_View;
            //SpanControl();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Windows.CreateCoefficient createCoefficient = new Windows.CreateCoefficient(1);
            createCoefficient.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Windows.CreateCoefficient createBasal = new Windows.CreateCoefficient(2);
            createBasal.Show();
        }
    }
}
