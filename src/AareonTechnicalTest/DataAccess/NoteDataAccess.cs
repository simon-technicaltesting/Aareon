using AareonTechnicalTest.Exceptions;
using AareonTechnicalTest.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AareonTechnicalTest.DataAccess
{
    public interface INoteDataAccess
    {
        Task<Note> AddNote(Note noteToAdd);
        Task<Note> GetNote(int noteId);
        Task<Note> UpdateNote(Note noteToUpdate, string noteContent, Ticket relatedTicket, int updatedById);
        Task DeleteNote(int noteId);
    }

    public class NoteDataAccess : INoteDataAccess
    {
        private readonly ApplicationContext _context;

        public NoteDataAccess(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Note> AddNote(Note noteToAdd)
        {
            _context.Add(noteToAdd);
            await _context.SaveChangesAsync();
            return noteToAdd;
        }
        
        public async Task<Note> GetNote(int noteId)
        {
            return
                await _context
                    .Notes
                    .FirstOrDefaultAsync(t => t.Id == noteId);
        }

        public async Task<Note> UpdateNote(Note noteToUpdate, string noteContent, Ticket relatedTicket, int updatedById)
        {
            noteToUpdate.Update(noteContent, relatedTicket, updatedById);
            await _context.SaveChangesAsync();
            return noteToUpdate;
        }
        
        public async Task DeleteNote(int noteId)
        {
            var matchedNote = await GetNote(noteId);
            if (matchedNote == null)
            {
                throw new EntityNotFoundException();
            }
        
            _context.Notes.Remove(matchedNote);
            await _context.SaveChangesAsync();
        }
    }
}