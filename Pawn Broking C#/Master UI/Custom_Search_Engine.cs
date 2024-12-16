using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using ComponentFactory.Krypton.Toolkit;


namespace Pawn_Broking.UI
{
    public partial class Custom_Search_Engine : Form
    {
        public Custom_Search_Engine()
        {
            InitializeComponent();
         
        }

        private void CustomSearchEng_Load(object sender, EventArgs e)
        {

        }

        private void button5_MouseHover(object sender, EventArgs e)
        {
            button5.BackColor = Color.LightBlue;
            
        }
        private void button5_MouseLeave(object sender, EventArgs e)
        {
            button5.BackColor = SystemColors.Control;
        }
    }
}
