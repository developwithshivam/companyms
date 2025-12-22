using CompanyMS.Shared.App_CBL;
using CompanyMS.Shared.App_CBO;
using CompanyMS.Shared.Common;
using CompanyMS.Shared.DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CompanyMS
{
    public partial class management : System.Web.UI.Page
    {
        DataTable dtaddemp = new DataTable();
        //MasterCBO cbomaster;
        //MasterCBL masterCBL;
        string constr = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DBconnections dbc = new DBconnections();
                constr = dbc.CMDB;
                if (Session["RoleId"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                if (!IsPostBack)
                {
                    Filldropdown();
                    CreateEmpGrid();
                    BindEmployeeGrid();
                    Bindbranch();
                    BindbranchHis();
                    if (hdnFormMode.Value == "add")
                    {
                        btnupdateall.Enabled = false;
                        btnupdateall.Visible = false;
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            { Response.Write(ex.Message); }
        }

        private void Filldropdown()
        {
            try
            {
                string qry = "select branch_id,branch_name from tbl_branchmaster";
                ModCommon.Fill_combo(qry, ddlbranchname, constr);
                qry = "select branchdep_id,branchdep_name from tbl_branchdepartment";
                ModCommon.Fill_combo(qry, ddlbranchdep, constr);
            }
            catch (Exception ex) { Response.Write(ex.Message); }

        }

        private void BindEmployeeGrid()
        {
            try
            {
                if (!string.IsNullOrEmpty(hdnbranchid.Value))
                {
                    Loademployeebybranch(hdnbranchid.Value);
                }
                else
                {
                    gvEmployee.DataSource = ViewState["dt"];
                    gvEmployee.DataBind();

                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }


        }

        protected void btnaddemp_Click(object sender, EventArgs e)
        {
            try
            {
                string emp_name = txtempname.Text.Trim();
                string empEmail = txtempemail.Text.Trim();
                if (string.IsNullOrEmpty(txtempname.Text.Trim()) || string.IsNullOrEmpty(txtsalary.Text.Trim()) || string.IsNullOrEmpty(txtempemail.Text.Trim()) || string.IsNullOrEmpty(txtempbranch.Text.Trim()))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertbtnaddemp", "alert('Enter all the Employee Details');", true);

                }

                if (ddlempgender.SelectedIndex == 0)
                {
                    lblddlgender.Text = "Please select gender";
                    return;  // stop execution
                }
                else
                {
                    lblddlgender.Text = "";
                }
                if (IsEmailPresent(empEmail))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "exists", "alert('Employee already present with this email!');", true);
                    return;
                }

                if (hdnFormMode.Value == "add")
                {
                    DataTable dt = ViewState["dt"] as DataTable;
                    DataRow dr = dt.NewRow();

                    dr["emp_id"] = Guid.NewGuid();
                    dr["emp_name"] = txtempname.Text.Trim();
                    dr["emp_salary"] = txtsalary.Text.Trim();
                    dr["emp_emailid"] = txtempemail.Text.Trim();
                    dr["emp_gender"] = ddlempgender.SelectedValue;
                    dr["emp_branch"] = txtempbranch.Text.Trim();
                    dr["branch_id"] = DBNull.Value;
                    dr["branch_kid"] = DBNull.Value;
                    dr["mode"] = 0;

                    dt.Rows.Add(dr);
                    ViewState["dt"] = dt;
                    BindEmployeeGrid();
                    txtempname.Text = " ";
                    txtsalary.Text = " ";
                    txtempemail.Text = " ";
                    //txtempbranch.Text = " ";
                    ddlempgender.SelectedIndex = 0;
                    ddlempgender.Text = "";
                }

                if (hdnFormMode.Value == "update")
                {
                    DataTable dtup = ViewState["EmployeeDT"] as DataTable;
                    DataRow drup = dtup.NewRow();
                    drup["emp_id"] = Guid.NewGuid();
                    drup["emp_name"] = txtempname.Text.Trim();
                    drup["emp_salary"] = txtsalary.Text.Trim();
                    drup["emp_emailid"] = txtempemail.Text.Trim();
                    drup["emp_gender"] = ddlempgender.SelectedValue;
                    drup["emp_branch"] = txtempbranch.Text.Trim();
                    drup["branch_kid"] = hdnbranchid.Value;
                    drup["branch_id"] = ddlbranchname.SelectedValue;
                    if (!dtup.Columns.Contains("mode"))
                    {
                        dtup.Columns.Add("mode", typeof(int));
                    }
                    drup["mode"] = 0;

                    dtup.Rows.Add(drup);
                    ViewState["EmployeeDT"] = dtup;
                    gvEmployee.DataSource = dtup;
                    gvEmployee.DataBind();

                    txtempname.Text = " ";
                    txtsalary.Text = " ";
                    txtempemail.Text = " ";
                    txtempbranch.Text = " ";
                    ddlempgender.SelectedIndex = 0;
                    ddlempgender.Text = "";


                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }

        }

        protected void btnsaveall_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdnFormMode.Value == "add")
                {
                    if (string.IsNullOrEmpty(txtempname.Text) || string.IsNullOrEmpty(txtsalary.Text) || string.IsNullOrEmpty(txtempemail.Text) && string.IsNullOrEmpty(txtempbranch.Text))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertbtnsave", "alert('Enter all the Records');", true);

                    }
                    else
                    {
                        DataTable dt = ViewState["dt"] as DataTable;
                        MasterCBO cbomaster = new MasterCBO();
                        cbomaster.branch_id = ddlbranchname.SelectedValue;
                        cbomaster.branchdep_id = ddlbranchdep.SelectedValue;
                        cbomaster.branch_name = ddlbranchname.SelectedItem.Text;
                        cbomaster.branch_department = ddlbranchdep.SelectedItem.Text;
                        cbomaster.branch_head = txtbranchhead.Text;
                        cbomaster.branch_kid = Guid.NewGuid();
                        cbomaster.mode = 0;

                        //List<Employeeinfo> employee = (List<Employeeinfo>)ViewState["EMPLOYEE"];

                        foreach (DataRow dr in dt.Rows)
                        {
                            dr["branch_kid"] = cbomaster.branch_kid.ToString();
                            dr["branch_id"] = Guid.Parse(cbomaster.branch_id);
                            //dr["emp_branch"] = cbomaster.branch_name;
                            dr["mode"] = 0;
                        }

                        List<Employeeinfo> empList = new List<Employeeinfo>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            Employeeinfo emp = new Employeeinfo();
                            emp.emp_id = (Guid)dr["emp_id"];
                            emp.emp_name = dr["emp_name"].ToString();
                            emp.emp_salary = dr["emp_salary"].ToString();
                            emp.emp_emailid = dr["emp_emailid"].ToString();
                            emp.emp_gender = dr["emp_gender"].ToString();
                            emp.emp_branch = dr["emp_branch"].ToString();
                            emp.branch_id = dr["branch_id"].ToString();
                            emp.branch_kid = dr["branch_kid"].ToString();
                            emp.mode = Convert.ToInt32(dr["mode"]);

                            empList.Add(emp);
                        }
                        MasterCBL objcbl = new MasterCBL();
                        bool status = objcbl.SaveorUpdate(cbomaster, empList);

                        if (status)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('record successfully saved');", true);
                            Bindbranch();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('failed');", true);
                        }
                        Clearallrecords();
                    }
                }
            }
            catch (Exception ex)
            { Response.Write(ex.Message); }


        }

        private void CreateEmpGrid()
        {
            try
            {
                dtaddemp = new DataTable();
                DataColumn column = new DataColumn();
                dtaddemp.Columns.Add("emp_id", typeof(Guid));
                dtaddemp.Columns.Add("emp_name", typeof(string));
                dtaddemp.Columns.Add("emp_salary", typeof(string));
                dtaddemp.Columns.Add("emp_emailid", typeof(string));
                dtaddemp.Columns.Add("emp_gender", typeof(string));
                dtaddemp.Columns.Add("emp_branch", typeof(string));
                dtaddemp.Columns.Add("branch_id", typeof(Guid));
                dtaddemp.Columns.Add("branch_kid", typeof(Guid));
                dtaddemp.Columns.Add("mode", typeof(int));

                ViewState["dt"] = dtaddemp;
            }
            catch (Exception ex)
            { Response.Write(ex.Message); }

        }

        private void Clearallrecords()
        {
            ddlbranchname.SelectedIndex = 0;
            ddlbranchdep.SelectedIndex = 0;
            txtbranchhead.Text = "";

            txtempname.Text = "";
            txtsalary.Text = "";
            txtempemail.Text = "";
            txtempbranch.Text = "";
            ddlempgender.SelectedIndex = 0;

            lblddlgender.Text = "";
            txtempbranch.Enabled = true;

            DataTable dt = ViewState["dt"] as DataTable;
            if (dt != null)
            {
                dt.Rows.Clear();
                ViewState["dt"] = dt;

                gvEmployee.DataSource = dt;
                gvEmployee.DataBind();
            }

        }

        protected void gvEmployee_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvEmployee_RowEditing(object sender, GridViewEditEventArgs e)
        {
            if (hdnFormMode.Value == "add")
            {
                //txtempname.Enabled = false;

                gvEmployee.EditIndex = e.NewEditIndex;
                BindEmployeeGrid();
                // ScriptManager.RegisterStartupScript(this, GetType(), "tab", "openTab('tabAdd');", true);

            }
            else
            { 

                gvEmployee.EditIndex = e.NewEditIndex;
                //BindEmployeeGridFromViewState();
                DataTable dt = ViewState["EmployeeDT"] as DataTable;
                gvEmployee.DataSource = dt;
                gvEmployee.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "tab", "openTab('tabAdd','this');", true);

            }


        }

        protected void gvEmployee_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (hdnFormMode.Value == "add")
            {
                DataTable dt = ViewState["dt"] as DataTable;

                int rowindex = e.RowIndex;
                Guid id = (Guid)gvEmployee.DataKeys[rowindex].Value;
                TextBox txtname = (TextBox)gvEmployee.Rows[rowindex].Cells[0].FindControl("txtempname");
                string name = txtname.Text;
                TextBox txtsalary = (TextBox)gvEmployee.Rows[rowindex].Cells[0].FindControl("txtempsalary");
                string salary = txtsalary.Text;
                TextBox txtempemailid = (TextBox)gvEmployee.Rows[rowindex].Cells[0].FindControl("txtempemailid");
                string emailid = txtempemailid.Text;
                //string salary = ((TextBox)gvEmployee.Rows[rowindex].Cells[1].Controls[0]).Text;
                //string emailid = ((TextBox)gvEmployee.Rows[rowindex].Cells[2].Controls[0]).Text;
                DropDownList ddlempender = (DropDownList)gvEmployee.Rows[rowindex].Cells[3].FindControl("ddlempgender");
                TextBox txtbranch = (TextBox)gvEmployee.Rows[rowindex].FindControl("txtempbranch");
                string branch = txtbranch.Text;


                if (string.IsNullOrEmpty(salary) || string.IsNullOrEmpty(emailid))
                {
                    Response.Write("<script>alert('rows can not be empty while updating')</script>");
                    gvEmployee.EditIndex = rowindex;
                    BindEmployeeGrid();
                    return;
                }

                DataRow row = dt.Select("emp_id = '" + id + "'")[0];
                row["emp_name"] = name;
                row["emp_salary"] = salary;
                row["emp_emailid"] = emailid;
                row["emp_gender"] = ddlempender.SelectedValue;
                row["emp_branch"] = branch;
                ViewState["dt"] = dt;
                gvEmployee.EditIndex = -1;
                BindEmployeeGrid();

            }
            else
            {
                DataTable dt = ViewState["EmployeeDT"] as DataTable;
                int rowindex = e.RowIndex;
                Guid id = (Guid)gvEmployee.DataKeys[rowindex].Value;

                TextBox txtname = (TextBox)gvEmployee.Rows[rowindex].FindControl("txtempname");
                TextBox txtsalary = (TextBox)gvEmployee.Rows[rowindex].FindControl("txtempsalary");
                TextBox txtemail = (TextBox)gvEmployee.Rows[rowindex].FindControl("txtempemailid");
                DropDownList ddlempgender = (DropDownList)gvEmployee.Rows[rowindex].FindControl("ddlempgender");
                //TextBox branch = (TextBox)gvEmployee.Rows[rowindex].FindControl("txtempbranch"); //((TextBox)gvEmployee.Rows[rowindex].Cells[4].Controls[0]).Text;

                // Get new values
                string name = txtname.Text.Trim();
                string salary = txtsalary.Text.Trim();
                string emailid = txtemail.Text.Trim();
                string gender = ddlempgender.SelectedValue;
                //string branch_emp = branch.Text.Trim();

                // Validation
                if (string.IsNullOrEmpty(salary) || string.IsNullOrEmpty(emailid)) //|| string.IsNullOrEmpty(branch))
                {
                    Response.Write("<script>alert('Fields cannot be empty');</script>");
                    gvEmployee.EditIndex = rowindex;
                    BindEmployeeGrid();
                    return;
                }

                // Find DataRow
                DataRow row = dt.Select("emp_id = '" + id + "'")[0];
                row["emp_name"] = name;
                row["emp_salary"] = salary;
                row["emp_emailid"] = emailid;
                row["emp_gender"] = gender;
                //row["emp_branch"] = branch;

                //gvEmployee.DataSource = ViewState["EmployeeDT"];
                //gvEmployee.DataBind();
                ViewState["EmployeeDT"] = dt;
                gvEmployee.EditIndex = -1;
                BindEmployeeGridFromViewState();

                ScriptManager.RegisterStartupScript(this, GetType(), "OpenTab", "openTab('tabAdd','this');", true);
            }

        }

        protected void gvEmployee_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            if (hdnFormMode.Value == "add")
            {
                gvEmployee.EditIndex = -1;
                BindEmployeeGrid();
                ScriptManager.RegisterStartupScript(this, GetType(), "OpenTab", "openTab('tabAdd');", true);
            }
            else
            {
                gvEmployee.EditIndex = -1;
                BindEmployeeGridFromViewState();
            }


        }

        protected void gvEmployee_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if(e.Row.RowType)
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlempgender");
                string gender = DataBinder.Eval(e.Row.DataItem, "emp_gender").ToString();
                ddl.SelectedValue = gender;
                //ddl.Enabled = false;

                //TextBox txtname = (TextBox)e.Row.FindControl("txtempname");
                //if (txtname != null)
                //{
                //    txtname.Enabled = false;
                //}
            }
        }

        protected void ddlbranchname_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (hdnFormMode.Value == "add")
                {
                    txtempbranch.Text = ddlbranchname.SelectedItem.ToString();
                    txtempbranch.Enabled = false;
                }
                else
                {
                    txtempbranch.Text = ddlbranchname.SelectedItem.ToString();
                    string newBranch = ddlbranchname.SelectedItem.Text;

                    // Loop the grid and update textboxes
                    foreach (GridViewRow row in gvEmployee.Rows)
                    {
                        TextBox txtBranch = (TextBox)row.FindControl("txtempbranch");

                        if (txtBranch != null)
                        {
                            txtBranch.Text = newBranch;  // update branch
                            txtBranch.ReadOnly = true;   // ensure readonly
                        }
                    }
                    DataTable dt = (DataTable)ViewState["EmployeeDT"];
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["emp_branch"] = newBranch;
                    }
                    ViewState["EmployeeDT"] = dt;


                }
            }
            catch (Exception ex)
            { Response.Write(ex.Message); }


        }

        protected void btneditbranch_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedbranchid = "";
                foreach (GridViewRow row in gvBranch.Rows)
                {
                    CheckBox chk = (CheckBox)row.FindControl("chkselect");
                    if (chk != null && chk.Checked)
                    {
                        selectedbranchid = gvBranch.DataKeys[row.RowIndex].Value.ToString().Trim();
                        break;
                    }

                }
                if (!string.IsNullOrEmpty(selectedbranchid))
                {
                    if (ddlbranchname.Items.Count == 0)  // <<< only bind if not already bound
                    {
                        Filldropdown();
                    }
                    if (ddlbranchname.Items.FindByValue(selectedbranchid) != null)
                    {
                        ddlbranchname.Items.Remove(ddlbranchname.Items.FindByValue(selectedbranchid));
                        //ddlbranchname.SelectedValue = selectedbranchid;
                    }
                    SwitchToUpdatePanel(selectedbranchid);
                    //Loademployeebybranch(selectedbranchid);


                }
                else
                {
                    Response.Write("<script>alert('please select branch to edit')</script>");
                }
            }
            catch (Exception ex)
            { Response.Write(ex.Message); }

        }

        private void Bindbranch()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    string str = "select branch_kid,branch_id,branch_name,branch_department,branch_head from Branch_info where isnull(isDeleted,0)=0";
                    using (SqlCommand cmd = new SqlCommand(str, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        gvBranch.DataSource = dt;
                        gvBranch.DataBind();

                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }

        }

        private void BindbranchHis()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    string str = "select branch_kid,branch_id,branch_name,branch_department,branch_head from Branch_info where isnull(isDeleted,0)=1";
                    using (SqlCommand cmd = new SqlCommand(str, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        gvHistory.DataSource = dt;
                        gvHistory.DataBind();

                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }

        }


        private void Loademployeebybranch(string branch_kid)
        {
            if(hdnFormMode.Value == "view")
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        string str = "select emp_id,emp_name,emp_salary,emp_emailid,emp_gender,emp_branch,branch_id,branch_kid from Employeeinfo where branch_kid = @branch_kid and isnull(isDeleted,0)=1";
                        using (SqlCommand cmd = new SqlCommand(str, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@branch_kid", branch_kid);
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            gvEmployee.DataSource = dt;
                            gvEmployee.DataBind();

                            ViewState["EmployeeDT"] = dt;

                        }

                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }


            }
            else
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        string str = "select emp_id,emp_name,emp_salary,emp_emailid,emp_gender,emp_branch,branch_id,branch_kid from Employeeinfo where branch_kid = @branch_kid and isnull(isDeleted,0)=0";
                        using (SqlCommand cmd = new SqlCommand(str, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@branch_kid", branch_kid);
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            gvEmployee.DataSource = dt;
                            gvEmployee.DataBind();

                            ViewState["EmployeeDT"] = dt;

                        }

                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }


            }

        }

        private void SwitchToUpdatePanel(string branch_kid)
        {
            if (hdnFormMode.Value == "view")
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        string str = "select branch_kid,branch_id,branch_name,branch_department,branch_head,branchdep_id from Branch_info where branch_kid = @branch_kid and isnull(isDeleted,0)=1";
                        using (SqlCommand cmd = new SqlCommand(str, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@branch_kid", branch_kid);
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                //ddlbranchname.SelectedItem.Text = dt.Rows[0]["branch_name"].ToString();
                                //ddlbranchdep.SelectedItem.Text = dt.Rows[0]["branch_department"].ToString();
                                ddlbranchname.SelectedValue = dt.Rows[0]["branch_id"].ToString();
                                ddlbranchdep.SelectedValue = dt.Rows[0]["branchdep_id"].ToString();
                                txtbranchhead.Text = dt.Rows[0]["branch_head"].ToString();

                            }

                        }
                    }

                    Loademployeebybranch(branch_kid);



                    btnaddemp.Visible = false;
                    emppanel.Visible = false;
                    btnsaveall.Visible = false;
                    btnupdateall.Enabled = false;
                    btnupdateall.Visible = false;
                    ddlbranchname.Enabled = false;
                    ddlbranchdep.Enabled = false;
                    txtbranchhead.Enabled = false;

                    hdnbranchid.Value = branch_kid;

                    string script = "openTab('tabAdd')";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "switchtab", script, true);

                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }

            }
            else 
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        string str = "select branch_kid,branch_id,branch_name,branch_department,branch_head,branchdep_id from Branch_info where branch_kid = @branch_kid";
                        using (SqlCommand cmd = new SqlCommand(str, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@branch_kid", branch_kid);
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                //ddlbranchname.SelectedItem.Text = dt.Rows[0]["branch_name"].ToString();
                                //ddlbranchdep.SelectedItem.Text = dt.Rows[0]["branch_department"].ToString();
                                ddlbranchname.SelectedValue = dt.Rows[0]["branch_id"].ToString();
                                ddlbranchdep.SelectedValue = dt.Rows[0]["branchdep_id"].ToString();
                                txtbranchhead.Text = dt.Rows[0]["branch_head"].ToString();

                            }

                        }
                    }

                    Loademployeebybranch(branch_kid);



                    btnaddemp.Visible = true;
                    //pnlemployeefrm.Visible = true;
                    btnsaveall.Visible = false;
                    btnupdateall.Enabled = true;
                    btnupdateall.Visible = true;
                    ddlbranchname.Enabled = false;
                    ddlbranchdep.Enabled = false;

                    hdnbranchid.Value = branch_kid;
                    hdnFormMode.Value = "update";

                    string script = "openTab('tabAdd')";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "switchtab", script, true);

                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }


            }
        }
        private void BindEmployeeGridFromViewState()
        {
            try
            {
                if (ViewState["EmployeeDT"] != null)
                {
                    gvEmployee.DataSource = (DataTable)ViewState["EmployeeDT"];
                    gvEmployee.DataBind();
                }
            }
            catch (Exception ex) { Response.Write(ex.Message); }

        }

        protected void btnupdateall_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = ViewState["EmployeeDT"] as DataTable;
                if (!dt.Columns.Contains("branch_id"))
                    dt.Columns.Add("branch_id", typeof(Guid));

                if (!dt.Columns.Contains("branch_kid"))
                    dt.Columns.Add("branch_kid", typeof(Guid));

                if (!dt.Columns.Contains("mode"))
                    dt.Columns.Add("mode", typeof(int));

                if (!dt.Columns.Contains("emp_branch"))
                    dt.Columns.Add("emp_branch", typeof(string));
                MasterCBO upcbo = new MasterCBO();
                upcbo.branch_id = ddlbranchname.SelectedValue;
                upcbo.branch_name = ddlbranchname.SelectedItem.Text;
                upcbo.branch_head = txtbranchhead.Text;
                upcbo.branch_department = ddlbranchdep.SelectedItem.Text;
                upcbo.branch_kid = Guid.Parse(hdnbranchid.Value);
                upcbo.branchdep_id = ddlbranchdep.SelectedValue;
                upcbo.mode = 1;

                foreach (DataRow dr in dt.Rows)
                {
                    //dr["branch_kid"] = cbomaster.branch_kid.ToString();
                    //dr["branch_id"] = Guid.Parse(cbomaster.branch_id);
                    //dr["emp_branch"] = cbomaster.branch_name;

                    dr["branch_kid"] = upcbo.branch_kid.ToString();
                    dr["branch_id"] = Guid.Parse(upcbo.branch_id);
                    dr["emp_branch"] = upcbo.branch_name;

                    if (dr["mode"] == DBNull.Value || Convert.ToInt32(dr["mode"]) == 1)
                    {
                        dr["mode"] = 1;
                    }

                }
                List<Employeeinfo> empList = new List<Employeeinfo>();
                foreach (DataRow dr in dt.Rows)
                {
                    Employeeinfo emp = new Employeeinfo();
                    emp.emp_id = (Guid)dr["emp_id"];
                    emp.emp_name = dr["emp_name"].ToString();
                    emp.emp_salary = dr["emp_salary"].ToString();
                    emp.emp_emailid = dr["emp_emailid"].ToString();
                    emp.emp_gender = dr["emp_gender"].ToString();
                    emp.emp_branch = dr["emp_branch"].ToString();
                    emp.branch_id = dr["branch_id"].ToString();
                    emp.branch_kid = dr["branch_kid"].ToString();
                    emp.mode = Convert.ToInt32(dr["mode"]);

                    empList.Add(emp);
                }
                MasterCBL objcbl = new MasterCBL();
                bool status = objcbl.SaveorUpdate(upcbo, empList);
                if (status)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('record updated saved');", true);
                    Bindbranch();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('failed');", true);
                }
                if (dt != null)
                {
                    dt.Rows.Clear();
                    ViewState["EmployeeDT"] = dt;
                    gvEmployee.DataSource = dt;
                    gvEmployee.DataBind();
                }
                ddlbranchname.SelectedIndex = 0;
                ddlbranchdep.SelectedIndex = 0;
                txtbranchhead.Text = "";

                string script = "openTab('tabView','this')";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "changetab", script, true);


            }
            catch (Exception ex)
            { Response.Write(ex.Message); }


        }

        private bool IsEmailPresent(string email)
        {
            string emailToCheck = email.Trim().ToLower();
            if (hdnFormMode.Value=="add")
            {
                if (string.IsNullOrEmpty(email)) return false;
               


                DataTable dtadd = ViewState["dt"] as DataTable;
                if (dtadd != null)
                {
                    foreach (DataRow row in dtadd.Rows)
                    {
                        string exitingEmail = row["emp_emailid"].ToString().Trim().ToLower();
                        if (exitingEmail == emailToCheck) return true;
                    }
                   
                }

            }
            else
            {
                DataTable dt2 = ViewState["EmployeeDT"] as DataTable;
                if (dt2 != null)
                {
                    foreach (DataRow row in dt2.Rows)
                    {
                        string existingEmail = row["emp_emailid"].ToString().Trim().ToLower();
                        if (existingEmail == emailToCheck)
                            return true;
                    }
                }
            }
            // 3) Check GridView rows (if the email is displayed in GridView)
            

            return false;


        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
           Response.Redirect(Request.RawUrl, true);
        }

        protected void btndelete_Click(object sender, EventArgs e)
        {
            bool issucess = false;
            bool issucessemp = false;
            //string selectedbranchid = "";
            foreach (GridViewRow row in gvBranch.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkselect");
                if (chk != null && chk.Checked)
                {
                    Guid selectedbranchid = (Guid)gvBranch.DataKeys[row.RowIndex].Value;
                     issucess = DeleteBranch(selectedbranchid);
                    //issucessemp = DeleteEmployees(selectedbranchid);
                }
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "delsucessfailed", "alert('Please Select Branch to Delete')", true);
                //    string script = "openTab('tabView','this')";
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "delete", script, true);
                //    return;
                //}

            }
           
            Bindbranch();
            if (issucess == true || issucessemp == true)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "delsucess", "alert('Selected Branch  Deleted')", true);
                string script = "openTab('tabView','this')";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "delete", script, true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "delsucessfailed", "alert('Selected Branch  Deleted failed')", true);

            }
         


        }
        private bool DeleteEmployees(Guid branchkId)
        {
            int isresult = 0;
            bool issucess = false;
            try 
            {
                
                using (SqlConnection con = new SqlConnection(constr))
                {
                    SqlCommand cmd = new SqlCommand("SP_saveupdateemployee", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@mode", 2);
                    cmd.Parameters.AddWithValue("@branch_kid", branchkId);

                    con.Open();
                    isresult = cmd.ExecuteNonQuery();
                    issucess = isresult > -1 ? true : false;
                    //if (issucess)
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "delsucess", "alert('Selected Branch emp Deleted')", true);
                    //}
                    //else
                    //{
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "delfailed", "alert('Selected Branch emp Deleted failed')", true);

                    //}
                }
                
            }
            
            catch (Exception ex) 
            {
                string msg = ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "exception", msg, true);
            }
            return issucess;
        }
        private bool DeleteBranch(Guid branchkId)
        {
            int isresult = 0;
            bool issucess = false;
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand("SP_saveupdatebranch", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@mode", 2);
                cmd.Parameters.AddWithValue("@branch_kid", branchkId);

                con.Open();
                isresult =  cmd.ExecuteNonQuery();
                issucess=isresult > -1 ? true :false;
            }
            DeleteEmployees(branchkId);
            return issucess;
        }

        protected void btnview_Click(object sender, EventArgs e)
        {
            hdnFormMode.Value = "view";
            string selectedbranchid = "";
            foreach (GridViewRow row in gvHistory.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkselect");
                if (chk != null && chk.Checked)
                {
                    selectedbranchid = gvHistory.DataKeys[row.RowIndex].Value.ToString().Trim();
                    gvEmployee.Columns[gvEmployee.Columns.Count-1].Visible = false;
                    break;
                }
 

            }
            SwitchToUpdatePanel(selectedbranchid);

        }

        protected void gvEmployee_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Guid id = (Guid)((gvEmployee.DataKeys[e.RowIndex].Value));

            GvDeleteEmployees(id);
        }
        private void GvDeleteEmployees(Guid id) 
        {
            if(hdnFormMode.Value == "update")
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    string str = "update Employeeinfo set isDeleted=1 where emp_id = @emp_id and isnull(isDeleted,0)=0";

                    using (SqlCommand cmd = new SqlCommand(str, conn))
                    {
                        cmd.Parameters.AddWithValue("@emp_id", id);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                BindEmployeeGrid();
            }
            else
            {
                DataTable dt = ViewState["dt"] as DataTable;
                DataRow[] rows = dt.Select("emp_id = '" + id + "'");
                if (rows.Length > 0)
                {
                    rows[0].Delete();
                    // Accept the changes after deletion
                    dt.AcceptChanges();
                }
                ViewState["dt"] = dt;
                BindEmployeeGrid();

            }
        }
    }
}