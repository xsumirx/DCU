using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using Utility;
using System.IO;
using System.IO.Ports;


namespace LCD16x2
{



    public partial class Form1 : Form
    {

        /// <summary>
        /// KeyboardMode = 0 i.e; Command Mode
        /// KeyboardMode = 1 i.e; Char Mode
        /// KeyboardMode = 2 i.e; String Mode
        /// </summary>


        [DllImport("Coredll.dll")]
        extern static void GwesPowerOffSystem();

        const ulong Megabyte = 1048576;
        const ulong Gigabyte = 1073741824;

        [DllImport("coredll.dll")]
        static extern int GetDiskFreeSpaceEx(
        string DirectoryName,
        out ulong lpFreeBytesAvailableToCaller,
        out ulong lpTotalNumberOfBytes,
        out ulong lpTotalNumberOfFreeBytes);
        

        public UInt64 stage = 0;
        public UInt64 F_state = 0;
        public uint F1_state = 0;
        public uint F2_state = 0;
        public uint F4_state = 0;
        public uint F8_state = 0;
        public uint F11_state = 0;
        public int key_press = 0;
        /// <summary>
        /// Frame Area
        /// </summary>

        public bool shouldChangeDatabase = false;
        public bool shouldChangeDataSetting = false;


        public bool ManualorAutoMode = true; //True for Manual and False for Auto
       
        
        public Stack<UInt64> FrameBaseHistory = new Stack<UInt64>();
        public Stack<UInt64> FrameIdHistory = new Stack<UInt64>();

        public static bool SD_Card_Status = false;

        public UInt64 KeyboardtoFrameID(string str) {
            

             //Frame ID
            switch (str) {
                case "F1":
                    if (key_press == 0)
                    {
                        F_state = 1;
                        key_press = 1;
                        stage = 1;
                        FrameBaseHistory.Push(stage);

                    }
                    break;

                case "F2":
                    if (key_press == 0)
                    {
                        F_state = 2;
                        key_press = 1;
                        stage = 2;
                        FrameBaseHistory.Push(stage);
                    }
                    break;

                case "F3":
                    if (key_press == 0)
                    {
                        F_state = 3;
                        key_press = 1;
                        stage = 3;
                        FrameBaseHistory.Push(stage);

                    }
                    break;

                case "F4":
                    if (key_press == 0)
                    {
                        F_state = 4;
                        key_press = 1;
                        stage = 4;
                        FrameBaseHistory.Push(stage);

                    }
                    break;

                case "F5":
                    if (key_press == 0)
                    {
                        F_state = 5;
                        key_press = 1;
                        stage = 5;
                        FrameBaseHistory.Push(stage);

                    }
                    break;

                case "F6":
                    if (key_press == 0)
                    {
                        F_state = 6;
                        key_press = 1;
                        stage = 6;
                        FrameBaseHistory.Push(stage);

                    }
                    break;

                case "F7":
                    if (key_press == 0)
                    {
                        F_state = 7;
                        key_press = 1;
                        stage = 7;
                        FrameBaseHistory.Push(stage);

                    }
                    break;

                case "F8":
                    if (key_press == 0)
                    {
                        F_state = 8;
                        key_press = 1;
                        stage = 8;
                        FrameBaseHistory.Push(stage);

                    }
                    break;

                case "F9":
                    if (key_press == 0)
                    {
                        F_state = 9;
                        key_press = 1;
                        stage = 9;
                        FrameBaseHistory.Push(stage);

                    }
                    break;

                case "F10":
                    if (key_press == 0)
                    {
                        F_state = 10;
                        key_press = 1;
                        stage = 10;
                        FrameBaseHistory.Push(stage);

                    }
                    break;

                case "F11":
                    if (key_press == 0)
                    {
                        F_state = 11;
                        key_press = 1;
                        stage = 11;
                        FrameBaseHistory.Push(stage);


                    }
                    break;

                case "F12":
                    if (key_press == 0)
                    {
                        F_state = 12;
                        key_press = 1;
                        stage = 12;
                        FrameBaseHistory.Push(stage);


                    }
                    break;

                case "D1":{
                    if (key_press == 1 || F_state > 100)
                    {
                        key_press ++;
                        stage = (100 * F_state) + 1;
                        F_state = stage;
                        FrameBaseHistory.Push(stage);

                    }
                    break;
                }
                    
                case "D2":
                {
                    if (key_press == 1 || F_state > 100)
                    {
                        
                        stage = (100 * F_state) + 2;
                        key_press++;
                        F_state = stage;
                        FrameBaseHistory.Push(stage);
                    }
                    break;
                }
                case "D3":
                {
                    if (key_press == 1 || F_state > 100)
                    {
                        
                        stage = (100 * F_state) + 3;
                        key_press++;
                        F_state = stage;
                        FrameBaseHistory.Push(stage);
                    }
                    break;
                }
                case "D4":
                {
                    if (key_press == 1 || F_state > 100)
                    {
                        
                        stage = (100 * F_state) + 4;
                        key_press++;
                        F_state = stage;
                        FrameBaseHistory.Push(stage);
                    }
                    break;
                }
                case "D5":
                {
                    if (key_press == 1 || F_state > 100)
                    {
                        
                        stage = (100 * F_state) + 5;
                        key_press++;
                        F_state = stage;
                        FrameBaseHistory.Push(stage);
                    }
                    break;
                }
                case "D6":
                {
                    if (key_press == 1 || F_state > 100)
                    {
                        
                        stage = (100 * F_state) + 6;
                        key_press++;
                        F_state = stage;
                        FrameBaseHistory.Push(stage);
                    }
                    break;
                }
                case "D7":
                {
                    if (key_press == 1 || F_state > 100)
                    {
                        
                        stage = (100 * F_state) + 7;
                        key_press++;
                        F_state = stage;
                        FrameBaseHistory.Push(stage);
                    }
                    break;

                }
                case "D8":
                    {
                        if (key_press == 1 || F_state > 100)
                        {
                            
                            stage = (100 * F_state) + 8;
                            key_press++;
                            F_state = stage;
                            FrameBaseHistory.Push(stage);
                        }
                        break;
                    }
                case "D9":
                    {
                        if (key_press == 1 || F_state > 100)
                        {
                            
                            stage = (100 * F_state) + 9;
                            key_press++;
                            F_state = stage;
                            FrameBaseHistory.Push(stage);
                        }
                        break;
                    }

                case "Delete": {
                    key_press = 0;
                    F_state = 0;
                    stage = 0;
                    FrameBaseHistory.Clear();
                    break;
                }
                case "Escape":
                    {
                        key_press --;
                        if (key_press <= 0) { 
                            key_press = 0;   
                        }
                        
                        if(F_state > 100){
                        	F_state /= 100;
                        }

                        if(FrameBaseHistory.Count > 0)
                            FrameBaseHistory.Pop();

                        if (FrameBaseHistory.Count > 0)
                        {
                            stage = FrameBaseHistory.Pop();
                            FrameBaseHistory.Push(stage);
                        }
                        else {
                            stage = 0;
                        }

                        
                        break;
                    }
                case "Ret": {
                    
                    if (LCDFrameProcessor.FrameBase == 101) {
                        //LCDFrameProcessor.startOperation(29850);
                        if (!ProcessData.IsSpaceAvailable(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID))
                        {   //No User  Doesn't Exist ..Want to Create;
                            FrameBaseHistory.Push(10110);
                            stage = 10110;
                            key_press++;
                        }
                        else {
                            //User Exist..Want to Delete
                            FrameBaseHistory.Push(10120);
                            stage = 10120;
                            key_press++;
                        }
                    }
                    else if (LCDFrameProcessor.FrameBase == 10112)
                    {
                        //New Member Added.....After Few MS go to 101
                        FrameBaseHistory.Push(10113);
                        stage = 10113;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 102) {
                        if (!ProcessData.IsSpaceAvailable(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID))
                        {   //No User  Doesn't Exist ..Want to Create;
                            FrameBaseHistory.Push(10110);
                            stage = 10110;
                            key_press++;
                        }
                        else
                        {
                            //User Exist..Modify
                            //Detail Page
                            FrameBaseHistory.Push(10210);
                            stage = 10210;
                            key_press++;
                        }
                    }
                    else if (LCDFrameProcessor.FrameBase == 10210) {
                        //Added ....After few ms Go
                        FrameBaseHistory.Push(10211);
                        stage = 10211;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 3)
                    {
                        //Date and Time Updated
                        FrameBaseHistory.Push(301);
                        stage = 301;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 5)
                    {
                        if (!LCDFrameProcessor.F5_LOCK_STATUS)
                        {
                            FrameBaseHistory.Push(504);
                            F_state = 504;
                            stage = 504;
                            key_press++;
                        }
                        
                    }
                    else if (LCDFrameProcessor.FrameBase == 6)
                    {
                            FrameBaseHistory.Push(601);
                            
                            stage = 601;
                            key_press++;

                    }
                    else if (LCDFrameProcessor.FrameBase == 7) {
                        FrameBaseHistory.Push(701);
                        stage = 701;
                        key_press++;
                    }
                    
                    else if (LCDFrameProcessor.FrameBase == 9)
                    {
                        FrameBaseHistory.Push(901);
                        stage = 901;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 204)
                    {
                        FrameBaseHistory.Push(20410);
                        stage = 20410;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 40110)
                    {
                        //Do Change Operation and if sucessfull return 40111 and make comitt flag  true;
                        //if change unsuessfull return 40112 and make comitt flag false
                        FrameBaseHistory.Push(40111);
                        stage = 40111;
                        key_press++;

                    }
                    else if (LCDFrameProcessor.FrameBase == 402)
                    {
                        FrameBaseHistory.Push(40210);
                        stage = 40210;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 403)
                    {
                        FrameBaseHistory.Push(40310);
                        stage = 40310;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 404)
                    {
                        FrameBaseHistory.Push(40410);
                        stage = 40410;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 405)
                    {
                        FrameBaseHistory.Push(40510);
                        stage = 40510;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 501 || LCDFrameProcessor.FrameBase == 502)
                    {
                        FrameBaseHistory.Push(50110);
                        stage = 50110;
                        key_press++;
                    }

                    else if (LCDFrameProcessor.FrameBase == 802)
                    {
                        FrameBaseHistory.Push(80210);
                        stage = 80210;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 801)
                    {
                        FrameBaseHistory.Push(80110);
                        stage = 80110;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 803)
                    {
                        FrameBaseHistory.Push(80310);
                        stage = 80310;
                        F_state = stage;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 804)
                    {
                        FrameBaseHistory.Push(80410);
                        stage = 80410;
                        F_state = stage;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 805)
                    {
                        FrameBaseHistory.Push(80510);
                        stage = 80510;
                        F_state = stage;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 806)
                    {
                        FrameBaseHistory.Push(80610);
                        stage = 80610;
                        F_state = stage;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 80701)
                    {
                        FrameBaseHistory.Push(8070110);
                        stage = 8070110;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 80702)
                    {
                        FrameBaseHistory.Push(8070210);
                        stage = 8070210;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 80803)
                    {
                        FrameBaseHistory.Push(8080310);
                        stage = 8080310;
                        F_state = stage;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 80804)
                    {
                        FrameBaseHistory.Push(8080410);
                        stage = 8080410;
                        F_state = stage;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 901)
                    {
                        FrameBaseHistory.Push(90110);
                        stage = 90110;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 1001)
                    {
                        FrameBaseHistory.Push(100110);
                        stage = 100110;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 1002)
                    {
                        FrameBaseHistory.Push(100210);
                        stage = 100210;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 100301)
                    {
                        FrameBaseHistory.Push(10030110);
                        stage = 10030110;
                        F_state = stage;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 100401)
                    {
                        FrameBaseHistory.Push(10040110);
                        stage = 10040110;
                        F_state = stage;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 100302)
                    {
                        FrameBaseHistory.Push(10030210);
                        stage = 10030210;
                        F_state = stage;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 1005)
                    {
                        FrameBaseHistory.Push(100510);
                        stage = 100510;
                        F_state = stage;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 40610)
                    {
                        //Do Change Operation and if sucessfull return 40611 and make comitt flag  true;
                        //if change unsuessfull return 40612 and make comitt flag false
                        FrameBaseHistory.Push(40611);
                        stage = 40611;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 1101)
                    {

                        FrameBaseHistory.Push(110110);
                        stage = 110110;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 1102)
                    {

                        FrameBaseHistory.Push(110210);
                        stage = 110210;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 110110)
                    {

                        FrameBaseHistory.Push(110111);
                        stage = 110111;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 1204)
                    {

                        FrameBaseHistory.Push(120410);
                        stage = 120410;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 120101)
                    {

                        FrameBaseHistory.Push(12010110);
                        stage = 12010110;
                        key_press++;
                    }

                    else if (LCDFrameProcessor.FrameBase == 120102)
                    {

                        FrameBaseHistory.Push(12010210);
                        stage = 12010210;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 120103)
                    {

                        FrameBaseHistory.Push(12010310);
                        stage = 12010310;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 120201)
                    {

                        FrameBaseHistory.Push(12020110);
                        stage = 12020110;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 120301)
                    {

                        FrameBaseHistory.Push(12020110);
                        stage = 12020110;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 120302)
                    {

                        FrameBaseHistory.Push(12020110);
                        stage = 12020110;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 120303)
                    {

                        FrameBaseHistory.Push(12020110);
                        stage = 12020110;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 120304)
                    {
                        if (Directory.Exists(@"\USB HD"))
                        {
                            FrameBaseHistory.Push(12030410);
                            stage = 12030410;
                            key_press++;
                        }
                        else
                        {
                            FrameBaseHistory.Push(12030411);
                            stage = 12030411;
                            key_press++;
                        }


                    }
                    else if (LCDFrameProcessor.FrameBase == 20101)
                    {

                        FrameBaseHistory.Push(2010110);
                        stage = 2010110;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 20102)
                    {

                        FrameBaseHistory.Push(2010210);
                        stage = 2010210;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 20103)
                    {
                        if (Math.Round(double.Parse(LCDFrameProcessor.ActiveFrameInfo.float1_0data), 2) > Math.Round(double.Parse(LCDFrameProcessor.ActiveFrameInfo.float3_0data), 2) || int.Parse(LCDFrameProcessor.ActiveFrameInfo.integer2_0) > int.Parse(LCDFrameProcessor.ActiveFrameInfo.integer2_1))
                        {
                            FrameBaseHistory.Push(2010511);
                            stage = 2010511;
                            key_press++;
                        }
                        else
                        {
                            FrameBaseHistory.Push(2010310);
                            stage = 2010310;
                            key_press++;
                        }
                    }
                    else if (LCDFrameProcessor.FrameBase == 20104)
                    {
                        if (Math.Round(double.Parse(LCDFrameProcessor.ActiveFrameInfo.float1_0data), 2) > Math.Round(double.Parse(LCDFrameProcessor.ActiveFrameInfo.float3_0data), 2) || int.Parse(LCDFrameProcessor.ActiveFrameInfo.integer2_0) > int.Parse(LCDFrameProcessor.ActiveFrameInfo.integer2_1))
                        {
                            FrameBaseHistory.Push(2010511);
                            stage = 2010511;
                            key_press++;
                        }
                        else
                        {
                            FrameBaseHistory.Push(2010410);
                            stage = 2010410;
                            key_press++;
                        }
                    }
                    else if (LCDFrameProcessor.FrameBase == 20105)
                    {
                        if (Math.Round(double.Parse(LCDFrameProcessor.ActiveFrameInfo.float1_0data), 2) > Math.Round(double.Parse(LCDFrameProcessor.ActiveFrameInfo.float3_0data), 2) || Math.Round(double.Parse(LCDFrameProcessor.ActiveFrameInfo.float1_1data), 2) > Math.Round(double.Parse(LCDFrameProcessor.ActiveFrameInfo.float3_1data), 2))
                        {
                            FrameBaseHistory.Push(2010511);
                            stage = 2010511;
                            key_press++;
                        }
                        else
                        {
                            FrameBaseHistory.Push(2010510);
                            stage = 2010510;
                            key_press++;
                        }
                    }
                    else if (LCDFrameProcessor.FrameBase == 20106)
                    {
                        if (Math.Round(double.Parse(LCDFrameProcessor.ActiveFrameInfo.float1_0data), 2) > Math.Round(double.Parse(LCDFrameProcessor.ActiveFrameInfo.float3_0data), 2) || Math.Round(double.Parse(LCDFrameProcessor.ActiveFrameInfo.float1_1data), 2) > Math.Round(double.Parse(LCDFrameProcessor.ActiveFrameInfo.float3_1data), 2))
                        {
                            FrameBaseHistory.Push(2010511);
                            stage = 2010511;
                            key_press++;
                        }
                        else
                        {
                            FrameBaseHistory.Push(2010610);
                            stage = 2010610;
                            key_press++;
                        }


                    }
                    else if (LCDFrameProcessor.FrameBase == 20107)
                    {
                        if (Math.Round(double.Parse(LCDFrameProcessor.ActiveFrameInfo.float1_0data), 2) > Math.Round(double.Parse(LCDFrameProcessor.ActiveFrameInfo.float3_0data), 2))
                        {
                            FrameBaseHistory.Push(2010511);
                            stage = 2010511;
                            key_press++;
                        }
                        else
                        {
                            FrameBaseHistory.Push(2010710);
                            stage = 2010710;
                            key_press++;
                        }
                    }
                    else if (LCDFrameProcessor.FrameBase == 20108)
                    {
                        if (Math.Round(double.Parse(LCDFrameProcessor.ActiveFrameInfo.float1_0data), 2) > Math.Round(double.Parse(LCDFrameProcessor.ActiveFrameInfo.float3_0data), 2))
                        {
                            FrameBaseHistory.Push(2010511);
                            stage = 2010511;
                            key_press++;
                        }
                        else
                        {
                            FrameBaseHistory.Push(2010810);
                            stage = 2010810;
                            key_press++;
                        }
                    }
                    else if (LCDFrameProcessor.FrameBase == 20201)
                    {
                        FrameBaseHistory.Push(2020110);
                        stage = 2020110;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 20202)
                    {
                        FrameBaseHistory.Push(2020210);
                        stage = 2020210;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 20203)
                    {
                        FrameBaseHistory.Push(2020310);
                        stage = 2020310;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 20204)
                    {
                        FrameBaseHistory.Push(2020410);
                        stage = 2020410;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 20205)
                    {
                        FrameBaseHistory.Push(2020510);
                        stage = 2020510;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 20206)
                    {
                        FrameBaseHistory.Push(2020610);
                        stage = 2020610;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 20207)
                    {
                        FrameBaseHistory.Push(2020710);
                        stage = 2020710;
                        key_press++;
                    }
                    else if (
                                LCDFrameProcessor.FrameBase == 202080805 ||
                                LCDFrameProcessor.FrameBase == 202080804 ||
                                LCDFrameProcessor.FrameBase == 202080803 ||
                                LCDFrameProcessor.FrameBase == 202080802 ||
                                LCDFrameProcessor.FrameBase == 202080801 ||

                                LCDFrameProcessor.FrameBase == 202080705 ||
                                LCDFrameProcessor.FrameBase == 202080704 ||
                                LCDFrameProcessor.FrameBase == 202080703 ||
                                LCDFrameProcessor.FrameBase == 202080702 ||
                                LCDFrameProcessor.FrameBase == 202080701 ||

                                LCDFrameProcessor.FrameBase == 202080605 ||
                                LCDFrameProcessor.FrameBase == 202080604 ||
                                LCDFrameProcessor.FrameBase == 202080603 ||
                                LCDFrameProcessor.FrameBase == 202080602 ||
                                LCDFrameProcessor.FrameBase == 202080601 ||

                                LCDFrameProcessor.FrameBase == 202080505 ||
                                LCDFrameProcessor.FrameBase == 202080504 ||
                                LCDFrameProcessor.FrameBase == 202080503 ||
                                LCDFrameProcessor.FrameBase == 202080502 ||
                                LCDFrameProcessor.FrameBase == 202080501 ||

                                LCDFrameProcessor.FrameBase == 202080405 ||
                                LCDFrameProcessor.FrameBase == 202080404 ||
                                LCDFrameProcessor.FrameBase == 202080403 ||
                                LCDFrameProcessor.FrameBase == 202080402 ||
                                LCDFrameProcessor.FrameBase == 202080401 ||

                                LCDFrameProcessor.FrameBase == 202080305 ||
                                LCDFrameProcessor.FrameBase == 202080304 ||
                                LCDFrameProcessor.FrameBase == 202080303 ||
                                LCDFrameProcessor.FrameBase == 202080302 ||
                                LCDFrameProcessor.FrameBase == 202080301 ||

                                LCDFrameProcessor.FrameBase == 202080205 ||
                                LCDFrameProcessor.FrameBase == 202080204 ||
                                LCDFrameProcessor.FrameBase == 202080203 ||
                                LCDFrameProcessor.FrameBase == 202080202 ||
                                LCDFrameProcessor.FrameBase == 202080201 ||

                                LCDFrameProcessor.FrameBase == 202080105 ||
                                LCDFrameProcessor.FrameBase == 202080104 ||
                                LCDFrameProcessor.FrameBase == 202080103 ||
                                LCDFrameProcessor.FrameBase == 202080102 ||
                                LCDFrameProcessor.FrameBase == 202080101
                        )
                    {
                        FrameBaseHistory.Push(20208010110);
                        stage = 20208010110;
                        F_state = stage;
                        key_press++;
                    }
                    break;
                
                }
                case "Yes": {

                    if (LCDFrameProcessor.FrameBase == 10110) { 
                        //Created ...Want to Add New Member
                        FrameBaseHistory.Push(10111);
                        stage = 10111;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 10111) { 
                        //New Member Data Entry Frame
                        FrameBaseHistory.Push(10112);
                        stage = 10112;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 10120) { 
                        //Deleted and go to F1
                        FrameBaseHistory.Push(10121);
                        stage = 10121;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 103)
                    {
                        //Show all Member List
                        FrameBaseHistory.Push(10310);
                        stage = 10310;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 104)
                    {
                        //Show all Member List
                        FrameBaseHistory.Push(10410);
                        stage = 10410;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 105)
                    {
                        //Show all Member List
                        FrameBaseHistory.Push(10510);
                        stage = 10510;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 401) {
                        FrameBaseHistory.Push(40110);
                        stage = 40110;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 406) {
                        FrameBaseHistory.Push(40610);
                        stage = 40610;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 5)
                    {
                        if (LCDFrameProcessor.F5_LOCK_STATUS)
                        {
                            stage = 5;
                            ManualorAutoMode = true;
                        }
                    }
                    else if (LCDFrameProcessor.FrameBase == 110111)
                    {
                        FrameBaseHistory.Push(110112);
                        stage = 110112;
                        key_press++;
                    }
                    else if (LCDFrameProcessor.FrameBase == 110210)
                    {
                        FrameBaseHistory.Push(110211);
                        stage = 110211;
                        key_press++;
                    }
                    break;
                }
                case "No": {
                    if (LCDFrameProcessor.FrameBase == 10110)
                    {
                        //Created ...Want to Add New Member
                        FrameBaseHistory.Pop();
                        FrameBaseHistory.Pop();
                        stage = 1;
                        key_press = 1;
                    }
                    else if (LCDFrameProcessor.FrameBase == 10111)
                    {
                        //New Member Data Entry Frame
                        FrameBaseHistory.Pop();
                        FrameBaseHistory.Pop();
                        FrameBaseHistory.Pop();
                        stage = 1;
                        key_press = 1;
                    }
                    else if (LCDFrameProcessor.FrameBase == 10120)
                    {
                        //Deleted and go to F1
                        FrameBaseHistory.Pop();
                        FrameBaseHistory.Pop();
                        stage = 1;
                        key_press = 1;
                    }
                    else if (LCDFrameProcessor.FrameBase == 103)
                    {
                        //Back to 1
                        FrameBaseHistory.Pop();
                        FrameBaseHistory.Pop();
                        stage = 1;
                        key_press = 1;
                    }
                    else if (LCDFrameProcessor.FrameBase == 104)
                    {
                        //Back to 1
                        FrameBaseHistory.Pop();
                        FrameBaseHistory.Pop();
                        stage = 1;
                        key_press = 1;
                    }
                    else if (LCDFrameProcessor.FrameBase == 105)
                    {
                        //KeyboardtoFrameID("Escape");
                        UInt64 x = KeyboardtoFrameID("Escape");
                        stage = x;
                    }
                    else if (LCDFrameProcessor.FrameBase == 401) {
                        FrameBaseHistory.Pop();
                        stage = 4;
                        key_press = 1;
                    }
                    else if (LCDFrameProcessor.FrameBase == 406)
                    {
                        FrameBaseHistory.Pop();
                        stage = 4;
                        key_press = 1;
                    }
                    else if (LCDFrameProcessor.FrameBase == 5)
                    {
                        if (LCDFrameProcessor.F5_LOCK_STATUS)
                        {
                            stage = 5;
                            ManualorAutoMode = false;
                        }
                    }
                        
                    else if (LCDFrameProcessor.FrameBase == 110111)
                    {
                        KeyboardtoFrameID("Escape");
                        KeyboardtoFrameID("Escape");
                        //KeyboardtoFrameID("Escape");
                        UInt64 x = KeyboardtoFrameID("Escape");
                        stage = x;
                        
                    }
                    else if (LCDFrameProcessor.FrameBase == 110210)
                    {
                        KeyboardtoFrameID("Escape");
                        KeyboardtoFrameID("Escape");
                        //KeyboardtoFrameID("Escape");
                        UInt64 x = KeyboardtoFrameID("Escape");
                        stage = x;
                    }
                    break;
                }

                default: { break; }
            }
            textBox1.BeginInvoke((Action)delegate() { textBox1.Text += " " + FrameBaseHistory.Count.ToString() + " " + stage.ToString() + " :"; });
            KeyMatrix.stage = stage;
            return stage;
        }

        
        #region DllImports

