namespace WinTOTAL_Quick_List_Copier.data
{
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Infrastructure.DependencyResolution;
    using System.Data.SqlClient;

    [DbConfigurationType(typeof(EntityFrameworkDbConfiguration))]
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


    /// <summary>
    /// A configuration class for SQL Server that specifies SQL 2005 compatability.
    /// </summary>
    internal sealed class EntityFrameworkDbConfiguration : DbConfiguration
    {
        /// <summary>
        /// The provider manifest token to use for SQL Server.
        /// </summary>
        private const string SqlServerManifestToken = @"2005";

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkDbConfiguration"/> class.
        /// </summary>
        public EntityFrameworkDbConfiguration()
        {
            this.AddDependencyResolver(new SingletonDependencyResolver<IManifestTokenResolver>(new ManifestTokenService()));
        }

        /// <inheritdoc />
        private sealed class ManifestTokenService : IManifestTokenResolver
        {
            /// <summary>
            /// The default token resolver.
            /// </summary>
            private static readonly IManifestTokenResolver DefaultManifestTokenResolver = new DefaultManifestTokenResolver();

            /// <inheritdoc />
            public string ResolveManifestToken(DbConnection connection)
            {
                if (connection is SqlConnection)
                {
                    return SqlServerManifestToken;
                }

                return DefaultManifestTokenResolver.ResolveManifestToken(connection);
            }
        }
    }
}
