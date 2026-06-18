using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SomaliskDanskForening_Test.Selenium
{
    public abstract class SeleniumBase
    {
        protected static readonly string BaseUrl = Environment.GetEnvironmentVariable("SDF_BASE_URL") ?? "http://127.0.0.1:5500";
        protected static string AdminUser => Environment.GetEnvironmentVariable("SDF_ADMIN_USER") ?? "SomaliskDanskForening";
        protected static string AdminPass => Environment.GetEnvironmentVariable("SDF_ADMIN_PASS") ?? "SoomaaliiyooHolbæk";
        protected static readonly string LoginUrl = BaseUrl + "/Login/index.html";

        // Statisk delt driver for alle tests
        private static IWebDriver? _sharedDriver;
        private static bool _isLoggedIn = false;

        protected IWebDriver Driver
        {
            get
            {
                if (_sharedDriver == null)
                {
                    InitSharedDriver();
                }
                return _sharedDriver;
            }
        }

        private static void InitSharedDriver()
        {
            var options = new ChromeOptions();
            if (Environment.GetEnvironmentVariable("SDF_HEADLESS") == "1")
                options.AddArgument("--headless=new");
            
            // Tilføj fullscreen ved opstart
            options.AddArgument("--start-maximized");
            options.AddArgument("--no-first-run");
            options.AddArgument("--no-default-browser-check");
            
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            
            _sharedDriver = new ChromeDriver(options);
            
            // Sikr at vinduet er maksimeret
            _sharedDriver.Manage().Window.Maximize();
            
            _sharedDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            _sharedDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
        }

        protected void InitDriver()
        {
            // Driver bruges nu via property - intet at gøre her
        }

        protected void QuitDriver()
        {
            // Lukker kun når alle tests er færdige
        }

        public static void QuitSharedDriver()
        {
            _sharedDriver?.Quit();
            _sharedDriver?.Dispose();
            _sharedDriver = null;
            _isLoggedIn = false;
        }

        // Skriver i et felt med simpel retry-logik
        protected void Type(By by, string text)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            var element = wait.Until(d => d.FindElement(by));
            element.Clear();
            element.SendKeys(text);
            Thread.Sleep(300); // Lille delay for at sikre input er registreret
        }

        // Klikker på et element
        protected void Click(By by)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            var element = wait.Until(d => d.FindElement(by));
            element.Click();
            Thread.Sleep(300);
        }

        protected void LoginAsAdmin()
        {
            // Spring kun over hvis vi allerede er logget ind OG ikke står på login-siden.
            // (Ellers springer en dedikeret login-test selve login'et over pga. den delte static-tilstand.)
            if (_isLoggedIn && !Driver.Url.Contains("/Login"))
            {
                return;
            }

            Driver.Navigate().GoToUrl(LoginUrl);
            Thread.Sleep(1000);

            Type(By.Id("username"), AdminUser);
            Type(By.Id("password"), AdminPass);
            Click(By.CssSelector("button[type='submit']"));

            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
            wait.Until(d =>
                d.FindElements(By.CssSelector(".message-box.success")).Count > 0 ||
                !d.Url.Contains("/Login/index.html"));

            _isLoggedIn = true;
        }
    }
}
