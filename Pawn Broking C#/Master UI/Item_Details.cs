using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using Pawn_Broking.BLL;
using Pawn_Broking.DAL;
using System.Data.SqlClient;
using Pawn_Broking.Master_UI;

namespace Pawn_Broking.UI
{
    public partial class Item_Details : Form
    {
        private bool UpdateMode = false;
        public Item_Details()
        {
            InitializeComponent();
        }
        private void formitemdeatails_Load(object sender, EventArgs e)
        {
            DisplayNextID();
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            panelFind.Location = new Point(12, 345);
            cmbitemtype.SelectedIndex = 1;
            
        }

        private void DisplayNextID()
        {
            string query = "SELECT ISNULL(MAX(ItemNo), 0) + 1 FROM ItemDetail";

            using (SqlConnection dbcon = new SqlConnection("Data Source=DESKTOP-6JKB17U\\SQL;Initial Catalog=PawnBrokingNew;User ID=sa;Password=1234;")) // Replace with your actual connection string
            {

                using (SqlCommand cmd = new SqlCommand(query, dbcon))
                {
                    try
                    {
                        dbcon.Open();
                        object result = cmd.ExecuteScalar();

                        txtID.Text = result != null ? result.ToString() : "1";
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

        ItemDetailBLL item = new ItemDetailBLL();
        ItemDetailDAL dal = new ItemDetailDAL();
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtItemName.Text))
            {
                MessageBox.Show("Enter Item Name", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtItemName.Focus();
                return;
            }
            if (cmbitemtype.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an Item Type", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbitemtype.Focus();
                return;
            }

            item.ItemNo = int.Parse(txtID.Text);
            item.ItemName = txtItemName.Text;
            item.ItemType = cmbitemtype.Text;

            if (!UpdateMode)
            {
                bool success = dal.Insert(item);

                if (success)
                {
                    MessageBox.Show("New Item Successfully added", "Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear();
                    DisplayNextID();
                }
                else
                {
                    MessageBox.Show("Failed to add Item", "Item", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                item.ItemCode = int.Parse(txtID.Text);

                bool success = dal.Update(item);

                if (success)
                {
                    MessageBox.Show("Item Updated Successfully", "Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear();
                    btnFind.Enabled = true;
                    UpdateMode = false;
                    DisplayNextID();
                }
                else
                {
                    MessageBox.Show("Failed to update Item", "Item", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Please select a Item to delete.", "Item", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            item.ItemNo = int.Parse(txtID.Text);

            bool success = dal.Delete(item);

            if (success)
            {
                MessageBox.Show("Item Deleted Successfully", "Item", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Failed to Delete Item", "Item", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
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
            txtID.Text = "";
            txtItemName.Text = "";
            cmbitemtype.Text = "";
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            panelFind.Location = new Point(12, 23);

            btnEdit.Enabled = true;
            btnSave.Enabled = false;

            DataTable dt = dal.Select();
            dgvItems.DataSource = dt;

            dgvItems.Columns["ItemCode"].Visible = false;

            dgvItems.Columns["ItemType"].HeaderText = "பொருள் வகை";
            dgvItems.Columns["ItemName"].HeaderText = "பொருள்";
            dgvItems.Columns["ItemNo"].HeaderText = "வ.எண்";

            dgvItems.Columns["ItemName"].Width = 150;
            dgvItems.Columns["ItemNo"].Width = 80;

            foreach (DataGridViewColumn column in dgvItems.Columns)
            {
                column.HeaderCell.Style.BackColor = Color.LightSteelBlue;
                column.DefaultCellStyle.Font = new Font("", 9, FontStyle.Bold);
                column.HeaderCell.Style.Font = new Font("", 13, FontStyle.Bold);

                //dgvItems.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                //dgvItems.DefaultCellStyle.BackColor = Color.White;
            }

            dgvItems.EnableHeadersVisualStyles = false;
        }

        #region DGV Cell Click
        private void dgvItems_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvItems.Rows.Count)
            {
                DataGridViewRow selectedRow = dgvItems.Rows[e.RowIndex];

                txtID.Text = selectedRow.Cells["ItemNo"].Value?.ToString();
                txtItemName.Text = selectedRow.Cells["ItemName"].Value?.ToString();
                cmbitemtype.Text = selectedRow.Cells["ItemType"].Value?.ToString();

                UpdateMode = true;

                btnDelete.Enabled = true;
                panelFind.Location = new Point(12, 345);
                btnFind.Enabled = false;
            }
        }
        private void dgvItems_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvItems.Rows.Count)
            {
                DataGridViewRow selectedRow = dgvItems.Rows[e.RowIndex];

                txtID.Text = selectedRow.Cells["ItemNo"].Value?.ToString();
                txtItemName.Text = selectedRow.Cells["ItemName"].Value?.ToString();
                cmbitemtype.Text = selectedRow.Cells["ItemType"].Value?.ToString();

                UpdateMode = true;

                btnDelete.Enabled = true;
                panelFind.Location = new Point(12, 345);
                btnFind.Enabled = false;
            }
        }

        private void dgvItems_RowHeaderMouseClick_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvItems.Rows.Count)
            {
                DataGridViewRow selectedRow = dgvItems.Rows[e.RowIndex];

                txtID.Text = selectedRow.Cells["ItemNo"].Value?.ToString();
                txtItemName.Text = selectedRow.Cells["ItemName"].Value?.ToString();
                cmbitemtype.Text = selectedRow.Cells["ItemType"].Value?.ToString();

                UpdateMode = true;

                btnDelete.Enabled = true;
                panelFind.Location = new Point(12, 345);
                btnFind.Enabled = false;
            }
        }

        #endregion

        private void txtboxSearch_TextChanged(object sender, EventArgs e)
        {
            string keywords = txtboxSearch.Text;

            string selectedField = cmbSelectItem.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedField))
            {
                if (!string.IsNullOrWhiteSpace(keywords))
                {
                    DataTable dt = dal.Search(selectedField, keywords);
                    dgvItems.DataSource = dt;
                }
                else
                {
                    DataTable dt = dal.Select();
                    dgvItems.DataSource = dt;
                }
            }
            else
            {
                MessageBox.Show("Please select ItemType or ItemName", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtboxSearch.Text = "";
            }
        }
    }
}
    