<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="AccountConfirmation.aspx.cs" Inherits="ProgramaholicsAnonymous.AccountConfirmation" %>
<%@ MasterType VirtualPath="~/Layout.Master"%>
<%@ PreviousPageType VirtualPath="~/Login.aspx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadSection" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigationArea" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form method="post" runat="server">
        <asp:Label ID="lblPassword" runat="server" Visible="false"/>
        <table class="table table-dark table-hover">
            <tr>
                <td>Username:</td>
                <td><asp:Label ID="lblUsername" runat="server"/></td>
            </tr>
        </table>

        <div class="form-group">
            <label>City</label>
            <asp:TextBox CssClass="form-control" ID="txtCity" runat="server"/>
        </div>
        
        <div class="form-group">
            <label>State</label>
            <asp:TextBox CssClass="form-control" ID="txtState" runat="server"/>
        </div>
        
        <div class="form-group">
            <label>Least Favorite Programming Language</label>
            <asp:TextBox CssClass="form-control" ID="txtLeastFavorite" runat="server"/>
        </div>
        
        <div class="form-group">
            <label>Favorite Programming Language</label>
            <asp:TextBox CssClass="form-control" ID="txtFavorite" runat="server"/>
        </div>

        <asp:Button CssClass="btn btn-primary" runat="server" Text="Create Account" ID="btnCreateAccount" OnClick="btnCreateAccount_Click" />
        <asp:Button CssClass="btn btn-danger" runat="server" PostBackUrl="~/Login.aspx" Text="Cancel"/>
    </form>
</asp:Content>
