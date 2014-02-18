<%@ Control Language="vb" AutoEventWireup="false" Inherits="Connect.Modules.UserManagement.AccountUpdate.Templates" Codebehind="Templates.ascx.vb" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>

<div id="ConnectAccountUpdateTemplates" class="dnnForm dnnClear">

    <div class="dnnFormItem">
        <dnn:label id="plTheme" controlname="drpThemes" runat="server" />
        <asp:DropDownList ID="drpThemes" runat="server" AutoPostBack="true"></asp:DropDownList>
        &nbsp;
        <asp:LinkButton ID="cmdCopySelected" runat="server"></asp:LinkButton>
        &nbsp;
        <asp:LinkButton ID="cmdDeleteSelected" runat="server"></asp:LinkButton>
    </div>

    <div class="dnnFormItem" runat="server" id="pnlLocales" visible="false">
        <dnn:label id="plLocale" controlname="drpLocales" runat="server" />
        <asp:DropDownList ID="drpLocales" runat="server" AutoPostBack="true"></asp:DropDownList>
    </div>

    <div class="dnnFormItem">
        <dnn:label id="plUseTheme" controlname="txtTemplateName" runat="server" />
        <asp:CheckBox ID="chkUseTheme" runat="server" />
    </div>       

    <div class="dnnFormItem" runat="server" id="pnlTemplateName" visible="false">
        <dnn:label id="plTemplateName" controlname="txtTemplateName" runat="server" />
        <asp:TextBox ID="txtTemplateName" runat="server"></asp:TextBox>
    </div>

    <ul id="AccountUpdateTemplatesTabs" runat="Server" class="dnnAdminTabNav dnnClear">
        <li><a href="#dvFormTemplate"><asp:Label id="lblFormTemplate" runat="server" resourcekey="lblFormTemplate" /></a></li>
        <li><a href="#dvEmailUser"><asp:Label id="lblEmailUser" runat="server" resourcekey="lblEmailUser" /></a></li>
        <li><a href="#dvEmailAdmin"><asp:Label id="lblEmailAdmin" runat="server" resourcekey="lblEmailAdmin" /></a></li>
    </ul>

    <div class="dnnFormItem dnnClear" id="dvFormTemplate">
        <asp:TextBox ID="txtFormTemplate" runat="server" TextMode="MultiLine" rows="20"></asp:TextBox>
    </div>

    <div class="dnnFormItem dnnClear" id="dvEmailUser">
        <asp:TextBox ID="txtEmailUser" runat="server" TextMode="MultiLine" rows="20"></asp:TextBox>
    </div>

    <div class="dnnFormItem dnnClear" id="dvEmailAdmin">
        <asp:TextBox ID="txtEmailAdmin" runat="server" TextMode="MultiLine" rows="20"></asp:TextBox>
    </div>

    <ul class="dnnActions">
        <li><asp:LinkButton ID="cmdUpdateSettings" runat="server" CssClass="dnnPrimaryAction"></asp:LinkButton></li>
        <li><asp:LinkButton ID="cmdCancel" runat="server" CssClass="dnnSecondaryAction"></asp:LinkButton></li>
    </ul>

</div>

<script language="javascript" type="text/javascript">
/*globals jQuery, window, Sys */
(function ($, Sys) {

    function setupFormSettings() {
        $('#ConnectAccountUpdateTemplates').dnnTabs();
    }

    $(document).ready(function () {
        setupFormSettings();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            setupFormSettings();
        });
    });

} (jQuery, window.Sys));
</script>

