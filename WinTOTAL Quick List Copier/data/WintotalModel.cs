namespace WinTOTAL_Quick_List_Copier.data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class WintotalModel : DbContext
    {
        public WintotalModel(string connectionString)
            : base(connectionString)
        {
            // Disable DB initialization
            Database.SetInitializer<WintotalModel>(null);
        }

        public virtual DbSet<QuickList> QuickLists { get; set; }
        public virtual DbSet<QuickListEntry> QuickListEntries { get; set; }
        public virtual DbSet<QuickListName> QuickListNames { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuickList>()
                .Property(e => e.FieldName)
                .IsUnicode(false);

            modelBuilder.Entity<QuickList>()
                .Property(e => e.FormType)
                .IsUnicode(false);

            modelBuilder.Entity<QuickList>()
                .Property(e => e.ShortName)
                .IsUnicode(false);

            modelBuilder.Entity<QuickListEntry>()
                .Property(e => e.FieldName)
                .IsUnicode(false);

            modelBuilder.Entity<QuickListEntry>()
                .Property(e => e.EntryText)
                .IsUnicode(false);

            modelBuilder.Entity<QuickListName>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<QuickListName>()
                .HasMany(e => e.QuickLists)
                .WithRequired(e => e.QuickListName)
                .WillCascadeOnDelete(false);
        }
    }
}
