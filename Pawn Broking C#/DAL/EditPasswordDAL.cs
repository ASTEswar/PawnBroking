using Pawn_Broking.BLL;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Pawn_Broking.DAL
{
    internal class EditPasswordDAL
    {
        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        #region Select Data from Database
        public DataTable Select()
        {
            SqlConnection conn = new SqlConnection(myconnstrng);
            DataTable dt = new DataTable();
            try
            {
                string sql = "SELECT * FROM PasswordEdit";
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
        #endregion
        public bool Insert(EditPasswordBLL edit)
        {
            bool isSucces = false;

            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                string sql = "INSERT INTO PasswordEdit (EPassword) VALUES (@EPassword)";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@EPassword", edit.EPassword);
                
                conn.Open();

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    isSucces = true;
                }
                else
                {
                    isSucces = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return isSucces;
        }
        public bool loginCheck(EditPasswordBLL l)
        {
            bool isSuccess = false;
         
            using (SqlConnection conn = new SqlConnection(myconnstrng))
            {
                try
                {
                    string sql = "SELECT * FROM PasswordEdit WHERE EPassword=@EPassword";
              
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@EPassword", l.EPassword);
                
                    conn.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd); 
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {                    
                        isSuccess = true;
                    }
                    else
                    {                     
                        isSuccess = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            return isSuccess;
        }
    }
}
