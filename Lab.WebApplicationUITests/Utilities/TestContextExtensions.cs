using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lab.WebApplicationUITests.Utilities
{
    public static class TestContextExtensions
    {
        /// <summary>
        /// Finds the solution directory.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// A <see cref="string"/> instance.
        /// </returns>
        public static string FindSolutionDirectory(this TestContext context)
        {
            string startPath = context != null
                ? context.TestDir
                : Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (string.IsNullOrWhiteSpace(startPath))
            {
                throw new InvalidOperationException
                (
                    "No reference point was determined in order to search for the solution directory."
                );
            }

            var directory = new DirectoryInfo(startPath);

            while (directory.Exists)
            {
                var solutionFiles = directory.EnumerateFiles("*.sln", SearchOption.TopDirectoryOnly);

                if (solutionFiles.Any())
                {
                    return directory.FullName;
                }

                if (directory.Parent == null)
                {
                    throw new InvalidOperationException("Failed to identify the solution directory.");
                }

                directory = directory.Parent;
            }

            throw new InvalidOperationException("Failed to identify the solution directory.");
        }
    }

}