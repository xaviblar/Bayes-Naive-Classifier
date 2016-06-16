<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AnalisisResult.aspx.cs" Inherits="BayesAI.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="intro" style="text-align:center">
        <h1>&nbsp;Analisis Results</h1>
        <br />
        <asp:Label ID="Label1" runat="server" Text="Label">Total Analized messages: </asp:Label>
        <asp:Label ID="TotalMessagesLbl" runat="server"></asp:Label>
        <br>
        <asp:Label ID="Label2" runat="server" Text="Label">Total Users: </asp:Label>
        <asp:Label ID="UsersLbl" runat="server"></asp:Label>
        <br />
        <asp:Label ID="Label4" runat="server" Text="Label">Show Information Charts: </asp:Label>
        <asp:DropDownList ID="DropDownList1" runat="server">
            <asp:ListItem>User Percentage per Category</asp:ListItem>
            <asp:ListItem>User Percentage per Language</asp:ListItem>
            <asp:ListItem>Categories Percentage per Analisis</asp:ListItem>
        </asp:DropDownList>
        <br />
        <br />
        <br />
        <asp:Chart ID="myChart" runat="server" Width="600px">
           
        </asp:Chart>
        <br />
        <br />
        <br />

    </div>

</asp:Content>
