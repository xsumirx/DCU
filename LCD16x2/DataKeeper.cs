using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using LCD16x2;
using System.IO;
using System.Data;
using System.Threading;

namespace LCD16x2
{
    static class DataKeeper
    {
        //This is Used to HOLD the Temp Data Used in Project
        public  static string path = @"FlashDisk\UserSettings";

        public static DataSet ds;
        public static DataTable dt;

        private static DataColumn key_ID;
        private static DataColumn key_Name;
        private static DataColumn key_Value;

        //Thread with which all the operation of this class will  be performed;
        public static Thread Thread_DatabaseHandler;

        //Semaphore for this Class;
        public static bool Semaphore = false;

        public  delegate void ExceptionHandlerProcessDataClass(string str);
        public static event ExceptionHandlerProcessDataClass ExceptionHandlerProcessDataClassEvent;

        public static void InitDatabase()
        {
            try
            {
                
                ds = new DataSet("Settings");
                dt = new DataTable("UserSettings");

                System.Type typeInt32 = System.Type.GetType("System.UInt32");
                System.Type typeString = System.Type.GetType("System.String");


                key_ID = new DataColumn("KEY_ID", typeInt32);
                key_ID.Unique = true;
                dt.Columns.Add(key_ID);
                

                
                key_Name = new DataColumn("KEY_NAME", typeString);
                key_Name.Unique = true;
                dt.Columns.Add(key_Name);

                key_Value = new DataColumn("KEY_VALUE", typeString);
                dt.Columns.Add(key_Value);

                dt.PrimaryKey = new DataColumn[] { dt.Columns["KEY_NAME"] };
                ds.Tables.Add(dt);

                CreateSettingXML();

                //CreateDatabase();
            }
            catch (Exception ex)
            {

                ExceptionHandlerProcessDataClassEvent(ex.ToString());
            }


        }

        public static void CreateSettingXML()
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (!File.Exists(path + @"\Settings.xml"))
                ds.WriteXml(path + @"\Settings.xml");
        }

        public static bool AddOrUpdate(string _KEY_NAME, string _KEY_VALUE,bool shouldShave)
        {
            bool _status = false;
            uint _last_key_id = uint.Parse(dt.Rows.Count.ToString());
            DataRow Temp_DataRow;
            try
            {
                Temp_DataRow = dt.Rows.Find(new object[] { _KEY_NAME });
                if (Temp_DataRow != null)
                {
                    Temp_DataRow["KEY_VALUE"] = _KEY_VALUE;
                    _status = true;
                }
                else {
                    Temp_DataRow = dt.NewRow();
                    Temp_DataRow["KEY_NAME"] = _KEY_NAME;
                    Temp_DataRow["KEY_VALUE"] = _KEY_VALUE;
                    Temp_DataRow["KEY_ID"] = _last_key_id;
                    dt.Rows.Add(Temp_DataRow);
                    _status = true;
                }
            }
            catch (Exception ex)
            {
                _status = false;
            }
            if(shouldShave)
                SaveChanges();

            return _status;
        }

        public static bool Update(string _KEY_NAME, string _KEY_VALUE)
        {
            bool _status = false;
            DataRow Temp_DataRow;
            try
            {
                Temp_DataRow = dt.Rows.Find(new object[] { _KEY_NAME });
                if (Temp_DataRow != null)
                {
                    Temp_DataRow["KEY_VALUE"] = _KEY_VALUE;
                    _status = true;
                }
                
            }
            catch (Exception ex)
            {
                _status = false;
            }
            SaveChanges();
            return _status;
        }

        public static string GetValue(string _KEY_NAME)
        {
            string _value = "0";
            DataRow Temp_DataRow;
            try
            {
                Temp_DataRow = dt.Rows.Find(new object[] { _KEY_NAME });
                if (Temp_DataRow != null)
                {
                    _value = Temp_DataRow["KEY_VALUE"].ToString();
                }

            }
            catch (Exception ex)
            {
               //Nothing
            }

            return _value;
        }

        //Return Value of a Particular Slot
        public static object[] F2_GetBonusDeduct(string _isBonus, string _isFat, string _isCow, uint _slotNo) {
            object[] ReturnVal = new object[] {0.0,0.0,0.0 };
            string BonusDeduct = _isBonus;
            string FatSnf = _isFat;
            string CowBuf = _isCow;
            string Slot = "SLOT" + _slotNo.ToString();

