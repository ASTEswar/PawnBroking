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
    public partial class Loan_Item_in_Hand : Form
    {
        public Loan_Item_in_Hand()
        {
            InitializeComponent();
        }

        private void Loan_Item_in_Hand_Load(object sender, EventArgs e)
        {
            cmbItemType.Items.Add("º¿õ¶ñ¢"); // º¿õ¶ñ¢ (Overall)
            cmbItemType.Items.Add("ªð£ù¢");    // ªð£ù¢ (Gold)
            cmbItemType.Items.Add("ªõ÷¢÷¤");  // ªõ÷¢÷¤ (Silver)
            cmbItemType.SelectedIndex = 0;
        }

        //string itemType = cmbItemType.Text.Trim();

        //CrysReportViewer reportViewer = new CrysReportViewer();
        //reportViewer.LoanItemInHandReport(itemType);
        //reportViewer.ShowDialog();  

        private void btnShow_Click(object sender, EventArgs e)
        {
            string reportPath = System.IO.Path.Combine(Application.StartupPath, "Report", "RptLoanIteminHandEdited.rpt");
            string selectionFormula = "";
            int selectedIndex = cmbItemType.SelectedIndex;

            switch (selectedIndex)
            {
                case 0: //Overall 
                    selectionFormula = "";
                    break;
                case 1: // Gold
                    selectionFormula = $"{{Loan.ItemType}} = 'ªð£ù¢'";
                    break;
                case 2: // Silver
                    selectionFormula = $"{{Loan.ItemType}} = 'ªõ÷¢÷¤'";
                    break;
                default:
                    MessageBox.Show("Invalid item type selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }
            CrysReportViewer reportViewer = new CrysReportViewer();
            reportViewer.LoanItemInHandReport(reportPath, selectionFormula);
            reportViewer.ShowDialog();
        }
    }
}
