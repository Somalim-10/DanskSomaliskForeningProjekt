using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SomaliskDanskForening_Test.Selenium
{
    [TestClass]
    [DoNotParallelize]
    public class DonationSeleniumTests : SeleniumBase
    {
        private static readonly string Url = BaseUrl + "/Donation/Donation.html";

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
        public void DonationPage_TitleIsCorrect()
        {
            Assert.IsTrue(Driver.Title.Contains("Donation") || Driver.Title.Contains("Forening"));
        }

        [TestMethod]
        public void DonationPage_HeroBadgeShowsDonation()
        {
            Assert.AreEqual("Donation", Driver.FindElement(By.CssSelector(".hero-badge")).Text.Trim());
        }

        [TestMethod]
        public void DonationPage_HasThreeStepCards()
        {
            Assert.AreEqual(3, Driver.FindElements(By.CssSelector(".step-card")).Count);
        }

        [TestMethod]
        public void DonationPage_HasThreeBankDetails()
        {
            Assert.AreEqual(3, Driver.FindElements(By.CssSelector(".bank-detail-item")).Count);
        }

        [TestMethod]
        public void DonationPage_HasAtLeastFourFeatureCards()
        {
            Assert.IsTrue(Driver.FindElements(By.CssSelector(".feature-card")).Count >= 4);
        }

        [TestMethod]
        public void DonationPage_HasThreeImpactCards()
        {
            Assert.AreEqual(3, Driver.FindElements(By.CssSelector(".impact-card")).Count);
        }

        [TestMethod]
        public void DonationPage_AdminSeesEditButton()
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
            wait.Until(d => d.FindElements(By.CssSelector(".hero-edit-btn, .mobilepay-edit-btn")).Count > 0 ||
                            d.FindElements(By.CssSelector(".mobilepay-loading")).Count > 0);

            bool hasEditBtn = Driver.FindElements(By.CssSelector(".hero-edit-btn, .mobilepay-edit-btn")).Count > 0;
            Assert.IsTrue(hasEditBtn, "Admin skal se rediger-knap pa donationssiden");
        }

        [TestMethod]
        public void DonationPage_HasFooterWithYear()
        {
            Assert.IsTrue(Driver.FindElement(By.CssSelector("footer.footer")).Text.Contains("2026"));
        }

        [DataTestMethod]
        [DataRow("#bankoverforsel")]
        [DataRow("footer.footer")]
        public void DonationPage_RequiredSectionsExist(string selector)
        {
            Assert.IsNotNull(Driver.FindElement(By.CssSelector(selector)));
        }
    }
}
