<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pgConfirm.aspx.cs" Inherits="BraunsmaWeek1.pgConfirm" %>
<%@ PreviousPageType VirtualPath="~/pgCheckOut.aspx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <table>
            <thead>
                <tr>
                    <th></th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
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
        <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
        <p>
        <asp:Button ID="btnSubmit" runat="server" Text="Submit Order" />
        </p>
    </form>
</body>
</html>
