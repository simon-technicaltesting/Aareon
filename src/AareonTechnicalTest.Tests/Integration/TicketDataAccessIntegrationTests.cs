using AareonTechnicalTest.DataAccess;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace AareonTechnicalTest.Tests.Integration
{
    public class TicketDataAccessIntegrationTests : IntegrationTestBase
    {
        [Test]
        public async Task GetTicket_ValidId_ReturnsCorrectTicket()
        {
            var ticketDataAccess = new TicketDataAccess(DbContext);

            var retrievedTicket = await ticketDataAccess.GetTicket(TicketWithNote.Id);

            Assert.IsNotNull(retrievedTicket);
            Assert.AreEqual( TicketWithNote.Content, retrievedTicket.Content);
            Assert.AreEqual( TicketWithNote.Notes.Count, retrievedTicket.Notes.Count);
            Assert.AreEqual( TicketWithNote.Notes.First().Content, retrievedTicket.Notes.First().Content);
        }
        
        [Test]
        public async Task GetTicket_IdDoesNotExist_ReturnsNull()
        {
            var ticketDataAccess = new TicketDataAccess(DbContext);
            var retrievedTicket = await ticketDataAccess.GetTicket(999);

            Assert.IsNull(retrievedTicket);
        }
    }
}