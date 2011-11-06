<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestPage.aspx.cs" Inherits="FaceMovieApplication.Management.TestPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Button ID="Login" runat="server" onclick="Login_Click" Text="Login" />
    
        <br />
    
    </div>
    <asp:Label ID="Movie1" runat="server" Text="Movie 1"></asp:Label>
    <asp:TextBox ID="TextBoxMovie1" runat="server"></asp:TextBox>
    <asp:Label ID="Movie2" runat="server" Text="Movie 2"></asp:Label>
    <asp:TextBox ID="TextBoxMovie2" runat="server"></asp:TextBox>
    <asp:Button ID="Similarity" runat="server" onclick="Similarity_Click" 
        Text="Similarity" />
    <br />
    <br />
    <asp:Button ID="UpdateSimilarities" runat="server" 
        onclick="UpdateSimilarities_Click" Text="UpdateSimilarities" />
    <br />
    <br />
    <asp:Label ID="LabelOut" runat="server"></asp:Label>
    </form>
</body>
</html>
