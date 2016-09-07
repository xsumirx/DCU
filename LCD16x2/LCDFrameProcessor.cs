using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;

namespace LCD16x2
{

    public class SearchDataColumn {
        private string _col1;
        private string _col2;
        private string _col3;

        public string col1 {
            set {
                if (value.Length > 3)
                {
                    _col1 = value.Remove(3, value.Length - 3);
                }
                else {
                    _col1 = value;
                }

            }
            get {
                return _col1;
            }
        
        }

        public string col2
        {
            set
            {
                if (value.Length > 7)
                {
                    _col2 = value.Remove(7, value.Length - 7);
                }
                else
                {
                    _col2 = value;
                }

            }
            get
            {
                return _col2;
            }
        


            
        }

        public string col3
        {
            set
            {
                if (value.Length > 7)
                {
                    _col3 = value.Remove(7, value.Length - 7);
                }
                else
                {
                    _col3 = value;
                }

            }
            get
            {
                return _col3;
            }

        }

        public SearchDataColumn(String _temp1, String _temp2, String _temp3)
        {
            col1 = _temp1;
            col2 = _temp2;
            col3 = _temp3;
        }
    }

    public class FrameInputInfo
    {
        public uint LX;
        public uint LY;
        public uint WhichData;
        public uint inputIndex;

        public FrameInputInfo(uint _lx, uint _ly, uint _whichData, uint _inputIndex) {
            LX = _lx;
            LY = _ly;
            WhichData = _whichData;
            inputIndex = _inputIndex;
        }
    }

    public static class TempCalculationVariable { 
        //frame 2010510
        public static double FATmin = 0;
        public static double SNFmin = 0;
        public static double FATmax = 0;
        public static double SNFmax = 0;
        public static int CLRmin = 0;
        public static int CLRmax = 0;
        public static string value = "0";

        public static double FATminFIX = 0;
        public static double SNFminFIX = 0;
        public static int CLRminFIX = 0;


    }

    static class LCDFrameProcessor
    {
        //Frame
        public static bool backTimerFlag = false;
        public static UInt64 FrameBase;
        public static uint KeyboardMode = 0;
        public static bool keyboardNumMode = false;
        public static bool LCDrefreshFlag = false;
        public static uint keyboardMaxChar = 0;
        public static bool Busy = false;

        public delegate void BackEscape();
        public static event BackEscape BackSpaceEvent;

        public static string customMessgae = "";
        public static string customMessgae1 = "";

        public static char[] Shift = new char[] {'M','E'};
        public static string[] DayofWeek = new string[] {"Sun","Mon","Tue","Wed","Thr","Fri","Sat" };
        public static string[] YesOrNo = new string[] {"No ","Yes" };
        public static string[] MachineCollection = new string[] {"No        ","Lecto Scan","Eko Milk  ","EMT       ","Computer  " };
        public static string[] PrinterCollection = new string[] {"Printer 1","Printer 2","Printer 3" };
        public static string[] WeighCollection = new string[] { "Machine 1", "Machine 2", "Machine 3", "Machine 4", "Machine 5", "Machine 6", };

        //COMport Setting
        public static string[] BaudRateCollection = new string[] {" 1200 "," 2400 "," 4800 "," 9600 ","19200 ","38400 ","57600 ","115200" };
        public static string[] DataBitsCollection = new string[] {"7","8" };
        public static string[] StopBitsCollection = new string[] { " 0 ", " 1 ","1.5"," 2 " };
        public static string[] ParityCollection = new string[] {" None ","Even "," Odd ","Space","Mark " };
        public static string[] handShakeCollection = new string[] { "    None     ", "RequestToSend" };

        //RateChart and F2 Stuffs
        public static string[] RateChartCollectionCOW = new string[] {"  FAT-SNF Auto      ","   FAT-SNF Manual   ","FAT-CLR(dnsty) Frmla","FAT-CLR(dnsty) Mnual","FAT-CLR(SNF) Manual ","FAT-CLR(dnsty) Cmptr","   FAT-Only Manual  " };

        //Genral
        public static string[] AutoOrManual = new string[] { "Manual", "Auto  " ," Ask  "};

        public static string[] COWorBUF = new string[] { "COW","BUF"};


        public static string[] TransmitterDeviceCollection = new string[] {"Display","Printer","  PC   " };
        public static string[] ReceiverDeviceCollection = new string[] {"  Analyser ","Weigh Scale","      PC   " };

        public static string[] ExportImportCollection = new string[] {"Export","Import" };
        public static string[] ExportImportDataCollection = new string[] { "   All    ", "Collection","Rate Chart" };


        public static string FATorSNFD = "FAT";
        public static string DEDorBON = "DEDUCT";
        public static string COWorBUFD = "COW";
        public static int _slot_id = 1;


        public static bool F5_LOCK_STATUS = true;
        public static bool F12_PASSWORD_STATE = true;
        public static bool F2_PASSWORD_STATE = true;
        public static bool F10_PASSWORD_STATE = true;


        public struct ActiveFrameInfo
        {


            public static bool IsInput;
            public static UInt64 FrameBase;

            public static bool isYN = false;

            public static int LCD_X;
            public static int LCD_Y;

            public static uint ActiveY{
                set {
                    LCD_Y = int.Parse(value.ToString());
                }
                get {
                    return uint.Parse(LCD_Y.ToString());
                }
            }

            public static uint ActiveX {
                set
                {
                    LCD_X = int.Parse(value.ToString());
                }
                get {
                    return uint.Parse(LCD_X.ToString());
                }

            }

            public static List<FrameInputInfo> InputList = new List<FrameInputInfo>();
            public static FrameInputInfo ActiveFrameInput;

            public static string FrameData_Name = "";
            public static uint FrameData_UserID = 0;
            public static string FrameData_Phone = "";

            public static DateTime FrameData_Date = new DateTime();
           


            public static uint whichShift = 0;
            public static uint whichShift_1 = 0;
            //0 ===> Morning
            //1 ===> Evening

            public static string newPassword = "";
            public static string oldPassword = "";
            public static string RetypePassword = "";
            public static uint toggleSwitch = 0;
            public static string screenTittle = "";

            public static string[] float1_0 = new string[] {"0",".","0"};
            public static uint float1_0index = 0;
            public static string float1_0data = "";

            public static string[] float1_1 = new string[] { "0", ".", "0" };
            public static uint float1_1index = 0;
            public static string float1_1data = "";

            public static string[] float2_0 = new string[] { "0", ".", "0","0" };
            public static uint float2_0index = 0;
            public static string float2_0data = "";

            public static string[] float2_1 = new string[] { "0", ".", "0", "0" };
            public static uint float2_1index = 0;
            public static string float2_1data = "";

            public static string[] float3_0 = new string[] { "0", "0", ".", "0" };
            public static uint float3_0index = 0;
            public static string float3_0data = "";

            public static string[] float3_1 = new string[] { "0", "0", ".", "0" };
            public static uint float3_1index = 0;
            public static string float3_1data = "";

            public static string[] float4_0 = new string[] { "0", "0", ".", "0","0" };
            public static uint float4_0index = 0;
            public static string float4_0data = "";

            public static string[] float4_1 = new string[] { "0", "0", ".", "0", "0" };
            public static uint float4_1index = 0;
            public static string float4_1data = "";

            public static string[] float4_2 = new string[] { "0", "0", ".", "0", "0" };
            public static uint float4_2index = 0;
            public static string float4_2data = "";

            public static string[] float4_3 = new string[] { "0", "0", ".", "0", "0" };
            public static uint float4_3index = 0;
            public static string float4_3data = "";

            public static string[] float4_4 = new string[] { "0", "0", ".", "0", "0" };
            public static uint float4_4index = 0;
            public static string float4_4data = "";

            public static string[] float4_5 = new string[] { "0", "0", ".", "0", "0" };
            public static uint float4_5index = 0;
            public static string float4_5data = "";

            public static string[] float5_0 = new string[] { "0", "0", "0", ".", "0", "0" };
            public static uint float5_0index = 0;
            public static string float5_0data = "";

            public static string[] float6_0 = new string[] { "0", "0", "0", "0", ".", "0", "0" };
            public static uint float6_0index = 0;
            public static string float6_0data = "";

            public static string[] float7_0 = new string[] { "0", "0", "0", "0", "0", ".", "0", "0" };
            public static uint float7_0index = 0;
            public static string float7_0data = "";

            public static string[] float5_1 = new string[] { "0", "0", "0", ".", "0", "0" };
            public static uint float5_1index = 0;
            public static string float5_1data = "";

            public static string[] float6_1 = new string[] { "0", "0", "0", "0", ".", "0", "0" };
            public static uint float6_1index = 0;
            public static string float6_1data = "";

            public static string[] float7_1 = new string[] { "0", "0", "0", "0", "0", ".", "0", "0" };
            public static uint float7_1index = 0;
            public static string float7_1data = "";

            public static string integer1_0 = "";
            public static string integer2_0 = "";
            public static string integer3_0 = "";
            public static string integer4_0 = "";
            public static string integer5_0 = "";

            public static string integer1_1 = "";
            public static string integer2_1 = "";
            public static string integer3_1 = "";
            public static string integer4_1 = "";
            public static string integer5_1 = "";

            public static string passwordGlobal = "";
            public static string passwordGlobalReal = "";

            public static List<SearchDataColumn> SearchData = new List<SearchDataColumn>();
            public static int SearchDataIndex = 0;

            public static SearchDataColumn[] SearchDataDisplay = new SearchDataColumn[4];
            public static int SearchDataDisplayIndex = 0;

            public static uint WhichDataInFrame = 0;
            //What is Data
            /*
             *1.UserID
             *2.Name
             *3.Phone
             *4.Day
             *5.Month
             *6.Year
             *7.hour
             *8.minute
             *9.seconds
             *10.day of week
             *11.shift
             *12.Old Password
             *13.New Password
             *14.Retype Password
             *15.ToggleSwitch
             *16.screen title
             *17.Active Machine
             *18.Active Printer
             *19.Active Weigh Scale
             *20.float1_0
             *21.float2_0
             *22.float3_0
             *23.float4_0
             *24.float1_1
             *25.float2_1
             * 26.float3_1
             * 27.float4_1
             * 28.VechileNo.
             * 
             * 29.float5_0
             * 30.float6_0
             * 31.float7_0
             * 32.float5_1
             * 33.float6_1
             * 34.float7_1
             * 
             * 35.float4_2
             * 36.float4_3
             * 37.float4_4
             * 38.float4_5
             * 
             * //ComPort Setting
             * 40.BaudRate
             * 41.Databit\
             * 42.Stopbit
             * 43.Handshake
             * 44.parity
             * 
             * 45.integer1_0
             * 46.integer2_0
             * 47.integer3_0
             * 48.integer4_0
             * 49.integer5_0
             * 
             * 50.integer1_1
             * 51.integer2_1
             * 52.integer3_1
             * 53.integer4_1
             * 54.integer5_1
             * 
             * 55.whichRatechart
             * 56.shiftSwitch3_0
             * 57.shiftSwitch3_1
             * 
             * 61.day_1
             * 62.month_1
             * 63.year_1
             * 64.whichShift_1
             * 65.Password
             * 
             * 66.DataColumn
             */

            //Date and Time Entry Trackers
            public static string day = "";
            public static string month = "";
            public static string year = "";

            public static string hour = "";
            public static string minute = "";
            public static string second = "";

            public static string day_1 = "";
            public static string month_1 = "";
            public static string year_1 = "";

            public static uint whichDayofWeek = 0;
            //Ends
            public static uint ActiveMachine = 0;
            public static uint ActiveWeigh = 0;
            public static uint ActivePrinter = 0;

            //Vechile No.
            public static string VechileNo = "";

            //Comport Setting
            public static uint whichBaudRate = 0;
            public static uint whichDataBit = 0;
            public static uint whichStopBit = 0;
            public static uint whichParity = 0;
            public static uint whichHandShake = 0;

            public static uint shiftSwitch3_0 = 0;
            public static uint shiftSwitch3_1 = 0;

            //Ratechart and F2 sTUFFS
            public static uint whichRatechart = 0;

