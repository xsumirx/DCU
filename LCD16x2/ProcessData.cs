using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Threading;

namespace LCD16x2
{
	 static class  ProcessData
     {
         #region Variable and Delegate Declaration
         public static DataSet ds;
        public static DataTable dt;

        public static  DataColumn col_ID;
        public static   DataColumn col_Name;
        public static DataColumn col_PhoneNo;
        public static DataColumn col_IsActive;

         //IF ANY DATA IS FECTED FROM THREAD ...IT WILL BE STORED IN THIS VARIABLE
        public static uint ID_FETCHED;
        public static string NAME_FETCHED;
        public static string PHONE_FETCHED;


        //IF ANY DATA IS FECTED FROM THREAD ...IT WILL BE STORED IN THIS VARIABLE
        public static uint ID_INFO;
        public static string NAME_INFO;
        public static string PHONE_INFO;


         /*
          * What Kind of Function Thread will Perform
          * 1.Add New Entry to Database
          * 2.Modify or Update some Value to Database
          * 3.Delete Value to Database
         */
        public static uint ThreadAction = 0;

         //Thread with which all the operation of this class will  be performed;
        public static Thread Thread_DatabaseHandler;

         //Semaphore for this Class;
        public static bool Semaphore = false; 

         //This is for Others Class to Know What Exception has been Thrown by Any Function
        public delegate void ExceptionHandlerProcessDataClass(string str);
        public static event ExceptionHandlerProcessDataClass ExceptionHandlerProcessDataClassEvent;
         
 
         //This is to Let Other Class Know .... Thread Completed it Task
        public delegate void ProcessDataClassThreadCompleted(uint _id, string _name, string _phone, bool _isSucess,string _message);
        public static event ProcessDataClassThreadCompleted ProcessDataClassThreadCompletedEvent;

        public static string path = @"FlashDisk\UserDetail";

         //All Member List
        private static Thread Thread_GetAllMember;
        private static List<uint> allActiveID = new List<uint>();
        public static string[] activeIDforDisplay = new string[4] { "", "", "", "" };
        public static List<int> groupIndex = new List<int>();
        public static int groupIndexTracker = 0;
        private static uint listTracker = 0;
        public static uint displayScrollBar = 0;
        public static bool Flag_GetAllMemberBusy = true;
        private static bool isDatabaseChanged = true;

        public static bool isAllMemberListEmpty = false;

         //Print All Member Variable
        public static List<EachMemberDetail> AllMemberPrint_List = new List<EachMemberDetail>();



#endregion
        public static void InitDatabase() {
            try
            {
                ds = new DataSet("Database");
                dt = new DataTable("UserDetails");

                System.Type typeInt32 = System.Type.GetType("System.UInt32");
                System.Type typeString = System.Type.GetType("System.String");
                System.Type typeBoolean = System.Type.GetType("System.Boolean");


                col_ID = new DataColumn("ID", typeInt32);
                col_ID.Unique = true;

                dt.Columns.Add(col_ID);
                dt.PrimaryKey = new DataColumn[] { dt.Columns["ID"] };

                col_Name = new DataColumn("Person Name", typeString);
                dt.Columns.Add(col_Name);

                col_PhoneNo = new DataColumn("Phone No", typeString);
                dt.Columns.Add(col_PhoneNo);

                col_IsActive = new DataColumn("IsActive", typeBoolean);
                dt.Columns.Add(col_IsActive);

                ds.Tables.Add(dt);

                CreateDatabase();
            }
            catch (Exception ex) {

                //ExceptionHandlerProcessDataClassEvent(ex.ToString());
            }


        }

        #region CriticalRegion
        public static void CreateDatabase() {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if(!File.Exists(path + @"\Users.xml"))
                ds.WriteXml(path + @"\Users.xml");
        }


        //Create New Row With Primary Key  ||| Use It CareFully
        public static object[] AddtoDatabase(uint cID,string name,string P_no) {
            object[] no = new object[] { true, "All Right" }; 
            try
            {
                ds.Clear();
                ds.ReadXml(path + @"\Users.xml");
                DataRow dr = dt.NewRow();

                dr["Person Name"] = name;
                dr["Phone No"] = P_no;
                dr["ID"] = cID;
                dr["IsActive"] = true;

                dt.Rows.Add(dr);
                //ds.AcceptChanges();
                //ds.WriteXml(path + @"\Users.xml");
            }
            catch (Exception ex) {
                no[0] = false;
                no[1] = ex.Message;
            }

