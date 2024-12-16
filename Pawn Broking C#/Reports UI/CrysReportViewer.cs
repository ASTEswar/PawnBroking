using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Pawn_Broking.UI
{
    public partial class CrysReportViewer : Form
    {
        public CrysReportViewer()
        {
            InitializeComponent();
        }
        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }

        #region  Loan Register
        public void LoadLoanRegisterReport(DateTime startDate, DateTime endDate, string loanNo = "")
        {
            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.Refresh();

            ReportDocument reportDocument = new ReportDocument();
            string reportPath = "G:\\Pawn Broking C#\\Pawn Broking\\Report\\rptLoanRegister.rpt";

            if (System.IO.File.Exists(reportPath))
            {
                try
                {
                    reportDocument.Load(reportPath);

                    reportDocument.SetDatabaseLogon("sa", "1234", "DESKTOP-6JKB17U\\SQL", "PawnBrokingNew");

                    bool loanParamExists = false;
                    ParameterFieldDefinitions parameterFieldDefinitions = reportDocument.DataDefinition.ParameterFields;

                    foreach (ParameterFieldDefinition paramField in parameterFieldDefinitions)
                    {
                        if (paramField.Name == "LoanNoParam")
                        {
                            loanParamExists = true;
                            break;
                        }
                    }

                    if (loanParamExists && !string.IsNullOrEmpty(loanNo))
                    {
                        reportDocument.SetParameterValue("LoanNoParam", loanNo);
                    }
                    else
                    {
                        foreach (ParameterFieldDefinition paramField in parameterFieldDefinitions)
                        {
                            if (paramField.Name == "StartDate" & paramField.Name == "EndDate")
                            {
                                loanParamExists = true;
                                break;
                            }
                        }
                        if (reportDocument.DataDefinition.ParameterFields["StartDate"] != null &&
                            reportDocument.DataDefinition.ParameterFields["EndDate"] != null)
                        {
                            reportDocument.SetParameterValue("StartDate", startDate);
                            reportDocument.SetParameterValue("EndDate", endDate);
                        }
                        else
                        {
                            MessageBox.Show("Date parameters 'StartDate' or 'EndDate' not found in the report.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    crystalReportViewer1.ReportSource = reportDocument;
                    crystalReportViewer1.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"Report file not found: {reportPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Loan Action Report
        public void LoadAddressReport(DateTime fromDate, DateTime toDate) //Address Button
        {
            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.Refresh();

            ReportDocument reportDocument = new ReportDocument();
            string reportPath = "G:\\Pawn Broking C#\\Pawn Broking\\Report\\rptAddressnew.rpt";

            if (System.IO.File.Exists(reportPath))
            {
                try
                {
                    reportDocument.Load(reportPath);

                    reportDocument.SetDatabaseLogon("sa", "1234", "DESKTOP-6JKB17U\\SQL", "PawnBrokingNew");

                    bool fromDateParamExists = false;
                    bool toDateParamExists = false;

                    ParameterFieldDefinitions parameterFieldDefinitions = reportDocument.DataDefinition.ParameterFields;

                    foreach (ParameterFieldDefinition paramField in parameterFieldDefinitions)
                    {
                        if (paramField.Name == "FromDate")
                        {
                            fromDateParamExists = true;
                        }
                        if (paramField.Name == "ToDate")
                        {
                            toDateParamExists = true;
                        }
                    }

                    if (fromDateParamExists && toDateParamExists)
                    {
                        reportDocument.SetParameterValue("FromDate", fromDate);
                        reportDocument.SetParameterValue("ToDate", toDate);
                    }
                    else
                    {
                        MessageBox.Show("Date parameters 'FromDate' or 'ToDate' not found in the report.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    crystalReportViewer1.ReportSource = reportDocument;
                    crystalReportViewer1.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"Report file not found: {reportPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void LoadPendingLoanReport(DateTime fromDate, DateTime toDate, string loanNo = "") 
           
            //Action Details Button and Pending Details Button
        {
            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.Refresh();

            ReportDocument reportDocument = new ReportDocument();
            string reportPath = string.IsNullOrEmpty(loanNo)
                                ? "G:\\Pawn Broking C#\\Pawn Broking\\Report\\RptLoanPendingRegister.rpt"  // Pending Loan Report path
                                : "G:\\Pawn Broking C#\\Pawn Broking\\Report\\RptLoanHistory.rpt";          // Loan History Report path

            if (System.IO.File.Exists(reportPath))
            {
                try
                {
                    reportDocument.Load(reportPath);

                    reportDocument.SetDatabaseLogon("sa", "1234", "DESKTOP-6JKB17U\\SQL", "PawnBrokingNew");

                    bool loanNoParamExists = false;
                    bool fromDateParamExists = false;
                    bool toDateParamExists = false;

                    ParameterFieldDefinitions parameterFieldDefinitions = reportDocument.DataDefinition.ParameterFields;

                    foreach (ParameterFieldDefinition paramField in parameterFieldDefinitions)
                    {
                        if (paramField.Name == "LoanNo")
                        {
                            loanNoParamExists = true;
                        }
                        if (paramField.Name == "FromDate")
                        {
                            fromDateParamExists = true;
                        }
                        if (paramField.Name == "ToDate")
                        {
                            toDateParamExists = true;
                        }
                    }

                    if (!string.IsNullOrEmpty(loanNo))
                    {
                        if (loanNoParamExists)
                        {
                            reportDocument.SetParameterValue("LoanNo", loanNo);
                        }
                        else
                        {
                            MessageBox.Show("Parameter 'LoanNo' not found in the report.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        if (fromDateParamExists && toDateParamExists)
                        {
                            reportDocument.SetParameterValue("FromDate", fromDate);
                            reportDocument.SetParameterValue("ToDate", toDate);
                        }
                        else
                        {
                            MessageBox.Show("Date parameters 'FromDate' or 'ToDate' not found in the report.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    crystalReportViewer1.ReportSource = reportDocument;
                    crystalReportViewer1.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"Report file not found: {reportPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Item in Hand
        //public void LoadItemInHandReport(string itemType)
        //{
        //    crystalReportViewer1.ReportSource = null;
        //    crystalReportViewer1.Refresh();

        //    ReportDocument reportDocument = new ReportDocument();
        //    string reportPath = System.IO.Path.Combine(Application.StartupPath, "Report", "RptItemInHand.rpt");

        //    if (System.IO.File.Exists(reportPath))
        //    {
        //        try
        //        {
        //            reportDocument.Load(reportPath);
        //            reportDocument.SetDatabaseLogon("sa", "1234", "DESKTOP-6JKB17U\\SQL", "PawnBrokingNew");

        //            if (itemType == "Overall")
        //            {
        //                reportDocument.RecordSelectionFormula = "{ItemInHand.ItemType} = 'Overall'";
        //            }
        //            else
        //            {

        //                reportDocument.RecordSelectionFormula = "{ItemInHand.ItemType} = '" + itemType + "'";
        //                //reportDocument.RecordSelectionFormula = "{ItemInHand.ItemType} = N'" + itemType + "'";
        //            }

        //            crystalReportViewer1.ReportSource = reportDocument;
        //            crystalReportViewer1.Refresh();
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"Error loading report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show($"Report file not found: {reportPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        // ------------------------------------------------------------------------------------------------

        //public void LoadItemInHandReport(string itemType)
        //{
        //    crystalReportViewer1.ReportSource = null;
        //    crystalReportViewer1.Refresh();

        //    ReportDocument reportDocument = new ReportDocument();
        //    string reportPath = System.IO.Path.Combine(Application.StartupPath, "Report", "RptItemInHand.rpt"); // Path to your Crystal Report file

        //    if (System.IO.File.Exists(reportPath))
        //    {
        //        reportDocument.Load(reportPath);
        //        reportDocument.SetDatabaseLogon("sa", "1234", "DESKTOP-6JKB17U\\SQL", "PawnBrokingNew");

        //        try
        //        {
        //            if (itemType == "º¿õ¶ñ¢") // Overall option
        //            {
        //                // Set the RecordSelectionFormula to fetch all records
        //                reportDocument.RecordSelectionFormula = ""; // Empty string fetches all records
        //            }
        //            else
        //            {
        //                // Check if the parameter exists in the report
        //                bool parameterExists = false;
        //                ParameterFieldDefinitions parameterFieldDefinitions = reportDocument.DataDefinition.ParameterFields;

        //                foreach (ParameterFieldDefinition paramField in parameterFieldDefinitions)
        //                {
        //                    if (paramField.Name == "ItemTypeParam") // Ensure this matches the parameter name in your report
        //                    {
        //                        parameterExists = true;
        //                        break;
        //                    }
        //                }

        //                if (parameterExists)
        //                {
        //                    reportDocument.SetParameterValue("ItemTypeParam", itemType);
        //                }
        //                else
        //                {
        //                    MessageBox.Show("Parameter 'ItemTypeParam' not found in the report.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                }
        //            }

        //            // Set the report source
        //            crystalReportViewer1.ReportSource = reportDocument;
        //            crystalReportViewer1.Refresh();
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"An error occurred while setting the report parameter: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show($"Report file not found: {reportPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        //--------------------------------------------------------------------------------

        //public void LoadItemInHandReport(string itemType)
        //{
        //    crystalReportViewer1.ReportSource = null;
        //    crystalReportViewer1.Refresh();

        //    ReportDocument reportDocument = new ReportDocument();
        //    string reportPath = System.IO.Path.Combine(Application.StartupPath, "Report", "RptItemInHand.rpt"); // Path to your Crystal Report file

        //    if (System.IO.File.Exists(reportPath))
        //    {
        //        reportDocument.Load(reportPath);
        //        reportDocument.SetDatabaseLogon("sa", "1234", "DESKTOP-6JKB17U\\SQL", "PawnBrokingNew");

        //        try
        //        {
        //            if (itemType == "º¿õ¶ñ¢") 
        //            {                     
        //                reportDocument.RecordSelectionFormula = ""; 
        //            }
        //            else
        //            {
        //                reportDocument.SetParameterValue("ItemTypeParam", itemType);
        //            }

        //            crystalReportViewer1.ReportSource = reportDocument;
        //            crystalReportViewer1.Refresh();
        //            //crystalReportViewer1.RefreshReport(); 
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"An error occurred while setting the report parameter: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show($"Report file not found: {reportPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        // ----------------------------------------------------------------------------------
        // -----------------------------------------------------------------------------------

        //public void ItemInHandReport(string itemType)
        //{

        //    crystalReportViewer1.ReportSource = null;
        //    crystalReportViewer1.Refresh();

        //    ReportDocument reportDocument = new ReportDocument();
        //    string reportPath = System.IO.Path.Combine(Application.StartupPath, "Report", "Rptiteminhand.rpt");

        //    if (System.IO.File.Exists(reportPath))
        //    {
        //        reportDocument.Load(reportPath);
        //        reportDocument.SetDatabaseLogon("sa", "1234", "DESKTOP-6JKB17U\\SQL", "PawnBrokingNew");

        //        try
        //        {
        //            if (itemType == "Overall")
        //            {
        //                reportDocument.RecordSelectionFormula = string.Empty;
        //                Console.WriteLine("RecordSelectionFormula set to empty string.");
        //            }
        //            else
        //            {
        //                bool parameterExists = false;
        //                ParameterFieldDefinitions parameterFieldDefinitions = reportDocument.DataDefinition.ParameterFields;

        //                foreach (ParameterFieldDefinition paramField in parameterFieldDefinitions)
        //                {
        //                    if (paramField.Name == "ItemtypeParam")
        //                    {
        //                        parameterExists = true;
        //                        break;
        //                    }
        //                }

        //                if (parameterExists)
        //                {
        //                    reportDocument.SetParameterValue("ItemtypeParam", itemType);
        //                    Console.WriteLine($"Parameter 'ItemtypeParam' set to '{itemType}'.");
        //                }
        //                else
        //                {
        //                    MessageBox.Show("Parameter 'ItemTypeParam' not found in the report.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                    return;
        //                }
        //            }

        //            crystalReportViewer1.ReportSource = reportDocument;
        //            crystalReportViewer1.Refresh();
        //            Console.WriteLine("Report refreshed.");
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"An error occurred while setting the report parameter: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show($"Report file not found: {reportPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        public void ItemInHandReport(string reportPath, string selectionFormula)
        {
            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.Refresh();

            ReportDocument reportDocument = new ReportDocument();

            if (System.IO.File.Exists(reportPath))
            {
                reportDocument.Load(reportPath);
                reportDocument.SetDatabaseLogon("sa", "1234", "DESKTOP-6JKB17U\\SQL", "PawnBrokingNew");

                try
                {
                    if (!string.IsNullOrEmpty(selectionFormula))
                    {
                        reportDocument.RecordSelectionFormula = selectionFormula;
                    }

                    crystalReportViewer1.ReportSource = reportDocument;
                    crystalReportViewer1.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while setting the report parameter: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"Report file not found: {reportPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        #endregion

        #region Loan Item in Hand
        //public void LoanItemInHandReport(string itemType)
        //{
        //    crystalReportViewer1.ReportSource = null;
        //    crystalReportViewer1.Refresh();

        //    ReportDocument reportDocument = new ReportDocument();
        //    string reportPath = System.IO.Path.Combine(Application.StartupPath, "Report", "Rptloaniteminhand.rpt");

        //    if (System.IO.File.Exists(reportPath))
        //    {
        //        reportDocument.Load(reportPath);
        //        reportDocument.SetDatabaseLogon("sa", "1234", "DESKTOP-6JKB17U\\SQL", "PawnBrokingNew");

        //        try
        //        {
        //            if (itemType == "º¿õ¶ñ¢")
        //            {
        //                reportDocument.RecordSelectionFormula = "";
        //            }
        //            else
        //            {
        //                bool parameterExists = false;
        //                ParameterFieldDefinitions parameterFieldDefinitions = reportDocument.DataDefinition.ParameterFields;

        //                foreach (ParameterFieldDefinition paramField in parameterFieldDefinitions)
        //                {
        //                    if (paramField.Name == "ItemtypeParam")
        //                    {
        //                        parameterExists = true;
        //                        break;
        //                    }
        //                }

        //                if (parameterExists)
        //                {
        //                    reportDocument.SetParameterValue("ItemTypeParam", itemType);
        //                }
        //                else
        //                {
        //                    MessageBox.Show("Parameter 'ItemTypeParam' not found in the report.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                }
        //            }

        //            crystalReportViewer1.ReportSource = reportDocument;
        //            crystalReportViewer1.Refresh();
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"An error occurred while setting the report parameter: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show($"Report file not found: {reportPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        public void LoanItemInHandReport(string reportPath, string selectionFormula)
        {
            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.Refresh();

            ReportDocument reportDocument = new ReportDocument();

            if (System.IO.File.Exists(reportPath))
            {
                reportDocument.Load(reportPath);
                reportDocument.SetDatabaseLogon("sa", "1234", "DESKTOP-6JKB17U\\SQL", "PawnBrokingNew");

                try
                {
                    if (!string.IsNullOrEmpty(selectionFormula))
                    {
                        reportDocument.RecordSelectionFormula = selectionFormula;
                    }

                    crystalReportViewer1.ReportSource = reportDocument;
                    crystalReportViewer1.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while setting the report parameter: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"Report file not found: {reportPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Daily Loan
        //public void LoanItemInHandReport(string reportPath, string dateSelectionFormula, string status)
        //{
        //    crystalReportViewer1.ReportSource = null;
        //    crystalReportViewer1.Refresh();

        //    ReportDocument reportDocument = new ReportDocument();

        //    if (System.IO.File.Exists(reportPath))
        //    {
        //        reportDocument.Load(reportPath);
        //        reportDocument.SetDatabaseLogon("sa", "1234", "DESKTOP-6JKB17U\\SQL", "PawnBrokingNew");

        //        try
        //        {
        //            reportDocument.RecordSelectionFormula = $"{dateSelectionFormula} AND {{RptDailyLoanReturnQry.Status}} = '{status}'";

        //            crystalReportViewer1.ReportSource = reportDocument;
        //            crystalReportViewer1.Refresh();
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"An error occurred while setting the report parameter: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show($"Report file not found: {reportPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        public void DailyLoanReport(string reportPath, string dateSelectionFormula, int selectedIndex)
        {
            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.Refresh();

            ReportDocument reportDocument = new ReportDocument();

            if (System.IO.File.Exists(reportPath))
            {
                reportDocument.Load(reportPath);
                reportDocument.SetDatabaseLogon("sa", "1234", "DESKTOP-6JKB17U\\SQL", "PawnBrokingNew");

                try
                {
                    string fullSelectionFormula = dateSelectionFormula;
                    string status = string.Empty;

                    switch (selectedIndex)
                    {
                        case 0: // Return
                            status = "R";
                            fullSelectionFormula += $" AND {{RptDailyLoanReturnQry.Status}} = '{status}'";
                            break;
                        case 1: // Not Return
                            status = "L";
                            fullSelectionFormula += $" AND {{RptDailyLoanQry.Status}} = '{status}'";
                            break;
                        case 2: // Auction
                            status = "A";
                            fullSelectionFormula += $" AND {{RptDailyLoanQry.Status}} = '{status}'";
                            break;
                        default:
                            MessageBox.Show("Invalid selection type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                    }

                    // Apply the full selection formula
                    reportDocument.RecordSelectionFormula = fullSelectionFormula;

                    // Set the report to the Crystal Report Viewer
                    crystalReportViewer1.ReportSource = reportDocument;
                    crystalReportViewer1.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while setting the report parameter: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"Report file not found: {reportPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Daily Loan Details
        public void DailyLoanDetailsReport(string reportPath, string dateSelectionFormula, int selectedIndex)
        {
            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.Refresh();

            ReportDocument reportDocument = new ReportDocument();

            if (System.IO.File.Exists(reportPath))
            {
                reportDocument.Load(reportPath);
                reportDocument.SetDatabaseLogon("sa", "1234", "DESKTOP-6JKB17U\\SQL", "PawnBrokingNew");

                try
                {
                    string fullSelectionFormula = dateSelectionFormula;
                    string status = string.Empty;

                    switch (selectedIndex)
                    {
                        case 0: // Return
                            status = "R";
                            fullSelectionFormula += $" AND {{RptnewLoanDetails_Qry.Status}} = '{status}'";
                            break;
                        case 1: // Not Return
                            status = "L";
                            fullSelectionFormula += $" AND {{RptnewLoanDetails_Qry.Status}}  = '{status}'";
                            break;
                        case 2: // Auction
                            status = "A";
                            fullSelectionFormula += $" AND {{RptnewLoanDetails_Qry.Status}}  = '{status}'";
                            break;
                        default:
                            MessageBox.Show("Invalid selection type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                    }

                    reportDocument.RecordSelectionFormula = fullSelectionFormula;

                    crystalReportViewer1.ReportSource = reportDocument;
                    crystalReportViewer1.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while setting the report parameter: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"Report file not found: {reportPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Loan Details Item in Hand
        public void LoanItemInHandReport(string reportPath, string selectionFormula, int selectedIndex)
        {
            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.Refresh();

            ReportDocument reportDocument = new ReportDocument();

            if (System.IO.File.Exists(reportPath))
            {
                reportDocument.Load(reportPath);
                reportDocument.SetDatabaseLogon("sa", "1234", "DESKTOP-6JKB17U\\SQL", "PawnBrokingNew"); // Set database login credentials

                try
                {
                    string fullSelectionFormula = selectionFormula;
                    string status = "L";

                    switch (selectedIndex)
                    {
                        case 0: // Overall
                            fullSelectionFormula = "{RptLoanDetailsItemInHandQry.Status} = 'L'";
                            break;
                        case 1: // Gold
                        case 2: // Silver
                            break;
                        default:
                            MessageBox.Show("Invalid selection type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                    }

                    reportDocument.RecordSelectionFormula = fullSelectionFormula;

                    crystalReportViewer1.ReportSource = reportDocument;
                    crystalReportViewer1.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while setting the report parameter: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"Report file not found: {reportPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Account Interest Amount
        public void LoanAccountReport(string reportPath, string selectionFormula)
        {
            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.Refresh();

            ReportDocument reportDocument = new ReportDocument();

            if (System.IO.File.Exists(reportPath))
            {
                reportDocument.Load(reportPath);
                reportDocument.SetDatabaseLogon("sa", "1234", "DESKTOP-6JKB17U\\SQL", "PawnBrokingNew");

                try
                {
                    if (!string.IsNullOrEmpty(selectionFormula))
                    {
                        reportDocument.RecordSelectionFormula = selectionFormula;
                    }

                    crystalReportViewer1.ReportSource = reportDocument;
                    crystalReportViewer1.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while setting the report parameter: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"Report file not found: {reportPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Loan History
        public void LoadLoanReport(string loanNo)
        {
            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.Refresh();

            ReportDocument reportDocument = new ReportDocument();
            string reportPath = System.IO.Path.Combine(Application.StartupPath, "Report", "RptLoanHistory.rpt");

            if (System.IO.File.Exists(reportPath))
            {
                reportDocument.Load(reportPath);

                reportDocument.SetDatabaseLogon("sa", "1234", "DESKTOP-6JKB17U\\SQL", "PawnBrokingNew");

                try
                {
                    bool parameterExists = false;
                    ParameterFieldDefinitions parameterFieldDefinitions = reportDocument.DataDefinition.ParameterFields;

                    foreach (ParameterFieldDefinition paramField in parameterFieldDefinitions)
                    {
                        if (paramField.Name == "LoanNo")
                        {
                            parameterExists = true;
                            break;
                        }
                    }

                    if (parameterExists)
                    {

                        reportDocument.SetParameterValue("LoanNo", loanNo);
                    }
                    else
                    {
                        MessageBox.Show("Parameter 'LoanNo' not found in the report.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                    crystalReportViewer1.ReportSource = reportDocument;
                    crystalReportViewer1.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while setting the report parameter: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"Report file not found: {reportPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Pending Details

        //public void LoadReport(int months, DateTime fromDate, DateTime toDate)
        //{
        //    crystalReportViewer1.ReportSource = null;
        //    crystalReportViewer1.Refresh();

        //    ReportDocument reportDocument = new ReportDocument();
        //    string reportPath = System.IO.Path.Combine(Application.StartupPath, "Report", "rptAddressnew.rpt");  // Path to your Crystal Report file

        //    // Check if the report file exists
        //    if (System.IO.File.Exists(reportPath))
        //    {
        //        reportDocument.Load(reportPath);

        //        // Set the database login credentials
        //        reportDocument.SetDatabaseLogon("sa", "1234", "DESKTOP-6JKB17U\\SQL", "PawnBrokingNew");

        //        try
        //        {
        //            // Check if the report has the necessary parameters
        //            bool monthParamExists = false;
        //            bool fromDateParamExists = false;
        //            bool toDateParamExists = false;

        //            ParameterFieldDefinitions parameterFieldDefinitions = reportDocument.DataDefinition.ParameterFields;

        //            // Iterate through the report parameters and set the flag if the parameter exists
        //            foreach (ParameterFieldDefinition paramField in parameterFieldDefinitions)
        //            {
        //                if (paramField.Name == "MonthsParam") monthParamExists = true;
        //                if (paramField.Name == "FromDate") fromDateParamExists = true;
        //                if (paramField.Name == "ToDate") toDateParamExists = true;
        //            }

        //            // Set parameters if they exist in the report
        //            if (monthParamExists)
        //            {
        //                reportDocument.SetParameterValue("MonthsParam", months);
        //            }
        //            else
        //            {
        //                MessageBox.Show("Parameter 'MonthsParam' not found in the report.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            }

        //            if (fromDateParamExists)
        //            {
        //                reportDocument.SetParameterValue("FromDate", fromDate);
        //            }
        //            else
        //            {
        //                MessageBox.Show("Parameter 'FromDate' not found in the report.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            }

        //            if (toDateParamExists)
        //            {
        //                reportDocument.SetParameterValue("ToDate", toDate);
        //            }
        //            else
        //            {
        //                MessageBox.Show("Parameter 'ToDate' not found in the report.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            }

        //            // Set the report source to the Crystal Report Viewer
        //            crystalReportViewer1.ReportSource = reportDocument;
        //            crystalReportViewer1.Refresh();
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"An error occurred while setting the report parameters: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show($"Report file not found: {reportPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        public void LoadReport(int months, DateTime fromDate, DateTime toDate)  //Address Button
        {
            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.Refresh();

            ReportDocument reportDocument = new ReportDocument();
            string reportPath = System.IO.Path.Combine(Application.StartupPath, "Report", "rptAddressNewEdited.rpt");  

            if (System.IO.File.Exists(reportPath))
            {
                reportDocument.Load(reportPath);
                reportDocument.SetDatabaseLogon("sa", "1234", "DESKTOP-6JKB17U\\SQL", "PawnBrokingNew");

                try
                {
                    DataTable loanData = GetLoanData(months, fromDate, toDate);
                    reportDocument.SetDataSource(loanData); 

                    crystalReportViewer1.ReportSource = reportDocument;
                    crystalReportViewer1.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while loading the report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"Report file not found: {reportPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable GetLoanData(int months, DateTime fromDate, DateTime toDate) 
        {
            DataTable dt = new DataTable();

            string connectionString = "Data Source=DESKTOP-6JKB17U\\SQL;Initial Catalog=PawnBrokingNew;User ID=sa;Password=1234"; // Update with your connection string

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM LoanFindQry " +
                               $"WHERE DATEADD(month, {months}, LoanDate) >= @FromDate " +
                               $"AND DATEADD(month, {months}, LoanDate) <= @ToDate " +
                               "ORDER BY dbo.fn_loanchar(LoanNo), dbo.fn_loannum(LoanNo)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {                 
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }

            return dt;
        }
        public void LoadLoanRegisterReport(int months, DateTime fromDate, DateTime toDate)  //Details Button
        {
            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.Refresh();

            ReportDocument reportDocument = new ReportDocument();
            string reportPath = System.IO.Path.Combine(Application.StartupPath, "Report", "RptLoanRegisterEdited.rpt");  

            if (System.IO.File.Exists(reportPath))
            {
                reportDocument.Load(reportPath);
                reportDocument.SetDatabaseLogon("sa", "1234", "DESKTOP-6JKB17U\\SQL", "PawnBrokingNew"); 

                try
                {

                    DataTable loanData = LoanDetailsData(months, fromDate, toDate);
                    reportDocument.SetDataSource(loanData); 

           
                    crystalReportViewer1.ReportSource = reportDocument;
                    crystalReportViewer1.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while loading the report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"Report not found: {reportPath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable LoanDetailsData(int months, DateTime fromDate, DateTime toDate)
        { 
            DataTable dt = new DataTable();

            string connectionString = "Data Source=DESKTOP-6JKB17U\\SQL;Initial Catalog=PawnBrokingNew;User ID=sa;Password=1234"; // Update with your connection string

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM LoanRegisterNew WHERE ID IN " +
                               "(SELECT ID FROM Loan WHERE Status='L' AND " +
                               $"DATEADD(month, {months}, LoanDate) >= @FromDate " +
                               $"AND DATEADD(month, {months}, LoanDate) <= @ToDate) " +
                               "ORDER BY dbo.fn_loanchar(LoanNo), dbo.fn_loannum(LoanNo)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }

            return dt;
        }

        #endregion
    }
}
