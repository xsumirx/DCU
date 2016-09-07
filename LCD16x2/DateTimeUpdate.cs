using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace LCD16x2
{
     static class DateTimeUpdate
    {
        [DllImport("coredll.dll", SetLastError = true)]
        public extern static void GetSystemTime(ref SystemTime sysTime);

        [DllImport("coredll.dll", SetLastError = true)]
        public extern static bool SetSystemTime(ref SystemTime sysTime);

         [StructLayout(LayoutKind.Sequential)]
         public struct SystemTime
         {
             public ushort Year;
             public ushort Month;
             public ushort DayOfWeek;
             public ushort Day;
             public ushort Hour;
             public ushort Minute;
             public ushort Second;
             public ushort Millisecond;
         };

         private static DateTime tmpDateTime = new DateTime();
          private static DateTime RealDateTime = new DateTime();
         private static SystemTime st = new SystemTime();

         public  delegate void ErrorHandler(string message);
         public static event ErrorHandler ErrorHandlerEvent;


         public static void UpdateTime(int _year,int _month,int _day, int _hour,int _minute, int _second,int _dayofWeek )
         {
             try
             {
                 tmpDateTime = new DateTime(_year, _month, _day, _hour, _minute, _second);
                
                 RealDateTime = tmpDateTime.ToUniversalTime();

                 GetSystemTime(ref st);

                 st.Day = ushort.Parse(RealDateTime.Day.ToString());
                 st.Month = ushort.Parse(RealDateTime.Month.ToString());
                 st.Year = ushort.Parse(RealDateTime.Year.ToString());
                 st.Hour = ushort.Parse(RealDateTime.Hour.ToString());
                 st.Minute = ushort.Parse(RealDateTime.Minute.ToString());
                 st.Second = ushort.Parse(RealDateTime.Second.ToString());
                 st.DayOfWeek = (ushort)_dayofWeek;

                 SetSystemTime(ref st);

             }
             catch (Exception ex)
             {
                 ErrorHandlerEvent("DateTimeUpdate clASS : \n\n" + ex.Message);
             }
         }

    }
}
