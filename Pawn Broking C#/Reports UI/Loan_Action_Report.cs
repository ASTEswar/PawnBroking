using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pawn_Broking.UI
{
    public partial class Loan_Action_Report : Form
    {
        public Loan_Action_Report()
        {
            InitializeComponent();
        }

        private void btnAddress_Click(object sender, EventArgs e)
        {
            DateTime fromDate = dtpFromDate.Value.Date;
            DateTime toDate = dtpToDate.Value.Date;

            if (toDate < fromDate)
            {
                MessageBox.Show("End date cannot be earlier than Start date.", "Date Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            CrysReportViewer reportViewer = new CrysReportViewer();
            reportViewer.LoadAddressReport(fromDate, toDate);
            reportViewer.ShowDialog();
        }

        private void btnPendingDetails_Click(object sender, EventArgs e)
        {
            DateTime fromDate = dtpFromDate.Value;
            DateTime toDate = dtpToDate.Value;
            string loanNo = txtLoanNo.Text;

            //if (toDate < fromDate)
            //{
            //    MessageBox.Show("End date cannot be earlier than the start date.", "Date Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            CrysReportViewer reportViewer = new CrysReportViewer();
            reportViewer.LoadPendingLoanReport(fromDate, toDate, loanNo);
            reportViewer.ShowDialog();
        }

        private void btnActionDetails_Click(object sender, EventArgs e)
        {

            DateTime fromDate = dtpFromDate.Value;
            DateTime toDate = dtpToDate.Value;
            string loanNo = txtLoanNo.Text;

            //if (toDate < fromDate)
            //{
            //    MessageBox.Show("End date cannot be earlier than the start date.", "Date Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            CrysReportViewer reportViewer = new CrysReportViewer();
            reportViewer.LoadPendingLoanReport(fromDate, toDate, loanNo);
            reportViewer.ShowDialog();
        }

        private void LoanActionReport_Load(object sender, EventArgs e)
        {

        }
    }
}
