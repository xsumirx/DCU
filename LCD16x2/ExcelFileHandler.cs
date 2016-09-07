using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using System.IO;
using FlexCel.XlsAdapter;


namespace LCD16x2
{
    public class ExcelDetailOneMember {
         /*
         * 1.ID
         * 2.NAME
         * 3.PHONE
         * 4.MILK WEIGHT
         * 5.FAT
         * 6.SNF
         * 7.CLR
         * 8.RATE
         * 9.CAT
         * 10.LACTOSE
         * 11.PROTEIN
         * 12.SOLID
         * 13.FRPOINT
         * 14.TEMPRATURE
         * 15.DENSITY
         * 16.ADDED WATER
         * 17.UPDATE TIME
         * 18.Auto/Man
         */

        public string ID;
        public string Name;
        public string Phone;
        public string MilkWeight;
        public string Fat;
        public string Snf;

        public string Clr;
        public string Rate;
        public string Category;
        public string Lactose;
        public string Protein;
        public string Solid;

        public string FrPoint;
        public string Temprature;
        public string Density;
        
        public string AddedWater;
        public string UpdateTime;
        public string AutoMan;


        public ExcelDetailOneMember(string _id, string _name, string _phone, string _milkweight, string _fat, string _snf, string _clr, string _rate, string _cat, string _Updatetime, string _lactose, string _protein, string _solid, string _frPoint, string _temprature, string _addedwater, string _density, string _AutoMan) {

            ID = _id;
            Name = _name;
            Phone = _phone;
            MilkWeight = _milkweight;
            Fat = _fat;
            Snf = _snf;

            Clr = _clr;
            Rate = _rate;
            Category = _cat;
            Lactose = _lactose;
            Protein = _protein;
            Solid = _solid;

            FrPoint = _frPoint;
            Temprature = _temprature;
            Density = _density;
            AddedWater = _addedwater;
            UpdateTime = _Updatetime;
            AutoMan = _AutoMan;
        }

    }
    static class ExcelFileHandler
    {
        public static bool collection_pathChange;


        public static string collection_xlsfilename;
        public static string collection_xlsfolder;

        public static string rateChart_xlsfilename;
        public static string rateChart_xlsfolder;

        public static string sd_card_name = "SD Card";
        public static string bankSheet_xlsfilename = "Bank";
        public static string bankSheet_xlsfolder = sd_card_name + @"\Bank";

        public static string saleSaheet_xlsFolder = sd_card_name + @"\SaleSheet";
        public static string saleSaheet_xlsFileName = "Test.xls";

        public static int rateChartExcelPath = 0;
        public static int collectionExcelPath = 0;
        public static string[] rateChartPaths = new string[] { @"FlashDisk\Excel", sd_card_name + @"\Excel" };
        public static string[] collectionPaths = new string[] { @"FlashDisk\Excel", sd_card_name + @"\Excel" };

        private static XlsFile ratechartEF;
        private static XlsFile collectionEF;

        private static XlsFile saleSheetEF;

        private static XlsFile bankSheet;

        private static XlsFile singleMemberLedger;
        public static List<singleMemberLedgerRecord> singleMemberLedgerList = new List<singleMemberLedgerRecord>();


        private static XlsFile allMemberLedger;
        public static List<allMemberLedgerRecord> allMemberLedgerList = new List<allMemberLedgerRecord>();


        private static XlsFile shiftReportfile;
        public static List<ExcelDetailOneMember> shiftReportListBuff = new List<ExcelDetailOneMember>();
        public static List<ExcelDetailOneMember> shiftReportListCow = new List<ExcelDetailOneMember>();

        public static shiftReport shiftReportData;

        public delegate void ErrorHandler(string message);
        public static event ErrorHandler ErrorHandlerEvent;

