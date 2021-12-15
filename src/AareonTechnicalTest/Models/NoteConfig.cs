using Microsoft.EntityFrameworkCore;

namespace AareonTechnicalTest.Models
{
    public static class NoteConfig
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Note>(
                entity =>
                {
                    entity.HasKey(n => n.Id);

                    entity
                        .HasOne(n => n.Ticket)
                        .WithMany(t => t.Notes)
                        .HasForeignKey("TicketId")
                        .IsRequired(false);
                    
                    // To keep table name consistent, wasn't plural by default
                    entity.ToTable("Notes");
                });
        }
    }
}