            try {
                //Set Bonus
                


                if (IsKeyExist(BonusDeduct + "_" + CowBuf + "_" + FatSnf + "_" + Slot))
                {
                    //Yess Key Exist Extract Value
                    string Parameter_Val = GetValue(BonusDeduct + "_" + CowBuf + "_" + FatSnf + "_" + Slot);
                    if (Parameter_Val != "" && Parameter_Val != null) { 
                        //Use String Extraction Algorith
                        int IndexofL_Limit = Parameter_Val.IndexOf("L_LIMIT ");
                        if (IndexofL_Limit != -1)
                        {
                            string _Local_Llimit = Parameter_Val.Substring(IndexofL_Limit + 8);
                            int localIndex = _Local_Llimit.IndexOf("END");
                            if (localIndex != -1)
                            {
                                _Local_Llimit = _Local_Llimit.Remove(localIndex, _Local_Llimit.Length - localIndex);
                                ReturnVal[0] = float.Parse(_Local_Llimit);
                            }
                        }


                        int IndexofU_Limit = Parameter_Val.IndexOf("U_LIMIT ");
                        if (IndexofU_Limit != -1)
                        {
                            string _Local_Ulimit = Parameter_Val.Substring(IndexofU_Limit + 8);
                            int localIndex = _Local_Ulimit.IndexOf("END");
                            if (localIndex != -1)
                            {
                                _Local_Ulimit = _Local_Ulimit.Remove(localIndex, _Local_Ulimit.Length - localIndex);
                                ReturnVal[1] = float.Parse(_Local_Ulimit);
                            }
                        }


                        int IndexofAmount = Parameter_Val.IndexOf("AMOUNT ");
                        if (IndexofAmount != -1)
                        {
                            string _Local_Amount = Parameter_Val.Substring(IndexofAmount + 7);
                            int localIndex = _Local_Amount.IndexOf("END");
                            if (localIndex != -1)
                            {
                                _Local_Amount = _Local_Amount.Remove(localIndex, _Local_Amount.Length - localIndex);
                                ReturnVal[2] = float.Parse(_Local_Amount);
                            }
                        }
                    }
                }
                else { 
                
                    //No Key Doesn't Exist, 
                    //Do Nothing, Let the Value remturn As it is
                }

            }
            catch (Exception ex) { }

            return ReturnVal;
        }

        //Return Value in Particular Range
        public static object[] F2_GetAmountInRange(string _isBonus, string _isFat, string _isCow,float _limit)
        {
            object[] ReturnVal = new object[] {0.0,false,"Remark" };

            try
            {
                


                object[] RetVal;

                for (int i = 1; i < 6; i++) {
                    RetVal = F2_GetBonusDeduct(_isBonus, _isFat, _isCow, uint.Parse(i.ToString()));
                    if (RetVal != null) {
                        if ((float)RetVal[0] <= _limit && (float)RetVal[1] >= _limit) {
                            ReturnVal[0] = (float)RetVal[2];
                            ReturnVal[1] = true;
                            ReturnVal[2] = "Exctracted";
                            break;
                        }
                    }
                }

            }
            catch (Exception ex) { }

            return ReturnVal;
        }

        //Does Range Already Exist
        public static bool F2_DoesRangeExist(bool _isBonus, bool _isFat, bool _isCow, float _llimit, float _ulimit) {
            bool ReturnRangeExistanceStatus = false;

            return ReturnRangeExistanceStatus;
        }

        public static bool IsKeyExist(string _KEY_NAME)
        {
            bool IsExist = false;
            DataRow Temp_DataRow;
            try
            {
                Temp_DataRow = dt.Rows.Find(new object[] {_KEY_NAME });
                if (Temp_DataRow != null)
                    IsExist = true;
            }
            catch (Exception ex)
            {
                ExceptionHandlerProcessDataClassEvent(ex.ToString());
            }
            return IsExist;
        }

        public static void Thread_ProcessData_Work()
        {

            while (Semaphore)
            {
                Thread.Sleep(100);
            }

            Semaphore = true;
            try
            {
                ds.AcceptChanges();
                ds.WriteXml(path + @"\Settings.xml");
                ds.Clear();
                ds.ReadXml(path + @"\Settings.xml");
            }
            catch (Exception ex) { }

            Semaphore = false;

        }

        public static void SaveChanges()
        {
            Thread_DatabaseHandler = new Thread(Thread_ProcessData_Work);
            Thread_DatabaseHandler.IsBackground = true;
            Thread_DatabaseHandler.Priority = ThreadPriority.BelowNormal;
            Thread_DatabaseHandler.Start();
        }

    }
}
