namespace AareonTechnicalTest.DTOs.Inbound.Notes
{
    public class UpdateNoteDto : BaseInboundNoteDto
    {
        public UpdateNoteDto(string content, int? ticketId) : base(content, ticketId)
        {
        }
    }
}