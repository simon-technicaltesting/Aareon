using AareonTechnicalTest.Models;

namespace AareonTechnicalTest.DTOs
{
    public class NoteDto
    {
        public NoteDto(Note note)
        {
            Id = note.Id;
            Content = note.Content;
        }

        public int Id { get; }
        public string Content { get; }
    }
}