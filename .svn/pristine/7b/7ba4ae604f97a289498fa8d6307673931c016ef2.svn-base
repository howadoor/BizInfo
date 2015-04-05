<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ChangePassword.aspx.cs" Inherits="BizInfo.WebApp.Account.ChangePassword" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Change Password
    </h2>
    <p>
        Use the form below to change your password.
    </p>
    <p>
        New passwords are required to be a minimum of <%= Membership.MinRequiredPasswordLength %> characters in length.
    </p>
    <asp:ChangePassword ID="ChangeUserPassword" runat="server" CancelDestinationPageUrl="~/" EnableViewState="false" RenderOuterTable="false" 
         SuccessPageUrl="ChangePasswordSuccess.aspx">
        <ChangePasswordTemplate>
            <span class="failureNotification">
                <asp:Literal ID="FailureText" runat="server"></asp:Literal>
            </span>
            <asp:ValidationSummary ID="ChangeUserPasswordValidationSummary" runat="server" CssClass="failureNotification" 
                 ValidationGroup="ChangeUserPasswordValidationGroup"/>
            <div class="accountInfo">
                <fieldset class="changePassword">
                    <legend>Account Information</legend>
                    <p>
                        <asp:Label ID="CurrentPasswordLabel" runat="server" AssociatedControlID="CurrentPassword">Staré heslo:</asp:Label>
                        <asp:TextBox ID="CurrentPassword" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="CurrentPassword" 
                             CssClass="failureNotification" ErrorMessage="Heslo je povinné." ToolTip="Staré heslo je povinné." 
                             ValidationGroup="ChangeUserPasswordValidationGroup">*</asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:Label ID="NewPasswordLabel" runat="server" AssociatedControlID="NewPassword">Nové heslo:</asp:Label>
                        <asp:TextBox ID="NewPassword" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="NewPassword" 
                             CssClass="failureNotification" ErrorMessage="Nové heslo je povinné." ToolTip="Nové heslo je povinné." 
                             ValidationGroup="ChangeUserPasswordValidationGroup">*</asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:Label ID="ConfirmNewPasswordLabel" runat="server" AssociatedControlID="ConfirmNewPassword">Confirm New Password:</asp:Label>
                        <asp:TextBox ID="ConfirmNewPassword" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword" 
                             CssClass="failureNotification" Display="Dynamic" ErrorMessage="Zopakování nového hesla je povinné."
                             ToolTip="Zopakování nového hesla je povinné." ValidationGroup="ChangeUserPasswordValidationGroup">*</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword" 
                             CssClass="failureNotification" Display="Dynamic" ErrorMessage="Opakování nového hesla musí být stejné jako nové heslo."
                             ValidationGroup="ChangeUserPasswordValidationGroup">*</asp:CompareValidator>
                    </p>
                </fieldset>
                <p class="submitButton">
                    <asp:Button ID="CancelPushButton" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel"/>
                    <asp:Button ID="ChangePasswordPushButton" runat="server" CommandName="ChangePassword" Text="Change Password" 
                         ValidationGroup="ChangeUserPasswordValidationGroup"/>
                </p>
            </div>
        </ChangePasswordTemplate>
    </asp:ChangePassword>
</asp:Content>
