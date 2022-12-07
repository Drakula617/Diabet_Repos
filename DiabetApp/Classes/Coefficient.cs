using System;

namespace DiabetApp
{
    public class Coefficient: PropertyClass
    {
        TimeSpan timeBegin;
        TimeSpan timeEnd;
        double coef;
        int id;
        int hourBegin;
        int hourEnd;
        int minuteBegin;
        int minuteEnd;
        public void HourMinuteUpdate()
        {
            MinuteBegin = TimeBegin.Minutes;
            HourBegin = TimeBegin.Hours;
            MinuteEnd = TimeEnd.Minutes;
            HourEnd = TimeEnd.Hours;
        }
        public int ID
        { get { return id; } set { id = value; OnPropertyChanged("ID"); } }
        public int HourBegin
        {
            get { return hourBegin; }
            set { hourBegin = value; OnPropertyChanged("HourBegin"); }
        }
        public int HourEnd
        {
            get { return hourEnd; }
            set { hourEnd = value; OnPropertyChanged("HourEnd"); }
        }
        public int MinuteBegin
        {
            get { return minuteBegin; }
            set { minuteBegin = value; OnPropertyChanged("MinuteBegin"); }
        }
        public int MinuteEnd
        {
            get { return minuteEnd; }
            set { minuteEnd = value;OnPropertyChanged("MinuteEnd"); }
        }
        public TimeSpan TimeBegin
        {
            get { return timeBegin; }
            set { timeBegin = value;
                HourMinuteUpdate();
                OnPropertyChanged("TimeBegin"); }
        }
        public TimeSpan TimeEnd
        {
            get { return timeEnd; }
            set { timeEnd = value;
                HourMinuteUpdate();
                OnPropertyChanged("TimeEnd"); }
        }
        public double Coef
        {
            get { return coef; }
            set { coef = value; OnPropertyChanged("Coef"); }
        }
    }
}
