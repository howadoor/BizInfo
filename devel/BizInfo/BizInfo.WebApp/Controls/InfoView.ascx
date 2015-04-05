<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoView.ascx.cs" Inherits="BizInfo.WebApp.Controls.InfoView" %>
<%@ Register Src="CustomDataPager.ascx" TagName="CustomDataPager" TagPrefix="uc2" %>
<div class="info-view">
    <div class="navigation">
        <div id="search-conditions">
            Vyhledávaná slova
            <asp:TextBox ID="SearchPhraseBox" runat="server" AutoPostBack="true" OnTextChanged="OnSearchPhraseTextBoxChanged"></asp:TextBox>
            <span class="field-description">Můžete použít spojek AND, OR, závorek, hvězdiček a uvozovek k přesnější
                specifikaci dotazu (jako na Googlu). Brn* najde "Brno", "Brně" i "Brněnský".</span>
        </div>
        <uc2:CustomDataPager ID="CustomDataPager1" runat="server" />
        <div class="clear"></div>
    </div>
    <asp:ListView ID="ListView1" runat="server" DataSourceID="SqlDataSource">
        <EmptyDataTemplate>
            Nebyly nalezeny žádné záznamy
        </EmptyDataTemplate>
        <ItemTemplate>
            <div class="item">
                <%# GetBizInfoImage(Eval("PhotoUrls") as string) %>
                <%# GetWebSourceNavigation(Eval("WebSourceId") as Int64?) %>
                <h2>
                    <%# Eval("Summary") %>
                    <span class="more">
                        <%# Eval("CreationTime") %>
                        &bull;
                        <%# GetCategoryString(Eval("NativeCategory") as int?) %>
                        <spa style="font-weight: normal; color: Gray;">
                        &bull;
                        #<%# Eval("RowNumber") %> Id<%# Eval("Id") %></span></span></h2>
                <div class="text">
                    <%# ProcessText (Eval("Text") as string) %></div>
            </div>
        </ItemTemplate>
        <LayoutTemplate>
            <div class="info-list-view">
                <div id="itemPlaceholder" runat="server" />
            </div>
        </LayoutTemplate>
    </asp:ListView>
    <asp:SqlDataSource ID="SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:BizInfoConnectionString %>" OnSelecting="OnSqlDataSourceSelecting" EnableCaching="true"
        DataSourceMode="DataSet"></asp:SqlDataSource>
</div> 