using CompanyMS.Shared.Common;
using CompanyMS.Shared.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CompanyMS
{
    public partial class UserRegistration : System.Web.UI.Page
    {
        string constr = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            DBconnection();
            var master = this.Master as CompanyMS.Site1;
            if (master != null)
            {
                master.ShowSidebar = false;
                master.Logouttab = false;
            }
        }

        protected void btn_register_Click(object sender, EventArgs e)
        {
            string username = txt_username.Text.Trim();
            string password = txt_password.Text.Trim();
            string confirmpass = txt_confirmpass.Text.Trim();
            string email = txt_email.Text.Trim();
            string contact = txt_contact.Text.Trim();
            string address = txt_address.Text.Trim();
            bool isconfirmpass = ModCommon.passwordcheck(password, confirmpass);
            bool isexists = ModCommon.usernamecheck(username, constr);
            bool success = ModCommon.RegisterUser(username, password, email, contact, address, constr);

            if (!isconfirmpass)
            {
                lbl_confirmpass.Text = "password dose not match, check password";
            }
            if (isexists)
            {
                lblusermessage.Text = "username already exits use different one";
            }


            if (success)
            {
                lblmessage.Text = "Registration Successful ! you will shortly redirected to login page";
                string script = "setTimeout(function(){window.location=('Login.aspx');},10000)";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "successregiste", script, true);
            }
            else
            {
                lblmessage.Text = "registration failed try again";
            }
        }

        private void DBconnection()
        {
            DBconnections dbc = new DBconnections();
            constr = dbc.CMDB;
        }
    }
}