        [DllImport("GPIOLib.dll", CharSet = CharSet.Auto)]
        public static extern bool SetPinAltFn(Int32 pinNum, Int32 altFn, bool dirOut);

        [DllImport("GPIOLib.dll", CharSet = CharSet.Auto)]
        public static extern void InitGPIOLib();

        [DllImport("GPIOLib.dll", CharSet = CharSet.Auto)]
        public static extern bool SetPinLevel(Int32 gpioNum, Int32 val);

        [DllImport("GPIOLib.dll", CharSet = CharSet.Auto)]
        public static extern void DeInitGPIOLib();




        #endregion

        #region PinDetailsofLCD

        /*
         *  D4 - 97
         *  D5 - 85
         *  D6 - 79
         *  D7 - 45
         * 
         * 
         *   EN - 103
         *   RS - 101
         */

        #endregion

        public Form1()
        {
            InitializeComponent();
            Load += new EventHandler(Form1_Load);
        }
        
        
        //For Clock on Desktop or HomePage
        Thread BackTimer;
        System.Threading.Timer Thread_BackLoop;
        public event TimerCallback Thread_BackLoop_Delegate;

        public static bool AppRunningStatus = true;

        public delegate void UpdateUI(string message);
        public event UpdateUI uui;

        public delegate void CommandDetected(string keyMessage);
        public event CommandDetected SuscribeCommandDetected;

        public delegate void CharDetected(string keyMessage);
        public event CharDetected SuscribeCharDetected;

        public delegate void StringDetected(string keyMessage);
        public event StringDetected SuscribeStringDetected;


        public System.Threading.Timer Timer_ShowAllMember;
        public event TimerCallback Timer_ShowAllMember_Work_Delegate;
 
         
        void Form1_Load(object sender, EventArgs e)
        {
            
            //throw new NotImplementedException();
            InitGPIOLib();
            SetPinAltFn(45, -1, true); //D7
            SetPinAltFn(79, -1, true); //D6
            SetPinAltFn(85, -1, true); //D5
            SetPinAltFn(97, -1, true); //D4

            SetPinAltFn(101, -1, true);
            SetPinAltFn(103, -1, true);
            SetPinAltFn(133, -1, true);
            SetPinAltFn(98, -1, true);

            SetPinLevel(103, 0); //EN
            SetPinLevel(101, 0); //RS

            LCDFunctions.InitLCD();

            SetPinLevel(133, 1);
            LCDFunctions.gotoLCD(1, 2);
            LCDFunctions.sendString("Initilizing......");

            //Check for SD Card Status
            bool _SD_State = false;
            DirectoryInfo root = new DirectoryInfo("\\");
            UInt64 count = 0;
            while (!SD_Card_Status)
            {
                Thread.Sleep(500);
                
                DirectoryInfo[] directoryList = root.GetDirectories();
                for (int i = 0; i < directoryList.Length; ++i)
                {
                    if ((directoryList[i].Attributes & FileAttributes.Temporary) != 0 && directoryList[i].FullName.StartsWith("\\SD Card"))
                    {
                        _SD_State = true;
                        ExcelFileHandler.sd_card_name = directoryList[i].FullName;
                        break;
                    }
                }

                SD_Card_Status = _SD_State;

                if (!SD_Card_Status && count == 0) {
                    LCDFunctions.clearLCD();
                    LCDFunctions.gotoLCD(1, 2);
                    LCDFunctions.sendString("SD Card not Found");
                }
                count++;
            }

            LCDFunctions.clearLCD();
            LCDFunctions.gotoLCD(1, 2);
            LCDFunctions.sendString("SD Card Detected");
            LCDFunctions.gotoLCD(1, 3);
            LCDFunctions.sendString("Wait......");


            BackTimer = new Thread(BackTimer_Work);
            BackTimer.IsBackground = true;

            uui += updateUImethod;
            HookKeys.HookEvent += HookEventProc;
            HookKeys.Start();

            try
            {
                KeyMatrix.Start();
                KeyMatrix.KeypadMatrixEvent += MatrixHook;
            }
            catch (Exception ex) { }






            

            LCDFrameProcessor.HomeStartedSubscriberEvent += StartBackThread;
            LCDFrameProcessor.HomeStopSubscriberEvent += StopBackThread;
            SuscribeCommandDetected += CommandProcessor;
            SuscribeCharDetected += CharProcessor;
            SuscribeStringDetected += StringProcessor;

            //Init Serial Class
           
           //SerialP.ErrorMessageEvent += ClassExceptionCatcher;
           SerialP.Initialize();

           Printer.InitPrinter();

            //InitDateTimeUpdate
           DateTimeUpdate.ErrorHandlerEvent += ClassExceptionCatcher;

            //InitExcelFileHandler
           ExcelFileHandler.Initialize();
           ExcelFileHandler.ErrorHandlerEvent += ClassExceptionCatcher;

            //InitDataKeeper
           DataKeeper.InitDatabase();
           DataKeeper.ds.ReadXml(DataKeeper.path + @"\Settings.xml");
           DataKeeper.ExceptionHandlerProcessDataClassEvent += ClassExceptionCatcher;

            //InitDatabase
            ProcessData.ExceptionHandlerProcessDataClassEvent += ClassExceptionCatcher;
            ProcessData.ProcessDataClassThreadCompletedEvent += ProcessDataThreadCompleted;

            ProcessData.InitDatabase();
            ProcessData.ds.ReadXml(ProcessData.path + @"\Users.xml");

            //Second time Initilization
            LCDFunctions.InitLCD();
            LCDFrameProcessor.FrameBase = 1; //Important to Make FrameBase 1 When Application Starts
            CommandProcessor("Delete"); //Replace "Delete" with "Home" if Home Key is Available
            SetPinLevel(133, 1);

            //Initilize Setting
            SerialP.ChangeReceiverPhysicalMAP();
            SerialP.ChangeTransmitterPhysicalMAP();


            AppRunningStatus = true;
            Thread_BackLoop_Delegate += new TimerCallback(Thread_BackLoop_Work);
            Thread_BackLoop = new System.Threading.Timer(new TimerCallback(Thread_BackLoop_Delegate), new object(), 100, 1500);

            LCDFrameProcessor.BackSpaceEvent += new LCDFrameProcessor.BackEscape(LCDFrameProcessor_BackSpaceEvent);

            Thread.Sleep(2000);

            bool SecStat = false;
            try {
                SecStat = Secuity.VerifySecuity();
            }
            catch (Exception ex) { }

            

            if (!SecStat) { 
                //Wrong Secuirity
                LCDFrameProcessor.backTimerFlag = false;
                StopBackThread();
                LCDFunctions.clearLCD();
                LCDFunctions.printStringWithCmd("Secuity Violated", 0, 1);
                LCDFunctions.printStringWithCmd("Contact Retailer", 0, 2);
                Application.Exit();
            }
        }

        void LCDFrameProcessor_BackSpaceEvent()
        {
            //throw new NotImplementedException();
            KeyboardtoFrameID("Escape");
        }

        public void BackEscape() { 
        
        }

        public void Thread_BackLoop_Work(object state) {

            try
            {
                //Check for SD Card Status
                DirectoryInfo root = new DirectoryInfo("\\");
                DirectoryInfo[] directoryList = root.GetDirectories();
                ulong FreeBytesAvailable;
                ulong TotalCapacity;
                ulong TotalFreeBytes;
                bool _SD_State = false;

                for (int i = 0; i < directoryList.Length; ++i)
                {
                    if ((directoryList[i].Attributes & FileAttributes.Temporary) != 0 && directoryList[i].FullName.StartsWith("\\SD Card"))
                    {
                        _SD_State = true;
                        ExcelFileHandler.sd_card_name = directoryList[i].FullName;
                        break;
                    }
                }

                SD_Card_Status = _SD_State;
                ExcelFileHandler.bankSheet_xlsfolder = ExcelFileHandler.sd_card_name + @"\Bank";
                ExcelFileHandler.saleSaheet_xlsFolder = ExcelFileHandler.sd_card_name + @"\SaleSheet";
                ExcelFileHandler.rateChartPaths[1] = ExcelFileHandler.sd_card_name + @"\Excel";
                ExcelFileHandler.collectionPaths[1] = ExcelFileHandler.sd_card_name + @"\Excel";


                if (!SD_Card_Status && AppRunningStatus)
                {
                    AppRunningStatus = false;
                    SuspendApp();
                }
                else if (SD_Card_Status && !AppRunningStatus)
                {
                    AppRunningStatus = true;
                    ResumeApp();
                }
            }
            catch (Exception ex) { }
            
        }

        public void SuspendApp() {
            try
            {
                if (LCDFrameProcessor.FrameBase == 0)
                {
                    BackTimer.Abort();
                }

                LCDFunctions.clearLCD();
                LCDFunctions.gotoLCD(1, 1);
                LCDFunctions.sendString("Re-insert SD Card");
            }
            catch (Exception ex) { }
        
        }

        public void ResumeApp() {
          

            LCDFunctions.clearLCD();
            LCDFrameProcessor.RefreshFrame();
        }

        public void StartBackThread()
        {
            //textBox1.Text = DateTime.Now.ToString("hh:mm:ss tt").ToString();
            BackTimer = new Thread(BackTimer_Work);
            BackTimer.IsBackground = true;
            BackTimer.Priority = ThreadPriority.AboveNormal;
            BackTimer.Start();
        }

        public void StopBackThread() {
            BackTimer.Abort();
        }

       



        #region LCD Region
        private void printButton_Click(object sender, EventArgs e)
        {
            
            
        }

        private void lcdClearButton_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox3.Text = "";
        }

        private void textBoxLCD_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        #endregion

        #region ExceptionCatherFromOtherClass

        public void ClassExceptionCatcher(string str) {
            //MessageBox.Show(str);
        }

        #endregion

        #region MenuButtons

        private void menuItem_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuItem_about_Click(object sender, EventArgs e)
        {
            about Form2 = new about();
            Form2.Show();

        }

        #endregion

        public void Timer_ShowAllMember_Work(object state)
        {
            try
            {
                if (!ProcessData.Flag_GetAllMemberBusy)
                {
                    Timer_ShowAllMember.Dispose();
                    uui("Up");
                }
            }
            catch (Exception ex) { }
        }

        public void BackTimer_Work(){

            try
            {
                while (true)
                {

                    //LCDFunctions.InitLCD();
                    // LCDFunctions.clearLCD();                            

                    /*LCDFunctions.gotoLCD(0, 0);
                    LCDFunctions.sendString("Data Processing Unit");
                    LCDFunctions.gotoLCD(7, 1);
                    LCDFunctions.sendString("Ver.:1.0");
                     */
                    if (LCDFrameProcessor.backTimerFlag)
                    {

                        while (LCDFunctions.LCDLock == true)
                        {
                            Thread.Sleep(50);
                        }

                        LCDFunctions.LCDLock = true;
                        LCDFunctions.gotoLCD(6, 2);
                        LCDFunctions.sendString(DateTime.Now.ToString("dd:MM:yy"));

                        LCDFunctions.gotoLCD(6, 3);
                        LCDFunctions.sendString(DateTime.Now.ToString("hh:mm:ss tt"));

                        LCDFunctions.LCDLock = false;
                        Thread.Sleep(350);
                    }
                }
            }
            catch (Exception ex) { }
        }

        

