<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestPage.aspx.cs" Inherits="FaceMovieApplication.Management.TestPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Label ID="LabelFacebook" runat="server" Font-Bold="True" Text="Facebook"></asp:Label>
        <br />
        <br />
    
        <asp:Button ID="Login" runat="server" onclick="Login_Click" Text="Login" />
    
        <br />
        <br />
        <asp:Label ID="LabelUpdateSection" runat="server" Font-Bold="True" 
            Text="Manual Update"></asp:Label>
        <br />
        <br />
    <asp:Button ID="UpdateSimilarities" runat="server" 
        onclick="UpdateSimilarities_Click" Text="Update All Similarities" />
        <asp:Button ID="ButtonUpdateMoviesInformation" runat="server" 
            onclick="ButtonUpdateMoviesInformation_Click" 
            Text="Update Movies Information" />
    
    </div>
    <br />
    <asp:Label ID="LabelTest" runat="server" Font-Bold="True" Text="Test"></asp:Label>
    <br />
    <br />
    <asp:Label ID="Movie1" runat="server" Text="Movie 1"></asp:Label>
    <asp:TextBox ID="TextBoxMovie1" runat="server"></asp:TextBox>
    <asp:Label ID="Movie2" runat="server" Text="Movie 2"></asp:Label>
    <asp:TextBox ID="TextBoxMovie2" runat="server"></asp:TextBox>
    <asp:Button ID="Similarity" runat="server" onclick="Similarity_Click" 
        Text="Similarity" />
    <asp:Button ID="SaveSimilarity" runat="server" onclick="SaveSimilarity_Click" 
        Text="Save Similarity" />
    <br />
    <br />
    <asp:Button ID="ButtonRandomize" runat="server" onclick="ButtonRandomize_Click" 
        Text="RandomizeUsersRatings" />
    <br />
    <br />
    <asp:Label ID="Movie" runat="server" Text="Movie"></asp:Label>
    <asp:TextBox ID="TextBoxMovieName" runat="server"></asp:TextBox>
    <asp:Button ID="ButtonInformation" runat="server" 
        onclick="ButtonInformation_Click" Text="Get IMDB Information" />
    <br />
    <br />
    <asp:Label ID="LabelImagen" runat="server" Text="Imagen:"></asp:Label>
&nbsp;&nbsp;
    <asp:Image ID="ImageMovie" runat="server" ImageAlign="Top" />
    <br />
    <br />
    <asp:Label ID="LabelOut" runat="server"></asp:Label>
    <br />
    <br />
    </form>
</body>
</html>
