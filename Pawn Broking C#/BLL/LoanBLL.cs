using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pawn_Broking.BLL
{
    internal class LoanBLL
    {
        public int ID { get; set; }
        public int CompanyId { get; set; }
        public string LoanNo { get; set; }
        public DateTime LoanDate { get; set; }
        public decimal Amount { get; set; }
        public string ItemType { get; set; }
        public int CustomerId { get; set; }
        public decimal Gram { get; set; }
        public int TotalQty { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime? RedeemDate { get; set; } 
        public DateTime? IntPaidUpto { get; set; }  
        public decimal IntPerc { get; set; }
        public decimal IntAmt { get; set; }
        public decimal AcctIntPerc { get; set; }
        public decimal AcctIntAmt { get; set; }
        public decimal ReceiptChg { get; set; }
        public decimal BalanceAmt { get; set; }
        public decimal IntPending { get; set; }
        public string Status { get; set; }
        public string GurName { get; set; }
        public string GurOccupation { get; set; }
        public string OldLoanno { get; set; }
        public string LoanMark { get; set; }       
        public decimal GivenAmount { get; set; }
        public byte[] CustomerPhoto { get; set; }
        public byte[] ItemPhoto { get; set; }
    }
}
