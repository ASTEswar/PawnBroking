using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pawn_Broking.UI
{
    public partial class Account_Interest_Amount : Form
    {
        public Account_Interest_Amount()
        {
            InitializeComponent();
        }

        private void Acc_Interest_Amount_Load(object sender, EventArgs e)
        {
            dtpFromDate.Value = DateTime.Now;
            dtpToDate.Value = DateTime.Now;
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            string reportPath = System.IO.Path.Combine(Application.StartupPath, "Report", "rptLoanAcct.rpt");
            string selectionFormula = "";

            selectionFormula = $"{{loanRegisterNew.LoanDate}} >= Date({dtpFromDate.Value.Year}, {dtpFromDate.Value.Month}, {dtpFromDate.Value.Day}) " +
                               $"AND {{loanRegisterNew.LoanDate}} <= Date({dtpToDate.Value.Year}, {dtpToDate.Value.Month}, {dtpToDate.Value.Day})";

           
            CrysReportViewer reportViewer = new CrysReportViewer();
            reportViewer.LoanAccountReport(reportPath, selectionFormula);
            reportViewer.ShowDialog();
        }
    }
}
//{{loanREGISTERnew.LOANDate} }