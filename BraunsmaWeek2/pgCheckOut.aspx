<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pgCheckOut.aspx.cs" Inherits="BraunsmaWeek2.pgCheckOut" 
MasterPageFile="~/Layout.Master" %>
<%@ MasterType VirtualPath="~/Layout.Master"%>

<asp:Content ID="ContentArea1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <label>First Name</label>
    <div>
        <asp:TextBox ID="txtFirstName" runat="server" Width="280px" CssClass="form-control"></asp:TextBox>
    </div>
    <label>Last Name</label>
    <div>
        <asp:TextBox ID="txtLastName" runat="server" Width="280px" CssClass="form-control"></asp:TextBox>
    </div>
    <div>
        <div>
            <label>Street</label>
            <asp:TextBox ID="txtStreet" runat="server" Width="280px" CssClass="form-control"></asp:TextBox>
        </div>
        <div>
            <label>City</label>
            <asp:TextBox ID="txtCity" runat="server" Width="280px" CssClass="form-control"></asp:TextBox>
            <label>State</label>
            <asp:TextBox ID="txtState" runat="server" Width="280px" CssClass="form-control"></asp:TextBox>
        </div>
    </div>  
    <label>Phone Number</label>
    <div>
        <asp:TextBox ID="txtPhone" runat="server" Width="280px" CssClass="form-control"></asp:TextBox>
    </div>
</asp:Content>
<asp:Content ID="ContentArea2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <label><strong>Payment Method</strong></label>
    <asp:RadioButtonList ID="rblCCType" runat="server" RepeatDirection="Horizontal" CssClass="form-check">
        <asp:ListItem>Visa</asp:ListItem>
        <asp:ListItem>Master Card</asp:ListItem>
        <asp:ListItem>Discover</asp:ListItem>
    </asp:RadioButtonList>
    <label><strong>Credit Card Number</strong></label>
    <asp:TextBox ID="txtCCNumber" runat="server" CssClass="form-control"></asp:TextBox><br />
    <asp:Button ID="btnSubmit" PostBackUrl="~/pgConfirm.aspx" runat="server" CssClass="btn btn-primary" Text="Submit"/>
</asp:Content>