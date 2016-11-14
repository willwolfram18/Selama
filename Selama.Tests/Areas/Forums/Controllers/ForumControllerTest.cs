using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Selama.Areas.Forums.Controllers;
using Selama.Areas.Forums.Models;
using Selama.Areas.Forums.Models.DAL;
using Selama.Areas.Forums.ViewModels;
using Selama.Controllers;
using Selama.Models;
using Selama.Tests.Areas.Forums.Models.DAL;
using Selama.Tests.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Selama.Tests.Areas.Forums.Controllers
{
    [TestClass]
    public class ForumControllerTest : _BaseControllerUnitTest<ForumController>
    {
        #region Properties
        #region Private properties
        private const int NUM_AUTHORS = 2;
        private const int NUM_FORUM_SECTIONS = 2;
        private const int NUM_FORUMS = 2 * NUM_FORUM_SECTIONS;
        private const int NUM_THREADS = NUM_FORUMS * 3;

        private MockForumsUnitOfWork ControllerDb { get; set; }
        #endregion
        #endregion

        #region Setup tests
        protected override ForumController SetupController()
        {
            ControllerDb = new MockForumsUnitOfWork();
            return new ForumController(ControllerDb);
        }
        protected override void AdditionalSetup()
        {
            SetupAuthors();
            SetupForumSections();
            SetupForums();
            SetupThreads();
        }
        #endregion

        #region Methods
        #region Public methods
        [TestMethod]
        public void IndexIsNotNull()
        {
            #region Arrange
            #endregion

            #region Act
            ViewResult result = Controller.Index() as ViewResult;
            #endregion

            #region Assert
            Assert.IsNotNull(result);
            #endregion
        }

        #region Threads unit tests
        [TestMethod]
        public async Task ThreadsActionWithoutForumIdRedirectsToIndex()
        {
            // Arrange

            // Act
            RedirectToRouteResult result = await Controller.Threads() as RedirectToRouteResult;

            // Assert
            AssertIsRedirectToIndex(result, "Threads");
        }

        [TestMethod]
        public async Task ThreadsWithInvalidThreadIdRedirectToIndex()
        {
            // Arrange

            // Act
            RedirectToRouteResult result = await Controller.Threads(NUM_THREADS + 1) as RedirectToRouteResult;

            // Assert
            AssertIsRedirectToIndex(result, "Threads");
        }

        [TestMethod]
        public async Task ThreadsWithPageNumLessThanOneRedirectsToPageOne()
        {
            // Arrange

            // Act
            RedirectToRouteResult result = await Controller.Threads(1, 0) as RedirectToRouteResult;

            // Assert
            AssertIsRedirectToActionPageOne(result, "Threads");
        }

        [TestMethod]
        public async Task ThreadsWithPageNumGreaterThanMaximumPageNumRedirectsToLastPage()
        {
            // Arrange
            int forumId = 1;
            Forum forum = ControllerDb.Forums.FindById(forumId);
            ApplicationUser author = ControllerDb.Authors.Get().ToList()[0];
            for (int i = 0; i < ForumController.PAGE_SIZE; i++)
            {
                InsertNewThreadToForum(forum, author, i);
            }

            // Act
            RedirectToRouteResult result = await Controller.Threads(1, 3) as RedirectToRouteResult;

            // Assert
            AssertIsRedirectToSpecificActionPage(result, "Threads", 2);
        }

        [TestMethod]
        public async Task ThreadsWithInactiveForumRedirectsToIndex()
        {
            #region Arrange
            int forumId = 1;
            ControllerDb.Forums.FindById(forumId).IsActive = false;
            #endregion

            #region Act
            RedirectToRouteResult result = await Controller.Threads(forumId) as RedirectToRouteResult;
            #endregion

            #region Assert
            AssertIsRedirectToIndex(result, "Threads");
            #endregion
        }

        [TestMethod]
        public async Task ThreadsWithNoThreadsReturnsModelWithNoThreads()
        {
            #region Arrange
            int forumId = 1;
            Forum expectedForum = ControllerDb.Forums.FindById(forumId);
            expectedForum.Threads.Clear();
            #endregion

            #region Act
            ViewResult result = await Controller.Threads(forumId) as ViewResult;
            #endregion

            #region Assert
            Assert.IsNotNull(result);
            ForumViewModel Model = result.Model as ForumViewModel;
            AssertForumMatchesViewModel(expectedForum, Model);
            #endregion
        }

        [TestMethod]
        public async Task ThreadsWithOnlyInactiveThreadsGivesCorrectReslut()
        {
            #region Arrange
            int forumId = 1;
            foreach (var thread in ControllerDb.Threads.Get(t => t.ForumId == forumId))
            {
                thread.IsActive = false;
            }
            #endregion

            #region Act
            ViewResult result = await Controller.Threads(forumId) as ViewResult;
            #endregion

            #region Assert
            Assert.IsNotNull(result);
            ForumViewModel Model = result.Model as ForumViewModel;
            AssertForumMatchesViewModel(ControllerDb.Forums.FindById(forumId), Model);
            #endregion
        }

        [TestMethod]
        public async Task ThreadsWithSomeInactiveThreadsGivesCorrectResult()
        {
            #region Arrange
            int forumId = 1;
            Forum expectedForum = ControllerDb.Forums.FindById(forumId);
            expectedForum.Threads.First().IsActive = false;
            #endregion

            #region Act
            ViewResult result = await Controller.Threads(forumId) as ViewResult;
            #endregion

            #region Assert
            Assert.IsNotNull(result);
            ForumViewModel Model = result.Model as ForumViewModel;
            AssertForumMatchesViewModel(expectedForum, Model);
            #endregion
        }

        [TestMethod]
        public async Task ThreadsPagedModelGivesCorrectSetOfThreads()
        {
            #region Arrange
            int forumId = 1;
            Forum expectedForum = ControllerDb.Forums.FindById(forumId);
            ApplicationUser author = ControllerDb.Authors.Get().ToList()[0];
            for (int i = 0; i < ForumController.PAGE_SIZE + 5; i++)
            {
                InsertNewThreadToForum(expectedForum, author, i);
            }
            int numToDeactivate = 3;
            foreach (var thread in expectedForum.Threads)
            {
                thread.IsActive = false;
                numToDeactivate--;
                if (numToDeactivate == 0)
                {
                    break;
                }
            }
            #endregion

            #region Act & Assert
            await AssertForumViewModelForSpecificPageMatchesExpected(expectedForum, 1, ForumController.PAGE_SIZE);
            await AssertForumViewModelForSpecificPageMatchesExpected(expectedForum, 2, expectedForum.GetThreads().Count() - ForumController.PAGE_SIZE);
            #endregion
        }

        [TestMethod]
        public async Task ThreadsWithValidIdReturnsCorrectResult()
        {
            #region Arrange
            int forumId = 1;
            Forum expectedForum = ControllerDb.Forums.FindById(forumId);
            #endregion

            #region Act
            ViewResult result = await Controller.Threads(forumId) as ViewResult;
            #endregion

            #region Assert
            Assert.IsNotNull(result);
            ForumViewModel Model = result.Model as ForumViewModel;
            AssertForumMatchesViewModel(expectedForum, Model);
            #endregion
        }
        #endregion

        #region Thread unit tests
        [TestMethod]
        public async Task ThreadWithoutIdRedirectsToIndex()
        {
            #region Arrange
            #endregion

            #region Act
            RedirectToRouteResult result = await Controller.Thread() as RedirectToRouteResult;
            #endregion

            #region Assert
            AssertIsRedirectToIndex(result, "Thread");
            #endregion
        }

        [TestMethod]
        public async Task ThreadWithInvalidIdRedirectsToIndex()
        {
            // Arrange

            // Act
            RedirectToRouteResult result = await Controller.Thread(NUM_THREADS + 1) as RedirectToRouteResult;

            // Assert
            AssertIsRedirectToIndex(result, "Thread");
        }

        [TestMethod]
        public async Task ThreadWithInactiveThreadResultsInRedirectToIndex()
        {
            // Arrange
            int threadId = 1;
            ControllerDb.Threads.FindById(threadId).IsActive = false;

            // Act
            RedirectToRouteResult result = await Controller.Thread(threadId) as RedirectToRouteResult;

            // Assert
            AssertIsRedirectToIndex(result, "Thread");
        }

        [TestMethod]
        public async Task ThreadWithBelowOnePageNumRedirectsToPageOne()
        {
            #region Arrange
            #endregion

            #region Act
            RedirectToRouteResult result = await Controller.Thread(1, 0) as RedirectToRouteResult;
            #endregion

            #region Assert
            AssertIsRedirectToActionPageOne(result, "Thread");
            #endregion
        }

        [TestMethod]
        public async Task ThreadWithPageNumGreaterThanMaximumPageNumRedirectsToLastPage()
        {
            #region Arrange
            int threadId = 1;
            Thread t = ControllerDb.Threads.FindById(threadId);
            for (int i = 0; i < ForumController.PAGE_SIZE + 2; i++)
            {
                InsertReplyToThread(t, i);
            }
            #endregion

            #region Act
            RedirectToRouteResult result = await Controller.Thread(threadId, 3) as RedirectToRouteResult;
            #endregion

            #region Assert
            AssertIsRedirectToSpecificActionPage(result, "Thread", 2);
            #endregion
        }

        [TestMethod]
        public async Task ThreadWithNegativeOnePageNumberRedirectsToLastPage()
        {
            #region Arrange
            int threadId = 1;
            Thread t = ControllerDb.Threads.FindById(threadId);
            for (int i = 0; i < ForumController.PAGE_SIZE + 2; i++)
            {
                InsertReplyToThread(t, i);
            }
            #endregion

            #region Act
            RedirectToRouteResult result = await Controller.Thread(threadId, -1) as RedirectToRouteResult;
            #endregion

            #region Assert
            AssertIsRedirectToSpecificActionPage(result, "Thread", 2);
            #endregion
        }

        [TestMethod]
        public async Task ThreadPagedResultsAreCorrect()
        {
            #region Arrange
            int threadId = 1;
            Thread expectedThread = ControllerDb.Threads.FindById(threadId);
            for (int i = 0; i < ForumController.PAGE_SIZE + 5; i++)
            {
                InsertReplyToThread(expectedThread, i);
                if (i < 3)
                {
                    ThreadReply toBeDisabled = expectedThread.Replies.Where(r => r.ReplyIndex == i).FirstOrDefault();
                    toBeDisabled.IsActive = false;
                }
            }
            #endregion

            #region Act & Assert
            // subtract 1 because thread content itself counts as an item for the page
            await AssertThreadViewModelForSpecificPageMatchesExpected(expectedThread, 1, ForumController.PAGE_SIZE - 1);
            // Add 1 here because the threasd, again, counts for the page items
            await AssertThreadViewModelForSpecificPageMatchesExpected(expectedThread, 2, expectedThread.GetReplies().Count() + 1 - ForumController.PAGE_SIZE);
            #endregion
        }

        [TestMethod]
        public async Task ThreadWithValidIdReturnsCorrectResult()
        {
            #region Arrange
            int threadId = 1;
            Thread expectedThread = ControllerDb.Threads.FindById(threadId);
            #endregion

            #region Act
            ViewResult result = await Controller.Thread(threadId) as ViewResult;
            #endregion

            #region Assert
            Assert.IsNotNull(result);
            ThreadViewModel Model = result.Model as ThreadViewModel;
            Assert.IsNotNull(Model);
            Assert.AreEqual(expectedThread.Title, Model.Title);
            Assert.AreEqual(expectedThread.Content, Model.Content);
            Assert.AreEqual(expectedThread.GetReplies().Count(), Model.Replies.Count);
            #endregion
        }
        #endregion

        #region PostReply
        [TestMethod]
        public async Task PostReplyToInvalidThreadIdGivesBadRequest()
        {
            #region Arrange
            int threadId = 0;
            ThreadReplyViewModel reply = CreatePostedReplyForThread(threadId);
            #endregion

            #region Act
            PartialViewResult result = await Controller.PostReply(reply, threadId);
            #endregion

            #region Assert
            AssertPostReplyIsBadRequest(reply, result);
            #endregion
        }

        [TestMethod]
        public async Task PostReplyToInactiveThreadGivesBadRequest()
        {
            #region Arrange
            int threadId = 1;
            Thread threadToPostTo = ControllerDb.Threads.FindById(threadId);
            threadToPostTo.IsActive = false;
            ThreadReplyViewModel reply = CreatePostedReplyForThread(threadId);
            #endregion

            #region Act
            PartialViewResult result = await Controller.PostReply(reply, threadId);
            #endregion

            #region Assert
            Assert.IsNotNull(result);
            MockReponse.VerifySet(r => r.StatusCode = _ControllerBase.HttpBadRequestStatus);
            Assert.AreEqual("EditorTemplates/ThreadReplyViewModel", result.ViewName);
            Assert.AreEqual(reply, result.Model);
            #endregion
        }

        [TestMethod]
        public async Task PostReplyToLockedThreadGivesBadRequest()
        {
            #region Arrange
            int threadId = 1;
            Thread threadToPostTo = ControllerDb.Threads.FindById(threadId);
            threadToPostTo.IsLocked = true;
            ThreadReplyViewModel reply = CreatePostedReplyForThread(threadId);
            #endregion

            #region Act
            PartialViewResult result = await Controller.PostReply(reply, threadId);
            #endregion

            #region Assert
            Assert.IsNotNull(result);
            MockReponse.VerifySet(r => r.StatusCode = _ControllerBase.HttpBadRequestStatus);
            Assert.AreEqual("EditorTemplates/ThreadReplyViewModel", result.ViewName);
            Assert.AreEqual(reply, result.Model);
            #endregion
        }

        [TestMethod]
        public async Task PostReplyToMismatchedThreadGivesBadRequest()
        {
            #region Arrange
            int threadId = 1;
            int replyThreadId = 2;
            ThreadReplyViewModel reply = CreatePostedReplyForThread(replyThreadId);
            #endregion

            #region Act
            PartialViewResult result = await Controller.PostReply(reply, threadId);
            #endregion

            #region Assert
            AssertPostReplyIsBadRequest(reply, result);
            #endregion
        }

        [TestMethod]
        public async Task PostReplyPlacesValueInThread()
        {
            #region Arrange
            int threadId = 1;
            ThreadReplyViewModel reply = CreatePostedReplyForThread(threadId);
            Thread threadPostedTo = ControllerDb.Threads.FindById(threadId);
            int startingNumReplies = ControllerDb.ThreadReplies.Get().Count();
            ApplicationUser replyAuthor = ControllerDb.Authors.Get().ToList()[0];
            MockUser.Setup(c => c.Identity).Returns(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, replyAuthor.Id)
            }));
            #endregion

            #region Act
            PartialViewResult result = await Controller.PostReply(reply, threadId);
            #endregion

            #region Assert
            Assert.IsNotNull(result);
            ThreadReplyViewModel Model = result.Model as ThreadReplyViewModel;
            Assert.AreEqual(reply.Content, Model.Content);
            Assert.AreEqual(replyAuthor.UserName, Model.Author);
            Assert.AreEqual(startingNumReplies + 1, ControllerDb.ThreadReplies.Get().Count());
            #endregion
        }
        #endregion

        #region EditThread
        #endregion
        #endregion

        #region Private methods
        private void SetupAuthors()
        {
            for (int i = 0; i < NUM_AUTHORS; i++)
            {
                ControllerDb.Authors.Add(new ApplicationUser
                {
                    IsActive = true,
                    UserName = "App User " + i.ToString(),
                    Email = string.Format("appuser{0}@email.com", i),
                    EmailConfirmed = true,
                    ThreadReplies = new List<ThreadReply>(),
                    WaitingReview = false,
                    Threads = new List<Thread>(),
                });
            }
        }
        private void SetupForumSections()
        {
            for (int i = 0; i < NUM_FORUM_SECTIONS; i++)
            {
                ControllerDb.ForumSections.Add(new ForumSection
                {
                    DisplayOrder = i,
                    IsActive = true,
                    Title = "Forum Section " + i.ToString(),
                    Forums = new List<Forum>(),
                });
            }
        }
        private void SetupForums()
        {
            for (int i = 0; i < NUM_FORUMS; i++)
            {
                int forumSectionId = (i % NUM_FORUM_SECTIONS) + 1;
                ForumSection section = ControllerDb.ForumSections.FindById(forumSectionId);
                Forum forum = new Forum
                {
                    ForumSectionId = forumSectionId,
                    ForumSection = section,
                    IsActive = true,
                    Title = "Forum " + i.ToString(),
                    SubTitle = "Subtitle for forum " + i.ToString(),
                    Threads = new List<Thread>(),
                };
                section.Forums.Add(forum);
                ControllerDb.Forums.Add(forum);
            }
        }
        private void SetupThreads()
        {
            for (int i = 0; i < NUM_THREADS; i++)
            {
                int forumId = (i % NUM_FORUMS) + 1;
                Forum forum = ControllerDb.Forums.FindById(forumId);
                ApplicationUser author = ControllerDb.Authors.Get().ToList()[i % NUM_AUTHORS];
                Thread thread = new Thread
                {
                    Content = string.Format("This is a sample paragraph for thread {0} in forum {1}.", i, forumId),
                    Forum = forum,
                    ForumId = forumId,
                    IsActive = true,
                    IsLocked = false,
                    IsPinned = false,
                    PostDate = DateTime.Now.AddMinutes(-i),
                    Title = "Thread " + i.ToString(),
                    Replies = new List<ThreadReply>(),
                    Author = author,
                    AuthorId = author.Id
                };
                forum.Threads.Add(thread);
                ControllerDb.Threads.Add(thread);
            }
        }

        private void AssertIsRedirectToIndex(RedirectToRouteResult result, string redirectFromAction)
        {
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"].ToString());
            Assert.AreEqual(redirectFromAction, result.RouteValues["redirectFrom"].ToString());
        }

        private void AssertIsRedirectToActionPageOne(RedirectToRouteResult result, string actionRedirectTarget)
        {
            AssertIsRedirectToSpecificActionPage(result, actionRedirectTarget, 1);
        }
        private void AssertIsRedirectToSpecificActionPage(RedirectToRouteResult result, string actionRedirectTarget, int targetPageNum)
        {
            Assert.IsNotNull(result);
            Assert.AreEqual(actionRedirectTarget, result.RouteValues["action"].ToString());
            Assert.AreEqual(targetPageNum, Convert.ToInt32(result.RouteValues["page"]));
        }

        private void InsertNewThreadToForum(Forum forum, ApplicationUser author, int counter)
        {
            Thread tempThread = new Thread
            {
                Content = string.Format("A sample paragraph for thread {0} in forum {1}", counter, forum.Id),
                IsActive = true,
                IsLocked = false,
                IsPinned = false,
                PostDate = DateTime.Now.AddHours(-counter),
                Title = "Extra Thread " + counter.ToString(),
                Author = author,
                AuthorId = author.Id,
                Forum = forum,
                ForumId = forum.Id,
                Replies = new List<ThreadReply>(),
            };
            ControllerDb.Threads.Add(tempThread);
            forum.Threads.Add(tempThread);
        }

        private void AssertForumMatchesViewModel(Forum expectedForum, ForumViewModel Model)
        {
            Assert.IsNotNull(Model);
            Assert.AreEqual(expectedForum.Title, Model.Title);
            Assert.AreEqual(expectedForum.SubTitle, Model.SubTitle);
            Assert.AreEqual(expectedForum.GetThreads().Count(), Model.Threads.Count());
        }
        private async Task AssertForumViewModelForSpecificPageMatchesExpected(Forum expectedForum, int page, int expectedThreadCount)
        {
            ViewResult result = await Controller.Threads(expectedForum.Id, page) as ViewResult;

            Assert.IsNotNull(result);
            ForumViewModel Model = result.Model as ForumViewModel;
            Assert.AreEqual(expectedForum.Title, Model.Title);
            Assert.AreEqual(expectedForum.SubTitle, Model.SubTitle);
            Assert.AreEqual(page, Model.ViewPageNum);

            var orderedThreadsFromExpected = expectedForum.GetThreads()
                .OrderByDescending(t => t.PostDate)
                .Skip(ForumController.PAGE_SIZE * (page - 1))
                .Take(ForumController.PAGE_SIZE)
                .ToList();
            var viewmodelThreads = Model.Threads.ToList();
            Assert.AreEqual(expectedThreadCount, viewmodelThreads.Count);
            for (int i = 0; i < expectedThreadCount; i++)
            {
                Thread expectedThread = orderedThreadsFromExpected[i];
                ThreadOverviewViewModel viewModel = viewmodelThreads[i];
                Assert.AreEqual(expectedThread.Title, viewModel.Title);
                Assert.AreEqual(expectedThread.Id, viewModel.ID);
            }
        }

        private void InsertReplyToThread(Thread thread, int replyIndex)
        {
            var reply = new ThreadReply
            {
                Thread = thread,
                ThreadId = thread.Id,
                Author = thread.Author,
                AuthorId = thread.AuthorId,
                Content = "Reply " + replyIndex + " to thread " + thread.Title,
                IsActive = true,
                PostDate = DateTime.Now.AddMinutes(replyIndex),
                ReplyIndex = replyIndex,
            };
            thread.Replies.Add(reply);
            ControllerDb.ThreadReplies.Add(reply);
        }

        private async Task AssertThreadViewModelForSpecificPageMatchesExpected(Thread expectedThread, int page, int expectedReplyCount)
        {
            ViewResult result = await Controller.Thread(expectedThread.Id, page) as ViewResult;

            Assert.IsNotNull(result);
            ThreadViewModel Model = result.Model as ThreadViewModel;
            Assert.IsNotNull(Model);
            Assert.AreEqual(expectedThread.Title, Model.Title);
            Assert.AreEqual(expectedThread.IsLocked, Model.IsLocked);
            Assert.AreEqual(expectedThread.IsPinned, Model.IsPinned);
            Assert.AreEqual(expectedThread.Content, Model.Content);
            Assert.AreEqual(expectedReplyCount, Model.Replies.Count);
        }

        private ThreadReplyViewModel CreatePostedReplyForThread(int threadId)
        {
            return new ThreadReplyViewModel
            {
                Content = "This is a posted reply for thread " + threadId.ToString(),
                ThreadID = threadId,
            };
        }

        private void AssertPostReplyIsBadRequest(ThreadReplyViewModel reply, PartialViewResult result)
        {
            Assert.IsNotNull(result);
            MockReponse.VerifySet(r => r.StatusCode = _ControllerBase.HttpBadRequestStatus);
            Assert.AreEqual("EditorTemplates/ThreadReplyViewModel", result.ViewName);
            Assert.AreEqual(reply, result.Model);
        }
        #endregion
        #endregion
    }
}
