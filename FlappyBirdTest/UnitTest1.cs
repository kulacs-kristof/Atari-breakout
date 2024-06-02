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

        [Test]
public void TestBirdMovementOnButtonPress()
{
    var initialY = (long)_js.ExecuteScript("return bird.y;");
    _js.ExecuteScript("document.dispatchEvent(new KeyboardEvent('keydown', {'key': ' ' }));");
    Thread.Sleep(100);
    var newY = (long)_js.ExecuteScript("return bird.y;");
    Assert.Less(newY, initialY);
}
        
        [Test]
public void TestBirdFallsWithoutButtonPress()
{
    var initialY = (long)_js.ExecuteScript("return bird.y;");
    Thread.Sleep(500);
    var newY = (long)_js.ExecuteScript("return bird.y;");
    Assert.Greater(newY, initialY);
}

        [Test]
public void TestBirdDiesOnCollision()
{
    // Move bird to a collision position
    _js.ExecuteScript("bird.y = 700; update();");
    Thread.Sleep(100);
    var birdYAfterCollision = (long)_js.ExecuteScript("return bird.y;");
    Assert.AreEqual(150, birdYAfterCollision);  // Bird reset position
}

        [Test]
public void TestGameOverTextAppears()
{
    // Move bird to a collision position
    _js.ExecuteScript("bird.y = 700; update();");
    Thread.Sleep(100);
    var gameOverText = _driver.FindElement(By.XPath("//*[contains(text(), 'Game Over')]"));
    Assert.IsNotNull(gameOverText);
}

        [Test]
public void TestBirdAppearsAtStart()
{
    var initialY = (long)_js.ExecuteScript("return bird.y;");
    Assert.AreEqual(150, initialY);
}

        [Test]
public void TestBackgroundMovesDuringGame()
{
    var initialBackgroundX = (long)_js.ExecuteScript("return backgroundImg.offsetLeft;");
    Thread.Sleep(1000);
    var newBackgroundX = (long)_js.ExecuteScript("return backgroundImg.offsetLeft;");
    Assert.AreNotEqual(initialBackgroundX, newBackgroundX);
}

        [Test]
public void TestCounterCountsPoints()
{
    var initialScore = (long)_js.ExecuteScript("return score;");
    Thread.Sleep(3000);  // Wait for some pipes to be passed
    var newScore = (long)_js.ExecuteScript("return score;");
    Assert.Greater(newScore, initialScore);
}



        [TearDown]
        public void Teardown()
        {
            _driver.Quit();
        }
    }
}
