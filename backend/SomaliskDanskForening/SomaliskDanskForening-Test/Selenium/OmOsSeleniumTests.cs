using OpenQA.Selenium;

namespace SomaliskDanskForening_Test.Selenium
{
    [TestClass]
    [DoNotParallelize]
    public class OmOsSeleniumTests : SeleniumBase
    {
        private static readonly string Url = BaseUrl + "/OmOs/OmOs.html";

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
        public void OmOsPage_TitleIsCorrect()
        {
            Assert.IsTrue(Driver.Title.Contains("Om os") || Driver.Title.Contains("Forening"));
        }

        [TestMethod]
        public void OmOsPage_HeroBadgeShowsOmOs()
        {
            Assert.AreEqual("Om os", Driver.FindElement(By.CssSelector(".hero-badge")).Text.Trim());
        }

        [TestMethod]
        public void OmOsPage_HasThreeMissionCards()
        {
            Assert.AreEqual(3, Driver.FindElements(By.CssSelector(".vm-card")).Count);
        }

        [TestMethod]
        public void OmOsPage_HasThreeStatItems()
        {
            Assert.AreEqual(3, Driver.FindElements(By.CssSelector(".stat-item")).Count);
        }

        [TestMethod]
        public void OmOsPage_StatsContains1997()
        {
            Assert.IsTrue(Driver.FindElements(By.CssSelector(".stat-item")).Any(s => s.Text.Contains("1997")));
        }

        [TestMethod]
        public void OmOsPage_HasAtLeastThreeTimelineItems()
        {
            Assert.IsTrue(Driver.FindElements(By.CssSelector(".timeline-item")).Count >= 3);
        }

        [TestMethod]
        public void OmOsPage_HasAtLeastFourValueCards()
        {
            Assert.IsTrue(Driver.FindElements(By.CssSelector(".vaerdi-card")).Count >= 4);
        }

        [TestMethod]
        public void OmOsPage_HasFooterWithYear()
        {
            Assert.IsTrue(Driver.FindElement(By.CssSelector("footer.footer")).Text.Contains("2026"));
        }

        [DataTestMethod]
        [DataRow(".omos-hero")]
        [DataRow(".vm-section")]
        [DataRow(".stats-section")]
        [DataRow(".historie-section")]
        [DataRow(".vaerdier-section")]
        [DataRow("footer.footer")]
        public void OmOsPage_RequiredSectionsExist(string selector)
        {
            Assert.IsNotNull(Driver.FindElement(By.CssSelector(selector)));
        }
    }
}
