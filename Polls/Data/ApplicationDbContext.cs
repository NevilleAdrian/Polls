using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Polls.Models;

namespace Polls.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Election> Elections { get; set; }
        public DbSet<ElectionWinner> ElectionWinners { get; set; }
        public DbSet<Grieviance> Grieviances { get; set; }
        public DbSet<GrievianceReply> GrievianceReplies { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Tie> Ties { get; set; }
        public DbSet<Voter> Voters { get; set; }
    }
}
