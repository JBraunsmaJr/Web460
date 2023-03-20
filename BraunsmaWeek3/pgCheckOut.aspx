<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pgCheckOut.aspx.cs" Inherits="BraunsmaWeek3.pgCheckOut" 
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
    
    <asp:Button ID="btnClearForm" Text="Clear Form" runat="server" OnClick="ClearForm_OnClick" CssClass="btn btn-secondary" />
    <asp:Button ID="btnAddCustomer" Text="Add Customer" runat="server" OnClick="btnAddCustomer_OnClick" CssClass="btn btn-primary" />    
    <asp:Button ID="btnFindLastName" Text="Find Last Name" runat="server" OnClick="btnFindLastName_OnClick" CssClass="btn btn-info" />
    <asp:Button ID="btnUpdateCustomer" Text="Update Customer" runat="server" OnClick="btnUpdateCustomer_OnClick" CssClass="btn btn-warning" />
    
    <br /><br /><br />
    <label>Existing Customers</label>
    <div style="width: 40%">
        <asp:DropDownList ID="dropdownLastNames" runat="server" CssClass="form-control"></asp:DropDownList>
    </div>
    <br/>
    <label><strong>Customer ID:</strong></label>
    <asp:Label ID="lblCustomerId" runat="server" />
    <br/><br /><br />
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