using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SuggestionBoard.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuggestionBoard.Data
{
    public class SuggestionBoardDbContext : IdentityDbContext<User, Role, Guid>
    {
        public static readonly ILoggerFactory ConsoleLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddFilter((category, level) =>
                category == DbLoggerCategory.Database.Command.Name
                && level == LogLevel.Information)
            .AddConsole();
        });

        public SuggestionBoardDbContext(DbContextOptions<SuggestionBoardDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("Users");
            builder.Entity<Role>().ToTable("Roles");
        }

        public virtual DbSet<SuggestionBase> Suggestions { get; set; }
        public virtual DbSet<SuggestionReaction> SuggestionReactions { get; set; }
        public virtual DbSet<SuggestionComment> SuggestionComments { get; set; }
    }
}
