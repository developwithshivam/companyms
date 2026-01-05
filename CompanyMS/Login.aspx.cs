using CompanyMS.Shared.Common;
using CompanyMS.Shared.DataLayer;
using CompanyMS.WebReferenceCms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CompanyMS
{
    public partial class Login : System.Web.UI.Page
    {
        string constr = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            DBconnection();
            if (!IsPostBack)
            {
                pnlLogin.Visible = true;
                pnlforgetpass.Visible = false;
            }
            var master = this.Master as CompanyMS.Site1;
            if (master != null)
            {
                master.ShowSidebar = false;
                master.Logouttab = false;
            }

        }

        protected void btn_button_Click(object sender, EventArgs e)
        {
            string username = txt_username.Text.Trim();
            string password = txt_password.Text.Trim();

            CmsWebService cms = new CmsWebService();
            DataTable dt = cms.ValidateUser(username, password);
            //DataRow row = dt.NewRow();
            //ModCommon.Validteuser(username, password, constr);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    ViewState["username"] = Convert.ToString(dt.Rows[0]["username"]);
                    ViewState["RoleId"] = Convert.ToInt32(dt.Rows[0]["RoleId"]);
                    ViewState["userRes_id"] = Convert.ToInt32(dt.Rows[0]["userRes_id"]);

                }
                // Login successful
                //  FormsAuthentication.RedirectFromLoginPage(username, false);
                //string script = "alert('welcome!')";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "successloginpage", script, true);

                bool success = Createlogininfo();
                ActiveInfo activeInfo = new ActiveInfo();
                if (success)
                {
                    if (ViewState["RoleId"].ToString() != "1")
                    {
                       

                        Response.Redirect(ResolveUrl("~/Pages/Default.aspx"));

                    }
                    else
                    {
                       
                        Response.Redirect(ResolveUrl("~/management.aspx"));
                    }

                }
                else
                {

                }


            }
            else
            {

                // Login failed
                string script = "alert('Login failed!')";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "flaiedloginpge", script, true);
                //lbl_msg.Text = "Invalid Username or Password";
                //lbl_msg.ForeColor = System.Drawing.Color.Red;
            }


        }
        private void DBconnection()
        {
            DBconnections dbc = new DBconnections();
            constr = dbc.CMDB;
        }

        protected void Linkforgetpassword_Click(object sender, EventArgs e)
        {
            pnlLogin.Visible = false;
            pnlforgetpass.Visible = true;

        }

        protected void btn_resetpass_Click(object sender, EventArgs e)
        {
            string username = txt_resetusername.Text.Trim();
            string resetpassword = txt_restpass.Text.Trim();
            string crmrestpassword = txt_crmpass.Text.Trim();
            bool ischeckresetpass = ModCommon.passwordcheck(resetpassword, crmrestpassword);
            bool isexists = ModCommon.usernamecheck(username, constr);


            if (!ischeckresetpass)
            {
                string script = "alert('confirm password doesn'nt match,enter correct password')";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "resetpassnotmatch", script, true);

            }
            if (isexists)
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_updatepassword", con))
                    {
                        string hashpass = ModCommon.Hashpassword(resetpassword);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@reset_password", hashpass);
                        con.Open();
                        int result = (int)cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            string script = "alert('password reset sucessfully')";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "resetpassword", script, true);
                        }
                        else
                        {
                            string script = "alert('Reset Password Failed')";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "failedresetpass", script, true);
                        }


                    }

                }

            }

        }
        public bool Createlogininfo()
        {
            bool success = false;
            DataTable dtLoginInfo = new DataTable();
            dtLoginInfo.Columns.Add("username", typeof(string));
            dtLoginInfo.Columns.Add("userRes_id", typeof(string));
            dtLoginInfo.Columns.Add("RoleId", typeof(string));

            DataRow dr = dtLoginInfo.NewRow();
            dr["username"] = Convert.ToString(ViewState["username"]);
            dr["userRes_id"] = Convert.ToString(ViewState["userRes_id"]);
            dr["RoleId"] = Convert.ToString(ViewState["RoleId"]);


            dtLoginInfo.Rows.Add(dr);
            dtLoginInfo.AcceptChanges();
            string userResid = Convert.ToString(dtLoginInfo.Rows[0]["userRes_id"]);
            HttpContext.Current.Session["LoginInfo"] = dtLoginInfo;
            string qry = "select IsLoggedIn,IsLoggedOut from tbl_userRegistration where userRes_id = '" + userResid + "'";
            DataTable dtu = ModCommon.ReturnDataTable(qry, constr);
            if (dtu != null)
            {
                if (dtu.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dtu.Rows[0]["IsLoggedOut"]) == false)
                    {
                        string SessionId = string.Empty;
                        SessionId = HttpContext.Current.Session.SessionID;

                        string str = "update tbl_userRegistration set IsLoggedIn=1 , IsLoggedOut=0,User_sessionId = '" + SessionId + "' where userRes_id = '" + userResid + "'";
                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            using (SqlCommand cmd = new SqlCommand(str, con))
                            {
                                cmd.CommandType = CommandType.Text;
                                con.Open();
                                int result = cmd.ExecuteNonQuery();
                                success = result > -1 ? true : false;

                            }

                        }
                    }
                }

            }
            return success;
        }
    }
}