            return no;
        }

        //Read All Entry In the Database and Return a Array of RowType ||CareFully
        public static List<DataRow> ReadAllDatabase() {
            ds.Clear();
            ds.ReadXml(path + @"\Users.xml");

            List<DataRow> tempDR = new List<DataRow>();

            for (int i = 0; i < 1000; i++) {
                try
                {
                    DataRow ddd = dt.Rows.Find(i);
                    if (bool.Parse(ddd["IsActive"].ToString()))
                        tempDR.Add(ddd);
                }
                catch (Exception ex) { };
            }



            return tempDR;

        }
        #endregion

        //Update a Value for a Particular Person ---- Either Update or Remove a Person fROM DataBase
        public static bool UpdateInDataBase(uint ID,string Person,string phone,bool Remove){
            bool no = false;
            try
            {
                //ds.Clear();
                //ds.ReadXml(path + @"\Users.xml");

                DataRow Temp_DataRow = dt.Rows.Find(ID);

                if (Remove)
                {
                    if (Temp_DataRow != null)
                    {
                        Temp_DataRow["IsActive"] = false;
                        no = true;
                    }
                }
                else {
                    if (Temp_DataRow != null)
                    {
                        Temp_DataRow["Person Name"] = Person;
                        Temp_DataRow["Phone No"] = phone;
                        Temp_DataRow["IsActive"] = true;
                        no = true;
                    }
                    else {
                        AddtoDatabase(ID, Person, phone);
                    }
                }
                
                
                
            }
            catch (Exception ex)
            {
                no = false;
            }

            return no;
        }

        public static object[] ReadEntry(uint ID) {
            object[] ValueReturn = new object[] {"",""};
            try
            {
                //ds.Clear();
                //ds.ReadXml(path + @"\Users.xml");


                DataRow ddd = dt.Rows.Find(ID);

                ValueReturn[0] = ddd["Person Name"].ToString();
                ValueReturn[1] = ddd["Phone No"].ToString();

                //ds.RejectChanges();
            }
            catch (Exception ex) {
                //ExceptionHandlerProcessDataClassEvent(ex.ToString());
            }
            return ValueReturn;
            
        }

        public static string GetName(uint ID)
        {
            string ValueReturn = "";
            try
            {
                DataRow ddd = dt.Rows.Find(ID);

                ValueReturn = ddd["Person Name"].ToString();
            }
            catch (Exception ex)
            {
                //ExceptionHandlerProcessDataClassEvent(ex.ToString());
            }
            return ValueReturn;

        }

        public static string GetPh(uint ID)
        {
            string ValueReturn = "";
            try
            {
                DataRow ddd = dt.Rows.Find(ID);

                ValueReturn = ddd["Phone No"].ToString();
            }
            catch (Exception ex)
            {
                //ExceptionHandlerProcessDataClassEvent(ex.ToString());
            }
            return ValueReturn;

        }

        //Check If ID is Assigned to a Particular Member
        public static bool IsSpaceAvailable(uint ID) {
            bool IsExist = false;
            //LCDFrameProcessor.startOperation(29850);
            try
            {
                //ds.Clear();
                //ds.ReadXml(path + @"\Users.xml");

                //throw new Exception("Read XML Completed");

                DataRow Temp_DataRow = dt.Rows.Find(ID);
                if (Temp_DataRow != null)
                {
                    IsExist = bool.Parse(Temp_DataRow["IsActive"].ToString());
                }
                //ds.RejectChanges();
            }
            catch (Exception ex) {
                //ExceptionHandlerProcessDataClassEvent(ex.ToString());
            }
            return IsExist;   
        }

        public static void Thread_ProcessData_Work() {

            while (Semaphore) {
                Thread.Sleep(100);
            }

            Semaphore = true;
            try
            {
                ds.AcceptChanges();
                ds.WriteXml(path + @"\Users.xml");
                ds.Clear();
                ds.ReadXml(path + @"\Users.xml");
            }
            catch (Exception ex) { }
            Semaphore = false;

            
        }