            public static string UpdateInfo {
                set {
                    if (WhichDataInFrame == 1) {
                        uint x = 0;
                        if (value != "")
                        {
                            x = FrameData_UserID * 10 + uint.Parse(value);


                            if (x >= 0 && x <= 999)
                            {
                                FrameData_UserID = x;
                            }
                        }
                    }else if(WhichDataInFrame == 2){
                        if (FrameData_Name.Length < 14) {
                            FrameData_Name += value;
                        }
                    }else if(WhichDataInFrame == 3){
                        if (FrameData_Phone.Length < 10)
                        {
                            FrameData_Phone += value;
                        }
                    }
                    else if (WhichDataInFrame == 4) { 
                        //Its Day
                        day += value;

                        if (uint.Parse(day) > 31 || day.Length > 2) {
                            day = day.Remove((day.Length - 1), 1);
                        }

                        if (day.Length == 2 && uint.Parse(day) == 0)
                        {
                            day = day.Remove((day.Length - 1), 1);
                        }
                        
                        
                    }
                    else if (WhichDataInFrame == 5)
                    {
                        //Its Month
                        month += value;

                        if (uint.Parse(month) > 12 || month.Length > 2)
                        {
                            month = month.Remove((month.Length - 1), 1);
                        }
                        if (month.Length == 2 && uint.Parse(month) == 0) {
                            month = month.Remove((month.Length - 1), 1);
                        } 
                    }
                    else if (WhichDataInFrame == 6)
                    {
                        //Its Year
                        year += value;

                        if (uint.Parse(year) > 99 || year.Length > 2)
                        {
                            year = year.Remove((year.Length - 1), 1);
                        }
                    }
                    else if(WhichDataInFrame == 7){
                        //Its hour
                        hour += value;

                        if (uint.Parse(hour) > 23 || hour.Length > 2)
                        {
                            hour = hour.Remove((hour.Length - 1), 1);
                        }
                        
                    }
                    else if (WhichDataInFrame == 8)
                    {
                        //Its minute
                        minute += value;

                        if (uint.Parse(minute) > 59 || minute.Length > 2)
                        {
                            minute = minute.Remove((minute.Length - 1), 1);
                        }

                    }
                    else if (WhichDataInFrame == 9)
                    {
                        //Its seconds
                        second += value;

                        if (uint.Parse(second) > 59 || second.Length > 2)
                        {
                            second = second.Remove((second.Length - 1), 1);
                        }

                    }
                    else if (WhichDataInFrame == 10)
                    {
                        //Its day of week
                        if (value == "Right") {
                            if (whichDayofWeek == 6)
                            {
                                whichDayofWeek = 0;
                            }
                            else {
                                whichDayofWeek++;
                            }
                        }
                        else if (value == "Left") {
                            if (whichDayofWeek == 0)
                            {
                                whichDayofWeek = 6;
                            }
                            else
                            {
                                whichDayofWeek--;
                            }
                        }
                        

                    }
                    else if (WhichDataInFrame == 11)
                    {
                        //Its Shift
                        if (value == "Right")
                        {
                            if (whichShift == 1)
                            {
                                whichShift = 0;
                            }
                            else
                            {
                                whichShift++;
                            }
                        }
                        else if (value == "Left")
                        {
                            if (whichShift == 0)
                            {
                                whichShift = 1;
                            }
                            else
                            {
                                whichShift--;
                            }
                        }

                    }
                    else if (WhichDataInFrame == 12)
                    {
                        //Its Old Password
                        if(oldPassword.Length < 4)
                            oldPassword += value;
                    }
                    else if (WhichDataInFrame == 13)
                    {
                        //Its New Password
                        if (newPassword.Length < 4)
                            newPassword += value;
                    }
                    else if (WhichDataInFrame == 14)
                    {
                        //Its Re-type Password
                        if (RetypePassword.Length < 4)
                            RetypePassword += value;
                    }
                    else if (WhichDataInFrame == 15)
                    {
                        //Its Toggle Switch
                        if (value == "Right")
                        {
                            if (toggleSwitch == 1)
                            {
                                toggleSwitch = 0;
                            }
                            else
                            {
                                toggleSwitch++;
                            }
                        }
                        else if (value == "Left")
                        {
                            if (toggleSwitch == 0)
                            {
                                toggleSwitch = 1;
                            }
                            else
                            {
                                toggleSwitch--;
                            }
                        }
                    }
                    else if (WhichDataInFrame == 16) {
                        if (screenTittle.Length < 20) {
                            screenTittle += value;
                        }
                    }
                    else if (WhichDataInFrame == 17)
                    {
                        //Its Active Machine
                        if (value == "Right")
                        {
                            if (ActiveMachine >= 4)
                            {
                                ActiveMachine = 0;
                            }
                            else
                            {
                                ActiveMachine++;
                            }
                        }
                        else if (value == "Left")
                        {
                            if (ActiveMachine == 0)
                            {
                                ActiveMachine = 4;
                            }
                            else
                            {
                                ActiveMachine--;
                            }
                        }
                    }
                    else if (WhichDataInFrame == 18)
                    {
                        //Its Active Printer
                        if (value == "Right")
                        {
                            if (ActivePrinter >= 2)
                            {
                                ActivePrinter = 0;
                            }
                            else
                            {
                                ActivePrinter++;
                            }
                        }
                        else if (value == "Left")
                        {
                            if (ActivePrinter == 0)
                            {
                                ActivePrinter = 2;
                            }
                            else
                            {
                                ActivePrinter--;
                            }
                        }
                    }
                    else if (WhichDataInFrame == 19)
                    {
                        //Its Active Weighing Scale
                        if (value == "Right")
                        {
                            if (ActiveWeigh >= 5)
                            {
                                ActiveWeigh = 0;
                            }
                            else
                            {
                                ActiveWeigh++;
                            }
                        }
                        else if (value == "Left")
                        {
                            if (ActiveWeigh == 0)
                            {
                                ActiveWeigh = 5;
                            }
                            else
                            {
                                ActiveWeigh--;
                            }
                        }
                    }
                    else if(WhichDataInFrame == 20){
                        
                        if (value != "") {

                            float1_0[float1_0index] = value;

                            float1_0data = "";
                            for (int i = 0; i <= float1_0index; i++)
                            {
                                float1_0data += float1_0[i];
                            }

                            if (!(float1_0index == 2))
                            {
                                float1_0index++;
                            }

                            if (float1_0index == 1)
                            {
                                //Its a DOT
                                float1_0index++;
                                float1_0data += ".";
                            }
                            
                        }
                    }
                    else if (WhichDataInFrame == 21)
                    {

                        if (value != "")
                        {
                            float2_0[float2_0index] = value;

                            float2_0data = "";
                            for (int i = 0; i <= float2_0index; i++)
                            {
                                float2_0data += float2_0[i];
                            }

                            if (!(float2_0index == 3))
                            {
                                float2_0index++;
                            }

                            if (float2_0index == 1)
                            {
                                float2_0index++; //its a Dot Place
                                float2_0data += ".";
                            }
                            
                        }
                    }
                    else if (WhichDataInFrame == 22)
                    {

                        if (value != "")
                        {
                            float3_0[float3_0index] = value;

                            float3_0data = "";
                            for (int i = 0; i <= float3_0index; i++)
                            {
                                float3_0data += float3_0[i];
                            }

                            if (!(float3_0index == 3))
                            {
                                float3_0index++;
                            }

                            if (float3_0index == 2)
                            {
                                float3_0index++; //its a Dot Place
                                float3_0data += ".";
                            }

                            
                        }
                    }
                    else if (WhichDataInFrame == 23)
                    {

                        if (value != "")
                        {
                            float4_0[float4_0index] = value;

                            float4_0data = "";
                            for (int i = 0; i <= float4_0index; i++)
                            {
                                float4_0data += float4_0[i];
                            }

                            if (!(float4_0index == 4))
                            {
                                float4_0index++;
                            }

                            if (float4_0index == 2)
                            {
                                float4_0index++; //its a Dot Place
                                float4_0data += ".";
                            }

                            
                        }
                    }
                    else if (WhichDataInFrame == 24) 
                    {
                        if (value != "")
                        {

                            float1_1[float1_1index] = value;

                            float1_1data = "";
                            for (int i = 0; i <= float1_1index; i++)
                            {
                                float1_1data += float1_1[i];
                            }

                            if (!(float1_1index == 2))
                            {
                                float1_1index++;
                            }

                            if (float1_1index == 1)
                            {
                                float1_1index++;
                                float1_1data += ".";
                            }

                        }
                    }
                    else if (WhichDataInFrame == 25)
                    {
                        if (value != "")
                        {
                            float2_1[float2_1index] = value;

                            float2_1data = "";
                            for (int i = 0; i <= float2_1index; i++)
                            {
                                float2_1data += float2_1[i];
                            }

                            if (!(float2_1index == 3))
                            {
                                float2_1index++;
                            }

                            if (float2_1index == 1)
                            {
                                float2_1index++; //its a Dot Place
                                float2_1data += ".";
                            }


                        }
                    }
                    else if (WhichDataInFrame == 26)
                    {
                        if (value != "")
                        {
                            float3_1[float3_1index] = value;

                            float3_1data = "";
                            for (int i = 0; i <= float3_1index; i++)
                            {
                                float3_1data += float3_1[i];
                            }

                            if (!(float3_1index == 3))
                            {
                                float3_1index++;
                            }

                            if (float3_1index == 2)
                            {
                                float3_1index++; //its a Dot Place
                                float3_1data += ".";
                            }


                        }
                    }
                    else if (WhichDataInFrame == 27)
                    {
                        if (value != "")
                        {
                            float4_1[float4_1index] = value;

                            float4_1data = "";
                            for (int i = 0; i <= float4_1index; i++)
                            {
                                float4_1data += float4_1[i];
                            }

                            if (!(float4_1index == 4))
                            {
                                float4_1index++;
                            }

                            if (float4_1index == 2)
                            {
                                float4_1index++; //its a Dot Place
                                float4_1data += ".";
                            }


                        }
                    }
                    else if (WhichDataInFrame == 28) {
                        if (VechileNo.Length <= 5) {
                            VechileNo += value;
                        }
                    }
                    else if (WhichDataInFrame == 29)
                    {
                        if (value != "")
                        {
                            float5_0[float5_0index] = value;

                            float5_0data = "";
                            for (int i = 0; i <= float5_0index; i++)
                            {
                                float5_0data += float5_0[i];
                            }

                            if (!(float5_0index == 5))
                            {
                                float5_0index++;
                            }

                            if (float5_0index == 3)
                            {
                                float5_0index++; //its a Dot Place
                                float5_0data += ".";
                            }


                        }
                    }
                    else if (WhichDataInFrame == 30)
                    {
                        if (value != "")
                        {
                            float6_0[float6_0index] = value;

                            float6_0data = "";
                            for (int i = 0; i <= float6_0index; i++)
                            {
                                float6_0data += float6_0[i];
                            }

                            if (!(float6_0index == 6))
                            {
                                float6_0index++;
                            }

                            if (float6_0index == 4)
                            {
                                float6_0index++; //its a Dot Place
                                float6_0data += ".";
                            }


                        }
                    }
                    else if (WhichDataInFrame == 31)
                    {
                        if (value != "")
                        {
                            float7_0[float7_0index] = value;

                            float7_0data = "";
                            for (int i = 0; i <= float7_0index; i++)
                            {
                                float7_0data += float7_0[i];
                            }

                            if (!(float7_0index == 7))
                            {
                                float7_0index++;
                            }

                            if (float7_0index == 5)
                            {
                                float7_0index++; //its a Dot Place
                                float7_0data += ".";
                            }


                        }
                    }
                    else if (WhichDataInFrame == 32)
                    {
                        if (value != "")
                        {
                            float5_1[float5_1index] = value;

                            float5_1data = "";
                            for (int i = 0; i <= float5_1index; i++)
                            {
                                float5_1data += float5_1[i];
                            }

                            if (!(float5_1index == 5))
                            {
                                float5_1index++;
                            }

                            if (float5_1index == 3)
                            {
                                float5_1index++; //its a Dot Place
                                float5_1data += ".";
                            }


                        }
                    }
                    else if (WhichDataInFrame == 33)
                    {
                        if (value != "")
                        {
                            float6_1[float6_1index] = value;

                            float6_1data = "";
                            for (int i = 0; i <= float6_1index; i++)
                            {
                                float6_1data += float6_1[i];
                            }

                            if (!(float6_1index == 6))
                            {
                                float6_1index++;
                            }

                            if (float6_1index == 4)
                            {
                                float6_1index++; //its a Dot Place
                                float6_1data += ".";
                            }


                        }
                    }
                    else if (WhichDataInFrame == 34)
                    {
                        if (value != "")
                        {
                            float7_1[float7_1index] = value;

                            float7_1data = "";
                            for (int i = 0; i <= float7_1index; i++)
                            {
                                float7_1data += float7_1[i];
                            }

                            if (!(float7_1index == 7))
                            {
                                float7_1index++;
                            }

                            if (float7_1index == 5)
                            {
                                float7_1index++; //its a Dot Place
                                float7_1data += ".";
                            }


                        }
                    }
                    else if (WhichDataInFrame == 35)
                    {

                        if (value != "")
                        {
                            float4_2[float4_2index] = value;

                            float4_2data = "";
                            for (int i = 0; i <= float4_2index; i++)
                            {
                                float4_2data += float4_2[i];
                            }

                            if (!(float4_2index == 4))
                            {
                                float4_2index++;
                            }

                            if (float4_2index == 2)
                            {
                                float4_2index++; //its a Dot Place
                                float4_2data += ".";
                            }


                        }
                    }
                    else if (WhichDataInFrame == 36)
                    {

                        if (value != "")
                        {
                            float4_3[float4_3index] = value;

                            float4_3data = "";
                            for (int i = 0; i <= float4_3index; i++)
                            {
                                float4_3data += float4_3[i];
                            }

                            if (!(float4_3index == 4))
                            {
                                float4_3index++;
                            }

                            if (float4_3index == 2)
                            {
                                float4_3index++; //its a Dot Place
                                float4_3data += ".";
                            }


                        }
                    }
                    else if (WhichDataInFrame == 37)
                    {

                        if (value != "")
                        {
                            float4_4[float4_4index] = value;

                            float4_4data = "";
                            for (int i = 0; i <= float4_4index; i++)
                            {
                                float4_4data += float4_4[i];
                            }

                            if (!(float4_4index == 4))
                            {
                                float4_4index++;
                            }

                            if (float4_4index == 2)
                            {
                                float4_4index++; //its a Dot Place
                                float4_4data += ".";
                            }


                        }
                    }
                    else if (WhichDataInFrame == 38)
                    {

                        if (value != "")
                        {
                            float4_5[float4_5index] = value;

                            float4_5data = "";
                            for (int i = 0; i <= float4_5index; i++)
                            {
                                float4_5data += float4_5[i];
                            }

                            if (!(float4_5index == 4))
                            {
                                float4_5index++;
                            }

                            if (float4_5index == 2)
                            {
                                float4_5index++; //its a Dot Place
                                float4_5data += ".";
                            }


                        }
                    }
                    else if (WhichDataInFrame == 40)
                    {
                        //Its BaudRate
                        if (value == "Right")
                        {
                            if (whichBaudRate == 7)
                            {
                                whichBaudRate = 0;
                            }
                            else
                            {
                                whichBaudRate++;
                            }
                        }
                        else if (value == "Left")
                        {
                            if (whichBaudRate == 0)
                            {
                                whichBaudRate = 7;
                            }
                            else
                            {
                                whichBaudRate--;
                            }
                        }

                    }
                    else if (WhichDataInFrame == 41)
                    {
                        //Its databits
                        if (value == "Right")
                        {
                            if (whichDataBit == 1)
                            {
                                whichDataBit = 0;
                            }
                            else
                            {
                                whichDataBit++;
                            }
                        }
                        else if (value == "Left")
                        {
                            if (whichDataBit == 0)
                            {
                                whichDataBit = 1;
                            }
                            else
                            {
                                whichDataBit--;
                            }
                        }

                    }
                    else if (WhichDataInFrame == 42)
                    {
                        //Its StopBits
                        if (value == "Right")
                        {
                            if (whichStopBit == 3)
                            {
                                whichStopBit = 0;
                            }
                            else
                            {
                                whichStopBit++;
                            }
                        }
                        else if (value == "Left")
                        {
                            if (whichStopBit == 0)
                            {
                                whichStopBit = 3;
                            }
                            else
                            {
                                whichStopBit--;
                            }
                        }

                    }
                    else if (WhichDataInFrame == 43)
                    {
                        //Its handshake
                        if (value == "Right")
                        {
                            if (whichHandShake == 1)
                            {
                                whichHandShake = 0;
                            }
                            else
                            {
                                whichHandShake++;
                            }
                        }
                        else if (value == "Left")
                        {
                            if (whichHandShake == 0)
                            {
                                whichHandShake = 1;
                            }
                            else
                            {
                                whichHandShake--;
                            }
                        }

                    }
                    else if (WhichDataInFrame == 44)
                    {
                        //Its Parity
                        if (value == "Right")
                        {
                            if (whichParity == 4)
                            {
                                whichParity = 0;
                            }
                            else
                            {
                                whichParity++;
                            }
                        }
                        else if (value == "Left")
                        {
                            if (whichParity == 0)
                            {
                                whichParity = 4;
                            }
                            else
                            {
                                whichParity--;
                            }
                        }

                    }
                    else if (WhichDataInFrame == 45) {
                        integer1_0 += value;

                        if (uint.Parse(integer1_0) > 9)
                        {
                            integer1_0 = integer1_0.Remove((integer1_0.Length - 1), 1);
                        }
                    }
                    else if (WhichDataInFrame == 46)
                    {
                        integer2_0 += value;

                        if (uint.Parse(integer2_0) > 99)
                        {
                            integer2_0 = integer2_0.Remove((integer2_0.Length - 1), 1);
                        }
                    }
                    else if (WhichDataInFrame == 47)
                    {
                        integer3_0 += value;

                        if (uint.Parse(integer3_0) > 999 || integer3_0.Length > 3)
                        {
                            integer3_0 = integer3_0.Remove((integer3_0.Length - 1), 1);
                        }
                    }
                    else if (WhichDataInFrame == 48)
                    {
                        integer4_0 += value;

                        if (uint.Parse(integer4_0) > 9999)
                        {
                            integer4_0 = integer4_0.Remove((integer4_0.Length - 1), 1);
                        }
                    }
                    else if (WhichDataInFrame == 49)
                    {
                        integer5_0 += value;

                        if (uint.Parse(integer5_0) > 99999)
                        {
                            integer5_0 = integer5_0.Remove((integer5_0.Length - 1), 1);
                        }
                    }
                    else if (WhichDataInFrame == 50)
                    {
                        integer1_1 += value;

                        if (uint.Parse(integer1_1) > 9)
                        {
                            integer1_1 = integer1_1.Remove((integer1_1.Length - 1), 1);
                        }
                    }
                    else if (WhichDataInFrame == 51)
                    {
                        integer2_1 += value;

                        if (uint.Parse(integer2_1) > 99)
                        {
                            integer2_1 = integer2_1.Remove((integer2_1.Length - 1), 1);
                        }
                    }
                    else if (WhichDataInFrame == 52)
                    {
                        integer3_1 += value;

                        if (uint.Parse(integer3_1) > 999 || integer3_0.Length > 3)
                        {
                            integer3_1 = integer3_1.Remove((integer3_1.Length - 1), 1);
                        }
                    }
                    else if (WhichDataInFrame == 53)
                    {
                        integer4_1 += value;

                        if (uint.Parse(integer4_1) > 9999)
                        {
                            integer4_1 = integer4_1.Remove((integer4_1.Length - 1), 1);
                        }
                    }
                    else if (WhichDataInFrame == 54)
                    {
                        integer5_1 += value;

                        if (uint.Parse(integer5_1) > 99999)
                        {
                            integer5_1 = integer5_1.Remove((integer5_1.Length - 1), 1);
                        }
                    }
                    else if (WhichDataInFrame == 55)
                    {
                        //Its Rate Charformula
                        if (value == "Right")
                        {
                            if (whichRatechart >= 6)
                            {
                                whichRatechart = 0;
                            }
                            else
                            {
                                whichRatechart++;
                            }
                        }
                        else if (value == "Left")
                        {
                            if (whichRatechart == 0)
                            {
                                whichRatechart = 6;
                            }
                            else
                            {
                                whichRatechart--;
                            }
                        }
                    }
                    else if (WhichDataInFrame == 56)
                    {
                        //Its shiftSwitch3_0
                        if (value == "Right")
                        {
                            if (shiftSwitch3_0 >= 2)
                            {
                                shiftSwitch3_0 = 0;
                            }
                            else
                            {
                                shiftSwitch3_0++;
                            }
                        }
                        else if (value == "Left")
                        {
                            if (shiftSwitch3_0 == 0)
                            {
                                shiftSwitch3_0 = 2;
                            }
                            else
                            {
                                shiftSwitch3_0--;
                            }
                        }
                    }
                    else if (WhichDataInFrame == 57)
                    {
                        //Its shiftSwitch3_1
                        if (value == "Right")
                        {
                            if (shiftSwitch3_1 >= 2)
                            {
                                shiftSwitch3_1 = 0;
                            }
                            else
                            {
                                shiftSwitch3_1++;
                            }
                        }
                        else if (value == "Left")
                        {
                            if (shiftSwitch3_1 == 0)
                            {
                                shiftSwitch3_1 = 2;
                            }
                            else
                            {
                                shiftSwitch3_1--;
                            }
                        }
                    }
                    else if (WhichDataInFrame == 61)
                    {
                        //Its Day_1
                        day_1 += value;

                        if (uint.Parse(day_1) > 31 || day_1.Length > 2)
                        {
                            day_1 = day_1.Remove((day_1.Length - 1), 1);
                        }

                        if (day_1.Length == 2 && uint.Parse(day_1) == 0)
                        {
                            day_1 = day_1.Remove((day_1.Length - 1), 1);
                        }


                    }
                    else if (WhichDataInFrame == 62)
                    {
                        //Its Month
                        month_1 += value;

                        if (uint.Parse(month_1) > 12 || month_1.Length > 2)
                        {
                            month_1 = month_1.Remove((month_1.Length - 1), 1);
                        }
                        if (month_1.Length == 2 && uint.Parse(month_1) == 0)
                        {
                            month_1 = month_1.Remove((month_1.Length - 1), 1);
                        }
                    }
                    else if (WhichDataInFrame == 63)
                    {
                        //Its Year
                        year_1 += value;

                        if (uint.Parse(year_1) > 99 || year_1.Length > 2)
                        {
                            year_1 = year_1.Remove((year_1.Length - 1), 1);
                        }
                    }
                    else if (WhichDataInFrame == 64)
                    {
                        //Its Shift
                        if (value == "Right")
                        {
                            if (whichShift_1 == 1)
                            {
                                whichShift_1 = 0;
                            }
                            else
                            {
                                whichShift_1++;
                            }
                        }
                        else if (value == "Left")
                        {
                            if (whichShift_1 == 0)
                            {
                                whichShift_1 = 1;
                            }
                            else
                            {
                                whichShift_1--;
                            }
                        }

                    }
                    else if (WhichDataInFrame == 65) { 
                        //Its Password
                        if (passwordGlobal.Length < 5 && passwordGlobalReal.Length < 5)
                        {
                            if (value.Length == 1)
                            {
                                passwordGlobalReal += value;
                                passwordGlobal += "*";
                            }
                        }
                    }

                    else if (WhichDataInFrame == 66)
                    {
                        //Its DataColumn
                        if (value == "Up") {
                            if (SearchDataIndex > 0) {
                                SearchDataIndex--;
                            }

                            int diff = SearchData.Count - SearchDataIndex;

                            if (diff >= 4)
                            {

                                SearchDataDisplay[0] = new SearchDataColumn(SearchData.ElementAt(SearchDataIndex).col1, SearchData.ElementAt(SearchDataIndex).col2, SearchData.ElementAt(SearchDataIndex).col3);
                                SearchDataDisplay[1] = new SearchDataColumn(SearchData.ElementAt(SearchDataIndex + 1).col1, SearchData.ElementAt(SearchDataIndex + 1).col2, SearchData.ElementAt(SearchDataIndex + 1).col3);
                                SearchDataDisplay[2] = new SearchDataColumn(SearchData.ElementAt(SearchDataIndex + 2).col1, SearchData.ElementAt(SearchDataIndex + 2).col2, SearchData.ElementAt(SearchDataIndex + 2).col3);
                                SearchDataDisplay[3] = new SearchDataColumn(SearchData.ElementAt(SearchDataIndex + 3).col1, SearchData.ElementAt(SearchDataIndex + 3).col2, SearchData.ElementAt(SearchDataIndex + 3).col3);
                                
                                if (SearchDataDisplayIndex > 0)
                                {
                                    SearchDataDisplayIndex--;
                                }
                                else if (SearchDataDisplayIndex < 0)
                                {
                                    SearchDataDisplayIndex = 0;
                                }
                            }
                            else if (diff == 3)
                            {
                                SearchDataDisplay[0] = new SearchDataColumn(SearchData.ElementAt(SearchDataIndex).col1, SearchData.ElementAt(SearchDataIndex).col2, SearchData.ElementAt(SearchDataIndex).col3);
                                SearchDataDisplay[1] = new SearchDataColumn(SearchData.ElementAt(SearchDataIndex + 1).col1, SearchData.ElementAt(SearchDataIndex + 1).col2, SearchData.ElementAt(SearchDataIndex + 1).col3);
                                SearchDataDisplay[2] = new SearchDataColumn(SearchData.ElementAt(SearchDataIndex + 2).col1, SearchData.ElementAt(SearchDataIndex + 2).col2, SearchData.ElementAt(SearchDataIndex + 2).col3);
                                SearchDataDisplay[3] = new SearchDataColumn("", "", "");
                                if (SearchDataDisplayIndex > 0)
                                {
                                    SearchDataDisplayIndex--;
                                }
                                else if (SearchDataDisplayIndex < 0)
                                {
                                    SearchDataDisplayIndex = 0;
                                }
                            }
                            else if (diff == 2)
                            {
                                SearchDataDisplay[0] = new SearchDataColumn(SearchData.ElementAt(SearchDataIndex).col1, SearchData.ElementAt(SearchDataIndex).col2, SearchData.ElementAt(SearchDataIndex).col3);
                                SearchDataDisplay[1] = new SearchDataColumn(SearchData.ElementAt(SearchDataIndex + 1).col1, SearchData.ElementAt(SearchDataIndex + 1).col2, SearchData.ElementAt(SearchDataIndex + 1).col3);
                                SearchDataDisplay[2] = new SearchDataColumn("", "", "");
                                SearchDataDisplay[3] = new SearchDataColumn("", "", "");
                                if (SearchDataDisplayIndex > 0)
                                {
                                    SearchDataDisplayIndex--;
                                }
                                else if (SearchDataDisplayIndex < 0)
                                {
                                    SearchDataDisplayIndex = 0;
                                }
                            }
                            else if (diff == 1)
                            {
                                SearchDataDisplay[0] = new SearchDataColumn(SearchData.ElementAt(SearchDataIndex).col1, SearchData.ElementAt(SearchDataIndex).col2, SearchData.ElementAt(SearchDataIndex).col3);
                                SearchDataDisplay[1] = new SearchDataColumn("", "", "");
                                SearchDataDisplay[2] = new SearchDataColumn("", "", "");
                                SearchDataDisplay[3] = new SearchDataColumn("", "", "");
                                if (SearchDataDisplayIndex > 0)
                                {
                                    SearchDataDisplayIndex--;
                                }
                                else if (SearchDataDisplayIndex < 0)
                                {
                                    SearchDataDisplayIndex = 0;
                                }
                            }
                            else if (diff == 0)
                            {
                                SearchDataDisplay[0] = new SearchDataColumn("", "", "");
                                SearchDataDisplay[1] = new SearchDataColumn("No", "Entry", "Found");
                                SearchDataDisplay[2] = new SearchDataColumn("", "", "");
                                SearchDataDisplay[3] = new SearchDataColumn("", "", "");
                            }



                        }
                        else if (value == "Down") {
                            int diff = SearchData.Count - SearchDataIndex;

                            if (diff >= 4) {
                                
                                SearchDataDisplay[0] = new SearchDataColumn(SearchData.ElementAt(SearchDataIndex).col1, SearchData.ElementAt(SearchDataIndex).col2, SearchData.ElementAt(SearchDataIndex).col3);
                                SearchDataDisplay[1] = new SearchDataColumn(SearchData.ElementAt(SearchDataIndex + 1).col1, SearchData.ElementAt(SearchDataIndex + 1).col2, SearchData.ElementAt(SearchDataIndex + 1).col3);
                                SearchDataDisplay[2] = new SearchDataColumn(SearchData.ElementAt(SearchDataIndex + 2).col1, SearchData.ElementAt(SearchDataIndex + 2).col2, SearchData.ElementAt(SearchDataIndex + 2).col3);
                                SearchDataDisplay[3] = new SearchDataColumn(SearchData.ElementAt(SearchDataIndex + 3).col1, SearchData.ElementAt(SearchDataIndex + 3).col2, SearchData.ElementAt(SearchDataIndex + 3).col3);

                                if (diff > 4)
                                {
                                    SearchDataIndex++;
                                }

                                if(SearchDataDisplayIndex < 3){
                                    SearchDataDisplayIndex++;
                                }
                                else if (SearchDataDisplayIndex > 3)
                                {
                                    SearchDataDisplayIndex = 0;
                                }
                            }
                            else if (diff == 3)
                            {
                                SearchDataDisplay[0] = new SearchDataColumn(SearchData.ElementAt(SearchDataIndex).col1, SearchData.ElementAt(SearchDataIndex).col2, SearchData.ElementAt(SearchDataIndex).col3);
                                SearchDataDisplay[1] = new SearchDataColumn(SearchData.ElementAt(SearchDataIndex + 1).col1, SearchData.ElementAt(SearchDataIndex + 1).col2, SearchData.ElementAt(SearchDataIndex + 1).col3);
                                SearchDataDisplay[2] = new SearchDataColumn(SearchData.ElementAt(SearchDataIndex + 2).col1, SearchData.ElementAt(SearchDataIndex + 2).col2, SearchData.ElementAt(SearchDataIndex + 2).col3);
                                SearchDataDisplay[3] = new SearchDataColumn("", "", "");
                                if (SearchDataDisplayIndex < 2)
                                {
                                    SearchDataDisplayIndex++;
                                }
                                else if (SearchDataDisplayIndex > 2)
                                {
                                    SearchDataDisplayIndex = 0;
                                }
                            }
                            else if (diff == 2)
                            {
                                SearchDataDisplay[0] = new SearchDataColumn(SearchData.ElementAt(SearchDataIndex).col1, SearchData.ElementAt(SearchDataIndex).col2, SearchData.ElementAt(SearchDataIndex).col3);
                                SearchDataDisplay[1] = new SearchDataColumn(SearchData.ElementAt(SearchDataIndex + 1).col1, SearchData.ElementAt(SearchDataIndex + 1).col2, SearchData.ElementAt(SearchDataIndex + 1).col3);
                                SearchDataDisplay[2] = new SearchDataColumn("", "", "");
                                SearchDataDisplay[3] = new SearchDataColumn("", "", "");
                                if (SearchDataDisplayIndex < 1)
                                {
                                    SearchDataDisplayIndex++;
                                }
                                else if (SearchDataDisplayIndex > 1)
                                {
                                    SearchDataDisplayIndex = 0;
                                }
                            }
                            else if (diff == 1)
                            {
                                SearchDataDisplay[0] = new SearchDataColumn(SearchData.ElementAt(SearchDataIndex).col1, SearchData.ElementAt(SearchDataIndex).col2, SearchData.ElementAt(SearchDataIndex).col3);
                                SearchDataDisplay[1] = new SearchDataColumn("", "", "");
                                SearchDataDisplay[2] = new SearchDataColumn("", "", "");
                                SearchDataDisplay[3] = new SearchDataColumn("", "", "");
                                if (SearchDataDisplayIndex > 0)
                                {
                                    SearchDataDisplayIndex = 0;
                                }
                            }
                            else if (diff == 0) {
                                SearchDataDisplay[0] = new SearchDataColumn("", "", "");
                                SearchDataDisplay[1] = new SearchDataColumn("No", "Entry", "Found");
                                SearchDataDisplay[2] = new SearchDataColumn("", "", "");
                                SearchDataDisplay[3] = new SearchDataColumn("", "", "");
                            }
                        }
                    }

                }
                get {
                    //Backspace Pressed
                    if(WhichDataInFrame == 1){
                        FrameData_UserID = FrameData_UserID/10;
                    }
                    else if (WhichDataInFrame == 2) {
                        if (FrameData_Name.Length > 0) {
                            FrameData_Name = FrameData_Name.Remove((FrameData_Name.Length - 1), 1);
                        }
                    }
                    else if (WhichDataInFrame == 3)
                    {
                        if (FrameData_Phone.Length > 0)
                        {
                            FrameData_Phone = FrameData_Phone.Remove((FrameData_Phone.Length - 1), 1);
                        }
                    }
                    else if (WhichDataInFrame == 4)
                    {
                        //Its Day
                        if(day.Length > 0)
                            day = day.Remove((day.Length - 1), 1);
                    }
                    else if (WhichDataInFrame == 5)
                    {
                        //Its Month
                        if (month.Length > 0)
                            month = month.Remove((month.Length - 1), 1);
                    }
                    else if (WhichDataInFrame == 6)
                    {
                        //Its Year
                        if (year.Length > 0)
                            year = year.Remove((year.Length - 1), 1);
                    }
                    else if (WhichDataInFrame == 7)
                    {
                        //Its hour
                        if (hour.Length > 0)
                            hour = hour.Remove((hour.Length - 1), 1);
                    }
                    else if (WhichDataInFrame == 8)
                    {
                        //Its minute
                        if (minute.Length > 0)
                            minute = minute.Remove((minute.Length - 1), 1);
                    }
                    else if (WhichDataInFrame == 9)
                    {
                        //Its second
                        if (second.Length > 0)
                            second = second.Remove((second.Length - 1), 1);
                    }
                    else if (WhichDataInFrame == 10)
                    {
                        //Its Day of Week
                        //Nothing to Do
                    }
                    else if (WhichDataInFrame == 12)
                    {
                        //Its Old Password
                        if (oldPassword.Length > 0)
                            oldPassword = oldPassword.Remove((oldPassword.Length - 1), 1);
                    }
                    else if (WhichDataInFrame == 13)
                    {
                        //Its New Password
                        if (newPassword.Length > 0)
                            newPassword = newPassword.Remove((newPassword.Length - 1), 1);
                    }
                    else if (WhichDataInFrame == 14)
                    {
                        //Its Re-type Password
                        if (RetypePassword.Length > 0)
                            RetypePassword = RetypePassword.Remove((RetypePassword.Length - 1), 1);
                    }
                    else if (WhichDataInFrame == 15)
                    {
                        //Its Toggle Switch
                        //Nothing to Do
                    }
                    else if (WhichDataInFrame == 16)
                    {
                        if (screenTittle.Length > 0)
                        {
                            screenTittle = screenTittle.Remove((screenTittle.Length - 1), 1);
                        }
                    }
                    else if (WhichDataInFrame == 20)
                    {

                        if (float1_0index == 1)
                        {
                            float1_0index = 0;
                            float1_0[0] = "";
                        }
                        else if (float1_0index == 0)
                        {
                            float1_0[0] = "";
                        }
                        else if (float1_0index == 2)
                        {
                            float1_0index = 0;
                            float1_0[0] = "";
                        }

                        float1_0data = "";
                        for (int i = 0; i <= float1_0index; i++)
                        {
                            float1_0data += float1_0[i];
                        }
                    }
                    else if (WhichDataInFrame == 21)
                    {

                        if (float2_0index == 1)
                        {
                            float2_0index = 0;
                            float2_0[0] = "";
                        }
                        else if (float2_0index == 0)
                        {
                            float2_0[0] = "";
                        }
                        else if (float2_0index == 2)
                        {
                            float2_0index = 0;
                            float2_0[0] = "";
                        }
                        else if (float2_0index == 3)
                        {
                            float2_0index = 2;
                            float2_0[2] = "";
                        }

                        float2_0data = "";
                        for (int i = 0; i <= float2_0index; i++)
                        {
                            float2_0data += float2_0[i];
                        }

                    }
                    else if (WhichDataInFrame == 22)
                    {

                        if (float3_0index == 1)
                        {
                            float3_0index = 0;
                            float3_0[0] = "";
                        }
                        else if (float3_0index == 0)
                        {
                            float3_0[0] = "";
                        }
                        else if (float3_0index == 3)
                        {
                            float3_0index = 1;
                            float3_0[1] = "";
                        }
                        else if (float3_0index == 4)
                        {
                            float3_0index = 3;
                            float3_0[3] = "";
                        }

                        float3_0data = "";
                        for (int i = 0; i <= float3_0index; i++)
                        {
                            float3_0data += float3_0[i];
                        }

                    }
                    else if (WhichDataInFrame == 23)
                    {
                        
                        if (float4_0index == 1)
                        {
                            float4_0index = 0;
                            float4_0[0] = "";
                        }
                        else if (float4_0index == 0) {
                            float4_0[0] = "";
                        }
                        else if (float4_0index == 3) {
                            float4_0index = 1;
                            float4_0[1] = "";
                        }
                        else if (float4_0index == 4) {
                            float4_0index = 3;
                            float4_0[3] = "";
                        }

                        float4_0data = "";
                        for (int i = 0; i <= float4_0index; i++) {
                            float4_0data += float4_0[i];
                        }

                    }
                    else if (WhichDataInFrame == 24) 
                    {
                        if (float1_1index == 1)
                        {
                            float1_1index = 0;
                            float1_1[0] = "";
                        }
                        else if (float1_1index == 0)
                        {
                            float1_1[0] = "";
                        }
                        else if (float1_1index == 2)
                        {
                            float1_1index = 0;
                            float1_1[0] = "";
                        }

                        float1_1data = "";
                        for (int i = 0; i <= float1_1index; i++)
                        {
                            float1_1data += float1_1[i];
                        }
                    }
                    else if (WhichDataInFrame == 25)
                    {
                        if (float2_1index == 1)
                        {
                            float2_1index = 0;
                            float2_1[0] = "";
                        }
                        else if (float2_1index == 0)
                        {
                            float2_1[0] = "";
                        }
                        else if (float2_1index == 2)
                        {
                            float2_1index = 0;
                            float2_1[0] = "";
                        }
                        else if (float2_1index == 3)
                        {
                            float2_1index = 2;
                            float2_1[2] = "";
                        }

                        float2_1data = "";
                        for (int i = 0; i <= float2_1index; i++)
                        {
                            float2_1data += float2_1[i];
                        }
                    }
                    else if (WhichDataInFrame == 26)
                    {
                        if (float3_1index == 1)
                        {
                            float3_1index = 0;
                            float3_1[0] = "";
                        }
                        else if (float3_1index == 0)
                        {
                            float3_1[0] = "";
                        }
                        else if (float3_1index == 3)
                        {
                            float3_1index = 1;
                            float3_1[1] = "";
                        }
                        else if (float3_1index == 4)
                        {
                            float3_1index = 3;
                            float3_1[3] = "";
                        }

                        float3_1data = "";
                        for (int i = 0; i <= float3_1index; i++)
                        {
                            float3_1data += float3_1[i];
                        }
                    }
                    else if (WhichDataInFrame == 27)
                    {
                        if (float4_1index == 1)
                        {
                            float4_1index = 0;
                            float4_1[0] = "";
                        }
                        else if (float4_1index == 0)
                        {
                            float4_1[0] = "";
                        }
                        else if (float4_1index == 3)
                        {
                            float4_1index = 1;
                            float4_1[1] = "";
                        }
                        else if (float4_1index == 4)
                        {
                            float4_1index = 3;
                            float4_1[3] = "";
                        }

                        float4_1data = "";
                        for (int i = 0; i <= float4_1index; i++)
                        {
                            float4_1data += float4_1[i];
                        }
                    }
                    else if (WhichDataInFrame == 28)
                    {
                        if (VechileNo.Length > 0)
                        {
                            VechileNo = VechileNo.Remove((VechileNo.Length - 1), 1);
                        }
                    }
                    else if (WhichDataInFrame == 29)
                    {
                        //000.00
                        if (float5_0index == 1)
                        {
                            float5_0index = 0;
                            float5_0[0] = "";
                        }
                        else if (float5_0index == 2)
                        {
                            float5_0index = 1;
                            float5_0[1] = "";
                        }
                        else if (float5_0index == 0)
                        {
                            float5_0[0] = "";
                        }
                        
                        else if (float5_0index == 4)
                        {
                            float5_0index = 2;
                            float5_0[2] = "";
                        }
                        else if (float5_0index == 5)
                        {
                            float5_0index = 4;
                            float5_0[4] = "";
                        }

                        float5_0data = "";
                        for (int i = 0; i <= float5_0index; i++)
                        {
                            float5_0data += float5_0[i];
                        }
                    }
                    else if (WhichDataInFrame == 30)
                    {
                        //0000.00
                        if (float6_0index == 1)
                        {
                            float6_0index = 0;
                            float6_0[0] = "";
                        }
                        else if (float6_0index == 2)
                        {
                            float6_0index = 1;
                            float6_0[1] = "";
                        }
                        else if (float6_0index == 3)
                        {
                            float6_0index = 2;
                            float6_0[2] = "";
                        }
                        else if (float6_0index == 0)
                        {
                            float6_0[0] = "";
                        }

                        else if (float6_0index == 5)
                        {
                            float6_0index = 3;
                            float6_0[3] = "";
                        }
                        else if (float6_0index == 6)
                        {
                            float6_0index = 5;
                            float6_0[5] = "";
                        }

                        float6_0data = "";
                        for (int i = 0; i <= float6_0index; i++)
                        {
                            float6_0data += float6_0[i];
                        }
                    }
                    else if (WhichDataInFrame == 31)
                    {
                        //00000.00
                        if (float7_0index == 1)
                        {
                            float7_0index = 0;
                            float7_0[0] = "";
                        }
                        else if (float7_0index == 2)
                        {
                            float7_0index = 1;
                            float7_0[1] = "";
                        }
                        else if (float7_0index == 3)
                        {
                            float7_0index = 2;
                            float7_0[2] = "";
                        }
                        else if (float7_0index == 4)
                        {
                            float7_0index = 3;
                            float7_0[3] = "";
                        }
                        else if (float7_0index == 0)
                        {
                            float7_0[0] = "";
                        }

                        else if (float7_0index == 6)
                        {
                            float7_0index = 4;
                            float7_0[4] = "";
                        }
                        else if (float7_0index == 7)
                        {
                            float7_0index = 6;
                            float7_0[6] = "";
                        }

                        float7_0data = "";
                        for (int i = 0; i <= float7_0index; i++)
                        {
                            float7_0data += float7_0[i];
                        }
                    }
                    else if (WhichDataInFrame == 32)
                    {
                        //000.00
                        if (float5_1index == 1)
                        {
                            float5_1index = 0;
                            float5_1[0] = "";
                        }
                        else if (float5_1index == 2)
                        {
                            float5_1index = 1;
                            float5_1[1] = "";
                        }
                        else if (float5_1index == 0)
                        {
                            float5_1[0] = "";
                        }

                        else if (float5_1index == 4)
                        {
                            float5_1index = 2;
                            float5_1[2] = "";
                        }
                        else if (float5_1index == 5)
                        {
                            float5_1index = 4;
                            float5_1[4] = "";
                        }

                        float5_1data = "";
                        for (int i = 0; i <= float5_1index; i++)
                        {
                            float5_1data += float5_1[i];
                        }
                    }
                    else if (WhichDataInFrame == 33)
                    {
                        //0000.00
                        if (float6_1index == 1)
                        {
                            float6_1index = 0;
                            float6_1[0] = "";
                        }
                        else if (float6_1index == 2)
                        {
                            float6_1index = 1;
                            float6_1[1] = "";
                        }
                        else if (float6_1index == 3)
                        {
                            float6_1index = 2;
                            float6_1[2] = "";
                        }
                        else if (float6_1index == 0)
                        {
                            float6_1[0] = "";
                        }

                        else if (float6_1index == 5)
                        {
                            float6_1index = 3;
                            float6_1[3] = "";
                        }
                        else if (float6_1index == 6)
                        {
                            float6_1index = 5;
                            float6_1[5] = "";
                        }

                        float6_1data = "";
                        for (int i = 0; i <= float6_1index; i++)
                        {
                            float6_1data += float6_1[i];
                        }
                    }
                    else if (WhichDataInFrame == 34)
                    {
                        //00000.00
                        if (float7_1index == 1)
                        {
                            float7_1index = 0;
                            float7_1[0] = "";
                        }
                        else if (float7_1index == 2)
                        {
                            float7_1index = 1;
                            float7_1[1] = "";
                        }
                        else if (float7_1index == 3)
                        {
                            float7_1index = 2;
                            float7_1[2] = "";
                        }
                        else if (float7_1index == 4)
                        {
                            float7_1index = 3;
                            float7_1[3] = "";
                        }
                        else if (float7_1index == 0)
                        {
                            float7_1[0] = "";
                        }

                        else if (float7_1index == 6)
                        {
                            float7_1index = 4;
                            float7_1[4] = "";
                        }
                        else if (float7_1index == 7)
                        {
                            float7_1index = 6;
                            float7_1[6] = "";
                        }

                        float7_1data = "";
                        for (int i = 0; i <= float7_1index; i++)
                        {
                            float7_1data += float7_1[i];
                        }
                    }
                    else if (WhichDataInFrame == 35)
                    {
                        if (float4_2index == 1)
                        {
                            float4_2index = 0;
                            float4_2[0] = "";
                        }
                        else if (float4_2index == 0)
                        {
                            float4_2[0] = "";
                        }
                        else if (float4_2index == 3)
                        {
                            float4_2index = 1;
                            float4_2[1] = "";
                        }
                        else if (float4_2index == 4)
                        {
                            float4_2index = 3;
                            float4_2[3] = "";
                        }

                        float4_2data = "";
                        for (int i = 0; i <= float4_2index; i++)
                        {
                            float4_2data += float4_2[i];
                        }
                    }
                    else if (WhichDataInFrame == 36)
                    {
                        if (float4_3index == 1)
                        {
                            float4_3index = 0;
                            float4_3[0] = "";
                        }
                        else if (float4_3index == 0)
                        {
                            float4_3[0] = "";
                        }
                        else if (float4_3index == 3)
                        {
                            float4_3index = 1;
                            float4_3[1] = "";
                        }
                        else if (float4_3index == 4)
                        {
                            float4_3index = 3;
                            float4_3[3] = "";
                        }

                        float4_3data = "";
                        for (int i = 0; i <= float4_3index; i++)
                        {
                            float4_3data += float4_3[i];
                        }
                    }
                    else if (WhichDataInFrame == 37)
                    {
                        if (float4_4index == 1)
                        {
                            float4_4index = 0;
                            float4_4[0] = "";
                        }
                        else if (float4_4index == 0)
                        {
                            float4_4[0] = "";
                        }
                        else if (float4_4index == 3)
                        {
                            float4_4index = 1;
                            float4_4[1] = "";
                        }
                        else if (float4_4index == 4)
                        {
                            float4_4index = 3;
                            float4_4[3] = "";
                        }

                        float4_4data = "";
                        for (int i = 0; i <= float4_4index; i++)
                        {
                            float4_4data += float4_4[i];
                        }
                    }
                    else if (WhichDataInFrame == 38)
                    {
                        if (float4_5index == 1)
                        {
                            float4_5index = 0;
                            float4_5[0] = "";
                        }
                        else if (float4_5index == 0)
                        {
                            float4_5[0] = "";
                        }
                        else if (float4_5index == 3)
                        {
                            float4_5index = 1;
                            float4_5[1] = "";
                        }
                        else if (float4_5index == 4)
                        {
                            float4_5index = 3;
                            float4_5[3] = "";
                        }

                        float4_5data = "";
                        for (int i = 0; i <= float4_5index; i++)
                        {
                            float4_5data += float4_5[i];
                        }
                    }
                    else if (WhichDataInFrame == 45) {
                        if (integer1_0.Length > 0)
                            integer1_0 = integer1_0.Remove((integer1_0.Length - 1), 1);
                    }
                    else if (WhichDataInFrame == 46)
                    {
                        if (integer2_0.Length > 0)
                            integer2_0 = integer2_0.Remove((integer2_0.Length - 1), 1);
                    }
                    else if (WhichDataInFrame == 47)
                    {
                        if (integer3_0.Length > 0)
                            integer3_0 = integer3_0.Remove((integer3_0.Length - 1), 1);
                    }
                    else if (WhichDataInFrame == 48)
                    {
                        if (integer4_0.Length > 0)
                            integer4_0 = integer4_0.Remove((integer4_0.Length - 1), 1);
                    }
                    else if (WhichDataInFrame == 49)
                    {
                        if (integer5_0.Length > 0)
                            integer5_0 = integer5_0.Remove((integer5_0.Length - 1), 1);
                    }
                    else if (WhichDataInFrame == 50)
                    {
                        if (integer1_1.Length > 0)
                            integer1_1 = integer1_1.Remove((integer1_1.Length - 1), 1);
                    }
                    else if (WhichDataInFrame == 51)
                    {
                        if (integer2_1.Length > 0)
                            integer2_1 = integer2_1.Remove((integer2_1.Length - 1), 1);
                    }
                    else if (WhichDataInFrame == 52)
                    {
                        if (integer3_1.Length > 0)
                            integer3_1 = integer3_1.Remove((integer3_1.Length - 1), 1);
                    }
                    else if (WhichDataInFrame == 53)
                    {
                        if (integer4_1.Length > 0)
                            integer4_1 = integer4_1.Remove((integer4_1.Length - 1), 1);
                    }
                    else if (WhichDataInFrame == 54)
                    {
                        if (integer5_1.Length > 0)
                            integer5_1 = integer5_1.Remove((integer5_1.Length - 1), 1);
                    }
                    else if (WhichDataInFrame == 65)
                    {
                        //Its Password
                        if (passwordGlobal.Length > 0 && passwordGlobalReal.Length > 0)
                        {
                            passwordGlobal = passwordGlobal.Remove((passwordGlobal.Length - 1), 1);
                            passwordGlobalReal = passwordGlobalReal.Remove((passwordGlobalReal.Length - 1), 1);
                        }
                    }
                    return "";
                }
            }

        };

        

