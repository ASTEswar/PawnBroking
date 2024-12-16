using Pawn_Broking.BLL;
using Pawn_Broking.DAL;
using Pawn_Broking.Master_UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pawn_Broking.UI
{
    public partial class Custom_Details_Change : Form
    {
        private bool UpdateMode = false;
        public Custom_Details_Change()
        {
            InitializeComponent();
        }

        private void Custom_Details_Change_Load(object sender, EventArgs e)
        {
            DisplayNextID();
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            panelFind.Location = new Point(31, 544);

        }
        private void DisplayNextID()
        {
            string query = "SELECT ISNULL(MAX(CustomerID), 0) + 1 FROM CustomerMaster";

            using (SqlConnection dbcon = new SqlConnection("Data Source=DESKTOP-6JKB17U\\SQL;Initial Catalog=PawnBrokingNew;User ID=sa;Password=1234;"))
            {

                using (SqlCommand cmd = new SqlCommand(query, dbcon))
                {
                    try
                    {

                        dbcon.Open();

                        object result = cmd.ExecuteScalar();

                        txtCustomerID.Text = result != null ? result.ToString() : "1";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        dbcon.Close();
                    }
                }
            }
        }

        CustomerDetailBLL customer = new CustomerDetailBLL();
        CustomerDetailDAL dal = new CustomerDetailDAL();

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            #region Validation
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Enter Customer Name", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtFName.Text))
            {
                MessageBox.Show("Enter Father Name", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtFName.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtMobile.Text))
            {
                MessageBox.Show("Enter Mobile No", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMobile.Focus();
                return;
            }
            //if (string.IsNullOrWhiteSpace(txtAadharNo.Text))
            //{
            //    MessageBox.Show("Enter Aadhar No", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    txtAadharNo.Focus();
            //    return;
            //}
            #endregion

            customer.CustomerID = int.Parse(txtCustomerID.Text);
            customer.Salutation = cmbNameSalutation.Text;
            customer.CustomerName = txtName.Text;
            customer.FHType = cmbFnameTitle.Text;
            customer.FHName = txtFName.Text;
            customer.Address = txtAddress.Text;
            customer.Town = txtTown.Text;
            customer.District = txtDistrict.Text;
            customer.Taluk = txtTaluk.Text;
            customer.occupation = txtOccupation.Text;
            customer.phone = txtMobile.Text;
            customer.AadhaarNo = txtAadharNo.Text;

            if (!UpdateMode)
            {
                bool success = dal.Insert(customer);

                if (success)
                {
                    MessageBox.Show("New Customer Successfully added", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear();
                    DisplayNextID();
                }
                else
                {
                    MessageBox.Show("Failed to add Customer", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                customer.CustomerID = int.Parse(txtCustomerID.Text);

                bool success = dal.Update(customer);

                if (success)
                {
                    MessageBox.Show("Customer Updated Successfully", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear();
                    btnFind.Enabled = true;
                    UpdateMode = false;
                    DisplayNextID();
                }
                else
                {
                    MessageBox.Show("Failed to update Customer", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            Password_Enter passenter = new Password_Enter();
            DialogResult result = passenter.ShowDialog();

            if (result == DialogResult.OK)
            {
                btnSave.Enabled = true;
                btnEdit.Enabled = false;
            }
            else
            {
                btnSave.Enabled = false;
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!UpdateMode)
            {
                MessageBox.Show("Please select a Customer to delete.", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            customer.CustomerID = int.Parse(txtCustomerID.Text);

            bool success = dal.Delete(customer);

            if (success)
            {
                MessageBox.Show("Customer Deleted Successfully", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Clear();
                UpdateMode = false;
                DisplayNextID();
                btnSave.Enabled = true;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnFind.Enabled = true;
            }
            else
            {
                MessageBox.Show("Failed to Delete Customer", "Customer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnReset_Click_1(object sender, EventArgs e)
        {
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = true;
            btnFind.Enabled = true;
            Clear();
            UpdateMode = false;
            DisplayNextID();
        }

        private void Clear()
        {
            cmbNameSalutation.Text = "";
            cmbFnameTitle.Text = "";
            txtName.Text = "";
            txtFName.Text = "";
            txtAddress.Text = "";
            txtTaluk.Text = "";
            txtDistrict.Text = "";
            txtTown.Text = "";
            txtOccupation.Text = "";
            txtMobile.Text = "";
            txtAadharNo.Text = "";
        }
        private void btnFind_Click_1(object sender, EventArgs e)
        {
            panelFind.Location = new Point(32, 76);

            btnEdit.Enabled = true;
            btnSave.Enabled = false;

            DataTable dt = dal.Select();
            dgvCustomerDetails.DataSource = dt;

            #region DGV Properties
            dgvCustomerDetails.Columns["CustomerID"].Visible = false;
            dgvCustomerDetails.Columns["Salutation"].Visible = false;
            dgvCustomerDetails.Columns["CustomerInitial"].Visible = false;
            dgvCustomerDetails.Columns["FHType"].Visible = false;
            dgvCustomerDetails.Columns["Address"].Visible = false;
            dgvCustomerDetails.Columns["Taluk"].Visible = false;
            dgvCustomerDetails.Columns["District"].Visible = false;
            dgvCustomerDetails.Columns["Occupation"].Visible = false;        
            dgvCustomerDetails.Columns["Balance"].Visible = false;
            #endregion
        }

        #region DGV Cell Click
        private void dgvCustomerDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvCustomerDetails.Rows.Count)
            {
                DataGridViewRow selectedRow = dgvCustomerDetails.Rows[e.RowIndex];

                txtCustomerID.Text = selectedRow.Cells["CustomerID"].Value?.ToString();
                txtName.Text = selectedRow.Cells["CustomerName"].Value?.ToString();
                cmbNameSalutation.Text = selectedRow.Cells["Salutation"].Value?.ToString();
                cmbFnameTitle.Text = selectedRow.Cells["FHType"].Value?.ToString();
                txtFName.Text = selectedRow.Cells["FHName"].Value?.ToString();
                txtAddress.Text = selectedRow.Cells["Address"].Value?.ToString();
                txtTown.Text = selectedRow.Cells["Town"].Value?.ToString();
                txtDistrict.Text = selectedRow.Cells["District"].Value?.ToString();
                txtTaluk.Text = selectedRow.Cells["Taluk"].Value?.ToString();
                txtOccupation.Text = selectedRow.Cells["occupation"].Value?.ToString();
                txtMobile.Text = selectedRow.Cells["phone"].Value?.ToString();
                txtAadharNo.Text = selectedRow.Cells["AadhaarNo"].Value?.ToString();

                UpdateMode = true;

                btnDelete.Enabled = true;
                panelFind.Location = new Point(31, 544);
                btnFind.Enabled = false;
            }
        }

        private void dgvCustomerDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0 && e.RowIndex < dgvCustomerDetails.Rows.Count)
            {
                DataGridViewRow selectedRow = dgvCustomerDetails.Rows[e.RowIndex];

                txtCustomerID.Text = selectedRow.Cells["CustomerID"].Value?.ToString();
                txtName.Text = selectedRow.Cells["CustomerName"].Value?.ToString();
                cmbNameSalutation.Text = selectedRow.Cells["Salutation"].Value?.ToString();
                cmbFnameTitle.Text = selectedRow.Cells["FHType"].Value?.ToString();
                txtFName.Text = selectedRow.Cells["FHName"].Value?.ToString();
                txtAddress.Text = selectedRow.Cells["Address"].Value?.ToString();
                txtTown.Text = selectedRow.Cells["Town"].Value?.ToString();
                txtDistrict.Text = selectedRow.Cells["District"].Value?.ToString();
                txtTaluk.Text = selectedRow.Cells["Taluk"].Value?.ToString();
                txtOccupation.Text = selectedRow.Cells["occupation"].Value?.ToString();
                txtMobile.Text = selectedRow.Cells["phone"].Value?.ToString();
                txtAadharNo.Text = selectedRow.Cells["AadhaarNo"].Value?.ToString();

                UpdateMode = true;

                btnDelete.Enabled = true;
                panelFind.Location = new Point(31, 544);
                btnFind.Enabled = false;
            }
        }

        private void dgvCustomerDetails_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (e.RowIndex >= 0 && e.RowIndex < dgvCustomerDetails.Rows.Count)
            {
                DataGridViewRow selectedRow = dgvCustomerDetails.Rows[e.RowIndex];

                txtCustomerID.Text = selectedRow.Cells["CustomerID"].Value?.ToString();
                txtName.Text = selectedRow.Cells["CustomerName"].Value?.ToString();
                cmbNameSalutation.Text = selectedRow.Cells["Salutation"].Value?.ToString();
                cmbFnameTitle.Text = selectedRow.Cells["FHType"].Value?.ToString();
                txtFName.Text = selectedRow.Cells["FHName"].Value?.ToString();
                txtAddress.Text = selectedRow.Cells["Address"].Value?.ToString();
                txtTown.Text = selectedRow.Cells["Town"].Value?.ToString();
                txtDistrict.Text = selectedRow.Cells["District"].Value?.ToString();
                txtTaluk.Text = selectedRow.Cells["Taluk"].Value?.ToString();
                txtOccupation.Text = selectedRow.Cells["occupation"].Value?.ToString();
                txtMobile.Text = selectedRow.Cells["phone"].Value?.ToString();
                txtAadharNo.Text = selectedRow.Cells["AadhaarNo"].Value?.ToString();

                UpdateMode = true;

                btnDelete.Enabled = true;
                panelFind.Location = new Point(31, 544);
                btnFind.Enabled = false;
            }
        }

        #endregion

        private void txtboxSearch_TextChanged(object sender, EventArgs e)
        {
            string keywords = txtboxSearch.Text;

            string selectedField = cmbCustomerSelect.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedField))
            {
                if (!string.IsNullOrWhiteSpace(keywords))
                {
                    DataTable dt = dal.Search(selectedField, keywords);
                    dgvCustomerDetails.DataSource = dt;
                }
                else
                {
                    DataTable dt = dal.Select();
                    dgvCustomerDetails.DataSource = dt;
                }
            }
            else
            {
                MessageBox.Show("Please Select any Value ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtboxSearch.Text = "";
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}
