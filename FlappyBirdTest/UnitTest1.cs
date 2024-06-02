using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace FlappyBirdTests
{
    public class FlappyBirdGameTests
    {
        private IWebDriver _driver;
        private IJavaScriptExecutor _js;

        [SetUp]
        public void Setup()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--disable-infobars");
            chromeOptions.AddArgument("--disable-extensions");
            chromeOptions.AddArgument("--headless"); // Run headless for testing purposes
            _driver = new ChromeDriver(chromeOptions);
            _js = (IJavaScriptExecutor)_driver;
            _driver.Navigate().GoToUrl("../../game.html");  // Adjust the path accordingly
            _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
        }

        [Test]
        public void TestGameInitialization()
        {
            var canvas = _driver.FindElement(By.Id("gameCanvas"));
            Assert.IsNotNull(canvas);

            var initialScore = (long)_js.ExecuteScript("return score;");
            Assert.AreEqual(0, initialScore);
        }

        

        [TearDown]
        public void Teardown()
        {
            _driver.Quit();
        }
    }
}
