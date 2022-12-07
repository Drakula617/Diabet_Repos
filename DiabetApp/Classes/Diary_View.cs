using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Data;
using LiveCharts;
using LiveCharts.Wpf;
using System.ComponentModel;
using System.Threading;

namespace DiabetApp
{
    public class Diary_View : PropertyClass
    {
        //ICollectionView
        public ICollectionView collectionProduct;
        public ICollectionView collectionDiary_Line;
        public ICollectionView collectionDiary_Product;

        //Объекты классов БД
        Person selected_Person; 
        Profile selected_Profile;
        GeneralDiary_Person selected_GeneralDiary_Person;

        Diary_Product selected_Diary_Product;
        Type_Product selected_Type_Product;
        Product selected_Product;
        public Person Selected_Person
        {
            get { return selected_Person; }
            set
            {
                selected_Person = value;
                OnPropertyChanged("Selected_Person");
                //App.diary_View = new Diary_View(); 
            }
        }
        public Profile Selected_Profile
        {
            get { return selected_Profile; }
            set
            {
                selected_Profile = value;
                UpdateGraph();
                if (collectionDiary_Line != null)
                {
                    collectionDiary_Line.Refresh();
                }
                OnPropertyChanged("Selected_Profile");
            }
        }
        public GeneralDiary_Person Selected_GeneralDiary_Person
        {
            get { return selected_GeneralDiary_Person; }
            set { selected_GeneralDiary_Person = value; OnPropertyChanged("GeneralDiary_Person"); }
        }
        public Diary_Product Selected_Diary_Product
        {
            get { return selected_Diary_Product; }
            set
            {
                selected_Diary_Product = value;
                OnPropertyChanged("Selected_Diary_Product");
            }
        }
        public Type_Product Selected_Type_Product
        {
            get { return selected_Type_Product; }
            set { selected_Type_Product = value; OnPropertyChanged("Selected_Type_Product"); }

        }
        public Product Selected_Product
        {
            get { return selected_Product; }
            set { selected_Product = value; OnPropertyChanged("Selected_Product"); }
        }
        //Объекты клвссов приложения

        Diary_Line selected_Diary_Lines;

        DateTime selected_DateTime;
        Coefficient selected_Coefficient = new Coefficient();

        public Diary_Line Selected_Diary_Lines
        {
            get { return selected_Diary_Lines; }
            set { selected_Diary_Lines = value; OnPropertyChanged("Selected_Diary_Lines"); }
        }

        public DateTime Selected_DateTime
        {
            get { return selected_DateTime; }
            set
            {
                selected_DateTime = value;
                UpdateGraph();
                Selected_GeneralDiary_Person = App.db.GeneralDiary_Person.ToList().Find(c => 
                c.Date == Selected_DateTime && c.ID_Person == Selected_Person.ID);
                collectionDiary_Line.Filter = UpdateDatetime;
                //collectionDiary_Line.Refresh();
                OnPropertyChanged("Selected_DateTime");
            }
        }
        public Coefficient Selected_Coefficient
        {
            get { return selected_Coefficient; }
            set { selected_Coefficient = value; OnPropertyChanged("Selected_Coefficient"); }
        }
        //ObservableCollection коллекции
        public ObservableCollection<Profile> Profiles { get; set; } = new ObservableCollection<Profile>(); 
        public ObservableCollection<GeneralDiary_Person> Gen_D_P { get; set; } = new ObservableCollection<GeneralDiary_Person>();        
        public ObservableCollection<Diary_Line> Diary_Lines { get; set;} = new ObservableCollection<Diary_Line>();
        public ObservableCollection<Diary_Product> Diary_Products { get; set; }= new ObservableCollection<Diary_Product>();
        public ObservableCollection<Type_Product> Type_Products { set; get; } = new ObservableCollection<Type_Product>();
        public ObservableCollection<Product> Products { get; set; }= new ObservableCollection<Product>();


        public ObservableCollection<Dose_Profile> Basals { get; set; } = new ObservableCollection<Dose_Profile>();
        public ObservableCollection<Coefficient> basalCoef = new ObservableCollection<Coefficient>();
        public ObservableCollection<Coefficient> BasalCoef
        {
            get { return basalCoef; }
            set { basalCoef = value; OnPropertyChanged("BasalCoef"); }
        }

        public ObservableCollection<Dose_Profile> carbCoef = new ObservableCollection<Dose_Profile>();
        public ObservableCollection<Dose_Profile> CarbCoef
        {
            get { return carbCoef; }
            set { carbCoef = value; OnPropertyChanged("CarbCoef"); }
        }


        //Коллекции точек для вывода графиков

        public SeriesCollection seriesPointsGlucose = new SeriesCollection();
        public SeriesCollection seriesPointsBasal = new SeriesCollection();
        public SeriesCollection seriesPointsCoef = new SeriesCollection();

