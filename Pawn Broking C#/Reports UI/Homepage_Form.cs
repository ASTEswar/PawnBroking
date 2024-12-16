using Pawn_Broking.Master_UI;
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
    public partial class Homepage_Form : Form
    {
        public Homepage_Form()
        {
            InitializeComponent();
        }

        private void itemDeatailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Item_Details itemdetails = new Item_Details();
            itemdetails.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void loanToolStripMenuItem_Click(object sender, EventArgs e)
        {
           Loan_Form loan = new Loan_Form();
            loan.Show();
        }

        private void customerSearchEngineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Custom_Search_Engine customSearchEng = new Custom_Search_Engine();
            customSearchEng.Show();
        }

        private void customerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Custom_Details_Change customChange = new Custom_Details_Change();
            customChange.Show();
        }

        private void ledgerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ledger_Form ledgerForm = new Ledger_Form();
            ledgerForm.Show();
        }

        private void bankTransactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bank_Transaction_Form banktransactionForm1 = new Bank_Transaction_Form();
            banktransactionForm1.Show();    
        }

        private void receiptsPaymentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Receipt_Entry_Form recieptentryForm1 = new Receipt_Entry_Form();
            recieptentryForm1.Show();
        }

        private void paymentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PaymententryForm1 paymententryForm1 = new PaymententryForm1();
            paymententryForm1.Show();
        }

        private void daybookEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DayBook_Entry_Form daybookentryForm1 = new DayBook_Entry_Form();
            daybookentryForm1.Show();   
        }

        private void receiptReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reciept_Report_Form recieptreportForm1= new Reciept_Report_Form();
            recieptreportForm1.Show();
        }

        private void paymentReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Payment_Report_Form paymentreportForm1 = new Payment_Report_Form();
            paymentreportForm1.Show();
        }

        private void ledgerStatementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ledger_Statment_Form ledgerstatmentForm1 = new Ledger_Statment_Form();
            ledgerstatmentForm1.Show(); 
        }

        private void dayBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DayBook_Form daybookForm1 = new DayBook_Form();
            daybookForm1.Show();
        }

        private void customerBalanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Customer_Balance_Form customer_Balance_Form = new Customer_Balance_Form();
            customer_Balance_Form.Show();
        }

        private void partyBalanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Party_Balance_Form partybalanceForm1 = new Party_Balance_Form();
            partybalanceForm1.Show();
        }

        private void editPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Edit_Password_Form editpass = new Edit_Password_Form();
            editpass.Show();

        }

        private void addressToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Address Add = new Address();
            Add.Show();
        }

        private void loanRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Loan_Register_Form LoanRegister = new Loan_Register_Form();
            LoanRegister.Show();
        }

        private void loanActionReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Loan_Action_Report actionreport = new Loan_Action_Report(); 
            actionreport.Show();
        }

        private void itemInHandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Item_in_HandForm iteminhand = new Item_in_HandForm();
            iteminhand.Show();
        }

        private void loanItemInHandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Loan_Item_in_Hand loaniteminhand = new Loan_Item_in_Hand();
            loaniteminhand.Show();
        }

        private void dailyLoanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Daily_Loan dailyloan = new Daily_Loan();
            dailyloan.Show();
        }

        private void dailyLoanDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Daily_Loan_Details daily_Loan_Details = new Daily_Loan_Details();
            daily_Loan_Details.Show();
        }

        private void loanDetailsItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Loan_Details_Itemin_Hand LoanDetItemHand = new Loan_Details_Itemin_Hand();
            LoanDetItemHand.Show(); 
        }

        private void accountInterestAmountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Account_Interest_Amount AccountInterestAmount = new Account_Interest_Amount();
            AccountInterestAmount.Show();
        }

        private void loanHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Loan_History LoanHistory = new Loan_History();
            LoanHistory.Show();
        }

        private void Homepage_Form_Load(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Pending_Details pending_Details = new Pending_Details();
            pending_Details.Show();
        }
    }
}
