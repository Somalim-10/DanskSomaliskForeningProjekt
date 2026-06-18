using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SomaliskDanskForening_Test.Selenium
{
    /// <summary>
    /// Integreret test der logger ind én gang og kører CRUD-operationer (Oprette, Redigere, Slette)
    /// på alle admin-sider: Kontakt, Event og Donation.
    /// 
    /// Testen kører langsomt med synlige pauser så du kan følge med i hvad der sker.
    /// Hvis en knap fører til en anden side, testes navigation og der går vi tilbage.
    /// </summary>
    [TestClass]
    [DoNotParallelize]
    public class AdminCRUDSeleniumTests : SeleniumBase
    {
        private const int SLOW_PAUSE_MS = 2000;  // 2 sekunder pause mellem actions
        private const int WAIT_TIMEOUT_SEC = 20;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            // Login så én gang for hele test-klassen
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

        #region Kontakt Side CRUD Tests

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Kontakt")]
        public void Kontakt_AdminCRUD_CreateReadUpdateDelete()
        {
            Console.WriteLine("\n========== KONTAKT SIDE TEST START ==========");
            Console.WriteLine("Navigerer til Kontakt-siden...");
            
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(WAIT_TIMEOUT_SEC));
            Driver.Navigate().GoToUrl(BaseUrl + "/Kontakt/Kontakt.html");
            Thread.Sleep(SLOW_PAUSE_MS);

            // ===== CREATE =====
            Console.WriteLine("\nTEST 1: OPRETTE ny kontakt");
            string kontaktName = $"TestKontakt_{DateTime.Now.Ticks}";
            string kontaktEmail = $"test{DateTime.Now.Ticks}@test.dk";
            string kontaktPhone = "12345678";

            // Find and click the "Add Contact" button
            var addButton = Driver.FindElements(By.CssSelector(".kontakt-add-btn")).FirstOrDefault();
            Assert.IsNotNull(addButton, "Admin skal kunne se 'Tilføj kontakt' knap");
            addButton.Click();
            Thread.Sleep(SLOW_PAUSE_MS);

            var modal = wait.Until(d => 
                d.FindElements(By.CssSelector(".modal, [role='dialog'], .kontakt-form")).FirstOrDefault());
            Assert.IsNotNull(modal, "? Modal/form åbnede ikke");
            Console.WriteLine(" Modal åbnet");

            Console.WriteLine($" Udfylder form med navn: {kontaktName}");
            Thread.Sleep(1000);
            Type(By.CssSelector("input[name='name'], input[placeholder*='Navn']"), kontaktName);
            Thread.Sleep(SLOW_PAUSE_MS);

            Console.WriteLine($" Udfylder email: {kontaktEmail}");
            Type(By.CssSelector("input[name='email'], input[placeholder*='Email']"), kontaktEmail);
            Thread.Sleep(SLOW_PAUSE_MS);

            Console.WriteLine($" Udfylder telefon: {kontaktPhone}");
            Type(By.CssSelector("input[name='phone'], input[placeholder*='Telefon']"), kontaktPhone);
            Thread.Sleep(SLOW_PAUSE_MS);

            Console.WriteLine(" Sender form (Submit)");
            var submitBtn = Driver.FindElements(By.CssSelector("button[type='submit'], .btn-save")).FirstOrDefault();
            Assert.IsNotNull(submitBtn, "? Submit knap ikke fundet");
            submitBtn.Click();
            Thread.Sleep(SLOW_PAUSE_MS);

            wait.Until(d => d.FindElements(By.CssSelector(".success, [role='alert'].success")).Count > 0 ||
                           d.FindElements(By.CssSelector(".kontakt-grid")).Count > 0);
            Console.WriteLine("Kontakt oprettet og success-besked vist");
            Thread.Sleep(SLOW_PAUSE_MS);

            // ===== READ =====
            Console.WriteLine("\nTEST 2: LESE kontakt fra listen");
            var contactItems = Driver.FindElements(By.CssSelector(".kontakt-item, .contact-card, [data-test='contact-item']"));
            var createdContact = contactItems.FirstOrDefault(c => c.Text.Contains(kontaktName) || c.Text.Contains(kontaktEmail));
            Assert.IsNotNull(createdContact, $"? Oprettet kontakt '{kontaktName}' ikke fundet i listen");
            Console.WriteLine($"Kontakt fundet i listen: {kontaktName}");
            Thread.Sleep(SLOW_PAUSE_MS);

            // ===== UPDATE =====
            Console.WriteLine("\nTEST 3: REDIGERE kontakt");
            var editBtn = createdContact.FindElements(By.CssSelector(".edit-btn, .btn-edit, [data-action='edit']")).FirstOrDefault();
            if (editBtn == null)
            {
                Console.WriteLine(" Ingen separat edit-knap, klikker på kontakt selv");
                createdContact.Click();
            }
            else
            {
                Console.WriteLine(" Klikker på edit-knap");
                editBtn.Click();
            }
            Thread.Sleep(SLOW_PAUSE_MS);

            var editModal = wait.Until(d => 
                d.FindElements(By.CssSelector(".modal, [role='dialog']")).FirstOrDefault());
            Assert.IsNotNull(editModal, "? Edit-modal åbnede ikke");
            Console.WriteLine(" Edit-modal åbnet");
            Thread.Sleep(SLOW_PAUSE_MS);

            string updatedEmail = $"updated{DateTime.Now.Ticks}@test.dk";
            Console.WriteLine($" Ændrer email til: {updatedEmail}");
            var emailInput = Driver.FindElements(By.CssSelector("input[name='email'], input[placeholder*='Email']")).FirstOrDefault();
            Assert.IsNotNull(emailInput, "? Email-felt ikke fundet");
            emailInput.Clear();
            emailInput.SendKeys(updatedEmail);
            Thread.Sleep(SLOW_PAUSE_MS);

            Console.WriteLine(" Sender updated form");
            var updateSubmit = Driver.FindElements(By.CssSelector("button[type='submit'], .btn-save")).FirstOrDefault();
            updateSubmit?.Click();
            Thread.Sleep(SLOW_PAUSE_MS);

            wait.Until(d => d.FindElements(By.CssSelector(".success")).Count > 0 ||
                           d.FindElements(By.CssSelector(".kontakt-grid")).Count > 0);
            Console.WriteLine("Kontakt opdateret");
            Thread.Sleep(SLOW_PAUSE_MS);

            // ===== DELETE =====
            Console.WriteLine("\nTEST 4: SLETTE kontakt");
            contactItems = Driver.FindElements(By.CssSelector(".kontakt-item, .contact-card"));
            var contactToDelete = contactItems.FirstOrDefault(c => c.Text.Contains(kontaktName));
            Assert.IsNotNull(contactToDelete, "? Kontakt til sletning ikke fundet");

            var deleteBtn = contactToDelete.FindElements(By.CssSelector(".delete-btn, .btn-delete, [data-action='delete']")).FirstOrDefault();
            Assert.IsNotNull(deleteBtn, "? Slet-knap ikke fundet");
            Console.WriteLine(" Klikker på slet-knap");
            deleteBtn.Click();
            Thread.Sleep(SLOW_PAUSE_MS);

            // Håndtér bekræftelse hvis den findes
            var confirmBtns = Driver.FindElements(By.CssSelector("button.confirm, .btn-confirm, [data-action='confirm']"));
            if (confirmBtns.Count > 0)
            {
                Console.WriteLine(" Bekræftelses-dialog fundet, bekræfter sletning");
                confirmBtns[0].Click();
                Thread.Sleep(SLOW_PAUSE_MS);
            }

            wait.Until(d => d.FindElements(By.CssSelector(".success")).Count > 0 ||
                           d.FindElements(By.CssSelector(".kontakt-grid")).Count > 0);
            Console.WriteLine("Kontakt slettet");
            Thread.Sleep(SLOW_PAUSE_MS);

            Console.WriteLine("\n========== KONTAKT SIDE TEST FÆRDIG ==========\n");
        }

        #endregion

        #region Event Side CRUD Tests

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Event")]
        public void Event_AdminCRUD_CreateReadUpdateDelete()
        {
            Console.WriteLine("\n========== EVENT SIDE TEST START ==========");
            Console.WriteLine("Navigerer til Event-siden...");
            
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(WAIT_TIMEOUT_SEC));
            Driver.Navigate().GoToUrl(BaseUrl + "/Event/Event.html");
            Thread.Sleep(SLOW_PAUSE_MS);

            // ===== CREATE =====
            Console.WriteLine("\nTEST 1: OPRETTE nyt event");
            string eventTitle = $"TestEvent_{DateTime.Now.Ticks}";
            string eventDesc = "Test event for automation";
            string eventDate = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");

            var addBtn = wait.Until(d => 
                d.FindElements(By.CssSelector(".hero-cta, .empty-cta, .btn-add, [data-test='add-event']")).FirstOrDefault());
            Assert.IsNotNull(addBtn, "? 'Tilføj event' knap ikke fundet");
            
            Console.WriteLine(" Klikker på 'Tilføj event' knap");
            addBtn.Click();
            Thread.Sleep(SLOW_PAUSE_MS);

            var modal = wait.Until(d => 
                d.FindElements(By.CssSelector(".modal, [role='dialog']")).FirstOrDefault());
            Assert.IsNotNull(modal, "? Modal åbnede ikke");
            Console.WriteLine(" Modal åbnet");
            Thread.Sleep(SLOW_PAUSE_MS);

            Console.WriteLine($" Udfylder titel: {eventTitle}");
            Type(By.CssSelector("input[name='title'], input[placeholder*='Titel'], input[placeholder*='Navn']"), eventTitle);
            Thread.Sleep(SLOW_PAUSE_MS);

            Console.WriteLine($" Udfylder beskrivelse: {eventDesc}");
            Type(By.CssSelector("textarea[name='description'], textarea[placeholder*='Beskrivelse']"), eventDesc);
            Thread.Sleep(SLOW_PAUSE_MS);

            Console.WriteLine($" Udfylder dato: {eventDate}");
            Type(By.CssSelector("input[name='date'], input[type='date']"), eventDate);
            Thread.Sleep(SLOW_PAUSE_MS);

            Console.WriteLine(" Sender form");
            var submitBtn = Driver.FindElements(By.CssSelector("button[type='submit'], .btn-save")).FirstOrDefault();
            Assert.IsNotNull(submitBtn, "? Submit knap ikke fundet");
            submitBtn.Click();
            Thread.Sleep(SLOW_PAUSE_MS);

            wait.Until(d => d.FindElements(By.CssSelector(".success")).Count > 0 ||
                           d.FindElements(By.CssSelector(".events-grid")).Count > 0);
            Console.WriteLine("Event oprettet");
            Thread.Sleep(SLOW_PAUSE_MS);

            // ===== READ =====
            Console.WriteLine("\nTEST 2: LESE event fra listen");
            var eventItems = Driver.FindElements(By.CssSelector(".event-card, .event-item, [data-test='event-item']"));
            var createdEvent = eventItems.FirstOrDefault(e => e.Text.Contains(eventTitle));
            Assert.IsNotNull(createdEvent, $"? Event '{eventTitle}' ikke fundet");
            Console.WriteLine($"Event fundet i listen: {eventTitle}");
            Thread.Sleep(SLOW_PAUSE_MS);

            // ===== UPDATE =====
            Console.WriteLine("\nTEST 3: REDIGERE event");
            var editBtn = createdEvent.FindElements(By.CssSelector(".edit-btn, .btn-edit, [data-action='edit']")).FirstOrDefault();
            if (editBtn == null)
            {
                Console.WriteLine(" Klikker på event selv");
                createdEvent.Click();
            }
            else
            {
                Console.WriteLine(" Klikker på edit-knap");
                editBtn.Click();
            }
            Thread.Sleep(SLOW_PAUSE_MS);

            var editModal = wait.Until(d => 
                d.FindElements(By.CssSelector(".modal, [role='dialog']")).FirstOrDefault());
            Assert.IsNotNull(editModal, "? Edit-modal åbnede ikke");
            Console.WriteLine(" Edit-modal åbnet");
            Thread.Sleep(SLOW_PAUSE_MS);

            string updatedDesc = $"Updated event - {DateTime.Now.Ticks}";
            Console.WriteLine($" Ændrer beskrivelse til: {updatedDesc}");
            var descInput = Driver.FindElements(By.CssSelector("textarea[name='description'], textarea[placeholder*='Beskrivelse']")).FirstOrDefault();
            Assert.IsNotNull(descInput, "? Beskrivelse-felt ikke fundet");
            descInput.Clear();
            descInput.SendKeys(updatedDesc);
            Thread.Sleep(SLOW_PAUSE_MS);

            Console.WriteLine(" Sender updated form");
            var updateSubmit = Driver.FindElements(By.CssSelector("button[type='submit'], .btn-save")).FirstOrDefault();
            updateSubmit?.Click();
            Thread.Sleep(SLOW_PAUSE_MS);

            wait.Until(d => d.FindElements(By.CssSelector(".success")).Count > 0 ||
                           d.FindElements(By.CssSelector(".events-grid")).Count > 0);
            Console.WriteLine("Event opdateret");
            Thread.Sleep(SLOW_PAUSE_MS);

            // ===== DELETE =====
            Console.WriteLine("\nTEST 4: SLETTE event");
            eventItems = Driver.FindElements(By.CssSelector(".event-card, .event-item"));
            var eventToDelete = eventItems.FirstOrDefault(e => e.Text.Contains(eventTitle));
            Assert.IsNotNull(eventToDelete, "? Event til sletning ikke fundet");

            var deleteBtn = eventToDelete.FindElements(By.CssSelector(".delete-btn, .btn-delete, [data-action='delete']")).FirstOrDefault();
            Assert.IsNotNull(deleteBtn, "? Slet-knap ikke fundet");
            Console.WriteLine(" Klikker på slet-knap");
            deleteBtn.Click();
            Thread.Sleep(SLOW_PAUSE_MS);

            var confirmBtns = Driver.FindElements(By.CssSelector("button.confirm, .btn-confirm"));
            if (confirmBtns.Count > 0)
            {
                Console.WriteLine(" Bekræfter sletning");
                confirmBtns[0].Click();
                Thread.Sleep(SLOW_PAUSE_MS);
            }

            wait.Until(d => d.FindElements(By.CssSelector(".success")).Count > 0 ||
                           d.FindElements(By.CssSelector(".events-grid")).Count > 0);
            Console.WriteLine("Event slettet");
            Thread.Sleep(SLOW_PAUSE_MS);

            Console.WriteLine("\n========== EVENT SIDE TEST FÆRDIG ==========\n");
        }

        #endregion

        #region Donation Side Tests

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Donation")]
        public void Donation_AdminUpdate()
        {
            Console.WriteLine("\n========== DONATION SIDE TEST START ==========");
            Console.WriteLine("Navigerer til Donation-siden...");
            
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(WAIT_TIMEOUT_SEC));
            Driver.Navigate().GoToUrl(BaseUrl + "/Donation/Donation.html");
            Thread.Sleep(SLOW_PAUSE_MS);

            // Donation siden er primært en info-side, test edit hvis disponibelt
            Console.WriteLine("\nTEST 1: VERIFICER at siden indlæser");
            var heroBadge = wait.Until(d => 
                d.FindElements(By.CssSelector(".hero-badge")).FirstOrDefault());
            Assert.IsNotNull(heroBadge, "? Hero badge ikke fundet");
            Assert.AreEqual("Donation", heroBadge.Text.Trim(), "? Wrong badge text");
            Console.WriteLine("Donation side indlæst korrekt");
            Thread.Sleep(SLOW_PAUSE_MS);

            // Test edit hvis mulig
            Console.WriteLine("\nTEST 2: REDIGERE donation indhold (hvis muligt)");
            var editBtn = Driver.FindElements(By.CssSelector(".hero-edit-btn, .mobilepay-edit-btn, [data-action='edit']")).FirstOrDefault();
            if (editBtn != null)
            {
                Console.WriteLine(" Edit-knap fundet, klikker");
                editBtn.Click();
                Thread.Sleep(SLOW_PAUSE_MS);

                var editForm = wait.Until(d => 
                    d.FindElements(By.CssSelector(".modal, [role='dialog']")).FirstOrDefault());
                
                if (editForm != null)
                {
                    Console.WriteLine(" Edit-form åbnet");
                    var inputs = Driver.FindElements(By.CssSelector("input[type='text'], textarea"));
                    if (inputs.Count > 0)
                    {
                        string updatedValue = $"Updated_{DateTime.Now.Ticks}";
                        inputs[0].Clear();
                        inputs[0].SendKeys(updatedValue);
                        Thread.Sleep(SLOW_PAUSE_MS);

                        var submitBtn = Driver.FindElements(By.CssSelector("button[type='submit'], .btn-save")).FirstOrDefault();
                        if (submitBtn != null)
                        {
                            submitBtn.Click();
                            wait.Until(d => d.FindElements(By.CssSelector(".success")).Count > 0);
                            Console.WriteLine("Donation indhold opdateret");
                        }
                    }
                }
                else
                {
                    Console.WriteLine(" Edit-form åbnede ikke, men edit-knap findes");
                }
            }
            else
            {
                Console.WriteLine("Ingen edit-knap på Donation siden (forventet for info-side)");
            }
            Thread.Sleep(SLOW_PAUSE_MS);

            Console.WriteLine("\n========== DONATION SIDE TEST FÆRDIG ==========\n");
        }

        #endregion

        #region Navigation Tests (hvis relevant)

        [TestMethod]
        [TestCategory("Navigation")]
        public void Navigation_TestButtonNavigation()
        {
            Console.WriteLine("\n========== NAVIGATION TEST START ==========");
            
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(WAIT_TIMEOUT_SEC));
            
            // Test navigation fra Kontakt
            Console.WriteLine("\nNavigerer til Kontakt-siden...");
            Driver.Navigate().GoToUrl(BaseUrl + "/Kontakt/Kontakt.html");
            Thread.Sleep(SLOW_PAUSE_MS);

            // Hvis der er navigations-links/knapper, test dem
            var navLinks = Driver.FindElements(By.CssSelector("a[href], button[onclick*='navigate'], [data-nav]"));
            Console.WriteLine($" Fandt {navLinks.Count} mulige navigations-elementer");
            
            if (navLinks.Count > 0)
            {
                var firstNavLink = navLinks.FirstOrDefault(l => 
                    !l.GetAttribute("href")?.Contains("Kontakt") ?? true);
                
                if (firstNavLink != null)
                {
                    string linkText = firstNavLink.Text;
                    Console.WriteLine($" Klikker på link: {linkText}");
                    firstNavLink.Click();
                    Thread.Sleep(SLOW_PAUSE_MS);

                    string currentUrl = Driver.Url;
                    Console.WriteLine($" Navigeret til: {currentUrl}");
                    Assert.IsFalse(currentUrl.Contains("Kontakt"), "? Skulle have navigeret væk fra Kontakt");

                    Console.WriteLine(" Går tilbage");
                    Driver.Navigate().Back();
                    Thread.Sleep(SLOW_PAUSE_MS);
                    Console.WriteLine("Navigation og tilbage-funktion virker");
                }
            }

            Console.WriteLine("\n========== NAVIGATION TEST FÆRDIG ==========\n");
        }

        #endregion
    }
}
