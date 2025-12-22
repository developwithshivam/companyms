<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserRegistration.aspx.cs" Inherits="CompanyMS.UserRegistration" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form-container {
            max-width: 500px;
            margin: 30px auto;
            background: #fff;
            padding: 30px 35px;
            border-radius: 10px;
            box-shadow: 0 6px 20px rgba(0,0,0,0.12);
        }

            .form-container h2 {
                text-align: center;
                color: #1e3a8a;
                margin-bottom: 35px;
            }

        .form-group {
            margin-bottom: 18px;
        }

            .form-group label {
                display: block;
                font-weight: 600;
                color: #333;
                margin-bottom: 6px;
            }

            .form-group input[type="text"],
            .form-group input[type="password"],
            .form-group input[type="email"],
            .form-group input[type="tel"],
            .form-group textarea {
                width: 100%;
                padding: 10px 12px;
                font-size: 14px;
                border-radius: 6px;
                border: 1px solid #ccc;
                transition: 0.3s;
                box-sizing: border-box;
            }

                .form-group input:focus,
                .form-group textarea:focus {
                    border-color: #2563eb;
                    box-shadow: 0 0 5px rgba(37, 99, 235, 0.5);
                    outline: none;
                }

        .btn-register {
            display: block;
            width: 100%;
            background: #2563eb;
            color: white;
            padding: 12px;
            border: none;
            border-radius: 6px;
            font-size: 16px;
            font-weight: bold;
            cursor: pointer;
            transition: 0.3s;
        }

            .btn-register:hover {
                background: #1e3a8a;
            }

        .text-danger {
            color: red;
            font-size: 13px;
            margin-top: 3px;
        }
        
        .auth-inline-note {
            font-size: 14px;
            display: block;
            text-align: center;
            margin-top: 10px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-container">
        <h2>User Registration</h2>

        <div class="form-group">
            <asp:Label ID="lbl_username" runat="server" Text="User Name"></asp:Label>
            <asp:TextBox ID="txt_username" runat="server" CssClass="form-control" placeholder="Enter User Name"></asp:TextBox>
            <asp:Label ID="lblusermessage" runat="server" CssClass="text-danger"></asp:Label>
        </div>

        <div class="form-group">
            <asp:Label ID="lbl_password" runat="server" Text="Password"></asp:Label>
            <asp:TextBox ID="txt_password" runat="server" TextMode="Password" CssClass="form-control" placeholder="Enter Password"></asp:TextBox>
        </div>

        <div class="form-group">
            <asp:Label ID="lbl_confirmpass" runat="server" Text="Confirm Password"></asp:Label>
            <asp:TextBox ID="txt_confirmpass" runat="server" TextMode="Password" CssClass="form-control" placeholder="Confirm Password"></asp:TextBox>
        </div>

        <div class="form-group">
            <asp:Label ID="lbl_email" runat="server" Text="Email"></asp:Label>
            <asp:TextBox ID="txt_email" runat="server" TextMode="Email" CssClass="form-control" placeholder="Enter Email"></asp:TextBox>
        </div>

        <div class="form-group">
            <asp:Label ID="lbl_contact" runat="server" Text="Contact"></asp:Label>
            <asp:TextBox ID="txt_contact" runat="server" TextMode="Phone" CssClass="form-control" placeholder="Enter Contact"></asp:TextBox>
        </div>

        <div class="form-group">
            <asp:Label ID="lbl_address" runat="server" Text="Address"></asp:Label>
            <asp:TextBox ID="txt_address" runat="server" CssClass="form-control" placeholder="Enter Address"></asp:TextBox>
        </div>

        <asp:Button ID="btn_register" runat="server" Text="Register" CssClass="btn-register" OnClick="btn_register_Click" />
        <asp:Label ID="lblmessage" runat="server" CssClass="text-danger"></asp:Label>

        <span class="auth-inline-note">
            <asp:Label ID="lblsignin" runat="server" Text="Already have an account?"></asp:Label>
            <asp:HyperLink ID="signinlink" runat="server" NavigateUrl="~/Login.aspx"> Sign in </asp:HyperLink>
        </span>
    </div>
</asp:Content>
