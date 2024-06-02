using System;
using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace FlappyBirdTests
{
    public class FlappyBirdGameTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly IJavaScriptExecutor _js;

        public FlappyBirdGameTests()
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

        [Fact]
        public void TestGameInitialization()
        {
            var canvas = _driver.FindElement(By.Id("gameCanvas"));
            if (canvas == null) throw new Exception("Canvas element not found.");

            var initialScore = (long)_js.ExecuteScript("return score;");
            if (initialScore != 0) throw new Exception($"Expected initial score to be 0, but found {initialScore}.");
        }

        [Fact]
        public void TestBirdMovementOnButtonPress()
        {
            var initialY = (long)_js.ExecuteScript("return bird.y;");
            _js.ExecuteScript("document.dispatchEvent(new KeyboardEvent('keydown', {'key': ' ' }));");
            Thread.Sleep(100);
            var newY = (long)_js.ExecuteScript("return bird.y;");
            if (newY >= initialY) throw new Exception("Bird did not move up on button press.");
        }

        [Fact]
        public void TestBirdFallsWithoutButtonPress()
        {
            var initialY = (long)_js.ExecuteScript("return bird.y;");
            Thread.Sleep(500);
            var newY = (long)_js.ExecuteScript("return bird.y;");
            if (newY <= initialY) throw new Exception("Bird did not fall without button press.");
        }

        [Fact]
        public void TestBirdDiesOnCollision()
        {
            _js.ExecuteScript("bird.y = 700; update();");
            Thread.Sleep(100);
            var birdYAfterCollision = (long)_js.ExecuteScript("return bird.y;");
            if (birdYAfterCollision != 150) throw new Exception("Bird did not reset position after collision.");
        }

        [Fact]
        public void TestGameOverTextAppears()
        {
            _js.ExecuteScript("bird.y = 700; update();");
            Thread.Sleep(100);
            var gameOverText = _driver.FindElement(By.XPath("//*[contains(text(), 'Game Over')]"));
            if (gameOverText == null) throw new Exception("Game Over text not found.");
        }

        [Fact]
        public void TestBirdAppearsAtStart()
        {
            var initialY = (long)_js.ExecuteScript("return bird.y;");
            if (initialY != 150) throw new Exception($"Expected bird to start at y=150, but found y={initialY}.");
        }

        [Fact]
        public void TestBackgroundMovesDuringGame()
        {
            var initialBackgroundX = (long)_js.ExecuteScript("return backgroundImg.offsetLeft;");
            Thread.Sleep(1000);
            var newBackgroundX = (long)_js.ExecuteScript("return backgroundImg.offsetLeft;");
            if (initialBackgroundX == newBackgroundX) throw new Exception("Background did not move during game.");
        }

        [Fact]
        public void TestCounterCountsPoints()
        {
            var initialScore = (long)_js.ExecuteScript("return score;");
            Thread.Sleep(3000);
            var newScore = (long)_js.ExecuteScript("return score;");
            if (newScore <= initialScore) throw new Exception("Score did not increase.");
        }

        [Fact]
        public void TestObstaclesAppearCorrectly()
        {
            var pipesCountBefore = (long)_js.ExecuteScript("return pipes.length;");
            Thread.Sleep(2000);
            var pipesCountAfter = (long)_js.ExecuteScript("return pipes.length;");
            if (pipesCountAfter <= pipesCountBefore) throw new Exception("New obstacles did not appear.");
        }

        public void Dispose()
        {
            _driver.Quit();
        }
    }
}