        public SeriesCollection SeriesPointsGlucose
        { get { return seriesPointsGlucose; } set { seriesPointsGlucose = value; OnPropertyChanged("SeriesPointsGlucose"); } }
        public SeriesCollection SeriesPointsBasal
        { get { return seriesPointsBasal; } set { seriesPointsBasal = value; OnPropertyChanged("SeriesPointsBasal"); } }
        public SeriesCollection SeriesPointsCoef
        { get { return seriesPointsCoef; } set { seriesPointsCoef = value; OnPropertyChanged("SeriesPointsCoef"); } }
        public List<string> TimesGluc { get; set; } = new List<string>();
        public List<string> TimesCoef { get; set; } = new List<string>();
        public List<string> TimesBasal { get; set; } = new List<string>();



        //Сумма коэффициентов за сутки
        public float summDose = 0;
        public float summBasal = 0;
        public float SummBasal
        {
            get { return summBasal; }
            set { summBasal = value; OnPropertyChanged("SummBasal"); }
        }
        public float SummDose
        {
            get { return summDose; }
            set { summDose = value; OnPropertyChanged("SummDose"); }
        }
        public void CalculationSummDose()
        {
            SummDose = (float)Diary_Lines.ToList().Where(c => 
            c.GeneralDiary_Person.Date == Selected_DateTime).ToList().Sum(c => c.Dose);
        }

        //загрузка списка продуктов
        public void ProductsAdd()
        {
            Products.Clear();
            foreach (var item in App.db.Product.ToList().Distinct())
            {
                
                var prod = (new Product()
                {
                    ID = item.ID,
                    Name = item.Name,
                    Calories = item.Calories,
                    Carbohydrates = item.Carbohydrates,
                    Fats = item.Fats,
                    Protein= item.Protein,
                    Glycemic_Index = item.Glycemic_Index,

                    Type_Product = item.Type_Product,
                    ID_Type_Product = item.ID_Type_Product
                });
                if (Products.ToList().Where(c => c.Name == item.Name).ToList().Count == 0)
                {
                    Products.Add(prod);
                }
                else
                {
                    
                }
            }
            
        }
        public void Type_ProductsAdd()
        {
            Type_Products.Clear();
            foreach (var item in App.db.Type_Product)
            {
                Type_Products.Add(item);
            }
        }

        //загрузка профилей пользователя
        public void ProfilesAdd()
        {
            Profiles.Clear();
            foreach (var item in App.db.Profile.ToList().Where(c=> c.Person.ID == Selected_Person.ID).ToList())
            {
                Profiles.Add(item);
            }
        }

        //загрузка журнала пользователя
        public void Diary_LineAdd()
        {
            try
            {
                Diary_Products.Clear();
            }
            catch
            { 
                
            }
            
            if (Selected_Person != null)
            {
                
         
                if (Selected_DateTime.Year < 10)
                { 
                    List<DateTime?> dates = new List<DateTime?>();
                    dates = App.db.Diary_Line.ToList().Where(c => c.GeneralDiary_Person.ID_Person == Selected_Person.ID).ToList().Select(c => c.GeneralDiary_Person.Date).ToList();
                    if (dates.Count == 0)
                    {
                        Selected_DateTime = DateTime.Now.Date;
                        var addGenGeneralDiary_Person = App.db.GeneralDiary_Person.Add(new GeneralDiary_Person()
                        { 
                            ID_Person = Selected_Person.ID,
                            Date = DateTime.Now.Date
                        });
                        App.db.SaveChanges();
                        Selected_GeneralDiary_Person = addGenGeneralDiary_Person;
                    }
                    else
                    {
                        Selected_DateTime = dates.Max().Value.Date;
                    }
                }

                Selected_GeneralDiary_Person.Person = Selected_Person;
                foreach (var item in App.db.Diary_Line.ToList().Where(c => c.GeneralDiary_Person.Person == Selected_Person).ToList())
                {
                    Diary_Lines.Add(item);
                } 

                UpdateGraph();
            }
            else
            {
               
            }
        }

        //обновление журнала по выбранной дате
        public bool UpdateDatetime(object obj)
        {
            Diary_Line Diary_Line = (Diary_Line)obj;
            if (Diary_Line.GeneralDiary_Person.Date == Selected_DateTime)
            {
                return true;
            }
            return false;
        }

