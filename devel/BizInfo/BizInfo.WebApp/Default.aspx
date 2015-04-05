<%@ Page Title="BizInfo" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="BizInfo.WebApp._Default" %>

<%@ Register src="Controls/InfoView.ascx" tagname="InfoView" tagprefix="uc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <uc1:InfoView ID="InfoView1" runat="server" />
</asp:Content>
