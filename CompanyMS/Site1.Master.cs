using CompanyMS.Shared.Common;
using CompanyMS.WebReferenceCms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CompanyMS
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        string constr = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            DBconnection();
            if (!IsPostBack)
            {
            }
            DataTable dt = (DataTable)HttpContext.Current.Session["LoginInfo"];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    ViewState["Roleid"] = Convert.ToInt32(dt.Rows[0]["RoleId"]);
                    ViewState["userRes_id"] = Convert.ToInt32(dt.Rows[0]["userRes_id"]);

                    int Roleid = Convert.ToInt32(dt.Rows[0]["RoleId"]);
                    LoadMenu(Roleid);


                }
            }

        }
        // inside CompanyMS.Site1 class
        public bool ShowSidebar
        {
            get
            {
                return LeftMenu != null && LeftMenu.Visible;
            }
            set
            {
                if (LeftMenu != null)
                {
                    LeftMenu.Visible = value;
                }
            }
        }
        public bool Logouttab
        {
            get
            {
                return logout != null && logout.Visible;
            }
            set
            {
                if (logout != null)
                {
                    logout.Visible = value;
                }
            }
        }

        protected void logout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
            }
        

        private void LoadMenu(int RoleId)
        {

            CmsWebService cms = new CmsWebService();
            DataTable dt = cms.GetMenuData(RoleId);
            StringBuilder sb = new StringBuilder();
            ActiveInfo actinfo = new ActiveInfo();
            //DataRow[] parents = dt.Select("Perentid=0");
            foreach (DataRow row in dt.Rows)
            {
                string url = ResolveUrl("~/" + row["NavigationUrl"]);
                sb.Append("<a href='").
                    Append(url).
                    Append("' class = 'menu-link'>").
                    Append("<span class = 'fa fa-angle-right menu-icon'></span>").
                    Append(row["MenuText"]).Append("</a>");
            }
            LeftMenu.InnerHtml += sb.ToString();
        }

        //private DataTable GetMenuData(int RoleId) 
        //{
        //    DataTable dt = new DataTable();
        //    try 
        //    {
        //        using (SqlConnection con = new SqlConnection(constr))
        //        {
        //            string str = "select m.Menuid,m.MenuText,m.NavigationUrl,m.perentid from tbl_Menu m inner join RoleMenu r on m.Menuid = r.Menuid where r.RoleId = @Roleid";
        //            using (SqlCommand cmd = new SqlCommand(str, con))
        //            {
        //                cmd.CommandType = CommandType.Text;
        //                cmd.Parameters.AddWithValue("@RoleId", RoleId);
        //                SqlDataAdapter da = new SqlDataAdapter(cmd);
        //                da.Fill(dt);

        //            }
                   
        //        }
        //    }
        //    catch(Exception ex)  
        //    {
        //        Response.Write(ex.Message);

        //    }
        //    return dt;
        //}

        private void DBconnection()
        {
            try 
            {
                DBconnections dbc = new DBconnections();
                constr = dbc.CMDB;
            }
            catch(Exception ex)
            {
                Response.Write(ex.Message);
            }
           
        }
    }
}