using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SomaliskDanskForening_Test.Selenium
{
    /// <summary>
    /// Integreret admin-test der logger ind en gang og koerer handlinger paa
    /// Event-, Kontakt- og Donation-siderne. Siderne bruger separate Add/Edit-sider
    /// (ikke modaler), og sletning sker via en browser-confirm()-dialog.
    /// </summary>
    [TestClass]
    [DoNotParallelize]
    public class AdminCRUDSeleniumTests : SeleniumBase
    {
        private const int SLOW_PAUSE_MS = 600;
        private const int WAIT_TIMEOUT_SEC = 20;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Console.WriteLine("Logger ind som admin...");
            var testInstance = new AdminCRUDSeleniumTests();
            testInstance.InitDriver();
            testInstance.LoginAsAdmin();
            Console.WriteLine("Admin er logget ind");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Console.WriteLine("Lukker browser...");
            SeleniumBase.QuitSharedDriver();
        }

        // ---------- Hjaelpemetoder ----------

        private WebDriverWait NewWait()
        {
            var w = new WebDriverWait(Driver, TimeSpan.FromSeconds(WAIT_TIMEOUT_SEC));
            // Vue gen-renderer lister, sa elementer kan blive foraeldede midt i et tjek
            w.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            return w;
        }

        // Saetter vaerdien paa et input/textarea og udloeser Vue's v-model (input + change)
        private void SetVueValue(string id, string value)
        {
            NewWait().Until(d => d.FindElements(By.Id(id)).Any(e => e.Displayed));
            ((IJavaScriptExecutor)Driver).ExecuteScript(
                "var el = document.getElementById(arguments[0]);" +
                "if (!el) { return false; }" +
                "var proto = el.tagName === 'TEXTAREA' ? window.HTMLTextAreaElement.prototype : window.HTMLInputElement.prototype;" +
                "var setter = Object.getOwnPropertyDescriptor(proto, 'value').set;" +
                "setter.call(el, arguments[1]);" +
                "el.dispatchEvent(new Event('input', { bubbles: true }));" +
                "el.dispatchEvent(new Event('change', { bubbles: true }));" +
                "return true;", id, value);
            Thread.Sleep(SLOW_PAUSE_MS);
        }

        private void ClickCss(string css)
        {
            var el = NewWait().Until(d => d.FindElements(By.CssSelector(css)).FirstOrDefault(x => x.Displayed));
            el.Click();
            Thread.Sleep(SLOW_PAUSE_MS);
        }

        // Accepterer en browser-confirm()-dialog hvis en er aaben
        private void AcceptConfirmIfPresent()
        {
            for (int i = 0; i < 20; i++)
            {
                try { Driver.SwitchTo().Alert().Accept(); return; }
                catch (NoAlertPresentException) { Thread.Sleep(250); }
            }
        }

        private IWebElement? FindEventCard(string titleText) =>
            Driver.FindElements(By.CssSelector(".event-card")).FirstOrDefault(c => c.Text.Contains(titleText));

        // ---------- Event: fuld CRUD via Add/Edit-sider ----------

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Event")]
        public void Event_AdminCRUD_CreateReadUpdateDelete()
        {
            Console.WriteLine("\n========== EVENT CRUD START ==========");
            var wait = NewWait();
            string title = $"TestEvent_{DateTime.Now.Ticks}";
            string desc = "Automatisk oprettet test-event";
            string date = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");

            // CREATE
            Console.WriteLine("CREATE: opretter nyt event");
            Driver.Navigate().GoToUrl(BaseUrl + "/Event/Event.html");
            ClickCss(".hero-cta, .empty-cta");                 // -> Add.html
            wait.Until(d => d.Url.Contains("Add.html"));
            SetVueValue("title", title);
            SetVueValue("date", date);
            SetVueValue("startTime", "10");
            SetVueValue("duration", "2");
            SetVueValue("description", desc);
            ClickCss(".btn-primary");                          // addItem -> redirect til Event.html
            wait.Until(d => d.Url.Contains("Event.html") && !d.Url.Contains("Add.html"));

            // READ
            Console.WriteLine("READ: finder eventet i listen");
            wait.Until(d => d.FindElements(By.CssSelector(".event-card")).Any(c => c.Text.Contains(title)));
            Assert.IsNotNull(FindEventCard(title), "Oprettet event blev ikke fundet i listen");

            // UPDATE
            Console.WriteLine("UPDATE: redigerer beskrivelsen");
            string updatedDesc = $"Opdateret beskrivelse {DateTime.Now.Ticks}";
            FindEventCard(title)!.FindElement(By.CssSelector(".edit-btn")).Click();   // -> Edit.html?id=
            wait.Until(d => d.Url.Contains("Edit.html"));
            SetVueValue("description", updatedDesc);
            ClickCss(".btn-primary");                          // updateItem -> redirect til Event.html
            wait.Until(d => d.Url.Contains("Event.html") && !d.Url.Contains("Edit.html"));
            wait.Until(d => d.FindElements(By.CssSelector(".event-card")).Any(c => c.Text.Contains(updatedDesc)));
            Console.WriteLine("Event opdateret");

            // DELETE
            Console.WriteLine("DELETE: sletter eventet");
            FindEventCard(title)!.FindElement(By.CssSelector(".delete-btn")).Click();
            AcceptConfirmIfPresent();                          // bekraeft browser-dialog
            wait.Until(d => !d.FindElements(By.CssSelector(".event-card")).Any(c => c.Text.Contains(title)));
            Console.WriteLine("Event slettet");
            Console.WriteLine("========== EVENT CRUD FAERDIG ==========\n");
        }

        // ---------- Kontakt: vis + rediger (siden har en post, ingen sletning i UI) ----------

        [TestMethod]
        [TestCategory("Kontakt")]
        public void Kontakt_Admin_ViewAndUpdate()
        {
            Console.WriteLine("\n========== KONTAKT START ==========");
            var wait = NewWait();
            Driver.Navigate().GoToUrl(BaseUrl + "/Kontakt/Kontakt.html");

            // Opret oplysninger hvis siden er tom (admin ser 'Tilfoej'-knap)
            if (Driver.FindElements(By.CssSelector(".kontakt-add-btn")).Any(e => e.Displayed))
            {
                Console.WriteLine("Ingen kontakt endnu - opretter");
                ClickCss(".kontakt-add-btn");                  // -> Add.html
                wait.Until(d => d.Url.Contains("Add.html"));
                SetVueValue("address", "Rosenvej 5, 4300 Holbaek");
                SetVueValue("phone", "+45 59 43 12 34");
                SetVueValue("email", "kontakt@somaliskdanskforening.dk");
                ClickCss(".btn-primary");
                wait.Until(d => d.Url.Contains("Kontakt.html") && !d.Url.Contains("Add.html"));
            }

            // READ
            Console.WriteLine("READ: viser kontaktoplysninger");
            var grid = wait.Until(d => d.FindElements(By.CssSelector(".kontakt-grid")).FirstOrDefault(e => e.Displayed));
            Assert.IsNotNull(grid, "Kontakt-oplysninger blev ikke vist");
            Assert.IsTrue(grid.Text.Trim().Length > 0, "Kontakt-kort var tomt");

            // UPDATE: rediger telefonnummer
            Console.WriteLine("UPDATE: aendrer telefonnummer");
            string newPhone = "70" + (DateTime.Now.Ticks % 1000000).ToString("D6");
            ClickCss(".kontakt-edit-btn");                     // -> Edit.html?id=
            wait.Until(d => d.Url.Contains("Edit.html"));
            SetVueValue("address", "Rosenvej 5, 4300 Holbaek");
            SetVueValue("email", "kontakt@somaliskdanskforening.dk");
            SetVueValue("phone", newPhone);
            ClickCss(".btn-primary");                          // updateItem -> redirect til Kontakt.html
            wait.Until(d => d.Url.Contains("Kontakt.html") && !d.Url.Contains("Edit.html"));

            Driver.Navigate().GoToUrl(BaseUrl + "/Kontakt/Kontakt.html");
            wait.Until(d => d.FindElements(By.CssSelector(".kontakt-grid")).Any(e => e.Displayed));
            wait.Until(d => d.PageSource.Contains(newPhone));
            Console.WriteLine("Telefonnummer opdateret og vist");
            Console.WriteLine("========== KONTAKT FAERDIG ==========\n");
        }

        // ---------- Donation: vis + rediger indhold ----------

        [TestMethod]
        [TestCategory("Donation")]
        public void Donation_AdminUpdate()
        {
            Console.WriteLine("\n========== DONATION START ==========");
            var wait = NewWait();
            Driver.Navigate().GoToUrl(BaseUrl + "/Donation/Donation.html");

            // Badge-teksten (CSS goer den til STORE bogstaver -> sammenlign versal-uafhaengigt)
            var badge = wait.Until(d => d.FindElements(By.CssSelector(".hero-badge")).FirstOrDefault(e => e.Displayed));
            Assert.IsNotNull(badge, "Hero-badge blev ikke fundet");
            Assert.AreEqual("donation", badge.Text.Trim().ToLowerInvariant(), "Forkert badge-tekst");
            Console.WriteLine("Donation-siden indlaest");

            // UPDATE: rediger donationsteksten
            Console.WriteLine("UPDATE: aendrer donationsindhold");
            string marker = $"Opdateret {DateTime.Now.Ticks}";
            string newText = $"Din stoette goer en forskel. {marker}";
            ClickCss(".hero-edit-btn");                        // -> Edit.html?id=
            wait.Until(d => d.Url.Contains("Edit.html"));
            SetVueValue("text", newText);
            ClickCss(".btn-primary");                          // updateItem -> redirect til Donation.html
            wait.Until(d => d.Url.Contains("Donation.html") && !d.Url.Contains("Edit.html"));

            Driver.Navigate().GoToUrl(BaseUrl + "/Donation/Donation.html");
            wait.Until(d => d.PageSource.Contains(marker));
            Console.WriteLine("Donationsindhold opdateret og vist");
            Console.WriteLine("========== DONATION FAERDIG ==========\n");
        }

        // ---------- Navigation ----------

        [TestMethod]
        [TestCategory("Navigation")]
        public void Navigation_TestButtonNavigation()
        {
            Console.WriteLine("\n========== NAVIGATION START ==========");
            var wait = NewWait();
            Driver.Navigate().GoToUrl(BaseUrl + "/Kontakt/Kontakt.html");
            Thread.Sleep(SLOW_PAUSE_MS);

            var navLinks = Driver.FindElements(By.CssSelector("a[href]"));
            Console.WriteLine($"Fandt {navLinks.Count} links");

            var firstNavLink = navLinks.FirstOrDefault(l =>
                !(l.GetAttribute("href")?.Contains("Kontakt") ?? true) &&
                l.Displayed);

            if (firstNavLink != null)
            {
                firstNavLink.Click();
                wait.Until(d => !d.Url.Contains("Kontakt"));
                Assert.IsFalse(Driver.Url.Contains("Kontakt"), "Skulle have navigeret vaek fra Kontakt");
                Driver.Navigate().Back();
                Console.WriteLine("Navigation og tilbage-funktion virker");
            }
            Console.WriteLine("========== NAVIGATION FAERDIG ==========\n");
        }
    }
}
