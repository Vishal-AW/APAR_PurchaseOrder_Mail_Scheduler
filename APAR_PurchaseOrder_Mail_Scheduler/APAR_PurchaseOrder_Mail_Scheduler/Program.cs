﻿using APAR_PurchaseOrder_Mail_Scheduler.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
///using APAR_ManpowerRequisition_Mail_Schedular.Models;

namespace APAR_PurchaseOrder_Mail_Scheduler
{
    class Program
    {
        static void Main()
        {
            //string filename = "log\\Log.txt";
            //CustomSharePointUtility.logFile = new StreamWriter(filename);
            //CustomSharePointUtility.WriteLog("*********************************************");
            //CustomSharePointUtility.WriteLog("Reminder Mail Starts: " + DateTime.Now.ToString());
            //CustomSharePointUtility.WriteLog("*********************************************");
            //Console.WriteLine("*********************************************");
            //Console.WriteLine("Reminder Mail starts : " + DateTime.Now.ToString());
            //Console.WriteLine("*********************************************");
            List<PurchaseOrder> SPPurchaseOrder = null;
            try
            {
                var siteUrl = ConfigurationManager.AppSettings["SP_Address_Live"];
                var RootsiteUrl = ConfigurationManager.AppSettings["SP_Address_RootLive"];
                string EmployeeMaster = ConfigurationManager.AppSettings["EmployeeMaster"];
                string TestingPurchaseHeaderList = ConfigurationManager.AppSettings["TestingPurchaseHeaderList"];
                string PurchaseApprovers = ConfigurationManager.AppSettings["PurchaseApprovers"];
                string EmailList = ConfigurationManager.AppSettings["EmailList"];
                string DaysDifference = ConfigurationManager.AppSettings["DaysDifference"];
                //string query = SQLUtility.ReadQuery("EmployeeMasterQuery.txt");
                SPPurchaseOrder = new List<PurchaseOrder>();
                //Task task_SPEmployeeMaster = Task.Run(() => SPTravelVoucher = CustomSharePointUtility.GetAll_TravelVoucherFromSharePoint(siteUrl, TestingTravelHeaderList));
                SPPurchaseOrder = CustomSharePointUtility.GetAll_PurchaseOrderFromSharePoint(siteUrl, TestingPurchaseHeaderList, DaysDifference);
                //List<TravelVoucher> empMasterFinal = new List<TravelVoucher>();
                List<PurchaseOrder> purchaseDataFinal = SPPurchaseOrder;

                Console.WriteLine("total count" + purchaseDataFinal.Count);
                if (purchaseDataFinal.Count > 0)
                {
                    for (var i = 0; i < purchaseDataFinal.Count; i++)
                    {
                        if (purchaseDataFinal[i].LocationType == "Office")
                        {
                            if (purchaseDataFinal[i].ApprovalStatus == "Submitted")
                            {
                                CustomSharePointUtility.EmployeeMasterData(purchaseDataFinal[i], RootsiteUrl, siteUrl, EmployeeMaster, EmailList);
                            }
                            else if (purchaseDataFinal[i].ApprovalStatus == "Approved By Functional Head")
                            {
                                CustomSharePointUtility.PurchaseApproversData(purchaseDataFinal[i], RootsiteUrl, siteUrl, PurchaseApprovers, EmailList);
                            }
                        }
                        else if (purchaseDataFinal[i].LocationType == "Plant")
                        {
                            //if (purchaseDataFinal[i].ApprovalStatus == "Submitted")
                            //{
                            //    CustomSharePointUtility.PurchaseApproversData(purchaseDataFinal[i], RootsiteUrl, siteUrl, PurchaseApprovers, EmailList);
                            //}
                            //else if (purchaseDataFinal[i].ApprovalStatus == "Approved By Purchase Head")
                            //{
                            //    CustomSharePointUtility.PurchaseApproversData(purchaseDataFinal[i], RootsiteUrl, siteUrl, PurchaseApprovers, EmailList);
                            //}
                            //else if (purchaseDataFinal[i].ApprovalStatus == "Approved By Plant Head")
                            //{
                            //    CustomSharePointUtility.PurchaseApproversData(purchaseDataFinal[i], RootsiteUrl, siteUrl, PurchaseApprovers, EmailList);
                            //}
                            if (purchaseDataFinal[i].NewFlow == "Yes")
                            { 
                                CustomSharePointUtility.PurchaseApproversDataPlant(purchaseDataFinal[i], RootsiteUrl, siteUrl, PurchaseApprovers, EmailList);
                            }
                        }
                        Console.WriteLine("No Pending Records." + i);

                    }

                   
                    //Console.WriteLine("Employee data synchronized successfully.");
                    //var success = CustomSharePointUtility.EmailData(empMasterFinal, siteUrl, EmailList);
                    //if (success)
                    //{
                    //    ///CustomSharePointUtility.WriteLog("Reminder Mail Sent Successfully.");
                    //    //Console.WriteLine("Reminder Mail Sent Successfully.");
                    //}
                }
                else
                {
                    //CustomSharePointUtility.WriteLog("No Pending Records.");
                    //Console.WriteLine("No Pending Records.");
                }
            }
            catch (Exception ex)
            {
                CustomSharePointUtility.WriteLog("Error in scheduler : " + ex.StackTrace);
                Console.WriteLine("Error in scheduler : " + ex.StackTrace);
            }
            finally
            {
                //CustomSharePointUtility.WriteLog("*********************************************");
                //CustomSharePointUtility.WriteLog("Reminder Mail ends : " + DateTime.Now.ToString());
                //CustomSharePointUtility.WriteLog("*********************************************");
                //Console.WriteLine("*********************************************");
                // Console.WriteLine("Reminder Mail ends : " + DateTime.Now.ToString());
                //Console.WriteLine("*********************************************");
                //CustomSharePointUtility.logFile.Close();
                //Console.ReadKey();

            }
        }
    }
}
