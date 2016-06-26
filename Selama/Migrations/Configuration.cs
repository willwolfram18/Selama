namespace Selama.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Selama.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Selama.Models.ApplicationDbContext";
        }

        protected override void Seed(Selama.Models.ApplicationDbContext context)
        {
            //context.Roles.AddOrUpdate(new IdentityRole
            //{
            //    Name = "Admin"
            //},
            //new IdentityRole
            //{
            //    Name = "User"
            //});

            context.Roles.AddOrUpdate(new IdentityRole
            {
                Name = "ForumMod"
            },
            new IdentityRole
            {
                Name = "GuildOfficer"
            });

            //IdentityRole admin = context.Roles.Where(r => r.Name == "Admin").FirstOrDefault();
            //IdentityRole user = context.Roles.Where(r => r.Name == "User").FirstOrDefault();
            //ApplicationUser billy = context.Users.Where(u => u.UserName == "billy@example.com").FirstOrDefault();
            //ApplicationUser billy2 = context.Users.Where(u => u.UserName == "billy2@example.com").FirstOrDefault();

            //billy2.Roles.Add(new IdentityUserRole()
            //{
            //    RoleId = admin.Id,
            //    UserId = billy2.Id
            //});
            //billy.Roles.Add(new IdentityUserRole()
            //{
            //    RoleId = user.Id,
            //    UserId = billy.Id
            //});

            //context.ThreadReplies.ToList().ForEach(r =>
            //{
            //    r.IsActive = true;
            //    context.ThreadReplies.AddOrUpdate(r);
            //});
        }
    }
}
