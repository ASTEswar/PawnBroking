using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pawn_Broking.BLL
{
    internal class CustomerDetailBLL
    {
        public int CustomerID { get; set; }
        public string Salutation { get; set; }
        public string CustomerInitial { get; set; }
        public string CustomerName { get; set; }
        public string FHType { get; set; }
        public string FHName { get; set; }
        public string Address { get; set; }
        public string Town { get; set; }
        public string Taluk { get; set; }
        public string District { get; set; }
        public string occupation { get; set; }
        public string phone { get; set; }
        public decimal Balance { get; set; }
        public string AadhaarNo { get; set; }
    }
}
