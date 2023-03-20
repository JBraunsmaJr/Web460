<%@ Page Title="Account Details" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="AccountDetails.aspx.cs" Inherits="ProgramaholicsAnonymous.AccountDetails" %>
<%@ MasterType VirtualPath="~/Layout.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadSection" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigationArea" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form method="post" runat="server">
        <table class="table table-dark table-hover table-striped">
            <tr style="vertical-align: top"> 
                <td>
                    <table class="table table-dark">
                        <tr>
                            <td>Username</td>
                            <td><asp:Label ID="lblUsername" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>City</td>
                            <td><asp:Label ID="lblCity" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>State</td>
                            <td><asp:Label ID="lblState" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>Favorite Programming Language</td>
                            <td><asp:Label ID="lblFavorite" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>Least Favorite Programming Language</td>
                            <td><asp:Label ID="lblLeastFavorite" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>Last Completed</td>
                            <td><asp:Label ID="lblLastCompleted" runat="server" /></td>
                        </tr>
                    </table>

                    <asp:Button runat="server" 
                        CssClass="btn btn-primary" 
                        Text="Edit Details" 
                        ID="btnEditDetails" 
                        OnClick="btnEditDetails_Click"/>

                    <asp:Button runat="server" 
                        CssClass="btn btn-danger" 
                        Text="Delete Account" 
                        ID="btnDeleteAccount" 
                        OnClick="btnDeleteAccount_Click" />
                </td>
                <td>
                    <asp:Button ID="btnExportStats" CssClass="btn btn-success" OnClick="btnExportStats_Click" Text="Export Stats" runat="server" />

                    <table class="table table-dark table-hover table-striped">
                        <thead>
                            <tr>
                                <th colspan="3" class="text-center">Applications</th>
                            </tr>
                            <tr>
                                <th>Application Name</th>
                                <th>Language</th>
                                <th>Date Completed</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td colspan="3" class="text-center">No records</td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</asp:Content>
