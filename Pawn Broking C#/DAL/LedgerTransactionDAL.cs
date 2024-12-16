using Pawn_Broking.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pawn_Broking.DAL
{
    internal class LedgerTransactionDAL
    {
        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
        public bool Insert(LedgerTransactionBLL ledgerTrans)
        {
            bool isSuccess = false;
            SqlConnection conn = new SqlConnection(myconnstrng); 

            try
            {
                string query = "INSERT INTO LedgerTransaction (TransId, TransDate, LedgerId, RefNo, Narration, Debit, Credit, DetsId, Companyid) " +
                               "VALUES (@TransId, @TransDate, @LedgerId, @RefNo, @Narration, @Debit, @Credit, @DetsId, @Companyid)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@TransId", ledgerTrans.TransId.HasValue ? (object)ledgerTrans.TransId.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@TransDate", ledgerTrans.TransDate.HasValue ? (object)ledgerTrans.TransDate.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@LedgerId", ledgerTrans.LedgerId.HasValue ? (object)ledgerTrans.LedgerId.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@RefNo", ledgerTrans.RefNo ?? (object)DBNull.Value); // Null check for RefNo
                cmd.Parameters.AddWithValue("@Narration", ledgerTrans.Narration ?? (object)DBNull.Value); // Null check for Narration
                cmd.Parameters.AddWithValue("@Debit", ledgerTrans.Debit.HasValue ? (object)ledgerTrans.Debit.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@Credit", ledgerTrans.Credit.HasValue ? (object)ledgerTrans.Credit.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@DetsId", ledgerTrans.DetsId.HasValue ? (object)ledgerTrans.DetsId.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@Companyid", ledgerTrans.CompanyId.HasValue ? (object)ledgerTrans.CompanyId.Value : DBNull.Value);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();

                isSuccess = rows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return isSuccess; 
        }

    }
}
