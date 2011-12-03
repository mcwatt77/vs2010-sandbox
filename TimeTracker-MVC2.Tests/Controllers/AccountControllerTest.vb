Imports System
Imports System.Security
Imports System.Security.Principal
Imports System.Web
Imports System.Web.Mvc
Imports System.Web.Routing
Imports System.Web.Security
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports TimeTracker_MVC2

<TestClass()> _
Public Class AccountControllerTest

    <TestMethod()> _
    Public Sub ChangePassword_Get_ReturnsView()
        ' Arrange
        Dim controller As AccountController = GetAccountController()

        ' Act
        Dim result As ActionResult = controller.ChangePassword()

        ' Assert
        Assert.IsInstanceOfType(result, GetType(ViewResult))
        Assert.AreEqual(10, CType(result, ViewResult).ViewData("PasswordLength"))
    End Sub

    <TestMethod()> _
    Public Sub ChangePassword_Post_ReturnsRedirectOnSuccess()
        ' Arrange
        Dim controller As AccountController = GetAccountController()
        Dim model As ChangePasswordModel = New ChangePasswordModel()
        model.OldPassword = "goodOldPassword"
        model.NewPassword = "goodNewPassword"
        model.ConfirmPassword = "goodNewPassword"

        ' Act
        Dim result As ActionResult = controller.ChangePassword(model)

        ' Assert
        Assert.IsInstanceOfType(result, GetType(RedirectToRouteResult))
        Dim redirectResult As RedirectToRouteResult = CType(result, RedirectToRouteResult)
        Assert.AreEqual("ChangePasswordSuccess", redirectResult.RouteValues("action"))
    End Sub

    <TestMethod()> _
    Public Sub ChangePassword_Post_ReturnsViewIfChangePasswordFails()
        ' Arrange
        Dim controller As AccountController = GetAccountController()
        Dim model As ChangePasswordModel = New ChangePasswordModel()
        model.OldPassword = "goodOldPassword"
        model.NewPassword = "badNewPassword"
        model.ConfirmPassword = "badNewPassword"

        ' Act
        Dim result As ActionResult = controller.ChangePassword(model)

        ' Assert
        Assert.IsInstanceOfType(result, GetType(ViewResult))
        Dim viewResult As ViewResult = CType(result, ViewResult)
        Assert.AreEqual(model, viewResult.ViewData.Model)
        Assert.AreEqual("The current password is incorrect or the new password is invalid.", controller.ModelState("").Errors(0).ErrorMessage)
        Assert.AreEqual(10, viewResult.ViewData("PasswordLength"))
    End Sub

    <TestMethod()> _
    Public Sub ChangePassword_Post_ReturnsViewIfModelStateIsInvalid()
        ' Arrange
        Dim controller As AccountController = GetAccountController()
        Dim model As ChangePasswordModel = New Changepasswordmodel()
        model.OldPassword = "goodOldPassword"
        model.NewPassword = "goodNewPassword"
        model.ConfirmPassword = "goodNewPassword"
        controller.ModelState.AddModelError("", "Dummy error message.")

        ' Act
        Dim result As ActionResult = controller.ChangePassword(model)

        ' Assert
        Assert.IsInstanceOfType(result, GetType(ViewResult))
        Dim viewResult As ViewResult = CType(result, ViewResult)
        Assert.AreEqual(model, viewResult.ViewData.Model)
        Assert.AreEqual(10, viewResult.ViewData("PasswordLength"))
    End Sub

    <TestMethod()> _
    Public Sub ChangePasswordSuccess_ReturnsView()
        ' Arrange
        Dim controller As AccountController = GetAccountController()

        ' Act
        Dim result As ActionResult = controller.ChangePasswordSuccess()

        ' Assert
        Assert.IsInstanceOfType(result, GetType(ViewResult))
    End Sub

    <TestMethod()> _
    Public Sub LogOff_LogsOutAndRedirects()
        ' Arrange
        Dim controller As AccountController = GetAccountController()

        ' Act
        Dim result As ActionResult = controller.LogOff()

        ' Assert
        Assert.IsInstanceOfType(result, GetType(RedirectToRouteResult))
        Dim redirectResult As RedirectToRouteResult = CType(result, RedirectToRouteResult)
        Assert.AreEqual("Home", redirectResult.RouteValues("controller"))
        Assert.AreEqual("Index", redirectResult.RouteValues("action"))
        Assert.IsTrue(CType(controller.FormsService, MockFormsAuthenticationService).SignOut_WasCalled)
    End Sub

    <TestMethod()> _
    Public Sub LogOn_Get_ReturnsView()
        ' Arrange
        Dim controller As AccountController = GetAccountController()

        ' Act
        Dim result As ActionResult = controller.LogOn()

        ' Assert
        Assert.IsInstanceOfType(result, GetType(ViewResult))
    End Sub

    <TestMethod()> _
    Public Sub LogOn_Post_ReturnsRedirectOnSuccess_WithoutReturnUrl()
        ' Arrange
        Dim controller As AccountController = GetAccountController()
        Dim model As Logonmodel = New logonmodel()
        model.UserName = "someUser"
        model.Password = "goodPassword"
        model.RememberMe = False

        ' Act
        Dim result As ActionResult = controller.LogOn(model, Nothing)

        ' Assert
        Assert.IsInstanceOfType(result, GetType(RedirectToRouteResult))
        Dim redirectResult As RedirectToRouteResult = CType(result, redirecttorouteresult)
        Assert.AreEqual("Home", redirectResult.RouteValues("controller"))
        Assert.AreEqual("Index", redirectResult.RouteValues("action"))
        Assert.IsTrue(CType(controller.formsservice, MockFormsAuthenticationService).SignIn_WasCalled)
    End Sub

    <TestMethod()> _
    Public Sub LogOn_Post_ReturnsRedirectOnSuccess_WithReturnUrl()
        ' Arrange
        Dim controller As AccountController = GetAccountController()
        Dim model As Logonmodel = New logonmodel()
        model.UserName = "someUser"
        model.Password = "goodPassword"
        model.RememberMe = False

        ' Act
        Dim result As ActionResult = controller.LogOn(model, "/someUrl")

        ' Assert
        Assert.IsInstanceOfType(result, GetType(RedirectResult))
        Dim redirectResult As RedirectResult = CType(result, RedirectResult)
        Assert.AreEqual("/someUrl", redirectResult.Url)
        Assert.IsTrue(CType(controller.FormsService, MockFormsAuthenticationService).SignIn_WasCalled)
    End Sub


    <TestMethod()> _
    Public Sub LogOn_Post_ReturnsViewIfModelStateIsInvalid()
        ' Arrange
        Dim controller As AccountController = GetAccountController()
        Dim model As Logonmodel = New logonmodel()
        model.UserName = "someUser"
        model.Password = "goodPassword"
        model.RememberMe = False
        controller.ModelState.AddModelError("", "Dummy error message.")

        ' Act
        Dim result As ActionResult = controller.LogOn(model, Nothing)

        ' Assert
        Assert.IsInstanceOfType(result, GetType(ViewResult))
        Dim viewResult As viewresult = CType(result, viewresult)
        Assert.AreEqual(model, viewResult.ViewData.Model)
    End Sub

    <TestMethod()> _
    Public Sub LogOn_Post_ReturnsViewIfValidateUserFails()
        ' Arrange
        Dim controller As AccountController = GetAccountController()
        Dim model As Logonmodel = New logonmodel()
        model.UserName = "someUser"
        model.Password = "badPassword"
        model.RememberMe = False

        ' Act
        Dim result As ActionResult = controller.LogOn(model, Nothing)

        ' Assert
        Assert.IsInstanceOfType(result, GetType(ViewResult))
        Dim viewResult As viewresult = CType(result, viewresult)
        Assert.AreEqual(model, viewResult.ViewData.Model)
        Assert.AreEqual("The user name or password provided is incorrect.", controller.ModelState("").Errors(0).ErrorMessage)
    End Sub

    <TestMethod()> _
    Public Sub Register_Get_ReturnsView()
        ' Arrange
        Dim controller As AccountController = GetAccountController()

        ' Act
        Dim result As ActionResult = controller.Register()

        ' Assert
        Assert.IsInstanceOfType(result, GetType(ViewResult))
        Assert.AreEqual(10, CType(result, ViewResult).ViewData("PasswordLength"))
    End Sub

    <TestMethod()> _
    Public Sub Register_Post_ReturnsRedirectOnSuccess()
        ' Arrange
        Dim controller As AccountController = GetAccountController()
        Dim model As registermodel = New registermodel()
        model.UserName = "someUser"
        model.Email = "goodEmail"
        model.Password = "goodPassword"
        model.ConfirmPassword = "goodPassword"

        ' Act
        Dim result As ActionResult = controller.Register(model)

        ' Assert
        Assert.IsInstanceOfType(result, GetType(RedirectToRouteResult))
        Dim redirectResult As RedirectToRouteResult = CType(result, redirecttorouteresult)
        Assert.AreEqual("Home", redirectResult.RouteValues("controller"))
        Assert.AreEqual("Index", redirectResult.RouteValues("action"))
    End Sub

    <TestMethod()> _
    Public Sub Register_Post_ReturnsViewIfRegistrationFails()
        ' Arrange
        Dim controller As AccountController = GetAccountController()
        Dim model As registermodel = New registermodel()
        model.UserName = "duplicateUser"
        model.Email = "goodEmail"
        model.Password = "goodPassword"
        model.ConfirmPassword = "goodPassword"

        ' Act
        Dim result As ActionResult = controller.Register(model)

        ' Assert
        Assert.IsInstanceOfType(result, GetType(ViewResult))
        Dim viewResult As viewresult = CType(result, viewresult)
        Assert.AreEqual(model, viewResult.ViewData.Model)
        Assert.AreEqual("Username already exists. Please enter a different user name.", controller.ModelState("").Errors(0).ErrorMessage)
        Assert.AreEqual(10, viewResult.ViewData("PasswordLength"))
    End Sub

    <TestMethod()> _
    Public Sub Register_Post_ReturnsViewIfModelStateIsInvalid()
        ' Arrange
        Dim controller As AccountController = GetAccountController()
        Dim model As registermodel = New registermodel()
        model.UserName = "someUser"
        model.Email = "goodEmail"
        model.Password = "goodPassword"
        model.ConfirmPassword = "goodPassword"
        controller.ModelState.AddModelError("", "Dummy error message.")

        ' Act
        Dim result As ActionResult = controller.Register(model)

        ' Assert
        Assert.IsInstanceOfType(result, GetType(ViewResult))
        Dim viewResult As viewresult = CType(result, viewresult)
        Assert.AreEqual(model, viewResult.ViewData.Model)
        Assert.AreEqual(10, viewResult.ViewData("PasswordLength"))
    End Sub

    Private Shared Function GetAccountController() As AccountController
        Dim controller As AccountController = New AccountController()
        controller.FormsService = New MockFormsAuthenticationService()
        controller.MembershipService = New MockMembershipService()
        controller.ControllerContext = New ControllerContext()
        controller.ControllerContext.Controller = controller
        controller.ControllerContext.RequestContext = New RequestContext(New MockHttpContext(), New RouteData())

        Return controller
    End Function

    Private Class MockFormsAuthenticationService
        Implements IFormsAuthenticationService

        Public SignIn_WasCalled As Boolean
        Public SignOut_WasCalled As Boolean

        Public Sub SignIn(ByVal userName As String, ByVal createPersistentCookie As Boolean) Implements IFormsAuthenticationService.SignIn
            ' verify that the arguments are what we expected
            Assert.AreEqual("someUser", userName)
            Assert.IsFalse(createPersistentCookie)

            SignIn_WasCalled = True
        End Sub

        Public Sub SignOut() Implements IFormsAuthenticationService.SignOut
            SignOut_WasCalled = True
        End Sub
    End Class

    Private Class MockHttpContext
        Inherits HttpContextBase

        Private ReadOnly _user As IPrincipal = New GenericPrincipal(New GenericIdentity("someUser"), Nothing)

        Public Overrides Property User() As IPrincipal
            Get
                Return _user
            End Get
            Set(ByVal value As System.Security.Principal.IPrincipal)
                MyBase.User = value
            End Set
        End Property
    End Class

    Private Class MockMembershipService
        Implements IMembershipService

        Public ReadOnly Property MinPasswordLength() As Integer Implements IMembershipService.MinPasswordLength
            Get
                Return 10
            End Get
        End Property

        Public Function ChangePassword(ByVal userName As String, ByVal oldPassword As String, ByVal newPassword As String) As Boolean Implements IMembershipService.ChangePassword
            Return userName = "someUser" AndAlso oldPassword = "goodOldPassword" AndAlso newPassword = "goodNewPassword"
        End Function

        Public Function CreateUser(ByVal userName As String, ByVal password As String, ByVal email As String) As System.Web.Security.MembershipCreateStatus Implements IMembershipService.CreateUser
            If userName = "duplicateUser" Then
                Return MembershipCreateStatus.DuplicateUserName
            End If

            ' verify that values are what we expected
            Assert.AreEqual("goodPassword", password)
            Assert.AreEqual("goodEmail", email)

            Return MembershipCreateStatus.Success
        End Function

        Public Function ValidateUser(ByVal userName As String, ByVal password As String) As Boolean Implements IMembershipService.ValidateUser
            Return userName = "someUser" AndAlso password = "goodPassword"
        End Function
    End Class

End Class


