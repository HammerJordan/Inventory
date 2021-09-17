using System;
using System.IO;
using Xunit;
using FluentAssertions;

namespace Inventory.DataAccess.Tests
{
    public class DbConnectionTests
    {
        [Fact]
        public void Should_CreateNewDB_When_NoDbFound()
        {
            var myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string path = @$"{myDocuments}\Inventory\TestInventoryDB.db";
            if (File.Exists(path))
                File.Delete(path);

            var dbConnection = new DbConnection(path);

            File.Exists(path).Should().BeTrue();
        }
    }
}