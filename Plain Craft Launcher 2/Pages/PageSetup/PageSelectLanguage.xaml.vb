Imports PCL.Core.App

Public Class PageSelectLanguage
    Private Sub BtnChinese_Click(sender As Object, e As MouseButtonEventArgs)
        Config.Language = "zh-CN"
        I18nService.LoadLanguage("zh-CN")
        
        ' Just close the dialog, Application.xaml.vb will handle creating the main window
        Me.Close()
    End Sub
    
    Private Sub BtnEnglish_Click(sender As Object, e As MouseButtonEventArgs)
        Config.Language = "en-US"
        I18nService.LoadLanguage("en-US")
        
        ' Just close the dialog, Application.xaml.vb will handle creating the main window
        Me.Close()
    End Sub
    
    Private Sub PageSelectLanguage_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        ' Initialize with default language to ensure UI elements can be translated
        I18nService.Initialize()
        
        ' Update UI text with translations
        TitleTextBlock.Text = I18nService.Get("LanguageSelectionTitle")
        PromptTextBlock.Text = I18nService.Get("LanguageSelectionPrompt")
        NoteTextBlock.Text = I18nService.Get("LanguageSelectionNote")
    End Sub
End Class