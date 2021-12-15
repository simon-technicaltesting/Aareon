using AareonTechnicalTest.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;

namespace AareonTechnicalTest.Tests.Integration
{
    public abstract class IntegrationTestBase
    {
        protected ApplicationContext DbContext = null!;

        // Sets up a new DB for each test
        [SetUp]
        public void SetUp()
        {
            DbContext = new ApplicationContext(new DbContextOptionsBuilder<ApplicationContext>()
                .UseSqlite("Filename=Test.db")
                .Options);

            Seed();
        }

        private void Seed()
        {
            DbContext.Database.EnsureDeleted();
            DbContext.Database.EnsureCreated();

            PersonOne = new Person
            {
                Forename = "Joe",
                Surname = "Bloggs",
            };
            DbContext.AddRange(PersonOne);
            //
            TicketWithNote = new Ticket
            {
                Content = "Ticket 1 Test Content",
                PersonId = PersonOne.Id,
                Notes = new List<Note>
                {
                    new()
                    {
                        Content = "Note 1 Test Content",
                    }
                }
            };
            DbContext.AddRange(TicketWithNote);
            //
            DbContext.SaveChanges();
        }

        protected Person PersonOne { get; private set; } = null!;
        protected Ticket TicketWithNote { get; private set; } = null!;

        [TearDown]
        public void TearDown()
        {
            DbContext.Dispose();
        }
    }
}