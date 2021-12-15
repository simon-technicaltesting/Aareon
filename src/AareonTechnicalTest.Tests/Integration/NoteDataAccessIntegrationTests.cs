using AareonTechnicalTest.DataAccess;
using AareonTechnicalTest.Models;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace AareonTechnicalTest.Tests.Integration
{
    public class NoteDataAccessIntegrationTests : IntegrationTestBase
    {
        // TODO Add all other tests...would normally aim for TDD approach...

        [Test]
        public async Task UpdateNote_ValidUpdateAddingToTicket_DbUpdatedCorrectly()
        {
            const string updatedContentText = "Updated content linking to ticket";

            var ticketForNote = new Ticket
            {
                Content = "Ticket to have note added",
                PersonId = PersonOne.Id
            };
            var noteToUpdate = new Note
            {
                Content = "Note to update and ticket to ticket",
            };
            DbContext.AddRange(ticketForNote, noteToUpdate);
            await DbContext.SaveChangesAsync();


            var noteDataAccess = new NoteDataAccess(DbContext);

            await noteDataAccess.UpdateNote(noteToUpdate, updatedContentText, ticketForNote, PersonOne.Id);

            // Bypass data access to ensure db has actual changes
            var noteInDb = await DbContext.Notes.FindAsync(noteToUpdate.Id);
            Assert.IsNotNull(noteInDb);
            Assert.AreEqual(updatedContentText, noteInDb.Content);
            Assert.AreEqual(ticketForNote.Id, noteInDb.Ticket.Id);
        }

        [Test]
        public async Task UpdateNote_ValidUpdateRemovingFromTicket_DbUpdatedCorrectly()
        {
            const string updatedContentText = "Updated content removing ticket link";
            var noteToUpdate = TicketWithNote.Notes.First();

            await new NoteDataAccess(DbContext).UpdateNote(noteToUpdate, updatedContentText, null, PersonOne.Id);

            // Bypass data access to ensure db has actual changes
            var noteInDb = await DbContext.Notes.FindAsync(noteToUpdate.Id);
            Assert.IsNotNull(noteInDb);
            Assert.AreEqual(updatedContentText, noteInDb.Content);
            Assert.IsNull(noteInDb.Ticket);
        }
    }
}