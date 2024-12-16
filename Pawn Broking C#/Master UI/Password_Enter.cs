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

namespace Pawn_Broking.Master_UI
{
    public partial class Password_Enter : Form
    {
        public Password_Enter()
        {
            InitializeComponent();
        }

        private void PasswordEnter_Load(object sender, EventArgs e)
        {

        }

        EditPasswordBLL EditPasswordBLL = new EditPasswordBLL();
        EditPasswordDAL dal = new EditPasswordDAL();
        public static string loggedIn;

        private void btnOk_Click(object sender, EventArgs e)
        {
            EditPasswordBLL.EPassword = txtPassword.Text;
            bool success = dal.loginCheck(EditPasswordBLL); 

            if (success)
            {
                //MessageBox.Show("Password Successfull", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                loggedIn = EditPasswordBLL.EPassword;
 
                this.DialogResult = DialogResult.OK;
                this.Close(); 
            }
            else
            {            
                MessageBox.Show("Password Wrong.Please Try Again", "Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }
    }
}
