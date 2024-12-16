using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;
using Pawn_Broking.BLL;
using Pawn_Broking.DAL;
using System.Threading;



namespace Pawn_Broking.UI
{
    public partial class Loan_Register_Form : Form
    {
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;

        public Loan_Register_Form()
        {
            InitializeComponent();
        }

        private void LoanregisterForm1_Load(object sender, EventArgs e)
        {
            dtpFromDate.Value = DateTime.Now;
            dtpToDate.Value = DateTime.Now;
        }

        //LoanDAL loanDAL = new LoanDAL();
        private void btnPrint_Click(object sender, EventArgs e)
        {
            string loanNo = txtLoanNo.Text;
            DateTime startDate = dtpFromDate.Value.Date; 
            DateTime endDate = dtpToDate.Value.Date; 

            if (endDate < startDate)
            {
                MessageBox.Show("End date cannot be earlier than the start date.", "Date Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            CrysReportViewer reportViewer = new CrysReportViewer();
            reportViewer.LoadLoanRegisterReport(startDate, endDate, loanNo);
            reportViewer.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