        public static void FindNextInputInFrame(bool choice) {

            //Move Forward
            if (choice)
            {
                if (ActiveFrameInfo.ActiveFrameInput.inputIndex == (ActiveFrameInfo.InputList.Count -1))
                {
                    ActiveFrameInfo.ActiveFrameInput = ActiveFrameInfo.InputList.ElementAt<FrameInputInfo>(0);
                }
                else
                {
                    ActiveFrameInfo.ActiveFrameInput = ActiveFrameInfo.InputList.ElementAt<FrameInputInfo>(int.Parse((ActiveFrameInfo.ActiveFrameInput.inputIndex + 1).ToString()));
                }
            }

                //Move Backward
            else if (!choice)
            {

                if (ActiveFrameInfo.ActiveFrameInput.inputIndex == 0)
                {
                    ActiveFrameInfo.ActiveFrameInput = ActiveFrameInfo.InputList.ElementAt<FrameInputInfo>(ActiveFrameInfo.InputList.Count - 1);
                }
                else
                {
                    ActiveFrameInfo.ActiveFrameInput = ActiveFrameInfo.InputList.ElementAt<FrameInputInfo>(int.Parse((ActiveFrameInfo.ActiveFrameInput.inputIndex - 1).ToString()));
                }
            }

            //Now Assigning the Evaluated Values in ActiveFrame
            ActiveFrameInfo.WhichDataInFrame = ActiveFrameInfo.ActiveFrameInput.WhichData;
            ActiveFrameInfo.ActiveY = ActiveFrameInfo.ActiveFrameInput.LY;
            ActiveFrameInfo.ActiveX = ActiveFrameInfo.ActiveFrameInput.LX;

            RefreshFrame();
        }
        


        public  delegate void HomeStartedSubscriber();
        public static event HomeStartedSubscriber HomeStartedSubscriberEvent;

        public  delegate void HomeStopSubscriber();
        public static event HomeStopSubscriber HomeStopSubscriberEvent;


        public static Thread Thread_FramePrinter;
        public static Thread Thread_RefreshFrame;

        public static void startOperation(UInt64 _id){
            LCDFrameProcessor.FrameBase = _id;
            Thread_FramePrinter = new Thread(Thread_FramePrinter_Work);
            Thread_FramePrinter.IsBackground = true;
            Thread_FramePrinter.Priority = ThreadPriority.Highest;
            ActiveFrameInfo.isYN = false;
            Thread_FramePrinter.Start();
        }

        public static void Thread_FramePrinter_Work() {
            while (LCDFunctions.LCDLock == true)
            {
                Thread.Sleep(200);
            }

            LCDFunctions.LCDLock = true;
            Busy = true;
            LCDFrameProcessor.FramePrinter();
            LCDFunctions.LCDLock = false;
            Busy = false;

        }

        public static void Thread_RefreshFrame_Work() {
            while (LCDFunctions.LCDLock == true)
            {
                Thread.Sleep(50);
            }

            LCDFunctions.LCDLock = true;
            Busy = true;
            LCDrefreshFlag = true;
            LCDFrameProcessor.FramePrinter();
            LCDrefreshFlag = false;
            LCDFunctions.LCDLock = false;
            Busy = false;
        }

        public static void RefreshFrame()
        {
            
            Thread_RefreshFrame = new Thread(Thread_RefreshFrame_Work);
            Thread_RefreshFrame.IsBackground = true;
            Thread_RefreshFrame.Priority = ThreadPriority.AboveNormal;
            Thread_RefreshFrame.Start();
        }

