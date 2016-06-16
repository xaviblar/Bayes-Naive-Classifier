<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="BayesAI._Default" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="intro" style="text-align:center">
        <br>
        <br>
        <br>
        <br>
        <br>
        <br>
        <br>
        <asp:Button ID="FacebookFileBtn" runat="server" Text="Facebook" style="margin-left: 0px; height: 26px;" Width="66px" OnClick="FacebookFileBtn_Click"  />
        <asp:Button ID="TwiterFileBtn" runat="server" Text="Twitter"  />
        <asp:TextBox ID="URLtxtBox" runat="server"  Width="108px" ForeColor="#FF9999">Directory Path</asp:TextBox>
        <asp:Button ID="GoURLBtn" runat="server" Text="Go" OnClick="GoURLBtn_Click" /> 
        <asp:FileUpload ID="FileUpload1" runat="server" style="margin: 0 auto" Width="267px" />     
        <br>
        <asp:Button ID="analiseBtn" runat="server" OnClick="analiseBtn_Click" Text="Analise File" />
        <br>
        <br>
        <br>
        <br>
        <br>
        <br>
        <br>
    </div>

</asp:Content>
