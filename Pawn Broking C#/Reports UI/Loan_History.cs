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
    public partial class Loan_History : Form
    {
        public Loan_History()
        {
            InitializeComponent();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                string loanNo = txtLoanNo.Text.Trim(); 

                if (!string.IsNullOrEmpty(loanNo))
                {
                    CrysReportViewer reportViewer = new CrysReportViewer(); 
                    reportViewer.LoadLoanReport(loanNo); 
                    reportViewer.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Please enter a valid Loan Number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
