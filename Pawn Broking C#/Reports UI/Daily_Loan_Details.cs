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
    public partial class Daily_Loan_Details : Form
    {
        public Daily_Loan_Details()
        {
            InitializeComponent();
        }
        private void Daily_Loan_Details_Load(object sender, EventArgs e)
        {
            dtpFromDate.Value = DateTime.Now;
            dtpToDate.Value = DateTime.Now;

            cmbLoanType.Items.Add("î¤¼ð¢ð¤ò¶"); // Return
            cmbLoanType.Items.Add("î¤¼ð¢ðõ¤ô¢¬ô"); // NotReturn
            cmbLoanType.Items.Add("ãôñ¢"); // Auction

            cmbLoanType.SelectedIndex = 0;
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            string reportPath = System.IO.Path.Combine(Application.StartupPath, "Report", "RptLoanDetails.rpt");
            string dateSelectionFormula = "";
            int selectedIndex = cmbLoanType.SelectedIndex;

            if (cmbLoanType.SelectedIndex == 0) // Return
            {
                dateSelectionFormula = $"{{RptnewLoanDetails_Qry.RetDate}} >= Date({dtpFromDate.Value.Year}, {dtpFromDate.Value.Month}, {dtpFromDate.Value.Day}) " +
                                       $"AND {{RptnewLoanDetails_Qry.RetDate}} <= Date({dtpToDate.Value.Year}, {dtpToDate.Value.Month}, {dtpToDate.Value.Day}) ";
                                       //$"AND {{RptnewLoanDetails_Qry.Status}} = 'R'";
            }
            else if (cmbLoanType.SelectedIndex == 1) // Not Return
            {
                dateSelectionFormula = $"{{RptnewLoanDetails_Qry.loandate}} >= Date({dtpFromDate.Value.Year}, {dtpFromDate.Value.Month}, {dtpFromDate.Value.Day}) " +
                                       $"AND {{RptnewLoanDetails_Qry.loandate}} <= Date({dtpToDate.Value.Year}, {dtpToDate.Value.Month}, {dtpToDate.Value.Day}) ";
                                       //$"AND {{RptnewLoanDetails_Qry.Status}} = 'L'";
            }
            else if (cmbLoanType.SelectedIndex == 2) // Auction
            {
                dateSelectionFormula = $"{{RptnewLoanDetails_Qry.ActionDate}} >= Date({dtpFromDate.Value.Year}, {dtpFromDate.Value.Month}, {dtpFromDate.Value.Day}) " +
                                       $"AND {{RptnewLoanDetails_Qry.ActionDate}} <= Date({dtpToDate.Value.Year}, {dtpToDate.Value.Month}, {dtpToDate.Value.Day}) ";
                                       //$"AND {{RptnewLoanDetails_Qry.Status}} = 'A'";
            }

            CrysReportViewer reportViewer = new CrysReportViewer();
            reportViewer.DailyLoanDetailsReport(reportPath, dateSelectionFormula, selectedIndex); 
            reportViewer.ShowDialog();
        }

       
    }
}
