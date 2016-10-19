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

namespace Selama.Tests.Areas.Forum.Models.DAL
{
    public class MockForumsUnitOfWork : IForumsUnitOfWork
    {
        public IGenericEntityRepository<Selama.Areas.Forums.Models.Forum> ForumRepository { get; private set; }
        public IGenericEntityRepository<ForumSection> ForumSectionRepository { get; private set; }
        public IGenericEntityRepository<ApplicationUser> IdentityRepository { get; private set; }
        public IGenericEntityRepository<ThreadReply> ThreadReplyRepository { get; private set; }
        public IGenericEntityRepository<Thread> ThreadRepository { get; private set; }
        public IGenericEntityRepository<GuildNewsFeedItem> GuildNewsFeedRepository { get; private set; }

        public MockForumsUnitOfWork()
        {
            ForumRepository = new MockGenericEntityRepository<Selama.Areas.Forums.Models.Forum>();
            ForumSectionRepository = new MockGenericEntityRepository<ForumSection>();
            IdentityRepository = new MockGenericEntityRepository<ApplicationUser>();
            ThreadReplyRepository = new MockGenericEntityRepository<ThreadReply>();
            ThreadRepository = new MockGenericEntityRepository<Thread>();
            GuildNewsFeedRepository = new MockGenericEntityRepository<GuildNewsFeedItem>();
        }

        public async Task AddNewThreadToNewsFeedAsync(Thread thread, string threadUrl)
        {
            if (thread.Author == null)
            {
                thread.Author = await IdentityRepository.FindByIdAsync(thread.AuthorID);
            }
            GuildNewsFeedRepository.Add(new GuildNewsFeedItem(thread, threadUrl));
        }

        public Thread CreateNewThread(ThreadViewModel threadToCreate, IPrincipal author, int forumId)
        {
            Thread t = new Thread(threadToCreate, author.Identity.GetUserId(), forumId);
            ThreadRepository.Add(t);
            return t;
        }

        public void DeleteThread(Thread thread)
        {
            thread.IsActive = false;
            foreach (var reply in ThreadReplyRepository.Get(r => r.ThreadID == thread.Id))
            {
                reply.IsActive = false;
                ThreadReplyRepository.Update(reply);
            }
            ThreadRepository.Update(thread);
        }

        public void Dispose()
        {
        }

        public void Reload(object entity)
        {
            if (entity is Thread)
            {
                SetReloadObject(entity, ThreadRepository);
            }
            else if (entity is ThreadReply)
            {
                SetReloadObject(entity, ThreadReplyRepository);
            }
            else if (entity is Selama.Areas.Forums.Models.Forum)
            {
                SetReloadObject(entity, ForumRepository);
            }
            else if (entity is ForumSection)
            {
                SetReloadObject(entity, ForumSectionRepository);
            }
            else if (entity is ApplicationUser)
            {
                SetReloadObject(entity, IdentityRepository);
            }
            else if (entity is GuildNewsFeedItem)
            {
                SetReloadObject(entity, GuildNewsFeedRepository);
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
