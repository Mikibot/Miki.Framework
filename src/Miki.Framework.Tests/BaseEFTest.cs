﻿using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Miki.Framework.Tests
{
    /// <summary>
    /// Creates a test environment with a single-column sqlite database.
    /// </summary>
    public class BaseEntityTest<T>
        where T : class
    {
        private readonly DbContextOptions options;

        public BaseEntityTest()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            options = new DbContextOptionsBuilder<TestContext<T>>()
                .UseSqlite(connection)
                .Options;

            using var context = NewDbContext();
            context.Database.EnsureCreated();
        }

        public IUnitOfWork NewContext()
        {
            return new UnitOfWork(NewDbContext());
        }

        protected TestContext<T> NewDbContext()
        {
            return new TestContext<T>(options, OnModelCreating);
        }

        protected virtual void OnModelCreating([NotNull] ModelBuilder builder)
        {
        }
    }
}
