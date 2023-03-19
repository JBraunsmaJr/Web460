<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pgConfirm.aspx.cs" Inherits="BraunsmaWeek3.pgConfirm" MasterPageFile="~/Layout.Master"%>
<%@ MasterType VirtualPath="~/Layout.Master"%>
<%@ PreviousPageType VirtualPath="~/pgCheckOut.aspx" %>

<asp:Content ID="ContentArea1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label Visible="false" runat="server" ID="lblFirstName" />
    <asp:Label Visible="false" runat="server" ID="lblLastName" />
    <asp:Label Visible="false" runat="server" ID="lblStreet" />
    <asp:Label Visible="false" runat="server" ID="lblState" />
    <asp:Label Visible="false" runat="server" ID="lblCity" />
    <table>
        <tr>
            <td><strong>Name</strong></td>
            <td>
                <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td><strong>Address</strong></td>
            <td>
                <asp:Label ID="lblAddress" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td><strong>Phone</strong></td>
            <td>
                <asp:Label ID="lblPhone" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="ContentArea2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <table>
        <thead>
            <tr>
                <th></th>
                <th></th>
            </tr>
        </thead>
        <tbody>
            
            <tr>
                <td><strong>Credit Card Type</strong></td>
                <td>
                    <asp:Label ID="lblCCType" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td><strong>Credit Card Number</strong></td>
                <td>
                    <asp:Label ID="lblCCNumber" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </tbody>
    </table>
    <asp:Button ID="btnSubmit" runat="server" Text="Submit Order" CssClass="btn btn-success" />
</asp:Content>
        
