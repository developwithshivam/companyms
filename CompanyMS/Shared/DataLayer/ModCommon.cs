using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CompanyMS.Shared.DataLayer
{
    public class ModCommon
    {

        public static bool executesql(List<sqlparams> arrParams, List<string> strArr, string constr)
        {
            int isResult = 0;
            bool IsSucess = false;
            SqlConnection _sqlcon = new SqlConnection(constr);
            _sqlcon.Open();
            SqlTransaction _tx = _sqlcon.BeginTransaction();
            SqlCommand _cmd = new SqlCommand();
            try
            {
                if (arrParams.Count > 0)
                {
                    _cmd.Connection = _sqlcon;
                    _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    _cmd.Transaction = _tx;
                }
                for (int i = 0; i <= arrParams.Count - 1; i++)
                {
                    _cmd.CommandText = strArr[i].ToString();
                    if (arrParams[i] != null)
                    {
                        _cmd.Parameters.Clear();
                        foreach (SqlParameter Params in arrParams[i].sqlparam)
                        {
                            _cmd.Parameters.Add(Params);
                        }

                    }
                    isResult = _cmd.ExecuteNonQuery();
                    IsSucess = isResult > -1 ? true : false;
                    if (!IsSucess)
                    {
                        throw new Exception("Transaction failed" + strArr[i].ToString());
                    }
                    
                }
                _tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _tx.Rollback();
                IsSucess = false;
            }
            finally
            {
                _sqlcon.Close();
            }
            return IsSucess;
        }
        public static bool Isempexists(string emp_name,string constr)
        {
            bool isexits = false;
            using(SqlConnection con = new SqlConnection(constr))
            {
                string str = "select count(emp_name) from Employeeinfo where emp_name = @emp_name";
                using(SqlCommand cmd = new SqlCommand(str,con))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@emp_name", emp_name);
                    con.Open();
                    int result = (int)cmd.ExecuteScalar();
                    if(result > 0)
                    {
                       isexits = true;
                    }
                }
                return isexits;
            }
            
        }
        public static void Fill_combo(string str, DropDownList ddl, string constr)
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(constr))
                using (SqlCommand sqlcmd = new SqlCommand(str, sqlcon))
                using (SqlDataAdapter sqladapter = new SqlDataAdapter(sqlcmd))
                {
                    sqlcon.Open();

                    DataTable dt = new DataTable();
                    sqladapter.Fill(dt);

                    ddl.Items.Clear();
                    ddl.Items.Insert(0, new ListItem("--Select--","" /*Guid.Empty.ToString()*/));   // Use EMPTY VALUE, not "0"

                    foreach (DataRow row in dt.Rows)
                    {
                        ddl.Items.Add(new ListItem(
                            row[1].ToString(), 
                            row[0].ToString()    
                        ));
                    }
                    ddl.ClearSelection();
                    ddl.SelectedIndex = 0;   // Select default item
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                // handle error
            }
        }
        public static DataRow Validteuser(string username, string password, string constr)
        {
          
            try
            {


                using (SqlConnection conn = new SqlConnection(constr))
                {
                    string hashpassword = ModCommon.Hashpassword(password);
                    using (SqlCommand cmd = new SqlCommand("sp_userlogin", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@userRes_password", hashpassword);
                        //cmd.Parameters.AddWithValue("@userRes_password", hashpassword);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                            return dt.Rows[0];
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error occured while validating user", ex.Message);
               
            }
            return null;
        }
        public static string Hashpassword(string password)
        {
            string hashstr = string.Empty;
            try
            {


                using (SHA256 sHA = SHA256.Create())
                {
                    byte[] bytes = sHA.ComputeHash(Encoding.UTF8.GetBytes(password));
                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in bytes)
                    { sb.Append(b.ToString("X2")); }
                    hashstr = sb.ToString(0, 50);
                    //return Convert.ToBase64String(bytes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("some error occured", ex.ToString());

            }
            return hashstr;


        }
        public static bool RegisterUser(string username, string password, string email, string contact, string address, string constr)
        {
            bool isRegistered = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_userRegistration", conn))
                    {
                        string hashpassword = ModCommon.Hashpassword(password);

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@userRes_password", hashpassword);
                        cmd.Parameters.AddWithValue("@Emailid", email);
                        cmd.Parameters.AddWithValue("@contact", contact);
                        cmd.Parameters.AddWithValue("@user_address", address);
                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        isRegistered = result > 0;

                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine("Error during registration: " + ex.Message);
                isRegistered = false;
            }

            return isRegistered;

        }
        public static bool usernamecheck(string username, string constr)
        {
            bool exists = false;
            try
            {



                using (SqlConnection conn = new SqlConnection(constr))
                {
                    string str = "select count(*) from tbl_userRegistration where username = @username";
                    using (SqlCommand cmd = new SqlCommand(str, conn))
                    {
                        conn.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@username", username);
                        int count = (int)cmd.ExecuteScalar();
                        exists = count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return exists;
        }
        public static bool passwordcheck(string password, string confirmpass)
        {
            bool checkcomfirmpass = false;
            try
            {
                if (password == confirmpass)
                {
                    return checkcomfirmpass = true;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("some error occured while confirming password", ex.Message);
                return checkcomfirmpass = false;

            }
            return checkcomfirmpass;

        }


    }
    public class sqlparams
        {
          private SqlParameter[] _sqlparams;

          public sqlparams(SqlParameter[] oparam )
        {
            _sqlparams = oparam;
        }

            public SqlParameter[] sqlparam
            {
                get { return _sqlparams; }
                set { _sqlparams = value; }
            }
        }
    }