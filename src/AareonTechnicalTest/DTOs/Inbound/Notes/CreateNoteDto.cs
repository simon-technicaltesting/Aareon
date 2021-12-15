namespace AareonTechnicalTest.DTOs.Inbound.Notes
{
    public class CreateNoteDto : BaseInboundNoteDto
    {
        public CreateNoteDto(string content, int? ticketId) : base(content, ticketId)
        {
        }
    }
}