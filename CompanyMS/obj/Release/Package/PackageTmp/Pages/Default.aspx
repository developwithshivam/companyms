<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CompanyMS.Pages.Default" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous">

    <style>
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

        body {
            font-family: 'Inter', system-ui, -apple-system, "Segoe UI", Roboto, "Helvetica Neue", Arial;
            background: var(--bg);
            margin: 0;
            padding: 1.25rem;
            color: #0f172a;
        }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableCdn="true"></asp:ScriptManager>
    <div class="page-wrap">
        <h1>Dashboard</h1>
    </div>
</asp:Content>
