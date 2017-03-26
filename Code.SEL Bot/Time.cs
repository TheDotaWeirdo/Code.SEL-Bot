namespace Code.SEL_Bot
{
    public class Time
    {
        private double Milliseconds { get; set; } = 0;
        private double Seconds { get; set; } = 0;
        private double Minutes { get; set; } = 0;
        private double Hours { get; set; } = 0;
        private double Days { get; set; } = 0;
        public double Total { get; set; } = 0;

        public Time(double Mil = 0, double S = 0, double Min = 0, double H = 0, double D = 0)
        {
            Milliseconds = Mil;
            Seconds = S;
            Minutes = Min;
            Hours = H;
            Days = D;
            Update();
        }

        public void AddMilliseconds(double M)
        { Milliseconds += M; Update(); }

        public void AddSeconds(double S)
        { Seconds += S; Update(); }

        public void AddMinutess(double M)
        { Minutes += M; Update(); }

        public void AddHours(double H)
        { Hours += H; Update(); }

        public void Add(Time T)
        {
            Milliseconds += T.Milliseconds;
            Seconds += T.Seconds;
            Minutes += T.Minutes;
            Hours += T.Hours;
            Days += T.Days;
            Update();
        }

        public void AddDays(double D)
        { Days += D; Update(); }

        private void Update()
        {
            while(Milliseconds >= 1000)
            { Seconds++; Milliseconds -= 1000; }
            while (Seconds >= 60)
            { Minutes++; Seconds -= 60; }
            while (Minutes >= 60)
            { Hours++; Minutes -= 60; }
            while (Hours >= 24)
            { Days++; Hours -= 24; }
            Total = Days * 86400000 + Hours * 3600000 + Minutes * 60000 + Seconds * 1000 + Milliseconds;
        }

        public string GetTime()
        {
            return Minutes + ":" + Seconds;
        }

        public string Get()
        {
            string S = "";
            if (Days > 0)
            {
                if (Days > 1)
                    S += Days + " Days, ";
                else
                    S += Days + " Day, ";
            }

            if (Hours > 0)
            {
                if (Hours > 1)
                    S += Hours + " Hours, ";
                else
                    S += Hours + " Hour, ";
            }

            if (Minutes > 0)
            {
                if (Minutes > 1)
                    S += Minutes + " Minutes, ";
                else
                    S += Minutes + " Minute, ";
            }

            if (Seconds > 0)
            {
                if (Seconds > 1)
                    S += Seconds + " Seconds, ";
                else
                    S += Seconds + " Second, ";
            }

            if (Milliseconds > 0)
            {
                if (Milliseconds > 1)
                    S += Milliseconds + " Milliseconds, ";
                else
                    S += Milliseconds + " Millisecond, ";
            }

            return S.Substring(0, S.Length - 2);
        }
    }
}
