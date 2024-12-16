using Pawn_Broking.BLL;
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
using System.Xml.Linq;

namespace Pawn_Broking.Master_UI
{
    public partial class Edit_Password_Form : Form
    {      
        EditPasswordBLL passwordBLL = new EditPasswordBLL();
        EditPasswordDAL passwordDAL = new EditPasswordDAL();
        public Edit_Password_Form()
        {
            InitializeComponent();
        }
         
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter the password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return;
            }

            passwordBLL.EPassword = txtName.Text;

            bool success = passwordDAL.Insert(passwordBLL);

            if (success)
            {
                MessageBox.Show("New Password Created Successfully.");

            }
            else
            {
                MessageBox.Show("Failed to Add New Password.");
            }
        }
    }
}
