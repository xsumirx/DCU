using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Utility;
using System.IO;
using System.IO.Ports;
using LCD16x2;

static class RecieverDeviceData { 

    //Device 0 Data Analyser
    public static double ANALYSER_FAT = 0;
    public static double ANALYSER_SNF = 0;
    public static double ANALYSER_WEIGHT = 0;
    public static double ANALYSER_PROTEIN = 0;
    public static double ANALYSER_LACTOSE = 0;
    public static double ANALYSER_SOLID = 0;
    public static double ANALYSER_ADDEDWATER = 0;
    public static double ANALYSER_DENSITY = 0;
    public static double ANALYSER_FRPOINT = 0;
    public static double ANALYSER_TEMP = 0;


    //Device 1 Data WeIgh Scale
    public static double WEIGH_WEIGHT = 0;

}

static class TransmitterDeviceData {

    

    //Device 1 Data Display Device
    public static string DISPLAY_STRING = "$S|0|0|0|0|0|&E\r\n";

}

public static class SerialP
    {
        private  static SerialPort comPort1, comPort2, comPort3;
        public static bool Semaphore_COM1 = false;
        public static bool Semaphore_COM2 = false;
        public static bool Semaphore_COM3 = false;

        public static string whichCOM_Setting = "COM1:";

        private static Queue<TransmitterTaskDetail> TransmitterTaskQueue = new Queue<TransmitterTaskDetail>();
        private static bool TransmitterTaskQueueSemaphore = false;

        private static int[] ReciverPhysicalMAP = new int[] {1,2,3 };
        private static bool[] ComReadFlag = new bool[] {false,false,false };

        private static int[] TransmitPhysicalMAP = new int[] {1,2,3 };

        private static Thread Thread_Transmitter;
        private static Thread Thread_ChangeSetting;
        
        /* To Start the Receiver, Things we need to Do !
         * 1.MAP the Devices
         * 2.Start the Thread
         */


       
        private static object[] extractAnalyser(string _message) {

            object[] _tempToReturn = null;

            float _tempF = 0;
            
            string weight = "0";
            string fat = "0";
            string snf = "0";
            int index = 0;
            try
            {
                if (_message.StartsWith("$|"))
                {
                    string s = _message.Remove(0, 2);
                    index = s.IndexOf("|", 0);
                    if (!(index <= 0) && index != null)
                    {
                        weight = s.Substring(0, index);
                        string s1 = s.Remove(0, (index + 1));

                        _tempF = float.Parse(weight);
                        if (true)
                        {
                            index = 0;
                            index = s1.IndexOf("|", 0);
                            if (!(index <= 0) && index != null)
                            {
                                fat = s1.Substring(0, index);
                                string s2 = s1.Remove(0, (index + 1));
                                _tempF = float.Parse(fat);
                                if (true)
                                {
                                    index = 0;
                                    index = s2.IndexOf("|", 0);
                                    if (!(index <= 0) && index != null)
                                    {
                                        snf = s2.Substring(0, index);
                                        string s3 = s2.Remove(0, (index + 1));
                                        _tempF = float.Parse(snf);
                                        if (true)
                                        {
                                            if (s3.StartsWith("$"))
                                            {
                                                _tempToReturn = new object[] {weight,fat,snf };
                                               
                                                //MessageBox.Show("FAT : " + fat + "\nSNF : " + snf + "\nWEIGHT : " + weight);

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }catch(Exception ex){
                //Do Not Respond
            }

            return _tempToReturn;
        }

        private static object[] manupulateAnalyserString(string _data)
        {
            //\n\rMILK SCANNER\n\rSN: F162   Mode:   1\n\rTemp. 29.5 C\n\rFat............13.6%\n\rSNF............ 6.3%\n\rDensity........ 0.0\n\rProtein........ 0.0%\n\rLactose........ 0.0%\n\rSolids......... 0.0%\n\rAdded water....99.9%\n\rFr. point.. -0.000 C\n\r\n\r\n\r\n\r\n\r

            /*
             * 1.FAT
             * 2.SNF
             * 3.PROTEIN
             * 4.LACTOSE
             * 5.SOLID
             * 6.ADDED WATER
             * 7.DENSITY
             * 8.FR.POINT
             * 9.TEMPERATURE
             * 
             */


            object[] retVal = new object[] { "0.0", "0.0", "0.0", "0.0", "0.0", "0.0", "0.0", "0.0", "0.0" };

            string fat_String = "0.0";
            string snf_String = "0.0";
            string protein_String = "0.0";
            string lactose_String = "0.0";
            string solid_String = "0.0";
            string addedwater_String = "0.0";

            string density_String = "0.0";
            string frPoint_String = "0.0";
            string temp_String = "0.0";

            try
            {
                if (_data.StartsWith("\n\r"))
                {
                    //49 Temprator

                    int index_of_fat = _data.IndexOf("Fat............");
                    int index_of_snf = _data.IndexOf("SNF............");
                    int index_of_protein = _data.IndexOf("Protein........");
                    int index_of_lactose = _data.IndexOf("Lactose........");
                    int index_of_solids = _data.IndexOf("Solids.........");
                    int index_of_addedwater = _data.IndexOf("Added water....");


                    int index_of_density = _data.IndexOf("Density........");
                    int index_of_frPoint = _data.IndexOf("Fr. point..");
                    int index_of_temp = _data.IndexOf("Temp.");

                    //Find FAT
                    if (index_of_fat >= 0)
                    {
                        fat_String = _data.Substring(index_of_fat + 15, 8);
                        int fat_index = fat_String.IndexOf("%");
                        if (fat_index >= 0)
                        {
                            fat_String = fat_String.Remove(fat_index, fat_String.Substring(fat_index).Length);
                        }
                        else
                            fat_String = "0.0";
                    }

                    //Find SNF
                    if (index_of_snf >= 0)
                    {
                        snf_String = _data.Substring(index_of_snf + 15, 8);
                        int snf_index = snf_String.IndexOf("%");
                        if (snf_index >= 0)
                            snf_String = snf_String.Remove(snf_index, snf_String.Substring(snf_index).Length);
                        else
                            snf_String = "0.0";
                    }


                    //Find Protein
                    if (index_of_protein >= 0)
                    {
                        protein_String = _data.Substring(index_of_protein + 15, 8);
                        int protein_index = protein_String.IndexOf("%");
                        if (protein_index >= 0)
                            protein_String = protein_String.Remove(protein_index, protein_String.Substring(protein_index).Length);
                        else
                            protein_String = "0.0";
                    }

                    //Find Lactose
                    if (index_of_lactose >= 0)
                    {
                        lactose_String = _data.Substring(index_of_lactose + 15, 8);
                        int lactose_index = lactose_String.IndexOf("%");
                        if (lactose_index >= 0)
                            lactose_String = lactose_String.Remove(lactose_index, lactose_String.Substring(lactose_index).Length);
                        else
                            lactose_String = "0.0";
                    }

                    //Find Solid
                    if (index_of_solids >= 0)
                    {
                        solid_String = _data.Substring(index_of_solids + 15, 8);
                        int solid_index = solid_String.IndexOf("%");
                        if (solid_index >= 0)
                            solid_String = solid_String.Remove(solid_index, solid_String.Substring(solid_index).Length);
                        else
                            solid_String = "0.0";
                    }

                    //Find Added Water
                    if (index_of_addedwater >= 0)
                    {
                        addedwater_String = _data.Substring(index_of_addedwater + 15, 8);
                        int addedwater_index = addedwater_String.IndexOf("%");
                        if (addedwater_index >= 0)
                            addedwater_String = addedwater_String.Remove(addedwater_index, addedwater_String.Substring(addedwater_index).Length);
                        else
                            addedwater_String = "0.0";
                    }

                    //Find Density
                    if (index_of_density >= 0)
                    {
                        density_String = _data.Substring(index_of_density + 15, 8);
                        int density_index = density_String.IndexOf("\n");
                        if (density_index >= 0)
                            density_String = density_String.Remove(density_index, density_String.Substring(density_index).Length);
                        else
                            density_String = "0.0";
                    }

                    //Find Fr.Point
                    if (index_of_frPoint >= 0)
                    {
                        frPoint_String = _data.Substring(index_of_frPoint + 11, 15);
                        int frPoint_Index = frPoint_String.IndexOf("C\n");
                        if (frPoint_Index >= 0)
                        {
                            frPoint_String = frPoint_String.Remove(frPoint_Index, frPoint_String.Substring(frPoint_Index).Length);
                        }
                        else
                            frPoint_String = "0.0";
                    }

                    //Find Temp
                    if (index_of_temp >= 0)
                    {
                        temp_String = _data.Substring(index_of_temp + 5, 8);
                        int temp_index = temp_String.IndexOf("C\n");
                        if (temp_index >= 0)
                            temp_String = temp_String.Remove(temp_index, temp_String.Substring(temp_index).Length);
                        else
                            temp_String = "0.0";
                    }


                    /*
                 * 1.FAT
                 * 2.SNF
                 * 3.PROTEIN
                 * 4.LACTOSE
                 * 5.SOLID
                 * 6.ADDED WATER
                 * 7.DENSITY
                 * 8.FR.POINT
                 * 9.TEMPERATURE
                 * 
                 */




                    //tb_Output.Text = "FAT : " + fat_String + Environment.NewLine + "SNF : " + snf_String + Environment.NewLine + "Protein : " + protein_String + Environment.NewLine + "Lactose : " + lactose_String + Environment.NewLine + "Solid : " + solid_String + Environment.NewLine + "Added Water : " + addedwater_String + Environment.NewLine + "Density : " + density_String + Environment.NewLine + " Fr. Point : " + frPoint_String + Environment.NewLine + "Temprature : " + temp_String; 
                }
            }
            catch (Exception ex) { }

            retVal[0] = fat_String;
            retVal[1] = snf_String;
            retVal[2] = protein_String;
            retVal[3] = lactose_String;
            retVal[4] = solid_String;
            retVal[5] = addedwater_String;
            retVal[6] = density_String;
            retVal[7] = frPoint_String;
            retVal[8] = temp_String;

            return retVal;
        }

        private static double manupulateWeighScale(string _data)
        {
            double retVal = 0;
            try
            {
                string _temp_s = "000.000";
                int index_N = _data.IndexOf('N');
                if (index_N != -1)
                {
                    _temp_s = _data.Substring(index_N + 1, 7);
                }
                System.Text.RegularExpressions.Match mat = System.Text.RegularExpressions.Regex.Match(_temp_s, "[0-9]*\\.*[0-9]*");
                if (mat.Success)
                {
                    retVal = double.Parse(mat.Value);
                }


            }
            catch (Exception ex) { }

            return retVal;
        }

        public static void sendDirect(int _DeviceID,string message)
        {
          
                int _COM_ID = 54;

                for (int i = 0; i < 3; i++)
                {
                    if (TransmitPhysicalMAP[i] == _DeviceID)
                    {
                        _COM_ID = i;
                        break;
                    }
                }
            

            try
            {
                if (_COM_ID == 0)
                {
                    if (comPort1.IsOpen)
                    {
                        comPort1.DiscardOutBuffer();
                        comPort1.WriteLine(message);
                    }
                }
                else if (_COM_ID == 1)
                {
                    if (comPort2.IsOpen)
                    {
                        comPort2.DiscardOutBuffer();
                        comPort2.WriteLine(message);
                    }
                }
                else if (_COM_ID == 2)
                {
                    if (comPort3.IsOpen)
                    {
                        comPort3.DiscardOutBuffer();
                        comPort3.WriteLine(message);
                    }
                }
            }
            catch (Exception ex) { 
            
            }
        }

        private static void Thread_Transmitter_Work() 
        {
            while (true) {
                Thread.Sleep(1000);

                if (TransmitterTaskQueue.Count > 0) {
                    TransmitterTaskDetail _tmpTD = TransmitterTaskQueue.Dequeue();
                    switch (_tmpTD.COM_ID) {
                        case 0: {
                            try {
                                if (comPort1.IsOpen) {
                                    if (Semaphore_COM1)
                                    {
                                        int _tempCount = 0;
                                        while (TransmitterTaskQueueSemaphore)
                                        {
                                            _tempCount++;
                                            Thread.Sleep(10);
                                            if (_tempCount >= 4)
                                            {
                                                TransmitterTaskQueue.Enqueue(_tmpTD);
                                                break;
                                            }
                                        }
                                    }
                                    else { 
                                        //No One is using COM 1
                                        Semaphore_COM1 = true;
                                        comPort1.WriteLine(_tmpTD.message);
                                        Semaphore_COM1 = false;
                                    }
                                }
                            }
                            catch (Exception ex) { 
                                //Dont Respond
                            }

                            break;
                        }
                        case 1: {
                            try
                            {
                                if (comPort2.IsOpen)
                                {
                                    if (Semaphore_COM2)
                                    {
                                        int _tempCount = 0;
                                        while (TransmitterTaskQueueSemaphore)
                                        {
                                            _tempCount++;
                                            Thread.Sleep(10);
                                            if (_tempCount >= 4)
                                            {
                                                TransmitterTaskQueue.Enqueue(_tmpTD);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //No One is using COM 2
                                        Semaphore_COM2 = true;
                                        comPort2.WriteLine(_tmpTD.message);
                                        Semaphore_COM2 = false;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                //Dont Respond
                            }

                            break;
                        }
                        case 2:
                            {
                                try
                                {
                                    if (comPort3.IsOpen)
                                    {
                                        if (Semaphore_COM3)
                                        {
                                            int _tempCount = 0;
                                            while (TransmitterTaskQueueSemaphore)
                                            {
                                                _tempCount++;
                                                Thread.Sleep(10);
                                                if (_tempCount >= 4)
                                                {
                                                    TransmitterTaskQueue.Enqueue(_tmpTD);
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //No One is using COM 3
                                            Semaphore_COM3 = true;
                                            comPort3.WriteLine(_tmpTD.message);
                                            Semaphore_COM3 = false;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //Dont Respond
                                }
                                break;
                            }
                    }
                
                }

            }
        }

        public static void sendData(int _DeviceID, string _message) { 
            //Find COM_ID from DEVICE_ID
            int _COM_ID = 54;
            try
            {
                for (int i = 0; i < 3; i++)
                {
                    if (TransmitPhysicalMAP[i] == _DeviceID)
                    {
                        _COM_ID = i;
                        break;
                    }
                }

                if (_COM_ID >= 0 && _COM_ID <= 2)
                {
                    while (TransmitterTaskQueueSemaphore)
                        Thread.Sleep(10);
                    TransmitterTaskQueueSemaphore = true;
                    TransmitterTaskQueue.Enqueue(new TransmitterTaskDetail(_COM_ID, _message));
                    TransmitterTaskQueueSemaphore = false;
                }
            }
            catch (Exception ex) { }
            //if(TransmitterDeviceData.TransmitterSemaphore)
        }

        public static void StartReceiving(int[] _DeviceID) {
            foreach (int _di in _DeviceID) {
                for (int i = 0; i <= 2; i++) {
                    if (ReciverPhysicalMAP[i] == _di) {
                        ComReadFlag[i] = true;
                        break;
                    }
                }
            }

            //Start Thread Here !
        }


        public static void StopReceiving(int[] _DeviceID)
        {
            foreach (int _di in _DeviceID)
            {
                for (int i = 0; i <= 2; i++)
                {
                    if (ReciverPhysicalMAP[i] == _di)
                    {
                        ComReadFlag[i] = false;
                        break;
                    }
                }
            }

            //Start Thread Here !
        }


        public static void Initialize() {
            comPort1 = new SerialPort();
            comPort2 = new SerialPort();
            comPort3 = new SerialPort();

            comPort1.PortName = "COM1:";
            
            if (comPort1.IsOpen) {
                comPort1.Close();
            }


            comPort1.BaudRate = 9600;
            comPort1.Parity = Parity.None;
            comPort1.DataBits = 8;
            comPort1.StopBits = StopBits.One;
            comPort1.Handshake = Handshake.None;
            comPort1.ReadTimeout = 500;
            comPort1.WriteTimeout = 500;

            comPort2.PortName = "COM2:";
            if (comPort2.IsOpen)
            {
                comPort2.Close();
            }
            comPort2.BaudRate = 9600;
            comPort2.Parity = Parity.None;
            comPort2.DataBits = 8;
            comPort2.StopBits = StopBits.One;
            comPort2.Handshake = Handshake.None;
            comPort2.ReadTimeout = 500;
            comPort2.WriteTimeout = 500;


            comPort3.PortName = "COM3:";
            if (comPort3.IsOpen)
            {
                comPort3.Close();
            }
            comPort3.BaudRate = 9600;
            comPort3.Parity = Parity.None;
            comPort3.DataBits = 8;
            comPort3.StopBits = StopBits.One;
            comPort3.Handshake = Handshake.None;
            comPort3.ReadTimeout = 500;
            comPort3.WriteTimeout = 500;

           
            try
            {
                comPort1.Open();
                comPort2.Open();
                comPort3.Open();
            }
            catch(IOException ex)
            {
                //ErrorMessageEvent(ex.Message);
            }

            try
            {
                comPort1.DiscardInBuffer();
                comPort2.DiscardInBuffer();
                comPort3.DiscardInBuffer();
            }
            catch (Exception ex) { }

            try
            {
                comPort1.DataReceived += new SerialDataReceivedEventHandler(comPort1_DataReceived);
                comPort2.DataReceived += new SerialDataReceivedEventHandler(comPort2_DataReceived);
                comPort3.DataReceived += new SerialDataReceivedEventHandler(comPort3_DataReceived);

            }
            catch (Exception ex) { }

            try
            {
                comPort1.ErrorReceived += new SerialErrorReceivedEventHandler(comPort1_ErrorReceived);
                comPort2.ErrorReceived += new SerialErrorReceivedEventHandler(comPort2_ErrorReceived);
                comPort3.ErrorReceived += new SerialErrorReceivedEventHandler(comPort3_ErrorReceived);
            }
            catch (Exception ex) { }

        }

        static void comPort3_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
               //throw new NotImplementedException();
            try
            {
                comPort3.DiscardInBuffer();
            }
            catch (Exception ex) { }
        }

        static void comPort2_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            //throw new NotImplementedException();
            try
            {
                comPort2.DiscardInBuffer();
            }
            catch (Exception ex) { }
        }

        static void comPort1_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            //throw new NotImplementedException();
            try
            {
                comPort1.DiscardInBuffer();
            }
            catch (Exception ex) { }
        }

        static void comPort3_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //throw new NotImplementedException();
            if (ComReadFlag[2])
            {
                if (ReciverPhysicalMAP[2] == 0)
                {
                    //Reciver Device 0 is at COM Port 3
                    //Device 0 Means Analyser
                    try
                    {
                        Thread.Sleep(1000);
                        string _tempRecieved = comPort3.ReadExisting();

                        if (_tempRecieved != "" && _tempRecieved != null)
                        {

                            /*
                             * 1.FAT
                             * 2.SNF
                             * 3.PROTEIN
                             * 4.LACTOSE
                             * 5.SOLID
                             * 6.ADDED WATER
                             * 7.DENSITY
                             * 8.FR.POINT
                             * 9.TEMPERATURE
                             * 
                             */

                            object[] _tmpObj = manupulateAnalyserString(_tempRecieved);
                            //RecieverDeviceData.ANALYSER_WEIGHT = double.Parse((string)_tmpObj[0]);
                            RecieverDeviceData.ANALYSER_FAT = double.Parse((string)_tmpObj[0]);
                            RecieverDeviceData.ANALYSER_SNF = double.Parse((string)_tmpObj[1]);
                            RecieverDeviceData.ANALYSER_PROTEIN = double.Parse((string)_tmpObj[2]);
                            RecieverDeviceData.ANALYSER_LACTOSE = double.Parse((string)_tmpObj[3]);
                            RecieverDeviceData.ANALYSER_SOLID = double.Parse((string)_tmpObj[4]);
                            RecieverDeviceData.ANALYSER_ADDEDWATER = double.Parse((string)_tmpObj[5]);
                            RecieverDeviceData.ANALYSER_DENSITY = double.Parse((string)_tmpObj[6]);
                            RecieverDeviceData.ANALYSER_FRPOINT = double.Parse((string)_tmpObj[7]);
                            RecieverDeviceData.ANALYSER_TEMP = double.Parse((string)_tmpObj[8]);

                            //LCDFrameProcessor.ActiveFrameInfo.float6_0data = (string)_tmpObj[0];
                            LCDFrameProcessor.ActiveFrameInfo.float4_0data = (string)_tmpObj[0];
                            LCDFrameProcessor.ActiveFrameInfo.float4_1data = (string)_tmpObj[1];
                            //RecieverDeviceData.ANALYSER_WEIGHT = _tempRecieved;
                            HookKeys.OnHookEvent(new HookEventArgs(), new KeyBoardInfo(49));
                        }
                    }
                    catch (Exception ex)
                    {
                        //Do Nothing
                    }

                }
                else if (ReciverPhysicalMAP[2] == 1)
                {
                    //Reciver Device 1 is at COM Port 3
                    //Weighing Scale = Device 1
                    try
                    {
                        Thread.Sleep(1000);
                        string _tempRecieved = comPort3.ReadExisting();
                        if (_tempRecieved != "" && _tempRecieved != null)
                        {
                            RecieverDeviceData.WEIGH_WEIGHT = manupulateWeighScale(_tempRecieved);
                            LCDFrameProcessor.ActiveFrameInfo.float6_0data = Math.Round(RecieverDeviceData.WEIGH_WEIGHT,2).ToString();
                            HookKeys.OnHookEvent(new HookEventArgs(), new KeyBoardInfo(49));

                        }
                    }
                    catch (Exception ex) { }
                }
                else if (ReciverPhysicalMAP[2] == 2)
                {
                    //Reciver Device 2 is at COM Port 3
                    try
                    {
                        Thread.Sleep(100);
                        string crap = comPort3.ReadExisting();
                    }
                    catch (Exception ex) { }
                }
            }
            else {
                try
                {
                    Thread.Sleep(100);
                    string crap = comPort3.ReadExisting();
                }
                catch (Exception ex) { }
            }
        }

        static void comPort2_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //throw new NotImplementedException();
            if (ComReadFlag[1])
            {
                if (ReciverPhysicalMAP[1] == 0)
                {
                    //Reciver Device 0 is at COM Port 2
                    //Device 0 Means Analyser
                    try
                    {
                        Thread.Sleep(1000);
                        string _tempRecieved = comPort2.ReadExisting();

                        if (_tempRecieved != "" && _tempRecieved != null)
                        {
                            /*
                             * 1.FAT
                             * 2.SNF
                             * 3.PROTEIN
                             * 4.LACTOSE
                             * 5.SOLID
                             * 6.ADDED WATER
                             * 7.DENSITY
                             * 8.FR.POINT
                             * 9.TEMPERATURE
                             * 
                             */

                            object[] _tmpObj = manupulateAnalyserString(_tempRecieved);
                            //RecieverDeviceData.ANALYSER_WEIGHT = double.Parse((string)_tmpObj[0]);
                            RecieverDeviceData.ANALYSER_FAT = double.Parse((string)_tmpObj[0]);
                            RecieverDeviceData.ANALYSER_SNF = double.Parse((string)_tmpObj[1]);
                            RecieverDeviceData.ANALYSER_PROTEIN = double.Parse((string)_tmpObj[2]);
                            RecieverDeviceData.ANALYSER_LACTOSE = double.Parse((string)_tmpObj[3]);
                            RecieverDeviceData.ANALYSER_SOLID = double.Parse((string)_tmpObj[4]);
                            RecieverDeviceData.ANALYSER_ADDEDWATER = double.Parse((string)_tmpObj[5]);
                            RecieverDeviceData.ANALYSER_DENSITY = double.Parse((string)_tmpObj[6]);
                            RecieverDeviceData.ANALYSER_FRPOINT = double.Parse((string)_tmpObj[7]);
                            RecieverDeviceData.ANALYSER_TEMP = double.Parse((string)_tmpObj[8]);

                            //LCDFrameProcessor.ActiveFrameInfo.float6_0data = (string)_tmpObj[0];
                            LCDFrameProcessor.ActiveFrameInfo.float4_0data = (string)_tmpObj[0];
                            LCDFrameProcessor.ActiveFrameInfo.float4_1data = (string)_tmpObj[1];
                            //RecieverDeviceData.ANALYSER_WEIGHT = _tempRecieved;
                            HookKeys.OnHookEvent(new HookEventArgs(), new KeyBoardInfo(49));

                        }
                    }
                    catch (Exception ex)
                    {
                        //Do Nothing
                    }

                }
                else if (ReciverPhysicalMAP[1] == 1)
                {
                    //Reciver Device 1 is at COM Port 2
                    //Weighing Scale = Device 1
                    try
                    {
                        Thread.Sleep(1000);
                        string _tempRecieved = comPort2.ReadExisting();
                        if (_tempRecieved != "" && _tempRecieved != null)
                        {
                            RecieverDeviceData.WEIGH_WEIGHT = manupulateWeighScale(_tempRecieved);
                            LCDFrameProcessor.ActiveFrameInfo.float6_0data = Math.Round(RecieverDeviceData.WEIGH_WEIGHT, 2).ToString();
                            HookKeys.OnHookEvent(new HookEventArgs(), new KeyBoardInfo(49));

                        }
                    }
                    catch (Exception ex) { }
                }
                else if (ReciverPhysicalMAP[1] == 2)
                {
                    //Reciver Device 3 is at COM Port 2
                    try
                    {
                        Thread.Sleep(100);
                        string crap = comPort2.ReadExisting();
                    }
                    catch (Exception ex) { }
                }
            }
            else {
                try
                {
                    Thread.Sleep(100);
                    string crap = comPort2.ReadExisting();
                }
                catch (Exception ex) { }
            }
        }

        static void comPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //throw new NotImplementedException();
            if (ComReadFlag[0])
            {

                if (ReciverPhysicalMAP[0] == 0)
                {
                    //Reciver Device 0 is at COM Port 1
                    //Device 0 Means Analyser
                    try
                    {
                        Thread.Sleep(1000);
                        string _tempRecieved = comPort1.ReadExisting();

                        if (_tempRecieved != "" && _tempRecieved != null)
                        {
                            object[] _tmpObj = manupulateAnalyserString(_tempRecieved);
                            if (_tmpObj != null)
                            {
                                /*
                             * 1.FAT
                             * 2.SNF
                             * 3.PROTEIN
                             * 4.LACTOSE
                             * 5.SOLID
                             * 6.ADDED WATER
                             * 7.DENSITY
                             * 8.FR.POINT
                             * 9.TEMPERATURE
                             * 
                             */


                                //RecieverDeviceData.ANALYSER_WEIGHT = double.Parse((string)_tmpObj[0]);
                                RecieverDeviceData.ANALYSER_FAT = double.Parse((string)_tmpObj[0]);
                                RecieverDeviceData.ANALYSER_SNF = double.Parse((string)_tmpObj[1]);
                                RecieverDeviceData.ANALYSER_PROTEIN = double.Parse((string)_tmpObj[2]);
                                RecieverDeviceData.ANALYSER_LACTOSE = double.Parse((string)_tmpObj[3]);
                                RecieverDeviceData.ANALYSER_SOLID = double.Parse((string)_tmpObj[4]);
                                RecieverDeviceData.ANALYSER_ADDEDWATER = double.Parse((string)_tmpObj[5]);
                                RecieverDeviceData.ANALYSER_DENSITY = double.Parse((string)_tmpObj[6]);
                                RecieverDeviceData.ANALYSER_FRPOINT = double.Parse((string)_tmpObj[7]);
                                RecieverDeviceData.ANALYSER_TEMP = double.Parse((string)_tmpObj[8]);

                                //LCDFrameProcessor.ActiveFrameInfo.float6_0data = (string)_tmpObj[0];
                                LCDFrameProcessor.ActiveFrameInfo.float4_0data = (string)_tmpObj[0];
                                LCDFrameProcessor.ActiveFrameInfo.float4_1data = (string)_tmpObj[1];
                                //RecieverDeviceData.ANALYSER_WEIGHT = _tempRecieved;
                                HookKeys.OnHookEvent(new HookEventArgs(), new KeyBoardInfo(49));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //Do Nothing
                    }

                }
                else if (ReciverPhysicalMAP[0] == 1)
                {
                    //Reciver Device 1 is at COM Port 1
                    //Weighing Scale = Device 1
                    try
                    {
                        Thread.Sleep(1000);
                        string _tempRecieved = comPort1.ReadExisting();
                        if (_tempRecieved != "" && _tempRecieved != null)
                        {
                            RecieverDeviceData.WEIGH_WEIGHT = manupulateWeighScale(_tempRecieved);
                            LCDFrameProcessor.ActiveFrameInfo.float6_0data = Math.Round(RecieverDeviceData.WEIGH_WEIGHT,2).ToString();
                            HookKeys.OnHookEvent(new HookEventArgs(), new KeyBoardInfo(49));

                        }
                    }
                    catch (Exception ex) { }
                }
                else if (ReciverPhysicalMAP[0] == 2)
                {
                    //Reciver Device 2 is at COM Port 1
                    Thread.Sleep(100);
                    try
                    {
                        string crap = comPort1.ReadExisting();
                    }
                    catch (Exception ex) { }
                }
            }
            else {
                Thread.Sleep(100);
                try
                {
                    string crap = comPort1.ReadExisting();
                }
                catch (Exception ex) { }
            }
            
        }

        public static void DeInitialize() {
            try
            {
                if (comPort1.IsOpen)
                {
                    comPort1.Close();
                }

                if (comPort2.IsOpen)
                {
                    comPort2.Close();
                }
                if (comPort3.IsOpen)
                {
                    comPort3.Close();
                }
            }
            catch (Exception ex) { }
        }

        public static void ChangeReceiverPhysicalMAP()
        {
            try
            {
                ReciverPhysicalMAP[0] = int.Parse(DataKeeper.GetValue("DEVICE_MAN_COM1_RX_DEVICE"));
                ReciverPhysicalMAP[1] = int.Parse(DataKeeper.GetValue("DEVICE_MAN_COM2_RX_DEVICE"));
                ReciverPhysicalMAP[2] = int.Parse(DataKeeper.GetValue("DEVICE_MAN_COM3_RX_DEVICE"));
            }
            catch (Exception ex) { }
        }

        public static void ChangeTransmitterPhysicalMAP()
        {
            try
            {
                TransmitPhysicalMAP[0] = int.Parse(DataKeeper.GetValue("DEVICE_MAN_COM1_TX_DEVICE"));
                TransmitPhysicalMAP[1] = int.Parse(DataKeeper.GetValue("DEVICE_MAN_COM2_TX_DEVICE"));
                TransmitPhysicalMAP[2] = int.Parse(DataKeeper.GetValue("DEVICE_MAN_COM3_TX_DEVICE"));
            }
            catch (Exception ex) { }
        }

        public static void Thread_ChangeCOMSettings_Work()
        {
            string _ComName = whichCOM_Setting;
            bool wasCOM1Open = false;
            bool wasCOM2Open = false;
            bool wasCOM3Open = false;

            try
            {
                if (_ComName == "COM1:")
                {
                    if (comPort1.IsOpen)
                    {
                        comPort1.Close();
                        wasCOM1Open = true;
                    }

                    while (Semaphore_COM1)
                        Thread.Sleep(100);

                    Semaphore_COM1 = true;
                    string br = DataKeeper.GetValue("COM1_BAUD_RATE");
                    string db = DataKeeper.GetValue("COM1_DATA_BITS");
                    string sb = DataKeeper.GetValue("COM1_STOP_BITS");
                    string hs = DataKeeper.GetValue("COM1_HANDSHAKE");
                    string parity = DataKeeper.GetValue("COM1_PARITY");

                    #region ChangeCOM1
                    //Set BaudRate
                    switch (int.Parse(br)) {

                        case 0: {
                            comPort1.BaudRate = 1200;
                            break;
                        }
                        case 1: {
                            comPort1.BaudRate = 2400;
                            break;
                        }
                        case 2:
                            {
                                comPort1.BaudRate = 4800;
                                break;
                            }
                        case 3:
                            {
                                comPort1.BaudRate = 9600;
                                break;
                            }
                        case 4:
                            {
                                comPort1.BaudRate = 19200;
                                break;
                            }
                        case 5:
                            {
                                comPort1.BaudRate = 38400;
                                break;
                            }
                        case 6:
                            {
                                comPort1.BaudRate = 57600;
                                break;
                            }
                        case 7:
                            {
                                comPort1.BaudRate = 115200;
                                break;
                            }
                        default:
                            break;
                    }

                        //stopbit
                    switch (int.Parse(sb)) {
                        case 0:
                            {
                                comPort1.StopBits = StopBits.None;
                                break;
                            }
                        case 1:
                            {
                                comPort1.StopBits = StopBits.One;
                                break;
                            }
                        case 2:
                            {
                                comPort1.StopBits = StopBits.OnePointFive;
                                break;
                            }
                        case 3:
                            {
                                comPort1.StopBits = StopBits.Two;
                                break;
                            }
                        default:
                            break;
                        
                    }

                    //DATA BIT
                    switch (int.Parse(db))
                    {
                        case 0:
                            {
                                comPort1.DataBits = 7;
                                break;
                            }
                        case 1:
                            {
                                comPort1.DataBits = 8;
                                break;
                            }
                        default:
                            break;
                    }

                    //HANDSHAKE
                    switch (int.Parse(hs))
                    {
                        case 0:
                            {
                                comPort1.Handshake = Handshake.None;
                                break;
                            }
                        case 1:
                            {
                                comPort1.Handshake = Handshake.RequestToSend;
                                break;
                            }
                        default:
                            break;
                    }


                    //PARITY
                    switch (int.Parse(parity))
                    {

                        case 0:
                            {
                                comPort1.Parity = Parity.None;
                                break;
                            }
                        case 1:
                            {
                                comPort1.Parity = Parity.Even;
                                break;
                            }
                        case 2:
                            {
                                comPort1.Parity = Parity.Odd;
                                break;
                            }
                        case 3:
                            {
                                comPort1.Parity = Parity.Space;
                                break;
                            }
                        case 4:
                            {
                                comPort1.Parity = Parity.Mark;
                                break;
                            }
                        
                        default:
                            break;
                    }

                    #endregion
                    

                    if(wasCOM1Open)
                        comPort1.Open();

                    Semaphore_COM1 = false;
                }

                else if (_ComName == "COM2:")
                {
                    if (comPort2.IsOpen)
                    {
                        comPort2.Close();
                        wasCOM2Open = true;
                    }

                    while (Semaphore_COM2)
                        Thread.Sleep(100);

                    Semaphore_COM2 = true;
                    string br = DataKeeper.GetValue("COM2_BAUD_RATE");
                    string db = DataKeeper.GetValue("COM2_DATA_BITS");
                    string sb = DataKeeper.GetValue("COM2_STOP_BITS");
                    string hs = DataKeeper.GetValue("COM2_HANDSHAKE");
                    string parity = DataKeeper.GetValue("COM2_PARITY");

                    #region ChangeCOM2
                    //Set BaudRate
                    switch (int.Parse(br))
                    {

                        case 0:
                            {
                                comPort2.BaudRate = 1200;
                                break;
                            }
                        case 1:
                            {
                                comPort2.BaudRate = 2400;
                                break;
                            }
                        case 2:
                            {
                                comPort2.BaudRate = 4800;
                                break;
                            }
                        case 3:
                            {
                                comPort2.BaudRate = 9600;
                                break;
                            }
                        case 4:
                            {
                                comPort2.BaudRate = 19200;
                                break;
                            }
                        case 5:
                            {
                                comPort2.BaudRate = 38400;
                                break;
                            }
                        case 6:
                            {
                                comPort2.BaudRate = 57600;
                                break;
                            }
                        case 7:
                            {
                                comPort2.BaudRate = 115200;
                                break;
                            }
                        default:
                            break;
                    }

                    //stopbit
                    switch (int.Parse(sb))
                    {
                        case 0:
                            {
                                comPort2.StopBits = StopBits.None;
                                break;
                            }
                        case 1:
                            {
                                comPort2.StopBits = StopBits.One;
                                break;
                            }
                        case 2:
                            {
                                comPort2.StopBits = StopBits.OnePointFive;
                                break;
                            }
                        case 3:
                            {
                                comPort2.StopBits = StopBits.Two;
                                break;
                            }
                        default:
                            break;

                    }

                    //DATA BIT
                    switch (int.Parse(db))
                    {
                        case 0:
                            {
                                comPort2.DataBits = 7;
                                break;
                            }
                        case 1:
                            {
                                comPort2.DataBits = 8;
                                break;
                            }
                        default:
                            break;
                    }

                    //HANDSHAKE
                    switch (int.Parse(hs))
                    {
                        case 0:
                            {
                                comPort2.Handshake = Handshake.None;
                                break;
                            }
                        case 1:
                            {
                                comPort2.Handshake = Handshake.RequestToSend;
                                break;
                            }
                        default:
                            break;
                    }


                    //PARITY
                    switch (int.Parse(parity))
                    {

                        case 0:
                            {
                                comPort2.Parity = Parity.None;
                                break;
                            }
                        case 1:
                            {
                                comPort2.Parity = Parity.Even;
                                break;
                            }
                        case 2:
                            {
                                comPort2.Parity = Parity.Odd;
                                break;
                            }
                        case 3:
                            {
                                comPort2.Parity = Parity.Space;
                                break;
                            }
                        case 4:
                            {
                                comPort2.Parity = Parity.Mark;
                                break;
                            }

                        default:
                            break;
                    }

                    #endregion


                    if (wasCOM2Open)
                        comPort2.Open();

                    Semaphore_COM2 = false;
                }
                else if (_ComName == "COM3:")
                {
                    if (comPort3.IsOpen)
                    {
                        comPort3.Close();
                        wasCOM3Open = true;
                    }

                    while (Semaphore_COM3)
                        Thread.Sleep(100);

                    Semaphore_COM3 = true;
                    string br = DataKeeper.GetValue("COM3_BAUD_RATE");
                    string db = DataKeeper.GetValue("COM3_DATA_BITS");
                    string sb = DataKeeper.GetValue("COM3_STOP_BITS");
                    string hs = DataKeeper.GetValue("COM3_HANDSHAKE");
                    string parity = DataKeeper.GetValue("COM3_PARITY");

                    #region ChangeCOM3
                    //Set BaudRate
                    switch (int.Parse(br))
                    {

                        case 0:
                            {
                                comPort3.BaudRate = 1200;
                                break;
                            }
                        case 1:
                            {
                                comPort3.BaudRate = 2400;
                                break;
                            }
                        case 2:
                            {
                                comPort3.BaudRate = 4800;
                                break;
                            }
                        case 3:
                            {
                                comPort3.BaudRate = 9600;
                                break;
                            }
                        case 4:
                            {
                                comPort3.BaudRate = 19200;
                                break;
                            }
                        case 5:
                            {
                                comPort3.BaudRate = 38400;
                                break;
                            }
                        case 6:
                            {
                                comPort3.BaudRate = 57600;
                                break;
                            }
                        case 7:
                            {
                                comPort3.BaudRate = 115200;
                                break;
                            }
                        default:
                            break;
                    }

                    //stopbit
                    switch (int.Parse(sb))
                    {
                        case 0:
                            {
                                comPort3.StopBits = StopBits.None;
                                break;
                            }
                        case 1:
                            {
                                comPort3.StopBits = StopBits.One;
                                break;
                            }
                        case 2:
                            {
                                comPort3.StopBits = StopBits.OnePointFive;
                                break;
                            }
                        case 3:
                            {
                                comPort3.StopBits = StopBits.Two;
                                break;
                            }
                        default:
                            break;

                    }

                    //DATA BIT
                    switch (int.Parse(db))
                    {
                        case 0:
                            {
                                comPort3.DataBits = 7;
                                break;
                            }
                        case 1:
                            {
                                comPort3.DataBits = 8;
                                break;
                            }
                        default:
                            break;
                    }

                    //HANDSHAKE
                    switch (int.Parse(hs))
                    {
                        case 0:
                            {
                                comPort3.Handshake = Handshake.None;
                                break;
                            }
                        case 1:
                            {
                                comPort3.Handshake = Handshake.RequestToSend;
                                break;
                            }
                        default:
                            break;
                    }


                    //PARITY
                    switch (int.Parse(parity))
                    {

                        case 0:
                            {
                                comPort3.Parity = Parity.None;
                                break;
                            }
                        case 1:
                            {
                                comPort3.Parity = Parity.Even;
                                break;
                            }
                        case 2:
                            {
                                comPort3.Parity = Parity.Odd;
                                break;
                            }
                        case 3:
                            {
                                comPort3.Parity = Parity.Space;
                                break;
                            }
                        case 4:
                            {
                                comPort3.Parity = Parity.Mark;
                                break;
                            }

                        default:
                            break;
                    }

                    #endregion


                    if (wasCOM3Open)
                        comPort3.Open();

                    Semaphore_COM3 = false;
                }

            }
            catch (Exception ex) {
                //ErrorMessageEvent(ex.Message);
            }
        }

        public static void ChangeCOMSettings(string _whichCOM) { 
            whichCOM_Setting = _whichCOM;
            Thread_ChangeSetting = new Thread(Thread_ChangeCOMSettings_Work);
            Thread_ChangeSetting.IsBackground = true;
            Thread_ChangeSetting.Priority = ThreadPriority.AboveNormal;
            Thread_ChangeSetting.Start();
        }
    }

    class TransmitterTaskDetail
    {
        public int COM_ID = 0;
        public string message = "";

        public TransmitterTaskDetail(int _ID, string _message)
        {
            COM_ID = _ID;
            message = _message;
        }
    }