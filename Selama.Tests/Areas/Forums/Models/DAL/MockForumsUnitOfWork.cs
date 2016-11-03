using Microsoft.AspNet.Identity;
using Selama.Areas.Forums.Models;
using Selama.Areas.Forums.Models.DAL;
using Selama.Areas.Forums.ViewModels;
using Selama.Models;
using Selama.Models.DAL;
using Selama.Tests.Models.DAL;
using System;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Selama.Tests.Areas.Forums.Models.DAL
{
    public class MockForumsUnitOfWork : IForumsUnitOfWork
    {
        public IGenericEntityRepository<Selama.Areas.Forums.Models.Forum> Forums { get; private set; }
        public IGenericEntityRepository<ForumSection> ForumSections { get; private set; }
        public IGenericEntityRepository<ApplicationUser> Identities { get; private set; }
        public IGenericEntityRepository<ThreadReply> ThreadReplies { get; private set; }
        public IGenericEntityRepository<Thread> Threads { get; private set; }
        public IGenericEntityRepository<GuildNewsFeedItem> GuildNewsFeedItems { get; private set; }

        public MockForumsUnitOfWork()
        {
            Forums = new MockGenericEntityRepository<Selama.Areas.Forums.Models.Forum>();
            ForumSections = new MockGenericEntityRepository<ForumSection>();
            Identities = new MockGenericEntityRepository<ApplicationUser>();
            ThreadReplies = new MockGenericEntityRepository<ThreadReply>();
            Threads = new MockGenericEntityRepository<Thread>();
            GuildNewsFeedItems = new MockGenericEntityRepository<GuildNewsFeedItem>();
        }

        public async Task AddNewThreadToNewsFeedAsync(Thread thread, string threadUrl)
        {
            if (thread.Author == null)
            {
                thread.Author = await Identities.FindByIdAsync(thread.AuthorID);
            }
            GuildNewsFeedItems.Add(new GuildNewsFeedItem(thread, threadUrl));
        }

        public Thread CreateNewThread(ThreadViewModel threadToCreate, IPrincipal author, int forumId)
        {
            Thread t = new Thread(threadToCreate, author.Identity.GetUserId(), forumId);
            Threads.Add(t);
            return t;
        }

        public void DeleteThread(Thread thread)
        {
            thread.IsActive = false;
            foreach (var reply in ThreadReplies.Get(r => r.ThreadID == thread.Id))
            {
                reply.IsActive = false;
                ThreadReplies.Update(reply);
            }
            Threads.Update(thread);
        }

        public void Dispose()
        {
        }

        public void Reload(object entity)
        {
            if (entity is Thread)
            {
                SetReloadObject(entity, Threads);
            }
            else if (entity is ThreadReply)
            {
                SetReloadObject(entity, ThreadReplies);
            }
            else if (entity is Selama.Areas.Forums.Models.Forum)
            {
                SetReloadObject(entity, Forums);
            }
            else if (entity is ForumSection)
            {
                SetReloadObject(entity, ForumSections);
            }
            else if (entity is ApplicationUser)
            {
                SetReloadObject(entity, Identities);
            }
            else if (entity is GuildNewsFeedItem)
            {
                SetReloadObject(entity, GuildNewsFeedItems);
            }
            else
            {
                throw new NotImplementedException("No implemented for type " + entity.GetType().ToString());
            }
        }

        private void SetReloadObject<T>(object entity, IGenericEntityRepository<T> repo)
            where T : class
        {
            T entityToReturn = entity as T;
            foreach (var t in repo.Get())
            {
                if (entityToReturn == t || GetObjectId(entity) == GetObjectId(t))
                {
                    entity = t;
                    break;
                }
            }
        }

        private object GetObjectId<T>(T entity)
        {
            return entity.GetType().GetProperty("ID").GetValue(entity);
        }

        public Task ReloadAsync(object entity)
        {
            try
            {
                Reload(entity);
            }
            catch
            {
                return Task.FromResult(1);
            }
            return Task.FromResult(0);
        }

        public void SaveChanges()
        {
        }

        public Task SaveChangesAsync()
        {
            return Task.FromResult(0);
        }

        public bool TrySaveChanges()
        {
            return true;
        }

        public Task<bool> TrySaveChangesAsync()
        {
            return Task.FromResult<bool>(true);
        }
    }
}
