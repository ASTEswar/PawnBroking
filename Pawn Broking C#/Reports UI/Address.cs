using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Pawn_Broking.DAL;
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
    public partial class Address : Form
    {
        private CustomerDetailDAL dataAccess = new CustomerDetailDAL();
        public Address()
        {
            InitializeComponent();
        }

        private void Address_Load(object sender, EventArgs e)
        {
            LoadTowns();
            cmbTown.SelectedIndex = 1;
        }
        private void LoadTowns()
        {
            DataTable towns = dataAccess.GetDistinctTowns();
            cmbTown.DataSource = towns;
            cmbTown.DisplayMember = "Town";
            cmbTown.ValueMember = "Town";
        }
        private void btnDetails_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbTown.Text))
            {
                MessageBox.Show("Select Any Place", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbTown.Focus();
            }
            else
            {
                PrintReport();
            }
        }
        private void PrintReport()
        {
            using (var reportViewerForm = new CrysReportViewer())
            {
                ReportDocument reportDocument = new ReportDocument();

                string reportPath = System.IO.Path.Combine(Application.StartupPath, "Report", "rptAddress.rpt");
                if (!System.IO.File.Exists(reportPath))
                {
                    MessageBox.Show("Report file not found: " + reportPath);
                    return;
                }

                try
                {
                    reportDocument.Load(reportPath);

                    ConnectionInfo connectionInfo = new ConnectionInfo
                    {
                        ServerName = "DESKTOP-6JKB17U\\SQL",
                        DatabaseName = "PawnBrokingNew",
                        UserID = "sa",
                        Password = "1234"
                    };
                    SetDatabaseLogonForReport(connectionInfo, reportDocument);
                    reportDocument.RecordSelectionFormula = "{CustomerMaster.Town} = '" + cmbTown.Text + "'";
                    reportViewerForm.crystalReportViewer1.ReportSource = reportDocument;
                    reportViewerForm.crystalReportViewer1.RefreshReport();
                    reportViewerForm.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error displaying report: " + ex.Message);
                }
            }
        }

        private void SetDatabaseLogonForReport(ConnectionInfo connectionInfo, ReportDocument reportDocument)
        {
                Tables tables = reportDocument.Database.Tables;
                foreach (Table table in tables)
            {
                TableLogOnInfo tableLogOnInfo = table.LogOnInfo;
                tableLogOnInfo.ConnectionInfo = connectionInfo;
                table.ApplyLogOnInfo(tableLogOnInfo);
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}
