using Microsoft.EntityFrameworkCore;

namespace notepad_react.Models
{
    public class NotesContext : DbContext
    {
        public NotesContext(DbContextOptions<NotesContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<NoteCategory>()
                .HasKey(e => new { e.CategoryID, e.NoteID});
            builder.Entity<NoteCategory>()
                .HasOne(e => e.Category)
                .WithMany(c => c.NoteCategories);
            builder.Entity<NoteCategory>()
                .HasOne(e => e.Note)
                .WithMany(n => n.NoteCategories);

            builder.Entity<Note>()
                .HasIndex(e => e.Title)
                .IsUnique(true);
        }

        public DbSet<Note> Notes {set; get;}
        public DbSet<Category> Categories {set; get;}
        public DbSet<NoteCategory> NoteCategories {set; get;}
    }
}