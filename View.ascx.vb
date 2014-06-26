Imports DotNetNuke.Entities.Modules
Imports Connect.Libraries.UserManagement
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Membership
Imports Telerik.Web.UI
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Entities.Profile

Namespace Connect.Modules.UserManagement.AccountUpdate

    Partial Class View
        Inherits ConnectUsersModuleBase

        Implements IActionable

        Private blnPageNeedsToPostback As Boolean = False

#Region "Event Handlers"

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

            'DotNetNuke.Framework.AJAX.RegisterScriptManager()

            ProcessFormTemplate(plhProfile, GetTemplate(ModuleTheme, Constants.TemplateName_Form, CurrentLocale, False), User)

            Dim btnUpdate As Button = CType(FindControlRecursive(plhProfile, plhProfile.ID & "_" & Constants.ControlId_UpdateButton), Button)
            If Not btnUpdate Is Nothing Then

                AddHandler btnUpdate.Click, AddressOf btnUpdate_Click

                If blnPageNeedsToPostback Then
                    DotNetNuke.Framework.AJAX.RegisterPostBackControl(btnUpdate)
                End If

            End If

            Dim btnDelete As Button = CType(FindControlRecursive(plhProfile, plhProfile.ID & "_" & Constants.ControlId_DeleteButton), Button)
            If Not btnDelete Is Nothing Then

                AddHandler btnDelete.Click, AddressOf btnDelete_Click

            End If

        End Sub

        Protected Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender

            ManageRegionLabel(Me.plhProfile)

        End Sub

        Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            UpdateAccount()
        End Sub

        Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            DeleteAccount()
        End Sub

#End Region

