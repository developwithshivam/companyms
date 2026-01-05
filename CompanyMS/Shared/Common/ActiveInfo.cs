using CompanyMS.Shared.DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CompanyMS.Shared.Common
{
    public class ActiveInfo
    {
        DBconnections mdb = new DBconnections();

        public string username { get; set; }

        public int userRes_id { get; set; }

        public int RoleId { get; set; }
        public ActiveInfo()
        {
            if (HttpContext.Current.Session == null || HttpContext.Current.Session["LoginInfo"] == null)
            {
                HttpContext.Current.Response.Redirect("~/Login.aspx?error=Session Time Out");
            }
            else
            {
                DataTable dtLogin = (DataTable)HttpContext.Current.Session["LoginInfo"];
                if (dtLogin != null)
                {
                    if (dtLogin.Rows.Count > 0)
                    {
                        username = Convert.ToString(dtLogin.Rows[0]["username"]);

                        userRes_id = Convert.ToInt32(dtLogin.Rows[0]["userRes_id"]);

                        RoleId = Convert.ToInt32(dtLogin.Rows[0]["RoleId"]);
                    }
                    string qry = "select username,userRes_id,RoleId,User_sessionId from tbl_userRegistration where userRes_id='" + userRes_id + "'";
                    string SessionId = string.Empty;
                    SessionId = HttpContext.Current.Session.SessionID.ToString();
                    DataTable dt = ModCommon.ReturnDataTable(qry, mdb.CMDB);
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (Convert.ToInt32(dt.Rows[0]["RoleId"])!=1)
                            {
                                if (Convert.ToString(dt.Rows[0]["User_sessionId"]) != SessionId)
                                {
                                    HttpContext.Current.Response.Redirect("~/Login.aspx?err=Mutiple Session Not Allowed");
                                }
                            }

                        }

                    }
                }
            }
        }
    }
}