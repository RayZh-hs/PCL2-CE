Imports System.Windows.Markup
Imports PCL.Core.App

<MarkupExtensionReturnType(GetType(String))>
Public Class TranslateExtension
    Inherits MarkupExtension

    <ConstructorArgument("key")>
    Public Property Key As String

    Public Sub New()
    End Sub

    Public Sub New(key As String)
        Me.Key = key
    End Sub

    Public Overrides Function ProvideValue(serviceProvider As IServiceProvider) As Object
        If String.IsNullOrEmpty(Key) Then
            Return ""
        End If
        Return I18nService.Get(Key)
    End Function
End Class
