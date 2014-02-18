<%@ Control Language="vb" AutoEventWireup="false" Inherits="Connect.Modules.UserManagement.AccountUpdate.View" Codebehind="View.ascx.vb" %>


<div class="connect_accountform">

    <asp:Panel ID="pnlError" runat="server" CssClass="dnnFormMessage dnnFormError" Visible="false">
        <asp:Literal id="lblError" runat="server"></asp:Literal>
    </asp:Panel> 
    <asp:Panel ID="pnlSuccess" runat="server" CssClass="dnnFormMessage dnnFormSuccess" Visible="false">
        <asp:Literal id="lblSucess" runat="server"></asp:Literal>
    </asp:Panel>     
    
    <asp:PlaceHolder ID="plhProfile" runat="server"></asp:PlaceHolder>       
                     
</div>



