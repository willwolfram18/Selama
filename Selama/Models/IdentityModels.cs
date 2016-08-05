using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Selama.Areas.Forums.Models;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Selama.Areas.Admin.ViewModels.Users;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System;

namespace Selama.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public bool IsActive { get; set; }

        [Required]
        public bool WaitingReview { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }

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

        public async Task UpdateFromViewModel(UserEditViewModel user)
        {
            IsActive = user.IsActive;
            if (user.RoleId != Roles.FirstOrDefault().RoleId)
            {
                using (var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>())
                {
                    using (var db = new ApplicationDbContext())
                    {
                        var currentRole = db.Roles.Find(Roles.FirstOrDefault().RoleId);
                        var newRole = db.Roles.Find(user.RoleId);
                        await userManager.RemoveFromRoleAsync(Id, currentRole.Name);
                        await userManager.AddToRoleAsync(Id, newRole.Name);

                        // Need to perform an update of the user's version after role adjustment
                        var updatedUser = db.Users.Find(Id);
                        user.Version = Convert.ToBase64String(updatedUser.Version);
                    }
                }
            }
            Convert.FromBase64String(user.Version).CopyTo(Version, 0);
        }
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