<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ProgramaholicsAnonymous.Login" %>
<%@ MasterType VirtualPath="~/Layout.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadSection" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="NavigationArea" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form method="post" runat="server">

        <div class="form-group">
            <label>Username</label>
            <asp:TextBox CssClass="form-control" ID="txtUsername" runat="server"/>
        </div>
        
        <div class="form-group">
            <label>Password:</label>
            <asp:TextBox TextMode="Password" CssClass="form-control" ID="txtPassword"  runat="server"/>
        </div>
        
        <asp:Button CssClass="btn btn-danger" runat="server" ID="btnRegister" OnClick="btnRegister_Click" Text="Register"/>
        <asp:Button CssClass="btn btn-primary" runat="server" ID="btnLogin" OnClick="btnLogin_Click" Text="Login" />

    </form>
</asp:Content>