        public static void SaveChanges() {
            Thread_DatabaseHandler = new Thread(Thread_ProcessData_Work);
            Thread_DatabaseHandler.IsBackground = true;
            Thread_DatabaseHandler.Priority = ThreadPriority.Highest;
            isDatabaseChanged = true;
            Thread_DatabaseHandler.Start();
        }

        public static void GetAllMemberForPrint() {
            try
            {
                if (true)
                {
                    isDatabaseChanged = false;
                    AllMemberPrint_List.Clear();

                    for (int i = 0; i < 1000; i++)
                    {
                        if (IsSpaceAvailable((uint)i))
                        {
                            AllMemberPrint_List.Add(new EachMemberDetail((uint)i, GetName((uint)i), GetPh((uint)i)));
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }

        private static void Thread_GetAllMember_Work() {
            
            Flag_GetAllMemberBusy = true;
            try
            {
                if (true)
                {
                    isDatabaseChanged = false;
                    allActiveID.Clear();
                    groupIndex.Clear();
                    groupIndexTracker = 0;
                    listTracker = 0;
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i <= 999; i++)
                        {
                            if (IsSpaceAvailable((uint)i))
                            {
                                allActiveID.Add((uint)i);
                            }
                        }
                    }

                    if (allActiveID.Count < 1)
                    {
                        isAllMemberListEmpty = true;
                        Flag_GetAllMemberBusy = false;
                        return;
                    }


                    int quotient = allActiveID.Count / 4;
                    int reminder = allActiveID.Count % 4;
                    int remain = allActiveID.Count;



                    if (quotient != 0)
                    {
                        for (int i = 1; i <= quotient; i++)
                        {
                            groupIndex.Add(i * 4);
                        }

                    }

                    if (reminder != 0)
                    {
                        groupIndex.Add(quotient * 4 + reminder);
                    }
                }
            }
            catch (Exception ex) { }

            Flag_GetAllMemberBusy = false;
        }
        
         public static void GetAllMember() {
             try
             {
                 if (true)
                 {
                     Thread_GetAllMember = new Thread(Thread_GetAllMember_Work);
                     Thread_GetAllMember.Priority = ThreadPriority.Highest;
                     Thread_GetAllMember.IsBackground = true;

                     Thread_GetAllMember.Start();
                 }
                 listTracker = 0;
                 displayScrollBar = 0;
             }
             catch (Exception ex) { }

        }

        public static void GetNextIDfromAllMemberList(bool ForwardDirection)
        {

            uint TEMP_ID1 = 0;
            uint TEMP_ID2 = 0;
            uint TEMP_ID3 = 0;
            uint TEMP_ID4 = 0;
            try
            {
                if (!Flag_GetAllMemberBusy && !isAllMemberListEmpty) 
                {


                    if (ForwardDirection)
                    {

                        if (groupIndexTracker < groupIndex.Count - 1)
                        {
                            groupIndexTracker++;
                        }

                        if (groupIndexTracker == groupIndex.Count - 1)
                        {
                            activeIDforDisplay[0] = "";
                            activeIDforDisplay[1] = "";
                            activeIDforDisplay[2] = "";
                            activeIDforDisplay[3] = "";
                            int x = groupIndex.ElementAt(groupIndex.Count - 1) - groupIndex.ElementAt(groupIndex.Count - 2);

                            if (x == 1) {
                                TEMP_ID1 = allActiveID.ElementAt(groupIndex.ElementAt(groupIndexTracker) - 1);
                                activeIDforDisplay[0] = TEMP_ID1.ToString();

                            }
                            else if (x == 2) {
                                TEMP_ID1 = allActiveID.ElementAt(groupIndex.ElementAt(groupIndexTracker) - 2);
                                activeIDforDisplay[0] = TEMP_ID1.ToString();
                                TEMP_ID2 = allActiveID.ElementAt(groupIndex.ElementAt(groupIndexTracker) - 1);
                                activeIDforDisplay[1] = TEMP_ID2.ToString();
                            }
                            else if (x == 3) {

                                TEMP_ID1 = allActiveID.ElementAt(groupIndex.ElementAt(groupIndexTracker) - 3);
                                activeIDforDisplay[0] = TEMP_ID1.ToString();
                                TEMP_ID2 = allActiveID.ElementAt(groupIndex.ElementAt(groupIndexTracker) - 2);
                                activeIDforDisplay[1] = TEMP_ID2.ToString();
                                TEMP_ID3 = allActiveID.ElementAt(groupIndex.ElementAt(groupIndexTracker) - 1);
                                activeIDforDisplay[2] = TEMP_ID3.ToString();
                            }
                            else if (x == 4)
                            {

                                TEMP_ID1 = allActiveID.ElementAt(groupIndex.ElementAt(groupIndexTracker) - 4);
                                activeIDforDisplay[0] = TEMP_ID1.ToString();
                                TEMP_ID2 = allActiveID.ElementAt(groupIndex.ElementAt(groupIndexTracker) - 3);
                                activeIDforDisplay[1] = TEMP_ID2.ToString();
                                TEMP_ID3 = allActiveID.ElementAt(groupIndex.ElementAt(groupIndexTracker) - 2);
                                activeIDforDisplay[2] = TEMP_ID3.ToString();
                                TEMP_ID4 = allActiveID.ElementAt(groupIndex.ElementAt(groupIndexTracker) - 1);
                                activeIDforDisplay[3] = TEMP_ID4.ToString();
                            }

                        }
                        else
                        {
                            TEMP_ID1 = allActiveID.ElementAt(groupIndex.ElementAt(groupIndexTracker) - 1);
                            TEMP_ID2 = allActiveID.ElementAt(groupIndex.ElementAt(groupIndexTracker) - 2);
                            TEMP_ID3 = allActiveID.ElementAt(groupIndex.ElementAt(groupIndexTracker) - 3);
                            TEMP_ID4 = allActiveID.ElementAt(groupIndex.ElementAt(groupIndexTracker) - 4);

                            activeIDforDisplay[0] = TEMP_ID4.ToString();
                            activeIDforDisplay[1] = TEMP_ID3.ToString();
                            activeIDforDisplay[2] = TEMP_ID2.ToString();
                            activeIDforDisplay[3] = TEMP_ID1.ToString();
                        }

                        ///-------------------------------------------


                    }
                    else if (!ForwardDirection)
                    {

                        if (groupIndexTracker > 0)
                        {
                            groupIndexTracker--;
                        }


                        TEMP_ID1 = allActiveID.ElementAt(groupIndex.ElementAt(groupIndexTracker) - 4);
                        TEMP_ID2 = allActiveID.ElementAt(groupIndex.ElementAt(groupIndexTracker) - 3);
                        TEMP_ID3 = allActiveID.ElementAt(groupIndex.ElementAt(groupIndexTracker) - 2);
                        TEMP_ID4 = allActiveID.ElementAt(groupIndex.ElementAt(groupIndexTracker) - 1);

                        activeIDforDisplay[0] = TEMP_ID1.ToString();
                        activeIDforDisplay[1] = TEMP_ID2.ToString();
                        activeIDforDisplay[2] = TEMP_ID3.ToString();
                        activeIDforDisplay[3] = TEMP_ID4.ToString();
                    }
                }
            }
            catch (Exception ex) { }
        }

        public static List<EachMemberDetail> GetAllMemberDetail_New() {

            List<EachMemberDetail> returnList = new List<EachMemberDetail>();
            try
            {
                for (int i = 0; i <= 999; i++) {
                    if (IsSpaceAvailable(uint.Parse(i.ToString()))) {
                        string loop_Name = GetName(uint.Parse(i.ToString()));
                        string loop_Phone = GetPh(uint.Parse(i.ToString()));
                        returnList.Add(new EachMemberDetail(uint.Parse(i.ToString()), loop_Name, loop_Phone));
                    }
                }



            }
            catch (Exception ex) { }

            return returnList;
        }
     }

     public class EachMemberDetail {
         public uint ID;
         public string Name;
         public string Ph;

         public EachMemberDetail(uint _ID, string _Name, string _Ph) {
             ID = _ID;
             Name = _Name;
             Ph = _Ph;
         }

     }
}