        public static void FramePrinter(){

            if (FrameBase > 0)
            {
                HomeStopSubscriberEvent();
                backTimerFlag = false;
            }


            switch (FrameBase) {

                case 0:

                    {
                        LCDFunctions.InitLCD();
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(0, 0);
                        LCDFunctions.sendString("Data Processing Unit");
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString(DataKeeper.GetValue("SCREEN_TITTLE_SELECTED"));

                        
                        LCDFunctions.gotoLCD(6, 2);
                        LCDFunctions.sendString(DateTime.Now.ToString("dd:MM:yy"));
                       
                        
                        LCDFunctions.gotoLCD(6, 3);
                        LCDFunctions.sendString(DateTime.Now.ToString("hh:mm:ss tt"));
                        LCDFunctions.CursorON_OFF(false);

                        Thread.Sleep(10);

                        HomeStartedSubscriberEvent();
                        backTimerFlag = true;
                        FrameInformationDatabase(0);
                        
                        F5_LOCK_STATUS = true;
                        F12_PASSWORD_STATE = true;
                        F2_PASSWORD_STATE = true;
                        F10_PASSWORD_STATE = true;
                        break;
                    }

                case 1: {
                    if (true)
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.gotoLCD(0, 0);
                        LCDFunctions.sendString("Create/Del Member  1");
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString("Add/Modify Name    2");
                        LCDFunctions.gotoLCD(0, 2);
                        LCDFunctions.sendString("Print/Show All   3/4");
                        LCDFunctions.gotoLCD(0, 3);
                        LCDFunctions.sendString("Power Off - - - -  5");
                        Thread.Sleep(1);
                        FrameInformationDatabase(1);
                        
                    }
                    break;
                }
                case 2:
                    {

                        if (!F2_PASSWORD_STATE)
                        {
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(false);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Add Rate Chart   : 1");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Ratechart Setting: 2");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("Print Rate Chart : 3");
                            LCDFunctions.gotoLCD(0, 3);
                            LCDFunctions.sendString("Lock Rate Chart  : 4");
                            LCDrefreshFlag = false;
                            
                        }
                        else if (F2_PASSWORD_STATE) {
                            if (!LCDrefreshFlag)
                            {
                                Thread.Sleep(1);
                                LCDFunctions.clearLCD();
                                LCDFunctions.CursorON_OFF(true);
                                LCDFunctions.gotoLCD(0, 0);
                                LCDFunctions.sendString("Password Protected");
                                LCDFunctions.gotoLCD(0, 1);
                                LCDFunctions.sendString("Enter : ");
                                FrameInformationDatabase(12);
                            }
                            else if (LCDrefreshFlag)
                            {
                                LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                                if (ActiveFrameInfo.WhichDataInFrame == 65)
                                {
                                    LCDFunctions.sendString(ActiveFrameInfo.passwordGlobal);
                                }
                            }
                        }

                        break;
                    }
                case 3:
                    {

                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Press Enter to Save.");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Time: 00:00:00");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("Date: 00/00/00");
                            LCDFunctions.gotoLCD(0, 3);
                            LCDFunctions.sendString("Day:  Sun");
                            LCDFunctions.CursorON_OFF(true);
                            LCDFunctions.gotoLCD(6, 1);
                            Thread.Sleep(1);
                            FrameInformationDatabase(3);
                        }
                        else if (LCDrefreshFlag) {

                            LCDFunctions.gotoLCD(6, 2);
                            LCDFunctions.sendString(ActiveFrameInfo.day);
                            LCDFunctions.gotoLCD(9, 2);
                            LCDFunctions.sendString(ActiveFrameInfo.month);
                            LCDFunctions.gotoLCD(12, 2);
                            LCDFunctions.sendString(ActiveFrameInfo.year);
                            LCDFunctions.gotoLCD(6, 1);
                            LCDFunctions.sendString(ActiveFrameInfo.hour);
                            LCDFunctions.gotoLCD(9, 1);
                            LCDFunctions.sendString(ActiveFrameInfo.minute);
                            LCDFunctions.gotoLCD(12, 1);
                            LCDFunctions.sendString(ActiveFrameInfo.second);
                            LCDFunctions.gotoLCD(6, 3);
                            LCDFunctions.sendString(DayofWeek[ActiveFrameInfo.whichDayofWeek]);
                       

                            
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X,ActiveFrameInfo.LCD_Y);
                            if(ActiveFrameInfo.WhichDataInFrame == 4)
                                LCDFunctions.sendString(ActiveFrameInfo.day);
                            else if (ActiveFrameInfo.WhichDataInFrame == 5)
                                LCDFunctions.sendString(ActiveFrameInfo.month);
                            else if(ActiveFrameInfo.WhichDataInFrame == 6)
                                LCDFunctions.sendString(ActiveFrameInfo.year);
                            else if (ActiveFrameInfo.WhichDataInFrame == 7)
                                LCDFunctions.sendString(ActiveFrameInfo.hour);
                            else if (ActiveFrameInfo.WhichDataInFrame == 8)
                                LCDFunctions.sendString(ActiveFrameInfo.minute);
                            else if (ActiveFrameInfo.WhichDataInFrame == 9)
                                LCDFunctions.sendString(ActiveFrameInfo.second);
                            else if (ActiveFrameInfo.WhichDataInFrame == 10)
                                LCDFunctions.sendString(DayofWeek[ActiveFrameInfo.whichDayofWeek]);
                            //LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                        }
                        break;
                    }

                case 4:
                    {
                        if (true)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(false);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("LOCK/Dairy Name :1/2");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Select Machine  :  3");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("Slct Printr/Wtg.:4/5");
                            LCDFunctions.gotoLCD(0, 3);
                            LCDFunctions.sendString("Manual Collection: 6");
                            FrameInformationDatabase(4);
                            Thread.Sleep(1);
                        }
                        break;
                    }

                case 5:
                    {
                        if (!LCDrefreshFlag)
                        {
                            //Input Data Frame
                            if (F5_LOCK_STATUS)
                            {
                                Thread.Sleep(1);
                                LCDFunctions.clearLCD();
                                LCDFunctions.CursorON_OFF(false);
                                LCDFunctions.gotoLCD(0, 1);
                                LCDFunctions.sendString("Go Manual(Y)/Auto(N)");
                                Thread.Sleep(1);
                                break;
                            }
                            else if (!F5_LOCK_STATUS)
                            {
                                Thread.Sleep(5);
                                LCDFunctions.CursorON_OFF(false);
                                LCDFunctions.clearLCD();
                                LCDFunctions.gotoLCD(0, 0);
                                LCDFunctions.sendString("Collection:");
                                LCDFunctions.gotoLCD(0, 1);
                                LCDFunctions.sendString("Enter Code:");
                                LCDFunctions.gotoLCD(8, 2);
                                LCDFunctions.sendString("000");
                                LCDFunctions.gotoLCD(10, 2);
                                LCDFunctions.CursorON_OFF(true);
                                Thread.Sleep(5);

                                FrameInformationDatabase(5);
                                break;
                            }
                        }
                        else 
                        {

                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            LCDFunctions.sendString("000");
                            LCDFunctions.gotoLCD((ActiveFrameInfo.LCD_X + 3 - ActiveFrameInfo.FrameData_UserID.ToString().Length), ActiveFrameInfo.LCD_Y);
                            LCDFunctions.sendString(ActiveFrameInfo.FrameData_UserID.ToString());
                            LCDFunctions.gotoLCD((ActiveFrameInfo.LCD_X + 2), ActiveFrameInfo.LCD_Y);
                        }
                        break;
                    }

                case 6:
                    {

                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(5);
                            LCDFunctions.CursorON_OFF(false);
                            LCDFunctions.clearLCD();
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Duplicate Slip:");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Enter Code:");
                            LCDFunctions.gotoLCD(8, 2);
                            LCDFunctions.sendString("000");
                            LCDFunctions.gotoLCD(10, 2);
                            LCDFunctions.CursorON_OFF(true);
                            Thread.Sleep(5);

                            FrameInformationDatabase(5);
                            break;
                        }
                        else if (LCDrefreshFlag) {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            LCDFunctions.sendString("000");
                            LCDFunctions.gotoLCD((ActiveFrameInfo.LCD_X + 3 - ActiveFrameInfo.FrameData_UserID.ToString().Length), ActiveFrameInfo.LCD_Y);
                            LCDFunctions.sendString(ActiveFrameInfo.FrameData_UserID.ToString());
                            LCDFunctions.gotoLCD((ActiveFrameInfo.LCD_X + 2), ActiveFrameInfo.LCD_Y);
                        
                        }
                        break;
                    }

                case 7:
                    {

                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Shift Report:");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Date: 00/00/00");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("Shift: (M)");
                            LCDFunctions.gotoLCD(6, 1);
                            Thread.Sleep(1);
                            FrameInformationDatabase(7);
                        }else if(LCDrefreshFlag){
                            LCDFunctions.gotoLCD(6, 1);
                            LCDFunctions.sendString(ActiveFrameInfo.day);
                            LCDFunctions.gotoLCD(9, 1);
                            LCDFunctions.sendString(ActiveFrameInfo.month);
                            LCDFunctions.gotoLCD(12, 1);
                            LCDFunctions.sendString(ActiveFrameInfo.year);
                            LCDFunctions.gotoLCD(8, 2);
                            LCDFunctions.sendString(Shift[ActiveFrameInfo.whichShift].ToString());

                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 4)
                                LCDFunctions.sendString(ActiveFrameInfo.day);
                            else if (ActiveFrameInfo.WhichDataInFrame == 5)
                                LCDFunctions.sendString(ActiveFrameInfo.month);
                            else if (ActiveFrameInfo.WhichDataInFrame == 6)
                                LCDFunctions.sendString(ActiveFrameInfo.year);
                            else if (ActiveFrameInfo.whichShift == 11) {
                                LCDFunctions.sendString(Shift[ActiveFrameInfo.whichShift].ToString());
                            }
                        }
                        

                        break;
                    }

                case 8:
                    {

                        if (true)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(false);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Add Loc./Truck   1/2");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Del. Loc./Truck  3/4");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("Print Loc./Truck 5/6");
                            LCDFunctions.gotoLCD(0, 3);
                            LCDFunctions.sendString("Setting/Search   7/8");
                            Thread.Sleep(1);
                        }
                        break;
                    }

                case 9:
                    {

                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.CursorON_OFF(false);
                            LCDFunctions.clearLCD();
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Member Ledgr:");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Fr. Code:");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("To. Code:");
                            LCDFunctions.gotoLCD(10, 1);
                            LCDFunctions.CursorON_OFF(true);
                            Thread.Sleep(5);

                            FrameInformationDatabase(9);
                        }
                        else
                        {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 47) {
                                LCDFunctions.sendString("    ");
                                LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                                LCDFunctions.sendString(ActiveFrameInfo.integer3_0);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 52) {
                                LCDFunctions.sendString("    ");
                                LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                                LCDFunctions.sendString(ActiveFrameInfo.integer3_1);
                            }
                        }
                        break;
                    }

                case 10:
                    {
                        if (!F10_PASSWORD_STATE)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(false);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Credit Amount     1");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Deduct Amount     2");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("Print Report      3");
                            LCDFunctions.gotoLCD(0, 3);
                            LCDFunctions.sendString("Setting/Detail  4/5");
                            LCDFunctions.gotoLCD(8, 1);
                            LCDFunctions.CursorON_OFF(false);
                            Thread.Sleep(1);
                        }
                        else if (F10_PASSWORD_STATE)
                        {
                            if (!LCDrefreshFlag)
                            {
                                Thread.Sleep(1);
                                LCDFunctions.clearLCD();
                                LCDFunctions.CursorON_OFF(true);
                                LCDFunctions.gotoLCD(0, 0);
                                LCDFunctions.sendString("Password Protected");
                                LCDFunctions.gotoLCD(0, 1);
                                LCDFunctions.sendString("Enter : ");
                                FrameInformationDatabase(12);
                            }
                            else if (LCDrefreshFlag)
                            {
                                LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                                if (ActiveFrameInfo.WhichDataInFrame == 65)
                                {
                                    LCDFunctions.sendString(ActiveFrameInfo.passwordGlobal);
                                }
                            }
                        }
                        break;
                    }
                case 11:
                    {

                        if (true)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Delete a Trans: 1");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Delete Shift  : 2");
                            Thread.Sleep(1);
                        }
                        break;
                    }

                case 12:
                    {
                        if (!F12_PASSWORD_STATE)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(false);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("COM Setting       1");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Collection        2");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("Manage Dev/Pass 3/4");
                            LCDFunctions.gotoLCD(0, 3);
                            LCDFunctions.sendString("");
                            Thread.Sleep(1);
                            FrameInformationDatabase(12);
                        }
                        else if(F12_PASSWORD_STATE) {
                            if (!LCDrefreshFlag)
                            {
                                Thread.Sleep(1);
                                LCDFunctions.clearLCD();
                                LCDFunctions.CursorON_OFF(true);
                                LCDFunctions.gotoLCD(0, 0);
                                LCDFunctions.sendString("Password Protected");
                                LCDFunctions.gotoLCD(0, 1);
                                LCDFunctions.sendString("Enter : ");
                                FrameInformationDatabase(12);
                            }
                            else if (LCDrefreshFlag) {
                                LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                                if (ActiveFrameInfo.WhichDataInFrame == 65)
                                {
                                    LCDFunctions.sendString(ActiveFrameInfo.passwordGlobal);
                                }
                            }
                        }
                        break;
                    }

                    //Base 100 for Menu 1
                case 101:
                    {

                        if (!LCDrefreshFlag)
                        {

                            Thread.Sleep(5);
                            LCDFunctions.CursorON_OFF(false);
                            LCDFunctions.clearLCD();
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Create/Del Memeber");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Enter Code:");
                            LCDFunctions.gotoLCD(8, 2);
                            LCDFunctions.sendString("000");
                            LCDFunctions.gotoLCD(10, 2);
                            LCDFunctions.CursorON_OFF(true);
                            Thread.Sleep(5);

                            FrameInformationDatabase(101);
                        }
                        else {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            LCDFunctions.sendString("000");
                            LCDFunctions.gotoLCD((ActiveFrameInfo.LCD_X + 3 - ActiveFrameInfo.FrameData_UserID.ToString().Length),ActiveFrameInfo.LCD_Y);
                            LCDFunctions.sendString(ActiveFrameInfo.FrameData_UserID.ToString());
                            LCDFunctions.gotoLCD((ActiveFrameInfo.LCD_X + 2), ActiveFrameInfo.LCD_Y);
                        }

                        break;
                    }

                case 102:
                    {
                        if (!LCDrefreshFlag)
                        {

                            Thread.Sleep(1);
                            LCDFunctions.CursorON_OFF(false);
                            LCDFunctions.clearLCD();
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Add/Mod Memeber");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Enter Code:");
                            LCDFunctions.gotoLCD(8, 2);
                            LCDFunctions.sendString("000");
                            LCDFunctions.gotoLCD(10, 2);
                            LCDFunctions.CursorON_OFF(true);
                            Thread.Sleep(1);

                            FrameInformationDatabase(102);
                        }
                        else
                        {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            LCDFunctions.sendString("000");
                            LCDFunctions.gotoLCD((ActiveFrameInfo.LCD_X + 3 - ActiveFrameInfo.FrameData_UserID.ToString().Length), ActiveFrameInfo.LCD_Y);
                            LCDFunctions.sendString(ActiveFrameInfo.FrameData_UserID.ToString());
                            LCDFunctions.gotoLCD((ActiveFrameInfo.LCD_X + 2), ActiveFrameInfo.LCD_Y);
                        }

                        break;
                    }

                case 103:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString("Print Memeber list? ");
                        LCDFunctions.gotoLCD(0, 2);
                        LCDFunctions.sendString("       (Y/N)        ");
                        ActiveFrameInfo.isYN = true;
                        Thread.Sleep(1);

                        break;
                    }


                case 104:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(0, 0);
                        LCDFunctions.sendString("                    ");
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString("Show Memeber list?  ");
                        LCDFunctions.gotoLCD(0, 2);
                        LCDFunctions.sendString("       (Y/N)        ");
                        LCDFunctions.gotoLCD(0, 3);
                        LCDFunctions.sendString("                    ");
                        Thread.Sleep(1);
                        ActiveFrameInfo.isYN = true;
                        break;
                    }
                case 105:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString("Turn Off Machine    ");
                        LCDFunctions.gotoLCD(0, 2);
                        LCDFunctions.sendString("Are You Sure? (Y/N) ");
                        Thread.Sleep(1);
                        ActiveFrameInfo.isYN = true;
                        break;
                    }

                /*case 10410:
                    {
                        if (!LCDrefreshFlag) {
                            Thread.Sleep(1);
                            FrameInformationDatabase(10410);
                            LCDFunctions.clearLCD();
                            LCDFunctions.gotoLCD(3, 1);
                            LCDFunctions.sendString("[Wait......]");
                        }
                        else if (LCDrefreshFlag)
                        {
                            if (ProcessData.isAllMemberListEmpty)
                            {
                                LCDFunctions.clearLCD();
                                LCDFunctions.gotoLCD(1, 1);
                                LCDFunctions.sendString("Empty List");
                            }
                            else
                            {
                                Thread.Sleep(1);
                                LCDFunctions.clearLCD();
                                LCDFunctions.gotoLCD(0, (int)ProcessData.displayScrollBar);
                                LCDFunctions.sendString("*");

                                LCDFunctions.gotoLCD(2, 0);
                                LCDFunctions.sendString(ProcessData.activeIDforDisplay[0].ToString());
                                LCDFunctions.gotoLCD(6, 0);
                                if (ProcessData.activeIDforDisplay[0] != "" && ProcessData.activeIDforDisplay[0] != null) {
                                    LCDFunctions.sendString(ProcessData.GetName(uint.Parse(ProcessData.activeIDforDisplay[0])));
                                }
                                

                                LCDFunctions.gotoLCD(2, 1);
                                LCDFunctions.sendString(ProcessData.activeIDforDisplay[1].ToString());
                                LCDFunctions.gotoLCD(6, 1);
                                if (ProcessData.activeIDforDisplay[1] != "" && ProcessData.activeIDforDisplay[1] != null)
                                {
                                    LCDFunctions.sendString(ProcessData.GetName(uint.Parse(ProcessData.activeIDforDisplay[1])));
                                }


                                LCDFunctions.gotoLCD(2, 2);
                                LCDFunctions.sendString(ProcessData.activeIDforDisplay[2].ToString());
                                LCDFunctions.gotoLCD(6, 2);
                                if (ProcessData.activeIDforDisplay[2] != "" && ProcessData.activeIDforDisplay[2] != null)
                                {
                                    LCDFunctions.sendString(ProcessData.GetName(uint.Parse(ProcessData.activeIDforDisplay[2])));
                                }


                                LCDFunctions.gotoLCD(2, 3);
                                LCDFunctions.sendString(ProcessData.activeIDforDisplay[3].ToString());
                                LCDFunctions.gotoLCD(6, 3);
                                if (ProcessData.activeIDforDisplay[3] != "" && ProcessData.activeIDforDisplay[3] != null)
                                {
                                    LCDFunctions.sendString(ProcessData.GetName(uint.Parse(ProcessData.activeIDforDisplay[3])));
                                }

                                Thread.Sleep(1);
                            }
                        }

                        break;
                    }*/
                case 10510:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString("Now Disconnect Power");
                        Thread.Sleep(1);
                        ActiveFrameInfo.isYN = false;
                        break;
                    }
                //Now Base Address 200 for menu 2
                case 201:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.gotoLCD(0, 0);
                        LCDFunctions.sendString("FAT/SNF-Frml C/B:1/2");
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString("FAT/CLR-Mnul C/B:3/4");
                        LCDFunctions.gotoLCD(0, 2);
                        LCDFunctions.sendString("FAT/SNF-Mnul C/B:5/6");
                        LCDFunctions.gotoLCD(0, 3);
                        LCDFunctions.sendString("FAT-Only Mnl C/B:7/8");
                        Thread.Sleep(10);

                        break;
                    }

                case 202:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(0, 0);
                        LCDFunctions.sendString("Bonus Seting C/B:1/2");
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString("Selct Rate Chart:3/4");
                        LCDFunctions.gotoLCD(0, 2);
                        LCDFunctions.sendString("Min/Mx Cutof C/B:5/6");
                        LCDFunctions.gotoLCD(0, 3);
                        LCDFunctions.sendString("Set AB/Deduct :  7/8");
                        Thread.Sleep(1);

                        break;
                    }
                case 203:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString("COW : 1");
                        LCDFunctions.gotoLCD(0, 2);
                        LCDFunctions.sendString("BUF : 2");
                        Thread.Sleep(1);

                        break;
                    }
                case 90110:
                case 20301: {
                    LCDFunctions.clearLCD();
                    LCDFunctions.gotoLCD(5, 2);
                    LCDFunctions.sendString("Printing....");
                    Thread.Sleep(1);
                    break;
                }
                case 20302:
                    {
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(5, 2);
                        LCDFunctions.sendString("Printing....");
                        Thread.Sleep(1);
                        break;
                    }
                case 100401:
                case 204:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Add/Mod/Rem Password");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Password : ");
                            LCDFunctions.gotoLCD(0, 3);
                            LCDFunctions.sendString("Leave Blank 2 Remove");
                            Thread.Sleep(1);
                            FrameInformationDatabase(204);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 65)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.passwordGlobal);
                            }
                        }
                        break;
                    }

                case 301: {
                    Thread.Sleep(1);
                    LCDFunctions.clearLCD();
                    LCDFunctions.gotoLCD(2, 1);
                    LCDFunctions.sendString("Time/Date Updated");
                    LCDFunctions.CursorON_OFF(false);
                    Thread.Sleep(1);
                    break;
                
                }

                    //Base 400 for menu 4
                case 401:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.CursorON_OFF(true);
                        LCDFunctions.gotoLCD(2, 1);
                        LCDFunctions.sendString("Change Password?");
                        LCDFunctions.gotoLCD(7, 2);
                        LCDFunctions.sendString("(Y/N)");
                        
                        Thread.Sleep(1);
                        FrameInformationDatabase(401);
                        ActiveFrameInfo.isYN = true;
                        break;
                    }
                case 402:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Line No.1");
                            LCDFunctions.gotoLCD(0, 1);
                            Thread.Sleep(1);
                            FrameInformationDatabase(402);
                        }
                        else if (LCDrefreshFlag) {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 16)
                                LCDFunctions.sendString(ActiveFrameInfo.screenTittle);
                        }

                        break;
                    }
                case 403:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Machine: Lacto Scan");
                            Thread.Sleep(1);
                            FrameInformationDatabase(403);
                        }
                        else if (LCDrefreshFlag) {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 17) {
                                LCDFunctions.sendString(MachineCollection[ActiveFrameInfo.ActiveMachine]);
                            }
                        }

                        break;
                    }
                case 404:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Printer: Printer 1");
                            Thread.Sleep(1);
                            FrameInformationDatabase(404);
                        }
                        else if (LCDrefreshFlag) {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 18) {
                                LCDFunctions.sendString(PrinterCollection[ActiveFrameInfo.ActivePrinter]);
                            }
                        }

                        break;
                    }

                case 405:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(false);
                            LCDFunctions.gotoLCD(4, 0);
                            LCDFunctions.sendString("Weigh  Scale");
                            LCDFunctions.gotoLCD(4, 2);
                            LCDFunctions.sendString("<Machine 1>");
                            Thread.Sleep(1);
                            FrameInformationDatabase(405);
                        }
                        else if (LCDrefreshFlag) {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 19) {
                                LCDFunctions.sendString(WeighCollection[ActiveFrameInfo.ActiveWeigh]);
                            }
                        }
                        break;
                    }
                case 406:
                    {
                        Thread.Sleep(500);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(0, 0);
                        LCDFunctions.sendString("Change Manual       ");
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString("Collection ? (Y/N)  ");
                        LCDFunctions.gotoLCD(0, 2);
                        LCDFunctions.sendString("                    ");
                        LCDFunctions.gotoLCD(0, 3);
                        LCDFunctions.sendString("                    ");
                        ActiveFrameInfo.isYN = true;
                        Thread.Sleep(500);

                        break;
                    }
                case 501: {
                    if (!LCDrefreshFlag) {
                        LCDFunctions.clearLCD();
                        
                        if (ProcessData.IsSpaceAvailable(ActiveFrameInfo.FrameData_UserID))
                        {
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString(ActiveFrameInfo.FrameData_UserID.ToString());

                            LCDFunctions.gotoLCD(4, 0);
                            LCDFunctions.sendString(ActiveFrameInfo.FrameData_Name);

                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("FAT:");

                            LCDFunctions.gotoLCD(4, 1);
                            LCDFunctions.sendString("00.00");

                            LCDFunctions.gotoLCD(10, 1);
                            LCDFunctions.sendString("SNF:");

                            LCDFunctions.gotoLCD(14, 1);
                            LCDFunctions.sendString("00.00");

                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("Qt:");

                            LCDFunctions.gotoLCD(3, 2);
                            LCDFunctions.sendString("0000.00");

                            LCDFunctions.gotoLCD(11, 2);
                            LCDFunctions.sendString("Rt:");

                            LCDFunctions.gotoLCD(14, 2);
                            LCDFunctions.sendString("000.00");

                            LCDFunctions.gotoLCD(0, 3);
                            LCDFunctions.sendString("Amount:");

                            LCDFunctions.gotoLCD(7, 3);
                            LCDFunctions.sendString("00000.00");

                            LCDFunctions.gotoLCD(16, 3);
                            LCDFunctions.sendString("COW");

                            FrameInformationDatabase(501);
                        }
                        else {
                            LCDFunctions.gotoLCD(3, 1);
                            LCDFunctions.sendString("User not Exist");
                        }

                    }
                    else if (LCDrefreshFlag) {

                        LCDFunctions.gotoLCD(14, 2);
                        LCDFunctions.sendString("      ");

                        LCDFunctions.gotoLCD(14, 2);
                        LCDFunctions.sendString(ActiveFrameInfo.float5_0data);

                        LCDFunctions.gotoLCD(7, 3);
                        LCDFunctions.sendString("        ");

                        LCDFunctions.gotoLCD(7, 3);
                        LCDFunctions.sendString(ActiveFrameInfo.float7_0data);

                        LCDFunctions.gotoLCD(16, 3);
                        LCDFunctions.sendString(COWorBUF[ActiveFrameInfo.whichShift]);


                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                        if (ActiveFrameInfo.WhichDataInFrame == 23) {
                            LCDFunctions.sendString(ActiveFrameInfo.float4_0data);
                        }
                        else if (ActiveFrameInfo.WhichDataInFrame == 27) {
                            LCDFunctions.sendString(ActiveFrameInfo.float4_1data);
                        }
                        else if (ActiveFrameInfo.WhichDataInFrame == 30)
                        {
                            LCDFunctions.sendString(ActiveFrameInfo.float6_0data);
                        }


                    
                    }
                    break;
                }
                case 502:
                    {
                        if (!LCDrefreshFlag)
                        {
                            LCDFunctions.clearLCD();

                            if (ProcessData.IsSpaceAvailable(ActiveFrameInfo.FrameData_UserID))
                            {
                                FrameInformationDatabase(502);

                                LCDFunctions.gotoLCD(0, 0);
                                LCDFunctions.sendString(ActiveFrameInfo.FrameData_UserID.ToString());

                                LCDFunctions.gotoLCD(4, 0);
                                LCDFunctions.sendString(ActiveFrameInfo.FrameData_Name);

                                LCDFunctions.gotoLCD(0, 1);
                                LCDFunctions.sendString("FAT:");

                                LCDFunctions.gotoLCD(4, 1);
                                LCDFunctions.sendString(ActiveFrameInfo.float4_0data);

                                LCDFunctions.gotoLCD(10, 1);
                                LCDFunctions.sendString("SNF:");

                                LCDFunctions.gotoLCD(14, 1);
                                LCDFunctions.sendString(ActiveFrameInfo.float4_1data);

                                LCDFunctions.gotoLCD(0, 2);
                                LCDFunctions.sendString("Qt:");

                                LCDFunctions.gotoLCD(3, 2);
                                LCDFunctions.sendString(ActiveFrameInfo.float6_0data);

                                LCDFunctions.gotoLCD(11, 2);
                                LCDFunctions.sendString("Rt:");

                                LCDFunctions.gotoLCD(14, 2);
                                LCDFunctions.sendString(ActiveFrameInfo.float5_0data);

                                LCDFunctions.gotoLCD(0, 3);
                                LCDFunctions.sendString("Amount:");

                                LCDFunctions.gotoLCD(7, 3);
                                LCDFunctions.sendString(ActiveFrameInfo.float7_0data);

                                LCDFunctions.gotoLCD(16, 3);
                                LCDFunctions.sendString("COW");

                                
                            }
                            else
                            {
                                LCDFunctions.gotoLCD(3, 1);
                                LCDFunctions.sendString("User not Exist");
                            }

                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(4, 1);
                            LCDFunctions.sendString("     ");

                            LCDFunctions.gotoLCD(4, 1);
                            LCDFunctions.sendString(ActiveFrameInfo.float4_0data);

                            LCDFunctions.gotoLCD(14, 1);
                            LCDFunctions.sendString("     ");

                            LCDFunctions.gotoLCD(14, 1);
                            LCDFunctions.sendString(ActiveFrameInfo.float4_1data);

                            LCDFunctions.gotoLCD(3, 2);
                            LCDFunctions.sendString("       ");

                            LCDFunctions.gotoLCD(3, 2);
                            LCDFunctions.sendString(ActiveFrameInfo.float6_0data);

                            LCDFunctions.gotoLCD(14, 2);
                            LCDFunctions.sendString("      ");

                            LCDFunctions.gotoLCD(14, 2);
                            LCDFunctions.sendString(ActiveFrameInfo.float5_0data);

                            LCDFunctions.gotoLCD(7, 3);
                            LCDFunctions.sendString("        ");

                            LCDFunctions.gotoLCD(7, 3);
                            LCDFunctions.sendString(ActiveFrameInfo.float7_0data);

                            LCDFunctions.gotoLCD(16, 3);
                            LCDFunctions.sendString(COWorBUF[ActiveFrameInfo.whichShift]);

                            /*
                            LCDFunctions.gotoLCD(4, 1);
                            LCDFunctions.sendString("     ");

                            LCDFunctions.gotoLCD(4, 1);
                            LCDFunctions.sendString(ActiveFrameInfo.float4_0data);

                            LCDFunctions.gotoLCD(14, 1);
                            LCDFunctions.sendString("     ");

                            LCDFunctions.gotoLCD(14, 1);
                            LCDFunctions.sendString(ActiveFrameInfo.float4_1data);

                            LCDFunctions.gotoLCD(14, 2);
                            LCDFunctions.sendString("      ");

                            LCDFunctions.gotoLCD(14, 2);
                            LCDFunctions.sendString(ActiveFrameInfo.float6_0data);
                             */
                        }
                        break;
                    }
                case 503:
                    {
                        break;
                    }
                case 504: 
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.CursorON_OFF(false);

                        LCDFunctions.gotoLCD(2, 0);
                        LCDFunctions.sendString(ActiveFrameInfo.FrameData_UserID.ToString());
                        LCDFunctions.gotoLCD(5, 0);
                        LCDFunctions.sendString(ProcessData.GetName(ActiveFrameInfo.FrameData_UserID));
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString("--------------------");
                        LCDFunctions.gotoLCD(0, 2);
                        LCDFunctions.sendString("Add/Delete    :  1/2");
                        LCDFunctions.gotoLCD(0, 3);
                        LCDFunctions.sendString("Modifiy/Print :  3/4");
                        LCDrefreshFlag = false;
                        break;
                    }
                case 505:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString("Go Manual(Y)/Auto(N)");
                        Thread.Sleep(1);
                        break;
                    }
                case 50110:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(2, 1);
                        LCDFunctions.sendString("Collection Saved");
                        Thread.Sleep(1);

                        break;
                    }
                case 601:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString("   Printing...");
                       
                        Thread.Sleep(1);

                        break;
                    }
                case 701: {
                    Thread.Sleep(1);
                    LCDFunctions.clearLCD();
                    LCDFunctions.CursorON_OFF(false);
                    LCDFunctions.gotoLCD(4, 1);
                    LCDFunctions.sendString("Printing....");
                    Thread.Sleep(1);

                    break;
                }
                    //Base 800 menu 8
                case 801:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            FrameInformationDatabase(801);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Serial No. " + ActiveFrameInfo.integer4_0);
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Quanity : 0000.00");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("Category : <COW>");
                            LCDFunctions.gotoLCD(0, 3);
                            LCDFunctions.sendString("Amount : 0");
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            Thread.Sleep(1);
                            
                        }
                        else if (LCDrefreshFlag) {
                            float amount = 0;
                            try
                            {
                                if (ActiveFrameInfo.toggleSwitch == 0)
                                {

                                    amount = float.Parse(DataKeeper.GetValue("LOCAL_COW_RATE")) * float.Parse(ActiveFrameInfo.float6_0data);
                                }
                                else if (ActiveFrameInfo.toggleSwitch == 1)
                                    amount = float.Parse(DataKeeper.GetValue("LOCAL_BUF_RATE")) * float.Parse(ActiveFrameInfo.float6_0data);
                            }
                            catch (Exception ex) { }

                            ActiveFrameInfo.float7_0data = amount.ToString();
                            LCDFunctions.printStringWithCmd(ActiveFrameInfo.float7_0data, 9, 3);

                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 30) {
                                //Quantity
                                LCDFunctions.sendString(ActiveFrameInfo.float6_0data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 15) {
                                LCDFunctions.sendString(COWorBUF[ActiveFrameInfo.toggleSwitch].ToString());
                            }
                            
                        }

                        break;
                    }

                case 802:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            FrameInformationDatabase(802);
                            LCDFunctions.CursorON_OFF(true);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("S.No. " + ActiveFrameInfo.integer4_0);
                            LCDFunctions.gotoLCD(11, 0);
                            LCDFunctions.sendString("TN:" + ActiveFrameInfo.VechileNo);
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Quanity : 0000.00");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("Category : <COW>");
                            LCDFunctions.gotoLCD(0, 3);
                            LCDFunctions.sendString("Amount : 0");
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            Thread.Sleep(1);
                            
                        }
                        else if (LCDrefreshFlag)
                        {
                            float amount = 0;
                            try
                            {
                                if (ActiveFrameInfo.toggleSwitch == 0)
                                {

                                    amount = float.Parse(DataKeeper.GetValue("TRUCK_COW_RATE")) * float.Parse(ActiveFrameInfo.float6_0data);
                                }
                                else if (ActiveFrameInfo.toggleSwitch == 1)
                                    amount = float.Parse(DataKeeper.GetValue("TRUCK_BUF_RATE")) * float.Parse(ActiveFrameInfo.float6_0data);
                            }
                            catch (Exception ex) { }

                            ActiveFrameInfo.float7_0data = amount.ToString();
                            LCDFunctions.printStringWithCmd(ActiveFrameInfo.float7_0data, 9, 3);

                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 30)
                            {
                                //Quantity
                                LCDFunctions.sendString(ActiveFrameInfo.float6_0data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 15)
                            {
                                //Category
                                LCDFunctions.sendString(COWorBUF[ActiveFrameInfo.toggleSwitch].ToString());
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 28)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.VechileNo);
                            }

                        }

                        break;
                    }
                case 804:
                case 803:
                    {
                        
                        if (!LCDrefreshFlag)
                        {
                            string Truck = "";
                            if (FrameBase == 803)
                                Truck = "Local ";
                            else if (FrameBase == 804)
                                Truck = "Truck ";
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Delete " + Truck);


                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("Date:");

                            LCDFunctions.gotoLCD(6, 2);
                            LCDFunctions.sendString("00");

                            LCDFunctions.gotoLCD(8, 2);
                            LCDFunctions.sendString("/");

                            LCDFunctions.gotoLCD(9, 2);
                            LCDFunctions.sendString("00");

                            LCDFunctions.gotoLCD(11, 2);
                            LCDFunctions.sendString("/");

                            LCDFunctions.gotoLCD(12, 2);
                            LCDFunctions.sendString("00");

                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Serial:");

                            Thread.Sleep(1);
                            FrameInformationDatabase(803);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(6, 2);
                            LCDFunctions.sendString(ActiveFrameInfo.day);
                            LCDFunctions.gotoLCD(9, 2);
                            LCDFunctions.sendString(ActiveFrameInfo.month);
                            LCDFunctions.gotoLCD(12, 2);
                            LCDFunctions.sendString(ActiveFrameInfo.year);

                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 4)
                                LCDFunctions.sendString(ActiveFrameInfo.day);
                            else if (ActiveFrameInfo.WhichDataInFrame == 5)
                                LCDFunctions.sendString(ActiveFrameInfo.month);
                            else if (ActiveFrameInfo.WhichDataInFrame == 6)
                                LCDFunctions.sendString(ActiveFrameInfo.year);
                            else if (ActiveFrameInfo.WhichDataInFrame == 49)
                            {
                                LCDFunctions.sendString("      ");
                                LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                                LCDFunctions.sendString(ActiveFrameInfo.integer5_0);
                            }

                        }

                        break;
                    }
                case 806:
                case 805:
                    {

                        if (!LCDrefreshFlag)
                        {
                            string Truck = "";
                            if (FrameBase == 805)
                                Truck = "Local ";
                            else if (FrameBase == 806)
                                Truck = "Truck ";
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Print " + Truck);


                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Date:");

                            LCDFunctions.gotoLCD(6, 1);
                            LCDFunctions.sendString("00");

                            LCDFunctions.gotoLCD(8, 1);
                            LCDFunctions.sendString("/");

                            LCDFunctions.gotoLCD(9, 1);
                            LCDFunctions.sendString("00");

                            LCDFunctions.gotoLCD(11, 1);
                            LCDFunctions.sendString("/");

                            LCDFunctions.gotoLCD(12, 1);
                            LCDFunctions.sendString("00");

                            

                            Thread.Sleep(1);
                            FrameInformationDatabase(805);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(6, 1);
                            LCDFunctions.sendString(ActiveFrameInfo.day);
                            LCDFunctions.gotoLCD(9, 1);
                            LCDFunctions.sendString(ActiveFrameInfo.month);
                            LCDFunctions.gotoLCD(12, 1);
                            LCDFunctions.sendString(ActiveFrameInfo.year);

                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 4)
                                LCDFunctions.sendString(ActiveFrameInfo.day);
                            else if (ActiveFrameInfo.WhichDataInFrame == 5)
                                LCDFunctions.sendString(ActiveFrameInfo.month);
                            else if (ActiveFrameInfo.WhichDataInFrame == 6)
                                LCDFunctions.sendString(ActiveFrameInfo.year);
                        }

                        break;
                    }
                case 807:
                    {
                        if (true)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(false);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Add Local Rate     1");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Add Truck Rate     2");
                            Thread.Sleep(1);
                        }
                        break;
                    }
                case 808:
                    {
                        if (true)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(false);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("(ALL) TODAY(L/T) 1/2");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("(ALL) DATE (L/T) 3/4");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("(LAST 10)  (L/T) 5/6");
                            Thread.Sleep(1);
                        }
                        break;
                    }


                case 80701:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                           
                            LCDFunctions.CursorON_OFF(true);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Enter Rates:");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Kg COW Rate: " + DataKeeper.GetValue("LOCAL_COW_RATE"));
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("Kg BUF Rate: " + DataKeeper.GetValue("LOCAL_BUF_RATE"));
                            FrameInformationDatabase(80701);
                            Thread.Sleep(1);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 29)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float5_0data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 32)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float5_1data);
                            }

                        }
                        break;
                    }
                case 80702:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();

                            LCDFunctions.CursorON_OFF(true);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Enter Rates:");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Kg COW Rate: " + DataKeeper.GetValue("TRUCK_COW_RATE"));
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("Kg BUF Rate: " + DataKeeper.GetValue("TRUCK_BUF_RATE"));
                            FrameInformationDatabase(80702);
                            Thread.Sleep(1);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 29)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float5_0data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 32)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float5_1data);
                            }

                        }
                        break;
                    }
                case 10410:
                case 80805:
                case 80806:
                case 8080310:
                case 8080410:
                case 80802:
                case 80801:
                    {
                        //Local Search
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            FrameInformationDatabase(80801);
                            LCDFunctions.CursorON_OFF(false);

                            LCDFunctions.gotoLCD(1, 0);
                            LCDFunctions.sendString(ActiveFrameInfo.SearchDataDisplay[0].col1);
                            LCDFunctions.gotoLCD(5, 0);
                            LCDFunctions.sendString(ActiveFrameInfo.SearchDataDisplay[0].col2);
                            LCDFunctions.gotoLCD(13, 0);
                            LCDFunctions.sendString(ActiveFrameInfo.SearchDataDisplay[0].col3);


                            LCDFunctions.gotoLCD(1, 1);
                            LCDFunctions.sendString(ActiveFrameInfo.SearchDataDisplay[1].col1);
                            LCDFunctions.gotoLCD(5, 1);
                            LCDFunctions.sendString(ActiveFrameInfo.SearchDataDisplay[1].col2);
                            LCDFunctions.gotoLCD(13, 1);
                            LCDFunctions.sendString(ActiveFrameInfo.SearchDataDisplay[1].col3);

                            LCDFunctions.gotoLCD(1, 2);
                            LCDFunctions.sendString(ActiveFrameInfo.SearchDataDisplay[2].col1);
                            LCDFunctions.gotoLCD(5, 2);
                            LCDFunctions.sendString(ActiveFrameInfo.SearchDataDisplay[2].col2);
                            LCDFunctions.gotoLCD(13, 2);
                            LCDFunctions.sendString(ActiveFrameInfo.SearchDataDisplay[2].col3);

                            LCDFunctions.gotoLCD(1, 3);
                            LCDFunctions.sendString(ActiveFrameInfo.SearchDataDisplay[3].col1);
                            LCDFunctions.gotoLCD(5, 3);
                            LCDFunctions.sendString(ActiveFrameInfo.SearchDataDisplay[3].col2);
                            LCDFunctions.gotoLCD(13, 3);
                            LCDFunctions.sendString(ActiveFrameInfo.SearchDataDisplay[3].col3);
                            
                            Thread.Sleep(1);
                        }
                        else if (LCDrefreshFlag)
                        {
                            //LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 66)
                            {
                                LCDFunctions.clearLCD();
                                LCDFunctions.gotoLCD(1, 0);
                                LCDFunctions.sendString(ActiveFrameInfo.SearchDataDisplay[0].col1);
                                LCDFunctions.gotoLCD(5, 0);
                                LCDFunctions.sendString(ActiveFrameInfo.SearchDataDisplay[0].col2);
                                LCDFunctions.gotoLCD(13, 0);
                                LCDFunctions.sendString(ActiveFrameInfo.SearchDataDisplay[0].col3);


                                LCDFunctions.gotoLCD(1, 1);
                                LCDFunctions.sendString(ActiveFrameInfo.SearchDataDisplay[1].col1);
                                LCDFunctions.gotoLCD(5, 1);
                                LCDFunctions.sendString(ActiveFrameInfo.SearchDataDisplay[1].col2);
                                LCDFunctions.gotoLCD(13, 1);
                                LCDFunctions.sendString(ActiveFrameInfo.SearchDataDisplay[1].col3);

                                LCDFunctions.gotoLCD(1, 2);
                                LCDFunctions.sendString(ActiveFrameInfo.SearchDataDisplay[2].col1);
                                LCDFunctions.gotoLCD(5, 2);
                                LCDFunctions.sendString(ActiveFrameInfo.SearchDataDisplay[2].col2);
                                LCDFunctions.gotoLCD(13, 2);
                                LCDFunctions.sendString(ActiveFrameInfo.SearchDataDisplay[2].col3);

                                LCDFunctions.gotoLCD(1, 3);
                                LCDFunctions.sendString(ActiveFrameInfo.SearchDataDisplay[3].col1);
                                LCDFunctions.gotoLCD(5, 3);
                                LCDFunctions.sendString(ActiveFrameInfo.SearchDataDisplay[3].col2);
                                LCDFunctions.gotoLCD(13, 3);
                                LCDFunctions.sendString(ActiveFrameInfo.SearchDataDisplay[3].col3);
                            }
                        }
                        break;
                    }
                case 80804:
                case 80803:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Search Date");

                            LCDFunctions.gotoLCD(5, 2);
                            LCDFunctions.sendString("DD");

                            LCDFunctions.gotoLCD(7, 2);
                            LCDFunctions.sendString("/");

                            LCDFunctions.gotoLCD(8, 2);
                            LCDFunctions.sendString("MM");

                            LCDFunctions.gotoLCD(10, 2);
                            LCDFunctions.sendString("/");

                            LCDFunctions.gotoLCD(11, 2);
                            LCDFunctions.sendString("YY");

                            LCDFunctions.gotoLCD(5, 1);
                            LCDFunctions.sendString("00");

                            LCDFunctions.gotoLCD(7, 1);
                            LCDFunctions.sendString("/");

                            LCDFunctions.gotoLCD(8, 1);
                            LCDFunctions.sendString("00");

                            LCDFunctions.gotoLCD(10, 1);
                            LCDFunctions.sendString("/");

                            LCDFunctions.gotoLCD(11, 1);
                            LCDFunctions.sendString("00");

                            Thread.Sleep(1);
                            FrameInformationDatabase(80803);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(5, 1);
                            LCDFunctions.sendString(ActiveFrameInfo.day);
                            LCDFunctions.gotoLCD(8, 1);
                            LCDFunctions.sendString(ActiveFrameInfo.month);
                            LCDFunctions.gotoLCD(11, 1);
                            LCDFunctions.sendString(ActiveFrameInfo.year);

                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 4)
                                LCDFunctions.sendString(ActiveFrameInfo.day);
                            else if (ActiveFrameInfo.WhichDataInFrame == 5)
                                LCDFunctions.sendString(ActiveFrameInfo.month);
                            else if (ActiveFrameInfo.WhichDataInFrame == 6)
                                LCDFunctions.sendString(ActiveFrameInfo.year);

                        }

                        break;
                    }
                case 901:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(false);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Member Ledgery");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Fr Date:00/00/00 (M)");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("To Date:00/00/00 (M)");
                            LCDFunctions.gotoLCD(8, 1);
                            LCDFunctions.CursorON_OFF(true);
                            FrameInformationDatabase(901);
                            Thread.Sleep(1);
                        }
                        else if (LCDrefreshFlag) {
                            LCDFunctions.gotoLCD(8, 1);
                            LCDFunctions.sendString(ActiveFrameInfo.day);
                            LCDFunctions.gotoLCD(11, 1);
                            LCDFunctions.sendString(ActiveFrameInfo.month);
                            LCDFunctions.gotoLCD(14, 1);
                            LCDFunctions.sendString(ActiveFrameInfo.year);
                            LCDFunctions.gotoLCD(18, 1);
                            LCDFunctions.sendString(Shift[ActiveFrameInfo.whichShift].ToString());

                            LCDFunctions.gotoLCD(8, 2);
                            LCDFunctions.sendString(ActiveFrameInfo.day_1);
                            LCDFunctions.gotoLCD(11, 2);
                            LCDFunctions.sendString(ActiveFrameInfo.month_1);
                            LCDFunctions.gotoLCD(14, 2);
                            LCDFunctions.sendString(ActiveFrameInfo.year_1);
                            LCDFunctions.gotoLCD(18, 2);
                            LCDFunctions.sendString(Shift[ActiveFrameInfo.whichShift_1].ToString());



                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 4)
                                LCDFunctions.sendString(ActiveFrameInfo.day);
                            else if (ActiveFrameInfo.WhichDataInFrame == 5)
                                LCDFunctions.sendString(ActiveFrameInfo.month);
                            else if (ActiveFrameInfo.WhichDataInFrame == 6)
                                LCDFunctions.sendString(ActiveFrameInfo.year);
                            else if (ActiveFrameInfo.WhichDataInFrame == 11)
                                LCDFunctions.sendString(Shift[ActiveFrameInfo.whichShift].ToString());
                            else if (ActiveFrameInfo.WhichDataInFrame == 61)
                                LCDFunctions.sendString(ActiveFrameInfo.day_1);
                            else if (ActiveFrameInfo.WhichDataInFrame == 62)
                                LCDFunctions.sendString(ActiveFrameInfo.month_1);
                            else if (ActiveFrameInfo.WhichDataInFrame == 63)
                                LCDFunctions.sendString(ActiveFrameInfo.year_1);
                            else if (ActiveFrameInfo.WhichDataInFrame == 64)
                                LCDFunctions.sendString(Shift[ActiveFrameInfo.whichShift_1].ToString());
                        }
                        break;
                    }

                    //base 1100 menu 11
                case 1101:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Delete Collection:");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Enter Code:");
                            LCDFunctions.gotoLCD(8, 2);
                            LCDFunctions.sendString("000");
                            Thread.Sleep(1);
                            FrameInformationDatabase(1101);
                        }
                        else if(LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            LCDFunctions.sendString("000");
                            LCDFunctions.gotoLCD((ActiveFrameInfo.LCD_X + 3 - ActiveFrameInfo.FrameData_UserID.ToString().Length), ActiveFrameInfo.LCD_Y);
                            LCDFunctions.sendString(ActiveFrameInfo.FrameData_UserID.ToString());
                            LCDFunctions.gotoLCD((ActiveFrameInfo.LCD_X + 2), ActiveFrameInfo.LCD_Y);
                        }

                        break;
                    }

                case 1102:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Enter to Delete.");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Date: 00/00/00");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("Shift: (E)");
                            Thread.Sleep(1);
                            FrameInformationDatabase(1102);
                        }
                        else if (LCDrefreshFlag) {
                            LCDFunctions.gotoLCD(6, 1);
                            LCDFunctions.sendString(ActiveFrameInfo.day);
                            LCDFunctions.gotoLCD(9, 1);
                            LCDFunctions.sendString(ActiveFrameInfo.month);
                            LCDFunctions.gotoLCD(12, 1);
                            LCDFunctions.sendString(ActiveFrameInfo.year);
                            LCDFunctions.gotoLCD(8, 2);
                            LCDFunctions.sendString(Shift[ActiveFrameInfo.whichShift].ToString());

                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X,ActiveFrameInfo.LCD_Y);
                            if(ActiveFrameInfo.WhichDataInFrame == 4)
                                LCDFunctions.sendString(ActiveFrameInfo.day);
                            else if (ActiveFrameInfo.WhichDataInFrame == 5)
                                LCDFunctions.sendString(ActiveFrameInfo.month);
                            else if(ActiveFrameInfo.WhichDataInFrame == 6)
                                LCDFunctions.sendString(ActiveFrameInfo.year);
                            else if (ActiveFrameInfo.WhichDataInFrame == 11)
                                LCDFunctions.sendString(Shift[ActiveFrameInfo.whichShift].ToString());
                        }
                        
                        break;
                    }
                case 1201:
                    {
                        if (true)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(false);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("COM1 Setting       1");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("COM2 Setting       2");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("COM3 Setting       3");
                            Thread.Sleep(1);
                            FrameInformationDatabase(1201);
                        }
                        break;
                    }
                case 1202:
                    {
                        if (true)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(false);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Manual/Automatic   1");
                            
                            Thread.Sleep(1);
                            FrameInformationDatabase(1201);
                        }
                        break;
                    }
                case 1203:
                    {
                        if (true)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(false);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("COM(1/2/3)    1/2/3");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Import/Export     4");
                            Thread.Sleep(1);
                            FrameInformationDatabase(1203);
                        }
                        break;
                    }
                case 1204:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Add/Mod/Rem Password");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Password : ");
                            LCDFunctions.gotoLCD(0, 3);
                            LCDFunctions.sendString("Leave Blank 2 Remove");
                            Thread.Sleep(1);
                            FrameInformationDatabase(1204);
                        }
                        else if (LCDrefreshFlag) {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 65)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.passwordGlobal);
                            }
                        }
                        break;
                    }
                case 120410:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.gotoLCD(1, 1);
                        LCDFunctions.sendString("Saving Changes !");
                        break;
                    }
                case 120101: {
                    if (!LCDrefreshFlag)
                    {
                        Thread.Sleep(1);
                        FrameInformationDatabase(120101);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(0, 0);
                        LCDFunctions.sendString("BR: <"+BaudRateCollection[ActiveFrameInfo.whichBaudRate]+">");
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString("DB: <"+DataBitsCollection[ActiveFrameInfo.whichDataBit]+"> | ");
                        LCDFunctions.gotoLCD(10, 1);
                        LCDFunctions.sendString("SB: <"+StopBitsCollection[ActiveFrameInfo.whichStopBit]+">");
                        LCDFunctions.gotoLCD(0, 2);
                        LCDFunctions.sendString("HS: <"+handShakeCollection[ActiveFrameInfo.whichHandShake]+">");
                        LCDFunctions.gotoLCD(0, 3);
                        LCDFunctions.sendString("Parity: <"+ParityCollection[ActiveFrameInfo.whichParity]+">");
                        Thread.Sleep(1);
                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                    }
                    else if (LCDrefreshFlag) {
                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                        if (ActiveFrameInfo.WhichDataInFrame == 40) {
                            LCDFunctions.sendString(BaudRateCollection[ActiveFrameInfo.whichBaudRate]);
                        }
                        else if (ActiveFrameInfo.WhichDataInFrame == 41) {
                            LCDFunctions.sendString(DataBitsCollection[ActiveFrameInfo.whichDataBit]);
                        }
                        else if (ActiveFrameInfo.WhichDataInFrame == 42)
                        {
                            LCDFunctions.sendString(StopBitsCollection[ActiveFrameInfo.whichStopBit]);
                        }
                        else if (ActiveFrameInfo.WhichDataInFrame == 43)
                        {
                            LCDFunctions.sendString(handShakeCollection[ActiveFrameInfo.whichHandShake]);
                        }
                        else if (ActiveFrameInfo.WhichDataInFrame == 44)
                        {
                            LCDFunctions.sendString(ParityCollection[ActiveFrameInfo.whichParity]);
                        }
                    }
                    break;
                }
                case 120102:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            FrameInformationDatabase(120102);
                            LCDFunctions.clearLCD();
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("BR: <" + BaudRateCollection[ActiveFrameInfo.whichBaudRate] + ">");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("DB: <" + DataBitsCollection[ActiveFrameInfo.whichDataBit] + "> | ");
                            LCDFunctions.gotoLCD(10, 1);
                            LCDFunctions.sendString("SB: <" + StopBitsCollection[ActiveFrameInfo.whichStopBit] + ">");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("HS: <" + handShakeCollection[ActiveFrameInfo.whichHandShake] + ">");
                            LCDFunctions.gotoLCD(0, 3);
                            LCDFunctions.sendString("Parity: <" + ParityCollection[ActiveFrameInfo.whichParity] + ">");
                            Thread.Sleep(1);
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 40)
                            {
                                LCDFunctions.sendString(BaudRateCollection[ActiveFrameInfo.whichBaudRate]);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 41)
                            {
                                LCDFunctions.sendString(DataBitsCollection[ActiveFrameInfo.whichDataBit]);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 42)
                            {
                                LCDFunctions.sendString(StopBitsCollection[ActiveFrameInfo.whichStopBit]);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 43)
                            {
                                LCDFunctions.sendString(handShakeCollection[ActiveFrameInfo.whichHandShake]);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 44)
                            {
                                LCDFunctions.sendString(ParityCollection[ActiveFrameInfo.whichParity]);
                            }
                        }
                        break;
                    }
                case 120103:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            FrameInformationDatabase(120103);
                            LCDFunctions.clearLCD();

                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("BR: <" + BaudRateCollection[ActiveFrameInfo.whichBaudRate] + ">");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("DB: <" + DataBitsCollection[ActiveFrameInfo.whichDataBit] + "> | ");
                            LCDFunctions.gotoLCD(10, 1);
                            LCDFunctions.sendString("SB: <" + StopBitsCollection[ActiveFrameInfo.whichStopBit] + ">");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("HS: <" + handShakeCollection[ActiveFrameInfo.whichHandShake] + ">");
                            LCDFunctions.gotoLCD(0, 3);
                            LCDFunctions.sendString("Parity: <" + ParityCollection[ActiveFrameInfo.whichParity] + ">");
                            Thread.Sleep(1);
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 40)
                            {
                                LCDFunctions.sendString(BaudRateCollection[ActiveFrameInfo.whichBaudRate]);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 41)
                            {
                                LCDFunctions.sendString(DataBitsCollection[ActiveFrameInfo.whichDataBit]);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 42)
                            {
                                LCDFunctions.sendString(StopBitsCollection[ActiveFrameInfo.whichStopBit]);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 43)
                            {
                                LCDFunctions.sendString(handShakeCollection[ActiveFrameInfo.whichHandShake]);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 44)
                            {
                                LCDFunctions.sendString(ParityCollection[ActiveFrameInfo.whichParity]);
                            }
                        }
                        break;
                    }
                case 120201:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Mode: < Manual >");
                            Thread.Sleep(1);
                            FrameInformationDatabase(120201);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 18)
                            {
                                LCDFunctions.sendString(AutoOrManual[ActiveFrameInfo.ActivePrinter]);
                            }
                        }

                        break;
                    }
                case 12020110:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(4, 1);
                        LCDFunctions.sendString("Setting Saved");
                        Thread.Sleep(1);
                        
                        break;
                    }
                case 120301:
                    {
                        if (!LCDrefreshFlag)
                        {

                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(false);
                            FrameInformationDatabase(120301);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Transmitter Device");
                            LCDFunctions.gotoLCD(3, 1);
                            LCDFunctions.sendString("< " + TransmitterDeviceCollection[ActiveFrameInfo.shiftSwitch3_0] + " >");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("Receiver Device");
                            LCDFunctions.gotoLCD(3, 3);
                            LCDFunctions.sendString("< " + ReceiverDeviceCollection[ActiveFrameInfo.shiftSwitch3_1] + " >");
                            LCDFunctions.gotoLCD(0, ActiveFrameInfo.LCD_Y);
                            LCDFunctions.sendString("*");
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            Thread.Sleep(1);
                        }
                        else if (LCDrefreshFlag) {
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString(" ");
                            LCDFunctions.gotoLCD(0, 3);
                            LCDFunctions.sendString(" ");

                            LCDFunctions.gotoLCD(0, ActiveFrameInfo.LCD_Y);
                            LCDFunctions.sendString("*");

                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X,ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 56) {
                                LCDFunctions.sendString(TransmitterDeviceCollection[ActiveFrameInfo.shiftSwitch3_0]);
                                
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 57) {
                                LCDFunctions.sendString(ReceiverDeviceCollection[ActiveFrameInfo.shiftSwitch3_1]);
                            }
                        }

                        break;
                    }

                case 120302:
                    {
                        if (!LCDrefreshFlag)
                        {

                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(false);
                            FrameInformationDatabase(120302);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Transmitter Device");
                            LCDFunctions.gotoLCD(3, 1);
                            LCDFunctions.sendString("< " + TransmitterDeviceCollection[ActiveFrameInfo.shiftSwitch3_0] + " >");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("Receiver Device");
                            LCDFunctions.gotoLCD(3, 3);
                            LCDFunctions.sendString("< " + ReceiverDeviceCollection[ActiveFrameInfo.shiftSwitch3_1] + " >");
                            LCDFunctions.gotoLCD(0, ActiveFrameInfo.LCD_Y);
                            LCDFunctions.sendString("*");
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            Thread.Sleep(1);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString(" ");
                            LCDFunctions.gotoLCD(0, 3);
                            LCDFunctions.sendString(" ");

                            LCDFunctions.gotoLCD(0, ActiveFrameInfo.LCD_Y);
                            LCDFunctions.sendString("*");

                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 56)
                            {
                                LCDFunctions.sendString(TransmitterDeviceCollection[ActiveFrameInfo.shiftSwitch3_0]);

                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 57)
                            {
                                LCDFunctions.sendString(ReceiverDeviceCollection[ActiveFrameInfo.shiftSwitch3_1]);
                            }
                        }

                        break;
                    }
                case 120303:
                    {
                        if (!LCDrefreshFlag)
                        {

                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(false);
                            FrameInformationDatabase(120303);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Transmitter Device");
                            LCDFunctions.gotoLCD(3, 1);
                            LCDFunctions.sendString("< " + TransmitterDeviceCollection[ActiveFrameInfo.shiftSwitch3_0] + " >");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("Receiver Device");
                            LCDFunctions.gotoLCD(3, 3);
                            LCDFunctions.sendString("< " + ReceiverDeviceCollection[ActiveFrameInfo.shiftSwitch3_1] + " >");
                            LCDFunctions.gotoLCD(0, ActiveFrameInfo.LCD_Y);
                            LCDFunctions.sendString("*");
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            Thread.Sleep(1);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString(" ");
                            LCDFunctions.gotoLCD(0, 3);
                            LCDFunctions.sendString(" ");

                            LCDFunctions.gotoLCD(0, ActiveFrameInfo.LCD_Y);
                            LCDFunctions.sendString("*");

                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 56)
                            {
                                LCDFunctions.sendString(TransmitterDeviceCollection[ActiveFrameInfo.shiftSwitch3_0]);

                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 57)
                            {
                                LCDFunctions.sendString(ReceiverDeviceCollection[ActiveFrameInfo.shiftSwitch3_1]);
                            }
                        }

                        break;
                    }
                case 120304:
                    {
                        if (!LCDrefreshFlag)
                        {

                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(false);
                            FrameInformationDatabase(120304);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Export - Import");
                            LCDFunctions.gotoLCD(3, 1);
                            LCDFunctions.sendString("< " + ExportImportCollection[ActiveFrameInfo.toggleSwitch] + " >");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("Data");
                            LCDFunctions.gotoLCD(3, 3);
                            LCDFunctions.sendString("< " + ExportImportDataCollection[ActiveFrameInfo.shiftSwitch3_0] + " >");
                            LCDFunctions.gotoLCD(0, ActiveFrameInfo.LCD_Y);
                            LCDFunctions.sendString("*");
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            Thread.Sleep(1);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString(" ");
                            LCDFunctions.gotoLCD(0, 3);
                            LCDFunctions.sendString(" ");

                            LCDFunctions.gotoLCD(0, ActiveFrameInfo.LCD_Y);
                            LCDFunctions.sendString("*");

                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 15)
                            {
                                LCDFunctions.sendString(ExportImportCollection[ActiveFrameInfo.toggleSwitch]);

                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 56)
                            {
                                LCDFunctions.sendString(ExportImportDataCollection[ActiveFrameInfo.shiftSwitch3_0]);
                            }
                        }

                        break;
                    }
                case 12030410:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(1, 1);
                        LCDFunctions.sendString("Transfering......");
                        Thread.Sleep(1);

                        break;
                    }
                case 12030411:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(1, 1);
                        LCDFunctions.sendString("Pendrive Not Found");
                        Thread.Sleep(1);

                        break;
                    }
                case 10110:
                    {
                        //Input Data From Here
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(0, 0);
                        LCDFunctions.sendString(("ID : " + ActiveFrameInfo.FrameData_UserID.ToString()));
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString("No Created !");
                        LCDFunctions.gotoLCD(0, 2);
                        LCDFunctions.sendString("Want to Create (Y/N)");
                        ActiveFrameInfo.isYN = true;
                        Thread.Sleep(1);
                        break;
                    }

                case 10111:
                    {
                        //Input Data Frame
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(0, 0);
                        LCDFunctions.sendString("Created !");
                        LCDFunctions.gotoLCD(0, 2);
                        LCDFunctions.sendString("Add Details(Y/N)");
                        ActiveFrameInfo.isYN = true;
                        Thread.Sleep(1);
                        break;
                    }
                case 10112:
                    {
                        //Make Entry As Input Frame
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Name:");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Phone:");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("TAB to Switch");
                            LCDFunctions.gotoLCD(0, 3);
                            LCDFunctions.sendString("Enter to Save");
                            Thread.Sleep(1);
                            FrameInformationDatabase(10112);
                        }
                        else {
                            
                            LCDFunctions.gotoLCD(int.Parse(ActiveFrameInfo.ActiveX.ToString()), int.Parse(ActiveFrameInfo.ActiveY.ToString()));
                            if (ActiveFrameInfo.WhichDataInFrame == 2) {
                                LCDFunctions.sendString(ActiveFrameInfo.FrameData_Name);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 3) {
                                LCDFunctions.sendString(ActiveFrameInfo.FrameData_Phone);
                            }
                        }
                        break;
                    }

                case 10113:
                    {
                       
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(0, 2);
                        LCDFunctions.sendString("ADDED !");
                   
                        Thread.Sleep(1);
                        break;
                    }
                case 10120:
                    {
                        //Input Frame
                        //Already Created Want to Delete
                        object[] s = ProcessData.ReadEntry(ActiveFrameInfo.FrameData_UserID);
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(0, 0);
                        LCDFunctions.sendString(ActiveFrameInfo.FrameData_UserID.ToString() + " : " + s[0].ToString());
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString("Already Exist");
                        LCDFunctions.gotoLCD(0, 2);
                        LCDFunctions.sendString("Want to Delete (Y/N)");
                        ActiveFrameInfo.isYN = true;
                        Thread.Sleep(1);
                        break;
                    }
                case 10121:
                    {
                        
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString("Deleted !");
                        Thread.Sleep(1);
                        break;
                    }
                case 10210: {
                    //Make Entry As Input Frame
                    if (!LCDrefreshFlag)
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(0, 0);
                        LCDFunctions.sendString("Name:");
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString("Phone:");
                        LCDFunctions.gotoLCD(0, 2);
                        LCDFunctions.sendString("TAB to Switch");
                        LCDFunctions.gotoLCD(0, 3);
                        LCDFunctions.sendString("Enter to Save");
                        Thread.Sleep(1);
                        FrameInformationDatabase(10210);
                    }
                    else
                    {

                        LCDFunctions.gotoLCD(int.Parse(ActiveFrameInfo.ActiveX.ToString()), int.Parse(ActiveFrameInfo.ActiveY.ToString()));
                        if (ActiveFrameInfo.WhichDataInFrame == 2)
                        {
                            LCDFunctions.sendString(ActiveFrameInfo.FrameData_Name);
                        }
                        else if (ActiveFrameInfo.WhichDataInFrame == 3)
                        {
                            LCDFunctions.sendString(ActiveFrameInfo.FrameData_Phone);
                        }
                    }

                    break;
                }
                case 10211:
                    {

                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(5, 1);
                        LCDFunctions.sendString("Modified !");
                        Thread.Sleep(1);
                        break;
                    }
                case 20101: {
                    if (!LCDrefreshFlag)
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.CursorON_OFF(true);
                        LCDFunctions.gotoLCD(0, 0);
                        LCDFunctions.sendString("Enter Rates:COW");
                        LCDFunctions.gotoLCD(0, 2);
                        LCDFunctions.sendString("Kg FAT Rate: 000.00");
                        LCDFunctions.gotoLCD(0, 3);
                        LCDFunctions.sendString("Kg SNF Rate: 000.00");

                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString("F-" + DataKeeper.GetValue("F2_COW_FATRATEKG") + " | S-" + DataKeeper.GetValue("F2_COW_SNFRATEKG"));

                        FrameInformationDatabase(20101);
                        Thread.Sleep(1);
                    }
                    else if (LCDrefreshFlag) {
                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                        if (ActiveFrameInfo.WhichDataInFrame == 29)
                        {
                            LCDFunctions.sendString(ActiveFrameInfo.float5_0data);
                        }
                        else if (ActiveFrameInfo.WhichDataInFrame == 32)
                        {
                            LCDFunctions.sendString(ActiveFrameInfo.float5_1data);
                        }
                        
                    }
                    break;
                }
                case 2010110: {
                    Thread.Sleep(1);
                    LCDFunctions.clearLCD();
                    LCDFunctions.CursorON_OFF(false);
                    LCDFunctions.gotoLCD(5, 1);
                    LCDFunctions.sendString("Saved !");
                    Thread.Sleep(1);
                    break;
                }

                case 20102:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Enter Rates:BUF");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("Kg FAT Rate: 000.00");
                            LCDFunctions.gotoLCD(0, 3);
                            LCDFunctions.sendString("Kg SNF Rate: 000.00");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("F-" + DataKeeper.GetValue("F2_BUF_FATRATEKG") + " | S-" + DataKeeper.GetValue("F2_BUF_SNFRATEKG"));
                            FrameInformationDatabase(20102);
                            Thread.Sleep(1);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 29)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float5_0data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 32)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float5_1data);
                            }

                        }
                        break;
                    }
                case 2010210:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.gotoLCD(5, 1);
                        LCDFunctions.sendString("Saved !");
                        Thread.Sleep(1);
                        break;
                    }
                case 20103:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("FAT-CLR Rate:COW");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("FAT: 0.0 to 00.0");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("CLR: 00  to 00");
                            FrameInformationDatabase(20103);
                            Thread.Sleep(1);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 20)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float1_0data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 22)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float3_0data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 46)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.integer2_0);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 51)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.integer2_1);
                            }

                        }
                        break;
                    }
                case 2010310:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            FrameInformationDatabase(2010310);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("FAT-CLR Rate:COW");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("FAT: " + TempCalculationVariable.FATmin.ToString());
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("CLR: " + TempCalculationVariable.CLRmin.ToString());
                            LCDFunctions.gotoLCD(5, 3);
                            LCDFunctions.sendString("00.00");
                            LCDFunctions.gotoLCD(5, 3);

                            Thread.Sleep(1);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(5, 1);
                            LCDFunctions.sendString("    ");
                            LCDFunctions.gotoLCD(5, 1);
                            LCDFunctions.sendString(TempCalculationVariable.FATmin.ToString());

                            LCDFunctions.gotoLCD(5, 2);
                            LCDFunctions.sendString("    ");
                            LCDFunctions.gotoLCD(5, 2);
                            LCDFunctions.sendString(TempCalculationVariable.CLRmin.ToString());


                            LCDFunctions.gotoLCD(5, 3);
                            LCDFunctions.sendString(ActiveFrameInfo.float4_0data);
                            if (ActiveFrameInfo.float4_0data.Length == 5)
                            {
                                LCDFunctions.gotoLCD(5, 3);
                            }

                        }
                        break;
                    }

                case 20104:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("FAT-CLR Rate:BUF");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("FAT: 0.0 to 00.0");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("CLR: 00  to 00");
                            FrameInformationDatabase(20104);
                            Thread.Sleep(1);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 20)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float1_0data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 22)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float3_0data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 46)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.integer2_0);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 51)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.integer2_1);
                            }

                        }
                        break;
                    }
                case 2010410:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            FrameInformationDatabase(2010410);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("FAT-CLR Rate:BUF");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("FAT: " + TempCalculationVariable.FATmin.ToString());
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("CLR: " + TempCalculationVariable.CLRmin.ToString());
                            LCDFunctions.gotoLCD(5, 3);
                            LCDFunctions.sendString("00.00");
                            LCDFunctions.gotoLCD(5, 3);

                            Thread.Sleep(1);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(5, 1);
                            LCDFunctions.sendString("    ");
                            LCDFunctions.gotoLCD(5, 1);
                            LCDFunctions.sendString(TempCalculationVariable.FATmin.ToString());

                            LCDFunctions.gotoLCD(5, 2);
                            LCDFunctions.sendString("    ");
                            LCDFunctions.gotoLCD(5, 2);
                            LCDFunctions.sendString(TempCalculationVariable.CLRmin.ToString());


                            LCDFunctions.gotoLCD(5, 3);
                            LCDFunctions.sendString(ActiveFrameInfo.float4_0data);
                            if (ActiveFrameInfo.float4_0data.Length == 5)
                            {
                                LCDFunctions.gotoLCD(5, 3);
                            }

                        }
                        break;
                    }   
                case 20105:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("FAT-SNF Rate:COW");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("FAT: 0.0 to 00.0");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("SNF: 0.0 to 00.0");
                            FrameInformationDatabase(20105);
                            Thread.Sleep(1);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 20)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float1_0data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 22)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float3_0data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 24)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float1_1data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 26)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float3_1data);
                            }

                        }
                        break;
                    }
                case 2010510:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            FrameInformationDatabase(2010510);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("FAT-SNF Rate:COW");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("FAT: " + TempCalculationVariable.FATmin.ToString());
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("SNF: " + TempCalculationVariable.SNFmin.ToString());
                            LCDFunctions.gotoLCD(5, 3);
                            LCDFunctions.sendString("00.00");
                            LCDFunctions.gotoLCD(14, 3);
                            LCDFunctions.sendString(ExcelFileHandler.readCellRateChart(TempCalculationVariable.FATmin.ToString(),TempCalculationVariable.SNFmin.ToString(),1));
                            LCDFunctions.gotoLCD(5, 3);
                            
                            Thread.Sleep(1);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(5, 1);
                            LCDFunctions.sendString("    ");
                            LCDFunctions.gotoLCD(5, 1);
                            LCDFunctions.sendString(TempCalculationVariable.FATmin.ToString());

                            LCDFunctions.gotoLCD(5, 2);
                            LCDFunctions.sendString("    ");
                            LCDFunctions.gotoLCD(5, 2);
                            LCDFunctions.sendString(TempCalculationVariable.SNFmin.ToString());

                            LCDFunctions.gotoLCD(14, 3);
                            LCDFunctions.sendString("     ");
                            LCDFunctions.gotoLCD(14, 3);
                            LCDFunctions.sendString(ExcelFileHandler.readCellRateChart(TempCalculationVariable.FATmin.ToString(), TempCalculationVariable.SNFmin.ToString(), 1));

                            
                            LCDFunctions.gotoLCD(5, 3);
                            LCDFunctions.sendString(ActiveFrameInfo.float4_0data);
                            if (ActiveFrameInfo.float4_0data.Length == 5) {
                                LCDFunctions.gotoLCD(5, 3);
                            }

                        }
                        break;
                    }
                case 2010511:
                    {
                        
                        LCDFunctions.clearLCD();
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.gotoLCD(0, 2);
                        LCDFunctions.sendString("Invalid Input !");
                        break;
                    }
                case 20106:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("FAT-SNF Rate:BUF");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("FAT: 0.0 to 00.0");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("SNF: 0.0 to 00.0");
                            FrameInformationDatabase(20106);
                            Thread.Sleep(1);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 20)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float1_0data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 22)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float3_0data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 24)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float1_1data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 26)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float3_1data);
                            }

                        }
                        break;
                    }
                case 2010610:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            FrameInformationDatabase(2010610);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("FAT-SNF Rate:BUF");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("FAT: " + TempCalculationVariable.FATmin.ToString());
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("SNF: " + TempCalculationVariable.SNFmin.ToString());
                            LCDFunctions.gotoLCD(5, 3);
                            LCDFunctions.sendString("00.00");
                            LCDFunctions.gotoLCD(5, 3);

                            Thread.Sleep(1);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(5, 1);
                            LCDFunctions.sendString("    ");
                            LCDFunctions.gotoLCD(5, 1);
                            LCDFunctions.sendString(TempCalculationVariable.FATmin.ToString());

                            LCDFunctions.gotoLCD(5, 2);
                            LCDFunctions.sendString("    ");
                            LCDFunctions.gotoLCD(5, 2);
                            LCDFunctions.sendString(TempCalculationVariable.SNFmin.ToString());


                            LCDFunctions.gotoLCD(5, 3);
                            LCDFunctions.sendString(ActiveFrameInfo.float4_0data);
                            if (ActiveFrameInfo.float4_0data.Length == 5)
                            {
                                LCDFunctions.gotoLCD(5, 3);
                            }

                        }
                        break;
                    }
                case 20107:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("FAT-SNF Rate:COW");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("FAT: 0.0 to 00.0");
                            FrameInformationDatabase(20107);
                            Thread.Sleep(1);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 20)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float1_0data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 22)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float3_0data);
                            }
                        }
                        break;
                    }
                case 2010710:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            FrameInformationDatabase(2010710);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("FAT-Only Rate:COW");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("FAT: " + TempCalculationVariable.FATmin.ToString());
                            
                            LCDFunctions.gotoLCD(5, 3);
                            LCDFunctions.sendString("00.00");
                            LCDFunctions.gotoLCD(5, 3);

                            Thread.Sleep(1);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(5, 1);
                            LCDFunctions.sendString("    ");
                            LCDFunctions.gotoLCD(5, 1);
                            LCDFunctions.sendString(TempCalculationVariable.FATmin.ToString());

                            LCDFunctions.gotoLCD(5, 3);
                            LCDFunctions.sendString(ActiveFrameInfo.float4_0data);
                            if (ActiveFrameInfo.float4_0data.Length == 5)
                            {
                                LCDFunctions.gotoLCD(5, 3);
                            }

                        }
                        break;
                    }
                case 20108:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("FAT-SNF Rate:BUF");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("FAT: 0.0 to 00.0");
                            FrameInformationDatabase(20108);
                            Thread.Sleep(1);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 20)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float1_0data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 22)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float3_0data);
                            }
                        }
                        break;
                    }
                case 2010810:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            FrameInformationDatabase(2010810);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("FAT-Only Rate:BUF");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("FAT: " + TempCalculationVariable.FATmin.ToString());

                            LCDFunctions.gotoLCD(5, 3);
                            LCDFunctions.sendString("00.00");
                            LCDFunctions.gotoLCD(5, 3);

                            Thread.Sleep(1);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(5, 1);
                            LCDFunctions.sendString("    ");
                            LCDFunctions.gotoLCD(5, 1);
                            LCDFunctions.sendString(TempCalculationVariable.FATmin.ToString());

                            LCDFunctions.gotoLCD(5, 3);
                            LCDFunctions.sendString(ActiveFrameInfo.float4_0data);
                            if (ActiveFrameInfo.float4_0data.Length == 5)
                            {
                                LCDFunctions.gotoLCD(5, 3);
                            }

                        }
                        break;
                    }
                case 20201:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Bonus Amount for COW");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("FAT LIMIT:0.0 to 0.0");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("Bonus Amount : 00.00");
                            FrameInformationDatabase(20201);
                            Thread.Sleep(1);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 20)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float1_0data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 24)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float1_1data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 23)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float4_0data);
                            }
                        }
                        break;
                    }
                case 2020110:
                    {

                        LCDFunctions.clearLCD();
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.gotoLCD(7, 2);
                        LCDFunctions.sendString("Saved");
                        break;
                    }
                case 20202:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Bonus Amount for BuF");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("FAT LIMIT:0.0 to 0.0");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("Bonus Amount : 00.00");
                            FrameInformationDatabase(20202);
                            Thread.Sleep(1);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 20)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float1_0data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 24)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float1_1data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 23)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float4_0data);
                            }
                        }
                        break;
                    }
                    case 2020210:
                    {

                        LCDFunctions.clearLCD();
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.gotoLCD(7, 2);
                        LCDFunctions.sendString("Saved");
                        break;
                    }
                case 20203:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(false);
                            
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Rate Chart for COW");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString(RateChartCollectionCOW[int.Parse(DataKeeper.GetValue("F2_COW_RATECHART"))]);
                            FrameInformationDatabase(20203);
                            Thread.Sleep(1);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 55)
                            {
                                LCDFunctions.sendString(RateChartCollectionCOW[ActiveFrameInfo.whichRatechart]);
                            }
                            
                        }
                        break;
                    }
                case 2020310:
                    {

                        LCDFunctions.clearLCD();
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.gotoLCD(7, 2);
                        LCDFunctions.sendString("Saved");
                        break;
                    }
                case 20204:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(false);

                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Rate Chart for BUF");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString(RateChartCollectionCOW[int.Parse(DataKeeper.GetValue("F2_BUF_RATECHART"))]);
                            FrameInformationDatabase(20204);
                            Thread.Sleep(1);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 55)
                            {
                                LCDFunctions.sendString(RateChartCollectionCOW[ActiveFrameInfo.whichRatechart]);
                            }

                        }
                        break;
                    }
                case 2020410:
                    {

                        LCDFunctions.clearLCD();
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.gotoLCD(7, 2);
                        LCDFunctions.sendString("Saved");
                        break;
                    }
                case 20205:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("COW   Min    Max");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("FAT: 00.00   00.00");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("SNF: 00.00   00.00");
                            LCDFunctions.gotoLCD(0, 3);
                            LCDFunctions.sendString("CLR: 00.00   00.00");
                            FrameInformationDatabase(20205);
                            Thread.Sleep(1);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 23)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float4_0data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 27)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float4_1data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 35)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float4_2data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 36)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float4_3data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 37)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float4_4data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 38)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float4_5data);
                            }
                        }
                        break;
                    }
                case 2020510:
                    {

                        LCDFunctions.clearLCD();
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.gotoLCD(7, 2);
                        LCDFunctions.sendString("Saved");
                        break;
                    }
                case 20206:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("BUF   Min    Max");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("FAT: 00.00   00.00");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("SNF: 00.00   00.00");
                            LCDFunctions.gotoLCD(0, 3);
                            LCDFunctions.sendString("CLR: 00.00   00.00");
                            FrameInformationDatabase(20206);
                            Thread.Sleep(1);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 23)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float4_0data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 27)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float4_1data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 35)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float4_2data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 36)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float4_3data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 37)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float4_4data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 38)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float4_5data);
                            }
                        }
                        break;
                    }
                case 2020610:
                    {

                        LCDFunctions.clearLCD();
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.gotoLCD(7, 2);
                        LCDFunctions.sendString("Saved");
                        break;
                    }
                case 20207:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Value A: 00.00");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Value B: 00.00");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("CLR CON: 00.00");
                            FrameInformationDatabase(20207);
                            Thread.Sleep(1);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 23)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float4_0data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 27)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float4_1data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 35)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float4_2data);
                            }
                        }
                        break;
                    }
                case 2020710:
                    {

                        LCDFunctions.clearLCD();
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.gotoLCD(7, 2);
                        LCDFunctions.sendString("Saved");
                        break;
                    }

                case 20208:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(0, 0);
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.sendString("FAT DEDUCT. C/B: 1/2");
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString("SNF DEDUCT. C/B: 3/4");
                        LCDFunctions.gotoLCD(0, 2);
                        LCDFunctions.sendString("FAT BONUS   C/B: 5/6");
                        LCDFunctions.gotoLCD(0, 3);
                        LCDFunctions.sendString("SNF BONUS   C/B: 7/8");
                        Thread.Sleep(1);

                        break;
                    }
                case 2020808:
                case 2020807:
                case 2020806:
                case 2020805:
                case 2020804:
                case 2020803:
                case 2020802:
                case 2020801:
                    {
                        FATorSNFD = "FAT";
                        DEDorBON = "DEDUCT";
                        COWorBUFD = "COW";

                        switch (FrameBase) {
                            case 2020801: 
                                {
                                    FATorSNFD = "FAT";
                                    DEDorBON = "DEDUCT";
                                    COWorBUFD = "COW";
                                    break;
                                }
                            case 2020802:
                                {
                                    FATorSNFD = "FAT";
                                    DEDorBON = "DEDUCT";
                                    COWorBUFD = "BUF";
                                    break;
                                }
                            case 2020803:
                                {
                                    FATorSNFD = "SNF";
                                    DEDorBON = "DEDUCT";
                                    COWorBUFD = "COW";
                                    break;
                                }
                            case 2020804:
                                {
                                    FATorSNFD = "SNF";
                                    DEDorBON = "DEDUCT";
                                    COWorBUFD = "BUF";
                                    break;
                                }
                            case 2020805:
                                {
                                    FATorSNFD = "FAT";
                                    DEDorBON = "BONUS";
                                    COWorBUFD = "COW";
                                    break;
                                }
                            case 2020806:
                                {
                                    FATorSNFD = "FAT";
                                    DEDorBON = "BONUS";
                                    COWorBUFD = "BUF";
                                    break;
                                }
                            case 2020807:
                                {
                                    FATorSNFD = "SNF";
                                    DEDorBON = "BONUS";
                                    COWorBUFD = "COW";
                                    break;
                                }
                            case 2020808:
                                {
                                    FATorSNFD = "SNF";
                                    DEDorBON = "BONUS";
                                    COWorBUFD = "BUF";
                                    break;
                                }
                            default:
                                {
                                    
                                    
                                    break;
                                }
                        }
                        

                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(0, 0);
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.sendString(FATorSNFD +" "+DEDorBON+" "+COWorBUFD);
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString("SLOT 1/2/3  :  1/2/3");
                        LCDFunctions.gotoLCD(0, 2);
                        LCDFunctions.sendString("SLOT 4/5    :    4/5");
                        Thread.Sleep(1);
                        break;
                    }
                case 202080805:
                case 202080804:
                case 202080803:
                case 202080802:
                case 202080801:

                case 202080705:
                case 202080704:
                case 202080703:
                case 202080702:
                case 202080701:

                case 202080605:
                case 202080604:
                case 202080603:
                case 202080602:
                case 202080601:

                case 202080505:
                case 202080504:
                case 202080503:
                case 202080502:
                case 202080501:

                case 202080405:
                case 202080404:
                case 202080403:
                case 202080402:
                case 202080401:

                case 202080305:
                case 202080304:
                case 202080303:
                case 202080302:
                case 202080301:

                case 202080205:
                case 202080204:
                case 202080203:
                case 202080202:
                case 202080201:

                case 202080105:
                case 202080104:
                case 202080103:
                case 202080102:
                case 202080101:
                    {
                        if (!LCDrefreshFlag)
                        {
                            try
                            {
                                string dam = FrameBase.ToString();
                                dam = dam.Remove(0, 8);
                                _slot_id = int.Parse(dam);
                            }
                            catch (Exception ex) { }

                            //FrameInformationDatabase(202080101);
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString(DEDorBON+" ["+COWorBUFD+" "+FATorSNFD+"] S" + _slot_id.ToString());
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("FAT LM:00.0 to 00.0");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("Amount : 0000.00");
                            FrameInformationDatabase(202080101);
                            try {

                                object[] tempObj = DataKeeper.F2_GetBonusDeduct(DEDorBON, FATorSNFD, COWorBUFD, uint.Parse(_slot_id.ToString()));

                                LCDFunctions.gotoLCD(0, 3);
                                LCDFunctions.sendString("["+tempObj[0].ToString()+"]["+tempObj[1].ToString()+"]-["+tempObj[2].ToString()+"]");
                            
                            }
                            catch (Exception ex) { }
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            Thread.Sleep(1);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 26)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float3_1data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 22)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float3_0data);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 30)
                            {
                                LCDFunctions.sendString(ActiveFrameInfo.float6_0data);
                            }
                        }
                        break;
                    }
                case 40110: {
                    if (!LCDrefreshFlag)
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(0, 0);
                        LCDFunctions.sendString("Set Password : (No )");
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString("Old Password : 0000");
                        LCDFunctions.gotoLCD(0, 2);
                        LCDFunctions.sendString("New Password : 0000");
                        LCDFunctions.gotoLCD(0, 3);
                        LCDFunctions.sendString("New Password : 0000");
                        Thread.Sleep(1);
                        FrameInformationDatabase(40110);
                    }
                    else if (LCDrefreshFlag) {
                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                        if (ActiveFrameInfo.WhichDataInFrame == 15)
                            LCDFunctions.sendString(YesOrNo[ActiveFrameInfo.toggleSwitch]);
                        else if (ActiveFrameInfo.WhichDataInFrame == 12)
                            LCDFunctions.sendString(ActiveFrameInfo.oldPassword);
                        else if (ActiveFrameInfo.WhichDataInFrame == 13)
                            LCDFunctions.sendString(ActiveFrameInfo.newPassword);
                        else if (ActiveFrameInfo.WhichDataInFrame == 14)
                            LCDFunctions.sendString(ActiveFrameInfo.RetypePassword);
                    }
                    break;
                }
                case 40112:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString("Failed");
                        Thread.Sleep(1);
                        FrameInformationDatabase(40112);
                        break;
                    }

                case 40111:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString("Password Set !");
                        Thread.Sleep(1);
                        FrameInformationDatabase(40111);
                        break;
                    }
                case 40210:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(4, 1);
                        LCDFunctions.sendString("Diary Setted !");
                        Thread.Sleep(1);
                        break;
                    }

                case 40310:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(4, 1);
                        LCDFunctions.sendString("Machine Updated !");
                        Thread.Sleep(1);
                        FrameInformationDatabase(40310);
                        break;
                    }

                case 40410:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(4, 1);
                        LCDFunctions.sendString("Printer Updated !");
                        Thread.Sleep(1);
                        FrameInformationDatabase(40410);
                        break;
                    }

                case 40510:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString("Weigh Scale Setted !");
                        Thread.Sleep(1);
                        FrameInformationDatabase(40510);
                        break;
                    }
                case 40610:
                    {
                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Manu. Collect:(No )");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Old Password : 0000");
                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("New Password : 0000");
                            LCDFunctions.gotoLCD(0, 3);
                            LCDFunctions.sendString("New Password : 0000");
                            Thread.Sleep(1);
                            LCDFunctions.gotoLCD(18, 0);
                            FrameInformationDatabase(40610);
                        }
                        else if (LCDrefreshFlag) {
                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 15) { 
                                //Toggle
                                LCDFunctions.sendString(YesOrNo[ActiveFrameInfo.toggleSwitch]);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 12) { 
                                //old password
                                LCDFunctions.sendString(ActiveFrameInfo.oldPassword);
                            }else if(ActiveFrameInfo.WhichDataInFrame == 13){
                                //New Password
                                LCDFunctions.sendString(ActiveFrameInfo.newPassword);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 14)
                            {
                                //Retype Password
                                LCDFunctions.sendString(ActiveFrameInfo.RetypePassword);
                            }
                        }
                        break;
                    }
                case 40611:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString("Value Updated !");
                        Thread.Sleep(1);
                        FrameInformationDatabase(40611);
                        break;
                    }
                    
                case 80210: {
                    Thread.Sleep(1);
                    LCDFunctions.clearLCD();
                    LCDFunctions.gotoLCD(0, 1);
                    LCDFunctions.sendString("Printing");
                    Thread.Sleep(1);
                    FrameInformationDatabase(80210);
                    break;
                
                }

                case 1003:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(0, 0);
                        LCDFunctions.sendString("Print by Date     1");
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString("Print All         2");

                        break;
                    }
                case 1004:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(0, 0);
                        LCDFunctions.sendString("Password          1");
                        

                        break;
                    }
                case 100510:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.clearLCD();

                        object[] tempAm = ExcelFileHandler.BankSheet_GetAmount(uint.Parse(ActiveFrameInfo.integer3_0));

                        if ((bool)tempAm[1])
                        {
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Balance : "+tempAm[0].ToString());
                        }
                        else {
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Account Not Found");
                        }


                        break;
                    }
                case 1002:
                case 1001:
                    {
                        if (!LCDrefreshFlag)
                        {
                            string CreditDeduct= "";
                            if (FrameBase == 1001)
                                CreditDeduct = "Credit to";
                            else if (FrameBase == 1002)
                                CreditDeduct = "Deduct fr";

                            Thread.Sleep(5);
                            LCDFunctions.CursorON_OFF(false);
                            LCDFunctions.clearLCD();
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString(CreditDeduct+" Member");
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("Code:");
                            LCDFunctions.gotoLCD(6, 1);
                            LCDFunctions.sendString("000");

                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("Amount:");
                            LCDFunctions.gotoLCD(8, 2);
                            LCDFunctions.sendString("00000.00");

                            LCDFunctions.gotoLCD(8, 1);
                            LCDFunctions.CursorON_OFF(true);
                            Thread.Sleep(5);

                            FrameInformationDatabase(1001);
                        }
                        else
                        {

                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 1)
                            {
                                LCDFunctions.sendString("000");
                                LCDFunctions.gotoLCD((ActiveFrameInfo.LCD_X + 3 - ActiveFrameInfo.FrameData_UserID.ToString().Length), ActiveFrameInfo.LCD_Y);
                                LCDFunctions.sendString(ActiveFrameInfo.FrameData_UserID.ToString());
                                LCDFunctions.gotoLCD((ActiveFrameInfo.LCD_X + 2), ActiveFrameInfo.LCD_Y);
                            }
                            else if (ActiveFrameInfo.WhichDataInFrame == 31) {
                                //LCDFunctions.sendString("        ");
                                LCDFunctions.sendString(ActiveFrameInfo.float7_0data);
                            }
                        }

                        break;
                    }
                case 100110: {

                    Thread.Sleep(1);
                    LCDFunctions.clearLCD();
                    LCDFunctions.gotoLCD(0, 1);
                    LCDFunctions.CursorON_OFF(false);
                    LCDFunctions.sendString("Amount Credited !");
                    LCDFunctions.gotoLCD(0, 1);

                    break;
                }
                case 100301:
                    {

                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString("Print by Date");

                            LCDFunctions.gotoLCD(0, 2);
                            LCDFunctions.sendString("Date:");

                            LCDFunctions.gotoLCD(6, 2);
                            LCDFunctions.sendString("00");

                            LCDFunctions.gotoLCD(8, 2);
                            LCDFunctions.sendString("/");

                            LCDFunctions.gotoLCD(9, 2);
                            LCDFunctions.sendString("00");

                            LCDFunctions.gotoLCD(11, 2);
                            LCDFunctions.sendString("/");

                            LCDFunctions.gotoLCD(12, 2);
                            LCDFunctions.sendString("00");

                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("  Code:");

                            Thread.Sleep(1);
                            FrameInformationDatabase(100301);
                        }
                        else if (LCDrefreshFlag)
                        {
                            LCDFunctions.gotoLCD(6, 2);
                            LCDFunctions.sendString(ActiveFrameInfo.day);
                            LCDFunctions.gotoLCD(9, 2);
                            LCDFunctions.sendString(ActiveFrameInfo.month);
                            LCDFunctions.gotoLCD(12, 2);
                            LCDFunctions.sendString(ActiveFrameInfo.year);

                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            if (ActiveFrameInfo.WhichDataInFrame == 4)
                                LCDFunctions.sendString(ActiveFrameInfo.day);
                            else if (ActiveFrameInfo.WhichDataInFrame == 5)
                                LCDFunctions.sendString(ActiveFrameInfo.month);
                            else if (ActiveFrameInfo.WhichDataInFrame == 6)
                                LCDFunctions.sendString(ActiveFrameInfo.year);
                            else if (ActiveFrameInfo.WhichDataInFrame == 47)
                            {
                                LCDFunctions.sendString("      ");
                                LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                                LCDFunctions.sendString(ActiveFrameInfo.integer3_0);
                            }

                        }

                        break;
                    }
                case 1005:
                case 100302:
                    {
                        string state1 = "Print All";
                        if (FrameBase == 1005)
                            state1 = "Member Detail";

                        if (!LCDrefreshFlag)
                        {
                            Thread.Sleep(1);
                            LCDFunctions.clearLCD();
                            LCDFunctions.CursorON_OFF(true);
                            LCDFunctions.gotoLCD(0, 0);
                            LCDFunctions.sendString(state1);
                            LCDFunctions.gotoLCD(0, 1);
                            LCDFunctions.sendString("  Code:");

                            Thread.Sleep(1);
                            FrameInformationDatabase(100302);
                        }
                        else if (LCDrefreshFlag)
                        {

                            LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                            
                            if (ActiveFrameInfo.WhichDataInFrame == 47)
                            {
                                LCDFunctions.sendString("      ");
                                LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                                LCDFunctions.sendString(ActiveFrameInfo.integer3_0);
                            }

                        }

                        break;
                    }
                case 10040110:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString("Password Updated");
                        Thread.Sleep(1);
                        FrameInformationDatabase(40611);
                        break;
                    }
                case 110110: {
                    if (!LCDrefreshFlag)
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(0, 0);
                        LCDFunctions.sendString("Enter to Delete.");
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString("Date: 00/00/00");
                        LCDFunctions.gotoLCD(0, 2);
                        LCDFunctions.sendString("Shift: (E)");
                        Thread.Sleep(1);
                        FrameInformationDatabase(110110);
                    }
                    else if (LCDrefreshFlag) {
                        LCDFunctions.gotoLCD(6, 1);
                        LCDFunctions.sendString(ActiveFrameInfo.day);
                        LCDFunctions.gotoLCD(9, 1);
                        LCDFunctions.sendString(ActiveFrameInfo.month);
                        LCDFunctions.gotoLCD(12, 1);
                        LCDFunctions.sendString(ActiveFrameInfo.year);
                        LCDFunctions.gotoLCD(8, 2);
                        LCDFunctions.sendString(Shift[ActiveFrameInfo.whichShift].ToString());

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X,ActiveFrameInfo.LCD_Y);
                        if(ActiveFrameInfo.WhichDataInFrame == 4)
                            LCDFunctions.sendString(ActiveFrameInfo.day);
                        else if (ActiveFrameInfo.WhichDataInFrame == 5)
                            LCDFunctions.sendString(ActiveFrameInfo.month);
                        else if(ActiveFrameInfo.WhichDataInFrame == 6)
                            LCDFunctions.sendString(ActiveFrameInfo.year);
                        else if (ActiveFrameInfo.WhichDataInFrame == 11)
                            LCDFunctions.sendString(Shift[ActiveFrameInfo.whichShift].ToString());
                    }
                    
                    break;
                }
                case 110111:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString("Want to Delete (Y/N)");
                        Thread.Sleep(1);
                        FrameInformationDatabase(110111);
                        ActiveFrameInfo.isYN = true;
                        break;
                    }

                case 110112: {
                    Thread.Sleep(1);
                    LCDFunctions.clearLCD();
                    LCDFunctions.CursorON_OFF(false);
                    LCDFunctions.gotoLCD(3, 1);
                    LCDFunctions.sendString("Deleted !");
                    Thread.Sleep(1);
                    FrameInformationDatabase(110112);
                    break;
                }
                case 110210: {
                    Thread.Sleep(1);
                    LCDFunctions.clearLCD();
                    LCDFunctions.CursorON_OFF(false);
                    LCDFunctions.gotoLCD(0, 1);
                    LCDFunctions.sendString("Want to Delete (Y/N)");
                    Thread.Sleep(1);
                    FrameInformationDatabase(110210);
                    ActiveFrameInfo.isYN = true;
                    break;
                }
                case 110211:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.gotoLCD(3, 1);
                        LCDFunctions.sendString("Deleted !");
                        Thread.Sleep(1);
                        break;
                    }

                case 12010110:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.gotoLCD(3, 1);
                        LCDFunctions.sendString("Saved !");
                        Thread.Sleep(1);
                        break;
                    }
                case 12010210:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.gotoLCD(3, 1);
                        LCDFunctions.sendString("Saved !");
                        Thread.Sleep(1);
                        break;
                    }
                case 12010310:
                    {
                        Thread.Sleep(1);
                        LCDFunctions.clearLCD();
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.gotoLCD(3, 1);
                        LCDFunctions.sendString("Saved !");
                        Thread.Sleep(1);
                        break;
                    }


                case 29850:
                    {
                        Thread.Sleep(5);
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(6, 2);
                        LCDFunctions.sendString("Wait....");
                        break;
                    }

                case 123456:
                    {
                        Thread.Sleep(5);
                        LCDFunctions.CursorON_OFF(false);
                        LCDFunctions.clearLCD();
                        LCDFunctions.gotoLCD(0, 1);
                        LCDFunctions.sendString(customMessgae);
                        LCDFunctions.gotoLCD(0, 2);
                        LCDFunctions.sendString(customMessgae1);
                        break;
                    }
                default:
                    {
                        BackSpaceEvent();
                        break;
                    }
            }

            
        }

        //Frames Information Database
        private static void FrameInformationDatabase(uint _id) {
            switch (_id) {
                case 0:
                    {
                        KeyboardMode = 0;
                        ActiveFrameInfo.IsInput = false;
                        ActiveFrameInfo.FrameBase = 0;
                        
                        break;
                    }
                case 1: {
                    KeyboardMode = 0;
                    ActiveFrameInfo.IsInput = false;
                    ActiveFrameInfo.FrameBase = 1;

    
                    break;
                }
                case 3: {
                    
                    ActiveFrameInfo.ActiveX = 6;
                    ActiveFrameInfo.ActiveY = 1;
                    ActiveFrameInfo.IsInput = true;
                    ActiveFrameInfo.FrameBase = FrameBase;
                    ActiveFrameInfo.year = "";
                    ActiveFrameInfo.month = "";
                    ActiveFrameInfo.day = "";
                    ActiveFrameInfo.hour = "";
                    ActiveFrameInfo.minute = "";
                    ActiveFrameInfo.second = "";
                    ActiveFrameInfo.whichShift = 0;
                    ActiveFrameInfo.whichDayofWeek = 0;

                    ActiveFrameInfo.WhichDataInFrame = 7;
                    ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(6, 1, 7, 0);

                    ActiveFrameInfo.InputList.Clear();
                    ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                    ActiveFrameInfo.InputList.Add(new FrameInputInfo(6, 1, 7, 0));
                    ActiveFrameInfo.InputList.Add(new FrameInputInfo(9, 1, 8, 1));
                    ActiveFrameInfo.InputList.Add(new FrameInputInfo(12, 1, 9, 2));
                    ActiveFrameInfo.InputList.Add(new FrameInputInfo(6, 2, 4, 3));
                    ActiveFrameInfo.InputList.Add(new FrameInputInfo(9, 2, 5, 4));
                    ActiveFrameInfo.InputList.Add(new FrameInputInfo(12, 2, 6, 5));
                    ActiveFrameInfo.InputList.Add(new FrameInputInfo(6, 3, 10, 6));

                    KeyboardMode = 2;
                    keyboardNumMode = true;
                    keyboardMaxChar = 3;
                    break;
                }
                case 4: {
                    KeyboardMode = 0;
                    break;
                }
                case 5:
                    {
                        ActiveFrameInfo.ActiveX = 8;
                        ActiveFrameInfo.ActiveY = 2;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;
                        
                        ActiveFrameInfo.FrameData_UserID = 0;

                        ActiveFrameInfo.WhichDataInFrame = 1;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(8, 2, 1, 0);


                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(8, 2, 1, 0));

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;
                        break;
                    }
                case 7:
                    {

                        ActiveFrameInfo.ActiveX = 6;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;
                        ActiveFrameInfo.year = "";
                        ActiveFrameInfo.month = "";
                        ActiveFrameInfo.day = "";
                        ActiveFrameInfo.whichShift = 0;
                        

                        ActiveFrameInfo.WhichDataInFrame = 4;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(6, 1, 4, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(6, 1, 4, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(9, 1, 5, 1));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(12, 1, 6, 2));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(8, 2, 11, 3));

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;
                        break;
                    }
                case 9:
                    {
                        ActiveFrameInfo.ActiveX = 10;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        ActiveFrameInfo.integer3_0 = "";
                        ActiveFrameInfo.integer3_1 = "";

                        ActiveFrameInfo.WhichDataInFrame = 47;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(10, 1, 47, 0);


                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(10, 1, 47, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(10, 2, 52, 1));

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;
                        break;
                    }
                case 12: {
                    if (F12_PASSWORD_STATE) {
                        ActiveFrameInfo.ActiveX = 8;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;
                        ActiveFrameInfo.passwordGlobal = "";
                        ActiveFrameInfo.passwordGlobalReal = "";


                        ActiveFrameInfo.WhichDataInFrame = 65;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(8, 1, 65, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(8, 1, 65, 0));


                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        LCDrefreshFlag = true;
                        LCDFunctions.gotoLCD(int.Parse(ActiveFrameInfo.ActiveX.ToString()), int.Parse(ActiveFrameInfo.ActiveY.ToString()));
                        LCDFunctions.CursorON_OFF(true);
                    }
                    break;
                }

                case 101:
                    {
                        ActiveFrameInfo.ActiveX = 8;
                        ActiveFrameInfo.ActiveY = 2;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;
                        ActiveFrameInfo.FrameData_Name = "";
                        ActiveFrameInfo.FrameData_Phone = "";
                        ActiveFrameInfo.FrameData_UserID = 0;

                        ActiveFrameInfo.WhichDataInFrame = 1;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(8, 2, 1, 0);
          

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(8,2,1,0));

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;
                        break;
                    }
                case 401: {
                    ActiveFrameInfo.IsInput = true;
                    ActiveFrameInfo.FrameBase = FrameBase;
                    break;
                }
                case 10110: {
                    ActiveFrameInfo.IsInput = true;
                    ActiveFrameInfo.FrameBase = FrameBase;
                    break;
                }
                case 10111:
                    {
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;
                        break;
                    }
                case 10112:
                    {
                        ActiveFrameInfo.ActiveX = 5;
                        ActiveFrameInfo.ActiveY = 0;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;
                        ActiveFrameInfo.FrameData_Name = "";
                        ActiveFrameInfo.FrameData_Phone = "";
                        

                        ActiveFrameInfo.WhichDataInFrame = 2;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(5, 0, 2, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 0, 2, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(6, 1, 3, 1));

                        
                        KeyboardMode = 2;
                        keyboardNumMode = false;
                        LCDrefreshFlag = true;
                        LCDFunctions.gotoLCD(int.Parse(ActiveFrameInfo.ActiveX.ToString()), int.Parse(ActiveFrameInfo.ActiveY.ToString()));

                        break;
                    }
                case 102:
                    {
                        ActiveFrameInfo.ActiveX = 8;
                        ActiveFrameInfo.ActiveY = 2;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;
                        ActiveFrameInfo.FrameData_Name = "";
                        ActiveFrameInfo.FrameData_Phone = "";
                        ActiveFrameInfo.FrameData_UserID = 0;

                        //Important Entries
                       

                        ActiveFrameInfo.WhichDataInFrame = 1;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(8, 2, 1, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(8, 2, 1, 0));

                        
                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;
                        break;
                    }
                case 10410:
                    {
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;


                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        break;
                    }
                case 402:
                    {
                        ActiveFrameInfo.ActiveX = 0;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;
                        ActiveFrameInfo.screenTittle = "";
                        

                        //Important Entries


                        ActiveFrameInfo.WhichDataInFrame = 16;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(0, 1, 16, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(0, 1, 16, 0));


                        KeyboardMode = 2;
                        keyboardNumMode = false;
                        keyboardMaxChar = 3;
                        break;
                    }
                case 403: {
                    ActiveFrameInfo.ActiveX = 9;
                    ActiveFrameInfo.ActiveY = 1;
                    ActiveFrameInfo.IsInput = true;
                    ActiveFrameInfo.FrameBase = FrameBase;
                    ActiveFrameInfo.ActiveMachine = 0;


                    //Important Entries


                    ActiveFrameInfo.WhichDataInFrame = 17;
                    ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(9, 1, 17, 0);

                    ActiveFrameInfo.InputList.Clear();
                    ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                    ActiveFrameInfo.InputList.Add(new FrameInputInfo(9, 1, 17, 0));


                    KeyboardMode = 2;
                    keyboardNumMode = true;
                    keyboardMaxChar = 3;
                    break;
                }
                case 404:
                    {
                        ActiveFrameInfo.ActiveX = 9;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;
                        ActiveFrameInfo.ActivePrinter = 0;


                        //Important Entries


                        ActiveFrameInfo.WhichDataInFrame = 18;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(9, 1, 18, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(9, 1, 18, 0));


                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;
                        break;
                    }
                case 405:
                    {
                        ActiveFrameInfo.ActiveX = 5;
                        ActiveFrameInfo.ActiveY = 2;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;
                        ActiveFrameInfo.ActiveWeigh = 0;


                        //Important Entries


                        ActiveFrameInfo.WhichDataInFrame = 19;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(5, 2, 19, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 2, 17, 0));


                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;
                        break;
                    }
                case 801:
                    {
                        ActiveFrameInfo.ActiveX = 10;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        try
                        {
                            ActiveFrameInfo.integer4_0 = ExcelFileHandler.Get_NextSerialNo(true).ToString();
                        }
                        catch (Exception ex) { }

                        ActiveFrameInfo.toggleSwitch = 0;
                        ActiveFrameInfo.float7_0data = "";

                        ActiveFrameInfo.float6_0data = "";
                        ActiveFrameInfo.float6_0index = 0;
                       


                        //Important Entries


                        ActiveFrameInfo.WhichDataInFrame = 30;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(10, 1, 30, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(10, 1, 30, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(12, 2, 15, 1));

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        
                        break;
                    }
                case 802:
                    {
                        ActiveFrameInfo.ActiveX = 14;
                        ActiveFrameInfo.ActiveY = 0;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        ActiveFrameInfo.toggleSwitch = 0;
                        ActiveFrameInfo.float7_0data = "";

                        ActiveFrameInfo.float6_0data = "";
                        ActiveFrameInfo.float6_0index = 0;

                        ActiveFrameInfo.VechileNo = "";


                        try {
                            ActiveFrameInfo.integer4_0 = ExcelFileHandler.Get_NextSerialNo(false).ToString();
                        }
                        catch (Exception ex) { }
                        //Important Entries


                        ActiveFrameInfo.WhichDataInFrame = 28;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(14, 0, 28, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(14, 0, 28, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(10, 1, 30, 1));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(12, 2, 15, 2));

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                        KeyboardMode = 2;
                        keyboardNumMode = true;

                        break;
                    }
                case 901:
                    {

                        ActiveFrameInfo.ActiveX = 8;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;
                        ActiveFrameInfo.year = "";
                        ActiveFrameInfo.month = "";
                        ActiveFrameInfo.day = "";
                        ActiveFrameInfo.whichShift = 0;

                        ActiveFrameInfo.year_1 = "";
                        ActiveFrameInfo.month_1 = "";
                        ActiveFrameInfo.day_1 = "";
                        ActiveFrameInfo.whichShift_1 = 0;

                        ActiveFrameInfo.WhichDataInFrame = 4;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(8, 1, 4, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(8, 1, 4, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(11, 1, 5, 1));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(14, 1, 6, 2));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(18, 1, 11, 3));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(8, 2, 61, 4));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(11, 2, 62, 5));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(14, 2, 63, 6));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(18, 2, 64, 7));

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;
                        break;
                    }

                case 1101: {
                    ActiveFrameInfo.ActiveX = 8;
                    ActiveFrameInfo.ActiveY = 2;
                    ActiveFrameInfo.IsInput = true;
                    ActiveFrameInfo.FrameBase = FrameBase;
                    ActiveFrameInfo.FrameData_UserID = 0;
                    

                    ActiveFrameInfo.WhichDataInFrame = 1;
                    ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(8, 2, 1, 0);

                    ActiveFrameInfo.InputList.Clear();
                    ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                    ActiveFrameInfo.InputList.Add(new FrameInputInfo(8, 2,1, 0));


                    KeyboardMode = 2;
                    keyboardNumMode = true;
                    LCDrefreshFlag = true;
                    LCDFunctions.gotoLCD(int.Parse(ActiveFrameInfo.ActiveX.ToString()), int.Parse(ActiveFrameInfo.ActiveY.ToString()));
                    LCDFunctions.CursorON_OFF(true);
                    break;
                }
                case 1102:
                    {
                        ActiveFrameInfo.ActiveX = 6;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;
                        ActiveFrameInfo.year = "";
                        ActiveFrameInfo.month = "";
                        ActiveFrameInfo.day = "";
                        ActiveFrameInfo.whichShift = 0;


                        ActiveFrameInfo.WhichDataInFrame = 4;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(6, 1, 4, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(6, 1, 4, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(9, 1, 5, 1));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(12, 1, 6, 2));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(8, 2, 11, 3));

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;
                        LCDFunctions.CursorON_OFF(true);
                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                        break;
                    }
                case 1204:
                    {
                        ActiveFrameInfo.ActiveX = 11;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;
                        ActiveFrameInfo.passwordGlobal = "";
                        ActiveFrameInfo.passwordGlobalReal = "";


                        ActiveFrameInfo.WhichDataInFrame = 65;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(11, 1, 65, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(11, 1, 65, 0));


                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        LCDrefreshFlag = true;
                        LCDFunctions.gotoLCD(int.Parse(ActiveFrameInfo.ActiveX.ToString()), int.Parse(ActiveFrameInfo.ActiveY.ToString()));
                        LCDFunctions.CursorON_OFF(true);
                        break;
                    }
                case 120101: {
                    ActiveFrameInfo.ActiveX = 5;
                    ActiveFrameInfo.ActiveY = 0;
                    ActiveFrameInfo.IsInput = true;
                    ActiveFrameInfo.FrameBase = FrameBase;
                    ActiveFrameInfo.whichBaudRate = uint.Parse(DataKeeper.GetValue("COM1_BAUD_RATE"));
                    ActiveFrameInfo.whichDataBit = uint.Parse(DataKeeper.GetValue("COM1_DATA_BIT"));
                    ActiveFrameInfo.whichStopBit = uint.Parse(DataKeeper.GetValue("COM1_STOP_BIT"));
                    ActiveFrameInfo.whichHandShake = uint.Parse(DataKeeper.GetValue("COM1_HANDSHAKE"));
                    ActiveFrameInfo.whichParity = uint.Parse(DataKeeper.GetValue("COM1_PARITY"));

                    ActiveFrameInfo.WhichDataInFrame = 40;
                    ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(5, 0, 40, 0);

                    ActiveFrameInfo.InputList.Clear();
                    ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                    ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 0, 40, 0));
                    ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 1, 41, 1));
                    ActiveFrameInfo.InputList.Add(new FrameInputInfo(15, 1, 42, 2));
                    ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 2, 43, 3));
                    ActiveFrameInfo.InputList.Add(new FrameInputInfo(9, 3, 44, 4));


                    KeyboardMode = 2;
                    keyboardNumMode = true;
                    LCDrefreshFlag = true;
                    LCDFunctions.gotoLCD(int.Parse(ActiveFrameInfo.ActiveX.ToString()), int.Parse(ActiveFrameInfo.ActiveY.ToString()));
                    LCDFunctions.CursorON_OFF(true);
                    break;
                }
                case 120102:
                    {
                        ActiveFrameInfo.ActiveX = 5;
                        ActiveFrameInfo.ActiveY = 0;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;
                        ActiveFrameInfo.whichBaudRate = uint.Parse(DataKeeper.GetValue("COM2_BAUD_RATE"));
                        ActiveFrameInfo.whichDataBit = uint.Parse(DataKeeper.GetValue("COM2_DATA_BIT"));
                        ActiveFrameInfo.whichStopBit = uint.Parse(DataKeeper.GetValue("COM2_STOP_BIT"));
                        ActiveFrameInfo.whichHandShake = uint.Parse(DataKeeper.GetValue("COM2_HANDSHAKE"));
                        ActiveFrameInfo.whichParity = uint.Parse(DataKeeper.GetValue("COM2_PARITY"));

                        ActiveFrameInfo.WhichDataInFrame = 40;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(5, 0, 40, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 0, 40, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 1, 41, 1));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(15, 1, 42, 2));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 2, 43, 3));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(9, 3, 44, 4));


                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        LCDrefreshFlag = true;
                        LCDFunctions.gotoLCD(int.Parse(ActiveFrameInfo.ActiveX.ToString()), int.Parse(ActiveFrameInfo.ActiveY.ToString()));
                        LCDFunctions.CursorON_OFF(true);
                        break;
                    }
                case 120103:
                    {
                        ActiveFrameInfo.ActiveX = 5;
                        ActiveFrameInfo.ActiveY = 0;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;
                        ActiveFrameInfo.whichBaudRate = uint.Parse(DataKeeper.GetValue("COM3_BAUD_RATE"));
                        ActiveFrameInfo.whichDataBit = uint.Parse(DataKeeper.GetValue("COM3_DATA_BIT"));
                        ActiveFrameInfo.whichStopBit = uint.Parse(DataKeeper.GetValue("COM3_STOP_BIT"));
                        ActiveFrameInfo.whichHandShake = uint.Parse(DataKeeper.GetValue("COM3_HANDSHAKE"));
                        ActiveFrameInfo.whichParity = uint.Parse(DataKeeper.GetValue("COM3_PARITY"));

                        ActiveFrameInfo.WhichDataInFrame = 40;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(5, 0, 40, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 0, 40, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 1, 41, 1));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(15, 1, 42, 2));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 2, 43, 3));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(9, 3, 44, 4));


                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        LCDrefreshFlag = true;
                        LCDFunctions.gotoLCD(int.Parse(ActiveFrameInfo.ActiveX.ToString()), int.Parse(ActiveFrameInfo.ActiveY.ToString()));
                        LCDFunctions.CursorON_OFF(true);
                        break;
                    }
                case 120301:
                    {
                        ActiveFrameInfo.ActiveX = 5;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;
                        ActiveFrameInfo.shiftSwitch3_0 = uint.Parse(DataKeeper.GetValue("DEVICE_MAN_COM1_TX_DEVICE"));
                        ActiveFrameInfo.shiftSwitch3_1 = uint.Parse(DataKeeper.GetValue("DEVICE_MAN_COM1_RX_DEVICE")); 
                       

                        ActiveFrameInfo.WhichDataInFrame = 56;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(5, 1, 56, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 1, 56, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 3, 57, 1));
                        


                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        LCDrefreshFlag = true;
                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                        LCDFunctions.CursorON_OFF(false);
                        break;
                    }
                case 120302:
                    {
                        ActiveFrameInfo.ActiveX = 5;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;
                        ActiveFrameInfo.shiftSwitch3_0 = uint.Parse(DataKeeper.GetValue("DEVICE_MAN_COM2_TX_DEVICE"));
                        ActiveFrameInfo.shiftSwitch3_1 = uint.Parse(DataKeeper.GetValue("DEVICE_MAN_COM2_RX_DEVICE"));


                        ActiveFrameInfo.WhichDataInFrame = 56;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(5, 1, 56, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 1, 56, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 3, 57, 1));



                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        LCDrefreshFlag = true;
                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                        LCDFunctions.CursorON_OFF(false);
                        break;
                    }
                case 120303:
                    {
                        ActiveFrameInfo.ActiveX = 5;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;
                        ActiveFrameInfo.shiftSwitch3_0 = uint.Parse(DataKeeper.GetValue("DEVICE_MAN_COM3_TX_DEVICE"));
                        ActiveFrameInfo.shiftSwitch3_1 = uint.Parse(DataKeeper.GetValue("DEVICE_MAN_COM3_RX_DEVICE"));


                        ActiveFrameInfo.WhichDataInFrame = 56;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(5, 1, 56, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 1, 56, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 3, 57, 1));



                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        LCDrefreshFlag = true;
                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                        LCDFunctions.CursorON_OFF(false);
                        break;
                    }
                case 120304:
                    {
                        ActiveFrameInfo.ActiveX = 5;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;
                        ActiveFrameInfo.toggleSwitch = 0;
                        ActiveFrameInfo.shiftSwitch3_0 = 0;


                        ActiveFrameInfo.WhichDataInFrame = 15;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(5, 1, 15, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 1, 15, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 3, 56, 1));



                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        LCDrefreshFlag = true;
                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                        LCDFunctions.CursorON_OFF(false);
                        break;
                    }
                case 10210:
                    {
                        ActiveFrameInfo.ActiveX = 5;
                        ActiveFrameInfo.ActiveY = 0;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;
                        ActiveFrameInfo.FrameData_Name = "";
                        ActiveFrameInfo.FrameData_Phone = "";

                        ActiveFrameInfo.WhichDataInFrame = 2;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(5, 0, 2, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 0, 2, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(6, 1, 3, 1));

                        
                        KeyboardMode = 2;
                        keyboardNumMode = false;
                        LCDrefreshFlag = true;
                        LCDFunctions.gotoLCD(int.Parse(ActiveFrameInfo.ActiveX.ToString()), int.Parse(ActiveFrameInfo.ActiveY.ToString()));

                        break;
                    }
                case 120201:
                    {
                        ActiveFrameInfo.ActiveX = 8;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;
                        ActiveFrameInfo.ActivePrinter = 0;


                        //Important Entries


                        ActiveFrameInfo.WhichDataInFrame = 18;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(8, 1, 18, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(8, 1, 18, 0));


                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;
                        break;
                    }
                case 40110:
                    {
                        ActiveFrameInfo.ActiveX = 16;
                        ActiveFrameInfo.ActiveY = 0;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;
                        ActiveFrameInfo.oldPassword = "";
                        ActiveFrameInfo.newPassword = "";
                        ActiveFrameInfo.RetypePassword = "";
                        ActiveFrameInfo.toggleSwitch = 0;

                        ActiveFrameInfo.WhichDataInFrame = 15;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(16, 0, 15, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(16, 0, 15, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(15, 1, 12, 1));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(15, 2, 13, 2));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(15, 3, 14, 3));


                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        LCDrefreshFlag = true;
                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X,ActiveFrameInfo.LCD_Y);
                        break;
                    }
                case 40610: {
                    ActiveFrameInfo.ActiveX = 15;
                    ActiveFrameInfo.ActiveY = 0;
                    ActiveFrameInfo.IsInput = true;
                    ActiveFrameInfo.FrameBase = FrameBase;
                    ActiveFrameInfo.oldPassword = "";
                    ActiveFrameInfo.newPassword = "";
                    ActiveFrameInfo.RetypePassword = "";
                    ActiveFrameInfo.toggleSwitch = 0;

                    ActiveFrameInfo.WhichDataInFrame = 15;
                    ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(15, 0, 15, 0);

                    ActiveFrameInfo.InputList.Clear();
                    ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                    ActiveFrameInfo.InputList.Add(new FrameInputInfo(15, 0, 15, 0));
                    ActiveFrameInfo.InputList.Add(new FrameInputInfo(15, 1, 12, 1));
                    ActiveFrameInfo.InputList.Add(new FrameInputInfo(15, 2, 13, 2));
                    ActiveFrameInfo.InputList.Add(new FrameInputInfo(15, 3, 14, 3));


                    KeyboardMode = 2;
                    keyboardNumMode = true;
                    LCDrefreshFlag = true;
                    LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                    break;
                }
                case 110110: {
                    ActiveFrameInfo.ActiveX = 6;
                    ActiveFrameInfo.ActiveY = 1;
                    ActiveFrameInfo.IsInput = true;
                    ActiveFrameInfo.FrameBase = FrameBase;
                    ActiveFrameInfo.year = "";
                    ActiveFrameInfo.month = "";
                    ActiveFrameInfo.day = "";
                    ActiveFrameInfo.whichShift = 0;
                    

                    ActiveFrameInfo.WhichDataInFrame = 4;
                    ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(6, 1, 4, 0);

                    ActiveFrameInfo.InputList.Clear();
                    ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                    ActiveFrameInfo.InputList.Add(new FrameInputInfo(6, 1, 4, 0));
                    ActiveFrameInfo.InputList.Add(new FrameInputInfo(9, 1, 5, 1));
                    ActiveFrameInfo.InputList.Add(new FrameInputInfo(12, 1, 6, 2));
                    ActiveFrameInfo.InputList.Add(new FrameInputInfo(8, 2, 11, 3));

                    KeyboardMode = 2;
                    keyboardNumMode = true;
                    keyboardMaxChar = 3;
                    break;
                }
                case 20101:
                    {
                        ActiveFrameInfo.ActiveX = 13;
                        ActiveFrameInfo.ActiveY = 2;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        ActiveFrameInfo.float5_1data = "";
                        ActiveFrameInfo.float5_0data = "";

                        ActiveFrameInfo.float5_1index = 0;
                        ActiveFrameInfo.float5_0index = 0;


                        ActiveFrameInfo.WhichDataInFrame = 29;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(13, 2, 29, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(13, 2, 29, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(13, 3, 32, 1));

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);

                        break;
                    }
                case 20102:
                    {
                        ActiveFrameInfo.ActiveX = 13;
                        ActiveFrameInfo.ActiveY = 2;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        ActiveFrameInfo.float5_1data = "";
                        ActiveFrameInfo.float5_0data = "";

                        ActiveFrameInfo.float5_1index = 0;
                        ActiveFrameInfo.float5_0index = 0;


                        ActiveFrameInfo.WhichDataInFrame = 29;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(13, 2, 29, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(13, 2, 29, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(13, 3, 32, 1));

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);

                        break;
                    }
                case 20103:
                    {
                        ActiveFrameInfo.ActiveX = 5;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        ActiveFrameInfo.float1_0data = "";
                        ActiveFrameInfo.float3_0data = "";

                        ActiveFrameInfo.float1_0index = 0;
                        ActiveFrameInfo.float3_0index = 0;

                        ActiveFrameInfo.integer2_0 = "";
                        ActiveFrameInfo.integer2_1 = "";




                        ActiveFrameInfo.WhichDataInFrame = 20;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(5, 1, 20, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 1, 20, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(12, 1, 22, 1));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 2, 46, 2));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(12, 2, 51, 3));

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);

                        break;
                    }
                case 20104:
                    {
                        ActiveFrameInfo.ActiveX = 5;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        ActiveFrameInfo.float1_0data = "";
                        ActiveFrameInfo.float3_0data = "";

                        ActiveFrameInfo.float1_0index = 0;
                        ActiveFrameInfo.float3_0index = 0;

                        ActiveFrameInfo.integer2_0 = "";
                        ActiveFrameInfo.integer2_1 = "";


                        ActiveFrameInfo.WhichDataInFrame = 20;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(5, 1, 20, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 1, 20, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(12, 1, 22, 1));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 2, 46, 2));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(12, 2, 51, 3));

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);

                        break;
                    }
                case 20105:
                    {
                        ActiveFrameInfo.ActiveX = 5;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;
                        LCDrefreshFlag = false;

                        ActiveFrameInfo.float1_0data = "";
                        ActiveFrameInfo.float3_0data = "";
                        ActiveFrameInfo.float1_1data = "";
                        ActiveFrameInfo.float3_1data = "";

                        ActiveFrameInfo.float1_0index = 0;
                        ActiveFrameInfo.float3_0index = 0;
                        ActiveFrameInfo.float1_1index = 0;
                        ActiveFrameInfo.float3_1index = 0;


                        ActiveFrameInfo.WhichDataInFrame = 20;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(5, 1, 20, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 1, 20, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(12, 1, 22, 1));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 2, 24, 2));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(12, 2, 26, 3));

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);

                        break;
                    }
                case 20106:
                    {
                        ActiveFrameInfo.ActiveX = 5;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        ActiveFrameInfo.float1_0data = "";
                        ActiveFrameInfo.float3_0data = "";
                        ActiveFrameInfo.float1_1data = "";
                        ActiveFrameInfo.float3_1data = "";

                        ActiveFrameInfo.float1_0index = 0;
                        ActiveFrameInfo.float3_0index = 0;
                        ActiveFrameInfo.float1_1index = 0;
                        ActiveFrameInfo.float3_1index = 0;


                        ActiveFrameInfo.WhichDataInFrame = 20;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(5, 1, 20, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 1, 20, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(12, 1, 22, 1));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 2, 24, 2));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(12, 2, 26, 3));

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);

                        break;
                    }
                case 20107:
                    {
                        ActiveFrameInfo.ActiveX = 5;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        ActiveFrameInfo.float1_0data = "";
                        ActiveFrameInfo.float3_0data = "";
                        

                        ActiveFrameInfo.float1_0index = 0;
                        ActiveFrameInfo.float3_0index = 0;
                        


                        ActiveFrameInfo.WhichDataInFrame = 20;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(5, 1, 20, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 1, 20, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(12, 1, 22, 1));

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);

                        break;
                    }
                case 20108:
                    {
                        ActiveFrameInfo.ActiveX = 5;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        ActiveFrameInfo.float1_0data = "";
                        ActiveFrameInfo.float3_0data = "";


                        ActiveFrameInfo.float1_0index = 0;
                        ActiveFrameInfo.float3_0index = 0;



                        ActiveFrameInfo.WhichDataInFrame = 20;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(5, 1, 20, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 1, 20, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(12, 1, 22, 1));

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);

                        break;
                    }
                case 20201:
                    {
                        ActiveFrameInfo.ActiveX = 10;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        ActiveFrameInfo.float1_0data = "";
                        ActiveFrameInfo.float1_1data = "";
                        ActiveFrameInfo.float4_0data = "";


                        ActiveFrameInfo.float1_0index = 0;
                        ActiveFrameInfo.float1_1index = 0;
                        ActiveFrameInfo.float4_0index = 0;



                        ActiveFrameInfo.WhichDataInFrame = 20;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(10, 1, 20, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(10, 1, 20, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(17, 1, 24, 1));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(15, 2, 23, 2));

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);

                        break;
                    }
                case 20202:
                    {
                        ActiveFrameInfo.ActiveX = 10;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        ActiveFrameInfo.float1_0data = "";
                        ActiveFrameInfo.float1_1data = "";
                        ActiveFrameInfo.float4_0data = "";


                        ActiveFrameInfo.float1_0index = 0;
                        ActiveFrameInfo.float1_1index = 0;
                        ActiveFrameInfo.float4_0index = 0;



                        ActiveFrameInfo.WhichDataInFrame = 20;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(10, 1, 20, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(10, 1, 20, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(17, 1, 24, 1));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(15, 2, 23, 2));

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);

                        break;
                    }
                case 20205:
                    {
                        ActiveFrameInfo.ActiveX = 5;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        ActiveFrameInfo.float4_0data = "";
                        ActiveFrameInfo.float4_1data = "";
                        ActiveFrameInfo.float4_2data = "";
                        ActiveFrameInfo.float4_3data = "";
                        ActiveFrameInfo.float4_4data = "";
                        ActiveFrameInfo.float4_5data = "";


                        ActiveFrameInfo.float4_0index = 0;
                        ActiveFrameInfo.float4_1index = 0;
                        ActiveFrameInfo.float4_2index = 0;
                        ActiveFrameInfo.float4_3index = 0;
                        ActiveFrameInfo.float4_4index = 0;
                        ActiveFrameInfo.float4_5index = 0;



                        ActiveFrameInfo.WhichDataInFrame = 23;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(5, 1, 23, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 1, 23, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(13, 1, 27, 1));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 2, 35, 2));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(13, 2, 36, 3));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 3, 37, 4));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(13, 3, 38, 5));

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);

                        break;
                    }
                case 20206:
                    {
                        ActiveFrameInfo.ActiveX = 5;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        ActiveFrameInfo.float4_0data = "";
                        ActiveFrameInfo.float4_1data = "";
                        ActiveFrameInfo.float4_2data = "";
                        ActiveFrameInfo.float4_3data = "";
                        ActiveFrameInfo.float4_4data = "";
                        ActiveFrameInfo.float4_5data = "";


                        ActiveFrameInfo.float4_0index = 0;
                        ActiveFrameInfo.float4_1index = 0;
                        ActiveFrameInfo.float4_2index = 0;
                        ActiveFrameInfo.float4_3index = 0;
                        ActiveFrameInfo.float4_4index = 0;
                        ActiveFrameInfo.float4_5index = 0;



                        ActiveFrameInfo.WhichDataInFrame = 23;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(5, 1, 23, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 1, 23, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(13, 1, 27, 1));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 2, 35, 2));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(13, 2, 36, 3));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 3, 37, 4));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(13, 3, 38, 5));

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);

                        break;
                    }

                case 20207:
                    {
                        ActiveFrameInfo.ActiveX = 9;
                        ActiveFrameInfo.ActiveY = 0;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        ActiveFrameInfo.float4_0data = "";
                        ActiveFrameInfo.float4_1data = "";
                        ActiveFrameInfo.float4_2data = "";

                        ActiveFrameInfo.float4_0index = 0;
                        ActiveFrameInfo.float4_1index = 0;
                        ActiveFrameInfo.float4_2index = 0;

                        ActiveFrameInfo.WhichDataInFrame = 23;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(9, 0, 23, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(9, 0, 23, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(9, 1, 27, 1));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(9, 2, 35, 2));

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);

                        break;
                    }
                case 20203:
                    {
                        ActiveFrameInfo.ActiveX = 0;
                        ActiveFrameInfo.ActiveY = 2;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        ActiveFrameInfo.whichRatechart = uint.Parse(DataKeeper.GetValue("F2_COW_RATECHART"));

                        ActiveFrameInfo.WhichDataInFrame = 55;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(0, 2, 55, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(0, 2, 55, 0));
                        

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);

                        break;
                    }
                case 20204:
                    {
                        ActiveFrameInfo.ActiveX = 0;
                        ActiveFrameInfo.ActiveY = 2;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        ActiveFrameInfo.whichRatechart = uint.Parse(DataKeeper.GetValue("F2_BUF_RATECHART"));

                        ActiveFrameInfo.WhichDataInFrame = 55;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(0, 2, 55, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(0, 2, 55, 0));


                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);

                        break;
                    }
                case 2010310:
                    {
                        ActiveFrameInfo.ActiveX = 5;
                        ActiveFrameInfo.ActiveY = 3;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        TempCalculationVariable.FATmin = double.Parse(ActiveFrameInfo.float1_0data);
                        TempCalculationVariable.FATmax = double.Parse(ActiveFrameInfo.float3_0data);
                        TempCalculationVariable.CLRmin = int.Parse(ActiveFrameInfo.integer2_0);
                        TempCalculationVariable.CLRmax = int.Parse(ActiveFrameInfo.integer2_1);
                       
                        TempCalculationVariable.FATminFIX = TempCalculationVariable.FATmin;
                        TempCalculationVariable.CLRminFIX = TempCalculationVariable.CLRmin;

                        ActiveFrameInfo.WhichDataInFrame = 23;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(5, 3, 23, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 3, 23, 0));


                        KeyboardMode = 4;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);

                        break;
                    }
                case 2010410:
                    {
                        ActiveFrameInfo.ActiveX = 5;
                        ActiveFrameInfo.ActiveY = 3;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        TempCalculationVariable.FATmin = double.Parse(ActiveFrameInfo.float1_0data);
                        TempCalculationVariable.FATmax = double.Parse(ActiveFrameInfo.float3_0data);
                        TempCalculationVariable.CLRmin = int.Parse(ActiveFrameInfo.integer2_0);
                        TempCalculationVariable.CLRmax = int.Parse(ActiveFrameInfo.integer2_1);

                        TempCalculationVariable.FATminFIX = TempCalculationVariable.FATmin;
                        TempCalculationVariable.CLRminFIX = TempCalculationVariable.CLRmin;

                        ActiveFrameInfo.WhichDataInFrame = 23;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(5, 3, 23, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 3, 23, 0));


                        KeyboardMode = 4;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);

                        break;
                    }
                case 2010510:
                    {
                        ActiveFrameInfo.ActiveX = 5;
                        ActiveFrameInfo.ActiveY = 3;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        TempCalculationVariable.FATmin = double.Parse(ActiveFrameInfo.float1_0data);
                        TempCalculationVariable.FATmax = double.Parse(ActiveFrameInfo.float3_0data);
                        TempCalculationVariable.SNFmin = double.Parse(ActiveFrameInfo.float1_1data);
                        TempCalculationVariable.SNFmax = double.Parse(ActiveFrameInfo.float3_1data);

                        TempCalculationVariable.SNFminFIX = TempCalculationVariable.SNFmin;
                        TempCalculationVariable.FATminFIX = TempCalculationVariable.FATmin;

                        ActiveFrameInfo.WhichDataInFrame = 23;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(5, 3, 23, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 3, 23, 0));


                        KeyboardMode = 4;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);

                        break;
                    }
                case 2010610:
                    {
                        ActiveFrameInfo.ActiveX = 5;
                        ActiveFrameInfo.ActiveY = 3;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        TempCalculationVariable.FATmin = double.Parse(ActiveFrameInfo.float1_0data);
                        TempCalculationVariable.FATmax = double.Parse(ActiveFrameInfo.float3_0data);
                        TempCalculationVariable.SNFmin = double.Parse(ActiveFrameInfo.float1_1data);
                        TempCalculationVariable.SNFmax = double.Parse(ActiveFrameInfo.float3_1data);

                        TempCalculationVariable.SNFminFIX = TempCalculationVariable.SNFmin;
                        TempCalculationVariable.FATminFIX = TempCalculationVariable.FATmin;

                        ActiveFrameInfo.WhichDataInFrame = 23;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(5, 3, 23, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 3, 23, 0));


                        KeyboardMode = 4;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);

                        break;
                    }
                case 2010710:
                    {
                        ActiveFrameInfo.ActiveX = 5;
                        ActiveFrameInfo.ActiveY = 3;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        TempCalculationVariable.FATmin = double.Parse(ActiveFrameInfo.float1_0data);
                        TempCalculationVariable.FATmax = double.Parse(ActiveFrameInfo.float3_0data);
                        

                        TempCalculationVariable.FATminFIX = TempCalculationVariable.FATmin;
                        

                        ActiveFrameInfo.WhichDataInFrame = 23;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(5, 3, 23, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 3, 23, 0));


                        KeyboardMode = 4;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);

                        break;
                    }
                case 2010810:
                    {
                        ActiveFrameInfo.ActiveX = 5;
                        ActiveFrameInfo.ActiveY = 3;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        TempCalculationVariable.FATmin = double.Parse(ActiveFrameInfo.float1_0data);
                        TempCalculationVariable.FATmax = double.Parse(ActiveFrameInfo.float3_0data);


                        TempCalculationVariable.FATminFIX = TempCalculationVariable.FATmin;


                        ActiveFrameInfo.WhichDataInFrame = 23;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(5, 3, 23, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 3, 23, 0));


                        KeyboardMode = 4;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);

                        break;
                    }
                case 202080101:
                    {
                        ActiveFrameInfo.ActiveX = 7;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        

                        ActiveFrameInfo.float3_1data = "";
                        ActiveFrameInfo.float3_0data = "";
                        ActiveFrameInfo.float6_0data = "";


                        ActiveFrameInfo.float3_1index = 0;
                        ActiveFrameInfo.float3_0index = 0;
                        ActiveFrameInfo.float6_0index = 0;



                        ActiveFrameInfo.WhichDataInFrame = 26;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(7, 1, 26, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(7, 1, 26, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(15, 1, 22, 1));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(9, 2, 30, 2));

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);

                        break;
                    }
                case 204:
                    {
                        ActiveFrameInfo.ActiveX = 11;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;
                        ActiveFrameInfo.passwordGlobal = "";
                        ActiveFrameInfo.passwordGlobalReal = "";


                        ActiveFrameInfo.WhichDataInFrame = 65;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(11, 1, 65, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(11, 1, 65, 0));


                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        LCDrefreshFlag = true;
                        LCDFunctions.gotoLCD(int.Parse(ActiveFrameInfo.ActiveX.ToString()), int.Parse(ActiveFrameInfo.ActiveY.ToString()));
                        LCDFunctions.CursorON_OFF(true);
                        break;
                    }
                case 501:
                    {
                        ActiveFrameInfo.ActiveX = 4;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        ActiveFrameInfo.float4_0data = "";
                        ActiveFrameInfo.float4_1data = "";
                        ActiveFrameInfo.float6_0data = "";
                        ActiveFrameInfo.float5_0data = "000.00";
                        ActiveFrameInfo.float7_0data = "00000.00";

                        ActiveFrameInfo.float4_0index = 0;
                        ActiveFrameInfo.float4_1index = 0;
                        ActiveFrameInfo.float6_0index = 0;
                        ActiveFrameInfo.float5_0index = 0;
                        ActiveFrameInfo.float7_0index = 0;

                        ActiveFrameInfo.whichShift = 0;

                        ActiveFrameInfo.WhichDataInFrame = 23;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(4, 1, 23, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(4, 1, 23, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(14, 1, 27, 1));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(3, 2, 30, 2));


                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                        LCDFunctions.CursorON_OFF(true);
                        break;
                    }
                case 502:
                    {
                        ActiveFrameInfo.ActiveX = 4;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        ActiveFrameInfo.float4_0data = "00.00";
                        ActiveFrameInfo.float4_1data = "00.00";
                        ActiveFrameInfo.float6_0data = "0000.00";
                        ActiveFrameInfo.float5_0data = "000.00";
                        ActiveFrameInfo.float7_0data = "00000.00";

                        ActiveFrameInfo.float4_0index = 0;
                        ActiveFrameInfo.float4_1index = 0;
                        ActiveFrameInfo.float6_0index = 0;
                        ActiveFrameInfo.float5_0index = 0;
                        ActiveFrameInfo.float7_0index = 0;

                        ActiveFrameInfo.whichShift = 0;

                        ActiveFrameInfo.WhichDataInFrame = 500;

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        


                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        //LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);
                        LCDFunctions.CursorON_OFF(false);
                        break;
                    }
                case 80801:
                    {
                        ActiveFrameInfo.ActiveX = 0;
                        ActiveFrameInfo.ActiveY = 0;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        ActiveFrameInfo.SearchData.Clear();

                        ActiveFrameInfo.SearchDataDisplay[0] = new SearchDataColumn("", "", "");
                        ActiveFrameInfo.SearchDataDisplay[1] = new SearchDataColumn("", "", "");
                        ActiveFrameInfo.SearchDataDisplay[2] = new SearchDataColumn("", "", "");
                        ActiveFrameInfo.SearchDataDisplay[3] = new SearchDataColumn("", "", "");

                        ActiveFrameInfo.SearchDataDisplayIndex = 0;
                        ActiveFrameInfo.SearchDataIndex = 0;

                        try
                        {
                            if (FrameBase == 80801)
                            {

                                var Ret = ExcelFileHandler.Sale_SearchAll(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, true);
                                if (Ret != null)
                                {

                                    foreach (SaleSheetOneEntry s in Ret)
                                    {
                                        ActiveFrameInfo.SearchData.Add(new SearchDataColumn(s.Serial, s.time, s.amount));
                                    }
                                }

                            }
                            else if (FrameBase == 80802)
                            {
                                var Ret = ExcelFileHandler.Sale_SearchAll(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, false);

                                if (Ret != null)
                                {

                                    foreach (SaleSheetOneEntry s in Ret)
                                    {
                                        ActiveFrameInfo.SearchData.Add(new SearchDataColumn(s.Serial, s.time, s.amount));
                                    }
                                }
                            }
                            else if (FrameBase == 8080310)
                            {
                                var Ret = ExcelFileHandler.Sale_SearchAll(int.Parse(ActiveFrameInfo.day), int.Parse(ActiveFrameInfo.month), int.Parse(ActiveFrameInfo.year)+2000, true);

                                if (Ret != null)
                                {

                                    foreach (SaleSheetOneEntry s in Ret)
                                    {
                                        ActiveFrameInfo.SearchData.Add(new SearchDataColumn(s.Serial, s.time, s.amount));
                                    }
                                }
                            }
                            else if (FrameBase == 8080410)
                            {
                                var Ret = ExcelFileHandler.Sale_SearchAll(int.Parse(ActiveFrameInfo.day), int.Parse(ActiveFrameInfo.month), int.Parse(ActiveFrameInfo.year)+2000, false);

                                if (Ret != null)
                                {

                                    foreach (SaleSheetOneEntry s in Ret)
                                    {
                                        ActiveFrameInfo.SearchData.Add(new SearchDataColumn(s.Serial, s.time, s.amount));
                                    }
                                }
                            }
                            else if (FrameBase == 80805)
                            {
                                var Ret = ExcelFileHandler.Sale_SearchLast10(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, true);

                                if (Ret != null)
                                {

                                    foreach (SaleSheetOneEntry s in Ret)
                                    {
                                        ActiveFrameInfo.SearchData.Add(new SearchDataColumn(s.Serial, s.time, s.amount));
                                    }
                                }
                            }
                            else if (FrameBase == 80806)
                            {
                                var Ret = ExcelFileHandler.Sale_SearchLast10(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, false);

                                if (Ret != null)
                                {

                                    foreach (SaleSheetOneEntry s in Ret)
                                    {
                                        ActiveFrameInfo.SearchData.Add(new SearchDataColumn(s.Serial, s.time, s.amount));
                                    }
                                }
                            }
                            else if (FrameBase == 10410) {
                                var Ret = ProcessData.GetAllMemberDetail_New();
                                if (Ret != null)
                                {

                                    foreach (EachMemberDetail s in Ret)
                                    {
                                        ActiveFrameInfo.SearchData.Add(new SearchDataColumn(s.ID.ToString(), s.Name, ""));
                                    }
                                }
                            }
                        }
                        catch (Exception ex) { }



                        ActiveFrameInfo.WhichDataInFrame = 66;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(0, 0, 66, 0);



                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(0, 0, 66, 0));

                        ActiveFrameInfo.UpdateInfo = "Down";

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        break;
                    }

                case 80701:
                    {
                        ActiveFrameInfo.ActiveX = 13;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        ActiveFrameInfo.float5_1data = DataKeeper.GetValue("LOCAL_BUF_RATE");
                        ActiveFrameInfo.float5_0data = DataKeeper.GetValue("LOCAL_COW_RATE");

                        ActiveFrameInfo.float5_1index = 0;
                        ActiveFrameInfo.float5_0index = 0;


                        ActiveFrameInfo.WhichDataInFrame = 29;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(13, 1, 29, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(13, 1, 29, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(13, 2, 32, 1));

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);

                        break;
                    }
                case 80702:
                    {
                        ActiveFrameInfo.ActiveX = 13;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        ActiveFrameInfo.float5_1data = DataKeeper.GetValue("TRUCK_BUF_RATE");
                        ActiveFrameInfo.float5_0data = DataKeeper.GetValue("TRUCK_COW_RATE");

                        ActiveFrameInfo.float5_1index = 0;
                        ActiveFrameInfo.float5_0index = 0;


                        ActiveFrameInfo.WhichDataInFrame = 29;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(13, 1, 29, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(13, 1, 29, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(13, 2, 32, 1));

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);

                        break;
                    }
                case 80803:
                    {
                        ActiveFrameInfo.ActiveX = 5;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        ActiveFrameInfo.day = "";
                        ActiveFrameInfo.month = "";
                        ActiveFrameInfo.year = "";

                        ActiveFrameInfo.WhichDataInFrame = 4;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(5, 1, 4, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(5, 1, 4, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(8, 1, 5, 1));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(11, 1, 6, 2));

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);

                        break;
                    }
                case 803:
                    {
                        ActiveFrameInfo.ActiveX = 8;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        ActiveFrameInfo.day = "";
                        ActiveFrameInfo.month = "";
                        ActiveFrameInfo.year = "";

                        ActiveFrameInfo.integer5_0 = "";

                        ActiveFrameInfo.WhichDataInFrame = 49;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(8, 1, 49, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(8, 1, 49, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(6, 2, 4, 1));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(9, 2, 5, 2));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(12, 2, 6, 3));

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);

                        break;
                    }
                case 805:
                    {
                        ActiveFrameInfo.ActiveX = 6;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        ActiveFrameInfo.day = "";
                        ActiveFrameInfo.month = "";
                        ActiveFrameInfo.year = "";

                        

                        ActiveFrameInfo.WhichDataInFrame = 4;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(6, 1, 4, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(6, 1, 4, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(9, 1, 5, 1));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(12, 1, 6, 2));

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);

                        break;
                    }
                case 1001:
                    {
                        ActiveFrameInfo.ActiveX = 6;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        ActiveFrameInfo.float7_0data = "";
                        ActiveFrameInfo.float7_0index = 0;

                        ActiveFrameInfo.FrameData_UserID = 0;

                        ActiveFrameInfo.WhichDataInFrame = 1;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(6, 1, 1, 0);


                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(6, 1, 1, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(8, 2, 31, 1));

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;
                        break;
                    }
                case 100301:
                    {
                        ActiveFrameInfo.ActiveX = 8;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        ActiveFrameInfo.day = "";
                        ActiveFrameInfo.month = "";
                        ActiveFrameInfo.year = "";

                        ActiveFrameInfo.integer3_0 = "";

                        ActiveFrameInfo.WhichDataInFrame = 47;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(8, 1, 47, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(8, 1, 47, 0));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(6, 2, 4, 1));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(9, 2, 5, 2));
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(12, 2, 6, 3));

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);

                        break;
                    }
                case 100302:
                    {
                        ActiveFrameInfo.ActiveX = 8;
                        ActiveFrameInfo.ActiveY = 1;
                        ActiveFrameInfo.IsInput = true;
                        ActiveFrameInfo.FrameBase = FrameBase;

                        

                        ActiveFrameInfo.integer3_0 = "";

                        ActiveFrameInfo.WhichDataInFrame = 47;
                        ActiveFrameInfo.ActiveFrameInput = new FrameInputInfo(8, 1, 47, 0);

                        ActiveFrameInfo.InputList.Clear();
                        ActiveFrameInfo.InputList = new List<FrameInputInfo>();
                        ActiveFrameInfo.InputList.Add(new FrameInputInfo(8, 1, 47, 0));
                       

                        KeyboardMode = 2;
                        keyboardNumMode = true;
                        keyboardMaxChar = 3;

                        LCDFunctions.gotoLCD(ActiveFrameInfo.LCD_X, ActiveFrameInfo.LCD_Y);

                        break;
                    }
            }
        }
    }
}
