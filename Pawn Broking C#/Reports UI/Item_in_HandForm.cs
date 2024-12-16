using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Windows.Forms;
using Pawn_Broking.DAL;
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
    public partial class Item_in_HandForm : Form
    {
        private ItemDetailDAL dataAccess = new ItemDetailDAL();
        public Item_in_HandForm()
        {
            InitializeComponent();
        }

        private void Item_in_HandForm_Load(object sender, EventArgs e)
        {
            //DataTable items = dataAccess.GetDistinctItems();
            //cmbItemType.DataSource = items;
            //cmbItemType.DisplayMember = "ItemType";
            //cmbItemType.ValueMember = "ItemType";
            cmbItemType.Items.Add("Overall"); // º¿õ¶ñ¢ (Overall)
            cmbItemType.Items.Add("ªð£ù¢");    // ªð£ù¢ (Gold)
            cmbItemType.Items.Add("ªõ÷¢÷¤");  // ªõ÷¢÷¤ (Silver)
            cmbItemType.SelectedIndex = 0;
        }

        //string itemType = cmbItemType.Text.Trim();

        //CrysReportViewer reportViewer = new CrysReportViewer();
        //reportViewer.ItemInHandReport(itemType);
        //reportViewer.ShowDialog();
        private void btnShow_Click(object sender, EventArgs e)
        {
            string reportPath = System.IO.Path.Combine(Application.StartupPath, "Report", "RptiteminhandEdited.rpt");
            string selectionFormula = "";
            int selectedIndex = cmbItemType.SelectedIndex;

            switch (selectedIndex)
            {
                case 0: // Overall 
                    selectionFormula = "";
                    break;
                case 1: // Gold
                    selectionFormula = $"{{IteminHand.ItemType}} = 'ªð£ù¢'";
                    break;
                case 2: // Silver
                    selectionFormula = $"{{IteminHand.ItemType}} = 'ªõ÷¢÷¤'";
                    break;
                default:
                    MessageBox.Show("Invalid item type selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            CrysReportViewer reportViewer = new CrysReportViewer();
            reportViewer.ItemInHandReport(reportPath, selectionFormula);
            reportViewer.ShowDialog();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
