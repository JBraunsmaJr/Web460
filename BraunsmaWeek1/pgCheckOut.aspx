<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pgCheckOut.aspx.cs" Inherits="BraunsmaWeek1.pgCheckOut" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" method="post" action="pgConfirm.aspx">
       <label>First Name</label>
        <div>
            <asp:TextBox ID="txtFirstName" runat="server" Width="280px"></asp:TextBox>
        </div>
        <label>Last Name</label>
        <div>
            <asp:TextBox ID="txtLastName" runat="server" Width="280px"></asp:TextBox>
        </div>
        <div>
            <div>
                <label>Street</label>
                <asp:TextBox ID="txtStreet" runat="server" Width="280px"></asp:TextBox>
            </div>
            <div>
                <label>City</label>
                <asp:TextBox ID="txtCity" runat="server" Width="280px"></asp:TextBox>
                <label>State</label>
                <asp:TextBox ID="txtState" runat="server" Width="280px"></asp:TextBox>
            </div>
        </div>  
        <label>Phone Number</label>
        <div>
            <asp:TextBox ID="txtPhone" runat="server" Width="280px"></asp:TextBox>
        </div>
        <label><strong>Payment Method</strong></label>
        <asp:RadioButtonList ID="rblCCType" runat="server" RepeatDirection="Horizontal">
            <asp:ListItem>Visa</asp:ListItem>
            <asp:ListItem>Master Card</asp:ListItem>
            <asp:ListItem>Discover</asp:ListItem>
        </asp:RadioButtonList>
        <asp:TextBox ID="txtCCNumber" runat="server"></asp:TextBox><br />
        <asp:Button ID="btnSubmit" PostBackUrl="~/pgConfirm.aspx" runat="server" Text="Submit"/>
    </form>
</body>
</html>
