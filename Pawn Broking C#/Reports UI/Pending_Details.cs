using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace Pawn_Broking.UI
{
    public partial class Pending_Details : Form
    {
        public Pending_Details()
        {
            InitializeComponent();
        }

        private void Pending_Details_Load(object sender, EventArgs e)
        {

        }

        private void btnAddress_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMonth.Text) || !int.TryParse(txtMonth.Text, out int months) || months <= 0)
            {
                MessageBox.Show("Enter valid Months", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMonth.Focus();
                return;
            }

            DateTime fromDate = dtpFromDate.Value;
            DateTime toDate = dtpToDate.Value;

            CrysReportViewer reportViewer = new CrysReportViewer();
            reportViewer.LoadReport(months, fromDate, toDate);
            reportViewer.ShowDialog();
        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMonth.Text) || !int.TryParse(txtMonth.Text, out int months) || months <= 0)
            {
                MessageBox.Show("Enter valid Months", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMonth.Focus();
                return;
            }

            DateTime fromDate = dtpFromDate.Value;
            DateTime toDate = dtpToDate.Value;

            CrysReportViewer reportViewer = new CrysReportViewer();
            reportViewer.LoadLoanRegisterReport(months, fromDate, toDate);
            reportViewer.ShowDialog();
        }
    }
}