#Region "Private Methods"

        Private Sub UpdateAccount()

            pnlSuccess.Visible = False
            pnlError.Visible = False

            Dim strMessages As New List(Of String)
            Dim strUpdated As String = ""

            Dim blnUpdateUsername As Boolean = False
            Dim blnUpdateFirstname As Boolean = False
            Dim blnUpdateLastname As Boolean = False
            Dim blnUpdateDisplayname As Boolean = False
            Dim blnUpdatePassword As Boolean = False
            Dim blnUpdateEmail As Boolean = False
            Dim blnUpdatePasswordQuestionAndAnswer As Boolean = False

            Dim txtEmail As TextBox = CType(FindControlRecursive(plhProfile, plhProfile.ID & "_" & Constants.ControlId_Email), TextBox)
            blnUpdateEmail = (Not txtEmail Is Nothing)

            If blnUpdateEmail Then
                If Not IsValidUserAttribute(Constants.User_Email, plhProfile) Then
                    strMessages.Add("Error_InvalidEmail")
                    AddErrorIndicator(Constants.User_Email, plhProfile)
                Else
                    RemoveErrorIndicator(Constants.User_Email, plhProfile, True)
                End If
            End If

            Dim txtPasswordCurrent As TextBox = CType(FindControlRecursive(plhProfile, plhProfile.ID & "_" & Constants.ControlId_PasswordCurrent), TextBox)
            Dim txtPassword1 As TextBox = CType(FindControlRecursive(plhProfile, plhProfile.ID & "_" & Constants.ControlId_Password1), TextBox)
            Dim txtPassword2 As TextBox = CType(FindControlRecursive(plhProfile, plhProfile.ID & "_" & Constants.ControlId_Password2), TextBox)
            Dim txtPasswordQuestion As TextBox = CType(FindControlRecursive(plhProfile, plhProfile.ID & "_" & Constants.ControlId_PasswordQuestion), TextBox)
            Dim txtPasswordAnswer As TextBox = CType(FindControlRecursive(plhProfile, plhProfile.ID & "_" & Constants.ControlId_PasswordAnswer), TextBox)

            blnUpdatePassword = (Not txtPasswordCurrent Is Nothing AndAlso Not txtPassword1 Is Nothing AndAlso Not txtPassword2 Is Nothing)
            blnUpdatePasswordQuestionAndAnswer = (Not txtPasswordQuestion Is Nothing AndAlso Not txtPasswordAnswer Is Nothing AndAlso Not txtPasswordCurrent Is Nothing)

            If blnUpdatePassword Then
                If txtPassword1.Text = "" AndAlso txtPassword2.Text = "" Then
                    blnUpdatePassword = False
                End If
            End If

            If blnUpdatePassword Then
                If Not IsValidUserAttribute(Constants.User_PasswordCurrent, plhProfile) Then
                    strMessages.Add("Error_MissingPasswordCurrent")
                    AddErrorIndicator(Constants.User_PasswordCurrent, plhProfile)
                Else
                    RemoveErrorIndicator(Constants.User_PasswordCurrent, plhProfile, True)
                End If
                If Not IsValidUserAttribute(Constants.User_Password1, plhProfile) Then
                    strMessages.Add("Error_MissingPassword")
                    AddErrorIndicator(Constants.User_Password1, plhProfile)
                Else
                    RemoveErrorIndicator(Constants.User_Password1, plhProfile, True)
                End If
                If Not IsValidUserAttribute(Constants.User_Password2, plhProfile) Then
                    strMessages.Add("Error_MissingPassword")
                    AddErrorIndicator(Constants.User_Password2, plhProfile)
                Else
                    RemoveErrorIndicator(Constants.User_Password2, plhProfile, True)
                End If
            End If

            If blnUpdatePasswordQuestionAndAnswer Then
                If txtPasswordAnswer.Text = "" AndAlso txtPasswordQuestion.Text = "" Then
                    blnUpdatePasswordQuestionAndAnswer = False
                End If
            End If

            If blnUpdatePasswordQuestionAndAnswer Then
                If Not IsValidUserAttribute(Constants.User_Password1, plhProfile) Then
                    strMessages.Add("Error_MissingPasswordForQuestionAndAnswer")
                    AddErrorIndicator(Constants.User_Password1, plhProfile)
                Else
                    RemoveErrorIndicator(Constants.User_Password1, plhProfile, True)
                End If
                If Not IsValidUserAttribute(Constants.User_PasswordAnswer, plhProfile) Then
                    strMessages.Add("Error_MissingPasswordAnswerAndQuestion")
                    AddErrorIndicator(Constants.User_PasswordAnswer, plhProfile)
                Else
                    RemoveErrorIndicator(Constants.User_PasswordAnswer, plhProfile, True)
                End If
                If Not IsValidUserAttribute(Constants.User_PasswordQuestion, plhProfile) Then
                    strMessages.Add("Error_MissingPasswordAnswerAndQuestion")
                    AddErrorIndicator(Constants.User_PasswordQuestion, plhProfile)
                Else
                    RemoveErrorIndicator(Constants.User_PasswordQuestion, plhProfile, True)
                End If
            End If

            Dim txtFirstName As TextBox = CType(FindControlRecursive(plhProfile, plhProfile.ID & "_" & Constants.ControlId_Firstname), TextBox)
            blnUpdateFirstname = (Not txtFirstName Is Nothing)

            If blnUpdateFirstname Then
                If Not IsValidUserAttribute(Constants.User_Firstname, plhProfile) Then
                    strMessages.Add("Error_MissingFirstname")
                    AddErrorIndicator(Constants.User_Firstname, plhProfile)
                Else
                    RemoveErrorIndicator(Constants.User_Firstname, plhProfile, True)
                End If
            End If

            Dim txtLastName As TextBox = CType(FindControlRecursive(plhProfile, plhProfile.ID & "_" & Constants.ControlId_Lastname), TextBox)
            blnUpdateLastname = (Not txtLastName Is Nothing)

            If blnUpdateLastname Then
                If Not IsValidUserAttribute(Constants.User_Lastname, plhProfile) Then
                    strMessages.Add("Error_MissingLastname")
                    AddErrorIndicator(Constants.User_Lastname, plhProfile)
                Else
                    RemoveErrorIndicator(Constants.User_Lastname, plhProfile, True)
                End If
            End If

            If CompareFirstNameLastName AndAlso (blnUpdateFirstname And blnUpdateLastname) Then
                If txtLastName.Text.ToLower.Trim = txtFirstName.Text.ToLower.Trim Then
                    strMessages.Add("Error_LastnameLikeFirstname")
                    AddErrorIndicator(Constants.User_Firstname, plhProfile)
                End If
            End If

            Dim txtDisplayName As TextBox = CType(FindControlRecursive(plhProfile, plhProfile.ID & "_" & Constants.ControlId_Displayname), TextBox)
            blnUpdateDisplayname = (Not txtDisplayName Is Nothing)

            If blnUpdateDisplayname Then
                If Not IsValidUserAttribute(Constants.User_Displayname, plhProfile) Then
                    strMessages.Add("Error_MissingDisplayName")
                    AddErrorIndicator(Constants.User_Displayname, plhProfile)
                Else
                    RemoveErrorIndicator(Constants.User_Displayname, plhProfile, True)
                End If
            End If

            Dim blnProfileErrorAdded As Boolean = False
            For Each itemProp As String In GetPropertiesFromTempate(GetTemplate(ModuleTheme, Constants.TemplateName_Form, CurrentLocale, False))
                Try
                    Dim prop As ProfilePropertyDefinition = ProfileController.GetPropertyDefinitionByName(PortalId, itemProp.Substring(2)) 'itemprop comes in the form U:Propertyname or P:Propertyname
                    If Not prop Is Nothing Then
                        If Not IsValidProperty(UserInfo, prop, plhProfile) Then
                            If blnProfileErrorAdded = False Then
                                strMessages.Add("Error_MissingProfileField")
                                blnProfileErrorAdded = True
                            End If
                            AddErrorIndicator(prop.PropertyDefinitionId.ToString, plhProfile)
                        Else
                            RemoveErrorIndicator(prop.PropertyDefinitionId.ToString, plhProfile, prop.Required)
                        End If
                    End If
                Catch
                End Try
            Next



            If strMessages.Count > 0 Then
                Me.pnlError.Visible = True
                Me.lblError.Text = "<ul>"
                For Each strMessage As String In strMessages
                    lblError.Text += "<li>" & Localization.GetString(strMessage, LocalResourceFile) & "</li>"
                Next
                lblError.Text += "</ul>"
                Exit Sub
            End If

            Dim oUser As UserInfo = UserController.GetCurrentUserInfo
            Dim oldAccount As UserInfo = UserController.GetCurrentUserInfo

            If blnUpdateEmail Then

                If UsernameMode = UsernameUpdateMode.Email Then
                    Try

                        UserController.ChangeUsername(oUser.UserID, txtEmail.Text)
                    Catch ex As Exception
                        'in use already, do not update e-mail adress
                        Me.pnlError.Visible = True
                        Me.lblError.Text = "<ul><li>" & Localization.GetString("DuplicateEmail.Text", LocalResourceFile) & "</li></ul>"
                        Exit Sub
                    End Try

                End If

                oUser.Email = txtEmail.Text
                If oUser.Email <> oldAccount.Email Then
                    strUpdated += Localization.GetString(Constants.User_Email, LocalResourceFile) & ", "
                End If
            End If

            'try updating password
            If blnUpdatePassword Then
                If txtPassword1.Text = txtPassword2.Text Then
                    If UserController.ValidatePassword(txtPassword1.Text) Then

                        If Not UserController.ChangePassword(oUser, txtPasswordCurrent.Text, txtPassword1.Text) Then

                            Me.pnlError.Visible = True
                            Me.lblError.Text = "<ul><li>" & Localization.GetString("PasswordUpdateError", LocalResourceFile) & "</li></ul>"
                            Exit Sub

                        End If

                        strUpdated += Localization.GetString(Constants.User_Password1, LocalResourceFile) & ", "
                    Else

                        Dim MinLength As Integer = 0
                        Dim MinNonAlphaNumeric As Integer = 0
                        Try
                            MinLength = DotNetNuke.Security.Membership.MembershipProvider.Instance().MinPasswordLength
                        Catch
                        End Try
                        Try
                            MinNonAlphaNumeric = DotNetNuke.Security.Membership.MembershipProvider.Instance().MinNonAlphanumericCharacters
                        Catch
                        End Try

                        Dim strPolicy As String = String.Format(Localization.GetString("PasswordPolicy_MinLength", LocalResourceFile), MinLength.ToString)
                        If MinNonAlphaNumeric > 0 Then
                            strPolicy += String.Format(Localization.GetString("PasswordPolicy_MinNonAlphaNumeric", LocalResourceFile), MinNonAlphaNumeric.ToString)
                        End If

                        Me.pnlError.Visible = True
                        Me.lblError.Text = "<ul><li>" & String.Format(Localization.GetString("InvalidPassword", LocalResourceFile), strPolicy) & "</li></ul>"
                        Exit Sub

                    End If
                Else
                    Me.pnlError.Visible = True
                    Me.lblError.Text = "<ul><li>" & Localization.GetString("PasswordsDontMatch.Text", LocalResourceFile) & "</li></ul>"
                    Exit Sub
                End If
            End If

            If blnUpdatePasswordQuestionAndAnswer Then
                UserController.ChangePasswordQuestionAndAnswer(oUser, txtPasswordCurrent.Text, txtPasswordQuestion.Text, txtPasswordAnswer.Text)
                oUser.Membership.PasswordQuestion = txtPasswordQuestion.Text
                oUser.Membership.PasswordAnswer = txtPasswordAnswer.Text
            End If

            UserController.UpdateUser(PortalId, oUser)

            Dim propertiesCollection As New ProfilePropertyDefinitionCollection
            UpdateProfileProperties(plhProfile, oUser, propertiesCollection, strUpdated, GetPropertiesFromTempate(GetTemplate(ModuleTheme, Constants.TemplateName_Form, CurrentLocale, False)))
            oUser = ProfileController.UpdateUserProfile(oUser, propertiesCollection)

            'make sure first and lastname are in sync with user and profile object
            If blnUpdateFirstname = True Then
                If oldAccount.FirstName <> txtFirstName.Text Then
                    strUpdated += Localization.GetString(Constants.User_Firstname, LocalResourceFile) & ", "
                End If
                oUser.Profile.FirstName = txtFirstName.Text
                oUser.FirstName = txtFirstName.Text
            Else
                If oUser.Profile.FirstName <> "" Then
                    oUser.FirstName = oUser.Profile.FirstName
                End If
            End If

            If blnUpdateLastname = True Then
                If oldAccount.LastName <> txtLastName.Text Then
                    strUpdated += Localization.GetString(Constants.User_Lastname, LocalResourceFile) & ", "
                End If
                oUser.Profile.LastName = txtLastName.Text
                oUser.LastName = txtLastName.Text
            Else
                If oUser.Profile.LastName <> "" Then
                    oUser.LastName = oUser.Profile.LastName
                End If
            End If

            If blnUpdateDisplayname Then
                If oldAccount.DisplayName <> txtDisplayName.Text Then
                    strUpdated += Localization.GetString(Constants.User_Displayname, LocalResourceFile) & ", "
                End If
                oUser.DisplayName = txtDisplayName.Text
            Else
                Select Case DisplaynameMode
                    Case DisplaynameUpdateMode.Email
                        If blnUpdateEmail Then
                            oUser.DisplayName = txtEmail.Text.Trim
                        End If
                    Case DisplaynameUpdateMode.FirstLetterLastname
                        If blnUpdateLastname AndAlso blnUpdateFirstname Then
                            oUser.DisplayName = oUser.FirstName.Trim.Substring(0, 1) & ". " & oUser.LastName
                        End If
                    Case DisplaynameUpdateMode.FirstnameLastname
                        If blnUpdateLastname AndAlso blnUpdateFirstname Then
                            oUser.DisplayName = oUser.FirstName & " " & oUser.LastName
                        End If
                    Case DisplaynameUpdateMode.Lastname
                        If blnUpdateLastname Then
                            oUser.DisplayName = oUser.LastName
                        End If
                End Select
            End If

            'update profile
            UserController.UpdateUser(PortalId, oUser)

            'add to role
            If AddToRoleOnSubmit <> Null.NullInteger Then
                Try
                    Dim rc As New RoleController

                    If AddToRoleStatus.ToLower = "pending" Then
                        rc.AddUserRole(PortalId, oUser.UserID, AddToRoleOnSubmit, RoleStatus.Pending, False, Date.Now, Null.NullDate)
                    Else
                        rc.AddUserRole(PortalId, oUser.UserID, AddToRoleOnSubmit, RoleStatus.Approved, False, Date.Now, Null.NullDate)
                    End If
                Catch
                End Try
            End If

            'remove from role
            If RemoveFromRoleOnSubmit <> Null.NullInteger Then
                Try
                    Dim rc As New RoleController
                    Dim r As RoleInfo = rc.GetRole(RemoveFromRoleOnSubmit, PortalId)

                    RoleController.DeleteUserRole(oUser, r, PortalSettings, False)
                Catch
                End Try
            End If

            Dim chkTest As CheckBox = CType(FindMembershipControlsRecursive(plhProfile, plhProfile.ID & "_" & Constants.ControlId_RoleMembership), CheckBox)
            If Not chkTest Is Nothing Then
                'at least on role membership checkbox found. Now lookup roles that could match
                Dim rc As New RoleController
                Dim roles As ArrayList
                roles = rc.GetPortalRoles(PortalId)
                For Each objRole As RoleInfo In roles

                    Dim blnPending As Boolean = False
                    Dim chkRole As CheckBox = CType(FindControlRecursive(plhProfile, plhProfile.ID & "_" & Constants.ControlId_RoleMembership & objRole.RoleName.Replace(" ", "")), CheckBox)
                    If chkRole Is Nothing Then
                        chkRole = CType(FindControlRecursive(plhProfile, plhProfile.ID & "_" & Constants.ControlId_RoleMembership & objRole.RoleName.Replace(" ", "") & "_Pending"), CheckBox)
                        blnPending = True
                    End If

                    If Not chkRole Is Nothing Then
                        If blnPending Then
                            rc.AddUserRole(PortalId, oUser.UserID, objRole.RoleID, RoleStatus.Pending, False, Date.Now, Null.NullDate)
                        Else
                            rc.AddUserRole(PortalId, oUser.UserID, objRole.RoleID, RoleStatus.Approved, False, Date.Now, Null.NullDate)
                        End If
                    End If

                Next
            End If

            'notify admin   
            If NotifyRole <> "" AndAlso strUpdated.Length > 0 Then

                Dim strBody As String = GetTemplate(ModuleTheme, Constants.TemplateName_EmailToAdmin, CurrentLocale, False)

                strBody = strBody.Replace("[PORTALURL]", PortalSettings.PortalAlias.HTTPAlias)
                strBody = strBody.Replace("[PORTALNAME]", PortalSettings.PortalName)
                strBody = strBody.Replace("[USERID]", oUser.UserID)
                strBody = strBody.Replace("[DISPLAYNAME]", oUser.DisplayName)
                If MembershipProvider.Instance().PasswordRetrievalEnabled Then
                    strBody = strBody.Replace("[PASSWORD]", MembershipProvider.Instance().GetPassword(oUser, ""))
                End If

                strBody = strBody.Replace("[USERNAME]", oUser.Username)
                strBody = strBody.Replace("[FIRSTNAME]", oUser.FirstName)
                strBody = strBody.Replace("[LASTNAME]", oUser.LastName)
                strBody = strBody.Replace("[UPDATED]", strUpdated.Substring(0, strUpdated.LastIndexOf(",")).Replace(":", ""))
                strBody = strBody.Replace("[USERURL]", NavigateURL(UsermanagementTab, "", "uid=" & oUser.UserID.ToString, "RoleId=" & PortalSettings.RegisteredRoleId.ToString))

                strBody = strBody.Replace("[RECIPIENTUSERID]", oUser.UserID.ToString)
                strBody = strBody.Replace("[USERID]", oUser.UserID.ToString)

                Dim ctrlRoles As New RoleController
                Dim NotificationUsers As ArrayList = ctrlRoles.GetUsersByRoleName(PortalId, NotifyRole)
                For Each NotificationUser As UserInfo In NotificationUsers
                    Try

                        strBody = strBody.Replace("[RECIPIENTUSERID]", NotificationUser.UserID.ToString)
                        strBody = strBody.Replace("[USERID]", NotificationUser.UserID.ToString)

                        DotNetNuke.Services.Mail.Mail.SendMail(PortalSettings.Email, NotificationUser.Email, "", String.Format(Localization.GetString("NotifySubject_ProfileUpdate.Text", LocalResourceFile), PortalSettings.PortalName), strBody, "", "HTML", "", "", "", "")
                    Catch
                    End Try
                Next

            End If

            If NotifyUser AndAlso strUpdated.Length > 0 Then

                Dim strBody As String = GetTemplate(ModuleTheme, Constants.TemplateName_EmailToUser, CurrentLocale, False)

                strBody = strBody.Replace("[PORTALURL]", PortalSettings.PortalAlias.HTTPAlias)
                strBody = strBody.Replace("[PORTALNAME]", PortalSettings.PortalName)
                strBody = strBody.Replace("[USERID]", oUser.UserID)
                strBody = strBody.Replace("[DISPLAYNAME]", oUser.DisplayName)
                If MembershipProvider.Instance().PasswordRetrievalEnabled Then
                    strBody = strBody.Replace("[PASSWORD]", MembershipProvider.Instance().GetPassword(oUser, ""))
                End If
                strBody = strBody.Replace("[USERNAME]", oUser.Username)
                strBody = strBody.Replace("[FIRSTNAME]", oUser.FirstName)
                strBody = strBody.Replace("[LASTNAME]", oUser.LastName)
                strBody = strBody.Replace("[UPDATED]", strUpdated.Substring(0, strUpdated.LastIndexOf(",")).Replace(":", ""))
                strBody = strBody.Replace("[USERURL]", NavigateURL(TabId))

                strBody = strBody.Replace("[RECIPIENTUSERID]", oUser.UserID.ToString)
                strBody = strBody.Replace("[USERID]", oUser.UserID.ToString)

                Try
                    DotNetNuke.Services.Mail.Mail.SendMail(PortalSettings.Email, oUser.Email, "", String.Format(Localization.GetString("NotifySubject_UserDetails", LocalResourceFile), PortalSettings.PortalName), strBody, "", "HTML", "", "", "", "")
                Catch
                End Try

            End If

            lblSucess.Text = "<ul><li>" & Localization.GetString("AccountUpdateSuccess.Text", LocalResourceFile) & "</li></ul>"
            pnlSuccess.Visible = True

            If ExternalInterface <> Null.NullString Then

                Dim objInterface As Object = Nothing

                If ExternalInterface.Contains(",") Then
                    Dim strAssembly As String = ExternalInterface.Split(Char.Parse(","))(0).Trim
                    Dim strClass As String = ExternalInterface.Split(Char.Parse(","))(1).Trim
                    objInterface = System.Activator.CreateInstance(strAssembly, strClass).Unwrap
                End If

                If Not objInterface Is Nothing Then
                    CType(objInterface, Interfaces.iAccountUpdate).FinalizeAccountUpdate(Server, Response, Request, oUser)
                End If

            End If

            If Not Request.QueryString("ReturnURL") Is Nothing Then
                Response.Redirect(Server.UrlDecode(Request.QueryString("ReturnURL")), True)
            End If

            If RedirectAfterSubmit <> Null.NullInteger Then
                Response.Redirect(NavigateURL(RedirectAfterSubmit))
            End If

        End Sub

        Private Sub DeleteAccount()

            Dim oUser As UserInfo = UserController.GetCurrentUserInfo

            If UserController.DeleteUser(oUser, False, False) Then
                lblSucess.Text = "<ul><li>" & Localization.GetString("AccountDeleted.Text", LocalResourceFile) & "</li></ul>"
                pnlSuccess.Visible = True
            Else
                lblError.Text = "<ul><li>" & Localization.GetString("AccountNotDeleted.Text", LocalResourceFile) & "</li></ul>"
                pnlError.Visible = True
            End If

        End Sub

#End Region

#Region "Optional Interfaces"

        Public ReadOnly Property ModuleActions() As Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
            Get
                Dim Actions As New Entities.Modules.Actions.ModuleActionCollection
                Actions.Add(GetNextActionID, Localization.GetString("ManageTemplates.Action", LocalResourceFile), Entities.Modules.Actions.ModuleActionType.AddContent, "", "", EditUrl("ManageTemplates"), False, DotNetNuke.Security.SecurityAccessLevel.Edit, True, False)
                Return Actions
            End Get
        End Property

#End Region

    End Class

End Namespace


