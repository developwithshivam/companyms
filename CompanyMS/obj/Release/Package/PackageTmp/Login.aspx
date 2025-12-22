<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CompanyMS.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* --- Main Container --- */
        .auth-container {
            display: flex;
            justify-content: center;
            align-items: center;
            padding: 50px 10px;
        }

        .auth-card {
            background: #fff;
            padding: 35px 40px;
            border-radius: 12px;
            width: 100%;
            max-width: 420px;
            box-shadow: 0 6px 20px rgba(0,0,0,0.12);
        }

        .auth-title {
            font-size: 24px;
            font-weight: 600;
            text-align: center;
            color: #1e3a8a;
            margin-bottom: 25px;
        }

        /* --- Input Styling --- */
        .auth-card input[type="text"],
        .auth-card input[type="password"] {
            width: 100%;
            padding: 12px;
            margin-bottom: 18px;
            border: 1px solid #ccc;
            border-radius: 8px;
            font-size: 14px;
            transition: 0.3s;
        }

        .auth-card input:focus {
            border-color: #2563eb;
            box-shadow: 0 0 5px rgba(37, 99, 235, 0.4);
            outline: none;
        }

        /* --- Buttons --- */
        .btn-primary-custom {
            width: 100%;
            background: #2563eb;
            color: #fff;
            padding: 12px;
            border: none;
            border-radius: 8px;
            font-size: 16px;
            font-weight: bold;
            cursor: pointer;
            transition: 0.3s;
            margin-top: 10px;
        }

        .btn-primary-custom:hover {
            background: #1e3a8a;
        }

        /* --- Links --- */
        .auth-links {
            margin-top: 15px;
            text-align: center;
        }

        .auth-links a {
            color: #2563eb;
            text-decoration: none;
            font-size: 14px;
        }

        .auth-links a:hover {
            text-decoration: underline;
        }

        .auth-inline-note {
            font-size: 14px;
            display: block;
            text-align: center;
            margin-top: 10px;
        }

        .text-danger {
            color: red;
            font-size: 13px;
            margin-bottom: 10px;
        }

        @media (max-width: 400px) {
            .auth-card {
                padding: 25px;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableCdn="true"></asp:ScriptManager>

    <div class="auth-container">

        <!-- LOGIN PANEL -->
        <asp:Panel ID="pnlLogin" runat="server" CssClass="auth-card">

            <div class="auth-title">Login</div>

            <asp:Label ID="lbl_username" runat="server" Text="Username"></asp:Label>
            <asp:TextBox ID="txt_username" runat="server" CssClass="form-control" placeholder="Enter Username"></asp:TextBox>

            <asp:Label ID="lbl_password" runat="server" Text="Password"></asp:Label>
            <asp:TextBox ID="txt_password" runat="server" TextMode="Password" CssClass="form-control" placeholder="Enter Password"></asp:TextBox>

            <asp:Button ID="btn_button" runat="server" Text="Login" CssClass="btn-primary-custom" OnClick="btn_button_Click" />

            <asp:Label ID="lbl_msg" runat="server" CssClass="text-danger"></asp:Label>

            <span class="auth-inline-note">
                <asp:Label ID="lblsignup" runat="server" Text="Don't have an account?"></asp:Label>
                <asp:HyperLink ID="signuplink" runat="server" NavigateUrl="~/UserRegistration.aspx"> Sign Up </asp:HyperLink>
            </span>

            <div class="auth-links">
                <asp:LinkButton ID="Linkforgetpassword" runat="server" OnClick="Linkforgetpassword_Click">Forgot Password?</asp:LinkButton>
            </div>

        </asp:Panel>

        <!-- RESET PASSWORD PANEL -->
        <asp:Panel ID="pnlforgetpass" runat="server" CssClass="auth-card" Visible="false">

            <div class="auth-title">Reset Password</div>

            <asp:Label ID="lblresetuser" runat="server" Text="Username"></asp:Label>
            <asp:TextBox ID="txt_resetusername" runat="server" CssClass="form-control" placeholder="Enter Username"></asp:TextBox>

            <asp:Label ID="lblResetPassword" runat="server" Text="New Password"></asp:Label>
            <asp:TextBox ID="txt_restpass" runat="server" TextMode="Password" CssClass="form-control" placeholder="Enter New Password"></asp:TextBox>

            <asp:Label ID="lblcrmresetpass" runat="server" Text="Confirm Password"></asp:Label>
            <asp:TextBox ID="txt_crmpass" runat="server" TextMode="Password" CssClass="form-control" placeholder="Confirm Password"></asp:TextBox>

            <asp:Button ID="btn_resetpass" runat="server" Text="Save" CssClass="btn-primary-custom" OnClick="btn_resetpass_Click" />

            <div class="auth-links" style="margin-top:15px;">
                <asp:HyperLink ID="lnkbacktologin" runat="server" NavigateUrl="~/Login.aspx">Back to Login</asp:HyperLink>
            </div>

        </asp:Panel>

    </div>
</asp:Content>
