using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace LCD16x2
{
    public static class Printer
    {
        //Printer Device ID is 1
        public static void InitPrinter() {
           
        }

        public static void printSaleSheet(uint _Date, uint _Month, uint _Year,bool _isLocal)
        {
            try
            {
                List<SaleSheetOneEntry> AllEntry = ExcelFileHandler.Sale_SearchAll(int.Parse(_Date.ToString()), int.Parse(_Month.ToString()), int.Parse(_Year.ToString()),_isLocal);

                string local = "";

                if (_isLocal)
                    local = "Local";
                else
                    local = "TRUCK";

                SerialP.sendDirect(1, local +" SALE REPORT");
                SerialP.sendDirect(1, ("DT:" + _Date.ToString() + "/" + _Month.ToString() + "/" + _Year.ToString()));
                if(!_isLocal)
                    SerialP.sendDirect(1, "Sr. Time  Ltr  Amount CAT  Tr.");
                else
                    SerialP.sendDirect(1, "Sr. Time  Ltr     Amount   CAT  ");
                SerialP.sendDirect(1,     "------------------------------");
                
                if (AllEntry.Count > 0)
                {
                    if (!_isLocal)
                    {
                        foreach (SaleSheetOneEntry SH in AllEntry)
                        {

                            SerialP.sendDirect(1, (SH.Serial + " " + SH.time + "  " + SH.quantity + "  " + SH.amount + " " + SH.cat + " " + SH.truckNo));
                        }
                    }
                    else
                    {
                        foreach (SaleSheetOneEntry SH in AllEntry)
                        {
                            SerialP.sendDirect(1, (SH.Serial + "  " + SH.time + "  " + SH.quantity + "  " + SH.amount + "  " + SH.cat));
                        }
                    }

                }
                else
                {
                    SerialP.sendDirect(1, "Empty List");
                }
                SerialP.sendDirect(1, "------------------------------");
                SerialP.sendDirect(1, "**Your Company**");
                SerialP.sendDirect(1, "  ");
                SerialP.sendDirect(1, "  ");
                SerialP.sendDirect(1, "  ");


            }
            catch (Exception ex) {
                SerialP.sendDirect(1, "Try Again : Error Occured");
                SerialP.sendDirect(1, "");
                SerialP.sendDirect(1, "");
            }
        }

        public static void printBankAll(uint _code) {
            try {
                List<BankSheetOneEntry> tempCol = ExcelFileHandler.BankSheet_getDetail(_code, "", false);
                SerialP.sendDirect(1, "Payment Report All");
                SerialP.sendDirect(1, "CODE : " + _code.ToString() + " | " + ProcessData.GetName(_code));
                SerialP.sendDirect(1, "Date  Time  Bal.  Credit  Ded.");
                SerialP.sendDirect(1, "------------------------------");
                if (tempCol.Count > 0)
                {
                    foreach (BankSheetOneEntry b in tempCol) {
                        SerialP.sendDirect(1, b.date + " " + b.time + " "+ b.balance+ " "+ b.credit + " " + b.deduct);
                    }
                }
                else {
                    SerialP.sendDirect(1, "No Entry Found");
                }

                SerialP.sendDirect(1, "------------------------------");
                SerialP.sendDirect(1, "Your Company");
                SerialP.sendDirect(1, "");
                SerialP.sendDirect(1, "");
                
            }
            catch (Exception ex) { }
        
        }

        public static void printBankDate(uint _code, string _date) {
            try
            {
                List<BankSheetOneEntry> tempCol = ExcelFileHandler.BankSheet_getDetail(_code, _date, true);
                SerialP.sendDirect(1, "Payment Report by Date");
                SerialP.sendDirect(1, "CODE : " + _code.ToString() + " | " + ProcessData.GetName(_code));
                SerialP.sendDirect(1, "DATE : " + _date );
                SerialP.sendDirect(1, "Date  Time  Bal.  Credit  Ded.");
                SerialP.sendDirect(1, "------------------------------");
                if (tempCol.Count > 0)
                {
                    foreach (BankSheetOneEntry b in tempCol)
                    {
                        SerialP.sendDirect(1, b.date + " " + b.time + " " + b.balance + " " + b.credit + " " + b.deduct);
                    }
                }
                else
                {
                    SerialP.sendDirect(1, "No Entry Found");
                }

                SerialP.sendDirect(1, "------------------------------");
                SerialP.sendDirect(1, "Your Company");
                SerialP.sendDirect(1, "");
                SerialP.sendDirect(1, "");

            }
            catch (Exception ex) { }
        }

        public static void printSingleLedgerList(uint _id,uint _id2, uint fromDate, uint fromMonth, uint fromYear, uint fromShift, uint toDate, uint toMonth, uint toYear, uint toShift)
        {
            try
            {
               
                    SerialP.sendDirect(1, "Member Ledger");
                    SerialP.sendDirect(1, ("DT:" + fromDate.ToString() + "/" + fromMonth.ToString() + "/" + fromYear.ToString() + "(" + LCDFrameProcessor.Shift[fromShift].ToString() + ") To DT:" + toDate.ToString() + "/" + toMonth.ToString() + "/" + toYear.ToString() + "(" + LCDFrameProcessor.Shift[toShift].ToString() + ")"));
                    SerialP.sendDirect(1, "Fr. CODE: " + _id.ToString()+ " To. CODE:"+_id2.ToString());
                    SerialP.sendDirect(1, "Date   FAT   SNF  Ltr  Amount");
                    SerialP.sendDirect(1, "-----------------------------");
                    if (ExcelFileHandler.singleMemberLedgerList.Count > 0)
                    {
                        foreach (singleMemberLedgerRecord rc in ExcelFileHandler.singleMemberLedgerList)
                        {
                            
                            SerialP.sendDirect(1, (rc.date.ToString() + "/" + rc.month.ToString() + LCDFrameProcessor.Shift[rc.shift].ToString() + "  " + rc.fat + "  " + rc.snf + "  " + rc.quantity + "  " + rc.amount));
                        }
                    }
                    else {
                        SerialP.sendDirect(1, "Empty List");
                    }
                    SerialP.sendData(1, "");


                    SerialP.sendDirect(1, "-----------------------------");
                    SerialP.sendDirect(1, "**Your Company**");
                    SerialP.sendDirect(1, "  ");
                    SerialP.sendDirect(1, "  ");
                    SerialP.sendDirect(1, "  ");
                
                

            }
            catch (Exception ex) { }
        }

        public static void printAllLedgerList(string fromCode,string toCode,uint fromDate, uint fromMonth, uint fromYear, uint fromShift, uint toDate, uint toMonth, uint toYear, uint toShift)
        {
            try
            {

                SerialP.sendDirect(1, "All Member Ledger");
                SerialP.sendDirect(1, ("DT:" + fromDate.ToString() + "/" + fromMonth.ToString() + "/" + fromYear.ToString() + "(" + LCDFrameProcessor.Shift[fromShift].ToString() + ") To DT:" + toDate.ToString() + "/" + toMonth.ToString() + "/" + toYear.ToString() + "(" + LCDFrameProcessor.Shift[toShift].ToString() + ")"));
                SerialP.sendDirect(1, "Fr. CODE: " + fromCode + " To. CODE:" + toCode);
                SerialP.sendDirect(1, "Code Name    Ltr   Amount  Sig");
                SerialP.sendDirect(1, "------------------------------");
                if (ExcelFileHandler.allMemberLedgerList.Count > 0)
                {
                    foreach (allMemberLedgerRecord rc in ExcelFileHandler.allMemberLedgerList)
                    {

                        SerialP.sendDirect(1, (rc.id.ToString() + "  " + rc.Name + "   " + rc.quantity + "   " + rc.amount));
                    }
                    SerialP.sendData(1, "");

                }
                else
                {
                    SerialP.sendDirect(1, "Empty List");
                }
                SerialP.sendDirect(1, "------------------------------");
                SerialP.sendDirect(1, "**Your Company**");
                SerialP.sendDirect(1, "  ");
                SerialP.sendDirect(1, "  ");
                SerialP.sendDirect(1, "  ");


            }
            catch (Exception ex) { }
        }

        public static void printShiftReport(uint _date, uint _month, uint _year, uint _shift) {
            try
            {
                SerialP.sendDirect(1, "Shift Report");
                SerialP.sendDirect(1, ("DT:" + _date.ToString() + "/" + _month.ToString() + "/" + _year.ToString() + "(" + LCDFrameProcessor.Shift[_shift].ToString() + ")"));
                SerialP.sendDirect(1, "--------------------------------");
                
                if (!ExcelFileHandler.shiftReportData.isCowListEmpty || !ExcelFileHandler.shiftReportData.isBuffListEmpty)
                {
                    SerialP.sendDirect(1, "          Cow Entries        ");
                    SerialP.sendDirect(1, "--------------------------------");
                    if (ExcelFileHandler.shiftReportListCow.Count > 0)
                    {
                        SerialP.sendDirect(1, "Code  Name  FAT  SNF Ltr. Amount");
                        SerialP.sendDirect(1, "---------------------------------");
                        foreach (ExcelDetailOneMember rc in ExcelFileHandler.shiftReportListCow)
                        {

                            SerialP.sendDirect(1, (rc.ID.ToString() + " " + rc.Name + " " + rc.Fat + "  " + rc.Snf + " " + rc.MilkWeight + "  " + rc.Rate));
                        }
                    }
                    else {

                        SerialP.sendDirect(1, "   No Entry Found for Cow     ");
                    }

                    SerialP.sendDirect(1, " ");
                    SerialP.sendDirect(1, "----------------------------------");
                    SerialP.sendDirect(1, "       Buffalo Entries        ");
                    SerialP.sendDirect(1, "----------------------------------");
                    if (ExcelFileHandler.shiftReportListBuff.Count > 0)
                    {
                        SerialP.sendDirect(1, "Code  Name  FAT  SNF Ltr. Amount");
                        SerialP.sendDirect(1, "----------------------------------");
                        foreach (ExcelDetailOneMember rc in ExcelFileHandler.shiftReportListBuff)
                        {

                            SerialP.sendDirect(1, (rc.ID.ToString() + " " + rc.Name + " " + rc.Fat + "  " + rc.Snf + " " + rc.MilkWeight + "  " + rc.Rate));
                        }
                    }
                    else
                    {

                        SerialP.sendDirect(1, " No Entry Found for Buffalo  ");
                    }

                    SerialP.sendDirect(1, "-----------------------------");
                    SerialP.sendDirect(1, "          Summary         ");
                    SerialP.sendDirect(1, "-----------------------------");
                    SerialP.sendDirect(1, "Buffalo Avg. FAT "+ExcelFileHandler.shiftReportData.averageShiftBuffFAT.ToString()+" Avg. SNF "+ ExcelFileHandler.shiftReportData.averageShiftBuffSNF.ToString()+" Tot. Milk " + ExcelFileHandler.shiftReportData.totalLitreBuff.ToString() + " Ltr. Tot. Amount "+ExcelFileHandler.shiftReportData.totalAmountBuff.ToString());
                    SerialP.sendDirect(1, "COW Avg. FAT " + ExcelFileHandler.shiftReportData.averageShiftCowFAT.ToString() + " Avg. SNF " + ExcelFileHandler.shiftReportData.averageShiftCowSNF.ToString() + " Tot. Milk " + ExcelFileHandler.shiftReportData.totalLitreCow.ToString() + " Ltr. Tot. Amount " + ExcelFileHandler.shiftReportData.totalAmountCow.ToString());
                    SerialP.sendDirect(1, "-----------------------------");
                    SerialP.sendDirect(1, "Avg. FAT " + ExcelFileHandler.shiftReportData.averageFat.ToString() + " Avg. SNF " + ExcelFileHandler.shiftReportData.averageSnf.ToString() + " Tot. Milk " + ExcelFileHandler.shiftReportData.totalLitre.ToString() + " Ltr. Tot. Amount " + ExcelFileHandler.shiftReportData.totalAmount.ToString());
                }
                else {
                    SerialP.sendData(1, "Empty Shift");
                }

                SerialP.sendDirect(1, "-----------------------------");
                SerialP.sendDirect(1, "**Your Company**");
                SerialP.sendDirect(1, "  ");
                SerialP.sendDirect(1, "  ");
                SerialP.sendDirect(1, "  ");

            }
            catch (Exception ex)
            {


            }
        }

        public static void printCollection_ID(string _id,string _date, string _month, string _year, string _shift,bool isDuplicate) {
            List<ExcelDetailOneMember> printOneMember = new List<ExcelDetailOneMember>();
            

            try
            {
                printOneMember = ExcelFileHandler.GetDetail_ID(_id, _date, _month, _year, _shift);

                if (printOneMember != null && printOneMember.Count >= 1)
                {
                    //Print

                }
                if (isDuplicate)
                {
                    SerialP.sendDirect(1, "Duplicate Slip");
                }
                else
                {
                    SerialP.sendDirect(1, "Collection Detail");
                }
                    SerialP.sendDirect(1, ("CODE: " + _id.ToString()+"   DT:" + _date + "/" + _month + "/" + _year + "(" + _shift + ")"));
                    SerialP.sendDirect(1, "-----------------------------");
                    foreach (ExcelDetailOneMember rc in printOneMember)
                    {

                        SerialP.sendDirect(1, "Name: " + rc.Name);
                        SerialP.sendDirect(1, "Milk Weight: " + rc.MilkWeight);
                        SerialP.sendDirect(1, "Amount: " + rc.Rate);
                        SerialP.sendDirect(1, "FAT: " + rc.Fat);
                        SerialP.sendDirect(1, "SNF: " + rc.Snf);
                        SerialP.sendDirect(1, "Entry: " + rc.AutoMan);

                    }
                    SerialP.sendData(1, "");


                    SerialP.sendDirect(1, "-----------------------------");
                    SerialP.sendDirect(1, "**Your Company**");
                    SerialP.sendDirect(1, "  ");
                    SerialP.sendDirect(1, "  ");
                    SerialP.sendDirect(1, "  ");

            }
            catch (Exception ex) { }
        }

        public static void printAllMember()
        {
            try
            {
                if (ProcessData.AllMemberPrint_List.Count > 0)
                {
                    SerialP.sendDirect(1, "All Member List");
                    
                    SerialP.sendDirect(1, "Code  Name      Phone");
                    SerialP.sendDirect(1, "----------------------------------");
                    foreach (EachMemberDetail rc in ProcessData.AllMemberPrint_List)
                    {

                        SerialP.sendDirect(1, (rc.ID.ToString() + " " + rc.Name + " " + rc.Ph));
                    }
                    SerialP.sendData(1, "");


                    SerialP.sendDirect(1, "----------------------------------");
                    SerialP.sendDirect(1, "**Your Company**");
                    SerialP.sendDirect(1, "  ");
                    SerialP.sendDirect(1, "  ");
                    SerialP.sendDirect(1, "  ");
                }

            }
            catch (Exception ex)
            {


            }
        }
    }
}
