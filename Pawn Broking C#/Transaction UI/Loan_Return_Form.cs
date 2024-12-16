using Pawn_Broking.Master_UI;
using Pawn_Broking.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Pawn_Broking.UI
{
    public partial class LoanReturnForm : Form
    {
        private Dictionary<Control, Control> controlFocusMap;
        public LoanReturnForm()
        {
            InitializeComponent();
            ApplyEnterLeaveEvents(this);

            #region Enter Function
            controlFocusMap = new Dictionary<Control, Control>
            {
                {DTPRDate,txtloanno },
                {txtproof,txtmonth },
                {txtmonth,txtintper },
                {txtintper,txtInterestAmt },
                {txtInterestAmt,txtDiscount },
                {txtDiscount,txtRetAmt },
                {txtRetAmt,BtnSave }

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

        #region Textbox Backcolor function
        private void ApplyEnterLeaveEvents(Control parentControl)
        {
            foreach (Control control in parentControl.Controls)
            {
                if (control is TextBox)
                {
                    control.Enter += TextBox_Enter;
                    control.Leave += TextBox_Leave;
                }

                if (control.HasChildren)
                {
                    ApplyEnterLeaveEvents(control);
                }
            }
        }
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
        #endregion

        private bool SaveFlg = false;
        private void LoanReturnForm_Load(object sender, EventArgs e)
        {
            BtnDelete.Enabled = false;
            FlexLoan.Location = new Point(209, 627);
            NewRec();
            NewRecNo();
            NewRecId();
            RetNo();

            //txtintper.ReadOnly = true;

            cmbfind.Items.Add("அடகு எண்"); // "Loan Number"
            cmbfind.Items.Add("பெயர்"); // "Name"
            cmbfind.Items.Add("திருப்பிய எண்"); // "RetNumber"
            cmbfind.SelectedIndex = 0;
            txtDiscount.TextChanged -= txtDiscount_TextChanged;
            txtDiscount.Text = "0";
            txtDiscount.TextChanged += txtDiscount_TextChanged;

            DTPRDate.ValueChanged -= DTPRDate_ValueChanged;
            DTPRDate.Value = DateTime.Now;
            DTPRDate.ValueChanged += DTPRDate_ValueChanged;

            txtReceiptCG.Enabled = false;

            this.KeyPreview = true;

            FlexItemDetailAlign();
        }
        #region Load Functions
        //private void LastCompanyId()
        //{
        //    string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        con.Open();
        //        using (SqlCommand cmd = new SqlCommand("SELECT CompanyId FROM CompanyMaster WHERE CompanyId = 1", dbcon))
        //        using (SqlDataReader reader = cmd.ExecuteReader())
        //        {
        //            if (reader.Read())
        //            {
        //                cmbCompany.SelectedValue = reader.GetValue(0).ToString();
        //                // txtAmount.Focus(); // Uncomment if focusing on txtAmount is needed
        //            }
        //        }
        //    }
        //}
        public void NewRec()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(MAX(Id) + 1, 1) FROM LoanReturn", conn))
                {
                    object result = cmd.ExecuteScalar();
                    lblid.Text = result.ToString();
                }
            }
        }
        public void RetNo()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(MAX(RetNo) + 1, 1) FROM LoanReturn", conn))
                {
                    object result = cmd.ExecuteScalar();
                    lblRetNo.Text = result.ToString();
                }

                txtdeliveryno.Text = "";

                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 dbo.fn_LoanChar(dbo.LoanReturn.Delno) FROM dbo.LoanReturn", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtdeliveryno.Text = reader.IsDBNull(0) ? "1" : reader.GetString(0);
                        }
                    }
                }

                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 CAST(ISNULL(MAX(dbo.fn_LoanNum(LoanReturn.Delno)) + 1, 1) AS nvarchar(50)) FROM LoanReturn", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtdeliveryno.Text += reader.IsDBNull(0) ? "1" : reader.GetString(0);
                        }
                        else
                        {
                            txtdeliveryno.Text = "1";
                        }
                    }
                }
            }
        }
        public void NewRecId()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(MAX(IntrestId) + 1, 1) FROM IntrestCollection", conn))
                {
                    object result = cmd.ExecuteScalar();
                    lblintId.Text = result.ToString();
                }
            }
        }
        public void NewRecNo()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(MAX(IntrestNo) + 1, 1) FROM IntrestCollection", conn))
                {
                    object result = cmd.ExecuteScalar();
                    lblintno.Text = result.ToString();
                }
            }
        }
        private void FlexItemDetailAlign()
        {
            FlexItemDetails.ColumnCount = 5;

            FlexItemDetails.Columns[0].Width = 90; // sno
            FlexItemDetails.Columns[1].Width = 300; // item details
            FlexItemDetails.Columns[2].Width = 100; // Qty
            FlexItemDetails.Columns[3].Visible = false; // ItemCode 
            FlexItemDetails.Columns[4].Width = 273; // Identification

            FlexItemDetails.Columns[0].HeaderText = "வ.எண்"; // sno
            FlexItemDetails.Columns[1].HeaderText = "பொருளின் விவரம்"; // Item Details
            FlexItemDetails.Columns[2].HeaderText = "உருப்படி"; //Qty
            FlexItemDetails.Columns[3].HeaderText = "ItemCode"; // ItemCode
            FlexItemDetails.Columns[4].HeaderText = "அடையாளக் குறிகள்"; // Identification

            FlexItemDetails.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; // Align right center
            FlexItemDetails.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;  // Align left center
            FlexItemDetails.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; // Align right center
            FlexItemDetails.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; // Align right center
            FlexItemDetails.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;  // Align left center

            FlexItemDetails.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            FlexItemDetails.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            for (int i = 2; i <= 3; i++)
            {
                FlexItemDetails.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            FlexItemDetails.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            foreach (DataGridViewColumn column in FlexItemDetails.Columns)
            {
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);

            }

            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.BackColor = Color.Purple;
            headerStyle.ForeColor = Color.Yellow;
            headerStyle.Font = new Font("Arial", 12, FontStyle.Bold);

            headerStyle.SelectionBackColor = Color.Purple;

            FlexItemDetails.ColumnHeadersDefaultCellStyle = headerStyle;
        }
        #endregion

        #region TxtLoanNo and Flexloan Keyup,down Functions
        private void txtloanno_KeyUp(object sender, KeyEventArgs e)
        {
            FlexLoan.Visible = true;
            FlexLoan.Location = new Point(131, 164);
            if (!txtloanno.ReadOnly)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT TOP 100 * FROM LoanFlexQry WHERE LoanNo LIKE @LoanNo AND Status = 'L' AND CompanyId = @CompanyId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@LoanNo", txtloanno.Text.Trim() + "%");
                        cmd.Parameters.AddWithValue("@CompanyId", 1);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);
                            FlexLoan.DataSource = dt;
                        }
                    }

                    FlexfindAlign1();
                }
            }
        }
        private void txtloanno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter)
            {
                if (FlexLoan.Visible && FlexLoan.Rows.Count > 0)
                {
                    foreach (DataGridViewCell cell in FlexLoan.Rows[0].Cells)
                    {
                        if (cell.Visible)
                        {
                            FlexLoan.CurrentCell = cell;
                            break;
                        }
                    }
                    FlexLoan.Focus();
                }
            }
        }
        private void FlexLoan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && FlexLoan.CurrentRow != null)
            {
                DataGridViewRow selectedRow = FlexLoan.CurrentRow;

                e.Handled = true;
                FlexLoan_DoubleClick(sender, e);

                FlexLoan.Visible = false;
                e.SuppressKeyPress = true;
            }
        }
        private void FlexLoan_DoubleClick(object sender, EventArgs e)
        {
            if (FlexLoan.Rows.Count > 0)
            {
                GetLoan();
                GetBalance();
                GetItemDetails();
                GetIntPending();
            }

            txtproof.Focus();
            FlexLoan.Visible = false;

            decimal interestAmt = ParseDecimal(txtInterestAmt.Text);
            decimal loanAmount = ParseDecimal(lblAmount.Text);
            decimal discount = ParseDecimal(txtDiscount.Text);
            decimal totalValue = interestAmt + loanAmount;

            lblTotalValue.Text = totalValue.ToString("0.00");
            decimal returnAmt = totalValue - discount;
            txtRetAmt.Text = returnAmt.ToString("0.00");

            decimal balanceAmt = totalValue - returnAmt - discount;
            lblBalanceAmt.Text = balanceAmt.ToString("0.00");
        }
        private decimal ParseDecimal(string input)
        {
            if (decimal.TryParse(input, out decimal result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        decimal CurrIntPending; DateTime IntPaidUpto; decimal IntPending; decimal AccIntAmtOne;
        decimal TotAccIntAmt; decimal PrinciplePaid; decimal IntPaid;

        #region Save Functions
        private void BtnSave_Click(object sender, EventArgs e)
        {
            CalcIntpending();

            if (CheckLog())
            {
                if (!SaveFlg)
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        //string insertLoanReturnQuery = "INSERT INTO LoanReturn (ID) VALUES (@ID)";
                        //using (SqlCommand cmdInsert = new SqlCommand(insertLoanReturnQuery, conn))
                        //{
                        //    cmdInsert.Parameters.AddWithValue("@ID", lblid.Text);
                        //    cmdInsert.ExecuteNonQuery();
                        //}

                        Store();
                        SaveToLedger();
                        MessageBox.Show("One Record Inserted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        DateTime intPaidUpto = Convert.ToDateTime(lblIntPaidUpto.Text)
                              .AddMonths(Convert.ToInt32(txtmonth.Text));


                        string updateCompanyMasterQuery = "UPDATE CompanyMaster SET LastRetNo = LastRetNo + 1 WHERE CompanyId = @CompanyId";
                        using (SqlCommand cmdUpdateCompany = new SqlCommand(updateCompanyMasterQuery, conn))
                        {
                            cmdUpdateCompany.Parameters.AddWithValue("@CompanyId", 1);
                            cmdUpdateCompany.ExecuteNonQuery();
                        }

                        string updateLoanQuery = @"UPDATE Loan
                                         SET BalanceAmt = @BalanceAmt,
                                             AcctIntAmt = @AcctIntAmt,
                                             IntPaidUpto = @IntPaidUpto,
                                             Status = @Status,
                                             IntPending = @IntPending
                                         WHERE id = @LoanId";

                        decimal currentAcctIntAmt = GetCurrentAcctIntAmtFromLoanId(lblloanid.Text, conn);
                        decimal retAmt = Convert.ToDecimal(txtRetAmt.Text);

                        using (SqlCommand cmdUpdateLoan = new SqlCommand(updateLoanQuery, conn))
                        {
                            cmdUpdateLoan.Parameters.AddWithValue("@BalanceAmt", lblBalanceAmt.Text);
                            cmdUpdateLoan.Parameters.AddWithValue("@AcctIntAmt", currentAcctIntAmt + retAmt);
                            cmdUpdateLoan.Parameters.AddWithValue("@IntPaidUpto", intPaidUpto.ToString("yyyy-MM-dd"));

                            if (Convert.ToDecimal(lblBalanceAmt.Text) == 0)
                            {
                                cmdUpdateLoan.Parameters.AddWithValue("@Status", "R");
                            }
                            else
                            {
                                string currentStatus = GetCurrentStatus(lblloanid.Text);
                                cmdUpdateLoan.Parameters.AddWithValue("@Status", currentStatus);
                            }

                            cmdUpdateLoan.Parameters.AddWithValue("@IntPending", CurrIntPending);
                            cmdUpdateLoan.Parameters.AddWithValue("@LoanId", lblloanid.Text);
                            cmdUpdateLoan.ExecuteNonQuery();

                        }

                        if (Convert.ToDecimal(txtRetAmt.Text) > 0)
                        {
                            StoreIntCol(conn);
                            //string insertInterestQuery = "INSERT INTO IntrestCollection (IntrestID) VALUES (@IntrestID)";
                            //using (SqlCommand cmdInsertInterest = new SqlCommand(insertInterestQuery, conn))
                            //{
                            //    cmdInsertInterest.Parameters.AddWithValue("@IntrestID", lblintId.Text);
                            //    cmdInsertInterest.ExecuteNonQuery();
                            //}
                        }
                    }
                    ResetForm();
                }
                else if (SaveFlg)
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string updateLoanReturnQuery = "UPDATE LoanReturn SET ID = @ID WHERE id = @LoanReturnId";
                        using (SqlCommand cmdUpdateLoanReturn = new SqlCommand(updateLoanReturnQuery, conn))
                        {
                            cmdUpdateLoanReturn.Parameters.AddWithValue("@ID", lblid.Text);
                            cmdUpdateLoanReturn.Parameters.AddWithValue("@LoanReturnId", lblid.Text);
                            cmdUpdateLoanReturn.ExecuteNonQuery();
                        }

                        Store();
                        SaveToLedger();
                        MessageBox.Show("One Record Updated", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        string updateLoanQuery = @"UPDATE Loan
                                         SET BalanceAmt = @BalanceAmt,
                                             AcctIntAmt = @AcctIntAmt,
                                             IntPaidUpto = @IntPaidUpto,
                                             Status = @Status,
                                             IntPending = @IntPending
                                         WHERE id = @LoanId";
                        using (SqlCommand cmdUpdateLoan = new SqlCommand(updateLoanQuery, conn))
                        {
                            cmdUpdateLoan.Parameters.AddWithValue("@BalanceAmt", lblBalanceAmt.Text);
                            cmdUpdateLoan.Parameters.AddWithValue("@AcctIntAmt", Convert.ToDecimal(txtRetAmt.Text));
                            cmdUpdateLoan.Parameters.AddWithValue("@IntPaidUpto", IntPaidUpto.ToString("yyyy-MM-dd"));
                            cmdUpdateLoan.Parameters.AddWithValue("@Status", Convert.ToDecimal(lblBalanceAmt.Text) == 0 ? "R" : "O");
                            cmdUpdateLoan.Parameters.AddWithValue("@IntPending", CurrIntPending);
                            cmdUpdateLoan.Parameters.AddWithValue("@LoanId", lblloanid.Text);
                            cmdUpdateLoan.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
        private bool CheckLog()
        {
            if (string.IsNullOrWhiteSpace(txtloanno.Text))
            {
                MessageBox.Show("Enter Loan No", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtmonth.Text))
            {
                MessageBox.Show("Enter Months", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtmonth.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtInterestAmt.Text))
            {
                MessageBox.Show("Enter Interest Amount", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtInterestAmt.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtdeliveryno.Text) || txtdeliveryno.Text == ".")
            {
                MessageBox.Show("Enter Delivery No", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtdeliveryno.Focus();
                return false;
            }
            else if (string.IsNullOrWhiteSpace(txtRetAmt.Text))
            {
                MessageBox.Show("Enter Return Amount", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtRetAmt.Focus();
                return false;
            }

            return true;
        }
        private void CalcIntpending()
        {
            try
            {
                decimal retAmt = string.IsNullOrWhiteSpace(txtRetAmt.Text) ? 0 : Convert.ToDecimal(txtRetAmt.Text);
                decimal discount = string.IsNullOrWhiteSpace(txtDiscount.Text) ? 0 : Convert.ToDecimal(txtDiscount.Text);
                decimal interestAmt = string.IsNullOrWhiteSpace(txtInterestAmt.Text) ? 0 : Convert.ToDecimal(txtInterestAmt.Text);

                if ((retAmt + discount) >= (IntPending + interestAmt))
                {
                    CurrIntPending = 0;
                }
                else
                {
                    CurrIntPending = (IntPending + interestAmt) - (discount + retAmt);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("One or more input fields contain invalid values. Please correct them.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private string GetCurrentStatus(string loanId)
        {
            string status = string.Empty;
            string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Status FROM Loan WHERE ID = @LoanId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@LoanId", loanId);
                    status = cmd.ExecuteScalar().ToString();
                }
            }
            return status;
        }

        private void Store()
        {
            try
            {
                AccIntCalc();

                string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("INSERT INTO LoanReturn (ID,Loanid, RetNo, RetDate, Months, IntPerc, IntAmt, TotalAmt, Discount, RetAmt, BalanceAmt, PrevintPending, Proof, Delno, LoanAmt, CurLoanAmt, IntPaidUpto) VALUES (@ID,@Loanid, @RetNo, @RetDate, @Months, @IntPerc, @IntAmt, @TotalAmt, @Discount, @RetAmt, @BalanceAmt, @PrevintPending, @Proof, @Delno, @LoanAmt, @CurLoanAmt, @IntPaidUpto)", conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", lblid.Text);
                        cmd.Parameters.AddWithValue("@Loanid", Convert.ToInt32(lblloanid.Text));
                        cmd.Parameters.AddWithValue("@RetNo", lblRetNo.Text.Trim());
                        cmd.Parameters.AddWithValue("@RetDate", DateTime.Parse(DTPRDate.Text).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@Months", txtmonth.Text.Trim());
                        cmd.Parameters.AddWithValue("@IntPerc", txtintper.Text.Trim());
                        cmd.Parameters.AddWithValue("@IntAmt", Convert.ToDecimal(txtInterestAmt.Text));
                        cmd.Parameters.AddWithValue("@TotalAmt", Convert.ToDecimal(lblTotalValue.Text));
                        cmd.Parameters.AddWithValue("@Discount", Convert.ToDecimal(txtDiscount.Text));
                        cmd.Parameters.AddWithValue("@RetAmt", Convert.ToDecimal(txtRetAmt.Text));
                        cmd.Parameters.AddWithValue("@BalanceAmt", Convert.ToDecimal(lblBalanceAmt.Text));
                        cmd.Parameters.AddWithValue("@PrevintPending", IntPending);
                        cmd.Parameters.AddWithValue("@Proof", txtproof.Text.Trim());
                        cmd.Parameters.AddWithValue("@Delno", txtdeliveryno.Text.Trim());
                        cmd.Parameters.AddWithValue("@LoanAmt", Convert.ToDecimal(lblAmount.Text));
                        cmd.Parameters.AddWithValue("@CurLoanAmt", Convert.ToDecimal(lblBalanceAmt.Text));
                        cmd.Parameters.AddWithValue("@IntPaidUpto", DateTime.Parse(lblIntPaidUpto.Text).AddMonths(1).ToString("yyyy-MM-dd"));

                        cmd.ExecuteNonQuery();
                    }

                    if (Convert.ToDecimal(lblBalanceAmt.Text) == 0)
                    {
                        using (SqlCommand updateCmd = new SqlCommand("UPDATE LoanReturn SET AcctIntAmt = @AcctIntAmt WHERE Loanid = @Loanid", conn))
                        {
                            updateCmd.Parameters.AddWithValue("@AcctIntAmt", TotAccIntAmt);
                            updateCmd.Parameters.AddWithValue("@Loanid", Convert.ToInt32(lblloanid.Text));
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void AccIntCalc()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT AcctIntPerc, Amount FROM Loan WHERE LoanNo = @LoanNo", conn))
                {
                    cmd.Parameters.AddWithValue("@LoanNo", txtloanno.Text);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        decimal AAmount = 0;
                        decimal AIntamt = 0;

                        if (reader.Read())
                        {
                            AAmount = Convert.ToDecimal(reader["Amount"]);
                            AIntamt = Convert.ToDecimal(reader["AcctIntPerc"]);
                        }

                        AccIntAmtOne = Math.Round((AAmount * AIntamt) / 100, 2);
                        TotAccIntAmt = AccIntAmtOne * Convert.ToDecimal(lblTotMonth.Text);
                    }
                }
            }
        }

        int UID; decimal DisInt; decimal DisPrinciple;
        private void Calc()
        {
            decimal IP;
            decimal PaidAmount;

            IP = (IntPending + Convert.ToDecimal(txtInterestAmt.Text));
            PaidAmount = Convert.ToDecimal(txtDiscount.Text) + Convert.ToDecimal(txtRetAmt.Text);

            if (PaidAmount <= IP)
            {
                PrinciplePaid = 0;
                DisPrinciple = 0;
                IntPaid = Convert.ToDecimal(txtRetAmt.Text);
                DisInt = Convert.ToDecimal(txtDiscount.Text);
            }
            else
            {
                IntPaid = IP;
                PrinciplePaid = PaidAmount - IP;

                if (Convert.ToDecimal(txtDiscount.Text) <= IP)
                {
                    DisInt = Convert.ToDecimal(txtDiscount.Text);
                    DisPrinciple = 0;
                }
                else
                {
                    DisInt = IP;
                    DisPrinciple = Convert.ToDecimal(txtDiscount.Text) - IP;
                }

                //IntPaid = IntPaid;  
                //PrinciplePaid = PrinciplePaid;  
            }
        }

        private void SaveToLedger()
        {
            string Nar = "Return" + txtloanno.Text.Trim() + "-" + lblName.Text.Trim();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    Calc();

                    if (PrinciplePaid > 0)
                    {
                        LedgerEntry(conn, DateTime.Parse(DTPRDate.Text), 2, 1, "Return" + lblid.Text, Nar, 0, PrinciplePaid, UID);
                        LedgerEntry(conn, DateTime.Parse(DTPRDate.Text), 1, 2, "Return" + lblid.Text, Nar, PrinciplePaid, 0, UID);
                    }

                    if (IntPaid > 0)
                    {
                        LedgerEntry(conn, DateTime.Parse(DTPRDate.Text), 1, 3, "Return" + lblid.Text, Nar, IntPaid, 0, UID);
                        LedgerEntry(conn, DateTime.Parse(DTPRDate.Text), 3, 1, "Return" + lblid.Text, Nar, 0, IntPaid, UID);
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while saving to Ledger: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public int gCID;
        private void LedgerEntry(SqlConnection conn, DateTime TDate, long LedId, long DLedId, string RefNo, string Narration, decimal Debit, decimal Credit, int? Id = null)
        {
            try
            {
                string query = "SELECT ISNULL(MAX(TransId), 0) + 1 FROM LedgerTransaction";
                SqlCommand cmdMaxId = new SqlCommand(query, conn);
                long tid = Convert.ToInt64(cmdMaxId.ExecuteScalar());

                string insertQuery = @"INSERT INTO LedgerTransaction (TransId, TransDate, LedgerId, RefNo, Narration, Debit, Credit, DetsId, Companyid) 
                               VALUES (@TransId, @TransDate, @LedgerId, @RefNo, @Narration, @Debit, @Credit, @DetsId, @Companyid)";
                using (SqlCommand cmdInsert = new SqlCommand(insertQuery, conn))
                {
                    cmdInsert.Parameters.AddWithValue("@TransId", tid);
                    cmdInsert.Parameters.AddWithValue("@TransDate", TDate.ToString("dd/MMM/yyyy"));
                    cmdInsert.Parameters.AddWithValue("@LedgerId", LedId);
                    cmdInsert.Parameters.AddWithValue("@RefNo", RefNo);
                    cmdInsert.Parameters.AddWithValue("@Narration", Narration);
                    cmdInsert.Parameters.AddWithValue("@Debit", Debit);
                    cmdInsert.Parameters.AddWithValue("@Credit", Credit);
                    cmdInsert.Parameters.AddWithValue("@DetsId", DLedId);
                    cmdInsert.Parameters.AddWithValue("@Companyid", gCID);

                    cmdInsert.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while inserting into LedgerTransaction: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StoreIntCol(SqlConnection conn)
        {
            string insertInterestCollectionQuery = @"INSERT INTO IntrestCollection 
        (IntrestID,AccId, IntrestNo, Loanid, LoanNo, IntColDate, Months, IntPer, IntAmt, CollectionAmt, IntPaidUpto, CurLoanAmt, Refno)
        VALUES 
        (@IntrestID,@AccId, @IntrestNo, @Loanid, @LoanNo,@IntColDate, @Months, @IntPer, @IntAmt, @CollectionAmt, @IntPaidUpto, @CurLoanAmt, @Refno)";

            IntPaidUpto = Convert.ToDateTime(lblloandate.Text)
                          .AddMonths(Convert.ToInt32(txtmonth.Text));

            using (SqlCommand cmdInsertInterestCol = new SqlCommand(insertInterestCollectionQuery, conn))
            {
                cmdInsertInterestCol.Parameters.AddWithValue("@IntrestID", lblintId.Text);
                cmdInsertInterestCol.Parameters.AddWithValue("@AccId", 1);
                cmdInsertInterestCol.Parameters.AddWithValue("@IntrestNo", Convert.ToInt32(lblintno.Text));
                cmdInsertInterestCol.Parameters.AddWithValue("@Loanid", Convert.ToInt32(lblloanid.Text));
                cmdInsertInterestCol.Parameters.AddWithValue("@LoanNo", txtloanno.Text.Trim());
                cmdInsertInterestCol.Parameters.AddWithValue("@IntColDate", DateTime.Parse(DTPRDate.Text).ToString("yyyy-MM-dd"));
                cmdInsertInterestCol.Parameters.AddWithValue("@Months", txtmonth.Text.Trim());
                cmdInsertInterestCol.Parameters.AddWithValue("@IntPer", txtintper.Text.Trim());
                cmdInsertInterestCol.Parameters.AddWithValue("@IntAmt", Convert.ToDecimal(txtInterestAmt.Text));
                cmdInsertInterestCol.Parameters.AddWithValue("@CollectionAmt", Convert.ToDecimal(txtRetAmt.Text));
                cmdInsertInterestCol.Parameters.AddWithValue("@IntPaidUpto", IntPaidUpto.ToString("yyyy-MM-dd"));
                cmdInsertInterestCol.Parameters.AddWithValue("@CurLoanAmt", Convert.ToDecimal(lblBalanceAmt.Text));
                cmdInsertInterestCol.Parameters.AddWithValue("@Refno", "LR" + Convert.ToInt32(lblid.Text));

                cmdInsertInterestCol.ExecuteNonQuery();
            }
        }

        private decimal GetCurrentAcctIntAmtFromLoanId(string loanId, SqlConnection conn)
        {
            string query = "SELECT AcctIntAmt FROM Loan WHERE id = @LoanId";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@LoanId", loanId);
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToDecimal(result) : 0;
            }
        }

        #endregion

        #region Find Functions
        public void FlexfindAlign1()
        {
            FlexLoan.Width = 600;

            FlexLoan.Columns[0].Visible = false;   // id (hidden)
            FlexLoan.Columns[1].Visible = false;   // companyid (hidden)
            FlexLoan.Columns[2].Width = 200; // loanNo
            FlexLoan.Columns[3].Width = 200; // loanDate
            FlexLoan.Columns[4].Width = 200; // Amount
            FlexLoan.Columns[5].Visible = false;   // Salutation (hidden)
            FlexLoan.Columns[6].Visible = false;   // CustomerInitial (hidden)
            FlexLoan.Columns[7].Visible = false;   // CustomerName (hidden)
            FlexLoan.Columns[8].Visible = false;   // FHType (hidden)
            FlexLoan.Columns[9].Visible = false;   // FHName (hidden)
            FlexLoan.Columns[10].Visible = false;  // Town (hidden)
            FlexLoan.Columns[11].Visible = false;  // ItemType (hidden)
            FlexLoan.Columns[12].Visible = false;  // Gram (hidden)
            FlexLoan.Columns[13].Visible = false;  // TotalQty (hidden)
            FlexLoan.Columns[14].Visible = false;  // TotalValue (hidden)
            FlexLoan.Columns[15].Visible = false;  // BalanceAmt (hidden)
            FlexLoan.Columns[16].Visible = false;  // IntPerc (hidden)
            FlexLoan.Columns[17].Visible = false;  // AcctIntPerc (hidden)
            FlexLoan.Columns[18].Visible = false;  // AcctIntAmt (hidden)
            FlexLoan.Columns[19].Visible = false;  // IntAmt (hidden)
            FlexLoan.Columns[20].Visible = false;  // IntPaidUpto (hidden)
            FlexLoan.Columns[21].Visible = false;  // Address (hidden)
            FlexLoan.Columns[22].Visible = false;  // (hidden)
            FlexLoan.Columns[23].Visible = false;  // CustId (hidden)
            FlexLoan.Columns[24].Visible = false;  // (hidden)

            foreach (DataGridViewColumn column in FlexLoan.Columns)
            {
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);

            }
            if (FlexLoan.Rows.Count > 0)
            {
                FlexLoan.ClearSelection();
                FlexLoan.Rows[0].Selected = true;
            }
        }
        private void btnFind_Click(object sender, EventArgs e)
        {
            pctFind.Location = new Point(7, 1);

            string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
            string query = "SELECT TOP 100 * FROM LoanReturnFindQry ORDER BY RetNo DESC";

            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                try
                {
                    dbcon.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(query, dbcon);
                    DataTable findTable = new DataTable();
                    adapter.Fill(findTable);

                    foreach (DataColumn column in findTable.Columns)
                    {
                        Console.WriteLine(column.ColumnName);  // Print column names to verify
                    }

                    if (findTable.Rows.Count > 0)
                    {
                        pctFind.Visible = true;
                        txtFind.ReadOnly = false;

                        pctFind.Location = new Point(55, 51);
                        FlexFind.Enabled = true;

                        FlexFind.DataSource = findTable;

                        FindGridAlign();
                        txtFind.Focus();
                        BtnDelete.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("Record Not Found", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }
        private void FindGridAlign()
        {

            FlexFind.Columns[0].HeaderText = "ID";
            FlexFind.Columns[1].HeaderText = "CompanyId";
            FlexFind.Columns[2].HeaderText = "தி.பெ.எண்";
            FlexFind.Columns[3].HeaderText = "தேதி";
            FlexFind.Columns[4].HeaderText = "அடகு எண்";
            FlexFind.Columns[5].HeaderText = "LoanDate";
            FlexFind.Columns[6].HeaderText = "IntPaidUpto";
            FlexFind.Columns[7].HeaderText = "Salutation";
            FlexFind.Columns[8].HeaderText = "CustomrInitial";
            FlexFind.Columns[9].HeaderText = "பெயர்";
            FlexFind.Columns[10].HeaderText = "FHType";
            FlexFind.Columns[11].HeaderText = "FHName";
            FlexFind.Columns[12].HeaderText = "முகவரி";
            FlexFind.Columns[13].HeaderText = "Town";
            FlexFind.Columns[14].HeaderText = "Months";
            FlexFind.Columns[15].HeaderText = "LoanAmt";
            FlexFind.Columns[16].HeaderText = "IntPerc";
            FlexFind.Columns[17].HeaderText = "IntAmt";
            FlexFind.Columns[18].HeaderText = "TotalAmt";
            FlexFind.Columns[19].HeaderText = "Discount";
            FlexFind.Columns[20].HeaderText = "RetAmt";
            FlexFind.Columns[21].HeaderText = "மீதத்தொகை";
            FlexFind.Columns[22].HeaderText = "Status";
            FlexFind.Columns[23].HeaderText = "ItemType";
            FlexFind.Columns[24].HeaderText = "Gram";
            FlexFind.Columns[25].HeaderText = "TotalQty";
            FlexFind.Columns[26].HeaderText = "Loanid";

            FlexFind.Columns[0].Visible = false;
            FlexFind.Columns[1].Visible = false;
            FlexFind.Columns[2].Width = 150;
            FlexFind.Columns[3].Width = 150;
            FlexFind.Columns[4].Width = 150;
            FlexFind.Columns[5].Visible = false;
            FlexFind.Columns[6].Visible = false;
            FlexFind.Columns[7].Visible = false;
            FlexFind.Columns[8].Visible = false;
            FlexFind.Columns[9].Width = 300;
            FlexFind.Columns[10].Visible = false;
            FlexFind.Columns[11].Visible = false;
            FlexFind.Columns[12].Width = 420;
            FlexFind.Columns[13].Visible = false;
            FlexFind.Columns[14].Visible = false;
            FlexFind.Columns[15].Visible = false;
            FlexFind.Columns[16].Visible = false;
            FlexFind.Columns[17].Visible = false;
            FlexFind.Columns[18].Visible = false;
            FlexFind.Columns[19].Visible = false;
            FlexFind.Columns[20].Visible = false;
            FlexFind.Columns[21].Width = 145;
            FlexFind.Columns[22].Visible = false;
            FlexFind.Columns[23].Visible = false;
            FlexFind.Columns[24].Visible = false;
            FlexFind.Columns[25].Visible = false;
            FlexFind.Columns[26].Visible = false;

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
            string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
            string query = "";
            string argString = " AND companyid <> 0";

            string selectedItem = cmbfind.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                if (selectedItem == "அடகு எண்")
                {
                    query = $"SELECT TOP 500 * FROM LoanReturnFindQry WHERE LoanNo LIKE '{txtFind.Text.Trim()}%' {argString} ORDER BY LoanNo DESC";
                }
                else if (selectedItem == "பெயர்")
                {
                    query = $"SELECT TOP 500 * FROM LoanReturnFindQry WHERE CustomerName LIKE '{txtFind.Text.Trim()}%' {argString} ORDER BY CustomerName";
                }
                else if (selectedItem == "திருப்பிய எண்")
                {
                    query = $"SELECT TOP 500 * FROM LoanReturnFindQry WHERE RetNo LIKE '{txtFind.Text.Trim()}%' {argString} ORDER BY RetNo DESC";
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
                            FlexFind.DataSource = null;
                            // FlexFind.Rows.Clear(); 
                        }
                    }
                }
            }
        }
        private void FlexFind_DoubleClick(object sender, EventArgs e)
        {
            GetDomain();
        }

        private double InterestAmt;
        public void GetDomain()
        {
            try
            {
                if (FlexFind.CurrentRow != null)
                {
                    var currentRow = FlexFind.CurrentRow;

                    lblid.Text = currentRow.Cells[0].Value?.ToString();
                    //cmbCompany.SelectedValue = currentRow.Cells[1].Value;
                    lblRetNo.Text = currentRow.Cells[2].Value?.ToString().Trim();
                    //DTPRDate.Value = Convert.ToDateTime(FlexFind.CurrentRow.Cells[3].Value);
                    txtloanno.Text = currentRow.Cells[4].Value?.ToString().Trim();
                    lblloandate.Text = Convert.ToDateTime(FlexFind.CurrentRow.Cells[5].Value).ToShortDateString();
                    lblIntPaidUpto.Text = Convert.ToDateTime(FlexFind.CurrentRow.Cells[6].Value).ToShortDateString();
                    lblSalutation.Text = currentRow.Cells[7].Value?.ToString().Trim();
                    lblName.Text = currentRow.Cells[9].Value?.ToString().Trim();

                    lblfhorHus.Text = currentRow.Cells[10].Value?.ToString().Trim();
                    if (lblfhorHus.Text == "S/O.") lblfhorHus.Text = "தந்தை பெயர்";
                    else if (lblfhorHus.Text == "W/O.") lblfhorHus.Text = "கணவர் பெயர்";
                    else if (lblfhorHus.Text == "C/O.") lblfhorHus.Text = "காப்பாளர் பெயர்";

                    lblFHName.Text = currentRow.Cells[11].Value?.ToString().Trim();
                    lblAddress.Text = currentRow.Cells[12].Value?.ToString().Trim();
                    lblTown.Text = currentRow.Cells[13].Value?.ToString().Trim();
                    txtmonth.Text = currentRow.Cells[14].Value?.ToString().Trim();
                    lblAmount.Text = Math.Truncate(Convert.ToDecimal(currentRow.Cells[15].Value)).ToString();
                    txtintper.Text = Math.Truncate(Convert.ToDecimal(currentRow.Cells[16].Value)).ToString();
                    InterestAmt = Math.Truncate(double.Parse(currentRow.Cells[17].Value?.ToString() ?? "0"));
                    txtInterestAmt.Text = currentRow.Cells[17].Value?.ToString().Trim();
                    lblTotalValue.Text = Math.Truncate(Convert.ToDecimal(currentRow.Cells[18].Value)).ToString();
                    txtDiscount.Text = Math.Truncate(Convert.ToDecimal(currentRow.Cells[19].Value)).ToString();
                    txtRetAmt.Text = Math.Truncate(Convert.ToDecimal(currentRow.Cells[20].Value)).ToString();
                    lblBalanceAmt.Text = Math.Truncate(Convert.ToDecimal(currentRow.Cells[21].Value)).ToString();
                    lblItemType.Text = currentRow.Cells[23].Value?.ToString().Trim();
                    lblGram.Text = Math.Truncate(Convert.ToDecimal(currentRow.Cells[24].Value)).ToString();
                    lblTotalQty.Text = currentRow.Cells[25].Value?.ToString().Trim();
                    lblloanid.Text = currentRow.Cells[26].Value?.ToString();
                    lblCustID.Text = currentRow.Cells[27].Value?.ToString();
                    txtproof.Text = currentRow.Cells[28].Value?.ToString().Trim();
                    txtdeliveryno.Text = currentRow.Cells[29].Value?.ToString().Trim();
                }

                GetItemDetails();

                string strsub = $"SELECT LoanId FROM Loan WHERE LoanId='{lblloanid.Text}'";
                string str = $"SELECT IntrestNo, IntColDate, Collectionamt, IntrestID FROM IntrestCollection WHERE LoanId IN ({strsub})";
                string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

                using (SqlConnection dbcon = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(str, dbcon))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    flexIntCol.DataSource = dt;
                }

                FlexIntColAlign();
                pctFind.Visible = false;
                SetReadOnly(this.Controls);
                BtnSave.Enabled = false;
                txtproof.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void SetReadOnly(Control.ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                if (ctrl is TextBox)
                {
                    var textBox = (TextBox)ctrl;
                    textBox.ReadOnly = true;
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    textBox.BackColor = Color.White;

                    if (textBox.Name == "txtloanno")
                    {
                        textBox.ReadOnly = false;
                        textBox.Enabled = false;
                    }
                }
                else if (ctrl is DateTimePicker)
                {
                    ((DateTimePicker)ctrl).Enabled = false;
                }
                else if (ctrl is Panel || ctrl is GroupBox)
                {
                    SetReadOnly(ctrl.Controls);
                }
            }
        }
        #endregion

        private void FlexIntColAlign()
        {
            if (flexIntCol.Rows.Count > 0)
            {
                flexIntCol.ClearSelection();
            }
            flexIntCol.Columns[0].Width = 83; // LoanNo
            flexIntCol.Columns[1].Width = 90; // LoanDate
            flexIntCol.Columns[2].Width = 115; // LoanAmount
            flexIntCol.Columns[3].Visible = false;

            flexIntCol.Columns[0].HeaderText = "வசூல் எண்";
            flexIntCol.Columns[1].HeaderText = "வசூல் தேதி";
            flexIntCol.Columns[2].HeaderText = "வசூல் தொகை";

            flexIntCol.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft; // Align left for LoanNo
            flexIntCol.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft; // Align left for LoanDate
            flexIntCol.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft; // Align right for LoanAmount
            flexIntCol.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; // Align right for the hidden column (this is redundant but keeping it in case needed)

            for (int i = 0; i <= 2; i++)
            {
                flexIntCol.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            foreach (DataGridViewColumn column in flexIntCol.Columns)
            {
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.DefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            }

            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.BackColor = Color.Purple;
            headerStyle.ForeColor = Color.Yellow;
            headerStyle.Font = new Font("Arial", 8, FontStyle.Bold);
            headerStyle.SelectionBackColor = Color.Purple;

            flexIntCol.ColumnHeadersDefaultCellStyle = headerStyle;
        }

        #region FlexLoan Double click functions
        private void FlexItemDetailAlignEdit()
        {
            FlexItemDetails.AutoGenerateColumns = false;
            FlexItemDetails.Columns.Clear();

            DataGridViewTextBoxColumn colSNo = new DataGridViewTextBoxColumn
            {
                HeaderText = "வ.எண்",
                DataPropertyName = "SNo",
                Width = 90,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
            };
            FlexItemDetails.Columns.Add(colSNo);

            DataGridViewTextBoxColumn colItemName = new DataGridViewTextBoxColumn
            {
                HeaderText = "பொருளின் விபரம்",
                DataPropertyName = "ItemName",
                Width = 300,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
            };
            FlexItemDetails.Columns.Add(colItemName);


            DataGridViewTextBoxColumn colTotalQty = new DataGridViewTextBoxColumn
            {
                HeaderText = "உருப்படி",
                DataPropertyName = "TotalQty",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
            };
            FlexItemDetails.Columns.Add(colTotalQty);

            DataGridViewTextBoxColumn colItemCode = new DataGridViewTextBoxColumn
            {
                HeaderText = "ItemCode",
                DataPropertyName = "ItemCode",
                Width = 0,
                Visible = false
            };
            FlexItemDetails.Columns.Add(colItemCode);

            DataGridViewTextBoxColumn colDescription = new DataGridViewTextBoxColumn
            {
                HeaderText = "அடையாளக் குறிகள்",
                DataPropertyName = "Description",
                Width = 273,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
            };
            FlexItemDetails.Columns.Add(colDescription);

            foreach (DataGridViewColumn column in FlexItemDetails.Columns)
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

            FlexItemDetails.ColumnHeadersDefaultCellStyle = headerStyle;
        }
        private void GetItemDetails()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
                using (SqlConnection dbcon = new SqlConnection(connectionString))
                {
                    dbcon.Open();
                    string query = "SELECT SNo, ItemName, TotalQty, ItemCode, Description FROM LoanDetailsFind WHERE Id = @LoanId ORDER BY SNo";

                    using (SqlCommand cmd = new SqlCommand(query, dbcon))
                    {
                        cmd.Parameters.AddWithValue("@LoanId", Convert.ToInt32(lblloanid.Text));

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            FlexItemDetails.DataSource = dt;
                        }
                    }
                }

                FlexItemDetailAlignEdit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void GetLoan()
        {
            int rowIndex = FlexLoan.CurrentCell.RowIndex;
            lblloanid.Text = FlexLoan.Rows[rowIndex].Cells[0].Value.ToString();
            lblCustID.Text = FlexLoan.Rows[rowIndex].Cells[23].Value.ToString();
            txtloanno.Text = FlexLoan.Rows[rowIndex].Cells[2].Value.ToString();
            lblloandate.Text = Convert.ToDateTime(FlexLoan.Rows[rowIndex].Cells[3].Value).ToString("dd/MM/yyyy");
            lblAmount.Text = FlexLoan.Rows[rowIndex].Cells[15].Value.ToString();
            lblSalutation.Text = FlexLoan.Rows[rowIndex].Cells[5].Value.ToString();
            lblName.Text = FlexLoan.Rows[rowIndex].Cells[7].Value.ToString();

            string salutation = FlexLoan.Rows[rowIndex].Cells[8].Value.ToString();
            if (salutation == "S/O." || salutation == "D/O.")
            {
                lblfhorHus.Text = "îï¢¬î ªðòó¢";
            }
            else if (salutation == "W/O.")
            {
                lblfhorHus.Text = "èíõó¢ ªðòó¢";
            }
            else if (salutation == "C/O.")
            {
                lblfhorHus.Text = "è£ð¢ð£÷ó¢ ªðòó¢";
            }
            else
            {
                lblfhorHus.Text = lblfhorHus.Text;
            }

            lblFHName.Text = FlexLoan.Rows[rowIndex].Cells[9].Value.ToString();
            lblTown.Text = FlexLoan.Rows[rowIndex].Cells[10].Value.ToString();
            lblAddress.Text = FlexLoan.Rows[rowIndex].Cells[21].Value.ToString();
            lblIntPaidUpto.Text = Convert.ToDateTime(FlexLoan.Rows[rowIndex].Cells[20].Value).ToString("dd/MM/yyyy");
            txtintper.Text = FlexLoan.Rows[rowIndex].Cells[16].Value.ToString();
            double InterestAmt = Convert.ToDouble(FlexLoan.Rows[rowIndex].Cells[19].Value);
            txtInterestAmt.Text = InterestAmt.ToString();
            lblGram.Text = FlexLoan.Rows[rowIndex].Cells[12].Value.ToString();
            lblTotalQty.Text = FlexLoan.Rows[rowIndex].Cells[13].Value.ToString();
            lblItemType.Text = FlexLoan.Rows[rowIndex].Cells[11].Value.ToString();

            string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM LoanReturn WHERE LoanId=@LoanId", con);
                cmd.Parameters.AddWithValue("@LoanId", lblloanid.Text);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    txtReceiptCG.Enabled = false;
                    txtReceiptCG.Text = "0";
                }
                else
                {
                    txtReceiptCG.Text = FlexLoan.Rows[rowIndex].Cells[24].Value.ToString();
                }
            }

            MonthCalc();
            IntMonthDets();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT IntrestNo, IntColDate, CollectionAmt, IntrestID " +
                               "FROM IntrestCollection WHERE LoanId=@LoanId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@LoanId", lblloanid.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                flexIntCol.DataSource = dt;
                FlexIntColAlign();
            }
        }
        private void GetBalance()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT Balance FROM CustomerMaster WHERE CustomerId=@CustomerId", con);
                cmd.Parameters.AddWithValue("@CustomerId", lblCustID.Text);
                object result = cmd.ExecuteScalar();
                lblBalance.Text = result != null ? result.ToString() : "0.00";
            }
        }

        decimal Principle;
        private void GetIntPending()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT IntPending FROM Loan WHERE Id=@LoanId", con);
                cmd.Parameters.AddWithValue("@LoanId", lblloanid.Text);
                object result = cmd.ExecuteScalar();
                int intPending = result != null ? Convert.ToInt32(result) : 0;

                Principle = Convert.ToDecimal(lblAmount.Text) - intPending;
            }
        }
        private void MonthCalc()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(lblIntPaidUpto.Text))
                {
                    DateTime lastPaidDate;
                    string[] validFormats = { "dd/MMM/yyyy", "dd/MM/yyyy" };

                    if (DateTime.TryParseExact(lblIntPaidUpto.Text, validFormats, null, System.Globalization.DateTimeStyles.None, out lastPaidDate))
                    {
                        int monthsDifference;
                        if (lastPaidDate.Year == DTPRDate.Value.Year)
                        {
                            monthsDifference = DTPRDate.Value.Month - lastPaidDate.Month;
                        }
                        else
                        {
                            int currentMonths = DTPRDate.Value.Year * 12 + DTPRDate.Value.Month;
                            int pastMonths = lastPaidDate.Year * 12 + lastPaidDate.Month;
                            monthsDifference = currentMonths - pastMonths;
                        }

                        txtmonth.Text = monthsDifference.ToString();
                        if (monthsDifference != 0 && DTPRDate.Value > lastPaidDate.AddMonths(monthsDifference))
                        {
                            txtmonth.Text = (monthsDifference + 1).ToString();
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Invalid date format for 'Interest Paid Upto': {lblIntPaidUpto.Text}. Please correct it.",
                                        "Invalid Date Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtmonth.Text = "0";
                    }
                }
                else
                {
                    txtmonth.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred while calculating the month difference: {ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtmonth.Text = "0";
            }
        }
        private void IntMonthDets()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT IntAmt, LoanDate, IntPaidUpto FROM Loan WHERE Status='L' AND LoanNo=@LoanNo";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@LoanNo", txtloanno.Text.Trim());

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    decimal iAmt = reader["IntAmt"] != DBNull.Value ? Convert.ToDecimal(reader["IntAmt"]) : 0;
                    lblIntAmt.Text = $"Rs.{iAmt}";

                    InterestAmt = reader["IntAmt"] != DBNull.Value ? Convert.ToDouble(reader["IntAmt"]) : 0;

                    DateTime loanDate = Convert.ToDateTime(reader["LoanDate"]);
                    DateTime intPaidUpto = reader["IntPaidUpto"] != DBNull.Value ? Convert.ToDateTime(reader["IntPaidUpto"]) : DateTime.MinValue;

                    lblTotMonth.Text = (DTPRDate.Value.Month - loanDate.Month + 12 * (DTPRDate.Value.Year - loanDate.Year)).ToString();

                    int totalMonths = (DTPRDate.Value.Month - intPaidUpto.Month + 12 * (DTPRDate.Value.Year - intPaidUpto.Year));
                    if (DTPRDate.Value.Day >= intPaidUpto.Day)
                    {
                        totalMonths++;
                    }

                    txtmonth.Text = totalMonths.ToString();
                    txtInterestAmt.Text = (totalMonths * InterestAmt).ToString();
                }
                else
                {
                    lblTotMonth.Text = "0";
                    lblIntAmt.Text = "Rs.0.00";
                }
            }
        }
        #endregion

        #region Reset Button 
        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }
        private void ResetForm()
        {
            try
            {
                BtnDelete.Enabled = false;
                BtnSave.Enabled = true;
                btnFind.Enabled = true;
                FlexItemDetails.DataSource = null;
                FlexLoan.DataSource = null;
                FlexLoan.Rows.Clear();
                FlexLoan.Hide();
                flexIntCol.DataSource = null;
                flexIntCol.Rows.Clear();
                lblTotMonth.Text = "0";
                lblIntAmt.Text = "0";

                ClearAll(this);

                DTPRDate.Value = DateTime.Now;
                txtReceiptCG.Enabled = false;
                FlexItemDetailAlign();
                NewRec();
                NewRecNo();
                NewRecId();
                RetNo();
                EnabledReset();
                pctFind.Visible = false;
                txtDiscount.TextChanged -= txtDiscount_TextChanged;
                txtDiscount.Text = "0";
                txtDiscount.TextChanged += txtDiscount_TextChanged;

                DTPRDate.ValueChanged -= DTPRDate_ValueChanged;
                DTPRDate.Value = DateTime.Now;
                DTPRDate.ValueChanged += DTPRDate_ValueChanged;
                DTPRDate.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error resetting form: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        private void EnabledReset()
        {
            txtloanno.ReadOnly = false;
            txtloanno.Enabled = true;
            txtproof.ReadOnly = false;
            txtdeliveryno.ReadOnly = false;
            txtmonth.ReadOnly = false;
            txtInterestAmt.ReadOnly = false;
            txtDiscount.ReadOnly = false;
            txtRetAmt.ReadOnly = false;
            DTPRDate.Enabled = true;
            lblloanid.Text = "0";
            lblCustID.Text = "0";

        }
        #endregion

        #region Value Changed Functions
        private void DTPRDate_ValueChanged(object sender, EventArgs e)
        {
            MonthCalc();
            IntMonthDets();

            decimal monthValue = string.IsNullOrWhiteSpace(txtmonth.Text) ? 0 : Convert.ToDecimal(txtmonth.Text);

            decimal interestAmount = monthValue * (decimal)InterestAmt;
            txtInterestAmt.Text = interestAmount.ToString("0");

            decimal lblAmountValue = string.IsNullOrWhiteSpace(lblAmount.Text) ? 0 : Convert.ToDecimal(lblAmount.Text);
            decimal totalValue = interestAmount + lblAmountValue;
            lblTotalValue.Text = totalValue.ToString("0");

            decimal discountValue = string.IsNullOrWhiteSpace(txtDiscount.Text) ? 0 : Convert.ToDecimal(txtDiscount.Text);
            decimal retAmount = totalValue - discountValue;
            txtRetAmt.Text = retAmount.ToString("0");

            decimal balanceAmount = totalValue - retAmount - discountValue;
            lblBalanceAmt.Text = balanceAmount.ToString("0");
        }
        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                decimal totalValue = string.IsNullOrWhiteSpace(lblTotalValue.Text) ? 0 : Convert.ToDecimal(lblTotalValue.Text);
                decimal discountValue = string.IsNullOrWhiteSpace(txtDiscount.Text) ? 0 : Convert.ToDecimal(txtDiscount.Text);

                decimal retAmount = totalValue - discountValue;
                txtRetAmt.Text = retAmount.ToString("F2"); // Format to two decimal places

                decimal balanceAmount = totalValue - retAmount - discountValue;
                lblBalanceAmt.Text = balanceAmount.ToString("F2"); // Format to two decimal places
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Invalid input format: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtRetAmt_TextChanged(object sender, EventArgs e)
        {
            CalcIntpending();
        }
        #endregion

        #region Delete functions
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
                                if (GetLoanReturn(Convert.ToInt32(lblloanid.Text), DTPRDate.Value))
                                {
                                    UpdatePrevintPending();

                                    DateTime intPaidUpto = Convert.ToDateTime(lblIntPaidUpto.Text).AddMonths(-Convert.ToInt32(txtmonth.Text));

                                    string updateLoanQuery = @"UPDATE Loan 
                                SET IntPaidUpto = @IntPaidUpto, 
                                    Status = 'L', 
                                    BalanceAmt = @BalanceAmt 
                                WHERE LoanNo = @LoanNo";

                                    using (SqlCommand cmd = new SqlCommand(updateLoanQuery, connection, transaction))
                                    {
                                        cmd.Parameters.AddWithValue("@IntPaidUpto", intPaidUpto.ToString("yyyy-MM-dd"));
                                        cmd.Parameters.AddWithValue("@BalanceAmt", Convert.ToDecimal(lblAmount.Text));
                                        cmd.Parameters.AddWithValue("@LoanNo", txtloanno.Text.Trim());
                                        cmd.ExecuteNonQuery();
                                    }

                                    string deleteLoanReturnQuery = "DELETE FROM LoanReturn WHERE id = @Id";
                                    using (SqlCommand cmd = new SqlCommand(deleteLoanReturnQuery, connection, transaction))
                                    {
                                        cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(lblid.Text));
                                        cmd.ExecuteNonQuery();
                                    }

                                    string deleteLedgerTransactionQuery = "DELETE FROM LedgerTransaction WHERE refno = @RefNo";
                                    using (SqlCommand cmd = new SqlCommand(deleteLedgerTransactionQuery, connection, transaction))
                                    {
                                        cmd.Parameters.AddWithValue("@RefNo", "Return" + lblid.Text);
                                        cmd.ExecuteNonQuery();
                                    }

                                    string deleteInterestCollectionQuery = "DELETE FROM IntrestCollection WHERE refno = @RefNo";
                                    using (SqlCommand cmd = new SqlCommand(deleteInterestCollectionQuery, connection, transaction))
                                    {
                                        cmd.Parameters.AddWithValue("@RefNo", "LR" + lblid.Text);
                                        cmd.ExecuteNonQuery();
                                    }

                                    string selectLoanQuery = "SELECT AcctIntAmt FROM Loan WHERE Id = @LoanId";
                                    using (SqlCommand cmd = new SqlCommand(selectLoanQuery, connection, transaction))
                                    {
                                        cmd.Parameters.AddWithValue("@LoanId", Convert.ToInt32(lblloanid.Text));
                                        decimal acctIntAmt = Convert.ToDecimal(cmd.ExecuteScalar());

                                        acctIntAmt -= Convert.ToDecimal(txtRetAmt.Text);

                                        string updateAcctIntAmtQuery = "UPDATE Loan SET AcctIntAmt = @AcctIntAmt WHERE Id = @LoanId";
                                        using (SqlCommand updateCmd = new SqlCommand(updateAcctIntAmtQuery, connection, transaction))
                                        {
                                            updateCmd.Parameters.AddWithValue("@AcctIntAmt", acctIntAmt);
                                            updateCmd.Parameters.AddWithValue("@LoanId", Convert.ToInt32(lblloanid.Text));
                                            updateCmd.ExecuteNonQuery();
                                        }
                                    }

                                    transaction.Commit();
                                    MessageBox.Show("Record is Deleted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    BtnDelete.Enabled = false;
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
                }

                btnReset_Click(sender, e);
            }
        }
        private bool GetLoanReturn(long loanId, DateTime date)
        {
            bool result = true;

            string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM LoanReturn WHERE LoanId = @LoanId AND RetDate > @RetDate";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@LoanId", loanId);
                    cmd.Parameters.AddWithValue("@RetDate", date);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            result = false;
                        }
                    }
                }
            }

            return result;
        }
        private void UpdatePrevintPending()
        {
            decimal prevInt = 0;

            string connectionString = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectPrevIntQuery = "SELECT Previntpending FROM LoanReturn WHERE id = @Id";
                using (SqlCommand cmd = new SqlCommand(selectPrevIntQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(lblid.Text));
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        prevInt = Convert.ToDecimal(result);
                    }
                }

                string updateLoanQuery = "UPDATE Loan SET IntPending = @PrevInt WHERE id = @LoanId";
                using (SqlCommand cmd = new SqlCommand(updateLoanQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@PrevInt", prevInt);
                    cmd.Parameters.AddWithValue("@LoanId", Convert.ToInt32(lblloanid.Text));
                    cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion

        private void txtInterestAmt_TextChanged(object sender, EventArgs e)
        {
            lblTotalValue.Text = string.IsNullOrEmpty(txtInterestAmt.Text) ? (Convert.ToDecimal(lblAmount.Text)).ToString("F2") :
                (Convert.ToDecimal(txtInterestAmt.Text.ToString()) + Convert.ToDecimal(lblAmount.Text)).ToString("F2");
            //lblTotalValue.Text = (Convert.ToDecimal(txtInterestAmt.Text.ToString()) + Convert.ToDecimal(lblAmount.Text)).ToString("F2");
        }
        private void txtRetAmt_KeyUp(object sender, KeyEventArgs e)
        {
            decimal totalValue = Convert.ToDecimal(lblTotalValue.Text);
            decimal retAmt = string.IsNullOrWhiteSpace(txtRetAmt.Text) ? 0 : Convert.ToDecimal(txtRetAmt.Text);
            decimal discount = string.IsNullOrWhiteSpace(txtDiscount.Text) ? 0 : Convert.ToDecimal(txtDiscount.Text);

            lblBalanceAmt.Text = (totalValue - retAmt - discount).ToString("F2");
        }

        private void txtInterestAmt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.' && txtInterestAmt.Text.Contains(".")) || (txtInterestAmt.Text == "" & e.KeyChar == '.'))
            {
                e.Handled = true;
            }
        }

    }
}
