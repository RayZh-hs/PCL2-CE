Public Class PageDownloadClient

    Private Sub LoaderInit() Handles Me.Initialized
        PageLoaderInit(Load, PanLoad, PanBack, Nothing, DlClientListLoader, AddressOf Load_OnFinish)
    End Sub
    Private Sub Init() Handles Me.Loaded
        PanBack.ScrollToHome()
    End Sub

    Private Sub Load_OnFinish()
        '结果数据化
        Try
            '归类
            Dim Dict As New Dictionary(Of String, List(Of JObject)) From {
                {"Release", New List(Of JObject)},
                {"Snapshot", New List(Of JObject)},
                {"Old", New List(Of JObject)},
                {"Special", New List(Of JObject)}
            }
            Dim Versions As JArray = DlClientListLoader.Output.Value("versions")
            For Each Version As JObject In Versions
                '确定分类
                Dim Type As String = Version("type")
                Select Case Type
                    Case "release"
                        Type = "Release"
                    Case "snapshot", "pending"
                        Type = "Snapshot"
                        'Mojang 误分类
                        If Version("id").ToString.StartsWith("1.") AndAlso
                            Not Version("id").ToString.ToLower.Contains("combat") AndAlso
                            Not Version("id").ToString.ToLower.Contains("rc") AndAlso
                            Not Version("id").ToString.ToLower.Contains("experimental") AndAlso
                            Not Version("id").ToString.ToLower.Equals("1.2") AndAlso
                            Not Version("id").ToString.ToLower.Contains("pre") Then
                            Type = "Release"
                            Version("type") = "release"
                        End If
                        '愚人节版本
                        Select Case Version("id").ToString.ToLower
                            Case "2point0_blue", "2point0_red", "2point0_purple", "2.0_blue", "2.0_red", "2.0_purple", "2.0"
                                Type = "Special"
                                Version("id") = Version("id").ToString().Replace("point", ".")
                                Version("type") = "special"
                                Version.Add("lore", GetMcFoolName(Version("id")))
                            Case "20w14infinite", "20w14∞"
                                Type = "Special"
                                Version("id") = "20w14∞"
                                Version("type") = "special"
                                Version.Add("lore", GetMcFoolName(Version("id")))
                            Case "3d shareware v1.34", "1.rv-pre1", "15w14a", "2.0", "22w13oneblockatatime", "23w13a_or_b", "24w14potato", "25w14craftmine"
                                Type = "Special"
                                Version("type") = "special"
                                Version.Add("lore", GetMcFoolName(Version("id")))
                            Case Else '4/1 自动视作愚人节版
                                Dim ReleaseDate = Version("releaseTime").Value(Of Date).ToUniversalTime().AddHours(2)
                                If ReleaseDate.Month = 4 AndAlso ReleaseDate.Day = 1 Then
                                    Type = "Special"
                                    Version("type") = "special"
                                End If
                        End Select
                    Case "special"
                        '已被处理的愚人节版
                        Type = "Special"
                    Case Else
                        Type = "Old"
                End Select
                '加入辞典
                Dict(Type).Add(Version)
            Next
            '排序
            For i = 0 To Dict.Keys.Count - 1
                Dict(Dict.Keys(i)) = Dict.Values(i).OrderByDescending(Function(v) v("releaseTime").Value(Of Date)).ToList
            Next
            '清空当前
            PanMain.Children.Clear()
            '添加最新版本
            Dim CardInfo As New MyCard With {.Title = PCL.Core.App.I18nService.Get("Download.VersionList.Latest"), .Margin = New Thickness(0, 0, 0, 15)}
            Dim TopestVersions As New List(Of JObject)
            Dim Release As JObject = Dict("Release")(0).DeepClone()
            Release("lore") = String.Format(PCL.Core.App.I18nService.Get("Download.VersionList.LatestRelease"), Release("releaseTime").Value(Of Date).ToString("yyyy'/'MM'/'dd HH':'mm"))
            TopestVersions.Add(Release)
            If Dict("Release")(0)("releaseTime").Value(Of Date) < Dict("Snapshot")(0)("releaseTime").Value(Of Date) Then
                Dim Snapshot As JObject = Dict("Snapshot")(0).DeepClone()
                Snapshot("lore") = String.Format(PCL.Core.App.I18nService.Get("Download.VersionList.LatestSnapshot"), Snapshot("releaseTime").Value(Of Date).ToString("yyyy'/'MM'/'dd HH':'mm"))
                TopestVersions.Add(Snapshot)
            End If
            Dim PanInfo As New StackPanel With {.Margin = New Thickness(20, MyCard.SwapedHeight, 18, 0), .VerticalAlignment = VerticalAlignment.Top, .RenderTransform = New TranslateTransform(0, 0), .Tag = TopestVersions}
            Dim PutMethod = Sub(Stack As StackPanel)
                                For Each item In Stack.Tag
                                    Stack.Children.Add(McDownloadListItem(item, AddressOf McDownloadMenuSave, True))
                                Next
                            End Sub
            MyCard.StackInstall(PanInfo, PutMethod)
            CardInfo.Children.Add(PanInfo)
            PanMain.Children.Add(CardInfo)
            '添加其他版本
            For Each Pair As KeyValuePair(Of String, List(Of JObject)) In Dict
                If Not Pair.Value.Any() Then Continue For
                '增加卡片
                Dim NewCard As New MyCard With {.Title = PCL.Core.App.I18nService.Get("Download.VersionList.Type." & Pair.Key) & " (" & Pair.Value.Count & ")", .Margin = New Thickness(0, 0, 0, 15)}
                Dim NewStack As New StackPanel With {.Margin = New Thickness(20, MyCard.SwapedHeight, 18, 0), .VerticalAlignment = VerticalAlignment.Top, .RenderTransform = New TranslateTransform(0, 0), .Tag = Pair.Value}
                NewCard.Children.Add(NewStack)
                NewCard.SwapControl = NewStack
                NewCard.InstallMethod = PutMethod
                NewCard.IsSwapped = True
                PanMain.Children.Add(NewCard)
            Next
        Catch ex As Exception
            Log(ex, "Error displaying MC version list", LogLevel.Feedback)
        End Try
    End Sub

    Public Sub DownloadStart(sender As MyListItem, e As Object)
        McDownloadClient(NetPreDownloadBehaviour.HintWhileExists, sender.Title, sender.Tag("url").ToString)
    End Sub

    ''介绍栏
    'Private Sub BtnWeb_Click(sender As Object, e As EventArgs) Handles BtnWeb.Click
    '    OpenWebsite("https://www.minecraft.net/zh-hans")
    'End Sub
    'Private Sub BtnInstall_Click(sender As Object, e As EventArgs) Handles BtnInstall.Click
    '    FrmMain.PageChange(FormMain.PageType.Download, FormMain.PageSubType.DownloadInstall)
    'End Sub

End Class
