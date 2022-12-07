using System;


namespace DiabetApp.Classes
{
    public class CheckInput
    {
        float previous_number;
        public float Previous_number
        {
            get { return previous_number; }
            set { previous_number = value; }
        }
        string present_number;
        public string Present_number
        {
            get { return present_number; }
            set { present_number = value; }
        }
        public CheckInput()
        {
            
        }

        public float CheckNumber(string num)
        {
            num = num.Replace('.', ',');
            try
            {
                if (Convert.ToDouble(num) > 0)
                {
                    Previous_number = (float)Convert.ToDouble(num);
                    return (float)Convert.ToDouble(num);
                }
                else
                {
                    return (float)Previous_number;
                }
            }
            catch
            {
                return Previous_number;
            }
        }
    }
}
