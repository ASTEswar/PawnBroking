using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pawn_Broking.DAL
{
    internal class LoanDAL
    {
        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
        //public bool LoanExists(string loanNo)
        //{
        //    bool exists = false;
        //    string query = "SELECT * FROM Loan WHERE loanno=@LoanNo";

        //    using (SqlConnection con = new SqlConnection(myconnstrng))
        //    {
        //        SqlCommand cmd = new SqlCommand(query, con);
        //        cmd.Parameters.AddWithValue("@LoanNo", loanNo);

        //        con.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        exists = reader.HasRows;
        //    }

        //    return exists;
        //}
        public bool LoanExists(string loanNo)
        {
            bool exists = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(myconnstrng))
                {
                    
                    string query = "SELECT COUNT(*) FROM LoanRegisterNew WHERE loanno = @LoanNo";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@LoanNo", loanNo);
                        conn.Open();
                        
                       int count = (int)cmd.ExecuteScalar();
                        exists = (count > 0); 
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error checking loan number: " + ex.Message);
            }

            return exists;
        }
        public DataTable Select()
        {
            SqlConnection conn = new SqlConnection(myconnstrng);
            DataTable dt = new DataTable();
            try
            {
                string sql = "SELECT * FROM LoanDetails";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                conn.Open();
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }
        public DataTable DistinctIdentification()
        {
            using (SqlConnection conn = new SqlConnection(myconnstrng))
            {
                string query = "SELECT DISTINCT Description FROM LoanDetails WHERE Description IS NOT NULL AND LTRIM(RTRIM(Description)) <> '' ORDER BY Description";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
        public DataTable SearchDescription(string keywords)
        {
            SqlConnection conn = new SqlConnection(myconnstrng);
            DataTable dt = new DataTable();
            try
            {
                string sql = "SELECT distinct Description FROM LoanDetails WHERE Description LIKE '%" + keywords + "%'";

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                conn.Open();
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }
    }
}
