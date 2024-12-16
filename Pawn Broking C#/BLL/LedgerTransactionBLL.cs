using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pawn_Broking.BLL
{
    internal class LedgerTransactionBLL
    {
        public double? TransId { get; set; }
        public DateTime? TransDate { get; set; }
        public int? LedgerId { get; set; }
        public string RefNo { get; set; }
        public string Narration { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public int? DetsId { get; set; }
        public int? CompanyId { get; set; }
    }
}
