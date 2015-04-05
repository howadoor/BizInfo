<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomDataPager.ascx.cs"
    Inherits="BizInfo.WebApp.MVC3.Controls.CustomDataPager" %>
<div class="pager">
    <asp:Label runat="server" ID="lblTotalRecord">0 z 0 záznamů</asp:Label>
    <asp:Button runat="server" ToolTip="Přejít na první stránku" ID="btnMoveFirst" CssClass="PagingFirst"
        CommandArgument="First" OnCommand="btnMove_Click" />
    <asp:Button runat="server" ToolTip="Přejít na předcházející stránku" ID="btnMovePrevious" CssClass="PagingPrevious"
        CommandArgument="Previous" OnCommand="btnMove_Click" />
    <asp:TextBox runat="server" AutoPostBack="true" CssClass="CurrentPage" ID="txtPage"
        OnTextChanged="txtPage_TextChanged" Width="50"></asp:TextBox>
    <asp:Label runat="server" ID="lblTotalPage">0 z 0 záznamů</asp:Label>
    <asp:Button runat="server" ToolTip="Přejít na následující stránku" ID="btnMoveNext" CssClass="PagingNext"
        CommandArgument="Next" OnCommand="btnMove_Click" />
    <asp:Button runat="server" ToolTip="Přejít na poslední stránku" ID="btnMoveLast" CssClass="PagingLast"
        CommandArgument="Last" OnCommand="btnMove_Click" />
    Záznamů na stránku
    <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlRecords" OnSelectedIndexChanged="ddlRecords_SelectedIndexChanged" />
</div>
