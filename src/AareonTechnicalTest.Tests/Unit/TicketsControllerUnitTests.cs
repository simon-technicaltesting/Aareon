using AareonTechnicalTest.Controllers;
using AareonTechnicalTest.DataAccess;
using AareonTechnicalTest.DTOs;
using AareonTechnicalTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AareonTechnicalTest.Tests.Unit
{
    public class TicketsControllerUnitTests
    {
        private Mock<ITicketDataAccess> _mockTicketDataAccess = null!;
        private TicketsController _ticketsController = null!;

        [SetUp]
        public void SetUp()
        {
            _mockTicketDataAccess = new Mock<ITicketDataAccess>();
            // TODO Once reworked to use data access context wont need to be passed
            _ticketsController = new TicketsController(new NullLogger<TicketsController>(),
                _mockTicketDataAccess.Object,
                null);
        }

        [Test]
        public async Task GetTicket_ValidId_ReturnsOkWithCorrectTicket()
        {
            var testTicket = new Ticket
            {
                Content = "Test Content", PersonId = 1,
                Notes = new List<Note>
                {
                    new() { Content = "Test Note", CreatedBy = 1 }
                }
            };

            _mockTicketDataAccess
                .Setup(x => x.GetTicket(1))
                .ReturnsAsync(testTicket);

            var actionResult = await _ticketsController.GetTicket(1);
            var okResult = actionResult as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult!.StatusCode);

            Assert.IsInstanceOf(typeof(TicketDto), okResult.Value);
            var retrievedTicket = okResult.Value as TicketDto;

            Assert.AreEqual(testTicket.Content, retrievedTicket!.Content);
            Assert.AreEqual(testTicket.Notes.Count, retrievedTicket!.Notes.Count);
            Assert.AreEqual(testTicket.Notes.First().Content, retrievedTicket.Notes.First().Content);
        }

        [Test]
        public async Task GetTicket_IdDoesNotExist_ReturnsNotFound()
        {
            _mockTicketDataAccess
                .Setup(x => x.GetTicket(1))
                .ReturnsAsync(null as Ticket);

            var actionResult = await _ticketsController.GetTicket(999);
            var notFoundResult = actionResult as NotFoundResult;

            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult!.StatusCode);
        }
    }
}