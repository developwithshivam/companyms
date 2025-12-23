using CompanyMS.Shared.App_CBO;
using CompanyMS.Shared.Common;
using CompanyMS.Shared.DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CompanyMS.Shared.App_CBL
{
    public class MasterCBL : MasterCBO
    {
        string constr = string.Empty;

        public MasterCBL() 
        {
            DBconnections dbc = new DBconnections();
            constr = dbc.CMDB;

        }

        public bool SaveorUpdate(MasterCBO masterCBO ,List<Employeeinfo> empList)
        {
            bool result = false;
            List<sqlparams> oListSqlParameter = new List<sqlparams>();
            List<string> oListArrSPName = new List<string>();
            int p = 0;
            try
            {
                SqlParameter[] _para =
                {
                    new SqlParameter("@branch_id",masterCBO.branch_id),
                    new SqlParameter("@branch_name",masterCBO.branch_name),
                    new SqlParameter("@branch_department",masterCBO.branch_department),
                    new SqlParameter("@branch_kid",masterCBO.branch_kid),
                    new SqlParameter("@branch_head", masterCBO.branch_head),
                    new SqlParameter("@branchdep_id",masterCBO.branchdep_id),
                    new SqlParameter("@mode",masterCBO.mode)
                };
                oListSqlParameter.Add(new sqlparams(_para));
                oListArrSPName.Add("SP_saveupdatebranch");

                for (p = 0; p < empList.Count; p++)
                {
                    SqlParameter[] _paraemp =
                        {
                        new SqlParameter("@emp_id",empList[p].emp_id),
                        new SqlParameter("@branch_id",empList[p].branch_id),
                        new SqlParameter("@emp_name", empList[p].emp_name),
                        new SqlParameter("@emp_salary", empList[p].emp_salary),
                        new SqlParameter("@emp_emailid", empList[p].emp_emailid),
                        new SqlParameter("@emp_gender", empList[p].emp_gender),
                        new SqlParameter("@emp_branch", empList[p].emp_branch),
                        new SqlParameter("@branch_kid", empList[p].branch_kid),
                        new SqlParameter("@mode", empList[p].mode)
                                    
                        };
                    oListSqlParameter.Add(new sqlparams(_paraemp));
                    oListArrSPName.Add("SP_saveupdateemployee");
                }
                result = ModCommon.executesql(oListSqlParameter, oListArrSPName, constr);
            }
            catch (Exception ex) 
            {

            }
            return result;
        }
        public DataTable Bindbranch()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    string str = "select branch_kid,branch_id,branch_name,branch_department,branch_head from Branch_info where isnull(isDeleted,0)=0";
                    using (SqlCommand cmd = new SqlCommand(str, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        //gvBranch.DataSource = dt;
                        //gvBranch.DataBind();

                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return dt;
        }
        public DataTable BindbranchHis()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    string str = "select branch_kid,branch_id,branch_name,branch_department,branch_head from Branch_info where isnull(isDeleted,0)=1";
                    using (SqlCommand cmd = new SqlCommand(str, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);

                    }
                }
            }
            catch (Exception ex)
            {
               // Response.Write(ex.Message);
            }
            return dt;
        }

    }
}