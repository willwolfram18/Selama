using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Selama.Areas.Forums.Models;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Selama.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        #region Navigation properties
        public virtual ICollection<Thread> Threads { get; set; }
        public virtual ICollection<ThreadReply> ThreadReplies { get; set; }
        #endregion
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Don't delete Threads/Thread Replies on user delete
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Threads)
                .WithRequired(t => t.Author)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.ThreadReplies)
                .WithRequired(t => t.Author)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Forum>()
            //    .HasMany<Thread>(f => f.Threads)
            //    .WithRequired(t => t.Forum);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        #region Forums Area Models
        public DbSet<ForumSection> ForumSections { get; set; }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<Thread> Threads { get; set; }
        public DbSet<ThreadReply> ThreadReplies { get; set; }
        #endregion
    }
}