using AareonTechnicalTest.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AareonTechnicalTest.DataAccess
{
    public interface ITicketDataAccess
    {
        Task<Ticket> GetTicket(int ticketId);
    }

    public class TicketDataAccess : ITicketDataAccess
    {
        private readonly ApplicationContext _context;

        public TicketDataAccess(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Ticket> GetTicket(int ticketId)
        {
            return
                await _context
                    .Tickets
                    .Include(t => t.Notes)
                    .FirstOrDefaultAsync(t => t.Id == ticketId);
        }
    }
}