        //методы для вывода графиков
        public void UpdateGraph()
        {       
                
                BasalLoad();
                Carb_CoefLoad();
                GraphGluc();
                GraphCoef();
                GraphBasal();
        }
        public void GraphGluc()
        {
            SeriesPointsGlucose = new SeriesCollection();
            TimesGluc.Clear();
            ChartValues<double> glucose = new ChartValues<double>();
            ChartValues<double> he = new ChartValues<double>();
            ChartValues<double> dose = new ChartValues<double>();
            LineSeries line = new LineSeries() { Title = "Сахар"};
            ColumnSeries columnHe = new ColumnSeries() { Title = "ХЕ"};
            ColumnSeries columnDose = new ColumnSeries() { Title = "Доза"};
            
            foreach (var item in Diary_Lines.ToList().Where(c => c.GeneralDiary_Person.Date == Selected_DateTime).ToList())
            {
                TimesGluc.Add(item.Time.ToString());
                glucose.Add(Math.Round((double)item.Glucose,1));
                he.Add(Math.Round(item.He,1));
                dose.Add(Math.Round(item.Dose,1));
            }
            line.Values = glucose;
            columnDose.Values = dose;
            columnHe.Values = he;
            SeriesPointsGlucose.Add(line);
            SeriesPointsGlucose.Add(columnHe);
            SeriesPointsGlucose.Add(columnDose);
        }
        public void GraphCoef()
        {
            SeriesPointsCoef = new SeriesCollection();
            LineSeries line1 = new LineSeries() { Title = "Коэф-т"};
            ChartValues<double> coef = new ChartValues<double>();
            TimesCoef.Clear();
            foreach (var item in CarbCoef.ToList())
            {
                coef.Add(Math.Round((double)item.Coefficient,1));
                TimesCoef.Add(item.Time_Begin.ToString());
            }
            line1.Values = coef;
            
            SeriesPointsCoef.Add(line1);
        }
        public void GraphBasal()
        {

            SeriesPointsBasal = new SeriesCollection();
            LineSeries line2 = new LineSeries() { Title = "Коэф-т" };
            ChartValues<double> basal = new ChartValues<double>();
            
            TimesBasal.Clear();
            foreach (var item in App.diary_View.BasalCoef.ToList())
            {
                basal.Add(Math.Round((double)item.Coef,2));
                TimesBasal.Add(item.TimeBegin.ToString());
            }
            line2.Values = basal;
            SeriesPointsBasal.Add(line2);
        }
        public void BasalLoad()
        {
            BasalCoef.Clear();
            SummBasal = 0;
            foreach (var item in App.db.Dose_Profile.ToList().Where(c=> c.ID_Profile == Selected_Profile.ID && c.ID_Type_Coefficient == 1))
            {
                BasalCoef.Add(new Coefficient()
                {
                    ID = item.ID,
                    TimeBegin = (TimeSpan)item.Time_Begin,
                    TimeEnd = (TimeSpan)item.Time_End,
                    Coef = (double)item.Coefficient
                });
                SummBasal += (float)((TimeSpan)item.Time_End - (TimeSpan)item.Time_Begin).TotalMinutes / 15 * (float)item.Coefficient;
            }
        }
        public void Carb_CoefLoad()
        {
            CarbCoef.Clear();
           
            foreach (var item in App.db.Dose_Profile.ToList().Where(c => c.ID_Profile == Selected_Profile.ID && c.ID_Type_Coefficient == 2))
            {
                CarbCoef.Add(new Dose_Profile()
                {
                    ID = item.ID,
                    Time_Begin = (TimeSpan)item.Time_Begin,
                    Time_End = (TimeSpan)item.Time_End,
                    Coefficient = item.Coefficient
                });
            }
            App.diary_View.CalculationSummDose();
        }

        //Конструктор до авторизации
        public Diary_View()
        {
            collectionProduct = CollectionViewSource.GetDefaultView(Products);
            collectionDiary_Line = CollectionViewSource.GetDefaultView(Diary_Lines);
            collectionDiary_Product = CollectionViewSource.GetDefaultView(Diary_Products);
            collectionProduct.Filter = FilterProduct;
        }

        //конструктор после авторизации
        public Diary_View(Person person)
        {
            collectionProduct = CollectionViewSource.GetDefaultView(Products);
            collectionDiary_Line = CollectionViewSource.GetDefaultView(Diary_Lines);
            collectionDiary_Product = CollectionViewSource.GetDefaultView(Diary_Products);
            collectionProduct.Filter = FilterProduct;
            if (person == null)
            {

            }
            else
            {
                Selected_Person = person;
                ProfilesAdd();
                if (Selected_Profile != null)
                {
                   LoadDiaryView();
                }
                else
                {

                }
                
            }
        }

        //Загрузка данных для пользователя с выбранным профилем
        public void LoadDiaryView()
        {
            Type_ProductsAdd();
            ProductsAdd();
            Diary_LineAdd();


        }
        private void Diary_Lines_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            collectionDiary_Line.Refresh();
            App.diary_View.CalculationSummDose();
        }
        //Фильтрация продуктов по типу
        public string filter_Type_Product;
        public string Filter_Type_Product
        {
            get { return filter_Type_Product; }
            set
            {
                filter_Type_Product = value;
                collectionProduct.Refresh();
                OnPropertyChanged("Filter_Type_Product");
            }
        }
        public bool FilterProduct(object obj)
        {
            Product product = (Product)obj;
            if (string.IsNullOrWhiteSpace(Filter_Type_Product))
            {
                return true;
            }
            else if(product.Type_Product.Name.Contains(Filter_Type_Product))
            {
                return true;
            }
            return false;
        }

    }
}
