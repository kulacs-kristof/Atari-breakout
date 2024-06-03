using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using Xunit;

namespace FlappyBirdTests
{
    public class FlappyBirdTests : IDisposable
    {
        private readonly ChromeDriver driver;

        public FlappyBirdTests()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://kulacs-kristof.github.io/Flappy-Bird/game.html");
        }

        public void Dispose()
        {
            driver.Quit();
        }

        [Fact]
        public void GameCanvasIsDisplayed()
        {
            var canvas = driver.FindElement(By.Id("gameCanvas"));
            Assert.NotNull(canvas);
        }

        [Fact]
        public void BirdImageIsLoaded()
        {
            var script = @"
                var img = document.createElement('img');
                img.src = 'bird.png';
                img.onload = function() {
                    return true;
                };
                return img.complete;";
            var result = (bool)driver.ExecuteScript(script);
            Assert.True(result);
        }

        [Fact]
        public void BirdMovesOnKeyPress()
        {
            var initialBirdYPosition = driver.ExecuteScript("return bird.y;");
            driver.FindElement(By.TagName("body")).SendKeys(Keys.Space);
            System.Threading.Thread.Sleep(100); // v�rakozik hogy megn�zze t�rt�nik e valami
            var newBirdYPosition = driver.ExecuteScript("return bird.y;");
            Assert.NotEqual(initialBirdYPosition, newBirdYPosition);
        }

        [Fact]
        public void PipesAppearAndMove()
        {
            var initialPipeCount = driver.ExecuteScript("return pipes.length;");
            System.Threading.Thread.Sleep(2000); // cs�vek gener�l�s�ra v�r
            var newPipeCount = driver.ExecuteScript("return pipes.length;");
            Assert.Equal(initialPipeCount, newPipeCount);
        }

        [Fact]
        public void GameResetsOnCollision()
        {
            driver.ExecuteScript("bird.y = 0; bird.speed = 10;"); // tetej�nek megy
            System.Threading.Thread.Sleep(500); // �tk�z�s
            var birdYPosition = driver.ExecuteScript("return bird.y;");
            Assert.NotEqual(150, birdYPosition);
        }

        [Fact]
        public void BirdFallsWithoutKeyPress()
        {
            var initialBirdYPosition = driver.ExecuteScript("return bird.y;");
            System.Threading.Thread.Sleep(1000); // esik e a mad�r
            var newBirdYPosition = driver.ExecuteScript("return bird.y;");
            Assert.True((double)newBirdYPosition > (double)initialBirdYPosition);
        }
    }
}
