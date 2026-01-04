Imports PCL.Core.App

Public Class PageOtherHelpDetail
    Implements IRefreshable
    Public Entry As HelpEntry

    Public Sub Refresh() Implements IRefreshable.Refresh
        Init(New HelpEntry(Entry.RawPath))
    End Sub

    Private Sub PageOtherHelpDetail_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        PanBack.ScrollToTop()
    End Sub

    ''' <summary>
    ''' 根据特定帮助项初始化页面 UI，返回是否成功加载。
    ''' </summary>
    Public Function Init(Entry As HelpEntry) As Boolean
        Dim Content = If(Entry.XamlContent, "")
        If Content = "" Then Throw New Exception(I18nService.Get("ToolsHelpDetail_XamlEmpty"))
        Try
            '修改时应同时修改 PageLaunchRight.LoadContent
            Content = HelpArgumentReplace(Content)
            If Content.Contains("xmlns") Then Content = Content.RegexReplace("xmlns[^""']*(""|')[^""']*(""|')", "").Replace("xmlns", "") '禁止声明命名空间
            Content = "<StackPanel xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:sys=""clr-namespace:System;assembly=System.Runtime"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" xmlns:local=""clr-namespace:PCL;assembly=Plain Craft Launcher 2"">" & Content & "</StackPanel>"
            Me.Entry = Entry
            PanCustom.Children.Clear()
            PanCustom.Children.Add(GetObjectFromXML(Content))
            Return True
        Catch ex As Exception
            Log(I18nService.Get("ToolsHelpDetail_CustomInfo") & vbCrLf & Content)
            Log(ex, I18nService.Get("ToolsHelpDetail_LoadXamlFailed"), LogLevel.Msgbox)
            Return False
        End Try
    End Function

End Class
