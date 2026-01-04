# I18n Manual Inspection List

The following strings were identified as potentially meme-based or informal and have been translated with a focus on functionality. They should be manually inspected to ensure the tone is appropriate.

## PageToolsTest.xaml.vb

1.  **Original:** "今日人品 - {currentDate}"
    **Translation:** "Today's Luck - {currentDate}"
    **Key:** `ToolsTest_TodaysLuck`
    **Context:** Title of the message box showing the daily luck value.

2.  **Original:** "你今天的人品值是：{luckValue}！{rating}"
    **Translation:** "Your luck value today is: {luckValue}! {rating}"
    **Key:** `ToolsTest_LuckValue`
    **Context:** Message body showing the daily luck value.

3.  **Original:** "虽然应该没人往这些地方放重要文件，但还是问一下，是否确认继续？"
    **Translation:** "Are you sure you want to continue? This will delete files."
    **Key:** `ToolsTest_CleanConfirmationDetail`
    **Context:** Confirmation dialog before cleaning cache files. The original text is conversational and assumes the user hasn't put important files there. The translation is more formal and direct.

4.  **Original:** "请在所有下载任务完成后再来清理吧……"
    **Translation:** "Please clean up after all download tasks are completed..."
    **Key:** `ToolsTest_CleanAfterDownload`
    **Context:** Warning when trying to clean cache while downloads are active. The original uses "……" which is slightly informal.

## PageToolsHelp.xaml.vb

1.  **Original:** "指南"
    **Translation:** Skipped (Potential Data Key)
    **Context:** The string "指南" is used in logic to reorder help categories (`Types.Contains("指南")`). It is also used as the display title. If the data source provides "指南" as a category name, translating this string in the code might break the reordering logic or fail to match the data. If the data source is not localized, this string should remain as is for logic, but the display title might need a separate translation map.
