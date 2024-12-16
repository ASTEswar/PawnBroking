using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pawn_Broking.UI
{
    public partial class Daily_Loan : Form
    {
        public Daily_Loan()
        {
            InitializeComponent();
        }

        private void Daily_Loan_Load(object sender, EventArgs e)
        {
            dtpFromDate.Value = DateTime.Now;
            dtpToDate.Value = DateTime.Now;

            cmbLoanType.Items.Add("î¤¼ð¢ð¤ò¶"); // Return
            cmbLoanType.Items.Add("î¤¼ð¢ðõ¤ô¢¬ô"); // NotReturn
            cmbLoanType.Items.Add("ãôñ¢"); // Auction

            cmbLoanType.SelectedIndex = 0;
        }

        //string reportPath = "";
        //string dateSelectionFormula = $"{{RptDailyLoanReturnQry.RetDate}} >= Date({dtpFromDate.Value.Year}, {dtpFromDate.Value.Month}, {dtpFromDate.Value.Day}) " +
        //                              $"AND {{RptDailyLoanReturnQry.RetDate}} <= Date({dtpToDate.Value.Year}, {dtpToDate.Value.Month}, {dtpToDate.Value.Day})";
        //string status = "";

        //if (cmbLoanType.SelectedIndex == 0) // Return
        //{
        //    reportPath = @"G:\Pawn Broking C#\Pawn Broking\Report\RptLoanReturn.rpt"; 
        //    status = "R";
        //}
        //else if (cmbLoanType.SelectedIndex == 1) // Not Return
        //{
        //    reportPath = @"G:\Pawn Broking C#\Pawn Broking\Report\RptLoan.rpt"; 
        //    status = "L";
        //}
        //else if (cmbLoanType.SelectedIndex == 2) // Auction
        //{
        //    reportPath = @"G:\Pawn Broking C#\Pawn Broking\Report\RptLoanAction.rpt"; 
        //    dateSelectionFormula = $"{{ActionMaster.ActionDate}} >= Date({dtpFromDate.Value.Year}, {dtpFromDate.Value.Month}, {dtpFromDate.Value.Day}) " +
        //                           $"AND {{ActionMaster.ActionDate}} <= Date({dtpToDate.Value.Year}, {dtpToDate.Value.Month}, {dtpToDate.Value.Day})";
        //    status = "A";
        //}

        //CrysReportViewer reportViewer = new CrysReportViewer();
        //reportViewer.LoanItemInHandReport(reportPath, dateSelectionFormula, status);
        //reportViewer.ShowDialog();
        private void btnPrint_Click(object sender, EventArgs e)
        {
            string reportPath = "";
            string dateSelectionFormula = "";
            int selectedIndex = cmbLoanType.SelectedIndex;

            if (selectedIndex == 0) // Return
            {
                reportPath = @"G:\Pawn Broking C#\Pawn Broking\Report\RptLoanReturn.rpt";
                dateSelectionFormula = $"{{RptDailyLoanReturnQry.RetDate}} >= Date({dtpFromDate.Value.Year}, {dtpFromDate.Value.Month}, {dtpFromDate.Value.Day}) " +
                                       $"AND {{RptDailyLoanReturnQry.RetDate}} <= Date({dtpToDate.Value.Year}, {dtpToDate.Value.Month}, {dtpToDate.Value.Day})";
            }
            else if (selectedIndex == 1) // Not Return
            {
                reportPath = @"G:\Pawn Broking C#\Pawn Broking\Report\RptLoan.rpt";
                dateSelectionFormula = $"{{RptDailyLoanQry.loandate}} >= Date({dtpFromDate.Value.Year}, {dtpFromDate.Value.Month}, {dtpFromDate.Value.Day}) " +
                                       $"AND {{RptDailyLoanQry.loandate}} <= Date({dtpToDate.Value.Year}, {dtpToDate.Value.Month}, {dtpToDate.Value.Day})";
            }
            else if (selectedIndex == 2) // Auction
            {
                reportPath = @"G:\Pawn Broking C#\Pawn Broking\Report\RptLoanAction.rpt";
                dateSelectionFormula = $"{{ActionMaster.ActionDate}} >= Date({dtpFromDate.Value.Year}, {dtpFromDate.Value.Month}, {dtpFromDate.Value.Day}) " +
                                       $"AND {{ActionMaster.ActionDate}} <= Date({dtpToDate.Value.Year}, {dtpToDate.Value.Month}, {dtpToDate.Value.Day})";
            }
            else
            {
                MessageBox.Show("Please select a valid loan type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
   
            CrysReportViewer reportViewer = new CrysReportViewer();
            reportViewer.DailyLoanReport(reportPath, dateSelectionFormula, selectedIndex);
            reportViewer.ShowDialog();
        }
     
    }
}
