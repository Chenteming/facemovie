<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FaceMovie.aspx.cs" Inherits="FaceMovieApplication.Pages.FaceMovie" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Button ID="ButtonMovieRecommendation" runat="server" 
        onclick="ButtonMovieRecommendation_Click" Text="Recomiéndame una película!" />
    <br />
    <br />
    <br />
    <table id="TableMovie" style="width: 80%;" runat="server">
        <tr>
            <td colspan="2">
                <asp:Label ID="LabelMovieName" runat="server" Font-Bold="True" 
                    Font-Size="XX-Large" Text="MovieName"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
            <td rowspan="4">
                <asp:Image ID="ImageMovie" runat="server" Height="250px" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelMovieGenre" runat="server" Font-Bold="True" Text="Género/s"></asp:Label>
            </td>
            <td align="left">
                <asp:Label ID="LabelGenre" runat="server"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelMoviePlot" runat="server" Font-Bold="True" 
                    Text="Descripción"></asp:Label>
            </td>
            <td align="left">
                <asp:Label ID="LabelPlot" runat="server"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="ButtonMovieIMDB" runat="server" onclick="ButtonMovieIMDB_Click" 
                    Text="Ver en IMDB" />
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    <br />
    <table id="TableRanking" style="width: 80%;" runat="server">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Font-Bold="True" 
                    Text="¿Ya la viste? ¡Ranquéala!"></asp:Label>
            </td>
            <td>
                <asp:RadioButtonList ID="RadioButtonListRanking" runat="server" 
                    RepeatDirection="Horizontal">
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem Value="4"></asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td>
                <asp:Button ID="ButtonRank" runat="server" onclick="ButtonRank_Click" 
                    Text="Ranquear" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                <asp:Button ID="ButtonOK" runat="server" Text="No (ya la busco en Cuevana)" 
                    onclick="ButtonOK_Click" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    <br />
    <asp:Label ID="LabelLinkIMDB" runat="server" Text="LinkIMDB" Visible="False"></asp:Label>
    <asp:HiddenField ID="HiddenFieldMovieId" runat="server" Value="0" />
    <br />
</asp:Content>