        #region KeyHooKupPro

        public void MatrixHook(string s) {
            try
            {
                textBox3.BeginInvoke((Action)delegate() { textBox3.Text = s; });
               
            }
            catch (Exception ex) { }
            try
            {
                
                uui(s);
            }
            catch (Exception ex) { }
        }

        public void HookEventProc(HookEventArgs hookArgs, KeyBoardInfo keyBoardInfo)
        {
            //System.Diagnostics.Debug.WriteLine("Hook called");
            string s = ((Keys)keyBoardInfo.vkCode).ToString();
            uui(s);
        }


        public void updateUImethod(string message)
        {
            //Code Here WhateveryOU wANT TO !!
            #region Basic
            //textBox2.Text = message;

            if (!SD_Card_Status) {
                return;
            }

            if (LCDFrameProcessor.KeyboardMode == 0)
            {
                //Command Mode
                SuscribeCommandDetected(message);
                //textBox1.Text = "Still 0";

            }

            else if (LCDFrameProcessor.KeyboardMode == 1)
            { 
                //Char Mode
                SuscribeCharDetected(message);
                LCDFrameProcessor.KeyboardMode = 0;

            }
            else if (LCDFrameProcessor.KeyboardMode == 2)
            { 
                //String Mode
                if (message == "Return")
                {
                    LCDFrameProcessor.KeyboardMode = 0;
                    LCDFrameProcessor.LCDrefreshFlag = false;
                    LCDFrameProcessor.keyboardNumMode = false;
                    LCDFrameProcessor.keyboardMaxChar = 0;
                    SuscribeStringDetected(" ");
                }
                else if (message == "Tab")
                { 
                    //Switch Between Different Frame in Same Frame
                    if (LCDFrameProcessor.ActiveFrameInfo.IsInput)
                    {
                        LCDFrameProcessor.FindNextInputInFrame(true);
                    }

                }

                else if (message == "Escape")
                {
                    LCDFrameProcessor.KeyboardMode = 0;
                    LCDFrameProcessor.LCDrefreshFlag = false;
                    LCDFrameProcessor.keyboardNumMode = false;
                    LCDFrameProcessor.keyboardMaxChar = 0;
                    SuscribeCommandDetected(message);
                }
                else
                {

                    if (LCDFrameProcessor.keyboardNumMode)
                    {
                        //  KeyBoard is in NUM MODE, Ony Numbers Accepted
                        if (message == "D1" || message == "D2" || message == "D3" || message == "D4" || message == "D5" || message == "D6" || message == "D7" || message == "D8" || message == "D9" || message == "D0")
                        {
                            LCDFrameProcessor.ActiveFrameInfo.UpdateInfo = message.TrimStart(new char[] { 'D' });
                            if (LCDFrameProcessor.ActiveFrameInfo.WhichDataInFrame == 4)
                            {
                                if (LCDFrameProcessor.ActiveFrameInfo.day.ToString().Length == 2)
                                {
                                    LCDFrameProcessor.FindNextInputInFrame(true);
                                }
                            }
                            else if (LCDFrameProcessor.ActiveFrameInfo.WhichDataInFrame == 5)
                            {
                                if (LCDFrameProcessor.ActiveFrameInfo.month.ToString().Length == 2)
                                {
                                    LCDFrameProcessor.FindNextInputInFrame(true);
                                }
                            }
                            else if (LCDFrameProcessor.ActiveFrameInfo.WhichDataInFrame == 7)
                            {
                                if (LCDFrameProcessor.ActiveFrameInfo.hour.Length == 2)
                                {
                                    LCDFrameProcessor.FindNextInputInFrame(true);
                                }
                            }
                            else if (LCDFrameProcessor.ActiveFrameInfo.WhichDataInFrame == 8)
                            {
                                if (LCDFrameProcessor.ActiveFrameInfo.minute.Length == 2)
                                {
                                    LCDFrameProcessor.FindNextInputInFrame(true);
                                }
                            }
                            if (LCDFrameProcessor.ActiveFrameInfo.WhichDataInFrame == 61)
                            {
                                if (LCDFrameProcessor.ActiveFrameInfo.day_1.ToString().Length == 2)
                                {
                                    LCDFrameProcessor.FindNextInputInFrame(true);
                                }
                            }
                            else if (LCDFrameProcessor.ActiveFrameInfo.WhichDataInFrame == 62)
                            {
                                if (LCDFrameProcessor.ActiveFrameInfo.month_1.ToString().Length == 2)
                                {
                                    LCDFrameProcessor.FindNextInputInFrame(true);
                                }
                            }

                            #region Frame_501
                            //Update F5 - 501 Values
                            if (LCDFrameProcessor.FrameBase == 501 || LCDFrameProcessor.FrameBase == 502)
                            {
                                double fat = 0;
                                double snf = 0;
                                double quantity = 0;
                                double rate = 0;
                                double amount = 0;

                                uint rateChartSettingCow = uint.Parse(DataKeeper.GetValue("F2_COW_RATECHART"));
                                uint rateChartSettingBuf = uint.Parse(DataKeeper.GetValue("F2_BUF_RATECHART"));



                                double COWFatPerKg = double.Parse(DataKeeper.GetValue("F2_COW_FATRATEKG"));
                                double COWSnfPerKg = double.Parse(DataKeeper.GetValue("F2_COW_SNFRATEKG"));

                                double BUFFatPerKg = double.Parse(DataKeeper.GetValue("F2_BUF_FATRATEKG"));
                                double BUFSnfPerKg = double.Parse(DataKeeper.GetValue("F2_BUF_SNFRATEKG"));

                                if (!(LCDFrameProcessor.ActiveFrameInfo.float4_0data == ""))
                                    fat = double.Parse(LCDFrameProcessor.ActiveFrameInfo.float4_0data);
                                if (!(LCDFrameProcessor.ActiveFrameInfo.float4_1data == ""))
                                    snf = double.Parse(LCDFrameProcessor.ActiveFrameInfo.float4_1data);
                                if (!(LCDFrameProcessor.ActiveFrameInfo.float6_0data == ""))
                                    quantity = double.Parse(LCDFrameProcessor.ActiveFrameInfo.float6_0data);



                                //Its Decide Weather COW or BUFF
                                if (fat >= 5)
                                {
                                    //Its Buffalo
                                    LCDFrameProcessor.ActiveFrameInfo.whichShift = 1;
                                }
                                else
                                {
                                    //Its COW
                                    LCDFrameProcessor.ActiveFrameInfo.whichShift = 0;
                                }
                                //Decision END Here


                                if (LCDFrameProcessor.ActiveFrameInfo.whichShift == 1)
                                {
                                    //Its Buffalo Milk
                                    if (rateChartSettingBuf == 0)
                                    {
                                        //FAT SNF Formula Here 
                                        //rate = (3.20 * fat) + (2.30 * snf);
                                        rate = (fat * BUFFatPerKg + snf * BUFSnfPerKg) / 100;
                                    }
                                    else if (rateChartSettingBuf == 1) {
                                        var temp_rate = ExcelFileHandler.readCellRateChart(fat.ToString(), snf.ToString(), 2);
                                        rate = double.Parse(temp_rate.ToString());
                                    }
                                }
                                else if (LCDFrameProcessor.ActiveFrameInfo.whichShift == 0)
                                {
                                    //Its Cow Milk
                                    if (rateChartSettingCow == 0)
                                    {
                                        rate = (fat * COWFatPerKg + snf * COWSnfPerKg) / 100;
                                    }
                                    else if (rateChartSettingCow == 1) {
                                        var temp_rate = ExcelFileHandler.readCellRateChart(fat.ToString(), snf.ToString(), 1);
                                        rate = double.Parse(temp_rate.ToString());
                                    }
                                }

                                if (LCDFrameProcessor.ActiveFrameInfo.whichShift == 1)
                                {
                                    //Its BUF ..Now Check for Bonus
                                    double FATBonusRangeMin = double.Parse(DataKeeper.GetValue("F2_BUF_FATBONUSLIMIT_LOWER"));
                                    double FATBonusRangeMax = double.Parse(DataKeeper.GetValue("F2_BUF_FATBONUSLIMIT_UPPER"));
                                    double FATBonusAmount = double.Parse(DataKeeper.GetValue("F2_BUF_FATBONUSAMOUNT"));

                                    double BonFAT = double.Parse(DataKeeper.F2_GetAmountInRange("BONUS", "FAT", "BUF", float.Parse(fat.ToString()))[0].ToString());
                                    double BonSNF = double.Parse(DataKeeper.F2_GetAmountInRange("BONUS", "SNF", "BUF", float.Parse(snf.ToString()))[0].ToString());

                                    double DedFAT = double.Parse(DataKeeper.F2_GetAmountInRange("DEDUCT", "FAT", "BUF", float.Parse(fat.ToString()))[0].ToString());
                                    double DedSNF = double.Parse(DataKeeper.F2_GetAmountInRange("DEDUCT", "SNF", "BUF", float.Parse(snf.ToString()))[0].ToString());

                                    double _amount = ((BonFAT + BonSNF) / 100) - (DedFAT + DedSNF);
                                    rate += _amount;
                                }
                                else if (LCDFrameProcessor.ActiveFrameInfo.whichShift == 0)
                                {
                                    //Its Cow..Now Check for Bonus
                                    double FATBonusRangeMin = double.Parse(DataKeeper.GetValue("F2_COW_FATBONUSLIMIT_LOWER"));
                                    double FATBonusRangeMax = double.Parse(DataKeeper.GetValue("F2_COW_FATBONUSLIMIT_UPPER"));
                                    double FATBonusAmount = double.Parse(DataKeeper.GetValue("F2_COW_FATBONUSAMOUNT"));

                                    double BonFAT = double.Parse(DataKeeper.F2_GetAmountInRange("BONUS", "FAT", "COW", float.Parse(fat.ToString()))[0].ToString());
                                    double BonSNF = double.Parse(DataKeeper.F2_GetAmountInRange("BONUS", "SNF", "COW", float.Parse(snf.ToString()))[0].ToString());

                                    double DedFAT = double.Parse(DataKeeper.F2_GetAmountInRange("DEDUCT", "FAT", "COW", float.Parse(fat.ToString()))[0].ToString());
                                    double DedSNF = double.Parse(DataKeeper.F2_GetAmountInRange("DEDUCT", "SNF", "COW", float.Parse(snf.ToString()))[0].ToString());

                                    double _amount = ((BonFAT + BonSNF) / 100) - (DedFAT + DedSNF);
                                    rate += _amount;
                                }

                                if (LCDFrameProcessor.ActiveFrameInfo.whichShift == 1)
                                {
                                    //Its BUF ..Now Check for Cuttoff
                                    double FATCutoffMin = double.Parse(DataKeeper.GetValue("F2_BUF_FATCUTOFF_MIN"));
                                    double FATCutoffMax = double.Parse(DataKeeper.GetValue("F2_BUF_FATCUTOFF_MAX"));
                                    double SNFCutoffMin = double.Parse(DataKeeper.GetValue("F2_BUF_SNFCUTOFF_MIN"));
                                    double SNFCutoffMax = double.Parse(DataKeeper.GetValue("F2_BUF_SNFCUTOFF_MAX"));


                                    if (Math.Round(FATCutoffMin, 2) > Math.Round(fat, 2) || Math.Round(FATCutoffMax, 2) < Math.Round(fat, 2))
                                    {
                                        rate = 0;
                                    }
                                    if (Math.Round(SNFCutoffMin, 2) > Math.Round(snf, 2) || Math.Round(SNFCutoffMax, 2) < Math.Round(snf, 2))
                                    {
                                        rate = 0;
                                    }
                                }
                                else if (LCDFrameProcessor.ActiveFrameInfo.whichShift == 0)
                                {
                                    //Its Cow , Now Check for CUTOFF
                                    double FATCutoffMin = double.Parse(DataKeeper.GetValue("F2_COW_FATCUTOFF_MIN"));
                                    double FATCutoffMax = double.Parse(DataKeeper.GetValue("F2_COW_FATCUTOFF_MAX"));
                                    double SNFCutoffMin = double.Parse(DataKeeper.GetValue("F2_COW_SNFCUTOFF_MIN"));
                                    double SNFCutoffMax = double.Parse(DataKeeper.GetValue("F2_COW_SNFCUTOFF_MAX"));


                                    if (Math.Round(FATCutoffMin, 2) > Math.Round(fat, 2) || Math.Round(FATCutoffMax, 2) < Math.Round(fat, 2))
                                    {
                                        rate = 0;
                                    }
                                    if (Math.Round(SNFCutoffMin, 2) > Math.Round(snf, 2) || Math.Round(SNFCutoffMax, 2) < Math.Round(snf, 2))
                                    {
                                        rate = 0;
                                    }
                                }

                                amount = quantity * rate;

                                LCDFrameProcessor.ActiveFrameInfo.float5_0data = Math.Round(rate, 2).ToString();
                                LCDFrameProcessor.ActiveFrameInfo.float7_0data = Math.Round(amount, 2).ToString();

                            }
                            #endregion

                        }
                        else if (message == "Back")
                        {
                            if (LCDFrameProcessor.ActiveFrameInfo.WhichDataInFrame == 5)
                            {
                                if (LCDFrameProcessor.ActiveFrameInfo.month.ToString().Length == 0)
                                {
                                    if (LCDFrameProcessor.ActiveFrameInfo.day.Length == 2)
                                        LCDFrameProcessor.ActiveFrameInfo.day = LCDFrameProcessor.ActiveFrameInfo.day.Remove((LCDFrameProcessor.ActiveFrameInfo.day.Length - 1), 1);
                                    LCDFrameProcessor.FindNextInputInFrame(false);
                                }
                                else
                                {
                                    string x = LCDFrameProcessor.ActiveFrameInfo.UpdateInfo;
                                }
                            }
                            else if (LCDFrameProcessor.ActiveFrameInfo.WhichDataInFrame == 6)
                            {
                                if (LCDFrameProcessor.ActiveFrameInfo.year.ToString().Length == 0)
                                {
                                    if (LCDFrameProcessor.ActiveFrameInfo.month.Length == 2)
                                        LCDFrameProcessor.ActiveFrameInfo.month = LCDFrameProcessor.ActiveFrameInfo.month.Remove((LCDFrameProcessor.ActiveFrameInfo.month.Length - 1), 1);
                                    LCDFrameProcessor.FindNextInputInFrame(false);
                                }
                                else
                                {
                                    string x = LCDFrameProcessor.ActiveFrameInfo.UpdateInfo;
                                }
                            }
                            else if (LCDFrameProcessor.ActiveFrameInfo.WhichDataInFrame == 8)
                            {
                                if (LCDFrameProcessor.ActiveFrameInfo.minute.ToString().Length == 0)
                                {
                                    if (LCDFrameProcessor.ActiveFrameInfo.hour.Length == 2)
                                        LCDFrameProcessor.ActiveFrameInfo.hour = LCDFrameProcessor.ActiveFrameInfo.hour.Remove((LCDFrameProcessor.ActiveFrameInfo.hour.Length - 1), 1);
                                    LCDFrameProcessor.FindNextInputInFrame(false);
                                }
                                else
                                {
                                    string x = LCDFrameProcessor.ActiveFrameInfo.UpdateInfo;
                                }
                            }
                            else if (LCDFrameProcessor.ActiveFrameInfo.WhichDataInFrame == 9)
                            {
                                if (LCDFrameProcessor.ActiveFrameInfo.second.ToString().Length == 0)
                                {
                                    if (LCDFrameProcessor.ActiveFrameInfo.minute.Length == 2)
                                        LCDFrameProcessor.ActiveFrameInfo.minute = LCDFrameProcessor.ActiveFrameInfo.minute.Remove((LCDFrameProcessor.ActiveFrameInfo.minute.Length - 1), 1);
                                    LCDFrameProcessor.FindNextInputInFrame(false);
                                }
                                else
                                {
                                    string x = LCDFrameProcessor.ActiveFrameInfo.UpdateInfo;
                                }
                            }
                            if (LCDFrameProcessor.ActiveFrameInfo.WhichDataInFrame == 62)
                            {
                                if (LCDFrameProcessor.ActiveFrameInfo.month_1.ToString().Length == 0)
                                {
                                    if (LCDFrameProcessor.ActiveFrameInfo.day_1.Length == 2)
                                        LCDFrameProcessor.ActiveFrameInfo.day_1 = LCDFrameProcessor.ActiveFrameInfo.day_1.Remove((LCDFrameProcessor.ActiveFrameInfo.day.Length - 1), 1);
                                    LCDFrameProcessor.FindNextInputInFrame(false);
                                }
                                else
                                {
                                    string x = LCDFrameProcessor.ActiveFrameInfo.UpdateInfo;
                                }
                            }
                            else if (LCDFrameProcessor.ActiveFrameInfo.WhichDataInFrame == 63)
                            {
                                if (LCDFrameProcessor.ActiveFrameInfo.year_1.ToString().Length == 0)
                                {
                                    if (LCDFrameProcessor.ActiveFrameInfo.month_1.Length == 2)
                                        LCDFrameProcessor.ActiveFrameInfo.month_1 = LCDFrameProcessor.ActiveFrameInfo.month_1.Remove((LCDFrameProcessor.ActiveFrameInfo.month_1.Length - 1), 1);
                                    LCDFrameProcessor.FindNextInputInFrame(false);
                                }
                                else
                                {
                                    string x = LCDFrameProcessor.ActiveFrameInfo.UpdateInfo;
                                }
                            }
                            else
                            {
                                string x = LCDFrameProcessor.ActiveFrameInfo.UpdateInfo;
                            }
                        }
                        else if (message == "Left" || message == "Right")
                        {
                            if (LCDFrameProcessor.ActiveFrameInfo.WhichDataInFrame == 11 || LCDFrameProcessor.ActiveFrameInfo.WhichDataInFrame == 10 || LCDFrameProcessor.ActiveFrameInfo.WhichDataInFrame == 17 || LCDFrameProcessor.ActiveFrameInfo.WhichDataInFrame == 18 || LCDFrameProcessor.ActiveFrameInfo.WhichDataInFrame == 19
                                || LCDFrameProcessor.ActiveFrameInfo.WhichDataInFrame == 40 || LCDFrameProcessor.ActiveFrameInfo.WhichDataInFrame == 41 || LCDFrameProcessor.ActiveFrameInfo.WhichDataInFrame == 42 || LCDFrameProcessor.ActiveFrameInfo.WhichDataInFrame == 43 || LCDFrameProcessor.ActiveFrameInfo.WhichDataInFrame == 44
                                || LCDFrameProcessor.ActiveFrameInfo.WhichDataInFrame == 55 || LCDFrameProcessor.ActiveFrameInfo.WhichDataInFrame == 56 || LCDFrameProcessor.ActiveFrameInfo.WhichDataInFrame == 57 || LCDFrameProcessor.ActiveFrameInfo.WhichDataInFrame == 64
                                )
                            {
                                LCDFrameProcessor.ActiveFrameInfo.UpdateInfo = message;
                            }
                            else if (LCDFrameProcessor.ActiveFrameInfo.WhichDataInFrame == 15)
                            {
                                LCDFrameProcessor.ActiveFrameInfo.UpdateInfo = message;
                            }
                        }

                        else if (message == "Up" || message == "Down")
                        {
                            if (message == "Up")
                            {
                                if (LCDFrameProcessor.FrameBase == 80801 || LCDFrameProcessor.FrameBase == 80802 || LCDFrameProcessor.FrameBase == 8080310 || LCDFrameProcessor.FrameBase == 8080410 || LCDFrameProcessor.FrameBase == 80805 || LCDFrameProcessor.FrameBase == 80806 || LCDFrameProcessor.FrameBase == 10410)
                                {
                                    LCDFrameProcessor.ActiveFrameInfo.UpdateInfo = message;
                                }
                                else if (LCDFrameProcessor.FrameBase == 10410)
                                {
                                    //ProcessData.GetNextIDfromAllMemberList(false);
                                }
                                else
                                {
                                    if (LCDFrameProcessor.ActiveFrameInfo.IsInput)
                                    {
                                        LCDFrameProcessor.FindNextInputInFrame(false);
                                    }
                                }

                            }
                            else if (message == "Down")
                            {
                                if (LCDFrameProcessor.FrameBase == 80801 || LCDFrameProcessor.FrameBase == 80802 || LCDFrameProcessor.FrameBase == 8080310 || LCDFrameProcessor.FrameBase == 8080410 || LCDFrameProcessor.FrameBase == 80805 || LCDFrameProcessor.FrameBase == 80806 || LCDFrameProcessor.FrameBase == 10410)
                                {
                                    LCDFrameProcessor.ActiveFrameInfo.UpdateInfo = message;
                                }
                                else if (LCDFrameProcessor.FrameBase == 10410)
                                {
                                    //ProcessData.GetNextIDfromAllMemberList(true);
                                }
                                else
                                {
                                    if (LCDFrameProcessor.ActiveFrameInfo.IsInput)
                                    {
                                        LCDFrameProcessor.FindNextInputInFrame(true);
                                    }
                                }
                            }

                        }

                    }
                    else
                    {

                        //No Keyboard is Not in Num Mode

                        if (message == "D1" || message == "D2" || message == "D3" || message == "D4" || message == "D5" || message == "D6" || message == "D7" || message == "D8" || message == "D9" || message == "D0")
                        {
                            LCDFrameProcessor.ActiveFrameInfo.UpdateInfo = message.TrimStart(new char[] { 'D' });
                        }
                        else if (message == "Back")
                        {
                            string x = LCDFrameProcessor.ActiveFrameInfo.UpdateInfo;
                        }
                        else if (message == "Space")
                        {
                            LCDFrameProcessor.ActiveFrameInfo.UpdateInfo = " ";
                        }
                        else if (message != "Capital")
                        {
                            LCDFrameProcessor.ActiveFrameInfo.UpdateInfo = message;
                        }
                    }


                    ////////
                    SuscribeCharDetected(message);

                }

            }
            #endregion

            else if (LCDFrameProcessor.KeyboardMode == 4) { 
                //Repeat Mode
                if (LCDFrameProcessor.FrameBase == 2010510 || LCDFrameProcessor.FrameBase == 2010610)
                {
                    if (LCDFrameProcessor.keyboardNumMode == true)
                    {
                        if (message == "D1" || message == "D2" || message == "D3" || message == "D4" || message == "D5" || message == "D6" || message == "D7" || message == "D8" || message == "D9" || message == "D0")
                        {
                            LCDFrameProcessor.ActiveFrameInfo.UpdateInfo = message.TrimStart(new char[] { 'D' });

                            if (LCDFrameProcessor.ActiveFrameInfo.float4_0data.Length == 5)
                            {
                                
                                string data = LCDFrameProcessor.ActiveFrameInfo.float4_0data;
                                LCDFrameProcessor.ActiveFrameInfo.float4_0index = 0;

                                int activeSheet = 0;
                                if (LCDFrameProcessor.FrameBase == 2010510)
                                {
                                    activeSheet = 1;
                                }
                                else {
                                    activeSheet = 2;
                                }

                                //Excel opERATIO Here
                                
                                    ExcelFileHandler.WriteCellRateChart(Math.Round(TempCalculationVariable.FATmin, 1).ToString(),Math.Round(TempCalculationVariable.SNFmin, 1).ToString(),LCDFrameProcessor.ActiveFrameInfo.float4_0data,activeSheet);
                                //Excel Here End

                                TempCalculationVariable.SNFmin += 0.1;
                                if (Math.Round(TempCalculationVariable.SNFmin, 1) > Math.Round(TempCalculationVariable.SNFmax, 1)) { 
                                    //Do here
                                    TempCalculationVariable.SNFmin = TempCalculationVariable.SNFminFIX;
                                    TempCalculationVariable.FATmin += 0.1;
                                    if (Math.Round(TempCalculationVariable.FATmin, 1) > Math.Round(TempCalculationVariable.FATmax, 1))
                                    {
                                        //Shut Operation Here
                                        LCDFrameProcessor.KeyboardMode = 0;
                                        Thread.Sleep(500);
                                        KeyboardtoFrameID("Escape");
                                        KeyboardtoFrameID("Escape");
                                        LCDFrameProcessor.FrameBase = KeyboardtoFrameID("Escape");

                                    }
                                }
                            }
                            
                        }
                        else if (message == "Back")
                        {
                            string x = LCDFrameProcessor.ActiveFrameInfo.UpdateInfo;
                        }
                        else if (message == "Escape")
                        {
                            LCDFrameProcessor.FrameBase = KeyboardtoFrameID("Escape");
                        }
                        SuscribeCharDetected(message);
                    }
                
                
                }
                else if (LCDFrameProcessor.FrameBase == 2010310 || LCDFrameProcessor.FrameBase == 2010410)
                {
                    if (LCDFrameProcessor.keyboardNumMode == true)
                    {
                        if (message == "D1" || message == "D2" || message == "D3" || message == "D4" || message == "D5" || message == "D6" || message == "D7" || message == "D8" || message == "D9" || message == "D0")
                        {
                            LCDFrameProcessor.ActiveFrameInfo.UpdateInfo = message.TrimStart(new char[] { 'D' });

                            if (LCDFrameProcessor.ActiveFrameInfo.float4_0data.Length == 5)
                            {

                                string data = LCDFrameProcessor.ActiveFrameInfo.float4_0data;
                                LCDFrameProcessor.ActiveFrameInfo.float4_0index = 0;

                                int activeSheet = 0;
                                if (LCDFrameProcessor.FrameBase == 2010310)
                                {
                                    activeSheet = 3;
                                }
                                else
                                {
                                    activeSheet = 4;
                                }

                                //Excel opERATIO Here
                                
                                ExcelFileHandler.WriteCellRateChart(Math.Round(TempCalculationVariable.FATmin, 1).ToString(), TempCalculationVariable.CLRmin.ToString(), LCDFrameProcessor.ActiveFrameInfo.float4_0data, activeSheet);
                                //Excel Here End

                                TempCalculationVariable.CLRmin += 1;
                                if (TempCalculationVariable.CLRmin > TempCalculationVariable.CLRmax)
                                {
                                    //Do here
                                    TempCalculationVariable.CLRmin = TempCalculationVariable.CLRminFIX;
                                    TempCalculationVariable.FATmin += 0.1;
                                    if (Math.Round(TempCalculationVariable.FATmin, 1) > Math.Round(TempCalculationVariable.FATmax, 1))
                                    {
                                        //Shut Operation Here
                                        LCDFrameProcessor.KeyboardMode = 0;
                                        Thread.Sleep(500);
                                        KeyboardtoFrameID("Escape");
                                        KeyboardtoFrameID("Escape");
                                        LCDFrameProcessor.FrameBase = KeyboardtoFrameID("Escape");

                                    }
                                }
                            }

                        }
                        else if (message == "Back")
                        {
                            string x = LCDFrameProcessor.ActiveFrameInfo.UpdateInfo;
                        }
                        else if (message == "Escape")
                        {
                            LCDFrameProcessor.FrameBase = KeyboardtoFrameID("Escape");
                        }
                        SuscribeCharDetected(message);
                    }


                }
                else if (LCDFrameProcessor.FrameBase == 2010710 || LCDFrameProcessor.FrameBase == 2010810)
                {
                    if (LCDFrameProcessor.keyboardNumMode == true)
                    {
                        if (message == "D1" || message == "D2" || message == "D3" || message == "D4" || message == "D5" || message == "D6" || message == "D7" || message == "D8" || message == "D9" || message == "D0")
                        {
                            LCDFrameProcessor.ActiveFrameInfo.UpdateInfo = message.TrimStart(new char[] { 'D' });

                            if (LCDFrameProcessor.ActiveFrameInfo.float4_0data.Length == 5)
                            {

                                string data = LCDFrameProcessor.ActiveFrameInfo.float4_0data;
                                LCDFrameProcessor.ActiveFrameInfo.float4_0index = 0;

                                int activeSheet = 0;
                                if (LCDFrameProcessor.FrameBase == 2010710)
                                {
                                    activeSheet = 5;
                                }
                                else
                                {
                                    activeSheet = 6;
                                }

                                //Excel opERATIO Here

                                ExcelFileHandler.WriteCellRateChart(Math.Round(TempCalculationVariable.FATmin, 1).ToString(), "1", LCDFrameProcessor.ActiveFrameInfo.float4_0data, activeSheet);
                                //Excel Here End

                                
                                //Do here
                              
                                TempCalculationVariable.FATmin += 0.1;
                                if (Math.Round(TempCalculationVariable.FATmin, 1) > Math.Round(TempCalculationVariable.FATmax, 1))
                                {
                                    //Shut Operation Here
                                    LCDFrameProcessor.KeyboardMode = 0;
                                    Thread.Sleep(500);
                                    KeyboardtoFrameID("Escape");
                                    KeyboardtoFrameID("Escape");
                                    LCDFrameProcessor.FrameBase = KeyboardtoFrameID("Escape");

                                }
                                
                            }

                        }
                        else if (message == "Back")
                        {
                            string x = LCDFrameProcessor.ActiveFrameInfo.UpdateInfo;
                        }
                        else if (message == "Escape")
                        {
                            LCDFrameProcessor.FrameBase = KeyboardtoFrameID("Escape");
                        }
                        SuscribeCharDetected(message);
                    }


                }
            }
        }

