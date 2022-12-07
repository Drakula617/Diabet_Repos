using System.ComponentModel;
namespace DiabetApp
{
    public class PropertyClass :INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyname = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
}
