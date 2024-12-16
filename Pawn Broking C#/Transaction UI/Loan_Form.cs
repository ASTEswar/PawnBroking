using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Pawn_Broking.BLL;
using Pawn_Broking.DAL;
using Pawn_Broking.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Pawn_Broking.Master_UI
{
    public partial class Loan_Form : Form
    {
        private Dictionary<Control, Control> controlFocusMap;
        Timer blinkTimer = new Timer();
        private bool SaveFlg = false;
        public Loan_Form()
        {
            InitializeComponent();

            blinkTimer.Interval = 500;
            blinkTimer.Tick += timerlblLoanAmnt_Tick;
            blinkTimer.Start();

            dtpLoanDate.Value = DateTime.Now;
            dtpRedeemDate.Value = dtpLoanDate.Value.AddMonths(1).AddDays(-1);

            #region Textbox BackColor Styling
            txtamount.Enter += TextBox_Enter;
            txtamount.Leave += TextBox_Leave;
            txtPhone.Enter += TextBox_Enter;
            txtPhone.Leave += TextBox_Leave;
            txtname.Enter += TextBox_Enter;
            txtname.Leave += TextBox_Leave;
            txtFName.Enter += TextBox_Enter;
            txtFName.Leave += TextBox_Leave;
            txtAddress.Enter += TextBox_Enter;
            txtAddress.Leave += TextBox_Leave;
            txtTown.Enter += TextBox_Enter;
            txtTown.Leave += TextBox_Leave;
            txtTaluk.Enter += TextBox_Enter;
            txtTaluk.Leave += TextBox_Leave;
            txtDistrict.Enter += TextBox_Enter;
            txtDistrict.Leave += TextBox_Leave;
            txtOccupation.Enter += TextBox_Enter;
            txtOccupation.Leave += TextBox_Leave;
            txtGuName.Enter += TextBox_Enter;
            txtGuName.Leave += TextBox_Leave;
            txtGuOccupation.Enter += TextBox_Enter;
            txtGuOccupation.Leave += TextBox_Leave;
            txtaadhaarno.Enter += TextBox_Enter;
            txtaadhaarno.Leave += TextBox_Leave;
            txtItemDetails.Enter += TextBox_Enter;
            txtItemDetails.Leave += TextBox_Leave;
            txtSeal.Enter += TextBox_Enter;
            txtSeal.Leave += TextBox_Leave;
            txtQty.Enter += TextBox_Enter;
            txtQty.Leave += TextBox_Leave;
            txtGram.Enter += TextBox_Enter;
            txtGram.Leave += TextBox_Leave;
            txtIdentification.Enter += TextBox_Enter;
            txtIdentification.Leave += TextBox_Leave;
            txtOldLoanNo.Enter += TextBox_Enter;
            txtOldLoanNo.Leave += TextBox_Leave;
            txtLoanMark.Enter += TextBox_Enter;
            txtLoanMark.Leave += TextBox_Leave;
            txtInterest.Enter += TextBox_Enter;
            txtInterest.Leave += TextBox_Leave;
            cmbIntPerc.Enter += TextBox_Enter;
            cmbIntPerc.Leave += TextBox_Leave;
            txtReceiptCG.Enter += TextBox_Enter;
            txtReceiptCG.Leave += TextBox_Leave;
            txtValue.Enter += TextBox_Enter;
            txtValue.Leave += TextBox_Leave;
            txtkg.Enter += TextBox_Enter;
            txtkg.Leave += TextBox_Leave;
            lblTotalQty.Enter += TextBox_Enter;
            lblTotalQty.Leave += TextBox_Leave;
            txtloanno.Enter += TextBox_Enter;
            txtloanno.Leave += TextBox_Leave;
            #endregion

            #region Enter Function
            controlFocusMap = new Dictionary<Control, Control>
            {
                {txtamount,dtpRedeemDate },
                {dtpRedeemDate,txtOldLoanNo },
                {txtOldLoanNo,txtLoanMark },
                {txtLoanMark,txtPhone },
                {txtPhone,cmbNameSalutation },
                {cmbNameSalutation,txtname },
                {txtname,cmbFnameTitle },
                {cmbFnameTitle,txtFName },
                {txtFName,txtAddress },
                {txtAddress,txtTown },
                {txtTown,txtTaluk },
                {txtTaluk,txtDistrict },
                {txtDistrict,txtOccupation },
                {txtOccupation,txtGuName },
                {txtGuName,txtGuOccupation },
                {txtGuOccupation,txtaadhaarno },
                {txtaadhaarno,cmbItemtype },
                {cmbItemtype,txtItemDetails },
                {txtItemDetails,txtSeal },
                {txtSeal,txtQty },
                {txtQty,txtGram },
                {txtGram,txtIdentification },
                {cmbIntPerc,txtInterest },
                {txtInterest,txtValue },
                {txtValue,txtReceiptCG },
                {txtReceiptCG,BtnSave }
            };

            foreach (var control in controlFocusMap.Keys)
            {
                control.KeyDown += new KeyEventHandler(Controls_KeyDown);
            }
            #endregion
        }
        private void Controls_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                e.SuppressKeyPress = true;

                if (controlFocusMap.TryGetValue(sender as Control, out Control nextControl))
                {
                    nextControl.Focus();
                }
            }
        }

        #region Textbox BackColor Function
        private void TextBox_Enter(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox.BackColor = Color.NavajoWhite;
            }
        }
        private void TextBox_Leave(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox.BackColor = Color.White;
            }
        }
        private void cmbIntPerc_Enter(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                comboBox.BackColor = Color.NavajoWhite;
            }
        }
        private void cmbIntPerc_Leave(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                comboBox.BackColor = Color.White;
            }
        }

        #endregion

        CustomerDetailBLL customer = new CustomerDetailBLL();
        CustomerDetailDAL dal = new CustomerDetailDAL();
        private void Loan_Form_Load(object sender, EventArgs e)
        {
            this.BeginInvoke((Action)(() => txtamount.Focus()));
            cmbNameSalutation.SelectedIndex = 0;
            cmbFnameTitle.SelectedIndex = 0;
            cmbItemtype.SelectedIndex = 0;
            cmbfind.SelectedIndex = 0;
            btnEdit.Enabled = false;
            btnPrint.Enabled = false;
            btnPrintAgreement.Enabled = false;
            btnPrintBackSide.Enabled = false;
            BtnDelete.Enabled = false;
            NewRecId();
            NewRecNo();
            NewLoanNo();
            GetTodayBalance();
            GetCompanyBalance();
            double a = 1;
            while (a <= 5)

            {
                cmbIntPerc.Items.Add(a);
                a += 0.5;
            }
            SetupDgvOverallItemDet();

            #region DGVs Load Location 
            dgvPhoneNo.Location = new Point(209, 627);
            dgvName.Location = new Point(209, 627);
            dgvFName.Location = new Point(209, 627);
            dgvAddress.Location = new Point(209, 627);
            dgvTown.Location = new Point(209, 627);
            dgvTaluk.Location = new Point(209, 627);
            dgvDistrict.Location = new Point(209, 627);
            dgvItemDetail.Location = new Point(209, 627);
            dgvIdentification.Location = new Point(209, 627);
            pctFind.Location = new Point(209, 627);
            #endregion
        }

        #region Phone No
        private void dgvPhoneNo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvPhoneNo.Rows.Count)
            {
                DataGridViewRow selectedRow = dgvPhoneNo.Rows[e.RowIndex];

                txtCustomerID.Text = selectedRow.Cells["CustomerID"].Value?.ToString();
                txtname.Text = selectedRow.Cells["CustomerName"].Value?.ToString();
                cmbNameSalutation.Text = selectedRow.Cells["Salutation"].Value?.ToString();
                cmbFnameTitle.Text = selectedRow.Cells["FHType"].Value?.ToString();
                txtFName.Text = selectedRow.Cells["FHName"].Value?.ToString();
                txtAddress.Text = selectedRow.Cells["Address"].Value?.ToString();
                txtTown.Text = selectedRow.Cells["Town"].Value?.ToString();
                txtDistrict.Text = selectedRow.Cells["District"].Value?.ToString();
                txtTaluk.Text = selectedRow.Cells["Taluk"].Value?.ToString();
                txtOccupation.Text = selectedRow.Cells["occupation"].Value?.ToString();
                txtPhone.Text = selectedRow.Cells["phone"].Value?.ToString();

                string customerName = txtname.Text.Trim();
                GetCustomerDetails(customerName);
                GetLoansForCustomer();
                GetCustomerBalance();

                cmbItemtype.Focus();
                dgvPhoneNo.Location = new Point(209, 627);
            }
        }
        private void GetCustomerDetails(string customerName)
        {
            string query = "SELECT Salutation, FHType, FHName, Address, Town FROM CustomerMaster WHERE CustomerName = @CustomerName";

            string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerName", customerName);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cmbNameSalutation.Text = reader["Salutation"].ToString();
                            cmbFnameTitle.Text = reader["FHType"].ToString();
                            txtFName.Text = reader["FHName"].ToString();
                            txtAddress.Text = reader["Address"].ToString();
                            txtTown.Text = reader["Town"].ToString();
                        }
                    }
                }
            }
        }
        private void GetLoansForCustomer()
        {
            string customerID = txtCustomerID.Text.Trim();
            if (string.IsNullOrEmpty(customerID)) return;

            string query = "SELECT LoanNo, LoanDate, Amount, ID FROM Loan WHERE Status = 'L' AND CustomerId = @CustomerId";

            string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", customerID);

                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable loanTable = new DataTable();
                    adapter.Fill(loanTable);

                    dgvLoanOld.DataSource = loanTable;
                    FlexLoanOldAlign();
                }
            }
        }
        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            string keywords = txtPhone.Text;
            if (!string.IsNullOrEmpty(keywords))
            {
                DataTable dt = dal.SearchPhoneNo(keywords);
                dgvPhoneNo.DataSource = dt;

                dgvPhoneNo.Visible = dt.Rows.Count > 0;
            }
            else
            {
                //DataTable dt = dal.Select();
                //dgvPhoneNo.DataSource = dt;
                dgvPhoneNo.Visible = false;
            }
        }
        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                return;
            }
            dgvPhoneNo.Visible = true;
            DataTable dt = dal.Select();
            dgvPhoneNo.DataSource = dt;

            #region DGV Properties
            dgvPhoneNo.Columns["CustomerID"].Visible = false;
            dgvPhoneNo.Columns["Salutation"].Visible = false;
            dgvPhoneNo.Columns["CustomerInitial"].Visible = false;
            dgvPhoneNo.Columns["FHType"].Visible = false;
            dgvPhoneNo.Columns["Address"].Visible = false;
            dgvPhoneNo.Columns["Taluk"].Visible = false;
            dgvPhoneNo.Columns["District"].Visible = false;
            dgvPhoneNo.Columns["Occupation"].Visible = false;
            dgvPhoneNo.Columns["Balance"].Visible = false;
            dgvPhoneNo.Columns["AadhaarNo"].Visible = false;
            #endregion
            dgvPhoneNo.Location = new Point(166, 31);
        }
        private void txtPhone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (dgvPhoneNo.Visible && dgvPhoneNo.Rows.Count > 0)
                {
                    foreach (DataGridViewCell cell in dgvPhoneNo.Rows[0].Cells)
                    {
                        if (cell.Visible)
                        {
                            dgvPhoneNo.CurrentCell = cell;
                            break;
                        }
                    }

                    dgvPhoneNo.Focus();
                }
            }
        }
        private void dgvPhoneNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && dgvPhoneNo.CurrentRow != null)
            {
                DataGridViewRow selectedRow = dgvPhoneNo.CurrentRow;

                txtCustomerID.Text = selectedRow.Cells["CustomerID"].Value?.ToString();
                txtname.Text = selectedRow.Cells["CustomerName"].Value?.ToString();
                cmbNameSalutation.Text = selectedRow.Cells["Salutation"].Value?.ToString();
                cmbFnameTitle.Text = selectedRow.Cells["FHType"].Value?.ToString();
                txtFName.Text = selectedRow.Cells["FHName"].Value?.ToString();
                txtAddress.Text = selectedRow.Cells["Address"].Value?.ToString();
                txtTown.Text = selectedRow.Cells["Town"].Value?.ToString();
                txtDistrict.Text = selectedRow.Cells["District"].Value?.ToString();
                txtTaluk.Text = selectedRow.Cells["Taluk"].Value?.ToString();
                txtOccupation.Text = selectedRow.Cells["Occupation"].Value?.ToString();
                txtPhone.Text = selectedRow.Cells["Phone"].Value?.ToString();

                dgvPhoneNo.Visible = false;
                string customerName = txtname.Text.Trim();
                GetCustomerDetails(customerName);
                GetLoansForCustomer();
                GetCustomerBalance();

                cmbItemtype.Focus();
                e.SuppressKeyPress = true;
            }
        }
        #endregion

        #region Customer Name 
        private void txtname_KeyPress(object sender, KeyPressEventArgs e)
        {
            dgvName.Visible = true;
            DataTable dt = dal.Select();
            dgvName.DataSource = dt;

            #region DGV Properties
            dgvName.Columns["CustomerID"].Visible = false;
            dgvName.Columns["Salutation"].Visible = false;
            dgvName.Columns["CustomerInitial"].Visible = false;
            dgvName.Columns["phone"].Visible = false;
            dgvName.Columns["FHName"].Visible = false;
            dgvName.Columns["Town"].Visible = false;
            dgvName.Columns["FHType"].Visible = false;
            dgvName.Columns["Address"].Visible = false;
            dgvName.Columns["Taluk"].Visible = false;
            dgvName.Columns["District"].Visible = false;
            dgvName.Columns["Occupation"].Visible = false;
            dgvName.Columns["Balance"].Visible = false;
            dgvName.Columns["AadhaarNo"].Visible = false;
            #endregion
            dgvName.Location = new Point(166, 65);
        }
        private void txtname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (dgvName.Visible && dgvName.Rows.Count > 0)
                {
                    foreach (DataGridViewCell cell in dgvName.Rows[0].Cells)
                    {
                        if (cell.Visible)
                        {
                            dgvName.CurrentCell = cell;
                            break;
                        }
                    }
                    dgvName.Focus();
                }
            }
        }
        private void txtname_TextChanged(object sender, EventArgs e)
        {
            string keywords = txtname.Text;
            if (!string.IsNullOrEmpty(keywords))
            {
                DataTable dt = dal.SearchName(keywords);
                dgvName.DataSource = dt;
                dgvName.Visible = dt.Rows.Count > 0;
            }
            else
            {
                DataTable dt = dal.Select();
                dgvName.DataSource = dt;
            }
        }
        private void dgvName_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvName.Rows.Count)
            {
                DataGridViewRow selectedRow = dgvName.Rows[e.RowIndex];

                txtname.Text = selectedRow.Cells["CustomerName"].Value?.ToString();

            }
            txtFName.Focus();
            dgvName.Location = new Point(209, 627);
        }
        private void dgvName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && dgvName.CurrentRow != null)
            {
                DataGridViewRow selectedRow = dgvName.CurrentRow;

                txtname.Text = selectedRow.Cells["CustomerName"].Value?.ToString();

                dgvName.Visible = false;
                txtFName.Focus();
                e.SuppressKeyPress = true;
            }
        }
        #endregion

        #region Father Name
        private void txtFName_KeyPress(object sender, KeyPressEventArgs e)
        {
            dgvFName.Visible = true;
            DataTable dt = dal.Select();
            dgvFName.DataSource = dt;

            #region DGV Properties
            dgvFName.Columns["CustomerID"].Visible = false;
            dgvFName.Columns["Salutation"].Visible = false;
            dgvFName.Columns["CustomerInitial"].Visible = false;
            dgvFName.Columns["phone"].Visible = false;
            dgvFName.Columns["CustomerName"].Visible = false;
            dgvFName.Columns["Town"].Visible = false;
            dgvFName.Columns["FHType"].Visible = false;
            dgvFName.Columns["Address"].Visible = false;
            dgvFName.Columns["Taluk"].Visible = false;
            dgvFName.Columns["District"].Visible = false;
            dgvFName.Columns["Occupation"].Visible = false;
            dgvFName.Columns["Balance"].Visible = false;
            dgvFName.Columns["AadhaarNo"].Visible = false;
            #endregion
            dgvFName.Location = new Point(163, 98);
        }
        private void txtFName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (dgvFName.Visible && dgvFName.Rows.Count > 0)
                {
                    foreach (DataGridViewCell cell in dgvFName.Rows[0].Cells)
                    {
                        if (cell.Visible)
                        {
                            dgvFName.CurrentCell = cell;
                            break;
                        }
                    }
                    dgvFName.Focus();
                }
            }
        }
        private void txtFName_TextChanged(object sender, EventArgs e)
        {
            string keywords = txtFName.Text;
            if (!string.IsNullOrEmpty(keywords))
            {
                DataTable dt = dal.SearchFName(keywords);
                dgvFName.DataSource = dt;
                dgvFName.Visible = dt.Rows.Count > 0;
            }
            else
            {
                DataTable dt = dal.Select();
                dgvFName.DataSource = dt;
            }
        }
        private void dgvFName_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvFName.Rows.Count)
            {
                DataGridViewRow selectedRow = dgvFName.Rows[e.RowIndex];

                txtFName.Text = selectedRow.Cells["FHName"].Value?.ToString();

            }
            txtAddress.Focus();
            dgvFName.Location = new Point(209, 627);
        }
        private void dgvFName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && dgvFName.CurrentRow != null)
            {
                DataGridViewRow selectedRow = dgvFName.CurrentRow;

                txtFName.Text = selectedRow.Cells["FHName"].Value?.ToString();

                dgvFName.Visible = false;
                txtAddress.Focus();
                e.SuppressKeyPress = true;
            }
        }
        #endregion

        #region Address
        private void txtAddress_TextChanged_1(object sender, EventArgs e)
        {
            string keywords = txtAddress.Text;
            if (!string.IsNullOrEmpty(keywords))
            {
                DataTable dt = dal.SearchAddress(keywords);
                dgvAddress.DataSource = dt;
                dgvAddress.Visible = dt.Rows.Count > 0;
            }
            else
            {
                DataTable dt = dal.Select();
                dgvAddress.DataSource = dt;
            }
        }
        private void txtAddress_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            dgvAddress.Visible = true;
            DataTable dt = dal.Select();
            dgvAddress.DataSource = dt;

            #region DGV Properties
            dgvAddress.Columns["CustomerID"].Visible = false;
            dgvAddress.Columns["Salutation"].Visible = false;
            dgvAddress.Columns["CustomerInitial"].Visible = false;
            dgvAddress.Columns["phone"].Visible = false;
            dgvAddress.Columns["CustomerName"].Visible = false;
            dgvAddress.Columns["Town"].Visible = false;
            dgvAddress.Columns["FHType"].Visible = false;
            dgvAddress.Columns["FHName"].Visible = false;
            dgvAddress.Columns["Taluk"].Visible = false;
            dgvAddress.Columns["District"].Visible = false;
            dgvAddress.Columns["Occupation"].Visible = false;
            dgvAddress.Columns["Balance"].Visible = false;
            dgvAddress.Columns["AadhaarNo"].Visible = false;
            #endregion
            dgvAddress.Location = new Point(163, 129);
        }
        private void txtAddress_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (dgvAddress.Visible && dgvAddress.Rows.Count > 0)
                {
                    foreach (DataGridViewCell cell in dgvAddress.Rows[0].Cells)
                    {
                        if (cell.Visible)
                        {
                            dgvAddress.CurrentCell = cell;
                            break;
                        }
                    }
                    dgvAddress.Focus();
                }
            }
        }
        private void dgvAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && dgvAddress.CurrentRow != null)
            {
                DataGridViewRow selectedRow = dgvAddress.CurrentRow;

                txtAddress.Text = selectedRow.Cells["Address"].Value?.ToString();

                dgvAddress.Visible = false;
                txtTown.Focus();
                e.SuppressKeyPress = true;
            }
        }
        private void dgvAddress_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvAddress.Rows.Count)
            {
                DataGridViewRow selectedRow = dgvAddress.Rows[e.RowIndex];

                txtAddress.Text = selectedRow.Cells["Address"].Value?.ToString();

            }
            txtTown.Focus();
            dgvAddress.Location = new Point(209, 627);
        }
        #endregion

        #region Town
        private void txtTown_TextChanged(object sender, EventArgs e)
        {
            string keywords = txtTown.Text;
            if (!string.IsNullOrEmpty(keywords))
            {
                DataTable dt = dal.SearchTown(keywords);
                dgvTown.DataSource = dt;
                dgvTown.Visible = dt.Rows.Count > 0;
            }
            else
            {
                DataTable dt = dal.Select();
                dgvTown.DataSource = dt;
            }
        }
        private void txtTown_KeyPress(object sender, KeyPressEventArgs e)
        {
            dgvTown.Visible = true;
            DataTable dt = dal.Select();
            dgvTown.DataSource = dt;

            #region DGV Properties
            dgvTown.Columns["CustomerID"].Visible = false;
            dgvTown.Columns["Salutation"].Visible = false;
            dgvTown.Columns["CustomerInitial"].Visible = false;
            dgvTown.Columns["phone"].Visible = false;
            dgvTown.Columns["CustomerName"].Visible = false;
            dgvTown.Columns["Address"].Visible = false;
            dgvTown.Columns["FHType"].Visible = false;
            dgvTown.Columns["FHName"].Visible = false;
            dgvTown.Columns["Taluk"].Visible = false;
            dgvTown.Columns["District"].Visible = false;
            dgvTown.Columns["Occupation"].Visible = false;
            dgvTown.Columns["Balance"].Visible = false;
            dgvTown.Columns["AadhaarNo"].Visible = false;
            #endregion
            dgvTown.Location = new Point(163, 161);
        }
        private void txtTown_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (dgvTown.Visible && dgvTown.Rows.Count > 0)
                {
                    foreach (DataGridViewCell cell in dgvTown.Rows[0].Cells)
                    {
                        if (cell.Visible)
                        {
                            dgvTown.CurrentCell = cell;
                            break;
                        }
                    }
                    dgvTown.Focus();
                }
            }
        }
        private void dgvTown_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvTown.Rows.Count)
            {
                DataGridViewRow selectedRow = dgvTown.Rows[e.RowIndex];

                txtTown.Text = selectedRow.Cells["Town"].Value?.ToString();

            }
            dgvTown.Location = new Point(209, 627);
        }
        private void dgvTown_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter && dgvTown.CurrentRow != null)
            {
                DataGridViewRow selectedRow = dgvTown.CurrentRow;

                txtTown.Text = selectedRow.Cells["Town"].Value?.ToString();

                dgvTown.Visible = false;
                txtTown.Focus();
                e.SuppressKeyPress = true;
            }
        }

        #endregion

        #region Taluk
        private void txtTaluk_TextChanged(object sender, EventArgs e)
        {
            string keywords = txtTaluk.Text;
            if (!string.IsNullOrEmpty(keywords))
            {
                DataTable dt = dal.SearchTaluk(keywords);
                dgvTaluk.DataSource = dt;
                dgvTaluk.Visible = dt.Rows.Count > 0;
            }
            else
            {
                DataTable dt = dal.DistinctTaluk();
                dgvTaluk.DataSource = dt;
            }
        }
        private void txtTaluk_KeyPress(object sender, KeyPressEventArgs e)
        {
            dgvTaluk.Visible = true;
            DataTable dt = dal.DistinctTaluk();
            dgvTaluk.DataSource = dt;

            #region DGV Properties
            foreach (DataGridViewColumn column in dgvTaluk.Columns)
            {
                column.Visible = false;
            }
            dgvTaluk.Columns["Taluk"].Visible = true;
            #endregion
            dgvTaluk.Location = new Point(604, 34);
        }
        private void txtTaluk_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (dgvTaluk.Visible && dgvTaluk.Rows.Count > 0)
                {
                    foreach (DataGridViewCell cell in dgvTaluk.Rows[0].Cells)
                    {
                        if (cell.Visible)
                        {
                            dgvTaluk.CurrentCell = cell;
                            break;
                        }
                    }
                    dgvTaluk.Focus();
                }
            }
        }
        private void dgvTaluk_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvTaluk.Rows.Count)
            {
                DataGridViewRow selectedRow = dgvTaluk.Rows[e.RowIndex];

                txtTaluk.Text = selectedRow.Cells["Taluk"].Value?.ToString();

            }
            txtDistrict.Focus();
            dgvTaluk.Location = new Point(209, 627);
        }
        private void dgvTaluk_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && dgvTaluk.CurrentRow != null)
            {
                DataGridViewRow selectedRow = dgvTaluk.CurrentRow;

                txtTaluk.Text = selectedRow.Cells["Taluk"].Value?.ToString();

                dgvTaluk.Visible = false;
                txtDistrict.Focus();
                e.SuppressKeyPress = true;
            }
        }

        #endregion

        #region District
        private void txtDistrict_TextChanged(object sender, EventArgs e)
        {
            string keywords = txtDistrict.Text;
            if (!string.IsNullOrEmpty(keywords))
            {
                DataTable dt = dal.SearchDistrict(keywords);
                dgvDistrict.DataSource = dt;
                dgvDistrict.Visible = dt.Rows.Count > 0;
            }
            else
            {
                DataTable dt = dal.DistinctDistrict();
                dgvDistrict.DataSource = dt;
            }
        }
        private void txtDistrict_KeyPress(object sender, KeyPressEventArgs e)
        {
            dgvDistrict.Visible = true;
            DataTable dt = dal.DistinctDistrict();
            dgvDistrict.DataSource = dt;

            #region DGV Properties

            foreach (DataGridViewColumn column in dgvDistrict.Columns)
            {
                column.Visible = false;
            }
            dgvDistrict.Columns["District"].Visible = true;
            #endregion
            dgvDistrict.Location = new Point(603, 65);
        }
        private void txtDistrict_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (dgvDistrict.Visible && dgvDistrict.Rows.Count > 0)
                {
                    foreach (DataGridViewCell cell in dgvDistrict.Rows[0].Cells)
                    {
                        if (cell.Visible)
                        {
                            dgvDistrict.CurrentCell = cell;
                            break;
                        }
                    }
                    dgvDistrict.Focus();
                }
            }
        }
        private void dgvDistrict_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvDistrict.Rows.Count)
            {
                DataGridViewRow selectedRow = dgvDistrict.Rows[e.RowIndex];

                txtDistrict.Text = selectedRow.Cells["District"].Value?.ToString();

            }
            txtOccupation.Focus();
            dgvDistrict.Location = new Point(209, 627);
        }
        private void dgvDistrict_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && dgvDistrict.CurrentRow != null)
            {
                DataGridViewRow selectedRow = dgvDistrict.CurrentRow;

                txtDistrict.Text = selectedRow.Cells["District"].Value?.ToString();

                dgvDistrict.Visible = false;
                txtOccupation.Focus();
                e.SuppressKeyPress = true;
            }
        }
        #endregion

        #region Item Names Detail
        ItemDetailDAL itemdal = new ItemDetailDAL();
        private void txtItemDetails_TextChanged(object sender, EventArgs e)
        {
            string keywords = txtItemDetails.Text;
            if (!string.IsNullOrEmpty(keywords))
            {
                DataTable dt = itemdal.SearchItemNames(keywords);
                dgvItemDetail.DataSource = dt;
                dgvItemDetail.Visible = dt.Rows.Count > 0;
            }
            else
            {
                DataTable dt = itemdal.Select();
                dgvItemDetail.DataSource = dt;
            }
        }

        private void txtItemDetails_KeyPress(object sender, KeyPressEventArgs e)
        {
            dgvItemDetail.Visible = true;
            DataTable dt = itemdal.Select();
            dgvItemDetail.DataSource = dt;

            #region DGV Properties
            dgvItemDetail.Columns["ItemNo"].Visible = false;
            dgvItemDetail.Columns["ItemCode"].Visible = false;
            dgvItemDetail.Columns["ItemType"].Visible = false;
            #endregion
            dgvItemDetail.Location = new Point(167, 415);
        }

        private void txtItemDetails_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (dgvItemDetail.Visible && dgvItemDetail.Rows.Count > 0)
                {
                    foreach (DataGridViewCell cell in dgvItemDetail.Rows[0].Cells)
                    {
                        if (cell.Visible)
                        {
                            dgvItemDetail.CurrentCell = cell;
                            break;
                        }
                    }
                    dgvItemDetail.Focus();
                }
            }
        }

        private void dgvItemDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvItemDetail.Rows.Count)
            {
                DataGridViewRow selectedRow = dgvItemDetail.Rows[e.RowIndex];

                txtItemDetails.Text = selectedRow.Cells["ItemName"].Value?.ToString();

            }
            txtSeal.Focus();
            dgvItemDetail.Location = new Point(209, 627);
        }

        private void dgvItemDetail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && dgvItemDetail.CurrentRow != null)
            {
                DataGridViewRow selectedRow = dgvItemDetail.CurrentRow;

                txtItemDetails.Text = selectedRow.Cells["ItemName"].Value?.ToString();

                dgvItemDetail.Visible = false;
                txtSeal.Focus();
                e.SuppressKeyPress = true;
            }
        }
        #endregion

        #region Form Load Functions
        public void NewRecId()
        {
            string query = "SELECT ISNULL(MAX(IntrestId), 0) + 1 FROM IntrestCollection";

            object result = dal.ExecuteScalar(query);
            lblintId.Text = (result != null) ? result.ToString() : "1";
        }
        public void NewRecNo()
        {
            string query = "SELECT ISNULL(MAX(IntrestNo), 0) + 1 FROM IntrestCollection";

            object result = dal.ExecuteScalar(query);
            lblintno.Text = (result != null) ? result.ToString() : "1";
        }
        private void NewLoanNo()
        {
            long lastLoanNo;
            string loanNoText;

            string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT LoanNoText, LastLoanNo FROM CompanyMaster WHERE CompanyId = 1";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            loanNoText = reader["LoanNoText"].ToString();
                            lastLoanNo = reader["LastLoanNo"] != DBNull.Value ? Convert.ToInt64(reader["LastLoanNo"]) : 0;

                            if (lastLoanNo == 0)
                            {
                                txtloanno.Text = loanNoText + "1";
                            }
                            else
                            {
                                txtloanno.Text = loanNoText + (lastLoanNo + 1).ToString();
                            }
                        }
                    }
                }
            }
        }
        public void GetTodayBalance()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
            string query = "SELECT SUM(Amount) FROM Loan WHERE LoanDate = @LoanDate";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@LoanDate", dtpLoanDate.Value.Date);
                        object result = cmd.ExecuteScalar();
                        lblTodayBalance.Text = result == DBNull.Value || result == null ? "0" : result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching balance: " + ex.Message);
                }
            }
        }
        public void GetCompanyBalance()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
            string query = "SELECT SUM(Amount) FROM Loan WHERE Companyid = @CompanyID AND LoanDate = @LoanDate";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CompanyID", 1);
                        cmd.Parameters.AddWithValue("@LoanDate", dtpLoanDate.Value.Date);
                        object result = cmd.ExecuteScalar();

                        lblCompanyBalance.Text = result == DBNull.Value || result == null ? "0" : result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching company balance: " + ex.Message);
                }
            }
        }
        private void GetCustomerBalance()
        {
            string customerID = txtCustomerID.Text.Trim();
            if (string.IsNullOrEmpty(customerID)) return;

            string query = "SELECT Balance FROM CustomerMaster WHERE CustomerId = @CustomerId";

            string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", customerID);

                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        lblBalance.Text = result.ToString();
                    }
                    else
                    {
                        lblBalance.Text = "0.00";
                    }
                }
            }
        }
        private void timerlblLoanAmnt_Tick(object sender, EventArgs e)
        {
            lblGivingAmount.Visible = !lblGivingAmount.Visible;
        }
        private void dtpLoanDate_ValueChanged(object sender, EventArgs e)
        {
            dtpRedeemDate.Value = dtpLoanDate.Value.AddMonths(1).AddDays(-1);

            GetTodayBalance();
            GetCompanyBalance();
        }
        #endregion

        #region Save Functions
        private void CustomerCreation()
        {
            string checkCustomerQuery = @"SELECT CustomerID FROM CustomerMaster WHERE Salutation = @Salutation AND CustomerName = @CustomerName 
                                        AND FHType = @FHType AND FHName = @FHName AND Address = @Address AND Town = @Town";

            SqlParameter[] parameters = new SqlParameter[]
            {
             new SqlParameter("@Salutation", cmbNameSalutation.Text.Trim()),
             new SqlParameter("@CustomerName", txtname.Text.Trim()),
             new SqlParameter("@FHType", cmbFnameTitle.Text.Trim()),
             new SqlParameter("@FHName", txtFName.Text.Trim()),
             new SqlParameter("@Address", txtAddress.Text.Trim()),
             new SqlParameter("@Town", txtTown.Text.Trim())
            };

            object existingCustomerId = dal.ExecuteScalar(checkCustomerQuery, parameters);

            if (existingCustomerId != null)
            {
                txtCustomerID.Text = existingCustomerId.ToString();
                return;
            }

            string getMaxIdQuery = "SELECT ISNULL(MAX(CustomerID), 0) + 1 FROM CustomerMaster";
            int newCustomerId = Convert.ToInt32(dal.ExecuteScalar(getMaxIdQuery));
            txtCustomerID.Text = newCustomerId.ToString();

            customer.CustomerID = newCustomerId;
            customer.Salutation = cmbNameSalutation.Text.Trim();
            customer.CustomerName = txtname.Text.Trim();
            customer.FHType = cmbFnameTitle.Text.Trim();
            customer.FHName = txtFName.Text.Trim();
            customer.Address = txtAddress.Text.Trim();
            customer.Town = txtTown.Text.Trim();
            customer.Taluk = txtTaluk.Text.Trim();
            customer.District = txtDistrict.Text.Trim();
            customer.occupation = txtOccupation.Text.Trim();
            customer.phone = txtPhone.Text.Trim();
            customer.AadhaarNo = txtaadhaarno.Text.Trim();

            bool success = dal.Insert(customer);
        }
        public void Store()
        {
            string query = @"
                           INSERT INTO Loan 
                           (ID, LoanNo, CompanyId, OldLoanNo, LoanDate, Amount, ItemType, CustomerId, Gram, TotalQty, TotalValue, RedeemDate, IntPaidUpto, IntPending, GurName, GurOccupation, IntPerc, IntAmt, AcctIntPerc, ReceiptChg, GivenAmount, BalanceAmt, Status, LoanMark) 
                           VALUES 
                           (@ID, @LoanNo, @CompanyId, @OldLoanNo, @LoanDate, @Amount, @ItemType, @CustomerId, @Gram, @TotalQty, @TotalValue, @RedeemDate, @IntPaidUpto, @IntPending, @GurName, @GurOccupation, @IntPerc, @IntAmt, @AcctIntPerc, @ReceiptChg, @GivenAmount, @BalanceAmt, @Status, @LoanMark)";

            string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                int newLoanId = GenId("Loan", "ID");

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", newLoanId);
                    command.Parameters.AddWithValue("@LoanNo", txtloanno.Text.Trim());
                    command.Parameters.AddWithValue("@CompanyId", 1);
                    command.Parameters.AddWithValue("@OldLoanNo", txtOldLoanNo.Text.Trim());
                    command.Parameters.AddWithValue("@LoanDate", dtpLoanDate.Value.ToString("yyyy-MM-dd")); // Ensure date is properly formatted
                    command.Parameters.AddWithValue("@Amount", Convert.ToDouble(txtamount.Text.Trim()));
                    command.Parameters.AddWithValue("@ItemType", cmbItemtype.Text.Trim());
                    command.Parameters.AddWithValue("@CustomerId", Convert.ToInt32(txtCustomerID.Text.Trim()));
                    command.Parameters.AddWithValue("@Gram", Convert.ToDouble(txtkg.Text.Trim()));

                    int TQty = 0;
                    for (int i = 0; i < dgvOverallItemDet.Rows.Count; i++)
                    {
                        if (dgvOverallItemDet.Rows[i].Cells[3].Value != null)
                        {
                            TQty += Convert.ToInt32(dgvOverallItemDet.Rows[i].Cells[3].Value);
                        }
                    }
                    lblTotalQty.Text = TQty.ToString();
                    command.Parameters.AddWithValue("@TotalQty", TQty);
                    command.Parameters.AddWithValue("@TotalValue", Convert.ToDouble(txtValue.Text.Trim()));
                    command.Parameters.AddWithValue("@RedeemDate", dtpRedeemDate.Value.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@IntPaidUpto", dtpLoanDate.Value.AddMonths(1).ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@IntPending", 0);
                    command.Parameters.AddWithValue("@GurName", txtGuName.Text.Trim());
                    command.Parameters.AddWithValue("@GurOccupation", txtGuOccupation.Text.Trim());

                    long AccIntPerc = 0;
                    using (SqlCommand cmd = new SqlCommand("SELECT AcctIntPerc FROM CompanyMaster WHERE CompanyId = @CompanyId", connection))
                    {
                        cmd.Parameters.AddWithValue("@CompanyId", 1);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read() && !reader.IsDBNull(0))
                            {
                                AccIntPerc = Convert.ToInt64(reader["AcctIntPerc"]);
                            }
                        }
                    }

                    command.Parameters.AddWithValue("@IntPerc", Convert.ToDouble(cmbIntPerc.Text.Trim()));
                    command.Parameters.AddWithValue("@IntAmt", Convert.ToDouble(txtInterest.Text.Trim()));
                    command.Parameters.AddWithValue("@AcctIntPerc", AccIntPerc);
                    command.Parameters.AddWithValue("@ReceiptChg", Convert.ToDouble(txtReceiptCG.Text.Trim()));
                    command.Parameters.AddWithValue("@GivenAmount", Convert.ToDouble(lblGivingAmount.Text));
                    command.Parameters.AddWithValue("@BalanceAmt", Convert.ToDouble(txtamount.Text.Trim()));
                    command.Parameters.AddWithValue("@Status", "L");
                    command.Parameters.AddWithValue("@LoanMark", txtLoanMark.Text.Trim());

                    command.ExecuteNonQuery();
                }
            }

            //MessageBox.Show("Loan record successfully inserted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public void SaveToLedger()
        {
            decimal amount = Convert.ToDecimal(txtamount.Text);
            decimal interest = Convert.ToDecimal(txtInterest.Text);
            decimal chitpaise = Convert.ToDecimal(txtReceiptCG.Text);
            decimal netAmount = amount - interest - chitpaise;
            string nar = "Loan Issued-" + txtloanno.Text.Trim() + "-" + txtname.Text.Trim();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string deleteQuery = "DELETE FROM LedgerTransaction WHERE RefNo = @RefNo";
                    using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@RefNo", "Loan" + lblid.Text);
                        cmd.ExecuteNonQuery();
                    }

                    LedgerEntry(conn, Convert.ToDateTime(dtpLoanDate.Value), 2, 1, "Loan" + lblid.Text, nar, amount, 0, 1);
                    LedgerEntry(conn, Convert.ToDateTime(dtpLoanDate.Value), 1, 2, "Loan" + lblid.Text, nar, 0, amount, 1);

                    if (interest > 0)
                    {
                        LedgerEntry(conn, Convert.ToDateTime(dtpLoanDate.Value), 3, 1, "Loan" + lblid.Text, nar, 0, interest, 1);
                        LedgerEntry(conn, Convert.ToDateTime(dtpLoanDate.Value), 1, 3, "Loan" + lblid.Text, nar, interest, 0, 1);
                    }

                    if (chitpaise > 0)
                    {
                        LedgerEntry(conn, Convert.ToDateTime(dtpLoanDate.Value), 4, 1, "Loan" + lblid.Text, nar, 0, chitpaise, 1);
                        LedgerEntry(conn, Convert.ToDateTime(dtpLoanDate.Value), 1, 4, "Loan" + lblid.Text, nar, chitpaise, 0, 1);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error while saving to Ledger", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void LedgerEntry(SqlConnection conn, DateTime TDate, int LedId, int DLedId, string RefNo, string Narration, decimal Debit, decimal Credit, int CompanyId)
        {
            try
            {
                int nextTransId = GetNextTransId(conn);
                string insertQuery = @"INSERT INTO LedgerTransaction 
                               (TransId, TransDate, LedgerId, RefNo, Narration, Debit, Credit, DetsId, CompanyId) 
                               VALUES (@TransId, @TransDate, @LedgerId, @RefNo, @Narration, @Debit, @Credit, @DetsId, @CompanyId)";
                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@TransId", nextTransId);
                    cmd.Parameters.AddWithValue("@TransDate", TDate);
                    cmd.Parameters.AddWithValue("@LedgerId", LedId);
                    cmd.Parameters.AddWithValue("@RefNo", RefNo);
                    cmd.Parameters.AddWithValue("@Narration", Narration);
                    cmd.Parameters.AddWithValue("@Debit", Debit);
                    cmd.Parameters.AddWithValue("@Credit", Credit);
                    cmd.Parameters.AddWithValue("@DetsId", DLedId);
                    cmd.Parameters.AddWithValue("@CompanyId", CompanyId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error in LedgerEntry", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void GridToData()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlTransaction transaction = dbcon.BeginTransaction();

                try
                {
                    int loanDetailId;
                    using (SqlCommand cmd = new SqlCommand("SELECT COALESCE(MAX(LoanDetailId), 0) + 1 FROM LoanDetails", dbcon, transaction))
                    {
                        var result = cmd.ExecuteScalar();
                        loanDetailId = result != null ? Convert.ToInt32(result) : 1;
                    }

                    lblDetailId.Text = loanDetailId.ToString();

                    for (int i = 0; i < dgvOverallItemDet.Rows.Count; i++)
                    {
                        using (SqlCommand insertLoanDetailCmd = new SqlCommand("INSERT INTO LoanDetails (Id, SNo, ItemCode, Seal, TotalQty, Description, Gram) VALUES (@Id, @SNo, @ItemCode, @Seal, @TotalQty, @Description, @Gram)", dbcon, transaction))
                        {

                            //insertLoanDetailCmd.Parameters.AddWithValue("@LoanDetailId", loanDetailId);
                            insertLoanDetailCmd.Parameters.AddWithValue("@Id", lblid.Text);
                            insertLoanDetailCmd.Parameters.AddWithValue("@SNo", Convert.ToDecimal(dgvOverallItemDet.Rows[i].Cells[0].Value)); // Change to decimal to match numeric type

                            string itemName = Trim(dgvOverallItemDet.Rows[i].Cells[1].Value?.ToString() ?? string.Empty);
                            decimal itemCode = 0;

                            using (SqlCommand itemCmd = new SqlCommand("SELECT ItemCode FROM ItemDetail WHERE ItemName = @ItemName", dbcon, transaction))
                            {
                                itemCmd.Parameters.AddWithValue("@ItemName", itemName);
                                object result = itemCmd.ExecuteScalar();
                                if (result == null)
                                {
                                    using (SqlCommand maxItemCodeCmd = new SqlCommand("SELECT COALESCE(MAX(ItemCode), 0) + 1 FROM ItemDetail", dbcon, transaction))
                                    {
                                        var itemCodeResult = maxItemCodeCmd.ExecuteScalar();
                                        itemCode = itemCodeResult != null ? Convert.ToDecimal(itemCodeResult) : 1; // Default to 1 if no items exist
                                    }

                                    using (SqlCommand insertItemCmd = new SqlCommand("INSERT INTO ItemDetail (ItemNo, ItemCode, ItemType, ItemName) VALUES (@ItemNo, @ItemCode, @ItemType, @ItemName)", dbcon, transaction))
                                    {
                                        insertItemCmd.Parameters.AddWithValue("@ItemNo", itemCode);
                                        insertItemCmd.Parameters.AddWithValue("@ItemCode", itemCode);
                                        insertItemCmd.Parameters.AddWithValue("@ItemType", Trim(cmbItemtype.Text));
                                        insertItemCmd.Parameters.AddWithValue("@ItemName", itemName);
                                        insertItemCmd.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    itemCode = Convert.ToDecimal(result);
                                }

                                dgvOverallItemDet.Rows[i].Cells[4].Value = itemCode;
                            }

                            insertLoanDetailCmd.Parameters.AddWithValue("@ItemCode", itemCode);
                            insertLoanDetailCmd.Parameters.AddWithValue("@Seal", Trim(dgvOverallItemDet.Rows[i].Cells[2].Value?.ToString() ?? string.Empty));
                            insertLoanDetailCmd.Parameters.AddWithValue("@TotalQty", Val(dgvOverallItemDet.Rows[i].Cells[3].Value));
                            insertLoanDetailCmd.Parameters.AddWithValue("@Description", Trim(dgvOverallItemDet.Rows[i].Cells[5].Value?.ToString() ?? string.Empty));
                            insertLoanDetailCmd.Parameters.AddWithValue("@Gram", Val(dgvOverallItemDet.Rows[i].Cells[6].Value));

                            insertLoanDetailCmd.ExecuteNonQuery();
                            loanDetailId++;
                        }
                    }

                    transaction.Commit();
                    //MessageBox.Show("Data saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private string Trim(string value)
        {
            return value?.Trim();
        }
        private decimal Val(object value)
        {
            return value != null ? Convert.ToDecimal(value) : 0;
        }
        public int GenId(string tableName, string fieldName)
        {
            int newId = 1;
            string query = $"SELECT MAX({fieldName}) + 1 FROM {tableName}";

            using (SqlConnection conn = new SqlConnection(myconnstrng))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    object result = cmd.ExecuteScalar();
                    newId = result == DBNull.Value ? 1 : Convert.ToInt32(result);
                }
            }
            return newId;
        }
        private int GetNextTransId(SqlConnection conn)
        {
            string query = "SELECT ISNULL(MAX(TransId), 0) + 1 FROM LedgerTransaction";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        private bool CheckLog()
        {

            if (string.IsNullOrWhiteSpace(txtloanno.Text))
            {
                MessageBox.Show("Enter Loan No", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtloanno.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtname.Text))
            {
                MessageBox.Show("Enter Customer Name", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtname.Focus();
                return false;
            }

            if (dgvOverallItemDet.Rows.Cast<DataGridViewRow>().Where(row => !row.IsNewRow).Count() < 1)
            {
                MessageBox.Show("Enter Item Details", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtItemDetails.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtamount.Text))
            {
                MessageBox.Show("Enter Amount", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtamount.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtInterest.Text))
            {
                MessageBox.Show("Enter Interest Amount", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtInterest.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtValue.Text))
            {
                MessageBox.Show("Enter Total Value", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtValue.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtReceiptCG.Text))
            {
                MessageBox.Show("Enter Receipt Charge", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtReceiptCG.Focus();
                return false;
            }

            return true;
        }
        #endregion

        #region Save Button
        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
        private void BtnSave_Click(object sender, EventArgs e)
        {
            txtLoanMark.Text = txtLoanMark.Text.ToUpper();

            if (CheckLog())
            {
                if (!SaveFlg)
                {
                    CustomerCreation();

                    string loanNo = txtloanno.Text.Trim();
                    int companyId = 1;

                    string query = "SELECT * FROM Loan WHERE loanno = @LoanNo AND companyid = @CompanyId";

                    using (SqlConnection conn = new SqlConnection(myconnstrng))
                    {
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@LoanNo", loanNo);
                            cmd.Parameters.AddWithValue("@CompanyId", companyId);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (!reader.HasRows)
                                {
                                    reader.Close();
                                    int newLoanId = GenId("Loan", "ID");
                                    lblid.Text = newLoanId.ToString();

                                    Store();
                                    SaveToLedger();
                                    GridToData();

                                    MessageBox.Show("One Record Inserted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    int i = 0;
                                    while (i < txtloanno.Text.Length && !char.IsDigit(txtloanno.Text[i]))
                                    {
                                        i++;
                                    }
                                    int genloanno = i < txtloanno.Text.Length ? Convert.ToInt32(txtloanno.Text.Substring(i)) : 0;

                                    string companyQuery = "SELECT * FROM CompanyMaster WHERE CompanyId = @CompanyId";
                                    using (SqlCommand companyCmd = new SqlCommand(companyQuery, conn))
                                    {
                                        companyCmd.Parameters.AddWithValue("@CompanyId", companyId);

                                        using (SqlDataReader companyReader = companyCmd.ExecuteReader())
                                        {
                                            if (companyReader.Read())
                                            {
                                                int firstLoanNo = companyReader["FirstLoanNo"] == DBNull.Value ? 0 : Convert.ToInt32(companyReader["FirstLoanNo"]);
                                                int lastLoanNo = companyReader["LastLoanNo"] == DBNull.Value ? 0 : Convert.ToInt32(companyReader["LastLoanNo"]);

                                                if (firstLoanNo == 0)
                                                {
                                                    companyReader.Close();
                                                    string updateFirstLoanNoQuery = "UPDATE CompanyMaster SET FirstLoanNo = @FirstLoanNo WHERE CompanyId = @CompanyId";
                                                    using (SqlCommand updateFirstCmd = new SqlCommand(updateFirstLoanNoQuery, conn))
                                                    {
                                                        updateFirstCmd.Parameters.AddWithValue("@FirstLoanNo", genloanno);
                                                        updateFirstCmd.Parameters.AddWithValue("@CompanyId", companyId);
                                                        updateFirstCmd.ExecuteNonQuery();
                                                    }
                                                }

                                                companyReader.Close();
                                                string updateLastLoanNoQuery = "UPDATE CompanyMaster SET LastLoanNo = @LastLoanNo WHERE CompanyId = @CompanyId";
                                                using (SqlCommand updateLastCmd = new SqlCommand(updateLastLoanNoQuery, conn))
                                                {
                                                    updateLastCmd.Parameters.AddWithValue("@LastLoanNo", genloanno);
                                                    updateLastCmd.Parameters.AddWithValue("@CompanyId", companyId);
                                                    updateLastCmd.ExecuteNonQuery();
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Loan Number already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    txtloanno.Focus();
                                }
                            }
                        }
                    }
                }
                else if (SaveFlg)
                {
                    CustomerCreationEdit();

                    using (SqlConnection conn = new SqlConnection(myconnstrng))
                    {
                        conn.Open();

                        string updateQuery = "SELECT * FROM Loan WHERE Id = @LoanId";
                        using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@LoanId", Convert.ToInt32(lblid.Text));

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    reader.Close();
                                    UpdateLoanTable(conn);
                                    SaveToLedger();

                                    string deleteLoanDetailsQuery = "DELETE FROM LoanDetails WHERE Id = @LoanId";
                                    using (SqlCommand deleteCmd = new SqlCommand(deleteLoanDetailsQuery, conn))
                                    {
                                        deleteCmd.Parameters.AddWithValue("@LoanId", Convert.ToInt32(lblid.Text));
                                        deleteCmd.ExecuteNonQuery();
                                    }

                                    GridToData();
                                    MessageBox.Show("One Record Updated", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    SaveFlg = false;
                                }
                            }
                        }

                        string deleteInterestQuery = "DELETE FROM IntrestCollection WHERE refno = 'LN" + lblid.Text + "'";
                        using (SqlCommand deleteCmd = new SqlCommand(deleteInterestQuery, conn))
                        {
                            deleteCmd.ExecuteNonQuery();
                        }

                        string insertInterestQuery = "INSERT INTO IntrestCollection (IntrestID) VALUES (@IntrestID)";
                        using (SqlCommand insertCmd = new SqlCommand(insertInterestQuery, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@IntrestID", Convert.ToInt32(lblintId.Text));
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }

                DialogResult result = MessageBox.Show("Do you want to Print Report?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    btnPrint_Click(sender, e);
                }
            }
            else
            {
                MessageBox.Show("Record could not be saved due to validation issues.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Update Functions (Customer and Loan Table)
        private void UpdateLoanTable(SqlConnection conn)
        {
            string updateQuery = @"UPDATE Loan SET LoanNo = @LoanNo, OldLoanNo = @OldLoanNo,LoanDate = @LoanDate,Amount = @Amount,
                                    ItemType = @ItemType,CustomerId = @CustomerId, Gram = @Gram, TotalQty = @TotalQty,TotalValue = @TotalValue,
                                    RedeemDate = @RedeemDate,IntPaidUpto = @IntPaidUpto,IntPending = @IntPending,GurName = @GurName,
                                    GurOccupation = @GurOccupation, IntPerc = @IntPerc,IntAmt = @IntAmt, AcctIntPerc = @AcctIntPerc,
                                    ReceiptChg = @ReceiptChg,GivenAmount = @GivenAmount,BalanceAmt = @BalanceAmt,Status = @Status,
                                    LoanMark = @LoanMark WHERE ID = @LoanId";

            using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
            {
                updateCmd.Parameters.AddWithValue("@LoanNo", txtloanno.Text.Trim());
                updateCmd.Parameters.AddWithValue("@OldLoanNo", txtOldLoanNo.Text.Trim());
                updateCmd.Parameters.AddWithValue("@LoanDate", dtpLoanDate.Value.ToString("yyyy-MM-dd"));
                updateCmd.Parameters.AddWithValue("@Amount", Convert.ToDouble(txtamount.Text.Trim()));
                updateCmd.Parameters.AddWithValue("@ItemType", cmbItemtype.Text.Trim());
                updateCmd.Parameters.AddWithValue("@CustomerId", Convert.ToInt32(txtCustomerID.Text.Trim()));
                updateCmd.Parameters.AddWithValue("@Gram", Convert.ToDouble(txtkg.Text.Trim()));

                int TQty = 0;
                for (int i = 0; i < dgvOverallItemDet.Rows.Count; i++)
                {
                    if (dgvOverallItemDet.Rows[i].Cells[3].Value != null)
                    {
                        TQty += Convert.ToInt32(dgvOverallItemDet.Rows[i].Cells[3].Value);
                    }
                }
                updateCmd.Parameters.AddWithValue("@TotalQty", TQty);
                updateCmd.Parameters.AddWithValue("@TotalValue", Convert.ToDouble(txtValue.Text.Trim()));
                updateCmd.Parameters.AddWithValue("@RedeemDate", dtpRedeemDate.Value.ToString("yyyy-MM-dd"));
                updateCmd.Parameters.AddWithValue("@IntPaidUpto", dtpLoanDate.Value.AddMonths(1).ToString("yyyy-MM-dd"));
                updateCmd.Parameters.AddWithValue("@IntPending", 0);
                updateCmd.Parameters.AddWithValue("@GurName", txtGuName.Text.Trim());
                updateCmd.Parameters.AddWithValue("@GurOccupation", txtGuOccupation.Text.Trim());

                long AccIntPerc = 0;
                using (SqlCommand cmd = new SqlCommand("SELECT AcctIntPerc FROM CompanyMaster WHERE CompanyId = @CompanyId", conn))
                {
                    cmd.Parameters.AddWithValue("@CompanyId", 1);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read() && !reader.IsDBNull(0))
                        {
                            AccIntPerc = Convert.ToInt64(reader["AcctIntPerc"]);
                        }
                    }
                }

                updateCmd.Parameters.AddWithValue("@IntPerc", Convert.ToDouble(cmbIntPerc.Text.Trim()));
                updateCmd.Parameters.AddWithValue("@IntAmt", Convert.ToDouble(txtInterest.Text.Trim()));
                updateCmd.Parameters.AddWithValue("@AcctIntPerc", AccIntPerc);
                updateCmd.Parameters.AddWithValue("@ReceiptChg", Convert.ToDouble(txtReceiptCG.Text.Trim()));
                updateCmd.Parameters.AddWithValue("@GivenAmount", Convert.ToDouble(lblGivingAmount.Text));
                updateCmd.Parameters.AddWithValue("@BalanceAmt", Convert.ToDouble(txtamount.Text.Trim()));
                updateCmd.Parameters.AddWithValue("@Status", "L");
                updateCmd.Parameters.AddWithValue("@LoanMark", txtLoanMark.Text.Trim());
                updateCmd.Parameters.AddWithValue("@LoanId", Convert.ToInt32(lblid.Text));

                updateCmd.ExecuteNonQuery();
            }
        }
        private void CustomerCreationEdit()
        {
            string query = "SELECT CustomerId FROM CustomerMaster WHERE " +
                           "Salutation = @Salutation AND CustomerName = @CustomerName AND FHType = @FHType AND " +
                           "FHName = @FHName AND Address = @Address AND Town = @Town AND Phone = @Phone AND AadhaarNo = @AadhaarNo";

            using (SqlConnection conn = new SqlConnection(myconnstrng))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Salutation", cmbNameSalutation.Text.Trim());
                    cmd.Parameters.AddWithValue("@CustomerName", txtname.Text.Trim());
                    cmd.Parameters.AddWithValue("@FHType", cmbFnameTitle.Text.Trim());
                    cmd.Parameters.AddWithValue("@FHName", txtFName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@Town", txtTown.Text.Trim());
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@AadhaarNo", txtaadhaarno.Text.Trim());

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtCustomerID.Text = reader["CustomerId"].ToString();
                        }
                        else
                        {
                            reader.Close();
                            using (SqlCommand maxIdCmd = new SqlCommand("SELECT ISNULL(MAX(CustomerId), 0) + 1 FROM CustomerMaster", conn))
                            {
                                int newCustomerId = (int)maxIdCmd.ExecuteScalar();
                                txtCustomerID.Text = newCustomerId.ToString();

                                string insertQuery = "INSERT INTO CustomerMaster (CustomerId, Salutation, CustomerName, FHType, FHName, Address, Town, Taluk, District, Phone, Occupation, Balance, AadhaarNo) " +
                                                     "VALUES (@CustomerId, @Salutation, @CustomerName, @FHType, @FHName, @Address, @Town, @Taluk, @District, @Phone, @Occupation, @Balance, @AadhaarNo)";

                                using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                                {
                                    insertCmd.Parameters.AddWithValue("@CustomerId", newCustomerId);
                                    insertCmd.Parameters.AddWithValue("@Salutation", cmbNameSalutation.Text.Trim());
                                    insertCmd.Parameters.AddWithValue("@CustomerName", txtname.Text.Trim());
                                    insertCmd.Parameters.AddWithValue("@FHType", cmbFnameTitle.Text.Trim());
                                    insertCmd.Parameters.AddWithValue("@FHName", txtFName.Text.Trim());
                                    insertCmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                                    insertCmd.Parameters.AddWithValue("@Town", txtTown.Text.Trim());
                                    insertCmd.Parameters.AddWithValue("@Taluk", txtTaluk.Text.Trim());
                                    insertCmd.Parameters.AddWithValue("@District", txtDistrict.Text.Trim());
                                    insertCmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                                    insertCmd.Parameters.AddWithValue("@Occupation", txtOccupation.Text.Trim());
                                    insertCmd.Parameters.AddWithValue("@Balance", Convert.ToDecimal(lblBalance.Text.Trim()));
                                    insertCmd.Parameters.AddWithValue("@AadhaarNo", txtaadhaarno.Text.Trim());

                                    insertCmd.ExecuteNonQuery();
                                }

                                string updateLoanQuery = "UPDATE Loan SET CustomerId = @NewCustomerId WHERE CustomerId = @OldCustomerId";
                                using (SqlCommand updateLoanCmd = new SqlCommand(updateLoanQuery, conn))
                                {
                                    updateLoanCmd.Parameters.AddWithValue("@NewCustomerId", newCustomerId);
                                    updateLoanCmd.Parameters.AddWithValue("@OldCustomerId", lblCustomerId2.Text);
                                    updateLoanCmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region ComboBox IntPercentage ,Interest, Amount, ReceiptCG  TEXTCHANGED and KEYPRESS
        private void cmbIntPerc_TextChanged(object sender, EventArgs e)
        {
            Calculateint();
        }
        private void txtInterest_TextChanged(object sender, EventArgs e)
        {
            double amount = 0;
            double receiptCG = 0;
            double interest = 0;

            if (double.TryParse(txtamount.Text.Trim(), out amount) &&
                double.TryParse(txtReceiptCG.Text.Trim(), out receiptCG) &&
                double.TryParse(txtInterest.Text.Trim(), out interest))
            {
                lblGivingAmount.Text = (amount - (receiptCG + interest)).ToString();
            }
            else if (double.TryParse(txtamount.Text.Trim(), out amount) &&
                     double.TryParse(txtReceiptCG.Text.Trim(), out receiptCG))
            {
                lblGivingAmount.Text = (amount - receiptCG).ToString();
            }
            else if (double.TryParse(txtamount.Text.Trim(), out amount))
            {
                lblGivingAmount.Text = amount.ToString();
            }
            else
            {
                txtInterest.Clear();
                lblGivingAmount.Text = "0";
            }

            GiveAmtCalc();
        }
        private void txtamount_TextChanged(object sender, EventArgs e)
        {
            double amount = 0;
            double receiptCG = 0;
            double interest = 0;

            if (double.TryParse(txtamount.Text.Trim(), out amount))
            {
                Calculateint();
                txtReceiptCG.Text = CalculateReceipt(amount).ToString();
                double.TryParse(txtReceiptCG.Text.Trim(), out receiptCG);
                double.TryParse(txtInterest.Text.Trim(), out interest);
                lblGivingAmount.Text = (amount - (receiptCG + interest)).ToString();
            }
            else
            {
                txtReceiptCG.Text = "0";
                txtInterest.Text = "0";
                lblGivingAmount.Text = "0";
            }

            GiveAmtCalc();
        }
        private void txtReceiptCG_TextChanged(object sender, EventArgs e)
        {
            double amount = 0;
            double receiptCG = 0;
            double interest = 0;

            double.TryParse(txtReceiptCG.Text.Trim(), out receiptCG);
            double.TryParse(txtamount.Text.Trim(), out amount);
            double.TryParse(txtInterest.Text.Trim(), out interest);

            lblGivingAmount.Text = (amount - (receiptCG + interest)).ToString();
            GiveAmtCalc();
        }
        private void txtamount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }
        private void txtInterest_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }
        private void txtReceiptCG_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }
        private void txtValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }
        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }
        #endregion

        #region Receipt,Interest,Amount Calculations
        public double CalculateReceipt(double amount)
        {
            double receiptCharge = 0;
            string query = "SELECT * FROM ReceiptCharge WHERE @Amt >= Amountfrom AND @Amt <= AmountTo AND itemtype = @ItemType";

            SqlParameter[] parameters = {
                                new SqlParameter("@Amt", amount),
                                new SqlParameter("@ItemType", cmbItemtype.Text.Trim())
                                         };

            DataTable dt = dal.SelectParameter(query, parameters);
            if (dt != null && dt.Rows.Count > 0)
            {
                receiptCharge = Convert.ToDouble(dt.Rows[0]["ReceiptCharge"]);
            }

            return receiptCharge;
        }
        private void Calculateint()
        {
            decimal amount = 0;
            decimal interestPercentage = 0;

            if (decimal.TryParse(txtamount.Text.Trim(), out amount) && amount > 0)
            {
                if (decimal.TryParse(cmbIntPerc.Text.Trim(), out interestPercentage))
                {
                    decimal interest = Math.Round((amount * interestPercentage) / 100, 2);
                    txtInterest.Text = interest.ToString();
                }
                else
                {
                    txtInterest.Text = "0";
                }
            }
            else
            {
                txtInterest.Text = "0";
                lblGivingAmount.Text = "0";
            }
        }
        public void GiveAmtCalc()
        {
            double givingAmount = Math.Round(Convert.ToDouble(lblGivingAmount.Text), 0);
            double remainderAmount = Convert.ToDouble(lblGivingAmount.Text) - givingAmount;

            lblGivingAmount.Text = remainderAmount > 0 ? (givingAmount + 1).ToString() : givingAmount.ToString();
        }
        #endregion

        #region Identification DGV
        //private void txtIdentification_KeyDown(object sender, KeyEventArgs e)
        //{          
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        if (dgvIdentification.Visible && dgvIdentification.CurrentRow != null)
        //        {
        //            DataGridViewRow selectedRow = dgvIdentification.CurrentRow;
        //            txtIdentification.Text = selectedRow.Cells["Description"].Value?.ToString();
        //            dgvIdentification.Visible = false;

        //            AddDataToDgv();
        //            ClearInputFields();

        //            dgvIdentification.Visible = false;
        //            e.Handled = true;
        //            e.SuppressKeyPress = true;
        //        }
        //        else
        //        {
        //            AddDataToDgv();
        //            ClearInputFields();
        //            dgvIdentification.Visible = false;
        //            e.Handled = true;
        //            e.SuppressKeyPress = true;
        //        }
        //    }

        //    else if (e.KeyCode == Keys.Down)
        //    {
        //        if (dgvIdentification.Visible && dgvIdentification.Rows.Count > 0)
        //        {
        //            dgvIdentification.Focus();
        //            foreach (DataGridViewCell cell in dgvIdentification.Rows[0].Cells)
        //            {
        //                if (cell.Visible)
        //                {
        //                    dgvIdentification.CurrentCell = cell;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}
        private void txtIdentification_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                if (dgvIdentification.Visible && dgvIdentification.CurrentRow != null)
                {
                    DataGridViewRow selectedRow = dgvIdentification.CurrentRow;
                    txtIdentification.Text = selectedRow.Cells["Description"].Value?.ToString();

                    dgvIdentification.Visible = false;
                }

                if (!ItemRepeatCheck())
                {
                    AddDataToDgv();
                }

                ClearInputFields();
                dgvIdentification.Visible = false;
                e.Handled = true;
                e.SuppressKeyPress = true;
            }

            else if (e.KeyCode == Keys.Down)
            {
                if (dgvIdentification.Visible && dgvIdentification.Rows.Count > 0)
                {
                    dgvIdentification.Focus();
                    dgvIdentification.CurrentCell = dgvIdentification.Rows[0].Cells["Description"];
                }
            }
        }

        LoanDAL loanDAL = new LoanDAL();
        private void txtIdentification_KeyPress(object sender, KeyPressEventArgs e)
        {
            dgvIdentification.Visible = true;
            DataTable dt = loanDAL.DistinctIdentification();
            dgvIdentification.DataSource = dt;

            foreach (DataGridViewColumn column in dgvIdentification.Columns)
            {
                column.Visible = false;
            }
            dgvIdentification.Columns["Description"].Visible = true;

            dgvIdentification.Location = new Point(760, 415);
        }
        private void txtIdentification_TextChanged(object sender, EventArgs e)
        {
            string keywords = txtIdentification.Text;
            if (!string.IsNullOrEmpty(keywords))
            {
                DataTable dt = loanDAL.SearchDescription(keywords);
                dgvIdentification.DataSource = dt;
                dgvIdentification.Visible = dt.Rows.Count > 0;
            }
            else
            {
                dgvIdentification.Visible = false;
            }
        }
        private bool ItemRepeatCheck()
        {
            string itemToCheck = txtItemDetails.Text.Trim();
            for (int i = 0; i < dgvOverallItemDet.Rows.Count; i++)
            {
                string existingItem = dgvOverallItemDet.Rows[i].Cells[1].Value.ToString().Trim();

                if (existingItem.Equals(itemToCheck, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Item Already Exists", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtItemDetails.Focus();
                    return true;
                }
            }
            return false;
        }
        private void dgvIdentification_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && dgvIdentification.CurrentRow != null)
            {
                DataGridViewRow selectedRow = dgvIdentification.CurrentRow;

                txtIdentification.Text = selectedRow.Cells["Description"].Value?.ToString();

                if (!ItemRepeatCheck())
                {
                    AddDataToDgv();
                }
                dgvIdentification.Visible = false;
                ClearInputFields();

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
        private void dgvIdentification_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvIdentification.Rows.Count)
            {
                DataGridViewRow selectedRow = dgvIdentification.Rows[e.RowIndex];
                txtIdentification.Text = selectedRow.Cells["Description"].Value?.ToString();

                if (!ItemRepeatCheck())
                {
                    AddDataToDgv();
                }

                dgvIdentification.Visible = false;
            }
            dgvIdentification.Location = new Point(209, 627);
        }
        #endregion

        #region Old Loan DGV Report Generation 
        private void FlexLoanOldAlign()
        {
            dgvLoanOld.Columns["LoanNo"].HeaderText = "அடகு எண்";
            dgvLoanOld.Columns["LoanDate"].HeaderText = "அடகு தேதி";
            dgvLoanOld.Columns["Amount"].HeaderText = "அடகு தொகை";

            dgvLoanOld.Columns["LoanNo"].Width = 90;
            dgvLoanOld.Columns["LoanDate"].Width = 110;
            dgvLoanOld.Columns["Id"].Visible = false;

            foreach (DataGridViewColumn column in dgvLoanOld.Columns)
            {
                column.DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            }

            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle
            {
                BackColor = Color.Purple,
                ForeColor = Color.Yellow,
                Font = new Font("Arial", 12, FontStyle.Bold),
                SelectionBackColor = Color.Purple
            };

            dgvLoanOld.ColumnHeadersDefaultCellStyle = headerStyle;
        }
        private void dgvLoanOld_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvLoanOld.Rows.Count > 1 && e.RowIndex >= 0)
            {
                using (var reportViewerForm = new CrysReportViewer())
                {
                    string reportPath = System.IO.Path.Combine(Application.StartupPath, "Report", "RptLoanHistoryNew.rpt");
                    if (!System.IO.File.Exists(reportPath))
                    {
                        MessageBox.Show("Report file not found: " + reportPath);
                        return;
                    }

                    string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);

                    ReportDocument reportDocument = new ReportDocument();
                    try
                    {
                        reportDocument.Load(reportPath);

                        ConnectionInfo connectionInfo = new ConnectionInfo
                        {
                            ServerName = builder.DataSource,
                            DatabaseName = builder.InitialCatalog,
                            UserID = builder.UserID,
                            Password = builder.Password
                        };

                        SetDatabaseLogonForReport(connectionInfo, reportDocument);

                        int selectedLoanId = Convert.ToInt32(dgvLoanOld.Rows[e.RowIndex].Cells["Id"].Value);

                        reportDocument.RecordSelectionFormula = "{LoanfindQry.ID} = " + selectedLoanId;

                        reportViewerForm.crystalReportViewer1.ReportSource = reportDocument;
                        reportViewerForm.crystalReportViewer1.RefreshReport();

                        reportViewerForm.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error displaying report: " + ex.Message);
                    }
                    finally
                    {
                        if (reportDocument != null)
                        {
                            reportDocument.Close();
                            reportDocument.Dispose();
                        }
                    }
                }
            }
        }
        private void SetDatabaseLogonForReport(ConnectionInfo connectionInfo, ReportDocument reportDocument)
        {
            Tables tables = reportDocument.Database.Tables;

            foreach (CrystalDecisions.CrystalReports.Engine.Table table in tables)
            {
                TableLogOnInfo tableLogonInfo = table.LogOnInfo;
                tableLogonInfo.ConnectionInfo = connectionInfo;
                table.ApplyLogOnInfo(tableLogonInfo);
            }
        }
        #endregion

        #region DGV OverallItemDetails 
        private void SetupDgvOverallItemDet()
        {
            dgvOverallItemDet.ColumnCount = 7;

            dgvOverallItemDet.Columns[0].HeaderText = "வ.எண்";
            dgvOverallItemDet.Columns[1].HeaderText = "பொருளின் விபரம்";
            dgvOverallItemDet.Columns[2].HeaderText = "குறியீடு";
            dgvOverallItemDet.Columns[3].HeaderText = "உருப்படி";
            dgvOverallItemDet.Columns[4].HeaderText = "ItemCode";
            dgvOverallItemDet.Columns[5].HeaderText = "அடையாளக் குறிகள்";
            dgvOverallItemDet.Columns[6].HeaderText = "கிராம்";

            dgvOverallItemDet.Columns[0].Width = 80;
            dgvOverallItemDet.Columns[1].Width = 280;
            dgvOverallItemDet.Columns[2].Width = 180;
            dgvOverallItemDet.Columns[3].Width = 80;
            dgvOverallItemDet.Columns[4].Width = 0;
            dgvOverallItemDet.Columns[4].Visible = false;
            dgvOverallItemDet.Columns[5].Width = 160;
            dgvOverallItemDet.Columns[6].Width = 100;

            dgvOverallItemDet.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvOverallItemDet.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            for (int i = 2; i <= 3; i++)
            {
                dgvOverallItemDet.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            dgvOverallItemDet.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            foreach (DataGridViewColumn column in dgvOverallItemDet.Columns)
            {
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);

            }

            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.BackColor = Color.Purple;
            headerStyle.ForeColor = Color.Yellow;
            headerStyle.Font = new Font("Arial", 12, FontStyle.Bold);

            headerStyle.SelectionBackColor = Color.Purple;

            dgvOverallItemDet.ColumnHeadersDefaultCellStyle = headerStyle;
        }
        private static int itemTypeCounter = 1;
        private void AddDataToDgv()
        {
            if (!string.IsNullOrWhiteSpace(txtItemDetails.Text) &&
                !string.IsNullOrWhiteSpace(txtSeal.Text) &&
                !string.IsNullOrWhiteSpace(txtQty.Text) &&
                !string.IsNullOrWhiteSpace(txtIdentification.Text) &&
                !string.IsNullOrWhiteSpace(txtGram.Text))
            {
                int qty = int.Parse(txtQty.Text);
                int gram = int.Parse(txtGram.Text);

                string itemType = itemTypeCounter.ToString();
                string itemDetails = txtItemDetails.Text;
                string seal = txtSeal.Text;
                string itemCode = "";
                string identification = txtIdentification.Text;

                UpdateTotals(qty, gram);

                dgvOverallItemDet.Rows.Add(itemType, itemDetails, seal, qty, itemCode, identification, gram);
                itemTypeCounter++;

                ClearInputFields();
                DialogResult result = MessageBox.Show("Do you want to add another Item?",
                                                      "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    txtItemDetails.Focus();
                }
                else if (result == DialogResult.No)
                {
                    cmbIntPerc.Focus();
                }
            }
            else
            {
                MessageBox.Show("Please fill in all fields before adding a new Item.", "Item", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }
        private void UpdateTotals(int qty, int gram)
        {
            int totalGrams = int.Parse(txtkg.Text == "" ? "0" : txtkg.Text);
            int totalQty = int.Parse(lblTotalQty.Text == "" ? "0" : lblTotalQty.Text);

            totalGrams += gram;
            totalQty += qty;

            txtkg.Text = totalGrams.ToString();
            lblTotalQty.Text = totalQty.ToString();
        }
        private void ClearInputFields()
        {
            txtItemDetails.Clear();
            txtSeal.Clear();
            txtQty.Clear();
            txtIdentification.Clear();
            txtGram.Clear();
            cmbItemtype.SelectedIndex = 0;
            //cmbItemtype.Focus();
        }
        private void UpdateRowSequence()
        {
            itemTypeCounter = 1;

            for (int i = 0; i < dgvOverallItemDet.Rows.Count; i++)
            {
                dgvOverallItemDet.Rows[i].Cells[0].Value = itemTypeCounter;
                itemTypeCounter++;
            }
        }
        private void dgvOverallItemDet_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvOverallItemDet.CurrentRow != null && dgvOverallItemDet.CurrentRow.Index >= 0)
            {
                var result = MessageBox.Show("Do you want to Delete this row?", "Confirmation", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    int qtyToDelete = 0;
                    int gramsToDelete = 0;

                    if (dgvOverallItemDet.CurrentRow.Cells[3].Value != null &&
                        int.TryParse(dgvOverallItemDet.CurrentRow.Cells[3].Value.ToString(), out int qty))
                    {
                        qtyToDelete = qty;
                    }

                    if (dgvOverallItemDet.CurrentRow.Cells[5].Value != null &&
                        int.TryParse(dgvOverallItemDet.CurrentRow.Cells[5].Value.ToString(), out int grams))
                    {
                        gramsToDelete = grams;
                    }

                    dgvOverallItemDet.Rows.RemoveAt(dgvOverallItemDet.CurrentRow.Index);

                    UpdateTotalsAfterDeletion();
                    UpdateRowSequence();
                }
            }
        }
        private void UpdateTotalsAfterDeletion()
        {
            int totalQty = 0;
            int totalGrams = 0;

            foreach (DataGridViewRow row in dgvOverallItemDet.Rows)
            {
                if (row.Cells[3].Value != null && int.TryParse(row.Cells[3].Value.ToString(), out int qty))
                {
                    totalQty += qty;
                }

                if (row.Cells[5].Value != null && int.TryParse(row.Cells[5].Value.ToString(), out int grams))
                {
                    totalGrams += grams;
                }
            }

            lblTotalQty.Text = totalQty.ToString();
            txtkg.Text = totalGrams.ToString();
        }
        #endregion

        #region Find Button 
        private void btnFind_Click(object sender, EventArgs e)
        {
            BtnDelete.Enabled = true;
            btnPrintAgreement.Enabled = true;
            btnPrintBackSide.Enabled = true;
            btnFind.Enabled = false;
            BtnSave.Enabled = false;
            btnEdit.Enabled = true;
            btnPrint.Enabled = true;
            pctFind.Location = new Point(55, 51);
            string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
            string query = "SELECT TOP 100 * FROM LoanFindQry WHERE Status = 'L' AND companyid <> 0 ORDER BY loandate DESC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();

                try
                {
                    conn.Open();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        pctFind.Visible = true;
                        txtFind.ReadOnly = false;

                        FlexFind.DataSource = dt;
                        FindGridAlign();
                        FlexFind.Enabled = true;
                        txtFind.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Record Not Found", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void FindGridAlign()
        {
            FlexFind.Columns[0].Visible = false;
            FlexFind.Columns[1].Visible = false;
            FlexFind.Columns[2].Width = 120; // loan no
            FlexFind.Columns[3].Width = 120; // loan date
            FlexFind.Columns[4].Width = 140; // amount
            FlexFind.Columns[5].Visible = false;
            FlexFind.Columns[6].Visible = false;
            FlexFind.Columns[7].Width = 140; // customername
            FlexFind.Columns[8].Width = 50;  // fhtype
            FlexFind.Columns[9].Width = 120; // fhname
            FlexFind.Columns[10].Width = 125; // address
            FlexFind.Columns[11].Visible = false;
            FlexFind.Columns[12].Width = 80; // item type
            FlexFind.Columns[13].Visible = false;
            FlexFind.Columns[14].Visible = false;
            FlexFind.Columns[15].Visible = false;
            FlexFind.Columns[16].Visible = false;
            FlexFind.Columns[17].Visible = false;
            FlexFind.Columns[18].Visible = false;
            FlexFind.Columns[19].Visible = false;
            FlexFind.Columns[20].Visible = false;
            FlexFind.Columns[21].Visible = false;
            FlexFind.Columns[22].Width = 100; //district
            FlexFind.Columns[23].Width = 100; //taluk
            FlexFind.Columns[24].Width = 100; //occupation
            FlexFind.Columns[25].Width = 100; //phone
            FlexFind.Columns[26].Width = 100; //gurname
            FlexFind.Columns[27].Width = 100; //guroccupation
            FlexFind.Columns[28].Width = 60; //oldloan no
            FlexFind.Columns[29].Width = 60; //loan mark
            FlexFind.Columns[30].Visible = false;
            FlexFind.Columns[31].Visible = false;
            FlexFind.Columns[32].Visible = false;
            FlexFind.Columns[33].Visible = false;

            FlexFind.Columns[2].HeaderText = "அடகு எண்";
            FlexFind.Columns[3].HeaderText = "அடகு தேதி";
            FlexFind.Columns[4].HeaderText = "தொகை";
            FlexFind.Columns[7].HeaderText = "பெயர்";
            FlexFind.Columns[8].HeaderText = "த.குறியீடு";
            FlexFind.Columns[9].HeaderText = "த.பெயர்";
            FlexFind.Columns[10].HeaderText = "முகவரி";
            FlexFind.Columns[12].HeaderText = "பொருள்";
            FlexFind.Columns[22].HeaderText = "மாவட்டம்";
            FlexFind.Columns[23].HeaderText = "தாலுக்கா";
            FlexFind.Columns[24].HeaderText = "தொழில்";
            FlexFind.Columns[25].HeaderText = "கைபேசி எண்";
            FlexFind.Columns[26].HeaderText = "கார்டியன் பெயர்";
            FlexFind.Columns[27].HeaderText = "கார்டியன் தொழில்";
            FlexFind.Columns[28].HeaderText = "ப.அடகு எண்";
            FlexFind.Columns[29].HeaderText = "குறியீடு";

            for (int i = 0; i < FlexFind.ColumnCount; i++)
            {
                FlexFind.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                FlexFind.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            foreach (DataGridViewColumn column in FlexFind.Columns)
            {
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);

            }

            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.BackColor = Color.Purple;
            headerStyle.ForeColor = Color.Yellow;
            headerStyle.Font = new Font("Arial", 12, FontStyle.Bold);

            headerStyle.SelectionBackColor = Color.Purple;

            FlexFind.ColumnHeadersDefaultCellStyle = headerStyle;

            if (FlexFind.Rows.Count > 0)
            {
                FlexFind.CurrentCell = FlexFind.Rows[0].Cells[2];
            }
        }
        private void txtFind_TextChanged(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString; string query = "";
            string argString = " and companyid <> 0";

            string selectedItem = cmbfind.Text;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                if (selectedItem == "அடகு எண்")
                {
                    query = $"SELECT TOP 500 * FROM LoanFindQry WHERE LoanNo LIKE '{RecordFind(txtFind.Text.Trim())}%' AND Status='L' {argString} ORDER BY loandate DESC, dbo.fn_Loanchar(loanno), dbo.fn_Loannum(loanno) DESC";
                }
                else if (selectedItem == "பெயர்")
                {
                    query = $"SELECT TOP 500 * FROM LoanFindQry WHERE CustomerName LIKE '{RecordFind(txtFind.Text.Trim())}%' AND Status='L' {argString} ORDER BY loandate DESC, CustomerName";
                }
                else if (selectedItem == "பொருள்")
                {
                    query = $"SELECT TOP 500 * FROM LoanFindQry WHERE ItemType LIKE '{RecordFind(txtFind.Text.Trim())}%' AND Status='L' {argString} ORDER BY loandate DESC, dbo.fn_Loanchar(loanno), dbo.fn_Loannum(loanno) DESC";
                }

                if (!string.IsNullOrEmpty(query))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            FlexFind.DataSource = dt;
                            FindGridAlign();
                        }
                        else
                        {
                            //FlexFind.DataSource = null;
                            //FlexFind.Rows.Clear();
                        }
                    }
                }
            }
        }
        public string RecordFind(string strInput)
        {
            strInput = strInput.Replace("[", "[[]");
            return strInput.Replace("'", "''");
        }
        private void FlexFind_DoubleClick(object sender, EventArgs e)
        {
            if (FlexFind.Rows.Count > 0)
            {
                lblid.Text = FlexFind.CurrentRow.Cells[0].Value.ToString();
                Getloan();

                if (btnEdit.Enabled)
                {
                    btnEdit.Focus();
                }
            }
        }
        public void Getloan()
        {
            try
            {
                if (FlexFind.CurrentRow != null)
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "SELECT LoanNo, LoanDate, Amount, BalanceAmt FROM Loan " +
                                       "WHERE Status='L' AND CustomerId=" + Convert.ToInt32(FlexFind.CurrentRow.Cells[6].Value) +
                                       " AND LoanDate <= '" + Convert.ToDateTime(FlexFind.CurrentRow.Cells[3].Value).ToString("dd/MMM/yyyy") +
                                       "' AND Id <> " + Convert.ToInt32(FlexFind.CurrentRow.Cells[0].Value);

                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                da.Fill(dt);
                                dgvLoanOld.AutoGenerateColumns = true;
                                dgvLoanOld.DataSource = dt;
                                FlexLoanOldAlignEdit();
                            }
                        }

                        ChangeCompany();
                        txtloanno.Text = FlexFind.CurrentRow.Cells[2].Value.ToString();
                        dtpLoanDate.Value = Convert.ToDateTime(FlexFind.CurrentRow.Cells[3].Value);
                        txtamount.Text = Convert.ToDecimal(FlexFind.CurrentRow.Cells[4].Value).ToString();
                        dtpRedeemDate.Value = Convert.ToDateTime(FlexFind.CurrentRow.Cells[5].Value);
                        txtCustomerID.Text = FlexFind.CurrentRow.Cells[6].Value.ToString();

                        string salutation = FlexFind.CurrentRow.Cells[19].Value.ToString();
                        if (salutation == "Mr.")
                            cmbNameSalutation.SelectedIndex = 0;
                        else if (salutation == "Mrs.")
                            cmbNameSalutation.SelectedIndex = 1;
                        else if (salutation == "Miss")
                            cmbNameSalutation.SelectedIndex = 2;

                        txtname.Text = FlexFind.CurrentRow.Cells[7].Value.ToString();
                        cmbFnameTitle.Text = FlexFind.CurrentRow.Cells[8].Value.ToString();
                        txtFName.Text = FlexFind.CurrentRow.Cells[9].Value.ToString();
                        txtAddress.Text = FlexFind.CurrentRow.Cells[10].Value.ToString();
                        txtTown.Text = FlexFind.CurrentRow.Cells[11].Value.ToString();
                        txtDistrict.Text = FlexFind.CurrentRow.Cells[22].Value.ToString();

                        if (FlexFind.CurrentRow.Cells[12].Value.ToString() == "பொன்")
                            cmbItemtype.SelectedIndex = 0;
                        else if (FlexFind.CurrentRow.Cells[12].Value.ToString() == "வெள்ளி")
                            cmbItemtype.SelectedIndex = 1;

                        txtkg.Text = FlexFind.CurrentRow.Cells[13].Value.ToString();
                        lblTotalQty.Text = FlexFind.CurrentRow.Cells[14].Value.ToString();
                        cmbIntPerc.Text = FlexFind.CurrentRow.Cells[15].Value.ToString();
                        txtInterest.Text = Convert.ToDecimal(FlexFind.CurrentRow.Cells[16].Value).ToString();
                        txtValue.Text = Convert.ToDecimal(FlexFind.CurrentRow.Cells[17].Value).ToString();
                        txtReceiptCG.Text = Convert.ToDecimal(FlexFind.CurrentRow.Cells[18].Value).ToString();

                        if (FlexFind.CurrentRow.Cells[23].Value != null)
                            txtTaluk.Text = FlexFind.CurrentRow.Cells[23].Value.ToString();
                        if (FlexFind.CurrentRow.Cells[24].Value != null)
                            txtOccupation.Text = FlexFind.CurrentRow.Cells[24].Value.ToString();
                        if (FlexFind.CurrentRow.Cells[25].Value != null)
                            txtPhone.Text = FlexFind.CurrentRow.Cells[25].Value.ToString();
                        if (FlexFind.CurrentRow.Cells[26].Value != null)
                            txtGuName.Text = FlexFind.CurrentRow.Cells[26].Value.ToString();
                        if (FlexFind.CurrentRow.Cells[27].Value != null)
                            txtGuOccupation.Text = FlexFind.CurrentRow.Cells[27].Value.ToString();
                        if (FlexFind.CurrentRow.Cells[28].Value != null)
                            txtOldLoanNo.Text = FlexFind.CurrentRow.Cells[28].Value.ToString();
                        if (FlexFind.CurrentRow.Cells[29].Value != null)
                            txtLoanMark.Text = FlexFind.CurrentRow.Cells[29].Value.ToString();

                        GetBalance();

                        string itemDetailsQuery = "SELECT SNo, ItemName, Seal, TotalQty, ItemCode, Description, Gram " +
                           "FROM LoanDetailsFind WHERE Id=" + Convert.ToInt32(lblid.Text) + " ORDER BY SNo";

                        using (SqlCommand cmdItemDetails = new SqlCommand(itemDetailsQuery, connection))
                        {
                            using (SqlDataAdapter daItemDetails = new SqlDataAdapter(cmdItemDetails))
                            {
                                DataTable dtItemDetails = new DataTable();
                                daItemDetails.Fill(dtItemDetails);
                                dgvOverallItemDet.DataSource = dtItemDetails;
                            }
                        }
                        SetupDgvOverallItemDetEdit();

                        pctFind.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FlexLoanOldAlignEdit()
        {
            if (dgvLoanOld.Columns.Contains("LoanNo") && dgvLoanOld.Columns.Contains("LoanDate"))
            {
                dgvLoanOld.Columns["LoanNo"].HeaderText = "அடகு எண்";
                dgvLoanOld.Columns["LoanDate"].HeaderText = "அடகு தேதி";
                dgvLoanOld.Columns["Amount"].HeaderText = "அடகு தொகை";

                dgvLoanOld.Columns["LoanNo"].Width = 80;
                dgvLoanOld.Columns["LoanDate"].Width = 110;

                if (dgvLoanOld.Columns.Contains("BalanceAmt"))
                {
                    dgvLoanOld.Columns["BalanceAmt"].HeaderText = "மீதத் தொகை";
                    dgvLoanOld.Columns["BalanceAmt"].Width = 90;
                }

                foreach (DataGridViewColumn column in dgvLoanOld.Columns)
                {
                    column.DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                }

                DataGridViewCellStyle headerStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.Purple,
                    ForeColor = Color.Yellow,
                    Font = new Font("Latha", 8),
                    SelectionBackColor = Color.Purple
                };

                dgvLoanOld.ColumnHeadersDefaultCellStyle = headerStyle;
            }
        }
        private void SetupDgvOverallItemDetEdit()
        {
            dgvOverallItemDet.AutoGenerateColumns = false;
            dgvOverallItemDet.Columns.Clear();

            DataGridViewTextBoxColumn colSNo = new DataGridViewTextBoxColumn
            {
                HeaderText = "வ.எண்",
                DataPropertyName = "SNo",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
            };
            dgvOverallItemDet.Columns.Add(colSNo);

            DataGridViewTextBoxColumn colItemName = new DataGridViewTextBoxColumn
            {
                HeaderText = "பொருளின் விபரம்",
                DataPropertyName = "ItemName",
                Width = 280,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
            };
            dgvOverallItemDet.Columns.Add(colItemName);

            DataGridViewTextBoxColumn colSeal = new DataGridViewTextBoxColumn
            {
                HeaderText = "குறியீடு",
                DataPropertyName = "Seal",
                Width = 180,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
            };
            dgvOverallItemDet.Columns.Add(colSeal);

            DataGridViewTextBoxColumn colTotalQty = new DataGridViewTextBoxColumn
            {
                HeaderText = "உருப்படி",
                DataPropertyName = "TotalQty",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
            };
            dgvOverallItemDet.Columns.Add(colTotalQty);

            DataGridViewTextBoxColumn colItemCode = new DataGridViewTextBoxColumn
            {
                HeaderText = "ItemCode",
                DataPropertyName = "ItemCode",
                Width = 0,
                Visible = false
            };
            dgvOverallItemDet.Columns.Add(colItemCode);

            DataGridViewTextBoxColumn colDescription = new DataGridViewTextBoxColumn
            {
                HeaderText = "அடையாளக் குறிகள்",
                DataPropertyName = "Description",
                Width = 160,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
            };
            dgvOverallItemDet.Columns.Add(colDescription);

            DataGridViewTextBoxColumn colGram = new DataGridViewTextBoxColumn
            {
                HeaderText = "கிராம்",
                DataPropertyName = "Gram",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
            };
            dgvOverallItemDet.Columns.Add(colGram);

            foreach (DataGridViewColumn column in dgvOverallItemDet.Columns)
            {
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            }

            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle
            {
                BackColor = Color.Purple,
                ForeColor = Color.Yellow,
                Font = new Font("Arial", 12, FontStyle.Bold),
                SelectionBackColor = Color.Purple
            };

            dgvOverallItemDet.ColumnHeadersDefaultCellStyle = headerStyle;
        }

        public static int gCID; public static string gNAME;
        private void ChangeCompany()
        {
            gCID = 1;
            gNAME = "Default";
        }
        public void GetBalance()
        {
            string query = "SELECT Balance FROM CustomerMaster WHERE CustomerId = @CustomerId";
            SqlParameter[] parameters = {
            new SqlParameter("@CustomerId", Convert.ToInt32(txtCustomerID.Text.Trim()))
        };
            string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))

            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddRange(parameters);
                    conn.Open();

                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        lblBalance.Text = Convert.ToDecimal(result).ToString("0.00");
                    }
                    else
                    {
                        lblBalance.Text = "0.00";
                    }
                }
            }
        }
        #endregion

        #region Edit Button
        private void btnEdit_Click(object sender, EventArgs e)
        {
            Password_Enter passenter = new Password_Enter();
            DialogResult result = passenter.ShowDialog();

            if (result == DialogResult.OK)
            {
                long selectedLoanId = Convert.ToInt64(lblid.Text);

                if (!GetLoanReturn(selectedLoanId))
                {
                    MessageBox.Show("Record cannot be edited. Loan Return Found.");
                    return;
                }

                SaveFlg = true;

                BtnSave.Enabled = true;
                btnEdit.Enabled = false;
            }
            else
            {
                BtnSave.Enabled = false;
            }
        }
        private bool GetLoanReturn(long loanId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
            string query = "SELECT * FROM loanreturn WHERE loanid = @loanid";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@loanid", loanId);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error checking loan return: " + ex.Message);
                    return false;
                }
            }
        }
        #endregion

        #region Reset Button 
        private void btnReset_Click(object sender, EventArgs e)
        {
            txtamount.Focus();
            BtnDelete.Enabled = false;
            btnPrintAgreement.Enabled = false;
            btnPrintBackSide.Enabled = false;
            btnFind.Enabled = true;
            dgvLoanOld.DataSource = null;
            dgvOverallItemDet.DataSource = null;
            dgvOverallItemDet.Rows.Clear();
            ClearAll(this);

            dtpLoanDate.Value = DateTime.Now;
            dtpRedeemDate.Value = dtpLoanDate.Value.AddMonths(1).AddDays(-1);

            cmbItemtype.SelectedIndex = 0;
            cmbFnameTitle.SelectedIndex = 0;
            cmbfind.SelectedIndex = 0;
            cmbNameSalutation.SelectedIndex = 0;
            cmbIntPerc.SelectedIndex = -1;
            lblintId.Text = "0";
            lblintno.Text = "0";

            dgvIdentification.Visible = false;
            pctFind.Visible = false;
            dgvPhoneNo.Visible = false;
            dgvName.Visible = false;
            dgvFName.Visible = false;
            dgvAddress.Visible = false;
            dgvTown.Visible = false;
            dgvItemDetail.Visible = false;
            dgvTaluk.Visible = false;
            dgvDistrict.Visible = false;
            BtnSave.Enabled = true;
            btnEdit.Enabled = false;
            btnPrint.Enabled = false;
            NewRecId();
            NewRecNo();
            NewLoanNo();
        }
        public void ClearAll(Control parent)
        {
            foreach (Control ctl in parent.Controls)
            {
                if (ctl is TextBox)
                    ((TextBox)ctl).Clear();
                else if (ctl.HasChildren)
                    ClearAll(ctl);
            }
        }
        #endregion

        #region Delete Button 
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            Password_Enter passenter = new Password_Enter();
            DialogResult result = passenter.ShowDialog();

            if (result == DialogResult.OK)
            {
                DialogResult delrec = MessageBox.Show("You Want To Delete Record", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (delrec == DialogResult.Yes)
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                if (GetloanReturn(Convert.ToInt32(lblid.Text)))
                                {
                                    string deleteLoanQuery = "DELETE FROM Loan WHERE id=@LoanId";
                                    using (SqlCommand cmd = new SqlCommand(deleteLoanQuery, connection, transaction))
                                    {
                                        cmd.Parameters.AddWithValue("@LoanId", Convert.ToInt32(lblid.Text));
                                        cmd.ExecuteNonQuery();
                                    }

                                    string deleteLoanDetailsQuery = "DELETE FROM LoanDetails WHERE id=@LoanId";
                                    using (SqlCommand cmd = new SqlCommand(deleteLoanDetailsQuery, connection, transaction))
                                    {
                                        cmd.Parameters.AddWithValue("@LoanId", Convert.ToInt32(lblid.Text));
                                        cmd.ExecuteNonQuery();
                                    }
                                    string deleteLedgerTransactionQuery = "DELETE FROM LedgerTransaction WHERE refno = @RefNo";
                                    using (SqlCommand cmd = new SqlCommand(deleteLedgerTransactionQuery, connection, transaction))
                                    {
                                        cmd.Parameters.AddWithValue("@RefNo", "Loan" + lblid.Text);
                                        cmd.ExecuteNonQuery();
                                    }
                                    transaction.Commit();
                                    MessageBox.Show("Record is Deleted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show("Record Cannot be deleted. Loan Return Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                MessageBox.Show("Error while deleting record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Record not Deleted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BtnSave.Enabled = false;
                }

                btnReset_Click(sender, e);
            }
        }
        private bool GetloanReturn(long loanId)
        {
            bool loanReturnExists = false;
            string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT * FROM loanreturn WHERE loanid = @LoanId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LoanId", loanId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                loanReturnExists = false;
                            }
                            else
                            {
                                loanReturnExists = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching loan return: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return loanReturnExists;
        }
        #endregion

        #region Reports Backside and Aggrement
        private void btnPrintBackSide_Click(object sender, EventArgs e)
        {
            using (var reportViewerForm = new CrysReportViewer())
            {
                string reportPath = System.IO.Path.Combine(Application.StartupPath, "Report", "RptLoanBackSide.rpt");
                if (!System.IO.File.Exists(reportPath))
                {
                    MessageBox.Show("Report file not found: " + reportPath);
                    return;
                }

                string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);

                ReportDocument reportDocument = new ReportDocument();
                try
                {
                    reportDocument.Load(reportPath);

                    ConnectionInfo connectionInfo = new ConnectionInfo
                    {
                        ServerName = builder.DataSource,
                        DatabaseName = builder.InitialCatalog,
                        UserID = builder.UserID,
                        Password = builder.Password
                    };

                    SetDatabaseLogonForReport(connectionInfo, reportDocument);

                    reportDocument.RecordSelectionFormula = "{LoanBill.LoanId} = " + Convert.ToInt32(lblid.Text);

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
        private void btnPrintAgreement_Click(object sender, EventArgs e)
        {
            using (var reportViewerForm = new CrysReportViewer())
            {
                reportViewerForm.crystalReportViewer1.ReportSource = null;
                reportViewerForm.crystalReportViewer1.Refresh();

                string reportPath = System.IO.Path.Combine(Application.StartupPath, "Report", "RptLoanAgreement.rpt");
                if (!System.IO.File.Exists(reportPath))
                {
                    MessageBox.Show("Report file not found: " + reportPath);
                    return;
                }

                string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);

                ReportDocument reportDocument = new ReportDocument();
                try
                {
                    reportDocument.Load(reportPath);

                    ConnectionInfo connectionInfo = new ConnectionInfo
                    {
                        ServerName = builder.DataSource,
                        DatabaseName = builder.InitialCatalog,
                        UserID = builder.UserID,
                        Password = builder.Password
                    };

                    SetDatabaseLogonForReport(connectionInfo, reportDocument);

                    reportDocument.RecordSelectionFormula = "{LoanBill.LoanId} = " + Convert.ToInt32(lblid.Text);

                    reportViewerForm.crystalReportViewer1.ReportSource = reportDocument;
                    reportViewerForm.crystalReportViewer1.RefreshReport();
                    reportViewerForm.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error displaying report: " + ex.Message);
                }
                finally
                {
                    if (reportDocument != null)
                    {
                        reportDocument.Close();
                        reportDocument.Dispose();
                    }
                }
            }
        }

        #endregion

        #region Print Functions
        private void btnPrint_Click(object sender, EventArgs e)
        {
            using (var reportViewerForm = new CrysReportViewer())
            {
                //string reportPath = @"G:\Pawn Broking C#\Pawn Broking\Report\RptLoanSheet.rpt";
                string reportPath = System.IO.Path.Combine(Application.StartupPath, "Report", "RptLoanSheet.rpt");

                if (!System.IO.File.Exists(reportPath))
                {
                    MessageBox.Show("Report file not found: " + reportPath);
                    return;
                }

                string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);

                ReportDocument reportDocument = new ReportDocument();
                try
                {
                    reportDocument.Load(reportPath);

                    ConnectionInfo connectionInfo = new ConnectionInfo
                    {
                        ServerName = builder.DataSource,
                        DatabaseName = builder.InitialCatalog,
                        UserID = builder.UserID,
                        Password = builder.Password
                    };

                    SetDatabaseLogonForReport(connectionInfo, reportDocument);

                    int selectedLoanId = Convert.ToInt32(lblid.Text);
                    reportDocument.RecordSelectionFormula = "{LoanBill.LoanId} = " + selectedLoanId;

                    string amtDets = "(" + DigitsToWords(txtamount.Text) + ")";
                    reportDocument.SetParameterValue("AmtDets", amtDets);
                    reportDocument.SetParameterValue("BILLHEATER", "CUSTOMER COPY");

                    reportViewerForm.crystalReportViewer1.ReportSource = reportDocument;
                    reportViewerForm.crystalReportViewer1.RefreshReport();

                    reportViewerForm.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error displaying report: " + ex.Message);
                }
                finally
                {
                    if (reportDocument != null)
                    {
                        reportDocument.Close();
                        reportDocument.Dispose();
                    }
                }
            }
        }
        public static string DigitsToWords(string sDigit)
        {
            if (!sDigit.Contains("."))
            {
                sDigit = sDigit + ".00";
            }

            if (sDigit.Length > 19)
            {
                throw new ArgumentException("Amount too big to express in words");
            }

            string sMystr = ConvertIt(sDigit);
            return sMystr;
        }
        public static string ConvertIt(string sDigConv)
        {
            string sMystr = "";
            string sMyStrPaisa = "";
            string chstr = "";

            if (sDigConv.Length > 12)
            {
                chstr = sDigConv.Substring(0, sDigConv.Length - 10) + ".00";
                sMystr = Rupees(chstr) + " Crore ";
                chstr = sDigConv.Substring(sDigConv.Length - 10);
            }
            else
            {
                chstr = sDigConv;
            }

            sMystr += Rupees(chstr.Substring(0, chstr.IndexOf(".")));
            string paisa = chstr.Substring(chstr.IndexOf(".") + 1);

            if (int.TryParse(paisa, out int paisaValue) && paisaValue > 0)
            {
                sMyStrPaisa = " and " + SpellIt(paisa) + " Paisa";
            }

            if (!string.IsNullOrEmpty(sMystr))
            {
                sMystr = "Rupees " + sMystr;
            }

            return sMystr.Trim() + " " + sMyStrPaisa + " Only";
        }
        public static string SpellIt(string sTemp)
        {
            switch (int.Parse(sTemp))
            {
                case 1: return "One ";
                case 2: return "Two ";
                case 3: return "Three ";
                case 4: return "Four ";
                case 5: return "Five ";
                case 6: return "Six ";
                case 7: return "Seven ";
                case 8: return "Eight ";
                case 9: return "Nine ";
                case 10: return "Ten ";
                case 11: return "Eleven ";
                case 12: return "Twelve ";
                case 13: return "Thirteen ";
                case 14: return "Fourteen ";
                case 15: return "Fifteen ";
                case 16: return "Sixteen ";
                case 17: return "Seventeen ";
                case 18: return "Eighteen ";
                case 19: return "Nineteen ";
                case 20: return "Twenty ";
                case 30: return "Thirty ";
                case 40: return "Forty ";
                case 50: return "Fifty ";
                case 60: return "Sixty ";
                case 70: return "Seventy ";
                case 80: return "Eighty ";
                case 90: return "Ninety ";
                default: return "";
            }
        }
        public static string Rupees(string sChstr)
        {
            string result = "";
            int temp;
            string beforePoint = sChstr;

            if (beforePoint.Length > 7)
            {
                temp = int.Parse(beforePoint.Substring(0, beforePoint.Length - 7));
                if (temp > 0)
                {
                    result += SpellRupees(temp.ToString(), 5) + " Crore ";
                }
                beforePoint = beforePoint.Substring(beforePoint.Length - 7);
            }

            if (beforePoint.Length > 5)
            {
                temp = int.Parse(beforePoint.Substring(0, beforePoint.Length - 5));
                if (temp > 0)
                {
                    result += SpellRupees(temp.ToString(), 4) + " Lakh ";
                }
                beforePoint = beforePoint.Substring(beforePoint.Length - 5);
            }

            if (beforePoint.Length > 3)
            {
                temp = int.Parse(beforePoint.Substring(0, beforePoint.Length - 3));
                if (temp > 0)
                {
                    result += SpellRupees(temp.ToString(), 3) + " Thousand ";
                }
                beforePoint = beforePoint.Substring(beforePoint.Length - 3);
            }

            if (beforePoint.Length > 2)
            {
                temp = int.Parse(beforePoint.Substring(0, beforePoint.Length - 2));
                if (temp > 0)
                {
                    result += SpellRupees(temp.ToString(), 2) + " Hundred ";
                }
                beforePoint = beforePoint.Substring(beforePoint.Length - 2);
            }

            if (beforePoint.Length > 0)
            {
                temp = int.Parse(beforePoint);
                if (temp > 0)
                {
                    result += SpellRupees(beforePoint, 1);
                }
            }

            return result.Trim();
        }
        public static string SpellRupees(string sTemp, int position)
        {
            int number = int.Parse(sTemp);

            if (number < 21)
            {
                return SpellIt(sTemp);
            }
            else
            {
                string result = SpellIt((number / 10 * 10).ToString());
                if (number % 10 > 0)
                {
                    result += SpellIt((number % 10).ToString());
                }
                return result;
            }
        }
        #endregion


    }
}


