using Microsoft.VisualStudio.TestTools.UnitTesting;
using SomaliskDanskForening_Test.Selenium;

[assembly: Parallelize(Scope = ExecutionScope.MethodLevel)]

namespace SomaliskDanskForening_Test
{
    [TestClass]
    public class TestAssemblyCleanup
    {
        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            SeleniumBase.QuitSharedDriver();
        }
    }
}
