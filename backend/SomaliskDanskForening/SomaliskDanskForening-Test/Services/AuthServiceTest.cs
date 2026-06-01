using SomaliskDanskForening_Lib.Services;

namespace SomaliskDanskForening_Test
{
    [TestClass]
    public class AuthServiceTest
    {
        [TestMethod]
        public void HashPassword_ReturnsNonEmptyString()
        {
            var hash = AuthService.HashPassword("TestPassword123");
            Assert.IsFalse(string.IsNullOrEmpty(hash));
        }

        [TestMethod]
        public void HashPassword_SamePasswordGivesDifferentHashes()
        {
            var hash1 = AuthService.HashPassword("TestPassword123");
            var hash2 = AuthService.HashPassword("TestPassword123");
            Assert.AreNotEqual(hash1, hash2, "Samme password skal give forskellige hashes pga. salt");
        }

        [TestMethod]
        public void HashPassword_DifferentPasswords_GiveDifferentHashes()
        {
            var hash1 = AuthService.HashPassword("Password1");
            var hash2 = AuthService.HashPassword("Password2");
            Assert.AreNotEqual(hash1, hash2);
        }

        [TestMethod]
        public void VerifyPassword_CorrectPassword_ReturnsTrue()
        {
            var password = "KorrektPassword!99";
            var hash = AuthService.HashPassword(password);
            Assert.IsTrue(AuthService.VerifyPassword(password, hash));
        }

        [TestMethod]
        public void VerifyPassword_WrongPassword_ReturnsFalse()
        {
            var hash = AuthService.HashPassword("RigtgtPassword");
            Assert.IsFalse(AuthService.VerifyPassword("ForkertPassword", hash));
        }

        [TestMethod]
        public void VerifyPassword_EmptyPassword_ReturnsFalse()
        {
            var hash = AuthService.HashPassword("EtPassword");
            Assert.IsFalse(AuthService.VerifyPassword("", hash));
        }

        [DataTestMethod]
        [DataRow("kort")]
        [DataRow("langtPasswordMedSpecialTegn!@#123")]
        [DataRow("Soomaaliiyoo")]
        public void VerifyPassword_RoundTrip_AlwaysTrue(string password)
        {
            var hash = AuthService.HashPassword(password);
            Assert.IsTrue(AuthService.VerifyPassword(password, hash));
        }
    }
}