        #endregion


        public void StringProcessor(string str)
        {
            CommandProcessor("Return");
        }

        public void CommandProcessor(string str)
        {
            
            //Dont Refresh LCD Unless New FrameBack
            //Call KeyboardtoFrameID just Once;
            UInt64 TempFrameBase = 50000;

            if (str == "Return") {
                TempFrameBase = KeyboardtoFrameID("Ret");
                if (TempFrameBase == 10113) {
                    if (ProcessData.UpdateInDataBase(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID, LCDFrameProcessor.ActiveFrameInfo.FrameData_Name, LCDFrameProcessor.ActiveFrameInfo.FrameData_Phone, false)) {
                        //ProcessData.SaveChanges();
                        shouldChangeDatabase = true;
                    }
                }
                else if (TempFrameBase == 10211) {
                    if (ProcessData.UpdateInDataBase(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID, LCDFrameProcessor.ActiveFrameInfo.FrameData_Name, LCDFrameProcessor.ActiveFrameInfo.FrameData_Phone, false))
                    {
                        //Modify or Update in Database
                        //ProcessData.SaveChanges();
                        shouldChangeDatabase = true;
                    }
                }

                else if (TempFrameBase == 301)
                {
                    //Time Updated
                    //textBox1.Text = "Time Updated ! Buddy";
                }
                else if (TempFrameBase == 504)
                {
                    if (!ProcessData.IsSpaceAvailable(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID))
                    {
                        KeyboardtoFrameID("Escape");
                        LCDFrameProcessor.customMessgae = "User Not Found";
                        LCDFrameProcessor.customMessgae1 = "";
                        FrameBaseHistory.Push(123456);
                        TempFrameBase = 123456;
                        key_press++;
                    }
                }
                else if (TempFrameBase == 601)
                {
                    string _local_shift = "M";
                    if (DateTime.Now.Hour < 13)
                    {
                        _local_shift = "M";
                    }
                    else
                    {
                        _local_shift = "E";
                    }
                    if (ExcelFileHandler.IsCollectionExist(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID.ToString(), DateTime.Now.Day.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString(), _local_shift))
                    {
                        //Print it
                        Printer.printCollection_ID(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID.ToString(), DateTime.Now.Day.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString(), _local_shift,true);
                    }
                    else
                    {
                        KeyboardtoFrameID("Escape");
                        LCDFrameProcessor.customMessgae = "No Collection Found";
                        LCDFrameProcessor.customMessgae1 = "";
                        FrameBaseHistory.Push(123456);
                        TempFrameBase = 123456;
                        key_press++;

                    }

                }



                else if (TempFrameBase == 701)
                {
                    if (LCDFrameProcessor.ActiveFrameInfo.day != "" &&
                        LCDFrameProcessor.ActiveFrameInfo.month != "" &&
                        LCDFrameProcessor.ActiveFrameInfo.year != ""
                        )
                    {
                        //---------------------------------------------------
                        //Get Shift Report
                        ExcelFileHandler.ReportShift(
                            uint.Parse(LCDFrameProcessor.ActiveFrameInfo.day),
                            uint.Parse(LCDFrameProcessor.ActiveFrameInfo.month),
                            uint.Parse(LCDFrameProcessor.ActiveFrameInfo.year),
                            LCDFrameProcessor.ActiveFrameInfo.whichShift
                        );

                        //Shift Report Fetched. Now Print it

                        Printer.printShiftReport(
                            uint.Parse(LCDFrameProcessor.ActiveFrameInfo.day),
                            uint.Parse(LCDFrameProcessor.ActiveFrameInfo.month),
                            uint.Parse(LCDFrameProcessor.ActiveFrameInfo.year),
                            LCDFrameProcessor.ActiveFrameInfo.whichShift
                        );

                    }
                    else
                    {
                        LCDFrameProcessor.customMessgae = "Enter Date Properly";
                        LCDFrameProcessor.customMessgae1 = "";
                        KeyboardtoFrameID("Escape");
                        FrameBaseHistory.Push(123456);
                        TempFrameBase = 123456;
                        key_press++;
                    }

                }
                else if (TempFrameBase == 80110 || TempFrameBase == 80210) {

                    float __amount = 0;
                    float __quantity = 0;
                    float __rate = 0;
                    string __cat = "";
                    try
                    {
                        if (LCDFrameProcessor.ActiveFrameInfo.toggleSwitch == 0){
                            __rate = float.Parse(DataKeeper.GetValue("TRUCK_COW_RATE"));
                            __cat = "COW";
                        }
                        else if (LCDFrameProcessor.ActiveFrameInfo.toggleSwitch == 1){
                            __rate = float.Parse(DataKeeper.GetValue("TRUCK_BUF_RATE"));
                            __cat = "BUF";
                        }


                        __quantity = float.Parse(LCDFrameProcessor.ActiveFrameInfo.float6_0data);
                        __amount = __rate * __quantity;
                    }
                    catch (Exception ex) { }

                    if (TempFrameBase == 80110) {
                        try {
                            ExcelFileHandler.AddNewLocalEntry(__amount, __quantity, __rate, __cat, "ENTRY FROM F8");
                            LCDFrameProcessor.customMessgae = "Local Sale Added";
                            LCDFrameProcessor.customMessgae1 = "";
                            KeyboardtoFrameID("Escape");
                            FrameBaseHistory.Push(123456);
                            TempFrameBase = 123456;
                            key_press++;
                        }
                        catch (Exception e) { }

                    }
                    else if (TempFrameBase == 80210) {
                        try
                        {
                            ExcelFileHandler.AddNewTruckEntry(__amount, __quantity, __rate, __cat,LCDFrameProcessor.ActiveFrameInfo.VechileNo);
                            LCDFrameProcessor.customMessgae = "Truck Sale Added";
                            LCDFrameProcessor.customMessgae1 = "";
                            KeyboardtoFrameID("Escape");
                            FrameBaseHistory.Push(123456);
                            TempFrameBase = 123456;
                            key_press++;
                        }
                        catch (Exception e) { }
                    }
                }
                else if (TempFrameBase == 80310 || TempFrameBase == 80410)
                {
                    uint day = 0;
                    uint month = 0;
                    uint year = 0;
                    uint serial = 0;
                    try
                    {
                        if (LCDFrameProcessor.ActiveFrameInfo.day != "" && LCDFrameProcessor.ActiveFrameInfo.month != "" && LCDFrameProcessor.ActiveFrameInfo.year != "" && LCDFrameProcessor.ActiveFrameInfo.integer5_0 != "")
                        {
                            day = uint.Parse(LCDFrameProcessor.ActiveFrameInfo.day);
                            month = uint.Parse(LCDFrameProcessor.ActiveFrameInfo.month);
                            year = uint.Parse(LCDFrameProcessor.ActiveFrameInfo.year);
                            serial = uint.Parse(LCDFrameProcessor.ActiveFrameInfo.integer5_0);
                        }
                            
                    }
                    catch (Exception ex) { }

                    if (day == 0 || month == 0 || serial == 0)
                    {

                        LCDFrameProcessor.customMessgae = "Date or Serial";
                        LCDFrameProcessor.customMessgae1 = "Not Valid Entry";
                        KeyboardtoFrameID("Escape");
                        FrameBaseHistory.Push(123456);
                        TempFrameBase = 123456;
                        key_press++;
                    }
                    else {
                        if (TempFrameBase == 80310)
                        {
                            if (ExcelFileHandler.DeleteLocalEntry(day, month, year+2000, serial))
                            {
                                LCDFrameProcessor.customMessgae = "Deleted";
                                LCDFrameProcessor.customMessgae1 = "";
                                KeyboardtoFrameID("Escape");
                                FrameBaseHistory.Push(123456);
                                TempFrameBase = 123456;
                                key_press++;
                            }
                            else
                            {
                                LCDFrameProcessor.customMessgae = "Error Occured";
                                LCDFrameProcessor.customMessgae1 = "";
                                KeyboardtoFrameID("Escape");
                                FrameBaseHistory.Push(123456);
                                TempFrameBase = 123456;
                                key_press++;
                            }
                        }
                        else if (TempFrameBase == 80410)
                        {
                            if (ExcelFileHandler.DeleteTruckEntry(day, month, year+2000, serial))
                            {
                                LCDFrameProcessor.customMessgae = "Deleted";
                                LCDFrameProcessor.customMessgae1 = "";
                                KeyboardtoFrameID("Escape");
                                FrameBaseHistory.Push(123456);
                                TempFrameBase = 123456;
                                key_press++;
                            }
                            else
                            {
                                LCDFrameProcessor.customMessgae = "Error Occured";
                                LCDFrameProcessor.customMessgae1 = "";
                                KeyboardtoFrameID("Escape");
                                FrameBaseHistory.Push(123456);
                                TempFrameBase = 123456;
                                key_press++;
                            }
                        }
                    }
                }
                else if (TempFrameBase == 80510 || TempFrameBase == 80610) {
                    uint day = 0;
                    uint month = 0;
                    uint year = 0;
                    
                    try
                    {
                        if (LCDFrameProcessor.ActiveFrameInfo.day != "" && LCDFrameProcessor.ActiveFrameInfo.month != "" && LCDFrameProcessor.ActiveFrameInfo.year != "")
                        {
                            day = uint.Parse(LCDFrameProcessor.ActiveFrameInfo.day);
                            month = uint.Parse(LCDFrameProcessor.ActiveFrameInfo.month);
                            year = uint.Parse(LCDFrameProcessor.ActiveFrameInfo.year);
                            
                        }

                    }
                    catch (Exception ex) { }

                    if (day == 0 || month == 0)
                    {

                        LCDFrameProcessor.customMessgae = "Date !";
                        LCDFrameProcessor.customMessgae1 = "Not Valid Entry";
                        KeyboardtoFrameID("Escape");
                        FrameBaseHistory.Push(123456);
                        TempFrameBase = 123456;
                        key_press++;
                    }
                    else { 
                        //Print
                        if (TempFrameBase == 80510) {
                            Printer.printSaleSheet(day, month, 2000 + year, true);
                        }
                        else if (TempFrameBase == 80610) {
                            Printer.printSaleSheet(day, month, 2000 + year, false);
                        }

                        LCDFrameProcessor.customMessgae = "Printing.....";
                        LCDFrameProcessor.customMessgae1 = "";
                        KeyboardtoFrameID("Escape");
                        FrameBaseHistory.Push(123456);
                        TempFrameBase = 123456;
                        key_press++;
                    }
                }
                else if (TempFrameBase == 8080310 || TempFrameBase == 8080410)
                {
                    int day = 0;
                    int month = 0;
                    int year = 0;

                    try
                    {
                        if (LCDFrameProcessor.ActiveFrameInfo.day != "" && LCDFrameProcessor.ActiveFrameInfo.month != "" && LCDFrameProcessor.ActiveFrameInfo.year != "")
                        {
                            day = int.Parse(LCDFrameProcessor.ActiveFrameInfo.day);
                            month = int.Parse(LCDFrameProcessor.ActiveFrameInfo.month);
                            year = int.Parse(LCDFrameProcessor.ActiveFrameInfo.year);
                        }
                    }
                    catch (Exception ex) { }

                    if (day == 0 || month == 0)
                    {
                        LCDFrameProcessor.customMessgae = "Enter Date Properly";
                        LCDFrameProcessor.customMessgae1 = "";
                        KeyboardtoFrameID("Escape");
                        FrameBaseHistory.Push(123456);
                        TempFrameBase = 123456;
                        key_press++;
                    }
                }
                else if (TempFrameBase == 90110)
                {
                    //Print Single Ledger
                    if (LCDFrameProcessor.ActiveFrameInfo.day != "" &&
                        LCDFrameProcessor.ActiveFrameInfo.day_1 != "" &&
                        LCDFrameProcessor.ActiveFrameInfo.month != "" &&
                        LCDFrameProcessor.ActiveFrameInfo.month_1 != "" &&
                        LCDFrameProcessor.ActiveFrameInfo.year != "" &&
                        LCDFrameProcessor.ActiveFrameInfo.integer3_0 != "" &&
                        LCDFrameProcessor.ActiveFrameInfo.integer3_1 != "" &&
                        LCDFrameProcessor.ActiveFrameInfo.year_1 != "")
                    {
                        //----------------------------------------------------------
                        int day = 0;
                        int day1 = 0;
                        int month = 0;
                        int month1 = 0;


                        try
                        {
                            day = int.Parse(LCDFrameProcessor.ActiveFrameInfo.day);
                            day1 = int.Parse(LCDFrameProcessor.ActiveFrameInfo.day_1);
                            month = int.Parse(LCDFrameProcessor.ActiveFrameInfo.month);
                            month1 = int.Parse(LCDFrameProcessor.ActiveFrameInfo.month_1);
                        }
                        catch (Exception ex) { }

                        if (day != 0 && day1 != 0 && month != 0 && month1 != 0)
                        {
                            object[] ob = ExcelFileHandler.returnAllmemberLedger(

                                uint.Parse(LCDFrameProcessor.ActiveFrameInfo.day),
                                uint.Parse(LCDFrameProcessor.ActiveFrameInfo.month),
                                uint.Parse(LCDFrameProcessor.ActiveFrameInfo.year),
                                LCDFrameProcessor.ActiveFrameInfo.whichShift,
                                uint.Parse(LCDFrameProcessor.ActiveFrameInfo.day_1),
                                uint.Parse(LCDFrameProcessor.ActiveFrameInfo.month_1),
                                uint.Parse(LCDFrameProcessor.ActiveFrameInfo.year_1),
                                LCDFrameProcessor.ActiveFrameInfo.whichShift_1,
                                uint.Parse(LCDFrameProcessor.ActiveFrameInfo.integer3_0),
                                uint.Parse(LCDFrameProcessor.ActiveFrameInfo.integer3_1)
                            );
                            try
                            {
                                if (ob != null)
                                {
                                    Printer.printAllLedgerList(
                                        LCDFrameProcessor.ActiveFrameInfo.integer3_0,
                                        LCDFrameProcessor.ActiveFrameInfo.integer3_1,
                                    uint.Parse(LCDFrameProcessor.ActiveFrameInfo.day),
                                    uint.Parse(LCDFrameProcessor.ActiveFrameInfo.month),
                                    uint.Parse(LCDFrameProcessor.ActiveFrameInfo.year),
                                    LCDFrameProcessor.ActiveFrameInfo.whichShift,
                                    uint.Parse(LCDFrameProcessor.ActiveFrameInfo.day_1),
                                    uint.Parse(LCDFrameProcessor.ActiveFrameInfo.month_1),
                                    uint.Parse(LCDFrameProcessor.ActiveFrameInfo.year_1),
                                    LCDFrameProcessor.ActiveFrameInfo.whichShift_1
                                );

                                }

                                else
                                {
                                    LCDFrameProcessor.customMessgae = "Something Wrong";
                                    LCDFrameProcessor.customMessgae1 = "";
                                    KeyboardtoFrameID("Escape");
                                    FrameBaseHistory.Push(123456);
                                    TempFrameBase = 123456;
                                    key_press++;
                                }
                            }
                            catch (Exception ex) { }
                        }
                        else
                        {
                            LCDFrameProcessor.customMessgae = "Month Can't be Zero";
                            LCDFrameProcessor.customMessgae1 = "Day Can't be Zero";
                            KeyboardtoFrameID("Escape");
                            FrameBaseHistory.Push(123456);
                            TempFrameBase = 123456;
                            key_press++;
                        }
                    }
                    else
                    {
                        LCDFrameProcessor.customMessgae = "Invalid Date or";
                        LCDFrameProcessor.customMessgae1 = "Code";
                        KeyboardtoFrameID("Escape");
                        KeyboardtoFrameID("Escape");
                        FrameBaseHistory.Push(123456);
                        TempFrameBase = 123456;
                        key_press++;
                    }
                }
                else if (TempFrameBase == 100110)
                {
                    float am = 0;
                    try
                    {
                        am = float.Parse(LCDFrameProcessor.ActiveFrameInfo.float7_0data);
                    }
                    catch (Exception ex) { }

                    if (ProcessData.IsSpaceAvailable(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID))
                    {
                        if (am > 50)
                        {
                            if (!ExcelFileHandler.BankSheet_Credit(am, "DIRECT CREDIT", LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID))
                            {
                                LCDFrameProcessor.customMessgae = "Could Not Credit";
                                LCDFrameProcessor.customMessgae1 = "";
                                KeyboardtoFrameID("Escape");
                                FrameBaseHistory.Push(123456);
                                TempFrameBase = 123456;
                                key_press++;
                            }
                            else
                            {
                                LCDFrameProcessor.customMessgae = "Amount Credited";
                                LCDFrameProcessor.customMessgae1 = "";
                                KeyboardtoFrameID("Escape");
                                FrameBaseHistory.Push(123456);
                                TempFrameBase = 123456;
                                key_press++;
                            }
                        }
                        else
                        {
                            LCDFrameProcessor.customMessgae = "Minimum Amount 50";
                            LCDFrameProcessor.customMessgae1 = "";
                            KeyboardtoFrameID("Escape");
                            FrameBaseHistory.Push(123456);
                            TempFrameBase = 123456;
                            key_press++;
                        }
                    }
                    else
                    {
                        LCDFrameProcessor.customMessgae = "User Not Found";
                        LCDFrameProcessor.customMessgae1 = "Add User First";
                        KeyboardtoFrameID("Escape");
                        FrameBaseHistory.Push(123456);
                        TempFrameBase = 123456;
                        key_press++;
                    }


                }
                else if (TempFrameBase == 100210)
                {
                    object[] DeductReturn = new object[] { "", false };


                    try
                    {
                        if (float.Parse(LCDFrameProcessor.ActiveFrameInfo.float7_0data) > 0)
                        {
                            DeductReturn = ExcelFileHandler.BankSheet_Deduct(float.Parse(LCDFrameProcessor.ActiveFrameInfo.float7_0data), "DIRECT DEDUCT", LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID);
                        }
                        else
                        {
                            DeductReturn[0] = "Can't Deduct Zero";
                        }
                    }
                    catch (Exception ex) { }

                    if (bool.Parse(DeductReturn[1].ToString()) == true)
                    {
                        LCDFrameProcessor.customMessgae = "Amount Deducted";
                        LCDFrameProcessor.customMessgae1 = "SucessFully";
                        KeyboardtoFrameID("Escape");
                        FrameBaseHistory.Push(123456);
                        TempFrameBase = 123456;
                        key_press++;
                    }
                    else if (bool.Parse(DeductReturn[1].ToString()) == false)
                    {
                        LCDFrameProcessor.customMessgae = "Amount Not Deducted";
                        LCDFrameProcessor.customMessgae1 = DeductReturn[0].ToString();
                        KeyboardtoFrameID("Escape");
                        FrameBaseHistory.Push(123456);
                        TempFrameBase = 123456;
                        key_press++;
                    }


                }
                else if (TempFrameBase == 10030110 || TempFrameBase == 10030210)
                {
                    if (TempFrameBase == 10030110)
                    {
                        int day = 0;
                        int month = 0;
                        int year = 0;


                        try
                        {
                            if (LCDFrameProcessor.ActiveFrameInfo.day != "" && LCDFrameProcessor.ActiveFrameInfo.month != "" && LCDFrameProcessor.ActiveFrameInfo.year != "")
                            {
                                day = int.Parse(LCDFrameProcessor.ActiveFrameInfo.day);
                                month = int.Parse(LCDFrameProcessor.ActiveFrameInfo.month);
                                year = int.Parse(LCDFrameProcessor.ActiveFrameInfo.year);
                            }
                        }
                        catch (Exception ex) { }

                        if (day == 0 || month == 0)
                        {
                            LCDFrameProcessor.customMessgae = "Enter Date Properly";
                            LCDFrameProcessor.customMessgae1 = "";
                            KeyboardtoFrameID("Escape");
                            FrameBaseHistory.Push(123456);
                            TempFrameBase = 123456;
                            key_press++;
                        }
                        else if (LCDFrameProcessor.ActiveFrameInfo.integer3_0 == "")
                        {
                            LCDFrameProcessor.customMessgae = "Invalid Code";
                            LCDFrameProcessor.customMessgae1 = "";
                            KeyboardtoFrameID("Escape");
                            FrameBaseHistory.Push(123456);
                            TempFrameBase = 123456;
                            key_press++;
                        }
                        else
                        {
                            //Send Print Command

                            try
                            {
                                Printer.printBankDate(uint.Parse(LCDFrameProcessor.ActiveFrameInfo.integer3_0), day.ToString() + "/" + month.ToString() + "/" + (year + 2000).ToString());
                            }
                            catch (Exception ex) { }

                            LCDFrameProcessor.customMessgae = "Printing....";
                            LCDFrameProcessor.customMessgae1 = "";
                            KeyboardtoFrameID("Escape");
                            FrameBaseHistory.Push(123456);
                            TempFrameBase = 123456;
                            key_press++;
                        }
                    }
                    else if (TempFrameBase == 10030210)
                    {
                        if (LCDFrameProcessor.ActiveFrameInfo.integer3_0 == "")
                        {
                            LCDFrameProcessor.customMessgae = "Invalid Code";
                            LCDFrameProcessor.customMessgae1 = "";
                            KeyboardtoFrameID("Escape");
                            FrameBaseHistory.Push(123456);
                            TempFrameBase = 123456;
                            key_press++;
                        }
                        else
                        {
                            //Send Print Command All
                            try
                            {
                                Printer.printBankAll(uint.Parse(LCDFrameProcessor.ActiveFrameInfo.integer3_0));
                            }
                            catch (Exception ex) { }
                            LCDFrameProcessor.customMessgae = "Printing....";
                            LCDFrameProcessor.customMessgae1 = "";
                            KeyboardtoFrameID("Escape");
                            FrameBaseHistory.Push(123456);
                            TempFrameBase = 123456;
                            key_press++;
                        }
                    }
                }
                else if (TempFrameBase == 100510) {
                    if (LCDFrameProcessor.ActiveFrameInfo.integer3_0 == "")
                    {
                        LCDFrameProcessor.customMessgae = "Invalid Code";
                        LCDFrameProcessor.customMessgae1 = "";
                        KeyboardtoFrameID("Escape");
                        FrameBaseHistory.Push(123456);
                        TempFrameBase = 123456;
                        key_press++;
                    }
                }
                else if (TempFrameBase == 78910)
                {
                    //Print All Ledger
                    if (LCDFrameProcessor.ActiveFrameInfo.day != "" &&
                        LCDFrameProcessor.ActiveFrameInfo.day_1 != "" &&
                        LCDFrameProcessor.ActiveFrameInfo.month != "" &&
                        LCDFrameProcessor.ActiveFrameInfo.month_1 != "" &&
                        LCDFrameProcessor.ActiveFrameInfo.year != "" &&
                        LCDFrameProcessor.ActiveFrameInfo.year_1 != "")
                    {
                        //----------------------------------------------------------
                        /* object[] ob = ExcelFileHandler.returnAllmemberLedger(
                             uint.Parse(LCDFrameProcessor.ActiveFrameInfo.day),
                             uint.Parse(LCDFrameProcessor.ActiveFrameInfo.month),
                             uint.Parse(LCDFrameProcessor.ActiveFrameInfo.year),
                             LCDFrameProcessor.ActiveFrameInfo.whichShift,
                             uint.Parse(LCDFrameProcessor.ActiveFrameInfo.day_1),
                             uint.Parse(LCDFrameProcessor.ActiveFrameInfo.month_1),
                             uint.Parse(LCDFrameProcessor.ActiveFrameInfo.year_1),
                             LCDFrameProcessor.ActiveFrameInfo.whichShift_1
                         );*/

                        /*if (ob != null)
                        {
                            Printer.printAllLedgerList(
                            uint.Parse(LCDFrameProcessor.ActiveFrameInfo.day),
                            uint.Parse(LCDFrameProcessor.ActiveFrameInfo.month),
                            uint.Parse(LCDFrameProcessor.ActiveFrameInfo.year),
                            LCDFrameProcessor.ActiveFrameInfo.whichShift,
                            uint.Parse(LCDFrameProcessor.ActiveFrameInfo.day_1),
                            uint.Parse(LCDFrameProcessor.ActiveFrameInfo.month_1),
                            uint.Parse(LCDFrameProcessor.ActiveFrameInfo.year_1),
                            LCDFrameProcessor.ActiveFrameInfo.whichShift_1
                        );
                        }
                        else
                        {
                            LCDFrameProcessor.customMessgae = "Something Wrong";
                            LCDFrameProcessor.customMessgae1 = "";
                            KeyboardtoFrameID("Escape");
                            FrameBaseHistory.Push(123456);
                            TempFrameBase = 123456;
                            key_press++;
                        }*/
                    }
                    else
                    {
                        LCDFrameProcessor.customMessgae = "Enter Date Properly";
                        LCDFrameProcessor.customMessgae1 = "";
                        KeyboardtoFrameID("Escape");
                        FrameBaseHistory.Push(123456);
                        TempFrameBase = 123456;
                        key_press++;
                    }
                }
                else if (TempFrameBase == 40210)
                {
                    //change Screen Title and Turn on the Comiit Flag
                }
                else if (TempFrameBase == 110111 || TempFrameBase == 110210)
                {
                    if (LCDFrameProcessor.ActiveFrameInfo.day != "" &&
                        LCDFrameProcessor.ActiveFrameInfo.month != "" &&
                        LCDFrameProcessor.ActiveFrameInfo.year != ""
                        )
                    {
                        //--------------------------------------------------------
                        //Delete a Transaction

                        //After User Press "Y"
                    }
                    else
                    {
                        LCDFrameProcessor.customMessgae = "Enter Date Properly";
                        LCDFrameProcessor.customMessgae1 = "";
                        KeyboardtoFrameID("Escape");
                        FrameBaseHistory.Push(123456);
                        TempFrameBase = 123456;
                        key_press++;
                    }
                }
                else if (TempFrameBase == 12)
                {
                    if (LCDFrameProcessor.F12_PASSWORD_STATE)
                    {
                        string actualPassword = DataKeeper.GetValue("F12_PASSWORD");
                        if (actualPassword == LCDFrameProcessor.ActiveFrameInfo.passwordGlobalReal)
                        {
                            LCDFrameProcessor.F12_PASSWORD_STATE = false;
                            LCDFrameProcessor.FrameBase = 456;

                        }
                        else
                        {
                            LCDFrameProcessor.customMessgae = "Incorrect Password";
                            LCDFrameProcessor.customMessgae1 = "";
                            FrameBaseHistory.Push(123456);
                            TempFrameBase = 123456;
                            key_press++;
                        }
                    }
                }

                else if (TempFrameBase == 2)
                {
                    if (LCDFrameProcessor.F2_PASSWORD_STATE)
                    {
                        string actualPassword = DataKeeper.GetValue("F2_PASSWORD");
                        if (actualPassword == LCDFrameProcessor.ActiveFrameInfo.passwordGlobalReal)
                        {
                            LCDFrameProcessor.F2_PASSWORD_STATE = false;
                            LCDFrameProcessor.FrameBase = 456;

                        }
                        else
                        {
                            LCDFrameProcessor.customMessgae = "Incorrect Password";
                            LCDFrameProcessor.customMessgae1 = "";
                            FrameBaseHistory.Push(123456);
                            TempFrameBase = 123456;
                            key_press++;
                        }
                    }
                }
                else if (TempFrameBase == 10)
                {
                    if (LCDFrameProcessor.F10_PASSWORD_STATE)
                    {
                        string actualPassword = DataKeeper.GetValue("F10_PASSWORD");
                        if (actualPassword == LCDFrameProcessor.ActiveFrameInfo.passwordGlobalReal)
                        {
                            LCDFrameProcessor.F10_PASSWORD_STATE = false;
                            LCDFrameProcessor.FrameBase = 456;

                        }
                        else
                        {
                            LCDFrameProcessor.customMessgae = "Incorrect Password";
                            LCDFrameProcessor.customMessgae1 = "";
                            FrameBaseHistory.Push(123456);
                            TempFrameBase = 123456;
                            key_press++;
                        }
                    }
                }
                


            }
            else if (str == "Y" || str == "y") {
                TempFrameBase = KeyboardtoFrameID("Yes");
                if (TempFrameBase == 10111) {
                    if (ProcessData.UpdateInDataBase(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID,"","",false)) {
                       // ProcessData.SaveChanges();
                        shouldChangeDatabase = true;
                    }
                }else if(TempFrameBase == 10121){
                    if (ProcessData.UpdateInDataBase(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID, "", "", true))
                    {
                        ExcelFileHandler.Bank_DeleteAccount(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID);
                       // ProcessData.SaveChanges();
                        shouldChangeDatabase = true;
                    }
                }
                else if (TempFrameBase == 10310) {
                    ProcessData.GetAllMemberForPrint();
                    if (ProcessData.AllMemberPrint_List.Count < 1)
                    {
                        LCDFrameProcessor.customMessgae = "No User Found";
                        LCDFrameProcessor.customMessgae1 = "";
                        KeyboardtoFrameID("Escape");
                        KeyboardtoFrameID("Escape");
                        FrameBaseHistory.Push(123456);
                        TempFrameBase = 123456;
                        key_press++;
                    }
                    else {

                        Printer.printAllMember();
                        LCDFrameProcessor.customMessgae = "Printing";
                        LCDFrameProcessor.customMessgae1 = "";
                        KeyboardtoFrameID("Escape");
                        KeyboardtoFrameID("Escape");
                        FrameBaseHistory.Push(123456);
                        TempFrameBase = 123456;
                        key_press++;
                    
                    }
                    
                }
                else if (TempFrameBase == 10410)
                {
                    //ProcessData.GetAllMember();
                    //Timer_ShowAllMember_Work_Delegate += new TimerCallback(Timer_ShowAllMember_Work);
                    //Timer_ShowAllMember = new System.Threading.Timer(Timer_ShowAllMember_Work_Delegate, new object(), 500, 1000);

                }
                else if (TempFrameBase == 10510) { 
                    //Shut Down
                    LCDFunctions.clearLCD();
                    LCDFunctions.gotoLCD(0, 1); LCDFunctions.CursorON_OFF(false);
                    LCDFunctions.sendString("Turning System off..");
                    Thread.Sleep(2000);
                    LCDFunctions.clearLCD();
                    LCDFunctions.gotoLCD(0, 1);
                    LCDFunctions.sendString("Wait.......");
                    

                    //GwesPowerOffSystem();
                    Thread.Sleep(2000);
                    LCDFunctions.clearLCD();
                    Close();
                    //SetSystemPowerState(null, POWER_STATE_OFF, POWER_FORCE);
                    //KernelIoControl(16842812, IntPtr.Zero, 0, IntPtr.Zero, 0, IntPtr.Zero);
                    
                }
                else if (TempFrameBase == 5)
                {
                    if (LCDFrameProcessor.F5_LOCK_STATUS)
                    {
                        LCDFrameProcessor.F5_LOCK_STATUS = false;
                        LCDFrameProcessor.startOperation(TempFrameBase);
                    }
                }
                else if (TempFrameBase == 501)
                {
                    if (ProcessData.IsSpaceAvailable(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID))
                    {
                        object[] _tempUser = ProcessData.ReadEntry(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID);
                        if (_tempUser != null)
                        {
                            LCDFrameProcessor.ActiveFrameInfo.FrameData_Name = _tempUser[0].ToString();
                            LCDFrameProcessor.ActiveFrameInfo.FrameData_Phone = _tempUser[1].ToString();
                        }
                    }
                }
                else if (TempFrameBase == 110112)
                {
                    //Delete a Transaction Here
                    ExcelFileHandler.DeleteTransaction(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID, uint.Parse(LCDFrameProcessor.ActiveFrameInfo.day), uint.Parse(LCDFrameProcessor.ActiveFrameInfo.month), uint.Parse(LCDFrameProcessor.ActiveFrameInfo.year), LCDFrameProcessor.ActiveFrameInfo.whichShift);
                }
                else if (TempFrameBase == 110211)
                {
                    //Delete a Transaction Here
                    ExcelFileHandler.DeleteShift(uint.Parse(LCDFrameProcessor.ActiveFrameInfo.day), uint.Parse(LCDFrameProcessor.ActiveFrameInfo.month), uint.Parse(LCDFrameProcessor.ActiveFrameInfo.year), LCDFrameProcessor.ActiveFrameInfo.whichShift);
                }
                
            }
            else if (str == "N" || str == "n")
            {
                TempFrameBase = KeyboardtoFrameID("No");
                if (TempFrameBase == 502)
                {
                    if (ProcessData.IsSpaceAvailable(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID))
                    {
                        object[] _tempUser = ProcessData.ReadEntry(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID);
                        if (_tempUser != null)
                        {
                            LCDFrameProcessor.ActiveFrameInfo.FrameData_Name = _tempUser[0].ToString();
                            LCDFrameProcessor.ActiveFrameInfo.FrameData_Phone = _tempUser[1].ToString();
                        }
                    }
                }
                else if (TempFrameBase == 5)
                {
                    if (LCDFrameProcessor.F5_LOCK_STATUS)
                    {
                        LCDFrameProcessor.F5_LOCK_STATUS = false;
                        LCDFrameProcessor.startOperation(TempFrameBase);
                    }
                }
            }
            else
            {
                TempFrameBase = KeyboardtoFrameID(str); ;

                if (TempFrameBase == 5)
                {
                    if (LCDFrameProcessor.F5_LOCK_STATUS)
                    {
                        if (DataKeeper.IsKeyExist("F5_COLLECTION_MODE"))
                        {
                            if (DataKeeper.GetValue("F5_COLLECTION_MODE") == "1")
                            {
                                //auto mode
                                ManualorAutoMode = false;
                                LCDFrameProcessor.F5_LOCK_STATUS = false;
                            }
                            else if (DataKeeper.GetValue("F5_COLLECTION_MODE") == "0")
                            {
                                //manual mode
                                ManualorAutoMode = true;
                                LCDFrameProcessor.F5_LOCK_STATUS = false;
                            }
                            else
                            {
                                //ask mode
                                //Do Nothing
                            }
                        }
                        else
                        {
                            LCDFrameProcessor.F5_LOCK_STATUS = true;
                            //Key Doest Exist ..Go for ask mode
                            //Do nothing
                        }
                    }
                }

                else if (TempFrameBase == 50401) { 
                    //Check if Collection Already Existed -- Add Collection
                    KeyboardtoFrameID("Escape");
                    KeyboardtoFrameID("Escape");
                    //UInt64 same = F_state;



                    string _local_shift = "M";
                    if (DateTime.Now.Hour < 13)
                    {
                        _local_shift = "M";
                    }
                    else
                    {
                        _local_shift = "E";
                    }

                    if (ExcelFileHandler.IsCollectionExist(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID.ToString(), DateTime.Now.Day.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString(), _local_shift))
                    {
                        LCDFrameProcessor.customMessgae = "Entry Already Exist";
                        LCDFrameProcessor.customMessgae1 = "   Modify it !";
                        FrameBaseHistory.Push(123456);
                        TempFrameBase = 123456;
                        key_press++;
                    }
                    else {
                        if (ProcessData.IsSpaceAvailable(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID))
                        {
                            object[] _tempUser = ProcessData.ReadEntry(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID);
                            if (_tempUser != null)
                            {
                                
                                LCDFrameProcessor.ActiveFrameInfo.FrameData_Name = _tempUser[0].ToString();
                                if (LCDFrameProcessor.ActiveFrameInfo.FrameData_Name == "") {
                                    LCDFrameProcessor.ActiveFrameInfo.FrameData_Name = "---";
                                }
                                LCDFrameProcessor.ActiveFrameInfo.FrameData_Phone = _tempUser[1].ToString();
                                if (LCDFrameProcessor.ActiveFrameInfo.FrameData_Phone == "")
                                {
                                    LCDFrameProcessor.ActiveFrameInfo.FrameData_Phone = "---";
                                }
                            }
                        }
                        if (ManualorAutoMode)
                        {
                            FrameBaseHistory.Push(501);
                            TempFrameBase = 501;
                            F_state = 501;
                            key_press++;
                        }
                        else {
                            FrameBaseHistory.Push(502);
                            TempFrameBase = 502;
                            F_state = 502;
                            key_press++;
                        }
                    }
                }
                else if (TempFrameBase == 50402) {
                    //Check if Collection Existed -- Delete Collection
                    KeyboardtoFrameID("Escape");
                    KeyboardtoFrameID("Escape");
                    //UInt64 same = F_state;
                    string _local_shift = "M";
                    if (DateTime.Now.Hour < 13)
                    {
                        _local_shift = "M";
                    }
                    else
                    {
                        _local_shift = "E";
                    }

                    if (ExcelFileHandler.IsCollectionExist(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID.ToString(), DateTime.Now.Day.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString(), _local_shift))
                    {
                        //Delete it
                        ExcelFileHandler.DeleteCollection_ID(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID.ToString(), DateTime.Now.Day.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString(), _local_shift);
                        LCDFrameProcessor.customMessgae = "Deleted";
                        LCDFrameProcessor.customMessgae1 = "";
                        FrameBaseHistory.Push(123456);
                        TempFrameBase = 123456;
                        key_press++;
                    }
                    else
                    {
                        LCDFrameProcessor.customMessgae = "No Entry Found";
                        LCDFrameProcessor.customMessgae1 = "";
                        FrameBaseHistory.Push(123456);
                        TempFrameBase = 123456;
                        key_press++;

                    }
                }
                else if (TempFrameBase == 50403) {
                    //Check if Collection Existed -- Modify Collection
                    KeyboardtoFrameID("Escape");
                    KeyboardtoFrameID("Escape");
                    //UInt64 same = F_state;
                    string _local_shift = "M";
                    if (DateTime.Now.Hour < 13)
                    {
                        _local_shift = "M";
                    }
                    else
                    {
                        _local_shift = "E";
                    }

                    if (ExcelFileHandler.IsCollectionExist(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID.ToString(), DateTime.Now.Day.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString(), _local_shift))
                    {
                        if (ProcessData.IsSpaceAvailable(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID))
                        {
                            object[] _tempUser = ProcessData.ReadEntry(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID);
                            if (_tempUser != null)
                            {
                                LCDFrameProcessor.ActiveFrameInfo.FrameData_Name = _tempUser[0].ToString();
                                if (LCDFrameProcessor.ActiveFrameInfo.FrameData_Name == "")
                                {
                                    LCDFrameProcessor.ActiveFrameInfo.FrameData_Name = "---";
                                }
                                LCDFrameProcessor.ActiveFrameInfo.FrameData_Phone = _tempUser[1].ToString();
                                if (LCDFrameProcessor.ActiveFrameInfo.FrameData_Phone == "")
                                {
                                    LCDFrameProcessor.ActiveFrameInfo.FrameData_Phone = "---";
                                }
                            }
                        }

                        if (ManualorAutoMode)
                        {
                            FrameBaseHistory.Push(501);
                            TempFrameBase = 501;
                            F_state = 501;
                            key_press++;
                        }
                        else
                        {
                            FrameBaseHistory.Push(502);
                            TempFrameBase = 502;
                            F_state = 502;
                            key_press++;
                        }
                    }
                    else
                    {
                        LCDFrameProcessor.customMessgae = "Doesn't Exist";
                        LCDFrameProcessor.customMessgae1 = "Add it First !";
                        FrameBaseHistory.Push(123456);
                        TempFrameBase = 123456;
                        key_press++;
                        
                    }

                }
                else if (TempFrameBase == 50404) { 
                    //Check Collection Already Exited -- Print Collection

                    KeyboardtoFrameID("Escape");
                    KeyboardtoFrameID("Escape");
                    //UInt64 same = F_state;
                    string _local_shift = "M";
                    if (DateTime.Now.Hour < 13)
                    {
                        _local_shift = "M";
                    }
                    else
                    {
                        _local_shift = "E";
                    }

                    if (ExcelFileHandler.IsCollectionExist(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID.ToString(), DateTime.Now.Day.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString(), _local_shift))
                    {
                        //Print it
                        Printer.printCollection_ID(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID.ToString(), DateTime.Now.Day.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString(), _local_shift,false);
                        LCDFrameProcessor.customMessgae = "Printing";
                        LCDFrameProcessor.customMessgae1 = "";
                        FrameBaseHistory.Push(123456);
                        TempFrameBase = 123456;
                        key_press++;
                    }
                    else
                    {
                        LCDFrameProcessor.customMessgae = "No Entry Found";
                        LCDFrameProcessor.customMessgae1 = "";
                        FrameBaseHistory.Push(123456);
                        TempFrameBase = 123456;
                        key_press++;

                    }
                }
                else if (TempFrameBase == 12) {
                    if (LCDFrameProcessor.F12_PASSWORD_STATE)
                    {
                        if (DataKeeper.IsKeyExist("F12_PASSWORD"))
                        {
                            if (DataKeeper.GetValue("F12_PASSWORD") == "")
                            {
                                //Password not Set
                                LCDFrameProcessor.F12_PASSWORD_STATE = false;
                            }
                            else
                            {
                                //Verify Password
                            }

                        }
                        else
                        {
                            LCDFrameProcessor.F12_PASSWORD_STATE = false;
                        }
                    }
                }
                else if (TempFrameBase == 2)
                {
                    if (LCDFrameProcessor.F2_PASSWORD_STATE)
                    {
                        if (DataKeeper.IsKeyExist("F2_PASSWORD"))
                        {
                            if (DataKeeper.GetValue("F2_PASSWORD") == "")
                            {
                                //Password not Set
                                LCDFrameProcessor.F2_PASSWORD_STATE = false;
                            }
                            else
                            {
                                //Verify Password
                            }

                        }
                        else
                        {
                            LCDFrameProcessor.F2_PASSWORD_STATE = false;
                        }
                    }
                }
                else if (TempFrameBase == 10)
                {
                    if (LCDFrameProcessor.F10_PASSWORD_STATE)
                    {
                        if (DataKeeper.IsKeyExist("F10_PASSWORD"))
                        {
                            if (DataKeeper.GetValue("F10_PASSWORD") == "")
                            {
                                //Password not Set
                                LCDFrameProcessor.F10_PASSWORD_STATE = false;
                            }
                            else
                            {
                                //Verify Password
                            }

                        }
                        else
                        {
                            LCDFrameProcessor.F10_PASSWORD_STATE = false;
                        }
                    }
                }

            }


            if (!(TempFrameBase == LCDFrameProcessor.FrameBase))
            {

                LCDFrameProcessor.startOperation(TempFrameBase);
            }

            if (shouldChangeDatabase) {
                shouldChangeDatabase = false;
                ProcessData.SaveChanges();
            }

            if (shouldChangeDataSetting) {
                shouldChangeDataSetting = false;
                //Do Operation
            }



            if (LCDFrameProcessor.FrameBase == 10113) {
                //Added
                Thread.Sleep(500);
                KeyboardtoFrameID("Escape");
                KeyboardtoFrameID("Escape");
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));

            }
            else if (LCDFrameProcessor.FrameBase == 10121)
            {
                //Deleted
                Thread.Sleep(500);
                KeyboardtoFrameID("Escape");
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if (LCDFrameProcessor.FrameBase == 10211)
            {
                //Modified
                Thread.Sleep(500);
                //Go to 102
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if(LCDFrameProcessor.FrameBase == 2010110)
            {
                Thread.Sleep(500);
                DataKeeper.AddOrUpdate("F2_COW_FATRATEKG", LCDFrameProcessor.ActiveFrameInfo.float5_0data, false);
                DataKeeper.AddOrUpdate("F2_COW_SNFRATEKG", LCDFrameProcessor.ActiveFrameInfo.float5_1data, true);
                KeyboardtoFrameID("Escape");
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));  
            }
            else if (LCDFrameProcessor.FrameBase == 2010210)
            {
                Thread.Sleep(500);
                DataKeeper.AddOrUpdate("F2_BUF_FATRATEKG", LCDFrameProcessor.ActiveFrameInfo.float5_0data, false);
                DataKeeper.AddOrUpdate("F2_BUF_SNFRATEKG", LCDFrameProcessor.ActiveFrameInfo.float5_1data, true);
                KeyboardtoFrameID("Escape");
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));  
            }
            else if (LCDFrameProcessor.FrameBase == 2010511)
            {
                //Modified
                Thread.Sleep(500);
                //Go to 102
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if (LCDFrameProcessor.FrameBase == 2020110) {
                Thread.Sleep(500);
                DataKeeper.AddOrUpdate("F2_COW_FATBONUSLIMIT_LOWER", LCDFrameProcessor.ActiveFrameInfo.float1_0data, false);
                DataKeeper.AddOrUpdate("F2_COW_FATBONUSLIMIT_UPPER", LCDFrameProcessor.ActiveFrameInfo.float1_1data, false);
                DataKeeper.AddOrUpdate("F2_COW_FATBONUSAMOUNT", LCDFrameProcessor.ActiveFrameInfo.float4_0data, true);
                KeyboardtoFrameID("Escape");
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if (LCDFrameProcessor.FrameBase == 2020210)
            {
                Thread.Sleep(500);
                DataKeeper.AddOrUpdate("F2_BUF_FATBONUSLIMIT_LOWER", LCDFrameProcessor.ActiveFrameInfo.float1_0data, false);
                DataKeeper.AddOrUpdate("F2_BUF_FATBONUSLIMIT_UPPER", LCDFrameProcessor.ActiveFrameInfo.float1_1data, false);
                DataKeeper.AddOrUpdate("F2_BUF_FATBONUSAMOUNT", LCDFrameProcessor.ActiveFrameInfo.float4_0data, true);
                KeyboardtoFrameID("Escape");
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if (LCDFrameProcessor.FrameBase == 2020310)
            {
                Thread.Sleep(500);
                DataKeeper.AddOrUpdate("F2_COW_RATECHART", LCDFrameProcessor.ActiveFrameInfo.whichRatechart.ToString(), true);
                
                KeyboardtoFrameID("Escape");
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if (LCDFrameProcessor.FrameBase == 2020410)
            {
                Thread.Sleep(500);
                DataKeeper.AddOrUpdate("F2_BUF_RATECHART", LCDFrameProcessor.ActiveFrameInfo.whichRatechart.ToString(), true);

                KeyboardtoFrameID("Escape");
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if (LCDFrameProcessor.FrameBase == 2020510)
            {
                Thread.Sleep(500);
                DataKeeper.AddOrUpdate("F2_COW_FATCUTOFF_MIN", LCDFrameProcessor.ActiveFrameInfo.float4_0data, false);
                DataKeeper.AddOrUpdate("F2_COW_FATCUTOFF_MAX", LCDFrameProcessor.ActiveFrameInfo.float4_1data, false);

                DataKeeper.AddOrUpdate("F2_COW_SNFCUTOFF_MIN", LCDFrameProcessor.ActiveFrameInfo.float4_2data, false);
                DataKeeper.AddOrUpdate("F2_COW_SNFCUTOFF_MAX", LCDFrameProcessor.ActiveFrameInfo.float4_3data, false);

                DataKeeper.AddOrUpdate("F2_COW_CLRCUTOFF_MIN", LCDFrameProcessor.ActiveFrameInfo.float4_4data, false);
                DataKeeper.AddOrUpdate("F2_COW_CLRCUTOFF_MAX", LCDFrameProcessor.ActiveFrameInfo.float4_5data, true);
                
                KeyboardtoFrameID("Escape");
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if (LCDFrameProcessor.FrameBase == 2020610)
            {
                Thread.Sleep(500);
                DataKeeper.AddOrUpdate("F2_BUF_FATCUTOFF_MIN", LCDFrameProcessor.ActiveFrameInfo.float4_0data, false);
                DataKeeper.AddOrUpdate("F2_BUF_FATCUTOFF_MAX", LCDFrameProcessor.ActiveFrameInfo.float4_1data, false);

                DataKeeper.AddOrUpdate("F2_BUF_SNFCUTOFF_MIN", LCDFrameProcessor.ActiveFrameInfo.float4_2data, false);
                DataKeeper.AddOrUpdate("F2_BUF_SNFCUTOFF_MAX", LCDFrameProcessor.ActiveFrameInfo.float4_3data, false);

                DataKeeper.AddOrUpdate("F2_BUF_CLRCUTOFF_MIN", LCDFrameProcessor.ActiveFrameInfo.float4_4data, false);
                DataKeeper.AddOrUpdate("F2_BUF_CLRCUTOFF_MAX", LCDFrameProcessor.ActiveFrameInfo.float4_5data, true);

                KeyboardtoFrameID("Escape");
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if (LCDFrameProcessor.FrameBase == 2020710)
            {
                Thread.Sleep(500);
                DataKeeper.AddOrUpdate("F2_VALUE_A", LCDFrameProcessor.ActiveFrameInfo.float4_0data, false);
                DataKeeper.AddOrUpdate("F2_VALUE_B", LCDFrameProcessor.ActiveFrameInfo.float4_1data, false);

                DataKeeper.AddOrUpdate("F2_CLR_CON", LCDFrameProcessor.ActiveFrameInfo.float4_2data, true);
                

                KeyboardtoFrameID("Escape");
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if (LCDFrameProcessor.FrameBase == 20301) { 
                //Print COW Rate Chart
                Thread.Sleep(500);
                
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if (LCDFrameProcessor.FrameBase == 20302)
            {
                //Print COW Rate Chart
                Thread.Sleep(500);
                
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if (LCDFrameProcessor.FrameBase == 20208010110) {
                Thread.Sleep(500);
                
                UInt64 prev_frame = KeyboardtoFrameID("Escape");

                double l_limit = 0;
                double u_limit = 0;
                double amount = 0;

                try
                {
                    l_limit = double.Parse(LCDFrameProcessor.ActiveFrameInfo.float3_1data);
                }
                catch { }

                try
                {
                    u_limit = double.Parse(LCDFrameProcessor.ActiveFrameInfo.float3_0data);
                }
                catch { }

                try
                {
                    amount = double.Parse(LCDFrameProcessor.ActiveFrameInfo.float6_0data);
                }
                catch(Exception ex) { }

                    try
                    {
                        DataKeeper.AddOrUpdate(LCDFrameProcessor.DEDorBON+"_"+LCDFrameProcessor.COWorBUFD+"_"+LCDFrameProcessor.FATorSNFD+"_SLOT"+LCDFrameProcessor._slot_id.ToString(), "L_LIMIT " + Math.Round(l_limit, 1).ToString() + "END-U_LIMIT " + Math.Round(u_limit, 1).ToString() + "END-AMOUNT " + Math.Round(amount, 1).ToString() + "END",true);
                    }
                    catch (Exception ex) { }

                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if (LCDFrameProcessor.FrameBase == 20410)
            {
                Thread.Sleep(500);
                DataKeeper.AddOrUpdate("F2_PASSWORD", LCDFrameProcessor.ActiveFrameInfo.passwordGlobalReal, true);
                KeyboardtoFrameID("Escape");
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if (LCDFrameProcessor.FrameBase == 301)
            {
                //Time Updated
                int _year = int.Parse("20" + LCDFrameProcessor.ActiveFrameInfo.year);
                int _month = int.Parse(LCDFrameProcessor.ActiveFrameInfo.month);
                int _day = int.Parse(LCDFrameProcessor.ActiveFrameInfo.day);
                int _hour = int.Parse(LCDFrameProcessor.ActiveFrameInfo.hour);
                int _minute = int.Parse(LCDFrameProcessor.ActiveFrameInfo.minute);
                int _second = int.Parse(LCDFrameProcessor.ActiveFrameInfo.second);


                DateTimeUpdate.UpdateTime(_year, _month, _day, _hour, _minute, _second, (int)LCDFrameProcessor.ActiveFrameInfo.whichDayofWeek);
                Thread.Sleep(500);
                //Go to 00 Home
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if (LCDFrameProcessor.FrameBase == 40111 || LCDFrameProcessor.FrameBase == 40112)
            {
                Thread.Sleep(500);
                DataKeeper.AddOrUpdate("DAIRY_PASSWORD", LCDFrameProcessor.ActiveFrameInfo.newPassword, true);
                KeyboardtoFrameID("Escape");
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if (LCDFrameProcessor.FrameBase == 40210)
            {
                Thread.Sleep(500);
                DataKeeper.AddOrUpdate("SCREEN_TITTLE_SELECTED", LCDFrameProcessor.ActiveFrameInfo.screenTittle, true);
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if (LCDFrameProcessor.FrameBase == 40310)
            {
                Thread.Sleep(500);
                //Machine Updated !
                DataKeeper.AddOrUpdate("F4_MACHINE_SELECTED", LCDFrameProcessor.ActiveFrameInfo.ActiveMachine.ToString(), true);
                //DataKeeper.SaveChanges(); //nO need for this This is already updated in Function Itsself
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if (LCDFrameProcessor.FrameBase == 40410)
            {
                Thread.Sleep(500);
                DataKeeper.AddOrUpdate("F4_PRINTER_SELECTED", LCDFrameProcessor.ActiveFrameInfo.ActivePrinter.ToString(), true);
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if (LCDFrameProcessor.FrameBase == 40510)
            {
                Thread.Sleep(500);
                DataKeeper.AddOrUpdate("F4_WEIGH_SCALE_SELECTED", LCDFrameProcessor.ActiveFrameInfo.ActiveWeigh.ToString(), true);
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if (LCDFrameProcessor.FrameBase == 40611 || LCDFrameProcessor.FrameBase == 40612)
            {
                Thread.Sleep(500);
                KeyboardtoFrameID("Escape");
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if (LCDFrameProcessor.FrameBase == 5)
            {
                collection_AutoMode.Enabled = false;
                SerialP.StopReceiving(new int[] { 0, 1 });
            }
            else if (LCDFrameProcessor.FrameBase == 501)
            {
                if (!ProcessData.IsSpaceAvailable(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID))
                {
                    
                    Thread.Sleep(500);
                    LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
                }

            }
            else if (LCDFrameProcessor.FrameBase == 502)
            {
                //Run All Procedure , to make automatic mode operational
                SerialP.StartReceiving(new int[] { 0, 1 });
                //collection_AutoMode.Interval = 500;
                //collection_AutoMode.Enabled = true;
            }

            else if (LCDFrameProcessor.FrameBase == 50110)
            {
                //Save Excel Here
                Thread.Sleep(500);

                UInt64 _xx = KeyboardtoFrameID("Escape");

                if (_xx == 501)
                {
                    double temp_fat = 0;
                    double temp_snf = 0;
                    double temp_milkw = 0;
                    if (LCDFrameProcessor.ActiveFrameInfo.float6_0data != "" && LCDFrameProcessor.ActiveFrameInfo.float6_0data != null)
                    {
                        temp_milkw = double.Parse(LCDFrameProcessor.ActiveFrameInfo.float6_0data);
                    }

                    if (LCDFrameProcessor.ActiveFrameInfo.float4_0data != "" && LCDFrameProcessor.ActiveFrameInfo.float4_0data != null)
                    {
                        temp_fat = double.Parse(LCDFrameProcessor.ActiveFrameInfo.float4_0data);
                    }

                    if (LCDFrameProcessor.ActiveFrameInfo.float4_1data != "" && LCDFrameProcessor.ActiveFrameInfo.float4_1data != null)
                    {
                        temp_snf = double.Parse(LCDFrameProcessor.ActiveFrameInfo.float4_1data);
                    }

                    string _local_shift = "M";
                    if (DateTime.Now.Hour < 13)
                    {
                        _local_shift = "M";
                    }
                    else
                    {
                        _local_shift = "E";
                    }

                    double _amount = 0;
                    if (LCDFrameProcessor.ActiveFrameInfo.float7_0data != "")
                    {
                        _amount = double.Parse(LCDFrameProcessor.ActiveFrameInfo.float7_0data);
                    }

                    float local_cash = (float)_amount;
                    float local_balance = 0;

                    

                    //Deduct Amount Automatically
                    object[] temp_bank = ExcelFileHandler.BankSheet_GetAmount(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID);

                    if ((bool)temp_bank[1] && (float)temp_bank[0] > 0) {
                        if ((float)temp_bank[0] >= local_cash)
                        {
                            ExcelFileHandler.BankSheet_Deduct(local_cash, "FROM F5 COLLECTION", LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID);

                            local_balance = local_cash;
                            local_cash = 0;
                        }
                        else {
                            local_balance = (float)temp_bank[0];
                            local_cash -= local_balance;
                            ExcelFileHandler.BankSheet_Deduct(local_balance, "FROM F5 COLLECTION", LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID);
                        }
                    }
                    
                    ExcelFileHandler.WriteCellCollection(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID.ToString(), LCDFrameProcessor.ActiveFrameInfo.FrameData_Name, LCDFrameProcessor.ActiveFrameInfo.FrameData_Phone, temp_milkw, temp_fat, temp_snf, 0, _amount, LCDFrameProcessor.COWorBUF[LCDFrameProcessor.ActiveFrameInfo.whichShift], _local_shift, 0, 0, 0, 0, 0, 0, 0, "Manual",local_cash,local_balance);


                    SerialP.sendDirect(1, "Collection Slip");
                    SerialP.sendDirect(1, DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year.ToString() + "(" + _local_shift + ") " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString());
                    SerialP.sendDirect(1, "Member: " + LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID.ToString() + "  " + ProcessData.GetName(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID));
                    SerialP.sendDirect(1, "------------------------------");
                    SerialP.sendDirect(1, "Quantity : " + temp_milkw.ToString());
                    SerialP.sendDirect(1, "Category : " + LCDFrameProcessor.COWorBUF[LCDFrameProcessor.ActiveFrameInfo.whichShift]);
                    SerialP.sendDirect(1, "Cash Amount : " + local_cash.ToString());
                    SerialP.sendDirect(1, "Bank Amount : " + local_balance.ToString());
                    SerialP.sendDirect(1, "Total Amount : " + _amount.ToString());
                    SerialP.sendDirect(1, "------------------------------");
                    SerialP.sendDirect(1, "        Your Company          ");
                    SerialP.sendDirect(1, "");
                    SerialP.sendDirect(1, "");
                    
                }
                else if (_xx == 502)
                {
                    collection_AutoMode.Enabled = false;
                    SerialP.StopReceiving(new int[] { 0, 1 });

                    string _local_shift = "M";
                    if (DateTime.Now.Hour < 13)
                    {
                        _local_shift = "M";
                    }
                    else
                    {
                        _local_shift = "E";
                    }

                    double _amount = 0;
                    if (LCDFrameProcessor.ActiveFrameInfo.float7_0data != "")
                    {
                        _amount = double.Parse(LCDFrameProcessor.ActiveFrameInfo.float7_0data);
                    }

                    float local_cash = (float)_amount;
                    float local_balance = 0;



                    //Deduct Amount Automatically
                    object[] temp_bank = ExcelFileHandler.BankSheet_GetAmount(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID);

                    if ((bool)temp_bank[1] && (float)temp_bank[0] > 0)
                    {
                        if ((float)temp_bank[0] >= local_cash)
                        {
                            ExcelFileHandler.BankSheet_Deduct(local_cash, "FROM F5 COLLECTION", LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID);

                            local_balance = local_cash;
                            local_cash = 0;
                        }
                        else
                        {
                            local_balance = (float)temp_bank[0];
                            local_cash -= local_balance;
                            ExcelFileHandler.BankSheet_Deduct(local_balance, "FROM F5 COLLECTION", LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID);
                        }
                    }

                    ExcelFileHandler.WriteCellCollection(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID.ToString(), LCDFrameProcessor.ActiveFrameInfo.FrameData_Name, LCDFrameProcessor.ActiveFrameInfo.FrameData_Phone, RecieverDeviceData.WEIGH_WEIGHT, RecieverDeviceData.ANALYSER_FAT, RecieverDeviceData.ANALYSER_SNF, 0, _amount, LCDFrameProcessor.COWorBUF[LCDFrameProcessor.ActiveFrameInfo.whichShift], _local_shift, RecieverDeviceData.ANALYSER_LACTOSE, RecieverDeviceData.ANALYSER_PROTEIN, RecieverDeviceData.ANALYSER_SOLID, RecieverDeviceData.ANALYSER_FRPOINT, RecieverDeviceData.ANALYSER_TEMP, RecieverDeviceData.ANALYSER_ADDEDWATER, RecieverDeviceData.ANALYSER_DENSITY, "Automatic",local_cash,local_balance);
                    SerialP.sendDirect(1, "Collection Slip");
                    SerialP.sendDirect(1, DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year.ToString() + "(" + _local_shift+") "+DateTime.Now.Hour.ToString() +":"+DateTime.Now.Minute.ToString());
                    SerialP.sendDirect(1, "Member: "+LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID.ToString()+"  "+ProcessData.GetName(LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID) );
                    SerialP.sendDirect(1, "------------------------------");
                    SerialP.sendDirect(1, "Quantity : "+RecieverDeviceData.ANALYSER_WEIGHT.ToString());
                    SerialP.sendDirect(1, "Category : " + LCDFrameProcessor.COWorBUF[LCDFrameProcessor.ActiveFrameInfo.whichShift]);
                    SerialP.sendDirect(1, "Cash Amount : " + local_cash.ToString());
                    SerialP.sendDirect(1, "Bank Amount : " + local_balance.ToString());
                    SerialP.sendDirect(1, "Total Amount : " + _amount.ToString());
                    SerialP.sendDirect(1, "------------------------------");
                    SerialP.sendDirect(1, "        Your Company          ");
                    SerialP.sendDirect(1, "");
                    SerialP.sendDirect(1, "");
                }


                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if (LCDFrameProcessor.FrameBase == 601) {
                Thread.Sleep(500);
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if (LCDFrameProcessor.FrameBase == 701)
            {
                //SerialP.AddTask(2, "Hello`");
                Thread.Sleep(500);
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if (LCDFrameProcessor.FrameBase == 80210)
            {
                //Print and Save Something
                //SerialP.AddTask(2, "Printing......");
                Thread.Sleep(500);
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if (LCDFrameProcessor.FrameBase == 8070110)
            {
                Thread.Sleep(500);
                DataKeeper.AddOrUpdate("LOCAL_COW_RATE", LCDFrameProcessor.ActiveFrameInfo.float5_0data, false);
                DataKeeper.AddOrUpdate("LOCAL_BUF_RATE", LCDFrameProcessor.ActiveFrameInfo.float5_1data, true);
                KeyboardtoFrameID("Escape");
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if (LCDFrameProcessor.FrameBase == 8070210)
            {
                Thread.Sleep(500);
                DataKeeper.AddOrUpdate("TRUCK_COW_RATE", LCDFrameProcessor.ActiveFrameInfo.float5_0data, false);
                DataKeeper.AddOrUpdate("TRUCK_BUF_RATE", LCDFrameProcessor.ActiveFrameInfo.float5_1data, true);
                KeyboardtoFrameID("Escape");
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if (LCDFrameProcessor.FrameBase == 90110)
            {
                //Print and Save Something
                //SerialP.AddTask(2, "Printing......");
                Thread.Sleep(500);
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if (LCDFrameProcessor.FrameBase == 10040110)
            {
                Thread.Sleep(1000);
                DataKeeper.AddOrUpdate("F10_PASSWORD", LCDFrameProcessor.ActiveFrameInfo.passwordGlobalReal, true);
                KeyboardtoFrameID("Escape");
                KeyboardtoFrameID("Escape");
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if (LCDFrameProcessor.FrameBase == 120410)
            {
                //Change Password
                Thread.Sleep(500);
                DataKeeper.AddOrUpdate("F12_PASSWORD", LCDFrameProcessor.ActiveFrameInfo.passwordGlobalReal, true);
                KeyboardtoFrameID("Escape");
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));
            }
            else if (LCDFrameProcessor.FrameBase == 12010110)
            {
                //Save Usersetting COM port and Back to home
                Thread.Sleep(500);
                DataKeeper.AddOrUpdate("COM1_BAUD_RATE", LCDFrameProcessor.ActiveFrameInfo.whichBaudRate.ToString(), false);
                DataKeeper.AddOrUpdate("COM1_DATA_BIT", LCDFrameProcessor.ActiveFrameInfo.whichDataBit.ToString(), false);
                DataKeeper.AddOrUpdate("COM1_STOP_BIT", LCDFrameProcessor.ActiveFrameInfo.whichStopBit.ToString(), false);
                DataKeeper.AddOrUpdate("COM1_HANDSHAKE", LCDFrameProcessor.ActiveFrameInfo.whichHandShake.ToString(), false);
                DataKeeper.AddOrUpdate("COM1_PARITY", LCDFrameProcessor.ActiveFrameInfo.whichParity.ToString(), true);
                SerialP.ChangeCOMSettings("COM1:");
                KeyboardtoFrameID("Escape");
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));

            }
            else if (LCDFrameProcessor.FrameBase == 12010210)
            {
                //Save Usersetting COM port and Back to home
                Thread.Sleep(500);
                DataKeeper.AddOrUpdate("COM2_BAUD_RATE", LCDFrameProcessor.ActiveFrameInfo.whichBaudRate.ToString(), false);
                DataKeeper.AddOrUpdate("COM2_DATA_BIT", LCDFrameProcessor.ActiveFrameInfo.whichDataBit.ToString(), false);
                DataKeeper.AddOrUpdate("COM2_STOP_BIT", LCDFrameProcessor.ActiveFrameInfo.whichStopBit.ToString(), false);
                DataKeeper.AddOrUpdate("COM2_HANDSHAKE", LCDFrameProcessor.ActiveFrameInfo.whichHandShake.ToString(), false);
                DataKeeper.AddOrUpdate("COM2_PARITY", LCDFrameProcessor.ActiveFrameInfo.whichParity.ToString(), true);
                SerialP.ChangeCOMSettings("COM2:");
                KeyboardtoFrameID("Escape");
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));

            }
            else if (LCDFrameProcessor.FrameBase == 12010310)
            {
                //Save Usersetting COM port and Back to home
                Thread.Sleep(500);
                DataKeeper.AddOrUpdate("COM3_BAUD_RATE", LCDFrameProcessor.ActiveFrameInfo.whichBaudRate.ToString(), false);
                DataKeeper.AddOrUpdate("COM3_DATA_BIT", LCDFrameProcessor.ActiveFrameInfo.whichDataBit.ToString(), false);
                DataKeeper.AddOrUpdate("COM3_STOP_BIT", LCDFrameProcessor.ActiveFrameInfo.whichStopBit.ToString(), false);
                DataKeeper.AddOrUpdate("COM3_HANDSHAKE", LCDFrameProcessor.ActiveFrameInfo.whichHandShake.ToString(), false);
                DataKeeper.AddOrUpdate("COM3_PARITY", LCDFrameProcessor.ActiveFrameInfo.whichParity.ToString(), true);
                SerialP.ChangeCOMSettings("COM3:");
                KeyboardtoFrameID("Escape");
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));

            }
            else if (LCDFrameProcessor.FrameBase == 12030410)
            {
                //Save Usersetting COM port and Back to home
                Thread.Sleep(500);

                if (LCDFrameProcessor.ActiveFrameInfo.toggleSwitch == 0)
                {
                    //Export
                    if (LCDFrameProcessor.ActiveFrameInfo.shiftSwitch3_0 == 2)
                    {
                        //Export Rate List
                        if (File.Exists(ExcelFileHandler.collection_xlsfolder + @"\ratechart.xls"))
                        {
                            if (!Directory.Exists(@"\USB HD\Excel"))
                                Directory.CreateDirectory(@"\USB HD\Excel");

                            File.Copy(ExcelFileHandler.collection_xlsfolder + @"\ratechart.xls", @"\USB HD\Excel\ratechart.xls", true);
                        }
                    }
                    else if (LCDFrameProcessor.ActiveFrameInfo.shiftSwitch3_0 == 0)
                    {
                        //Export All
                        if (Directory.Exists(ExcelFileHandler.collection_xlsfolder))
                        {
                            if (!Directory.Exists(@"\USB HD\Excel"))
                                Directory.CreateDirectory(@"\USB HD\Excel");
                            var allFiles = Directory.GetFiles(ExcelFileHandler.collection_xlsfolder, "*.xls");
                            if (allFiles != null)
                            {
                                foreach (string _name in allFiles)
                                {
                                    //if (_name != ExcelFileHandler.collection_xlsfolder + @"\ratechart.xls") {

                                    File.Copy(_name, @"\USB HD\Excel\" + _name.Substring(_name.LastIndexOf("\\") + 1), true);

                                    //}
                                }
                            }
                        }

                    }
                    else if (LCDFrameProcessor.ActiveFrameInfo.shiftSwitch3_0 == 1)
                    {
                        //Export Collection
                        if (Directory.Exists(ExcelFileHandler.collection_xlsfolder))
                        {
                            if (!Directory.Exists(@"\USB HD\Excel"))
                                Directory.CreateDirectory(@"\USB HD\Excel");
                            var allFiles = Directory.GetFiles(ExcelFileHandler.collection_xlsfolder, "*.xls");
                            if (allFiles != null)
                            {
                                foreach (string _name in allFiles)
                                {
                                    if (_name != ExcelFileHandler.collection_xlsfolder + @"\ratechart.xls")
                                    {
                                        string _lfname = _name.Substring(_name.LastIndexOf("\\") + 1);
                                        File.Copy(_name, @"\USB HD\Excel\" + _lfname, true);

                                    }
                                }
                            }
                        }
                    }
                }
                else if (LCDFrameProcessor.ActiveFrameInfo.toggleSwitch == 1)
                {
                    //Import
                    if (LCDFrameProcessor.ActiveFrameInfo.shiftSwitch3_0 == 0)
                    {
                        //Import All
                        LCDFrameProcessor.customMessgae = "Not Available";
                        LCDFrameProcessor.customMessgae1 = "";
                        LCDFrameProcessor.startOperation(123456);
                        Thread.Sleep(500);
                    }
                    else if (LCDFrameProcessor.ActiveFrameInfo.shiftSwitch3_0 == 1)
                    {
                        //Import Collection
                        LCDFrameProcessor.customMessgae = "Not Availailble";
                        LCDFrameProcessor.customMessgae1 = "";
                        LCDFrameProcessor.startOperation(123456);
                        Thread.Sleep(500);
                    }
                    else if (LCDFrameProcessor.ActiveFrameInfo.shiftSwitch3_0 == 2)
                    {
                        //Import RateChart
                        if (File.Exists(@"\USB HD\Excel\ratechart.xls"))
                        {
                            if (!Directory.Exists(ExcelFileHandler.collection_xlsfolder))
                                Directory.CreateDirectory(ExcelFileHandler.collection_xlsfolder);

                            File.Copy(@"\USB HD\Excel\ratechart.xls", ExcelFileHandler.collection_xlsfolder + @"\ratechart.xls", true);
                        }
                        else
                        {
                            LCDFrameProcessor.customMessgae = "RateChart Not Found";
                            LCDFrameProcessor.customMessgae1 = "";
                            LCDFrameProcessor.startOperation(123456);
                            Thread.Sleep(500);
                        }
                    }

                }


                KeyboardtoFrameID("Escape");
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));

            }
            else if (LCDFrameProcessor.FrameBase == 12030411)
            {
                Thread.Sleep(500);
                KeyboardtoFrameID("Escape");
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));

            }
            else if (LCDFrameProcessor.FrameBase == 12020110)
            {
                //Save Usersetting COM port and Back to home
                Thread.Sleep(500);
                UInt64 _xx = KeyboardtoFrameID("Escape");
                if (_xx == 120201)
                {
                    DataKeeper.AddOrUpdate("F5_COLLECTION_MODE", LCDFrameProcessor.ActiveFrameInfo.ActivePrinter.ToString(), true);
                }
                else if (_xx == 120301)
                {
                    DataKeeper.AddOrUpdate("DEVICE_MAN_COM1_TX_DEVICE", LCDFrameProcessor.ActiveFrameInfo.shiftSwitch3_0.ToString(), false);
                    DataKeeper.AddOrUpdate("DEVICE_MAN_COM1_RX_DEVICE", LCDFrameProcessor.ActiveFrameInfo.shiftSwitch3_1.ToString(), true);
                }
                else if (_xx == 120302)
                {
                    DataKeeper.AddOrUpdate("DEVICE_MAN_COM2_TX_DEVICE", LCDFrameProcessor.ActiveFrameInfo.shiftSwitch3_0.ToString(), false);
                    DataKeeper.AddOrUpdate("DEVICE_MAN_COM2_RX_DEVICE", LCDFrameProcessor.ActiveFrameInfo.shiftSwitch3_1.ToString(), true);
                }
                else if (_xx == 120303)
                {
                    DataKeeper.AddOrUpdate("DEVICE_MAN_COM3_TX_DEVICE", LCDFrameProcessor.ActiveFrameInfo.shiftSwitch3_0.ToString(), false);
                    DataKeeper.AddOrUpdate("DEVICE_MAN_COM3_RX_DEVICE", LCDFrameProcessor.ActiveFrameInfo.shiftSwitch3_1.ToString(), true);
                }

                if (_xx == 120301 || _xx == 120302 || _xx == 120303)
                {
                    SerialP.ChangeReceiverPhysicalMAP();
                    SerialP.ChangeTransmitterPhysicalMAP();
                }

                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));

            }
            else if (LCDFrameProcessor.FrameBase == 110112)
            {
                //Save Usersetting COM port and Back to home
                Thread.Sleep(500);
                KeyboardtoFrameID("Escape");
                KeyboardtoFrameID("Escape");
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));

            }
            else if (LCDFrameProcessor.FrameBase == 110211)
            {
                //Save Usersetting COM port and Back to home
                Thread.Sleep(500);
                KeyboardtoFrameID("Escape");
                KeyboardtoFrameID("Escape");
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));

            }
            else if (LCDFrameProcessor.FrameBase == 123456)
            {
                //Save Usersetting COM port and Back to home
                Thread.Sleep(1000);
                LCDFrameProcessor.startOperation(KeyboardtoFrameID("Escape"));

            }
            
        }



        public void CharProcessor(string str) {

            if (LCDFrameProcessor.ActiveFrameInfo.FrameBase == LCDFrameProcessor.FrameBase)
            {
                LCDFrameProcessor.RefreshFrame();
            }
            else {
                LCDFrameProcessor.startOperation(LCDFrameProcessor.FrameBase);
            }
            //textBox1.Text = LCDFrameProcessor.ActiveFrameInfo.FrameData_UserID.ToString();
        }


        //This function Executed Whenever Some Assinged Database work is Finished
        public void ProcessDataThreadCompleted(uint _id, string _name, string _phone, bool _isSucess, string _message)
        {
            LCDFrameProcessor.ActiveFrameInfo.FrameData_Name = _name; //Data[0].ToString();
            LCDFrameProcessor.ActiveFrameInfo.FrameData_Phone = _phone;//Data[1].ToString();
            LCDFrameProcessor.startOperation(2985);
        }
        


        private void Form1_Closing(object sender, CancelEventArgs e)
        {
            while (ProcessData.Semaphore) {
                Thread.Sleep(1000);
            }

            SerialP.DeInitialize();
            SetPinLevel(133, 0); //Backlight 
            LCDFunctions.clearLCD();
            LCDFunctions.CursorON_OFF(false);

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            uui(e.KeyData.ToString());
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            //textBox2.Text = e.KeyData.ToString();
            //uui(e.KeyData.ToString());
        }

        private void collection_AutoMode_Tick(object sender, EventArgs e)
        {
            CharProcessor("");
        }

}

    

    
    static class LCDFunctions
    {

        [DllImport("GPIOLib.dll", CharSet = CharSet.Auto)]
        public static extern bool SetPinAltFn(Int32 pinNum, Int32 altFn, bool dirOut);

        [DllImport("GPIOLib.dll", CharSet = CharSet.Auto)]
        public static extern void InitGPIOLib();

        [DllImport("GPIOLib.dll", CharSet = CharSet.Auto)]
        public static extern bool SetPinLevel(Int32 gpioNum, Int32 val);

        [DllImport("GPIOLib.dll", CharSet = CharSet.Auto)]
        public static extern void DeInitGPIOLib();

        //Better Use this Class in BackGround Thread;

        public static bool LCDLock = false;

        private static void PutaByte(int value)
        {
            int tempVal = value;
            //string CheckVal = "";

            //Setting Each Bits
            SetPinLevel(45, ((tempVal >> 3) & 0x01));
            SetPinLevel(79, ((tempVal >> 2) & 0x01));
            SetPinLevel(85, ((tempVal >> 1) & 0x01));
            SetPinLevel(97, ((tempVal >> 0) & 0x01));

            //SetPinLevel(101, ((tempVal >> 3) & 0x01));
            //SetPinLevel(103, ((tempVal >> 2) & 0x01));
            //SetPinLevel(133, ((tempVal >> 1) & 0x01));
            //SetPinLevel(98, ((tempVal) & 0x01));
            //MessageBox.Show(c.ToString());
        }

        public static void CursorON_OFF(bool _x) {
            if (_x)
            {
                //Display ON, CURSOR ON, BLINKING ON
                sendCommand(0x0f);
                
            }
            else { 
                //Display ON , CURSOR OFF, BLINKING OFF
                sendCommand(0x0C);
            }
        }

        public static void sendCommand(int value)
        {
            PutaByte(value>>4);
            SetPinLevel(101, 0); //Set RS to 0 for Command Mode
            SetPinLevel(103, 1); //Enable Pin Set
            Thread.Sleep(1);
            SetPinLevel(103, 0); //Enable Pin Reset
            Thread.Sleep(1);
            
            PutaByte((value&0x0f));
            SetPinLevel(101, 0); //Set RS to 0 for Command Mode
            SetPinLevel(103, 1); //Enable Pin Set
            Thread.Sleep(1);
            SetPinLevel(103, 0); //Enable Pin Reset
            Thread.Sleep(1);
            
        }
        public static void sendData(int value)
        {
            PutaByte(value>>4);
            SetPinLevel(101, 1); //Set RS to 1 for Data Mode
            SetPinLevel(103, 1); //Enable Pin Set
            Thread.Sleep(1);
            SetPinLevel(103, 0); //Enable Pin Reset
            
            
            PutaByte((value&0x0f));
            SetPinLevel(101, 1); //Set RS to 1 for Data Mode
            SetPinLevel(103, 1); //Enable Pin Set
            Thread.Sleep(1);
            SetPinLevel(103, 0); //Enable Pin Reset
            
           
        }

        public static void sendString(string s) {
            int i = 0;
            bool IsOnSecondLine = false;
            foreach (char _s in s) {
                i++;

                if (i > 20 && !IsOnSecondLine)
                {
                    IsOnSecondLine = true;
                    gotoLCD(0, 1);
                }

                sendData((int)_s);
            }
        }

        public static void InitLCD() {

            sendCommand(0x3);
            sendCommand(0x2);
            
            sendCommand(0x28);
              sendCommand(0x0C);
            //sendCommand(0x01);
            //sendCommand(0x02);
            sendCommand(0x06);
            //sendCommand(0x0F);
            sendCommand(0x01);
            
            //Here Left Writing Initilization if LCD
        }

        public static void clearLCD() {
            sendCommand(0x01);
        }
        public static void gotoLCD(int x, int y) {
            if(y==0)
                sendCommand(0x80 + x);
            else if(y==2)
                sendCommand(0x94 + x);
            else if(y==1)
                sendCommand(0xC0 + x);
            else if(y==3)
                sendCommand(0xd4 + x);
            //sendCommand((0x80 + (x + (0x40 * y))));           
        }

        public static void printStringWithCmd(string s, int x, int y) { 
            gotoLCD(x,y);
            sendString(s);
        }

    }

    

        
    
}
