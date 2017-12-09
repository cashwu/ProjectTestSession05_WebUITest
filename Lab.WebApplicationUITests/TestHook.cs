using System.IO;
using Lab.WebApplicationUITests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lab.WebApplicationUITests
{
    [TestClass]
    public class TestHook
    {
        /// <summary>
        /// Stores the IIS Express reference entity.
        /// </summary>
        private static IISExpressHelper _iisExpress;

        /// <summary>
        /// Initializes the assembly for running unit tests.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            var solutionDirectory = context.FindSolutionDirectory();
            var projectDirectory = Path.Combine(solutionDirectory, "Lab.WebApplication");
            var address = TestConfig.BaseWebAddress;

            _iisExpress = new IISExpressHelper();
            _iisExpress.Start(projectDirectory, address.Port, address);
        }

        /// <summary>
        /// Cleans up after running the unit tests in an assembly.
        /// </summary>
        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            _iisExpress.Dispose();
            _iisExpress = null;
        }
    }
}