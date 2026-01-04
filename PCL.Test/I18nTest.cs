using Microsoft.VisualStudio.TestTools.UnitTesting;
using PCL.Core.App;
using System.IO;
using System;

namespace PCL.Test
{
    [TestClass]
    public class I18nTest
    {
        [TestMethod]
        public void TestEnglishLoading()
        {
            // Ensure Languages directory exists in test output
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string langDir = Path.Combine(baseDir, "Languages");
            Assert.IsTrue(Directory.Exists(langDir), $"Languages directory not found at {langDir}");
            Assert.IsTrue(File.Exists(Path.Combine(langDir, "en-US.json")), "en-US.json not found");

            // Initialize with en-US
            I18nService.LoadLanguage("en-US");

            // Check a key
            string value = I18nService.Get("Language");
            Assert.AreEqual("Language", value, "Failed to load English translation for 'Language'");
        }

        [TestMethod]
        public void TestChineseLoading()
        {
             // Initialize with zh-CN
            I18nService.LoadLanguage("zh-CN");

            // Check a key
            string value = I18nService.Get("Language");
            Assert.AreEqual("语言", value, "Failed to load Chinese translation for 'Language'");
        }
        
        [TestMethod]
        public void TestFallback()
        {
            // Load non-existent language
            I18nService.LoadLanguage("xx-XX");
            
            // Should fallback to zh-CN
            string value = I18nService.Get("Language");
            Assert.AreEqual("语言", value, "Failed to fallback to Chinese");
        }
    }
}
