using System;
using Lab.Repository.Tests.DB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lab.Repository.Tests
{
    [TestClass]
    public class TestHook
    {
        internal static string ConnectionString => string.Format(TestDbConnection.LocalDb.LocalDbConnectionString, "SampleDB");

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            // SampleDB
            var sampleDatabase = new TestDatabase("SampleDB");
            if (sampleDatabase.IsLocalDbExists())
            {
                sampleDatabase.DeleteLocalDb();
            }
            sampleDatabase.CreateDatabase();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            var defaultDatabase = new TestDatabase("master");
            defaultDatabase.DeleteLocalDb(ConnectionString);
        }
    }
}