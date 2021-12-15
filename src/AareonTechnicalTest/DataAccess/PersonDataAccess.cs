using AareonTechnicalTest.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AareonTechnicalTest.DataAccess
{
    public interface IPersonDataAccess
    {
        Task<Person> GetPerson(int personId);
    }

    public class PersonDataAccess : IPersonDataAccess
    {
        private readonly ApplicationContext _context;

        public PersonDataAccess(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Person> GetPerson(int personId)
        {
            return
                await _context
                    .Persons
                    .FirstOrDefaultAsync(t => t.Id == personId);
        }
    }
}