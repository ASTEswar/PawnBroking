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
    public partial class Loan_Details_Itemin_Hand : Form
    {
        public Loan_Details_Itemin_Hand()
        {
            InitializeComponent();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string reportPath = System.IO.Path.Combine(Application.StartupPath, "Report", "RptLoanDetailsItemInHand.rpt");
            string selectionFormula = "";
            int selectedIndex = cmbLoanType.SelectedIndex;

            if (selectedIndex == 0) // Overall
            {
                selectionFormula = "{RptLoanDetailsItemInHandQry.Status}='L'"; 
            }
            else if (selectedIndex == 1 || selectedIndex == 2) // Gold or Silver
            {
                string itemType = cmbLoanType.Text.Trim();
                selectionFormula = $"{{RptLoanDetailsItemInHandQry.ItemType}}='{itemType}' AND {{RptLoanDetailsItemInHandQry.Status}}='L'";
            }
          
            CrysReportViewer reportViewer = new CrysReportViewer();
            reportViewer.LoanItemInHandReport(reportPath, selectionFormula, selectedIndex);
            reportViewer.ShowDialog();
        }

        private void Loan_Details_ItemIn_Hand_Load(object sender, EventArgs e)
        {
            cmbLoanType.Items.Add("Overall"); // º¿õ¶ñ¢ (Overall)
            cmbLoanType.Items.Add("ªð£ù¢");    // ªð£ù¢ (Gold)
            cmbLoanType.Items.Add("ªõ÷¢÷¤");  // ªõ÷¢÷¤ (Silver)
            cmbLoanType.SelectedIndex = 0;
        }
    }
}
