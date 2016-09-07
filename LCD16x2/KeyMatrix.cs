using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using HANDLE = System.IntPtr;  

namespace LCD16x2
{
    unsafe public static class KeyMatrix
    {

        [DllImport("TdxAllLibrariesDll.dll", CharSet = CharSet.Auto)]                          ///< Importing DLL
        public static extern HANDLE Adc_Init(string x);                                   ///< External function declaration

        [DllImport("TdxAllLibrariesDll.dll", CharSet = CharSet.Auto)]                          ///< Importing DLL
        public static extern bool Adc_SetConfigInt(HANDLE H, string x, Int32 val, Int32 v);

        [DllImport("TdxAllLibrariesDll.dll", CharSet = CharSet.Auto)]                          ///< Importing DLL
        public static extern bool Adc_Open(HANDLE H);

        [DllImport("TdxAllLibrariesDll.dll", CharSet = CharSet.Auto)]
        public static extern Boolean Adc_Read(HANDLE han, Int32* value, Int32 samples);

        private static System.Threading.Timer adcFetchTimer;
        private static TimerCallback timeCal;


        public  delegate void KeypadMatrixDelegate(string s);
        public static event KeypadMatrixDelegate KeypadMatrixEvent;

        public static UInt64 stage = 0;

        public static HANDLE hHan;
        public static Int32 data = 0;                                                     ///< Temporary Variable that stores raw data from ADC
        public static Double temp = 0;
        public static Int32 samples = 10;
        public static string RecentKey = "Null";

        public static void Start() {

            try
            {
                hHan = Adc_Init("ADC1");
                Adc_SetConfigInt(hHan, "BitResolution", 12, 1);
                Adc_SetConfigInt(hHan, "AvgSamples", 8, 1);
                Adc_Open(hHan);

                timeCal = new TimerCallback(Thread_Timer_Adc_Read);
                adcFetchTimer = new System.Threading.Timer(timeCal, null, 10, 150);
            }
            catch (Exception ex) { }

        }

        private static void Thread_Timer_Adc_Read(object o){

            try
            {
                fixed (Int32* Data = &data)
                {
                    Adc_Read(hHan, Data, 0);
                }
            }
            catch (Exception ex) { }

            /*
             * left arrow - 13 - [3222000 - 3230000 - 3238000 ]
             * Right arrow - 15 - [1651000 - 1659000 - 1667000 ]
             * Up arrow - 12 - [1276000 1284000 - 1292000]
             * Down arrow - 16 - [ 1328000 - 1334000 - 1342000 ]
             * 
             * 1 - 1 [2473000 - 2481000 - 2489000 ]
             * 2 - 2 [1802000 - 1810000 - 1818000 ]
             * 3 - 3 [1426000 - 1432000 - 1438000 ]
             * 
             * 4 - 5 [2688000 - 2694000 - 2702000 ]
             * 5 - 6 [1915000 - 1923000 - 1931000 ]
             * 6 - 7 [1494000 - 1502000 - 1508000 ]
             * 
             * 7 - 9 [2931000 - 2939000 - 2947000 ]
             * 8 - 10 [2033000 - 2041000 - 2049000 ]
             * 9 - 11 [1566000 - 1574000 - 1582000 ]
             * 
             * 0 - 14 [2172000 - 2178000 - 2186000 ]
             * 
             * Back - 4 [1178000 - 1186000 - 1192000 ]
             * Enter - 8 [1228000 - 1236000 - 1242000 ]
             * 
             */

            try
            {
                string NowKey = "Null";

                if (data > 1178000 && data < 1192000)
                {
                    //Back
                    NowKey = "Escape";
                }
                else if (data > 1228000 && data < 1242000)
                {
                    //Enter
                    NowKey = "Return";
                }
                else if (data > 1276000 && data < 1292000)
                {
                    //UpArrow
                    NowKey = "Up";

                }
                else if (data > 1328000 && data < 1342000)
                {
                    //Down Arrow
                    NowKey = "Down";
                }
                else if (data > 1426000 && data < 1438000)
                {
                    //3
                    if (stage > 0)
                        NowKey = "D3";
                    else
                        NowKey = "F3";

                }
                else if (data > 1494000 && data < 1508000)
                {
                    //6
                    if (stage > 0)
                        NowKey = "D6";
                    else
                        NowKey = "F6";

                }
                else if (data > 1566000 && data < 1582000)
                {
                    //9
                    if (stage > 0)
                        NowKey = "D9";
                    else
                        NowKey = "F9";

                }
                else if (data > 1651000 && data < 1667000)
                {
                    //Right Arrow
                    if (stage > 0)
                        NowKey = "Right";
                    else
                        NowKey = "F12";

                }
                else if (data > 1802000 && data < 1818000)
                {
                    //2
                    if (stage > 0)
                        NowKey = "D2";
                    else
                        NowKey = "F2";
                }
                else if (data > 1915000 && data < 1931000)
                {
                    //5
                    if (stage > 0)
                        NowKey = "D5";
                    else
                        NowKey = "F5";

                }
                else if (data > 2033000 && data < 2049000)
                {
                    //8
                    if (stage > 0)
                        NowKey = "D8";
                    else
                        NowKey = "F8";
                }
                else if (data > 2172000 && data < 2186000)
                {
                    //0
                    if (LCDFrameProcessor.ActiveFrameInfo.isYN && stage > 0)
                        NowKey = "Y";
                    else if (stage > 0)
                        NowKey = "D0";
                    else
                        NowKey = "F11";
                }
                else if (data > 2473000 && data < 2489000)
                {
                    //1
                    if (stage > 0)
                        NowKey = "D1";
                    else
                        NowKey = "F1";
                }
                else if (data > 2688000 && data < 2702000)
                {
                    //4
                    if (stage > 0)
                        NowKey = "D4";
                    else
                        NowKey = "F4";
                }
                else if (data > 2931000 && data < 2947000)
                {
                    //7
                    if (stage > 0)
                        NowKey = "D7";
                    else
                        NowKey = "F7";
                }
                else if (data > 3222000 && data < 3238000)
                {
                    //LeftArrow -->> BackSpace
                    if (stage > 0)
                        NowKey = "Back";
                    else
                        NowKey = "F10";
                }

                if (NowKey != "Null" && RecentKey != NowKey)
                    KeypadMatrixEvent(NowKey);

                RecentKey = NowKey;
                data = 0;
            }
            catch (Exception ex) { }

        }


    }
}
