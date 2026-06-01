using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SomaliskDanskForening_Test.Selenium
{
    [TestClass]
    [DoNotParallelize]
    public class LoginSeleniumTests : SeleniumBase
    {
        [TestInitialize]
        public void TestInitialize() => InitDriver();

        [TestCleanup]
        public void TestCleanup() => QuitDriver();

        [TestMethod]
        public void LoginPage_Loads_ShowsForm()
        {
            Driver.Navigate().GoToUrl(LoginUrl);
            Assert.IsNotNull(Driver.FindElement(By.Id("username")));
            Assert.IsNotNull(Driver.FindElement(By.Id("password")));
            Assert.IsNotNull(Driver.FindElement(By.CssSelector("button[type='submit']")));
        }

        [TestMethod]
        public void Login_WrongCredentials_ShowsErrorMessage()
        {
            Driver.Navigate().GoToUrl(LoginUrl);
            Type(By.Id("username"), "forkertbruger");
            Type(By.Id("password"), "forkertpassword");
            Click(By.CssSelector("button[type='submit']"));

            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
            var msg = wait.Until(d => d.FindElement(By.CssSelector(".message-box.error")));
            Assert.IsTrue(msg.Displayed);
        }

        [TestMethod]
        public void Login_EmptyFields_DoesNotCallApi()
        {
            Driver.Navigate().GoToUrl(LoginUrl);
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            // Vent til Vue er færdig med at montere (fjerner den skjulte besked-boks fra skabelonen)
            wait.Until(d => d.FindElements(By.CssSelector(".message-box")).Count == 0);

            Click(By.CssSelector("button[type='submit']"));

            // Tomme felter => browseren blokerer submit => login() kaldes aldrig => ingen besked, og vi bliver på login-siden
            bool noApiCall = wait.Until(d =>
                d.Url.Contains("/Login/index.html") &&
                d.FindElements(By.CssSelector(".message-box")).Count == 0);
            Assert.IsTrue(noApiCall);
        }

        [TestMethod]
        public void Login_CorrectCredentials_ShowsSuccessAndRedirects()
        {
            LoginAsAdmin();
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            bool ok = wait.Until(d =>
                d.FindElements(By.CssSelector(".message-box.success")).Count > 0 ||
                !d.Url.Contains("/Login/index.html"));
            Assert.IsTrue(ok);
        }
    }
}
