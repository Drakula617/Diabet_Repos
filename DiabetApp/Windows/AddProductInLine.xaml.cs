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
    /// Логика взаимодействия для AddProductInLine.xaml
    /// </summary>
    public partial class AddProductInLine
    {
        public AddProductInLine()
        {
            InitializeComponent();
            App.diary_View.Selected_Diary_Product = new Diary_Product()
            {
                
                Diary_Line = App.diary_View.Selected_Diary_Lines
            };
            DataContext = App.diary_View;
            gramText.DataContext = App.diary_View.Selected_Diary_Product;
        }

        private void ProdBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProdBox.SelectedItem as Product != null)
            {
                App.diary_View.Selected_Product = ProdBox.SelectedItem as Product;
                App.diary_View.Selected_Diary_Product.Product = App.diary_View.Selected_Product;
                //App.diary_View.Selected_Diary_Product.ProductCalculation();
                //Diary_Product_Property diary_Product_Property = App.diary_View.Selected_Diary_Product_Property;

                //diary_Product_Property.Id_Product = App.diary_View.Selected_Product.ID;
                //diary_Product_Property.Product = App.diary_View.Selected_Product;
                //App.diary_View.Selected_Diary_Product_Property = diary_Product_Property;
                //App.diary_View.Selected_Diary_Product_Property.ProductCalculation();
            }
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            var addDiary_Produact = App.db.Diary_Product.Add(App.diary_View.Selected_Diary_Product);
            //var addDiary_Produact = App.db.Diary_Product.Add()
            App.db.SaveChanges();
            App.diary_View.Diary_Products.Add(addDiary_Produact);
            App.diary_View.collectionDiary_Line.Refresh();
            App.diary_View.CalculationSummDose();
            //Diary_Product_Property diary = App.diary_View.Selected_Diary_Product_Property;
            //var addDiary_Produact =  App.db.Diary_Product.Add(new Diary_Product()
            //{
            //    Product = diary.Product,
            //    ID_Diary_Line = App.diary_View.Selected_Diary_Lines.ID,
            //    Grams = (float)Convert.ToDouble(diary.Gramm)
            //});
            //App.db.SaveChanges();
            //App.diary_View.Selected_Diary_Lines.Diary_Product.Add(addDiary_Produact);
            //App.diary_View.collectionDiary_Line.Refresh();
            //App.diary_View.CalculationSummDose();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        
    }
}
