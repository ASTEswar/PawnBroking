using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pawn_Broking.BLL;

namespace Pawn_Broking.DAL
{
    internal class CustomerDetailDAL
    {
        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        #region Select Data from Database
        public DataTable Select()
        {
            SqlConnection conn = new SqlConnection(myconnstrng);
            DataTable dt = new DataTable();
            try
            {
                string sql = "SELECT * FROM CustomerMaster";
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
       
        #region Insert Data into Database
        public bool Insert(CustomerDetailBLL customer)
        {
            bool isSuccess = false;
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                string sql = "INSERT INTO CustomerMaster (CustomerID,Salutation, CustomerName, FHType, FHName, Address, Town, Taluk, District, occupation, phone, AadhaarNo) " +
                             "VALUES (@CustomerID,@Salutation ,@CustomerName, @FHType, @FHName, @Address, @Town, @Taluk, @District, @Occupation, @Phone, @AadhaarNo)";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
                cmd.Parameters.AddWithValue("@Salutation", customer.Salutation);
                //cmd.Parameters.AddWithValue("@CustomerInitial", customer.CustomerInitial);
                cmd.Parameters.AddWithValue("@CustomerName", customer.CustomerName);
                cmd.Parameters.AddWithValue("@FHType", customer.FHType);
                cmd.Parameters.AddWithValue("@FHName", customer.FHName);
                cmd.Parameters.AddWithValue("@Address", customer.Address);
                cmd.Parameters.AddWithValue("@Town", customer.Town);
                cmd.Parameters.AddWithValue("@Taluk", customer.Taluk);
                cmd.Parameters.AddWithValue("@District", customer.District);
                cmd.Parameters.AddWithValue("@Occupation", customer.occupation);
                cmd.Parameters.AddWithValue("@Phone", customer.phone);
                //cmd.Parameters.AddWithValue("@Balance", customer.Balance);
                cmd.Parameters.AddWithValue("@AadhaarNo", customer.AadhaarNo);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();

                isSuccess = rows > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return isSuccess;
        }
        #endregion

        #region Update Data in Database
        public bool Update(CustomerDetailBLL customer)
        {
            bool isSuccess = false;
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                string sql = "UPDATE CustomerMaster SET Salutation=@Salutation, CustomerName=@CustomerName, FHType=@FHType, " +
                             "FHName=@FHName, Address=@Address, Town=@Town, Taluk=@Taluk, District=@District, occupation=@Occupation, phone=@Phone, " +
                             "AadhaarNo=@AadhaarNo WHERE CustomerID=@CustomerID";
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Salutation", customer.Salutation);
                //cmd.Parameters.AddWithValue("@CustomerInitial", customer.CustomerInitial);
                cmd.Parameters.AddWithValue("@CustomerName", customer.CustomerName);
                cmd.Parameters.AddWithValue("@FHType", customer.FHType);
                cmd.Parameters.AddWithValue("@FHName", customer.FHName);
                cmd.Parameters.AddWithValue("@Address", customer.Address);
                cmd.Parameters.AddWithValue("@Town", customer.Town);
                cmd.Parameters.AddWithValue("@Taluk", customer.Taluk);
                cmd.Parameters.AddWithValue("@District", customer.District);
                cmd.Parameters.AddWithValue("@Occupation", customer.occupation);
                cmd.Parameters.AddWithValue("@Phone", customer.phone);
                //cmd.Parameters.AddWithValue("@Balance", customer.Balance);
                cmd.Parameters.AddWithValue("@AadhaarNo", customer.AadhaarNo);
                cmd.Parameters.AddWithValue("@CustomerID", customer.CustomerID);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();

                isSuccess = rows > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return isSuccess;
        }
        #endregion

        #region Delete Data from Database
        public bool Delete(CustomerDetailBLL customer)
        {
            bool isSuccess = false;
            SqlConnection conn = new SqlConnection(myconnstrng);

            try
            {
                string sql = "DELETE FROM CustomerMaster WHERE CustomerID=@CustomerID";
                SqlCommand cmd = new SqlCommand(sql, conn);

                // Parameters to delete data
                cmd.Parameters.AddWithValue("@CustomerID", customer.CustomerID);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();

                isSuccess = rows > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return isSuccess;
        }
        #endregion

        #region Search Method for Customer Details
        public DataTable Search(string field, string keywords)
        {
            SqlConnection conn = new SqlConnection(myconnstrng);
            DataTable dt = new DataTable();

            try
            {
                string sql = $"SELECT * FROM CustomerMaster WHERE {field} LIKE N'%{keywords}%'";

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

        #region Town Names For Combobox
        public DataTable GetDistinctTowns()
        {
            using (SqlConnection conn = new SqlConnection(myconnstrng))
            {
                string query = "SELECT DISTINCT Town FROM CustomerMaster ORDER BY Town";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
        #endregion

        #region Loan Form Customer Details Textboxes Query
        public DataTable SearchPhoneNo(string keywords)
        {
            SqlConnection conn = new SqlConnection(myconnstrng);
            DataTable dt = new DataTable();
            try
            {
                string sql = "SELECT * FROM CustomerMaster WHERE phone LIKE '%" + keywords + "%'";

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
        public DataTable SearchName(string keywords)
        {
            SqlConnection conn = new SqlConnection(myconnstrng);
            DataTable dt = new DataTable();
            try
            {
                string sql = "SELECT * FROM CustomerMaster WHERE CustomerName LIKE '%" + keywords + "%'";

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
        public DataTable SearchFName(string keywords)
        {
            SqlConnection conn = new SqlConnection(myconnstrng);
            DataTable dt = new DataTable();
            try
            {
                string sql = "SELECT * FROM CustomerMaster WHERE FHName LIKE '%" + keywords + "%'";

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
        public DataTable SearchAddress(string keywords)
        {
            SqlConnection conn = new SqlConnection(myconnstrng);
            DataTable dt = new DataTable();
            try
            {
                string sql = "SELECT * FROM CustomerMaster WHERE Address LIKE '%" + keywords + "%'";

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
        public DataTable SearchTown(string keywords)
        {
            SqlConnection conn = new SqlConnection(myconnstrng);
            DataTable dt = new DataTable();
            try
            {
                string sql = "SELECT * FROM CustomerMaster WHERE Town LIKE '%" + keywords + "%'";

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
        public DataTable SearchTaluk(string keywords)
        {
            SqlConnection conn = new SqlConnection(myconnstrng);
            DataTable dt = new DataTable();
            try
            {
                string sql = "SELECT * FROM CustomerMaster WHERE Taluk LIKE '%" + keywords + "%'";

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
        public DataTable DistinctTaluk()
        {
            using (SqlConnection conn = new SqlConnection(myconnstrng))
            {
                string query = "SELECT DISTINCT Taluk FROM CustomerMaster WHERE Taluk IS NOT NULL AND LTRIM(RTRIM(Taluk)) <> '' ORDER BY Taluk";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
        public DataTable SearchDistrict(string keywords)
        {
            SqlConnection conn = new SqlConnection(myconnstrng);
            DataTable dt = new DataTable();
            try
            {
                string sql = "SELECT * FROM CustomerMaster WHERE District LIKE '%" + keywords + "%'";
                //   string sql = "SELECT DISTINCT LTRIM(RTRIM(District)) AS District " +
                //"FROM CustomerMaster " +
                //"WHERE LTRIM(RTRIM(District)) LIKE '%" + keywords + "%' " +
                //"ORDER BY District";

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
        public DataTable DistinctDistrict() 
        {
            using (SqlConnection conn = new SqlConnection(myconnstrng))
            {
                string query = "SELECT DISTINCT District FROM CustomerMaster WHERE District IS NOT NULL AND LTRIM(RTRIM(District)) <> '' ORDER BY District";

                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
        #endregion

        #region Customer Add Loan Form
        public DataTable SelectParameter(string query, SqlParameter[] parameters)
        { 
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(myconnstrng))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }
            return dt;
        }
        //public object ExecuteScalar(string query)
        //{
        //    object result;
        //    using (SqlConnection conn = new SqlConnection(myconnstrng))
        //    {
        //        using (SqlCommand cmd = new SqlCommand(query, conn))
        //        {
        //            conn.Open();
        //            result = cmd.ExecuteScalar();
        //        }
        //    }
        //    return result;
        //}
        public object ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            object result;
            using (SqlConnection conn = new SqlConnection(myconnstrng))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add parameters to the command if provided
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    conn.Open();
                    result = cmd.ExecuteScalar();
                }
            }
            return result;
        }

        #endregion

    }
}
