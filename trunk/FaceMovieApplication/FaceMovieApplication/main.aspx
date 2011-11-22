<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="FaceMovieApplication.WebForm1" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .grid { margin: 4px; border-collapse: collapse; width: 600px; }
        .head { background-color: #E8E8E8; font-weight: bold; color: #FFF; }
        .grid th, .grid td { border: 1px solid #C0C0C0; padding: 5px; }
        .alt { background-color: #E8E8E8; color: #000; }
        .product { width: 200px; font-weight:bold;}
    </style>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent"> 
<form action="">
        <br />

        <asp:gridview id="MovieGridView" 
            autogeneratecolumns="true" 
            runat="server">
        </asp:gridview>

        <asp:Button id="Recomendar_Button" runat="server" onclick="ClickHandler" Text="Recomendame una película"></asp:Button>
        <br />
        <asp:Image ID="img1" runat="server" ImageUrl=''></asp:Image>
        <h1>Película seleccionada</h1>
        <div>
            <asp:DataGrid ID="Grid" runat="server" PageSize="5" AllowPaging="True" DataKeyField="MovieId"
            AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None">
           <Columns>
                <asp:BoundColumn HeaderText="MovieID" DataField="MovieID"></asp:BoundColumn>

                <asp:BoundColumn HeaderText="MovieFacebookPageId" DataField="MovieFacebookPageId"></asp:BoundColumn>

                <asp:BoundColumn HeaderText="MovieName" DataField="MovieName"></asp:BoundColumn>
            
            </Columns>

            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
            <SelectedItemStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
            <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" Mode="NumericPages" />
            <AlternatingItemStyle BackColor="White" />
            <ItemStyle BackColor="#FFFBD6" ForeColor="#333333" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
        </asp:DataGrid>
    </div>

</form>



</asp:Content>
