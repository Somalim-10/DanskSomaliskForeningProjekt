using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SomaliskDanskForening_Test.Selenium
{
    [TestClass]
    [DoNotParallelize]
    public class KontaktSeleniumTests : SeleniumBase
    {
        private static readonly string Url = BaseUrl + "/Kontakt/Kontakt.html";

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
        public void KontaktPage_TitleIsCorrect()
        {
            Assert.IsTrue(Driver.Title.Contains("Kontakt") || Driver.Title.Contains("Forening"));
        }

        [TestMethod]
        public void KontaktPage_HeroBadgeShowsKontakt()
        {
            Assert.AreEqual("Kontakt", Driver.FindElement(By.CssSelector(".hero-badge")).Text.Trim());
        }

        [TestMethod]
        public void KontaktPage_ShowsContactInfoOrState()
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
            bool ok = wait.Until(d =>
                d.FindElements(By.CssSelector(".kontakt-grid, .kontakt-empty, .kontakt-error")).Count > 0);
            Assert.IsTrue(ok);
        }

        [TestMethod]
        public void KontaktPage_AdminSeesEditOrAddButton()
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
            wait.Until(d =>
                d.FindElements(By.CssSelector(".kontakt-edit-btn, .kontakt-add-btn, .kontakt-grid, .kontakt-empty")).Count > 0);

            bool hasBtn = Driver.FindElements(By.CssSelector(".kontakt-edit-btn, .kontakt-add-btn")).Count > 0;
            Assert.IsTrue(hasBtn, "Admin skal se rediger- eller tilfoej-knap pa kontaktsiden");
        }

        [TestMethod]
        public void KontaktPage_HasFooterWithYear()
        {
            Assert.IsTrue(Driver.FindElement(By.CssSelector("footer.footer")).Text.Contains("2026"));
        }

        [TestMethod]
        public void KontaktPage_HasPersondatapolitikLink()
        {
            Assert.IsNotNull(Driver.FindElement(By.CssSelector("footer a[href*='persondatapolitik']")));
        }

        [DataTestMethod]
        [DataRow(".kontakt-hero")]
        [DataRow(".kontakt-section")]
        [DataRow("footer.footer")]
        public void KontaktPage_RequiredElementsExist(string selector)
        {
            Assert.IsNotNull(Driver.FindElement(By.CssSelector(selector)));
        }
    }
}
