<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="BizInfo.WebApp.Experiment.DynamicData.PageTemplates.WebForm1" %>
<%@ Register TagPrefix="asp" TagName="GridViewPager" Src="~/DynamicData/Content/GridViewPager.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:DynamicDataManager ID="DynamicDataManager1" runat="server" 
        AutoLoadForeignKeys="True">
        <DataControls>
            <asp:DataControlReference ControlID="ListView1" />
        </DataControls>
    </asp:DynamicDataManager>

    <h2 class="DDSubHeader"><!--<%= table.DisplayName%>--></h2>

<div class="navigator">
            <asp:TextBox ID="SearchPhraseBox" runat="server"></asp:TextBox>
            <asp:Button ID="SearchButton"
                runat="server" Text="Hledej" onclick="SearchButton_Click" />

<div class="pager">
<asp:DataPager ID="DataPager1" runat="server" 
   PagedControlID="ListView1">
   <Fields>
      <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" 
           ShowNextPageButton="False" ShowPreviousPageButton="False" />
       <asp:NumericPagerField />
       <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="True" 
           ShowNextPageButton="False" ShowPreviousPageButton="False" />
   </Fields>
</asp:DataPager> 
</div>
</div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="DD">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
                    HeaderText="List of validation errors" CssClass="DDValidator" />
                <asp:DynamicValidator runat="server" ID="GridViewValidator" ControlToValidate="ListView1" Display="None" CssClass="DDValidator" />
                <!--
                <asp:QueryableFilterRepeater runat="server" ID="FilterRepeater">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("DisplayName") %>' OnPreRender="Label_PreRender" />
                        <asp:DynamicFilter runat="server" ID="DynamicFilter" OnFilterChanged="DynamicFilter_FilterChanged" /><br />
                    </ItemTemplate>
                </asp:QueryableFilterRepeater>
                -->
                <asp:ListView ID="ListView1" runat="server" DataSourceID="GridDataSource">
        <ItemTemplate>
          <div class="item" runat="server">
              <!--<asp:Label ID="FirstNameLabel" runat="Server" Text='<%#Eval("Id") %>' />-->
              <a href="<%# Eval("WebSourceId")  %>"><h3><asp:Label ID="Label1" runat="Server" Text='<%#Eval("Summary") %>' /></h3></a>
              <asp:Label ID="Label2" runat="Server" Text='<%#Eval("Text") %>' />
          </div>
        </ItemTemplate>
                        </asp:ListView>
                <br />
            </div>

            <asp:EntityDataSource ID="GridDataSource" runat="server" 
                ConnectionString="" DefaultContainerName="" 
                EntitySetName="" EntityTypeFilter="" Select="" Where="" AutoPage="True" 
                AutoSort="False" >
            </asp:EntityDataSource>
            <asp:QueryExtender TargetControlID="GridDataSource" ID="GridQueryExtender" runat="server">
<asp:SearchExpression SearchType="Contains" DataFields="Summary, Text">
  <asp:ControlParameter ControlID="SearchPhraseBox" />
</asp:SearchExpression>
            </asp:QueryExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

