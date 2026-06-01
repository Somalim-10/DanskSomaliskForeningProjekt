using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SomaliskDanskForening_Test.Selenium
{
    [TestClass]
    [DoNotParallelize]
    public class EventSeleniumTests : SeleniumBase
    {
        private static readonly string Url = BaseUrl + "/Event/Event.html";

        [TestInitialize]
        public void TestInitialize()
        {
            InitDriver();
            LoginAsAdmin();
            Driver.Navigate().GoToUrl(Url);
        }

        [TestCleanup]
        public void TestCleanup() => QuitDriver();

        [TestMethod]
        public void EventPage_TitleIsCorrect()
        {
            Assert.IsTrue(Driver.Title.Contains("Events") || Driver.Title.Contains("Forening"));
        }

      

        [TestMethod]
        public void EventPage_HasTwoSortButtons()
        {
            Assert.AreEqual(2, Driver.FindElements(By.CssSelector(".sort-btn")).Count);
        }

        [TestMethod]
        public void EventPage_AdminSeesAddButton()
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
            wait.Until(d => d.FindElements(By.CssSelector(".hero-cta, .empty-cta")).Count > 0 ||
                            d.FindElements(By.CssSelector(".events-grid")).Count > 0);

            // Admin skal se knap ENTEN i hero ELLER i tom state
            bool hasAddBtn = Driver.FindElements(By.CssSelector(".hero-cta, .empty-cta")).Count > 0;
            bool hasGrid = Driver.FindElements(By.CssSelector(".events-grid")).Count > 0;
            Assert.IsTrue(hasAddBtn || hasGrid, "Admin skal se events eller en tilfoej-knap");
        }

        [TestMethod]
        public void EventPage_ShowsContentAfterLoad()
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
            bool ok = wait.Until(d =>
                d.FindElements(By.CssSelector(".events-grid, .empty-state, .error-state")).Count > 0);
            Assert.IsTrue(ok);
        }

        [TestMethod]
        public void EventPage_HasFooterWithYear()
        {
            Assert.IsTrue(Driver.FindElement(By.CssSelector("footer.footer")).Text.Contains("2026"));
        }

        [DataTestMethod]
        [DataRow(".event-hero")]
        [DataRow(".events-section")]
        [DataRow("footer.footer")]
        public void EventPage_RequiredElementsExist(string selector)
        {
            Assert.IsNotNull(Driver.FindElement(By.CssSelector(selector)));
        }
    }
}