        public static void Initialize()
        {
            try {

                DirectoryInfo root = new DirectoryInfo("\\");
                DirectoryInfo[] directoryList = root.GetDirectories();
                string _SD_Name = "SD Card";

                for (int i = 0; i < directoryList.Length; ++i)
                {
                    if ((directoryList[i].Attributes & FileAttributes.Temporary) != 0 && directoryList[i].FullName.StartsWith("\\SD Card"))
                    {
                        _SD_Name = directoryList[i].FullName;
                        break;
                    }
                }

                sd_card_name = _SD_Name;
                bankSheet_xlsfolder = sd_card_name + @"\Bank";
                saleSaheet_xlsFolder = sd_card_name + @"\SaleSheet";
                rateChartPaths[1] = sd_card_name + @"\Excel";
                collectionPaths[1] = sd_card_name + @"\Excel";

                collection_xlsfilename = DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString();
                
                //ratechartEF = new XlsFile(rateChartPaths[rateChartExcelPath], true);
                collection_xlsfilename = DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString();
                collection_xlsfolder = collectionPaths[1];

                rateChart_xlsfolder = rateChartPaths[1];
                rateChart_xlsfilename = "ratechart";

                if (!Directory.Exists(collection_xlsfolder) && collection_xlsfolder != @"SD Card\Excel")
                {
                    Directory.CreateDirectory(collection_xlsfolder);
                }

                if (!Directory.Exists(saleSaheet_xlsFolder)) {
                    Directory.CreateDirectory(saleSaheet_xlsFolder);
                }

                if (!File.Exists(collection_xlsfolder + @"\" + collection_xlsfilename + ".xls") && collection_xlsfolder != @"SD Card\Excel")
                    CreateNewExcel(collection_xlsfilename, collection_xlsfolder);
                //collectionEF = new XlsFile(collection_xlsfolder + @"\" + collection_xlsfilename+".xls" , true);
            }
            catch (Exception ex) {
                //ErrorHandlerEvent("Excel Hnadler Class : \n\n" + ex.Message);
            }
        }

        #region rateChartExcelSheet

        public static void WriteCellRateChart(string row, string  col, string  value,int sheetNo)
        {

            /*1.FAT-SNF COW
             * 2.FAT-SNF BUFF
             * 3.FAT-CLR COW
             * 4.FAT-CLR BUFF
             * 5.FAT Only COW
             * 6.FAT Only Buff
             */

            try
            {

                if (!Directory.Exists(rateChart_xlsfolder))
                    Directory.CreateDirectory(rateChart_xlsfolder);


                if (!File.Exists(rateChart_xlsfolder + @"\" + rateChart_xlsfilename + ".xls"))
                {
                    //Rate Chart Excel File NOT Found....Create One
                    ratechartEF = new XlsFile();
                    ratechartEF.NewFile(6);

                    ratechartEF.ActiveSheet = 1;
                    ratechartEF.SheetName = "1";
                    ratechartEF.SetCellValue(1,1,"FAT-SNF (COW)");
                    //ratechartEF.SetCellFormat(1, 1, 50, 50, 0);


                    ratechartEF.ActiveSheet = 2;
                    ratechartEF.SheetName = "2";
                    ratechartEF.SetCellValue(1, 1, "FAT-SNF (BUF)");
                    //ratechartEF.SetCellFormat(1, 1, 50, 50, 0);

                    ratechartEF.ActiveSheet = 3;
                    ratechartEF.SheetName = "3";
                    ratechartEF.SetCellValue(1, 1, "FAT-CLR (COW)");
                    //ratechartEF.SetCellFormat(1, 1, 50, 50, 0);

                    ratechartEF.ActiveSheet = 4;
                    ratechartEF.SheetName = "4";
                    ratechartEF.SetCellValue(1, 1, "FAT-CLR (BUFF)");
                    //ratechartEF.SetCellFormat(1, 1, 50, 50, 0);

                    ratechartEF.ActiveSheet = 5;
                    ratechartEF.SheetName = "5";
                    ratechartEF.SetCellValue(1, 1, "CLR ONLY (COW)");
                    //ratechartEF.SetCellFormat(1, 1, 50, 50, 0);

                    ratechartEF.ActiveSheet = 6;
                    ratechartEF.SheetName = "6";
                    ratechartEF.SetCellValue(1, 1, "CLR ONLY (BUFF)");
                    //ratechartEF.SetCellFormat(1, 1, 50, 50, 0);

                    

                    ratechartEF = new XlsFile(rateChart_xlsfolder + @"\" + rateChart_xlsfilename + ".xls", true);
                }
                else { 
                    //Rate Chart Excel File Exist.....Read it
                    ratechartEF = new XlsFile(rateChart_xlsfolder + @"\" + rateChart_xlsfilename + ".xls",true);
                }


                
                //Select Rate Chart
                ratechartEF.ActiveSheet = sheetNo;

                int totalCol = ratechartEF.ColCountInRow(1);
                int totalRow = ratechartEF.RowCount;

                int ActiveCol = 0;
                int ActiveRow = 0;

                while (ratechartEF.GetCellValue(totalRow, 1) == null) {
                    totalRow--;
                }

                while (ratechartEF.GetCellValue(1, totalCol) == null)
                {
                    totalCol--;
                }


                var colNO = ratechartEF.Find(Math.Round(double.Parse(col),1), new FlexCel.Core.TXlsCellRange(1, 2, 1, totalCol), null, false, false, false, true);
                var rowNo = ratechartEF.Find(Math.Round(double.Parse(row),1), new FlexCel.Core.TXlsCellRange(2, 1, totalRow, 1), null, false, false, false, true);

                if (colNO == null)
                {
                    ratechartEF.SetCellValue(1, totalCol + 1, Math.Round(double.Parse(col), 1));
                    ActiveCol = totalCol + 1;
                }
                else
                {
                    ActiveCol = colNO.Col;
                }

                if (rowNo == null)
                {
                    ratechartEF.SetCellValue(totalRow + 1, 1, Math.Round(double.Parse(row), 1));
                    ActiveRow = totalRow + 1;
                }
                else
                {
                    ActiveRow = rowNo.Row;
                }

                ratechartEF.AllowOverwritingFiles = true;
                ratechartEF.SetCellFormat(ActiveRow, ActiveCol, 0);

                ratechartEF.SetCellValue(ActiveRow, ActiveCol, Math.Round(double.Parse(value),2));

                ratechartEF.Save(rateChart_xlsfolder + @"\" + rateChart_xlsfilename + ".xls");
                

            }
            catch (Exception ex)
            {
                //ErrorHandlerEvent("ExcelFileHandler ClASS : \n\n" + ex.Message);
            }

        }
        public static string readCellRateChart(string row, string col,int sheetNo)
        {
            string _tempReturn = "0";
            try
            {

                if (File.Exists(rateChart_xlsfolder + @"\" + rateChart_xlsfilename + ".xls"))
                {
                    ratechartEF = new XlsFile(rateChart_xlsfolder + @"\" + rateChart_xlsfilename + ".xls");
                    ratechartEF.ActiveSheet = sheetNo;

                    int totalCol = ratechartEF.ColCountInRow(1);
                    int totalRow = ratechartEF.GetRowCount(1);

                    int ActiveCol = 0;
                    int ActiveRow = 0;

                    var colNO = ratechartEF.Find(col, new FlexCel.Core.TXlsCellRange(1, 2, 1, ratechartEF.ColCountInRow(1)), null, false, false, false, false);
                    var rowNo = ratechartEF.Find(row, new FlexCel.Core.TXlsCellRange(2, 1, ratechartEF.GetRowCount(1), 1), null, false, false, false, false);

                    if (colNO != null && rowNo != null)
                    {
                        var s = ratechartEF.GetCellValue(rowNo.Row, colNO.Col);
                        _tempReturn = s.ToString();
                    }
                    else
                    {
                        return "0";
                    }
                }
            }
            catch (Exception ex)
            {
                //ErrorHandlerEvent("ExcelFileHandler ClASS : \n\n" + ex.Message);
                return "0";
            }

            return _tempReturn;
        }

        #endregion


        #region collectionExcelSheet

        public static void WriteCellCollection(string _id,string _name,string _phone,double _milkweight,double _fat,double _snf,double _clr,double _rate,string _cat,string _shift,double _lactose,double _protein,double _solid,double _frPoint,double _temprature,double _addedwater,double _density,string AutoMan,double _cash,double _bank) {
            try
            {
                string _f = DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString();
                if (_f != collection_xlsfilename) {
                    collection_pathChange = true;
                    collection_xlsfilename = _f;
                }

                if (File.Exists(collection_xlsfolder + @"\" + collection_xlsfilename + ".xls"))
                {
                    if (collectionEF == null)
                    {
                        collectionEF = new XlsFile(collection_xlsfolder + @"\" + collection_xlsfilename + ".xls", true);
                    }
                }
                else {
                    collection_pathChange = true;
                }

                if (collection_pathChange) {
                    if (File.Exists(collection_xlsfolder +@"\"+ collection_xlsfilename+".xls"))
                    {
                        collectionEF = new XlsFile(collection_xlsfolder + @"\" + collection_xlsfilename + ".xls", true);
                    }
                   else {
                       if (!Directory.Exists(collection_xlsfolder))
                           Directory.CreateDirectory(collection_xlsfolder);
                       CreateNewExcel(collection_xlsfilename, collection_xlsfolder);
                       collectionEF = new XlsFile(collection_xlsfolder + @"\" + collection_xlsfilename + ".xls", true);
                        
                    }   
                }

                //Select Sheet

                if (collectionEF.GetSheetIndex(DateTime.Now.Day.ToString() + "_" + _shift,false) == -1)
                {
                    int lastsheet = collectionEF.SheetCount;
                    collectionEF.InsertAndCopySheets(lastsheet, lastsheet, 1);
                    collectionEF.ActiveSheet = lastsheet;
                    collectionEF.SheetName = DateTime.Now.Day.ToString() + "_" + _shift;
                }
                else
                    collectionEF.ActiveSheetByName = DateTime.Now.Day.ToString() + "_" + _shift;


                
                int activeSheetNo = collectionEF.ActiveSheet;
                var rowNo = collectionEF.Find(double.Parse(_id), new FlexCel.Core.TXlsCellRange(1, 1, 1001, 1), new FlexCel.Core.TCellAddress(1, 1), true, false, false, true);

                if (rowNo != null)
                {
                    /*
                     * 1.ID
                     * 2.NAME
                     * 3.PHONE
                     * 4.MILK WEIGHT
                     * 5.FAT
                     * 6.SNF
                     * 7.CLR
                     * 8.RATE
                     * 9.CAT
                     * 10.LACTOSE
                     * 11.PROTEIN
                     * 12.SOLID
                     * 13.FRPOINT
                     * 14.TEMPRATURE
                     * 15.DENSITY
                     * 16.ADDED WATER
                     * 17.UPDATE TIME
                     * * 18.Auto/Man
                     */

                    collectionEF.SetCellValue(rowNo.Row, 2, _name);
                    collectionEF.SetCellValue(rowNo.Row, 3, _phone);
                    collectionEF.SetCellValue(rowNo.Row, 4, Math.Round(_milkweight,2));
                    collectionEF.SetCellValue(rowNo.Row, 5, Math.Round(_fat, 2));
                    collectionEF.SetCellValue(rowNo.Row, 6, Math.Round(_snf, 2));
                    collectionEF.SetCellValue(rowNo.Row, 7, Math.Round(_clr,2));
                    collectionEF.SetCellValue(rowNo.Row, 8, Math.Round(_rate, 2));
                    collectionEF.SetCellValue(rowNo.Row, 9, _cat);
                    collectionEF.SetCellValue(rowNo.Row, 10, Math.Round(_lactose, 2));
                    collectionEF.SetCellValue(rowNo.Row, 11, Math.Round(_protein, 2));
                    collectionEF.SetCellValue(rowNo.Row, 12, Math.Round(_solid, 2));
                    collectionEF.SetCellValue(rowNo.Row, 13, Math.Round(_frPoint, 2));
                    collectionEF.SetCellValue(rowNo.Row, 14, Math.Round(_temprature, 2));
                    collectionEF.SetCellValue(rowNo.Row, 15, Math.Round(_density, 2));
                    collectionEF.SetCellValue(rowNo.Row, 16, Math.Round(_addedwater, 2));
                    collectionEF.SetCellValue(rowNo.Row, 17, FlexCel.Core.FlxDateTime.ToOADate(DateTime.Now,false));
                    collectionEF.SetCellValue(rowNo.Row, 18, AutoMan);

                    collectionEF.Save(collection_xlsfolder +@"/"+ collection_xlsfilename+".xls");
                }
                else {
                    int totalRow = collectionEF.RowCount;
                    


                    while (collectionEF.GetCellValue(totalRow, 1) == null)
                    {
                        totalRow--;
                    }


                    /*
                    * 1.ID
                    * 2.NAME
                    * 3.PHONE
                    * 4.MILK WEIGHT
                    * 5.FAT
                    * 6.SNF
                    * 7.CLR
                    * 8.RATE
                    * 9.CAT
                    * 10.LACTOSE
                    * 11.PROTEIN
                    * 12.SOLID
                    * 13.FRPOINT
                    * 14.TEMPRATURE
                    * 15.DENSITY
                    * 16.ADDED WATER
                    * 17.UPDATE TIME
                     * 18.Auto/Man
                    */
                    
                    collectionEF.SetCellValue((totalRow + 1), 1, double.Parse(_id));
                    collectionEF.SetCellValue((totalRow + 1), 2, _name);
                    collectionEF.SetCellValue((totalRow + 1), 3, _phone);
                    collectionEF.SetCellValue((totalRow + 1), 4, Math.Round(_milkweight, 2));
                    collectionEF.SetCellValue((totalRow + 1), 5, Math.Round(_fat, 2));
                    collectionEF.SetCellValue((totalRow + 1), 6, Math.Round(_snf, 2));
                    collectionEF.SetCellValue((totalRow + 1), 7, Math.Round(_clr),2);
                    collectionEF.SetCellValue((totalRow + 1), 8, Math.Round(_rate, 2));
                    collectionEF.SetCellValue((totalRow + 1), 9, _cat);
                    collectionEF.SetCellValue((totalRow + 1), 10, Math.Round(_lactose, 2));
                    collectionEF.SetCellValue((totalRow + 1), 11, Math.Round(_protein, 2));
                    collectionEF.SetCellValue((totalRow + 1), 12, Math.Round(_solid, 2));
                    collectionEF.SetCellValue((totalRow + 1), 13, Math.Round(_frPoint, 2));
                    collectionEF.SetCellValue((totalRow + 1), 14, Math.Round(_temprature, 2));
                    collectionEF.SetCellValue((totalRow + 1), 15, Math.Round(_density, 2));
                    collectionEF.SetCellValue((totalRow + 1), 16, Math.Round(_addedwater, 2));
                    collectionEF.SetCellValue((totalRow + 1), 17, FlexCel.Core.FlxDateTime.ToOADate(DateTime.Now,false));
                    collectionEF.SetCellValue((totalRow + 1), 18, AutoMan);
                    collectionEF.SetCellValue((totalRow + 1), 19, Math.Round(_cash, 2));
                    collectionEF.SetCellValue((totalRow + 1), 20, Math.Round(_bank, 2));

                    collectionEF.Save(collection_xlsfolder + @"/" + collection_xlsfilename + ".xls");
                }
            }
            catch (Exception ex) {
                //ErrorHandlerEvent("ExcelFileHandler Class : \n\n" + ex.Message);
            }
        }
        public static object[] ReadCellCollection(string _id,string _day,string _month,string _year,string _shift) { 
            /*
             * 1.Name
             * 2.Phone
             * 3.milk weight
             * 4.fat
             * 5.snf
             * 6.clr
             * 7.rate
             * 8.shift
             */

            object[] dataToReturn = new object[] {"","","","","","","","" };

            try
            {
                if (File.Exists(collection_xlsfolder + @"\" + _month + "_" + _year + ".xls"))
                {
                    collectionEF.Open(collection_xlsfolder + @"\" + _month + "_" + _year + ".xls");
                }
                else {
                    return dataToReturn;
                }

                if (collectionEF.GetSheetIndex(_day + "_" + _shift, false) == -1)
                {
                    return dataToReturn;
                }
                else
                    collectionEF.ActiveSheetByName = _day + "_" + _shift;

                var rowNo = collectionEF.Find(double.Parse(_id), new FlexCel.Core.TXlsCellRange(1, 1, 1001, 1), new FlexCel.Core.TCellAddress(1, 1), true, false, false, true);

                if (rowNo != null)
                {   
                    
                    dataToReturn[0] = collectionEF.GetCellValue(rowNo.Row, 2).ToString();
                    dataToReturn[1] = collectionEF.GetCellValue(rowNo.Row, 3).ToString();
                    dataToReturn[2] = collectionEF.GetCellValue(rowNo.Row, 4).ToString();
                    dataToReturn[3] = collectionEF.GetCellValue(rowNo.Row, 5).ToString();
                    dataToReturn[4] = collectionEF.GetCellValue(rowNo.Row, 6).ToString();
                    dataToReturn[5] = collectionEF.GetCellValue(rowNo.Row, 7).ToString();
                    dataToReturn[6] = collectionEF.GetCellValue(rowNo.Row, 8).ToString();
                    dataToReturn[7] = collectionEF.GetCellValue(rowNo.Row, 9).ToString();
                }
                
            }
            catch (Exception ex)
            {
                //ErrorHandlerEvent("ExcelFileHandler Class : \n\n" + ex.Message);
                
            }

            return dataToReturn;

        }

        public static void WriteCellCollectionManual(string _id, string _name, string _phone, string _milkweight, string _fat, string _snf, string _clr, string _rate, string _shift,string _day,string _month, string _year)
        {
            try
            {
                collection_xlsfilename = _month + "_" + _year;


                if (true)
                {
                    if (File.Exists(collection_xlsfolder + @"\" + collection_xlsfilename + ".xls"))
                    {
                        collectionEF = new XlsFile(collection_xlsfolder + @"\" + collection_xlsfilename + ".xls", true);
                    }
                    else
                    {

                        CreateNewExcel(collection_xlsfilename, collection_xlsfolder);
                        collectionEF = new XlsFile(collection_xlsfolder + @"\" + collection_xlsfilename + ".xls", true);

                    }
                }

                //Select Sheet

                if (collectionEF.GetSheetIndex(_day+"_"+_shift, false) == -1)
                {
                    int lastsheet = collectionEF.SheetCount;
                    collectionEF.InsertAndCopySheets(lastsheet, lastsheet, 1);
                    collectionEF.ActiveSheet = lastsheet;
                    collectionEF.SheetName = _day + "_" + _shift;
                }
                else
                    collectionEF.ActiveSheetByName = _day + "_" + _shift;



                int activeSheetNo = collectionEF.ActiveSheet;
                var rowNo = collectionEF.Find(double.Parse(_id), new FlexCel.Core.TXlsCellRange(1, 1, 1001, 1), new FlexCel.Core.TCellAddress(1, 1), true, false, false, true);

                if (rowNo != null)
                {
                    collectionEF.SetCellValue(rowNo.Row, 2, _name);
                    collectionEF.SetCellValue(rowNo.Row, 3, _phone);
                    collectionEF.SetCellValue(rowNo.Row, 4, Math.Round(double.Parse(_milkweight), 2));
                    collectionEF.SetCellValue(rowNo.Row, 5, Math.Round(double.Parse(_fat), 2));
                    collectionEF.SetCellValue(rowNo.Row, 6, Math.Round(double.Parse(_snf), 2));
                    collectionEF.SetCellValue(rowNo.Row, 7, int.Parse(_clr));
                    collectionEF.SetCellValue(rowNo.Row, 8, Math.Round(double.Parse(_rate), 2));
                    //collectionEF.SetCellValue(rowNo.Row, 9, _shift);
                    collectionEF.Save(collection_xlsfolder + @"/" + collection_xlsfilename + ".xls");
                }
                else
                {
                    int totalRow = collectionEF.RowCount;



                    while (collectionEF.GetCellValue(totalRow, 1) == null)
                    {
                        totalRow--;
                    }



                    collectionEF.SetCellValue((totalRow + 1), 1, double.Parse(_id));
                    collectionEF.SetCellValue((totalRow + 1), 2, _name);
                    collectionEF.SetCellValue((totalRow + 1), 3, _phone);
                    collectionEF.SetCellValue((totalRow + 1), 4, Math.Round(double.Parse(_milkweight), 2));
                    collectionEF.SetCellValue((totalRow + 1), 5, Math.Round(double.Parse(_fat), 2));
                    collectionEF.SetCellValue((totalRow + 1), 6, Math.Round(double.Parse(_snf), 2));
                    collectionEF.SetCellValue((totalRow + 1), 7, int.Parse(_clr));
                    collectionEF.SetCellValue((totalRow + 1), 8, Math.Round(double.Parse(_rate), 2));
                    collectionEF.SetCellValue((totalRow + 1), 9, _shift);
                    collectionEF.Save(collection_xlsfolder + @"/" + collection_xlsfilename + ".xls");
                }
            }
            catch (Exception ex)
            {
                //ErrorHandlerEvent("ExcelFileHandler Class : \n\n" + ex.Message);
            }
        }

        public static bool IsCollectionExist(string _id,string _day,string _month,string _year,string _shift) {
            bool dataToReturn = false;
            try
            {
                if (File.Exists(collection_xlsfolder + @"\" + _month + "_" + _year + ".xls"))
                {

                        collectionEF = new XlsFile(collection_xlsfolder + @"\" + _month + "_" + _year + ".xls",true);
                }
                else
                {
                    return dataToReturn;
                }

                if (collectionEF.GetSheetIndex(_day + "_" + _shift, false) == -1)
                {
                    return dataToReturn;
                }
                else
                    collectionEF.ActiveSheetByName = _day + "_" + _shift;

                var rowNo = collectionEF.Find(double.Parse(_id), new FlexCel.Core.TXlsCellRange(1, 1, 1001, 1), new FlexCel.Core.TCellAddress(1, 1), true, false, false, true);

                if (rowNo != null)
                {
                    dataToReturn = true;
                }
            }
            catch (Exception ex) { }

            return dataToReturn;
        }

        public static List<ExcelDetailOneMember> GetDetail_ID(string _id, string _day, string _month, string _year, string _shift) {
            List<ExcelDetailOneMember> returnList = new List<ExcelDetailOneMember>();

            try
            {
                if (File.Exists(collection_xlsfolder + @"\" + _month + "_" + _year + ".xls"))
                {

                    collectionEF = new XlsFile(collection_xlsfolder + @"\" + _month + "_" + _year + ".xls", true);
                }
                else
                {
                    return returnList;
                }

                if (collectionEF.GetSheetIndex(_day + "_" + _shift, false) == -1)
                {
                    return returnList;
                }
                else
                    collectionEF.ActiveSheetByName = _day + "_" + _shift;

                var rowNo = collectionEF.Find(double.Parse(_id), new FlexCel.Core.TXlsCellRange(1, 1, 1001, 1), new FlexCel.Core.TCellAddress(1, 1), true, false, false, true);

                if (rowNo != null)
                {

                    string ID =         collectionEF.GetCellValue(rowNo.Row, 1).ToString();
                    string Name =       collectionEF.GetCellValue(rowNo.Row, 2).ToString();
                    string Phone =      collectionEF.GetCellValue(rowNo.Row, 3).ToString();
                    string MilkWeight = collectionEF.GetCellValue(rowNo.Row, 4).ToString();
                    string Fat =        collectionEF.GetCellValue(rowNo.Row, 5).ToString();
                    string Snf =        collectionEF.GetCellValue(rowNo.Row, 6).ToString();

                    string Clr =        collectionEF.GetCellValue(rowNo.Row, 7).ToString();
                    string Rate =       collectionEF.GetCellValue(rowNo.Row, 8).ToString();
                    string Category =   collectionEF.GetCellValue(rowNo.Row, 9).ToString();
                    string Lactose =    collectionEF.GetCellValue(rowNo.Row, 10).ToString();
                    string Protein =    collectionEF.GetCellValue(rowNo.Row, 11).ToString();
                    string Solid =      collectionEF.GetCellValue(rowNo.Row, 12).ToString();

                    string FrPoint =    collectionEF.GetCellValue(rowNo.Row, 13).ToString();
                    string Temprature = collectionEF.GetCellValue(rowNo.Row, 14).ToString();
                    string Density =    collectionEF.GetCellValue(rowNo.Row, 15).ToString();
                    string AddedWater = collectionEF.GetCellValue(rowNo.Row, 16).ToString();
                    string UpdateTime = collectionEF.GetCellValue(rowNo.Row, 17).ToString();
                    string AutoMan = collectionEF.GetCellValue(rowNo.Row, 18).ToString();

                    returnList.Add(new ExcelDetailOneMember(ID, Name, Phone, MilkWeight, Fat, Snf, Clr, Rate, Category, UpdateTime, Lactose, Protein, Solid, FrPoint, Temprature, AddedWater, Density, AutoMan));
                }
            }
            catch (Exception ex) { }

            return returnList;
        }

        public static void DeleteCollection_ID(string _id, string _day, string _month, string _year, string _shift) {
            try
            {
                if (File.Exists(collection_xlsfolder + @"\" + _month + "_" + _year + ".xls"))
                {
                        collectionEF = new XlsFile(collection_xlsfolder + @"\" + _month + "_" + _year + ".xls", true);
                }
                else
                {
                    return ;
                }

                if (collectionEF.GetSheetIndex(_day + "_" + _shift, false) == -1)
                {
                    return;
                }
                else
                    collectionEF.ActiveSheetByName = _day + "_" + _shift;

                var rowNo = collectionEF.Find(double.Parse(_id), new FlexCel.Core.TXlsCellRange(1, 1, 1001, 1), new FlexCel.Core.TCellAddress(1, 1), true, false, false, true);

                if (rowNo != null)
                {
                    collectionEF.DeleteRange(new FlexCel.Core.TXlsCellRange(rowNo.Row, 1, rowNo.Row, 18), FlexCel.Core.TFlxInsertMode.ShiftRowDown);
                    collectionEF.Save(collection_xlsfolder + @"\" + _month + "_" + _year + ".xls");
                }
            }
            catch (Exception ex) { }
        }

        #endregion

        public static void DeleteShift(uint _date, uint _month, uint _year, uint _whichShift) {
            try
            {
                collection_xlsfilename = _month.ToString() + "_20" + _year.ToString();

                if (File.Exists(collection_xlsfolder + @"\" + collection_xlsfilename + ".xls")) {
                    collectionEF = new XlsFile(collection_xlsfolder + @"\" + collection_xlsfilename + ".xls", true);

                    if (collectionEF.GetSheetIndex(_date + "_" + LCDFrameProcessor.Shift[_whichShift].ToString(), false) != -1)
                    {
                        collectionEF.ActiveSheetByName = _date + "_" + LCDFrameProcessor.Shift[_whichShift].ToString();
                        //var rowNo = collectionEF.Find(double.Parse(_id), new FlexCel.Core.TXlsCellRange(1, 1, 1001, 1), new FlexCel.Core.TCellAddress(1, 1), true, false, false, true);
                        collectionEF.DeleteSheet(1);
                        collectionEF.Save(collection_xlsfolder + @"/" + collection_xlsfilename + ".xls");
                    }

                }

            }
            catch (Exception ex) { 
            
            }
        }

        public static void ReportShift(uint _date, uint _month, uint _year, uint _whichShift)
        {
            try
            {
                shiftReportListBuff.Clear();
                shiftReportListCow.Clear();
                shiftReportData = new shiftReport();
                collection_xlsfilename = _month.ToString() + "_20" + _year.ToString();

                if (File.Exists(collection_xlsfolder + @"\" + collection_xlsfilename + ".xls"))
                {
                    shiftReportfile = new XlsFile(collection_xlsfolder + @"\" + collection_xlsfilename + ".xls", true);

                    if (shiftReportfile.GetSheetIndex(_date + "_" + LCDFrameProcessor.Shift[_whichShift].ToString(), false) != -1)
                    {
                        shiftReportfile.ActiveSheetByName = _date + "_" + LCDFrameProcessor.Shift[_whichShift].ToString();
                        if (shiftReportfile.RowCount > 1) {
                            for (int i = 2; i <= shiftReportfile.RowCount; i++) {
                                string ID = shiftReportfile.GetCellValue(i, 1).ToString();
                                string Name = shiftReportfile.GetCellValue(i, 2).ToString();
                                string Phone = shiftReportfile.GetCellValue(i, 3).ToString();
                                string MilkWeight = shiftReportfile.GetCellValue(i, 4).ToString();
                                string Fat = shiftReportfile.GetCellValue(i, 5).ToString();
                                string Snf = shiftReportfile.GetCellValue(i, 6).ToString();

                                string Clr = shiftReportfile.GetCellValue(i, 7).ToString();
                                string Rate = shiftReportfile.GetCellValue(i, 8).ToString();
                                string Category = shiftReportfile.GetCellValue(i, 9).ToString();
                                string Lactose = shiftReportfile.GetCellValue(i, 10).ToString();
                                string Protein = shiftReportfile.GetCellValue(i, 11).ToString();
                                string Solid = shiftReportfile.GetCellValue(i, 12).ToString();

                                string FrPoint = shiftReportfile.GetCellValue(i, 13).ToString();
                                string Temprature = shiftReportfile.GetCellValue(i, 14).ToString();
                                string Density = shiftReportfile.GetCellValue(i, 15).ToString();
                                string AddedWater = shiftReportfile.GetCellValue(i, 16).ToString();
                                string UpdateTime = shiftReportfile.GetCellValue(i, 17).ToString();
                                string AutoMan = shiftReportfile.GetCellValue(i, 18).ToString();

                                if (shiftReportfile.GetCellValue(i, 9).ToString() == "COW") {
                                    shiftReportListCow.Add(new ExcelDetailOneMember(ID, Name, Phone, MilkWeight, Fat, Snf, Clr, Rate, Category, UpdateTime, Lactose, Protein, Solid, FrPoint, Temprature, AddedWater, Density, AutoMan));
                                }
                                else if (shiftReportfile.GetCellValue(i, 9).ToString() == "BUF") {
                                    shiftReportListBuff.Add(new ExcelDetailOneMember(ID, Name, Phone, MilkWeight, Fat, Snf, Clr, Rate, Category, UpdateTime, Lactose, Protein, Solid, FrPoint, Temprature, AddedWater, Density, AutoMan));
                                }
                            }

                            if (shiftReportListCow != null && shiftReportListCow.Count > 0)
                            {
                                float temp_averageFATCow = 0;
                                float temp_averageSNFCow = 0;

                                float temp_totalAmount = 0;
                                float temp_totalLitre = 0;

                                foreach (ExcelDetailOneMember d in shiftReportListCow) {
                                    temp_averageFATCow += float.Parse(d.Fat);
                                    temp_averageSNFCow += float.Parse(d.Snf);
                                    temp_totalAmount += float.Parse(d.Rate);
                                    temp_totalLitre += float.Parse(d.MilkWeight);
                                }

                                temp_averageFATCow = temp_averageFATCow / shiftReportListCow.Count;
                                temp_averageSNFCow = temp_averageSNFCow / shiftReportListCow.Count;

                                shiftReportData.CowEntry(temp_averageFATCow, temp_averageSNFCow, temp_totalAmount, temp_totalLitre);
                            }
                            else {
                                shiftReportData.isCowListEmpty = true;
                            }

                            if (shiftReportListBuff != null && shiftReportListBuff.Count > 0)
                            {
                                float temp_averageFATBuff = 0;
                                float temp_averageSNFBuff = 0;

                                float temp_totalAmount = 0;
                                float temp_totalLitre = 0;

                                foreach (ExcelDetailOneMember d in shiftReportListBuff)
                                {
                                    temp_averageFATBuff += float.Parse(d.Fat);
                                    temp_averageSNFBuff += float.Parse(d.Snf);
                                    temp_totalAmount += float.Parse(d.Rate);
                                    temp_totalLitre += float.Parse(d.MilkWeight);
                                }

                                temp_averageFATBuff = temp_averageFATBuff / shiftReportListBuff.Count;
                                temp_averageSNFBuff = temp_averageSNFBuff / shiftReportListBuff.Count;

                                shiftReportData.BuffEntry(temp_averageFATBuff, temp_averageSNFBuff, temp_totalAmount, temp_totalLitre);
                            }
                            else
                            {
                                shiftReportData.isBuffListEmpty = true;
                            }

                            if (shiftReportListBuff.Count > 0 || shiftReportListCow.Count > 0) {
                                shiftReportData.CalculateTotal();
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {

            }
        }

        public static void DeleteTransaction(uint _id, uint _date, uint _month, uint _year, uint _whichShift) {
            try
            {
                collection_xlsfilename = _month.ToString() + "_20" + _year.ToString();

                if (File.Exists(collection_xlsfolder + @"\" + collection_xlsfilename + ".xls"))
                {
                    collectionEF = new XlsFile(collection_xlsfolder + @"\" + collection_xlsfilename + ".xls", true);

                    if (collectionEF.GetSheetIndex(_date + "_" + LCDFrameProcessor.Shift[_whichShift].ToString(), false) != -1)
                    {
                        collectionEF.ActiveSheetByName = _date + "_" + LCDFrameProcessor.Shift[_whichShift].ToString();
                        var rowNo = collectionEF.Find(double.Parse(_id.ToString()), new FlexCel.Core.TXlsCellRange(1, 1, 1001, 1), new FlexCel.Core.TCellAddress(1, 1), true, false, false, true);

                        if (rowNo != null) {

                            collectionEF.DeleteRange(new FlexCel.Core.TXlsCellRange(rowNo.Row, 1, rowNo.Row, 8), FlexCel.Core.TFlxInsertMode.ShiftRowDown);
                            
                            collectionEF.Save(collection_xlsfolder + @"/" + collection_xlsfilename + ".xls");
                        }


                        
                    }

                }

            }
            catch (Exception ex)
            {

            }
        }

        public static object[] ReturnSingleMemberLedger(uint _id,uint fromDate, uint fromMonth, uint fromYear, uint fromShift, uint toDate, uint toMonth, uint toYear, uint toShift) {
            object[] returnVal = new object[] { "Nothing", (new ErrorMessage(false, "")) };
            try{
                singleMemberLedgerList.Clear();
                if ((toYear - fromYear) == 0)
                {
                    if ((toMonth - fromYear) >= 0 && toMonth > 0 && toMonth < 13 && fromMonth > 0 && fromMonth < 13) { 
                        int monDif = (int)(toMonth - fromMonth);
                        int local_Month = (int)fromMonth;
                        do
                        {
                            if (File.Exists(collection_xlsfolder + @"\" + local_Month.ToString() + "_20" + fromYear.ToString() + ".xls")) {
                                singleMemberLedger = new XlsFile(collection_xlsfolder + @"\" + local_Month.ToString() + "_20" + fromYear.ToString() + ".xls");
                                if (singleMemberLedger != null) {
                                    if (fromShift == toShift) {
                                        if (fromShift == 0) { 
                                            //Morning Only
                                            singleMemberLedgerList.Clear();
                                            for (int i = 1; i < 32; i++) {
                                                if (singleMemberLedger.GetSheetIndex(i.ToString() + "_M",false) != -1) {
                                                    singleMemberLedger.ActiveSheetByName = i.ToString() + "_M";

                                                    var rowNo = singleMemberLedger.Find(double.Parse(_id.ToString()), new FlexCel.Core.TXlsCellRange(1, 1, 1001, 1), new FlexCel.Core.TCellAddress(1, 1), true, false, false, true);

                                                    if (rowNo != null)
                                                    {

                                                        singleMemberLedgerList.Add(new singleMemberLedgerRecord((uint)i,(uint)local_Month,fromShift,singleMemberLedger.GetCellValue(rowNo.Row,5).ToString(),singleMemberLedger.GetCellValue(rowNo.Row,6).ToString(),singleMemberLedger.GetCellValue(rowNo.Row,4).ToString(),singleMemberLedger.GetCellValue(rowNo.Row,8).ToString()));
                                                        rowNo = null;
                                                    }
                                                }
                                            }
                                        }
                                        else if (fromShift == 1)
                                        {
                                            //Evening Only
                                            singleMemberLedgerList.Clear();
                                            for (int i = 1; i < 32; i++)
                                            {
                                                if (singleMemberLedger.GetSheetIndex(i.ToString() + "_E", false) != -1)
                                                {
                                                    singleMemberLedger.ActiveSheetByName = i.ToString() + "_E";

                                                    var rowNo = singleMemberLedger.Find(double.Parse(_id.ToString()), new FlexCel.Core.TXlsCellRange(1, 1, 1001, 1), new FlexCel.Core.TCellAddress(1, 1), true, false, false, true);

                                                    if (rowNo != null)
                                                    {

                                                        singleMemberLedgerList.Add(new singleMemberLedgerRecord((uint)i, (uint)local_Month, fromShift, singleMemberLedger.GetCellValue(rowNo.Row, 5).ToString(), singleMemberLedger.GetCellValue(rowNo.Row, 6).ToString(), singleMemberLedger.GetCellValue(rowNo.Row, 4).ToString(), singleMemberLedger.GetCellValue(rowNo.Row, 8).ToString()));
                                                        rowNo = null;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (fromShift != toShift)
                                    {
                                        //Morning Evening Both
                                        singleMemberLedgerList.Clear();
                                        for (int i = 1; i < 32; i++)
                                        {
                                            if (singleMemberLedger.GetSheetIndex(i.ToString() + "_M", false) != -1)
                                            {
                                                singleMemberLedger.ActiveSheetByName = i.ToString() + "_M";

                                                var rowNo = singleMemberLedger.Find(double.Parse(_id.ToString()), new FlexCel.Core.TXlsCellRange(1, 1, 1001, 1), new FlexCel.Core.TCellAddress(1, 1), true, false, false, true);

                                                if (rowNo != null)
                                                {

                                                    singleMemberLedgerList.Add(new singleMemberLedgerRecord((uint)i, (uint)local_Month, fromShift, singleMemberLedger.GetCellValue(rowNo.Row, 5).ToString(), singleMemberLedger.GetCellValue(rowNo.Row, 6).ToString(), singleMemberLedger.GetCellValue(rowNo.Row, 4).ToString(), singleMemberLedger.GetCellValue(rowNo.Row, 8).ToString()));
                                                    rowNo = null;
                                                }
                                            }
                                        }
                                        for (int i = 1; i < 32; i++)
                                        {
                                            if (singleMemberLedger.GetSheetIndex(i.ToString() + "_E", false) != -1)
                                            {
                                                singleMemberLedger.ActiveSheetByName = i.ToString() + "_E";

                                                var rowNo = singleMemberLedger.Find(double.Parse(_id.ToString()), new FlexCel.Core.TXlsCellRange(1, 1, 1001, 1), new FlexCel.Core.TCellAddress(1, 1), true, false, false, true);

                                                if (rowNo != null)
                                                {

                                                    singleMemberLedgerList.Add(new singleMemberLedgerRecord((uint)i, (uint)local_Month, fromShift, singleMemberLedger.GetCellValue(rowNo.Row, 5).ToString(), singleMemberLedger.GetCellValue(rowNo.Row, 6).ToString(), singleMemberLedger.GetCellValue(rowNo.Row, 4).ToString(), singleMemberLedger.GetCellValue(rowNo.Row, 8).ToString()));
                                                    rowNo = null;
                                                }
                                            }
                                        }
                                    }
                                    singleMemberLedger = null;
                                }

                            }
                            local_Month++;
                            monDif -= 1;
                        } while (monDif >= 0);

                    }else
                        returnVal[1] = new ErrorMessage(false, "Enter Month Properly");
                }
                else
                     returnVal[1] = new ErrorMessage(false, "Year Should be Same");

            }catch(Exception ex){
                returnVal[1] = new ErrorMessage(false, "Exception Occured");
            }

            return returnVal;
        }

        public static object[] returnAllmemberLedger(uint fromDate, uint fromMonth, uint fromYear, uint fromShift, uint toDate, uint toMonth, uint toYear, uint toShift,uint fromCode,uint toCode)
        {
            object[] returnVal = new object[] { "Nothing", (new ErrorMessage(false, "")) };
            try
            {
                allMemberLedgerList.Clear();

                if ((toYear - fromYear) == 0)
                {
                    if ((toMonth - fromMonth) >= 0 && toMonth > 0 && toMonth < 13 && fromMonth > 0 && fromMonth < 13 && (toCode - fromCode) >= 0)
                    {
                        int monDif = (int)(toMonth - fromMonth);
                        int local_Month = (int)fromMonth;
                        do
                        {
                            if (File.Exists(collection_xlsfolder + @"\" + local_Month.ToString() + "_20" + fromYear.ToString() + ".xls"))
                            {
                                allMemberLedger = new XlsFile(collection_xlsfolder + @"\" + local_Month.ToString() + "_20" + fromYear.ToString() + ".xls");
                                if (allMemberLedger != null)
                                {
                                    int startDay = 1;
                                    int endDay = 1;

                                    if (local_Month == fromMonth && local_Month == toMonth) {
                                        startDay = (int)fromDate;
                                        endDay = (int)toDate;
                                    }
                                    else if (local_Month == fromMonth && local_Month < toMonth) {
                                        startDay = (int)fromDate;
                                        endDay = 31;
                                    }
                                    else if (local_Month > fromMonth && local_Month == toMonth) {
                                        startDay = 1;
                                        endDay = (int)toDate;
                                    }
                                    else if (local_Month > fromMonth && local_Month < toMonth)
                                    {
                                        startDay = 1;
                                        endDay = 31;
                                    }

                                    if (fromShift == toShift)
                                    {
                                        if (fromShift == 0)
                                        {
                                            //Morning Only
                                            //allMemberLedgerList.Clear();
                                            for (int i = startDay; i <= endDay; i++)
                                            {
                                                if (allMemberLedger.GetSheetIndex(i.ToString() + "_M", false) != -1)
                                                {
                                                    allMemberLedger.ActiveSheetByName = i.ToString() + "_M";

                                                    //We are into the Correct Sheet
                                                    if (allMemberLedger.RowCount > 1) {
                                                        for (int j = 2; j <= allMemberLedger.RowCount; j++) {
                                                            var id = allMemberLedger.GetCellValue(j, 1);
                                                            var quantity = allMemberLedger.GetCellValue(j, 4);
                                                            var amount = allMemberLedger.GetCellValue(j, 8);

                                                            if (id != null && uint.Parse(id.ToString()) >= fromCode && uint.Parse(id.ToString()) <= toCode)
                                                            {
                                                                string name = ProcessData.GetName(uint.Parse(id.ToString()));
                                                                allMemberLedgerList.Add(new allMemberLedgerRecord(uint.Parse(id.ToString()), name, quantity.ToString(), amount.ToString()));
                                                            }
                                                           
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else if (fromShift == 1)
                                        {
                                            //Evening Only
                                            //allMemberLedgerList.Clear();
                                            for (int i = startDay; i <= endDay; i++)
                                            {
                                                if (allMemberLedger.GetSheetIndex(i.ToString() + "_E", false) != -1)
                                                {
                                                    allMemberLedger.ActiveSheetByName = i.ToString() + "_E";

                                                    //We are into the Correct Sheet
                                                    if (allMemberLedger.RowCount > 1)
                                                    {
                                                        for (int j = 2; j <= allMemberLedger.RowCount; j++)
                                                        {
                                                            var id = allMemberLedger.GetCellValue(j, 1);
                                                            var quantity = allMemberLedger.GetCellValue(j, 4);
                                                            var amount = allMemberLedger.GetCellValue(j, 8);

                                                            if (id != null && uint.Parse(id.ToString()) >= fromCode && uint.Parse(id.ToString()) <= toCode)
                                                            {
                                                                string name = ProcessData.GetName(uint.Parse(id.ToString()));
                                                                allMemberLedgerList.Add(new allMemberLedgerRecord(uint.Parse(id.ToString()), name, quantity.ToString(), amount.ToString()));
                                                            }

                                                        }
                                                    }
                                                }
                                            }
                                           
                                        }
                                    }
                                    else if (fromShift != toShift)
                                    {

                                            //Morning first
                                            //allMemberLedgerList.Clear();
                                            for (int i = startDay; i <= endDay; i++)
                                            {
                                                if (allMemberLedger.GetSheetIndex(i.ToString() + "_M", false) != -1)
                                                {
                                                    allMemberLedger.ActiveSheetByName = i.ToString() + "_M";

                                                    //We are into the Correct Sheet
                                                    if (allMemberLedger.RowCount > 1)
                                                    {
                                                        for (int j = 2; j <= allMemberLedger.RowCount; j++)
                                                        {
                                                            var id = allMemberLedger.GetCellValue(j, 1);
                                                            var quantity = allMemberLedger.GetCellValue(j, 4);
                                                            var amount = allMemberLedger.GetCellValue(j, 8);

                                                            if (id != null && uint.Parse(id.ToString()) >= fromCode && uint.Parse(id.ToString()) <= toCode)
                                                            {
                                                                string name = ProcessData.GetName(uint.Parse(id.ToString()));
                                                                allMemberLedgerList.Add(new allMemberLedgerRecord(uint.Parse(id.ToString()), name, quantity.ToString(), amount.ToString()));
                                                            }

                                                        }
                                                    }
                                                }
                                            }
                                        
                                        
                                            //Evening Second
                                            //allMemberLedgerList.Clear();
                                            for (int i = startDay; i <= endDay; i++)
                                            {
                                                if (allMemberLedger.GetSheetIndex(i.ToString() + "_E", false) != -1)
                                                {
                                                    allMemberLedger.ActiveSheetByName = i.ToString() + "_E";

                                                    //We are into the Correct Sheet
                                                    if (allMemberLedger.RowCount > 1)
                                                    {
                                                        for (int j = 2; j <= allMemberLedger.RowCount; j++)
                                                        {
                                                            var id = allMemberLedger.GetCellValue(j, 1);
                                                            var quantity = allMemberLedger.GetCellValue(j, 4);
                                                            var amount = allMemberLedger.GetCellValue(j, 8);

                                                            if (id != null && uint.Parse(id.ToString()) >= fromCode && uint.Parse(id.ToString()) <= toCode)
                                                            {
                                                                string name = ProcessData.GetName(uint.Parse(id.ToString()));
                                                                allMemberLedgerList.Add(new allMemberLedgerRecord(uint.Parse(id.ToString()), name, quantity.ToString(), amount.ToString()));
                                                            }

                                                        }
                                                    }
                                                }
                                            }

                                        
                                        
                                    }
                                    allMemberLedger = null;
                                }

                            }
                            local_Month++;
                            monDif -= 1;
                        } while (monDif >= 0);

                    }
                    else
                        returnVal[1] = new ErrorMessage(false, "Enter Month Properly");
                }
                else
                    returnVal[1] = new ErrorMessage(false, "Year Should be Same");

            }
            catch (Exception ex)
            {
                returnVal[1] = new ErrorMessage(false, "Exception Occured");
            }

            return returnVal;
        }

        #region SaleSheet

        public static void AddNewLocalEntry(float _amount, float _quantity, float _rate, string _cat, string _remark) {
            try
            {
                string fileName = DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString();


                if (!Directory.Exists(saleSaheet_xlsFolder)) {
                    Directory.CreateDirectory(saleSaheet_xlsFolder);
                }

                if (!File.Exists(saleSaheet_xlsFolder + @"\" + fileName +".xls")) {
                    CreateSaleSheet(fileName, saleSaheet_xlsFolder);
                }

                if (File.Exists(saleSaheet_xlsFolder + @"\" + fileName + ".xls"))
                {
                    saleSheetEF = new XlsFile(saleSaheet_xlsFolder + @"\" + fileName + ".xls", true);
                }
                else
                    return;

                

                saleSheetEF.ActiveSheet = 1;

                int lastRow = 1;
                lastRow = saleSheetEF.RowCount;



                while (saleSheetEF.GetCellValue(lastRow, 1) == null)
                {
                    lastRow--;
                }

                uint lastSerial = 0;
                string lastSerialString = saleSheetEF.GetCellValue(lastRow, 1).ToString();
                if (lastSerialString == "Sr. No.")
                {
                    lastSerial = 1;
                }
                else {
                    lastSerial = uint.Parse(saleSheetEF.GetCellValue(lastRow, 1).ToString()) + 1;
                    if (lastSerial == 0) {
                        return;
                    }
                }

                string _date = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();

                saleSheetEF.SetCellValue(lastRow + 1, 1, lastSerial);
                saleSheetEF.SetCellValue(lastRow + 1, 2, _date, 0);
                saleSheetEF.SetCellValue(lastRow + 1, 3, DateTime.Now.Hour.ToString() +":"+DateTime.Now.Minute.ToString(), 0);
                saleSheetEF.SetCellValue(lastRow + 1, 4, _quantity, 0);
                saleSheetEF.SetCellValue(lastRow + 1, 5, _amount, 0);
                saleSheetEF.SetCellValue(lastRow + 1, 6, _rate, 0);
                saleSheetEF.SetCellValue(lastRow + 1, 7, _cat, 0);
                saleSheetEF.SetCellValue(lastRow + 1, 8, _remark, 0);

                saleSheetEF.Save(saleSaheet_xlsFolder + @"\" + fileName + ".xls");

            }
            catch (Exception ex) { }
        
        }

        public static void AddNewTruckEntry(float _amount, float _quantity, float _rate, string _cat, string _truckNo)
        {
            try
            {
                string fileName = DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString();


                if (!Directory.Exists(saleSaheet_xlsFolder))
                {
                    Directory.CreateDirectory(saleSaheet_xlsFolder);
                }

                if (!File.Exists(saleSaheet_xlsFolder + @"\" + fileName + ".xls"))
                {
                    CreateSaleSheet(fileName, saleSaheet_xlsFolder);
                }

                if (File.Exists(saleSaheet_xlsFolder + @"\" + fileName + ".xls"))
                {
                    saleSheetEF = new XlsFile(saleSaheet_xlsFolder + @"\" + fileName + ".xls", true);
                }
                else
                    return;


                saleSheetEF.ActiveSheet = 2;

                int lastRow = 1;
                lastRow = saleSheetEF.RowCount;

                while (saleSheetEF.GetCellValue(lastRow, 1) == null)
                {
                    lastRow--;
                }

                uint lastSerial = 0;
                string lastSerialString = saleSheetEF.GetCellValue(lastRow, 1).ToString();
                if (lastSerialString == "Sr. No.")
                {
                    lastSerial = 1;
                }
                else
                {
                    lastSerial = uint.Parse(saleSheetEF.GetCellValue(lastRow, 1).ToString()) + 1;
                    if (lastSerial == 0)
                    {
                        return;
                    }
                }

                string _date = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
                saleSheetEF.SetCellValue(lastRow + 1, 1, lastSerial);
                saleSheetEF.SetCellValue(lastRow + 1, 2, _date, 0);
                saleSheetEF.SetCellValue(lastRow + 1, 3, DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString(), 0);
                saleSheetEF.SetCellValue(lastRow + 1, 4, _quantity, 0);
                saleSheetEF.SetCellValue(lastRow + 1, 5, _amount, 0);
                saleSheetEF.SetCellValue(lastRow + 1, 6, _rate, 0);
                saleSheetEF.SetCellValue(lastRow + 1, 7, _cat, 0);
                saleSheetEF.SetCellValue(lastRow + 1, 8, _truckNo, 0);

                saleSheetEF.Save(saleSaheet_xlsFolder + @"\" + fileName + ".xls");

            }
            catch (Exception ex) { }

        }

        public static bool DeleteLocalEntry(uint _date, uint _month, uint _year, uint _EnterSerialNo) {
            bool ReturnFlag = false;
            try
            {
                string fileName = _date.ToString() + "_" + _month.ToString() + "_" + _year.ToString();
                if (!Directory.Exists(saleSaheet_xlsFolder))
                {
                    return false;
                }

                if (!File.Exists(saleSaheet_xlsFolder + @"\" + fileName + ".xls"))
                {
                    return false;
                }

                if (File.Exists(saleSaheet_xlsFolder + @"\" + fileName + ".xls"))
                {
                    saleSheetEF = new XlsFile(saleSaheet_xlsFolder + @"\" + fileName + ".xls", true);
                }
                else
                    return false;

                if(saleSheetEF.GetSheetIndex("LOCAL SALE",false) == -1)
                    return false;


                saleSheetEF.ActiveSheet = 1;

                var rowNo = saleSheetEF.Find(Math.Round(double.Parse(_EnterSerialNo.ToString()), 1), new FlexCel.Core.TXlsCellRange(2, 1, saleSheetEF.RowCount,1 ), null, false, false, false, true);
                if (rowNo != null)
                {
                    saleSheetEF.DeleteRange(new FlexCel.Core.TXlsCellRange(rowNo.Row, 1, rowNo.Row, 8), FlexCel.Core.TFlxInsertMode.ShiftRowDown);
                    saleSheetEF.Save(saleSaheet_xlsFolder + @"\" + fileName + ".xls");
                    ReturnFlag = true;
                }
                else
                    return false;
            }
            catch (Exception ex) { }
            return ReturnFlag;
        
        }

        public static bool DeleteTruckEntry(uint _date, uint _month, uint _year, uint _EnterSerialNo)
        {
            bool ReturnFlag = false;
            try
            {
                string fileName = _date.ToString() + "_" + _month.ToString() + "_" + _year.ToString();
                if (!Directory.Exists(saleSaheet_xlsFolder))
                {
                    return false;
                }

                if (!File.Exists(saleSaheet_xlsFolder + @"\" + fileName + ".xls"))
                {
                    return false;
                }

                if (File.Exists(saleSaheet_xlsFolder + @"\" + fileName + ".xls"))
                {
                    saleSheetEF = new XlsFile(saleSaheet_xlsFolder + @"\" + fileName + ".xls", true);
                }
                else
                    return false;

                if (saleSheetEF.GetSheetIndex("TRUCK SALE", false) == -1)
                    return false;


                saleSheetEF.ActiveSheet = 2;

                var rowNo = saleSheetEF.Find(Math.Round(double.Parse(_EnterSerialNo.ToString()), 1), new FlexCel.Core.TXlsCellRange(2, 1, saleSheetEF.RowCount, 1), null, false, false, false, true);
                if (rowNo != null)
                {
                    saleSheetEF.DeleteRange(new FlexCel.Core.TXlsCellRange(rowNo.Row, 1, rowNo.Row, 8), FlexCel.Core.TFlxInsertMode.ShiftRowDown);
                    saleSheetEF.Save(saleSaheet_xlsFolder + @"\" + fileName + ".xls");
                    ReturnFlag = true;
                }
                else
                    return false;
            }
            catch (Exception ex) { }

            return ReturnFlag;

        }

        public static string DeleteLastEntry(uint _date, uint _month, uint _year, int _LastEntry, bool _isLocal) {
            string Result = "SUCCESS";
            try
            {
                string fileName = _date.ToString() + "_" + _month.ToString() + "_" + _year.ToString();
                if (!Directory.Exists(saleSaheet_xlsFolder))
                {
                    Result = "FOLDER NOT FOUND";
                    return Result;
                }

                if (!File.Exists(saleSaheet_xlsFolder + @"\" + fileName + ".xls"))
                {
                    Result = "FILE NOT FOUND";
                    return Result;
                }

                if (File.Exists(saleSaheet_xlsFolder + @"\" + fileName + ".xls"))
                {
                    saleSheetEF = new XlsFile(saleSaheet_xlsFolder + @"\" + fileName + ".xls", true);
                }
                else
                {
                    Result = "FILE NOT FOUND";
                    return Result;
                }

                if (_isLocal == false)
                {
                    if (saleSheetEF.GetSheetIndex("TRUCK SALE", false) == -1)
                    {
                        Result = "SHEET NOT FOUND";
                        return Result;
                    }
                }
                else if (_isLocal) {
                    if (saleSheetEF.GetSheetIndex("LOCAL SALE", false) == -1)
                    {
                        Result = "SHEET NOT FOUND";
                        return Result;
                    }
                }

                if (_isLocal)
                    saleSheetEF.ActiveSheet = 1;
                else
                    saleSheetEF.ActiveSheet = 2;

                int totalRow = 0;
                totalRow = saleSheetEF.RowCount;



                while (saleSheetEF.GetCellValue(totalRow, 1) == null)
                {
                    totalRow--;
                }

                int RowPos;
                if (totalRow >= _LastEntry)
                {
                    RowPos = totalRow - _LastEntry + 1;
                }
                else {
                    Result = "NOT ENOUGH ENTRY";
                    return Result;
                }
                    

                //var rowNo = saleSheetEF.Find(Math.Round(double.Parse(_EnterSerialNo), 1), new FlexCel.Core.TXlsCellRange(2, 1, saleSheetEF.RowCount, 1), null, false, false, false, true);

                if (RowPos != 0 && RowPos != 1)
                {
                    saleSheetEF.DeleteRange(new FlexCel.Core.TXlsCellRange(RowPos, 1, RowPos, 8), FlexCel.Core.TFlxInsertMode.ShiftRowDown);
                    saleSheetEF.Save(saleSaheet_xlsFolder + @"\" + fileName + ".xls");
                }
                else {
                    Result = "NO ENTRY EXIST";
                }
            }
            catch (Exception ex) {Result = "EXCEPTION"; }

            return Result;
        }

        public static bool Local_isExisted(uint _date,uint _month,uint _year,uint _serial, bool _isLocal) {
            bool Result = false;
            try
            {
                string fileName = _date.ToString() + "_" + _month.ToString() + "_" + _year.ToString();
                if (!Directory.Exists(saleSaheet_xlsFolder))
                {
                    return false;
                }

                if (!File.Exists(saleSaheet_xlsFolder + @"\" + fileName + ".xls"))
                {
                    return false;
                }

                if (File.Exists(saleSaheet_xlsFolder + @"\" + fileName + ".xls"))
                {
                    saleSheetEF = new XlsFile(saleSaheet_xlsFolder + @"\" + fileName + ".xls");
                }
                else
                    return false;

                if (_isLocal)
                    saleSheetEF.ActiveSheet = 1;
                else
                    saleSheetEF.ActiveSheet = 2;


                var rowNo = saleSheetEF.Find(Math.Round(double.Parse(_serial.ToString()), 1), new FlexCel.Core.TXlsCellRange(2, 1, saleSheetEF.RowCount, 1), null, false, false, false, true);
                if (rowNo != null)
                {
                    Result = true;
                }
            }
            catch (Exception ex) { Result = false; }

            return Result;
        }

        public static int totalEntry(uint _date, uint _month, uint _year, bool _isLocal)
        {
            int Result = -1;
            try
            {
                string fileName = _date.ToString() + "_" + _month.ToString() + "_" + _year.ToString();
                if (!Directory.Exists(saleSaheet_xlsFolder))
                {
                    return -1;
                }

                if (!File.Exists(saleSaheet_xlsFolder + @"\" + fileName + ".xls"))
                {
                    return -1;
                }

                if (File.Exists(saleSaheet_xlsFolder + @"\" + fileName + ".xls"))
                {
                    saleSheetEF = new XlsFile(saleSaheet_xlsFolder + @"\" + fileName + ".xls");
                }
                else
                {
                    return -1;
                }

                if (_isLocal == false)
                {
                    if (saleSheetEF.GetSheetIndex("TRUCK SALE", false) == -1)
                    {
                        return -1;
                    }
                }
                else if (_isLocal)
                {
                    if (saleSheetEF.GetSheetIndex("LOCAL SALE", false) == -1)
                    {
                        return -1;
                    }
                }

                if (_isLocal)
                    saleSheetEF.ActiveSheet = 1;
                else
                    saleSheetEF.ActiveSheet = 2;

                int totalRow = 0;
                totalRow = saleSheetEF.RowCount;



                while (saleSheetEF.GetCellValue(totalRow, 1) == null)
                {
                    totalRow--;
                }

                totalRow--;

                return Result;
            }
            catch (Exception ex) { }

            return Result;
        }

        public static uint Get_NextSerialNo(bool _isLocal)
        {
            uint Result = 0;
            try
            {
                string fileName = DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString();
                if (!Directory.Exists(saleSaheet_xlsFolder))
                {
                    return 0;
                }

                if (!File.Exists(saleSaheet_xlsFolder + @"\" + fileName + ".xls"))
                {
                    CreateSaleSheet(fileName, saleSaheet_xlsFolder);
                }

                if (File.Exists(saleSaheet_xlsFolder + @"\" + fileName + ".xls"))
                {
                    saleSheetEF = new XlsFile(saleSaheet_xlsFolder + @"\" + fileName + ".xls");
                }
                else
                {
                    return 0;
                }

                if (_isLocal == false)
                {
                    if (saleSheetEF.GetSheetIndex("TRUCK SALE", false) == -1)
                    {
                        return 0;
                    }
                }
                else if (_isLocal)
                {
                    if (saleSheetEF.GetSheetIndex("LOCAL SALE", false) == -1)
                    {
                        return 0;
                    }
                }

                if (_isLocal)
                    saleSheetEF.ActiveSheet = 1;
                else
                    saleSheetEF.ActiveSheet = 2;

                int totalRow = 0;
                totalRow = saleSheetEF.RowCount;



                while (saleSheetEF.GetCellValue(totalRow, 1) == null)
                {
                    totalRow--;
                }

                uint getSerial = 0;
                if (totalRow > 1)
                {
                    getSerial = uint.Parse(saleSheetEF.GetCellValue(totalRow, 1).ToString());
                }
                else {
                    getSerial = 0;
                }

                Result = getSerial + 1;
            }
            catch (Exception ex) { }

            return Result;
        }

        public static List<SaleSheetOneEntry> Sale_SearchAll(int _Day,int _Month,int _Year,bool _isLocal) {

            List<SaleSheetOneEntry> ReturnSheet = new List<SaleSheetOneEntry>();
            try
            {
                string fileName = _Day.ToString() + "_" + _Month.ToString() + "_" + _Year.ToString();
                if (!Directory.Exists(saleSaheet_xlsFolder))
                {
                    return ReturnSheet;
                }

                if (!File.Exists(saleSaheet_xlsFolder + @"\" + fileName + ".xls"))
                {
                    return ReturnSheet;
                }

                if (File.Exists(saleSaheet_xlsFolder + @"\" + fileName + ".xls"))
                {
                    saleSheetEF = new XlsFile(saleSaheet_xlsFolder + @"\" + fileName + ".xls");
                }
                else
                {
                    return ReturnSheet;
                }

                if (_isLocal == false)
                {
                    if (saleSheetEF.GetSheetIndex("TRUCK SALE", false) == -1)
                    {
                        return ReturnSheet;
                    }
                }
                else if (_isLocal)
                {
                    if (saleSheetEF.GetSheetIndex("LOCAL SALE", false) == -1)
                    {
                        return ReturnSheet;
                    }
                }

                if (_isLocal)
                    saleSheetEF.ActiveSheet = 1;
                else
                    saleSheetEF.ActiveSheet = 2;


                int totalRow = 0;
                totalRow = saleSheetEF.RowCount;



                while (saleSheetEF.GetCellValue(totalRow, 1) == null)
                {
                    totalRow--;
                }

                if (totalRow > 1) {
                    for (int i = 2; i <= totalRow; i++) {
                        ReturnSheet.Add(new SaleSheetOneEntry(saleSheetEF.GetCellValue(i, 1).ToString(), saleSheetEF.GetCellValue(i, 2).ToString(), saleSheetEF.GetCellValue(i, 3).ToString(), saleSheetEF.GetCellValue(i, 6).ToString(), saleSheetEF.GetCellValue(i, 4).ToString(), saleSheetEF.GetCellValue(i, 5).ToString(), saleSheetEF.GetCellValue(i, 7).ToString(), saleSheetEF.GetCellValue(i, 8).ToString(), saleSheetEF.GetCellValue(i, 8).ToString()));
                    }
                
                }

                
            }
            catch (Exception ex) { }

            return ReturnSheet;
        }
        public static List<SaleSheetOneEntry> Sale_SearchLast10(int _Day, int _Month, int _Year, bool _isLocal) { 
            List<SaleSheetOneEntry> ReturnSheet = new List<SaleSheetOneEntry>();
            try
            {
                string fileName = _Day.ToString() + "_" + _Month.ToString() + "_" + _Year.ToString();
                if (!Directory.Exists(saleSaheet_xlsFolder))
                {
                    return ReturnSheet;
                }

                if (!File.Exists(saleSaheet_xlsFolder + @"\" + fileName + ".xls"))
                {
                    return ReturnSheet;
                }

                if (File.Exists(saleSaheet_xlsFolder + @"\" + fileName + ".xls"))
                {
                    saleSheetEF = new XlsFile(saleSaheet_xlsFolder + @"\" + fileName + ".xls");
                }
                else
                {
                    return ReturnSheet;
                }

                if (_isLocal == false)
                {
                    if (saleSheetEF.GetSheetIndex("TRUCK SALE", false) == -1)
                    {
                        return ReturnSheet;
                    }
                }
                else if (_isLocal)
                {
                    if (saleSheetEF.GetSheetIndex("LOCAL SALE", false) == -1)
                    {
                        return ReturnSheet;
                    }
                }

                if (_isLocal)
                    saleSheetEF.ActiveSheet = 1;
                else
                    saleSheetEF.ActiveSheet = 2;


                int totalRow = 0;
                totalRow = saleSheetEF.RowCount;



                while (saleSheetEF.GetCellValue(totalRow, 1) == null)
                {
                    totalRow--;
                }

                if (totalRow >= 11)
                {
                    for (int i = 0; i < 10; i++) {
                        ReturnSheet.Add(new SaleSheetOneEntry(saleSheetEF.GetCellValue(totalRow - i, 1).ToString(), saleSheetEF.GetCellValue(totalRow - i, 2).ToString(), saleSheetEF.GetCellValue(totalRow - i, 3).ToString(), saleSheetEF.GetCellValue(totalRow - i, 6).ToString(), saleSheetEF.GetCellValue(totalRow - i, 4).ToString(), saleSheetEF.GetCellValue(totalRow - i, 5).ToString(), saleSheetEF.GetCellValue(totalRow - i, 7).ToString(), saleSheetEF.GetCellValue(totalRow - i, 8).ToString(), saleSheetEF.GetCellValue(totalRow - i, 8).ToString()));
                    }
                }
                else {
                    for (int i = totalRow; i > 1; i--) {
                        ReturnSheet.Add(new SaleSheetOneEntry(saleSheetEF.GetCellValue(i, 1).ToString(), saleSheetEF.GetCellValue(i, 2).ToString(), saleSheetEF.GetCellValue(i, 3).ToString(), saleSheetEF.GetCellValue(i, 6).ToString(), saleSheetEF.GetCellValue(i, 4).ToString(), saleSheetEF.GetCellValue(i, 5).ToString(), saleSheetEF.GetCellValue(i, 7).ToString(), saleSheetEF.GetCellValue(i, 8).ToString(), saleSheetEF.GetCellValue(i, 8).ToString()));
                    }
                }

            }
            catch (Exception ex) { }
            return ReturnSheet;
        }

        #endregion


        #region Banking

        public static bool BankSheet_isSheetExist(uint _code) {
            bool BankSheet_isSheetExist_Return = false;
            try
            {
                XlsFile temp_xls = new XlsFile(bankSheet_xlsfolder + @"\" + bankSheet_xlsfilename + ".xls");
                if (temp_xls.GetSheetIndex(_code.ToString(), false) != -1)
                {
                    BankSheet_isSheetExist_Return = true;
                }

            }
            catch (Exception ex) { 
            
            }

            return BankSheet_isSheetExist_Return;
        }

        public static bool BankSheet_Credit(float _credit_amount,string _remark,uint _code) {
            bool BankSheet_Credit_Return_Result = false;
            if (_credit_amount < 50) {
                return false;
            }
            try
            {
                if (!Directory.Exists(bankSheet_xlsfolder))
                    Directory.CreateDirectory(bankSheet_xlsfolder);

                if (!File.Exists(bankSheet_xlsfolder + @"\" + bankSheet_xlsfilename + ".xls"))
                    CreateBankSheet(bankSheet_xlsfilename, bankSheet_xlsfolder);



                bankSheet = new XlsFile(bankSheet_xlsfolder + @"\" + bankSheet_xlsfilename + ".xls", true);


                    if (bankSheet.GetSheetIndex(_code.ToString(),false) == -1)
                    {
                        int ls = bankSheet.SheetCount;
                        bankSheet.InsertAndCopySheets(ls, ls, 1);
                        bankSheet.ActiveSheet = ls;
                        bankSheet.SheetName = _code.ToString();
                    }

                    bankSheet.ActiveSheetByName = _code.ToString();
                
                    //Credit Amount

                    int lastRow = bankSheet.RowCount;
                    while (bankSheet.GetCellValue(lastRow, 1) == null)
                    {
                        lastRow--;
                    }

                    float amount = 0;
                    if (lastRow > 1)
                    {
                        amount = float.Parse(bankSheet.GetCellValue(lastRow, 5).ToString());
                        amount += _credit_amount;
                    }
                    else {
                        amount = _credit_amount;
                    }
                    float credit = _credit_amount;
                    string date = DateTime.Now.Date.Day.ToString() + "/" + DateTime.Now.Date.Month.ToString() + "/" + DateTime.Now.Date.Year.ToString();
                    string time = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString();
                    float deduct = 0;

                    bankSheet.SetCellValue(lastRow + 1, 1, date);
                    bankSheet.SetCellValue(lastRow + 1, 2, time);
                    bankSheet.SetCellValue(lastRow + 1, 3, credit);
                    bankSheet.SetCellValue(lastRow + 1, 4, deduct);
                    bankSheet.SetCellValue(lastRow + 1, 5, amount);
                    bankSheet.SetCellValue(lastRow + 1, 6, _remark);

                    bankSheet.Save(bankSheet_xlsfolder + @"\" + bankSheet_xlsfilename + ".xls");
                    BankSheet_Credit_Return_Result = true;
                
            }
            catch (Exception ex) { }

            return BankSheet_Credit_Return_Result;
        }

        public static object[] BankSheet_Deduct(float _deduct_amount, string _remark, uint _code) {
            object[] BankSheet_Credit_Return_Result = new object[] { "Error", false };
            try
            {
                if (!Directory.Exists(bankSheet_xlsfolder)) {
                    BankSheet_Credit_Return_Result[0] = "Balance Zero";
                    return BankSheet_Credit_Return_Result;
                }


                if (!File.Exists(bankSheet_xlsfolder + @"\" + bankSheet_xlsfilename + ".xls"))
                {
                    BankSheet_Credit_Return_Result[0] = "Balance Zero";
                    return BankSheet_Credit_Return_Result;
                }

                if (BankSheet_isSheetExist(_code))
                {

                    bankSheet = new XlsFile(bankSheet_xlsfolder + @"\" + bankSheet_xlsfilename + ".xls", true);

                    bankSheet.ActiveSheetByName = _code.ToString();

                    //Deduct Amount

                    int lastRow = bankSheet.RowCount;
                    while (bankSheet.GetCellValue(lastRow, 1) == null)
                    {
                        lastRow--;
                    }

                    if (lastRow < 2)
                    {
                        BankSheet_Credit_Return_Result[0] = "Total Balance Zero";
                        return BankSheet_Credit_Return_Result;
                    }

                    float amount = float.Parse(bankSheet.GetCellValue(lastRow, 5).ToString());

                    if (amount >= _deduct_amount && _deduct_amount > 0)
                    {
                        amount -= _deduct_amount;
                    }

                    else {
                        BankSheet_Credit_Return_Result[0] = "Balance Not Enough";
                        return BankSheet_Credit_Return_Result;
                    }

                    if (amount < 0)
                    {
                        BankSheet_Credit_Return_Result[0] = "Balance Not Enough";
                        return BankSheet_Credit_Return_Result;
                    }

                    float credit = 0;
                    string date = DateTime.Now.Date.Day.ToString() + "/" + DateTime.Now.Date.Month.ToString() + "/" + DateTime.Now.Date.Year.ToString();
                    string time = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString();
                    float deduct = _deduct_amount;

                    bankSheet.SetCellValue(lastRow + 1, 1, date);
                    bankSheet.SetCellValue(lastRow + 1, 2, time);
                    bankSheet.SetCellValue(lastRow + 1, 3, credit);
                    bankSheet.SetCellValue(lastRow + 1, 4, deduct);
                    bankSheet.SetCellValue(lastRow + 1, 5, amount);
                    bankSheet.SetCellValue(lastRow + 1, 6, _remark);

                    bankSheet.Save(bankSheet_xlsfolder + @"\" + bankSheet_xlsfilename + ".xls");
                    BankSheet_Credit_Return_Result[0] = amount.ToString();
                    BankSheet_Credit_Return_Result[1] = true;
                }
                else {
                    BankSheet_Credit_Return_Result[0] = "Balance Zero";
                }
            }
            catch (Exception ex) { }

            return BankSheet_Credit_Return_Result;
        
        }

        public static object[] BankSheet_GetAmount(uint _code) {
            object[] Return_Result = new object[] {0.0,false};
            try {
                XlsFile temp_File = new XlsFile(bankSheet_xlsfolder + @"\" + bankSheet_xlsfilename + ".xls");

                temp_File.ActiveSheetByName = _code.ToString();

                //Deduct Amount

                int lastRow = temp_File.RowCount;
                while (temp_File.GetCellValue(lastRow, 1) == null)
                {
                    lastRow--;
                }

                if (lastRow < 2)
                {
                    return Return_Result;
                }

                float amount = float.Parse(temp_File.GetCellValue(lastRow, 5).ToString());
                Return_Result[0] = amount;
                Return_Result[1] = true;
            
            }
            catch (Exception ex) { }

            return Return_Result;
        }

        public static List<BankSheetOneEntry> BankSheet_getDetail(uint code, string _date,bool _isDate) {
            List<BankSheetOneEntry> returnEntry = new List<BankSheetOneEntry>();

            try { 
                if (!Directory.Exists(bankSheet_xlsfolder)) {
                    return returnEntry;
                }


                if (!File.Exists(bankSheet_xlsfolder + @"\" + bankSheet_xlsfilename + ".xls"))
                {
                    return returnEntry;
                }

                if (BankSheet_isSheetExist(code))
                {
                    bankSheet = new XlsFile(bankSheet_xlsfolder + @"\" + bankSheet_xlsfilename + ".xls");
                    bankSheet.ActiveSheetByName = code.ToString();

                    int lastRow = bankSheet.RowCount;
                    while (bankSheet.GetCellValue(lastRow, 1) == null)
                    {
                        lastRow--;
                    }

                    if (lastRow > 1) {
                        for (int i = 1; i <= lastRow; i++) {
                            if (_isDate)
                            {
                                if (_date == bankSheet.GetCellValue(i, 1).ToString())
                                {
                                    returnEntry.Add(new BankSheetOneEntry(code.ToString(), ProcessData.GetName(code), bankSheet.GetCellValue(i, 3).ToString(), bankSheet.GetCellValue(i, 4).ToString(), bankSheet.GetCellValue(i, 5).ToString(), bankSheet.GetCellValue(i, 1).ToString(), bankSheet.GetCellValue(i, 2).ToString(), bankSheet.GetCellValue(i, 6).ToString()));
                                }
                            }
                            else
                            {
                                returnEntry.Add(new BankSheetOneEntry(code.ToString(), ProcessData.GetName(code), bankSheet.GetCellValue(i, 3).ToString(), bankSheet.GetCellValue(i, 4).ToString(), bankSheet.GetCellValue(i, 5).ToString(), bankSheet.GetCellValue(i, 1).ToString(), bankSheet.GetCellValue(i, 2).ToString(), bankSheet.GetCellValue(i, 6).ToString()));
                            }
                        }
                    }
                
                }
            
            }
            catch (Exception ex) { }

            return returnEntry;
        }

        public static bool Bank_DeleteAccount(uint code) {
            bool Return_Result = false;
            try
            {
                XlsFile temp_File = new XlsFile(bankSheet_xlsfolder + @"\" + bankSheet_xlsfilename + ".xls",true);

                temp_File.ActiveSheetByName = code.ToString();
                temp_File.DeleteSheet(1);
                temp_File.Save(bankSheet_xlsfolder + @"\" + bankSheet_xlsfilename + ".xls");

            }
            catch (Exception ex) { }

            return Return_Result;
        }

        #endregion




        public static void CreateBankSheet(string _fileName, string _folderName) {
            try
            {
                bankSheet = new XlsFile();
                bankSheet.NewFile(1);

                bankSheet.ActiveSheet = 1;
                bankSheet.SheetName = "Template";

                /*
                 * 1.Date
                 * 2.Time
                 * 3.Credit
                 * 4.Deduct
                 * 5.Amount
                 * 6.Remark
                 */

                bankSheet.SetCellValue(1, 1, "Date");
                bankSheet.SetCellValue(1, 2, "Time");
                bankSheet.SetCellValue(1, 3, "Credit");
                bankSheet.SetCellValue(1, 4, "Deduct");
                bankSheet.SetCellValue(1, 5, "Total Amount");
                bankSheet.SetCellValue(1, 6, "Remark");

                bankSheet.Save(_folderName + @"\" + _fileName + ".xls");
            }
            catch (Exception ex) { 
            
            }
        }

        public static void CreateSaleSheet(string _fileName, string _folderName) {
            try
            {
                saleSheetEF = new XlsFile();
                saleSheetEF.NewFile(2);

                saleSheetEF.ActiveSheet = 1;
                saleSheetEF.SheetName = "LOCAL SALE";

                /*
                 * 1.Date
                 * 2.Time
                 * 3.Quantity
                 * 4.Amount
                 * 5.Rate
                 * 6.Category
                 * 7.Remark
                 */
                saleSheetEF.SetCellValue(1, 1, "Sr. No.");
                saleSheetEF.SetCellValue(1, 2, "Date");
                saleSheetEF.SetCellValue(1, 3, "Time");
                saleSheetEF.SetCellValue(1, 4, "Quantity");
                saleSheetEF.SetCellValue(1, 5, "Amount");
                saleSheetEF.SetCellValue(1, 6, "Rate");
                saleSheetEF.SetCellValue(1, 7, "Category");
                saleSheetEF.SetCellValue(1, 8, "Remark");


                saleSheetEF.ActiveSheet = 2;
                saleSheetEF.SheetName = "TRUCK SALE";

                /*
                 * 1.Date
                 * 2.Time
                 * 3.Quantity
                 * 4.Amount
                 * 5.Rate
                 * 6.Category
                 * 7.TRUCK NO
                 */

                saleSheetEF.SetCellValue(1, 1, "Sr. No.");
                saleSheetEF.SetCellValue(1, 2, "Date");
                saleSheetEF.SetCellValue(1, 3, "Time");
                saleSheetEF.SetCellValue(1, 4, "Quantity");
                saleSheetEF.SetCellValue(1, 5, "Amount");
                saleSheetEF.SetCellValue(1, 6, "Rate");
                saleSheetEF.SetCellValue(1, 7, "Category");
                saleSheetEF.SetCellValue(1, 8, "TRUCK NO");

                
                saleSheetEF.Save(_folderName + @"\" + _fileName +".xls");


            }
            catch (Exception ex) { }
        }

        public static void CreateNewExcel(string _fileName, string  _folderName) {

            /*
                    * 1.ID
                    * 2.NAME
                    * 3.PHONE
                    * 4.MILK WEIGHT
                    * 5.FAT
                    * 6.SNF
                    * 7.CLR
                    * 8.RATE
                    * 9.CAT
                    * 10.LACTOSE
                    * 11.PROTEIN
                    * 12.SOLID
                    * 13.FRPOINT
                    * 14.TEMPRATURE
                    * 15.DENSITY
                    * 16.ADDED WATER
                    * 17.UPDATE TIME
                    * 18.Auto/Man
                    */

            collectionEF = new XlsFile();
            collectionEF.NewFile(1);

            //Change Sheet 1
            collectionEF.ActiveSheet = 1;
            collectionEF.SetCellValue(1, 1, "ID");
            collectionEF.SetCellValue(1, 2, "NAME");
            collectionEF.SetCellValue(1, 3, "PHONE");
            collectionEF.SetCellValue(1, 4, "MILK WEIGHT");
            collectionEF.SetCellValue(1, 5, "FAT");
            collectionEF.SetCellValue(1, 6, "SNF");
            collectionEF.SetCellValue(1, 7, "CLR");
            collectionEF.SetCellValue(1, 8, "AMOUNT");
            collectionEF.SetCellValue(1, 9, "CATEGORY");
            collectionEF.SetCellValue(1, 10, "LACTOSE");
            collectionEF.SetCellValue(1, 11, "PROTEIN");
            collectionEF.SetCellValue(1, 12, "SOLID");
            collectionEF.SetCellValue(1, 13, "FR POINT");
            collectionEF.SetCellValue(1, 14, "TEMPRATURE");
            collectionEF.SetCellValue(1, 15, "DENSITY");
            collectionEF.SetCellValue(1, 16, "ADDED WATER");
            collectionEF.SetCellValue(1, 17, "MODIFIED DATE");
            collectionEF.SetCellValue(1, 18, "AUTO/MAN");
            collectionEF.SetCellValue(1, 19, "Cash");
            collectionEF.SetCellValue(1, 20, "Bank");
            
            collectionEF.SetCellFormat(1, 1, 1, 8, 0);
            collectionEF.SheetName = "0";

            collectionEF.Save(_folderName + @"/" + _fileName + ".xls");
        }
    
    }

    public class singleMemberLedgerRecord {
        public uint date;
        public uint month;
        public uint shift;
        public string fat;
        public string snf;
        public string quantity;
        public string amount;

        public singleMemberLedgerRecord(uint _Date, uint _month, uint _shift, string _fat, string _snf, string _quantity, string _amount) {
            date = _Date;
            month = _month;
            shift = _shift;

            fat = _fat;
            snf = _snf;
            quantity = _quantity;
            amount = _amount;
        }
    }

    public class allMemberLedgerRecord
    {
        public uint id;
        public string Name;
        private uint date;
        private uint month;
        private uint shift;
        private string fat;
        private string snf;
        public string quantity;
        public string amount;

        public allMemberLedgerRecord(uint _id, string _Name,string _quantity, string _amount)
        {
            id = _id;
            Name = _Name;
            quantity = _quantity;
            amount = _amount;
        }
    }

    public class shiftReport
    {
        public float averageShiftCowFAT;
        public float averageShiftBuffFAT;

        public float averageShiftCowSNF;
        public float averageShiftBuffSNF;

        public float totalAmountCow;
        public float totalAmountBuff;

        public float totalLitreCow;
        public float totalLitreBuff;

        public float totalLitre;
        public float totalAmount;
        public float averageFat;
        public float averageSnf;

        public bool isCowListEmpty;
        public bool isBuffListEmpty;

        public void BuffEntry(float _averageShiftBuffFAT, float _averageShiftBuffSNF, float _totalAmountBuff, float _totalLitreBuff) {
            
            averageShiftBuffFAT = _averageShiftBuffFAT;

           
            averageShiftBuffSNF = _averageShiftBuffSNF;

            totalAmountBuff = _totalAmountBuff;
           

            totalLitreBuff = _totalLitreBuff;
           

            isBuffListEmpty = false;
            
        }

        public void CowEntry(float _averageShiftCowFAT, float _averageShiftCowSNF, float _totalAmountCow, float _totalLitreCow) {
            averageShiftCowFAT = _averageShiftCowFAT;

            averageShiftCowSNF = _averageShiftCowSNF;

            totalAmountCow = _totalAmountCow;

            totalLitreCow = _totalLitreCow;

            isCowListEmpty = false;
        }

        

        public shiftReport(float _averageShiftCowFAT, float _averageShiftBuffFAT, float _averageShiftCowSNF, float _month, float _averageShiftBuffSNF, float _totalAmountCow, float _totalAmountBuff, float _totalLitreCow, float _totalLitreBuff, bool _isMCowEmpty, bool _isBuffEmpty)
        {
            averageShiftCowFAT = _averageShiftCowFAT;
            averageShiftBuffFAT = _averageShiftBuffFAT;

            averageShiftCowSNF = _averageShiftCowSNF;
            averageShiftBuffSNF = _averageShiftBuffSNF;

            totalAmountBuff = _totalAmountBuff;
            totalAmountCow = _totalAmountCow;

            totalLitreBuff = _totalLitreBuff;
            totalLitreCow = _totalLitreCow;

            totalAmount = totalAmountBuff + totalAmountCow;
            totalLitre = totalLitreBuff + totalLitreCow;
            averageFat = (averageShiftCowFAT + averageShiftBuffFAT) / 2;
            averageSnf = (averageShiftBuffSNF + averageShiftCowSNF) / 2;

            isBuffListEmpty = _isBuffEmpty;
            isCowListEmpty = _isMCowEmpty;
        }

        public void CalculateTotal() {

            totalAmount = totalAmountBuff + totalAmountCow;
            totalLitre = totalLitreBuff + totalLitreCow;
            averageFat = (averageShiftCowFAT + averageShiftBuffFAT) / 2;
            averageSnf = (averageShiftBuffSNF + averageShiftCowSNF) / 2;
        }

        public shiftReport() {
            averageShiftBuffFAT = 0;
            averageShiftBuffSNF = 0;
            averageShiftCowFAT = 0;
            averageShiftCowSNF = 0;

            totalAmountBuff = 0;
            totalAmountCow = 0;
            totalLitreBuff = 0;
            totalLitreCow = 0;

            totalAmount = 0;
            totalLitre = 0;
            averageFat = 0;
            averageSnf = 0;

            isCowListEmpty = true;
            isBuffListEmpty = true;
        }
    }

    public class BankSheetOneEntry {
        public string code;
        public string name;
        public string credit;
        public string deduct;
        public string balance;
        public string date;
        public string time;
        public string Remark;

        public BankSheetOneEntry(string _code, string _name, string _credit, string _deduct, string _balance, string _date, string _time,string _remaRK) {
            code = _code;
            name = _name;
            credit = _credit;
            deduct = _deduct;
            balance = _balance;
            date = _date;
            time = _time;
            Remark = _remaRK;

        }
    
    }

    public class SaleSheetOneEntry {
        public string Serial;
        public string date;
        public string time;
        public string rate;
        public string quantity;
        public string amount;
        public string cat;
        public string remark;
        public string truckNo;

        public SaleSheetOneEntry(string _serial,string _date, string _time, string _rate, string _quantity, string _amount, string _cat, string _remark, string _truckNo) {
            Serial = _serial;
            date = _date;
            time = _time;
            rate = _rate;
            quantity = _quantity;
            amount = _amount;
            cat = _cat;
            remark = _remark;
            truckNo = _truckNo;
        }
    
    }

    public class ErrorMessage
    {

        bool isSucess;
        string Message;

        public ErrorMessage(bool _isSucess, string _Message) {
            isSucess = _isSucess;
            Message = _Message;
        }
    }

}
