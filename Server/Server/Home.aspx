<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="WebApplication1._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">

        <div class="row">
            <div class="col-md-4">
                <div>
                    <asp:Label ID="lblWeb" runat="server" Text="Sending/Received :"></asp:Label>
                    <asp:TextBox ID="txtWeb" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-4">
                <asp:Button ID="btnSubmit" Text="Submit" runat="server" OnClick="btnSubmit_Click" />
            
                <asp:Button ID="btnRefresh" Text="Refresh" runat="server" OnClick="btnRefresh_Click" />
         
                <asp:Button ID="btnRefreshData" Text="Refresh Data" runat="server" OnClick="btnRefreshData_Click" />
            </div>
        </div>
        <div class="row">&nbsp;</div>
    <div class="row">
        <div class="col-md-8 mx-auto">

            <asp:GridView ID="gvReceived" class="table table-bordered table-condensed table-responsive table-hover" runat="server" AutoGenerateColumns="true">
            </asp:GridView>
        </div>
        <div class="row">
            <div class="col-md-8 mx-auto">
                <asp:GridView ID="gvSending" CssClass="table" runat="server" AutoGenerateColumns="true">
                </asp:GridView>
            </div>
        </div>
    </div>
         </div>



</asp:Content>
