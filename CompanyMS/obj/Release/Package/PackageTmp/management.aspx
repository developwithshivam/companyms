<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="management.aspx.cs" Inherits="CompanyMS.management" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <!-- Bootstrap 5 CSS (included because master page does not have it) -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous">

    <!-- Select2 CSS (kept because original used .select2) -->
    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />

    <!-- Google Font -->
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@300;400;600;700&display=swap" rel="stylesheet">

    <style>
        /* ===========================
           Theme variables (Cool Blue CRM)
           =========================== */
        :root {
            --bg: #F1F5F9; /* page background */
            --card: #ffffff; /* card background */
            --primary: #3B82F6; /* main blue */
            --primary-700: #2563EB;
            --muted: #6b7280;
            --accent: #20bf6b;
            --danger: #eb3b5a;
            --radius-lg: 12px;
            --shadow-soft: 0 10px 30px rgba(15, 23, 42, 0.06);
            --container-max: 1150px;
            --control-padding: .55rem .75rem;
            --focus-ring: 0 0 0 0.18rem rgba(59,130,246,0.15);
        }

        /* ===========================
           Base layout
           =========================== */
        body {
            font-family: 'Inter', system-ui, -apple-system, "Segoe UI", Roboto, "Helvetica Neue", Arial;
            background: var(--bg);
            margin: 0;
            padding: 1.25rem;
            color: #0f172a;
        }

        .page-wrap {
            max-width: var(--container-max);
            margin: 0 auto;
        }

        /* Header */
        .page-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            gap: 1rem;
            margin-bottom: 1rem;
        }

        .title {
            font-weight: 700;
            font-size: 1.25rem;
        }

        .subtitle {
            color: var(--muted);
            font-size: .95rem;
        }

        /* ===========================
           Tabs (visual) - we keep your JS openTab()
           =========================== */
        .tab-strip {
            display: flex;
            gap: .5rem;
            flex-wrap: wrap;
        }

        .tab-btn {
            padding: .5rem .9rem;
            border-radius: 10px;
            background: transparent;
            border: 1px solid transparent;
            color: var(--muted);
            font-weight: 600;
            cursor: pointer;
            transition: all .12s ease;
        }

            .tab-btn:hover {
                background: rgba(59,130,246,.06);
            }

            .tab-btn.active {
                background: linear-gradient(90deg,var(--primary),var(--primary-700));
                color: white;
                box-shadow: var(--shadow-soft);
                border-color: rgba(0,0,0,0.04);
            }

        /* ===========================
           Card container styling
           =========================== */
        .crm-card {
            background: var(--card);
            border-radius: var(--radius-lg);
            padding: 1.1rem;
            box-shadow: var(--shadow-soft);
            border: 1px solid rgba(15,23,42,0.04);
            margin-bottom: 1rem;
        }

        .section-title {
            display: flex;
            align-items: center;
            gap: .6rem;
            margin: .4rem;
            margin-bottom: .7rem;
            font-weight: 700;
            color: #07102a;
        }

            .section-title .dot {
                width: 48px;
                height: 4px;
                border-radius: 8px;
                background: linear-gradient(90deg,var(--primary),var(--primary-700));
                display: inline-block;
            }

        /* ===========================
           Form controls
           =========================== */
        .form-control, .form-select {
            border-radius: 10px;
            padding: var(--control-padding);
            border: 1px solid #e6eef9;
            font-size: .95rem;
            transition: box-shadow .12s, border-color .12s;
        }

            .form-control:focus, .form-select:focus {
                outline: none;
                box-shadow: var(--focus-ring);
                border-color: var(--primary-700);
            }

        label.form-label {
            font-weight: 600;
            margin-bottom: .35rem;
            display: block;
        }
        /* Outer box of selected value */
        .select2-container .select2-selection--single {
            height: auto !important;
            padding: var(--control-padding) !important;
            border-radius: 10px !important;
            border: 1px solid #e6eef9 !important;
            font-size: .95rem !important;
            transition: box-shadow .12s, border-color .12s !important;
            display: flex !important;
            align-items: center !important;
        }

            /* Selected text */
            .select2-container .select2-selection--single .select2-selection__rendered {
                padding-left: 5px !important;
                color: #444 !important;
                line-height: 1.4 !important;
            }

            /* Dropdown arrow */
            .select2-container .select2-selection--single .select2-selection__arrow {
                height: 100% !important;
                right: 8px !important;
            }

        /* Focus/hover effect */
        .select2-container--default.select2-container--open .select2-selection--single,
        .select2-container--default .select2-selection--single:focus,
        .select2-container--default .select2-selection--single:hover {
            border-color: #94b3fd !important;
            box-shadow: 0 0 0 3px rgba(135, 166, 255, 0.25) !important;
        }

        /* ===========================
           Grid / table styling
           =========================== */
        .grid-wrapper {
            overflow-x: auto;
        }

        .gvEmployee-new, .gridview-modern {
            width: 100%;
            border-collapse: collapse;
            font-size: .95rem;
            min-width: 700px;
        }

            .gvEmployee-new th, .gridview-modern th {
                padding: .75rem .9rem;
                text-align: left;
                color: #fff;
                font-weight: 700;
                background: linear-gradient(90deg,var(--primary),var(--primary-700));
            }

            .gvEmployee-new td, .gridview-modern td {
                padding: .6rem .9rem;
                border-bottom: 1px solid #eef6ff;
                vertical-align: middle;
            }

            .gvEmployee-new tr:nth-child(even), .gridview-modern tr:nth-child(even) {
                background: #fbfdff;
            }

            .gvEmployee-new tr:hover, .gridview-modern tr:hover {
                background: rgba(32,191,107,0.03);
            }

            /* Inputs inside GridView (edit mode) */
            .gvEmployee-new input[type="text"], .gvEmployee-new select,
            .gridview-modern input[type="text"], .gridview-modern select {
                width: 100% !important;
                padding: .45rem .55rem !important;
                border-radius: 6px !important;
                border: 1px solid #d6e3fb !important;
                font-size: .92rem !important;
                box-sizing: border-box;
            }

        /* Action links */
        .gv-action-link {
            color: #0b5ed7;
            font-weight: 700;
            text-decoration: none;
        }

            .gv-action-link:hover {
                text-decoration: underline;
            }

        /* Buttons */
        .btn-brand {
            background: linear-gradient(90deg,var(--primary),var(--primary-700));
            color: #fff;
            border: none;
            border-radius: 10px;
            padding: .5rem .85rem;
            font-weight: 600;
        }

        .btn-accent {
            background: var(--accent);
            color: #fff;
            border: none;
            border-radius: 10px;
            padding: .45rem .75rem;
        }

        .btn-danger-custom {
            background: var(--danger);
            color: #fff;
            border: none;
            border-radius: 10px;
            padding: .45rem .75rem;
        }

        .btn-group-gap {
            gap: .6rem;
            display: flex;
            flex-wrap: wrap;
            margin-top: .6rem;
        }

        /* Tab content visibility (controlled by JS) */
        .tab-content {
            display: none;
        }

            .tab-content.active {
                display: block;
            }

        /* Responsive tweaks */
        @media (max-width:767.98px) {
            .gvEmployee-new, .gridview-modern {
                min-width: 600px;
            }

            .page-header {
                flex-direction: column;
                align-items: flex-start;
                gap: .5rem;
            }
        }

        /* Minor helper */
        .muted {
            color: var(--muted);
            font-size: .92rem;
        }
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <!-- Keep ScriptManager as-is -->
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableCdn="true"></asp:ScriptManager>

    <div class="page-wrap">

        <!-- header + tab strip -->
        <div class="page-header mb-3">
            <div>
                <div class="title">Management</div>
                <div class="subtitle">Branches & Employees — Add, Update and View records</div>
            </div>

            <div class="tab-strip" role="tablist" aria-label="Management tabs">
                <div class="tab-btn active" onclick="openTab('tabAdd', this)" role="tab" tabindex="0" aria-controls="tabAdd">Add New / Update</div>
                <div class="tab-btn" onclick="openTab('tabView', this)" role="tab" tabindex="0" aria-controls="tabView">View All</div>
                <div class="tab-btn" onclick="openTab('tabHistory', this)" role="tab" tabindex="0" aria-controls="tabView">View All History</div>

            </div>
        </div>

        <!-- persist active tab across postbacks -->
        <asp:HiddenField ID="hdnactivetab" runat="server" />

        <!-- ------------------ ADD / UPDATE panel ------------------ -->
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div id="tabAdd" class="tab-content active" role="tabpanel" aria-hidden="false">
                    <div class="crm-card">

                        <!-- hidden fields -->
                        <asp:HiddenField ID="hdnbranchid" runat="server" />
                        <asp:HiddenField ID="hdnFormMode" runat="server" Value="add" />

                        <!-- Branch Details -->
                        <div class="card-section">
                            <div class="section-title">Branch Details <span class="dot"></span></div>
                            <div class="row g-3">
                                <div class="col-md-4">
                                    <label class="form-label">Branch Name</label>
                                    <asp:HiddenField ID="hdnbranchname" runat="server" />
                                    <asp:DropDownList ID="ddlbranchname" CssClass="form-select select2" runat="server"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlbranchname_SelectedIndexChanged" />
                                </div>

                                <div class="col-md-4">
                                    <label class="form-label">Branch Department</label>
                                    <asp:DropDownList ID="ddlbranchdep" CssClass="form-select select2" runat="server" />
                                </div>

                                <div class="col-md-4">
                                    <label class="form-label">Branch Head</label>
                                    <asp:TextBox ID="txtbranchhead" runat="server" CssClass="form-control" placeholder="Enter Branch Head" />
                                </div>
                            </div>
                        </div>
                        <asp:Panel ID="emppanel" runat="server">
                            <!-- Employee Add -->
                            <div class="card-section">
                                <div class="section-title">ADD Employee <span class="dot"></span></div>
                                <div class="row g-3">
                                    <div class="col-md-4">
                                        <label class="form-label">Employee Name</label>
                                        <asp:TextBox ID="txtempname" runat="server" CssClass="form-control" placeholder="Enter Employee Name" />
                                    </div>

                                    <div class="col-md-4">
                                        <label class="form-label">Salary</label>
                                        <asp:TextBox ID="txtsalary" runat="server" CssClass="form-control" placeholder="Enter Employee Salary" />
                                    </div>

                                    <div class="col-md-4">
                                        <label class="form-label">Email</label>
                                        <asp:TextBox ID="txtempemail" runat="server" CssClass="form-control" TextMode="Email" placeholder="Enter Employee Email" />
                                    </div>

                                    <div class="col-md-4">
                                        <label class="form-label">Gender</label>
                                        <asp:DropDownList ID="ddlempgender" runat="server" CssClass="form-select">
                                            <asp:ListItem Text="--Select-Gender--" />
                                            <asp:ListItem Text="Male" Value="male" />
                                            <asp:ListItem Text="Female" Value="female" />
                                        </asp:DropDownList>
                                        <asp:Label ID="lblddlgender" runat="server" ForeColor="Red" CssClass="muted"></asp:Label>
                                    </div>

                                    <div class="col-md-4">
                                        <label class="form-label">Branch</label>
                                        <asp:TextBox ID="txtempbranch" runat="server" CssClass="form-control" placeholder="Enter Employee Branch" />
                                    </div>

                                    <div class="col-12 d-flex justify-content-start btn-group-gap">
                                        <asp:Button ID="btnaddemp" runat="server" Text="Add Employee" CssClass="btn btn-brand" OnClick="btnaddemp_Click" />
                                    </div>
                                </div>
                            </div>

                        </asp:Panel>


                        <!-- Employee List -->
                        <div class="card-section">
                            <div class="section-title">Employee List <span class="dot"></span></div>

                            <div class="grid-wrapper">
                                <asp:GridView ID="gvEmployee" runat="server" AutoGenerateColumns="False"
                                    OnRowDataBound="gvEmployee_RowDataBound"
                                    OnRowEditing="gvEmployee_RowEditing"
                                    OnRowUpdating="gvEmployee_RowUpdating"
                                    OnRowCancelingEdit="gvEmployee_RowCancelingEdit"
                                    OnRowDeleting="gvEmployee_RowDeleting"
                                    CssClass="gvEmployee-new"
                                    DataKeyNames="emp_id"
                                     ShowHeaderWhenEmpty="true">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Name">
                                            <ItemTemplate>
                                                <%# Eval("emp_name") %>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtempname" runat="server" Text='<%# Bind("emp_name") %>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Salary">
                                            <ItemTemplate>
                                                <%# Eval("emp_salary") %>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtempsalary" runat="server" Text='<%# Bind("emp_salary") %>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="EmailID">
                                            <ItemTemplate>
                                                <%# Eval("emp_emailid") %>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtempemailid" runat="server" Text='<%# Bind("emp_emailid") %>' />
                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Gender">
                                            <ItemTemplate>
                                                <%# Eval("emp_gender") %>
                                                <asp:HiddenField ID="hdnempgender" runat="server" Value='<%# Eval("emp_gender") %>' />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlempgender" runat="server">
                                                    <asp:ListItem Text="male" Value="male" />
                                                    <asp:ListItem Text="female" Value="female" />
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Branch">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtempbranch" runat="server" ReadOnly="true" Text='<%# Eval("emp_branch") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEdit" CommandName="Edit" runat="server">Edit</asp:LinkButton>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:LinkButton ID="lnkUpdate" CommandName="Update" runat="server" Text="Update" CssClass="me-2" />
                                                <asp:LinkButton ID="lnkCancel" CommandName="Cancel" runat="server" Text="Cancel" />
                                                <asp:LinkButton ID="lnkDelete" CommandName="Delete" runat="server" Text="Delete" />

                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>

                            <div class="d-flex gap-2 mt-3 btn-group-gap">
                                <asp:Button ID="btnsaveall" runat="server" Text="Save All" CssClass="btn btn-brand" OnClick="btnsaveall_Click" />
                                <asp:Button ID="btnupdateall" runat="server" Text="Update All" CssClass="btn btn-brand" OnClick="btnupdateall_Click" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel All" CssClass="btn btn-danger-custom" OnClick="btnCancel_Click" />
                            </div>
                        </div>

                    </div>
                    <!-- /.crm-card -->
                </div>
                <!-- /#tabAdd -->
            </ContentTemplate>
        </asp:UpdatePanel>

        <!-- ------------------ VIEW ALL panel (separate UpdatePanel sibling) ------------------ -->
        <asp:UpdatePanel ID="upviewpnl" runat="server">
            <ContentTemplate>
                <div id="tabView" class="tab-content" role="tabpanel" aria-hidden="true">
                    <div class="crm-card">
                        <div class="section-title">View All Records <span class="dot"></span></div>

                        <div class="grid-wrapper">
                            <asp:GridView ID="gvBranch" runat="server" AutoGenerateColumns="false" CssClass="gridview-modern" ShowHeaderWhenEmpty="true"
                                DataKeyNames="branch_kid">
                                <Columns>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemStyle CssClass="checkbox-col" />
                                        <HeaderStyle CssClass="checkbox-col" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkselect" runat="server" onclick="singlecheck(this)" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="branch_kid" HeaderText="Branch KId" Visible="false" />
                                    <asp:BoundField DataField="branch_id" HeaderText="Branch Id" Visible="false" />
                                    <asp:BoundField DataField="branch_name" HeaderText="Branch Name" />
                                    <asp:BoundField DataField="branch_department" HeaderText="Branch Department" />
                                    <asp:BoundField DataField="branch_head" HeaderText="Branch Head" />
                                </Columns>
                            </asp:GridView>
                        </div>

                        <div class="d-flex gap-2 mt-3 btn-group-gap">
                            <asp:Button ID="btneditbranch" runat="server" Text="Edit" CssClass="btn btn-brand" OnClick="btneditbranch_Click" />
                            <asp:Button ID="btndelete" runat="server" Text="Delete" CssClass="btn btn-danger-custom" OnClick="btndelete_Click" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div id="tabHistory" class="tab-content" role="tabpanel" aria-hidden="true">
                    <div class="crm-card">
                        <div class="section-title">View All History <span class="dot"></span></div>

                        <div class="grid-wrapper">
                            <asp:GridView ID="gvHistory" runat="server" AutoGenerateColumns="false" CssClass="gridview-modern"
                                DataKeyNames="branch_kid" ShowHeaderWhenEmpty="true">
                                <Columns>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemStyle CssClass="checkbox-col" />
                                        <HeaderStyle CssClass="checkbox-col" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkselect" runat="server" onclick="singlecheck(this)" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="branch_kid" HeaderText="Branch KId" Visible="false" />
                                    <asp:BoundField DataField="branch_id" HeaderText="Branch Id" Visible="false" />
                                    <asp:BoundField DataField="branch_name" HeaderText="Branch Name" />
                                    <asp:BoundField DataField="branch_department" HeaderText="Branch Department" />
                                    <asp:BoundField DataField="branch_head" HeaderText="Branch Head" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="d-flex gap-2 mt-3 btn-group-gap">
                            <asp:Button ID="btnview" runat="server" Text="view" CssClass="btn btn-brand" OnClick="btnview_Click" />
                        </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
    <!-- /.page-wrap -->

    <!-- ===========================
         Scripts
         - jQuery (for select2)
         - Select2
         - Bootstrap bundle
         - Init scripts (openTab, singlecheck, select2 init & reinit)
         =========================== -->
    <script src="https://code.jquery.com/jquery-3.7.1.min.js" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>

    <script>
        // openTab: shows target tab and persists to hidden field; original logic preserved
        function openTab(tabId, tabButton) {
            document.querySelectorAll(".tab-content").forEach(function (el) {
                el.classList.remove("active");
                el.setAttribute("aria-hidden", "true");
            });
            document.querySelectorAll(".tab-btn").forEach(function (b) { b.classList.remove("active"); });

            var target = document.getElementById(tabId);
            if (target) {
                target.classList.add("active");
                target.setAttribute("aria-hidden", "false");
            }
            if (!tabButton || !tabButton.classList || !tabButton.classList.contains("tab-btn")) {
                tabButton = document.querySelector(`.tab-btn[onclick*="${tabId}"]`);
            }

            if (tabButton && tabButton.classList)
                tabButton.classList.add("active");

            var hf = document.getElementById('<%= hdnactivetab.ClientID %>');
            if (hf) hf.value = tabId;
        }

        // singlecheck: ensures only one checkbox selected in view grid (preserves original behavior)
        function singlecheck(clickedcheckbox) {
            const checkboxes = document.querySelectorAll("input[id*='chkselect']");
            checkboxes.forEach(cb => {
                if (cb !== clickedcheckbox) { cb.checked = false; }
            });
        }

        // Initialize select2 safely and re-init after partial postbacks
        function initSelect2() {
            try {
                $('.select2').select2({
                    placeholder: "--Select--",
                    allowClear: true,
                    minimumResultsForSearch: 0
                });
            } catch (e) {
                console.warn("Select2 init error:", e);
            }
        }

        // DOM ready: init select2 and restore active tab if present
        document.addEventListener("DOMContentLoaded", function () {
            initSelect2();

            var hf = document.getElementById('<%= hdnactivetab.ClientID %>');
            if (hf && hf.value) {
                var tabId = hf.value;
                var btn = document.querySelector(".tab-btn[onclick*=\"" + tabId + "\"]");
                if (btn) openTab(tabId, btn);
                else openTab('tabAdd', document.querySelector('.tab-btn'));
            } else {
                openTab('tabAdd', document.querySelector('.tab-btn'));
            }
        });

        // Re-init after UpdatePanel partial postbacks
        if (typeof (Sys) !== "undefined" && Sys.WebForms && Sys.WebForms.PageRequestManager) {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                initSelect2();
            });
        }
    </script>

</asp:Content>
