namespace Selama.Migrations
{
    using Areas.Forums.Models;
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
            //#region Roles
            //context.Roles.AddOrUpdate(new IdentityRole
            //{
            //    Name = "Admin"
            //},
            //new IdentityRole
            //{
            //    Name = "Guild Officer"
            //},
            //new IdentityRole
            //{
            //    Name = "Forum Mod"
            //},
            //new IdentityRole
            //{
            //    Name = "Guild Member"
            //});
            //#endregion

            //#region Forum sections
            //context.ForumSections.AddOrUpdate(new ForumSection
            //{
            //    Id = 1,
            //    Title = "Officers' Section",
            //    DisplayOrder = 1,
            //    IsActive = true,
            //},
            //new ForumSection
            //{
            //    Id = 2,
            //    Title = "General Section",
            //    DisplayOrder = 2,
            //    IsActive = true,
            //},
            //new ForumSection
            //{
            //    Id = 3,
            //    Title = "Members Only",
            //    DisplayOrder = 3,
            //    IsActive = true,
            //},
            //new ForumSection
            //{
            //    Id = 4,
            //    Title = "Eastern Kingdom Liberators",
            //    DisplayOrder = 4,
            //    IsActive = true,
            //},
            //new ForumSection
            //{
            //    Id = 5,
            //    Title = "Admin Section",
            //    DisplayOrder = 5,
            //    IsActive = true,
            //},
            //new ForumSection
            //{
            //    Id = 6,
            //    Title = "Lore",
            //    DisplayOrder = 6,
            //    IsActive = true,
            //});
            //#endregion

            //#region Forums
            //context.Forums.AddOrUpdate(new Forum
            //{
            //    Id = 1,
            //    Title = "Officers' Lounge",
            //    SubTitle = "Officer's only area for discussion",
            //    IsActive = true,
            //    ForumSectionId = 1,
            //},
            //new Forum
            //{
            //    Id = 2,
            //    Title = "General Discussion",
            //    SubTitle = "General discussion, open to everyone!",
            //    IsActive = true,
            //    ForumSectionId = 2,
            //},
            //new Forum
            //{
            //    Id = 3,
            //    Title = "Recruitment",
            //    SubTitle = null,
            //    IsActive = true,
            //    ForumSectionId = 2
            //},
            //new Forum
            //{
            //    Id = 4,
            //    Title = "News!",
            //    SubTitle = null,
            //    IsActive = true,
            //    ForumSectionId = 2,
            //},
            //new Forum
            //{
            //    Id = 5,
            //    Title = "Dossiers",
            //    SubTitle = "Members' profiles here. Read and learn about your fellow guildmates",
            //    IsActive = true,
            //    ForumSectionId = 3,
            //},
            //new Forum
            //{
            //    Id = 6,
            //    Title = "Helpful Tips and Guides",
            //    SubTitle = "Questions? Learn how to make a lore abiding character!",
            //    IsActive = true,
            //    ForumSectionId = 3,
            //},
            //new Forum
            //{
            //    Id = 7,
            //    Title = "Creativity Corner",
            //    SubTitle = "Creative works written by our talented members!",
            //    IsActive = true,
            //    ForumSectionId = 3,
            //},
            //new Forum
            //{
            //    Id = 8,
            //    Title = "Screenshots",
            //    SubTitle = null,
            //    IsActive = false,
            //    ForumSectionId = 3,
            //},
            //new Forum
            //{
            //    Id = 9,
            //    Title = "Synopses",
            //    SubTitle = "In game RP synopses",
            //    IsActive = true,
            //    ForumSectionId = 3,
            //},
            //new Forum
            //{
            //    Id = 10,
            //    Title = "Members Only Discussion",
            //    SubTitle = null,
            //    IsActive = true,
            //    ForumSectionId = 3,
            //},
            //new Forum
            //{
            //    Id = 11,
            //    Title = "Creativity Corner",
            //    SubTitle = null,
            //    IsActive = true,
            //    ForumSectionId = 4,
            //},
            //new Forum
            //{
            //    Id = 12,
            //    Title = "Dossiers",
            //    SubTitle = null,
            //    IsActive = true,
            //    ForumSectionId = 4,
            //},
            //new Forum
            //{
            //    Id = 13,
            //    Title = "Synopses",
            //    SubTitle = null,
            //    IsActive = true,
            //    ForumSectionId = 4,
            //},
            //new Forum
            //{
            //    Id = 14,
            //    Title = "Random Discussion",
            //    SubTitle = null,
            //    IsActive = true,
            //    ForumSectionId = 4,
            //},
            //new Forum
            //{
            //    Id = 15,
            //    Title = "Admin/Council Discussion",
            //    SubTitle = null,
            //    IsActive = true,
            //    ForumSectionId = 5,
            //},
            //new Forum
            //{
            //    Id = 16,
            //    Title = "Horde",
            //    SubTitle = null,
            //    IsActive = true,
            //    ForumSectionId = 6,
            //},
            //new Forum
            //{
            //    Id = 17,
            //    Title = "Alliance",
            //    SubTitle = null,
            //    IsActive = true,
            //    ForumSectionId = 6,
            //},
            //new Forum
            //{
            //    Id = 18,
            //    Title = "Dragons",
            //    SubTitle = null,
            //    IsActive = true,
            //    ForumSectionId = 6
            //},
            //new Forum
            //{
            //    Id = 19,
            //    Title = "Races",
            //    SubTitle = null,
            //    IsActive = true,
            //    ForumSectionId = 6
            //},
            //new Forum
            //{
            //    Id = 20,
            //    Title = "Organizations",
            //    SubTitle = null,
            //    IsActive = true,
            //    ForumSectionId = 6
            //});
            //#endregion
        }
    }
}
