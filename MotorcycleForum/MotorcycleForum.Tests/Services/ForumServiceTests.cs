using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using MotorcycleForum.Data;
using MotorcycleForum.Data.Entities;
using MotorcycleForum.Services.Forum;
using MotorcycleForum.Services.Models.Forum;
using MotorcycleForum.Services;
using MotorcycleForum.Data.Entities.Forum;
using MotorcycleForum.Services.Models.Requests;

namespace MotorcycleForum.Tests.Services.Forum
{
    [TestFixture]
    public class ForumServiceTests
    {
        private ForumService _forumService;
        private MotorcycleForumDbContext _context;
        private Mock<UserManager<User>> _userManagerMock;
        private Mock<S3Service> _s3ServiceMock;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<MotorcycleForumDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new MotorcycleForumDbContext(options);

            var store = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            _s3ServiceMock = new Mock<S3Service>("fakeKey", "fakeSecret", "fakeBucket", "fakeRegion");

            _forumService = new ForumService(_context, _userManagerMock.Object, _s3ServiceMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetTopicSelectListAsync_ReturnsEmpty_WhenNoTopicsExist()
        {
            var result = await _forumService.GetTopicSelectListAsync(null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetTopicSelectListAsync_ReturnsTopics_WhenTopicsExist()
        {
            _context.ForumTopics.Add(new Data.Entities.Forum.ForumTopic { TopicId = 1, Title = "Test Topic" });
            await _context.SaveChangesAsync();

            var result = await _forumService.GetTopicSelectListAsync(1);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Cast<SelectListItem>().First().Text, Is.EqualTo("Test Topic"));
        }

        [Test]
        public async Task GetForumPostsAsync_ReturnsEmpty_WhenNoPostsExist()
        {
            var result = await _forumService.GetForumPostsAsync(null, null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetForumPostsAsync_ReturnsFilteredPosts_WhenPostsExist()
        {
            var author = new User
            {
                Id = Guid.NewGuid(),
                FullName = "Jane Doe",
                Email = "jane@example.com",
                UserName = "janedoe",
                ProfilePictureUrl = "/images/jane.png",
                RegistrationDate = DateTime.UtcNow
            };

            var topic = new ForumTopic
            {
                TopicId = 1,
                Title = "Tech"
            };

            var post = new ForumPost
            {
                ForumPostId = Guid.NewGuid(),
                Title = "Interesting Title",
                Content = "Some content",
                AuthorId = author.Id,
                Author = author,
                TopicId = topic.TopicId,
                Topic = topic,
                CreatedDate = DateTime.UtcNow
            };

            _context.Users.Add(author);
            _context.ForumTopics.Add(topic);
            _context.ForumPosts.Add(post);
            await _context.SaveChangesAsync();

            // Act
            var result = await _forumService.GetForumPostsAsync(null, "Interesting");

            // Assert
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.First().Title, Is.EqualTo("Interesting Title"));
        }

        [Test]
        public async Task GetCreateViewModelAsync_ReturnsViewModelWithTopics()
        {
            _context.ForumTopics.AddRange(
                new Data.Entities.Forum.ForumTopic { TopicId = 1, Title = "General" },
                new Data.Entities.Forum.ForumTopic { TopicId = 2, Title = "Mechanical" }
            );
            await _context.SaveChangesAsync();
            var viewModel = await _forumService.GetCreateViewModelAsync();

            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.Topics, Is.Not.Null);
            Assert.That(viewModel.Topics.Count(), Is.EqualTo(2));
            Assert.That(viewModel.Topics.First().Text, Is.EqualTo("General"));
        }

        [Test]
        public async Task CreatePostAsync_CreatesPost_WhenValidModelAndUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = "Jane Doe",
                Email = "jane@example.com",
                UserName = "janedoe",
                ProfilePictureUrl = "/images/jane.png",
                RegistrationDate = DateTime.UtcNow
            };
            _userManagerMock.Setup(m => m.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(user);

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

            _context.Users.Add(user);
            _context.ForumTopics.Add(new Data.Entities.Forum.ForumTopic { TopicId = 1, Title = "Test" });
            await _context.SaveChangesAsync();

            var model = new ForumPostCreateViewModel
            {
                Title = "Test Post",
                Content = "This is a test post.",
                TopicId = 1
            };

            // Act
            var result = await _forumService.CreatePostAsync(model, claimsPrincipal);

            // Assert
            var createdPost = await _context.ForumPosts.FirstOrDefaultAsync(p => p.Title == "Test Post");
            Assert.That(result, Is.Not.Null);
            Assert.That(createdPost, Is.Not.Null);
            Assert.That(createdPost.Content, Is.EqualTo("This is a test post."));
            Assert.That(createdPost.AuthorId, Is.EqualTo(userId));
        }

        [Test]
        public async Task GetEditViewModelAsync_ReturnsModel_WhenUserIsAuthor()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = "Jane Doe",
                Email = "jane@example.com",
                UserName = "janedoe",
                ProfilePictureUrl = "/images/jane.png",
                RegistrationDate = DateTime.UtcNow
            };

            var forumTopic = new Data.Entities.Forum.ForumTopic { TopicId = 1, Title = "General" };
            var post = new Data.Entities.Forum.ForumPost
            {
                ForumPostId = postId,
                Title = "Edit Me",
                Content = "Editable content",
                AuthorId = userId,
                TopicId = 1,
                Images = new List<Data.Entities.Forum.ForumPostImage>
                {
                    new Data.Entities.Forum.ForumPostImage { ImageId = Guid.NewGuid(), ImageUrl = "https://s3.fake.com/image.jpg" }
                }
            };

            _context.Users.Add(user);
            _context.ForumTopics.Add(forumTopic);
            _context.ForumPosts.Add(post);
            await _context.SaveChangesAsync();

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

            var result = await _forumService.GetEditViewModelAsync(postId, claimsPrincipal);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo("Edit Me"));
            Assert.That(result.Content, Is.EqualTo("Editable content"));
            Assert.That(result.ExistingImageUrls.First(), Is.EqualTo("https://s3.fake.com/image.jpg"));
        }

        [Test]
        public async Task UpdatePostAsync_UpdatesPost_WhenValidModelAndUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                FullName = "Jane Doe",
                Email = "jane@example.com",
                UserName = "janedoe",
                ProfilePictureUrl = "/images/jane.png",
                RegistrationDate = DateTime.UtcNow
            };
            var forumTopic = new Data.Entities.Forum.ForumTopic { TopicId = 1, Title = "General" };
            var post = new Data.Entities.Forum.ForumPost
            {
                ForumPostId = postId,
                Title = "Old Title",
                Content = "Old Content",
                AuthorId = userId,
                TopicId = 1
            };

            _context.Users.Add(user);
            _context.ForumTopics.Add(forumTopic);
            _context.ForumPosts.Add(post);
            await _context.SaveChangesAsync();

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

            var model = new ForumPostEditViewModel
            {
                PostId = postId, Title = "New Title", Content = "New Content", TopicId = 1
            };

            // Act
            var result = await _forumService.UpdatePostAsync(postId, model, claimsPrincipal);

            // Assert
            var updatedPost = await _context.ForumPosts.FindAsync(postId);
            Assert.That(result, Is.True);
            Assert.That(updatedPost, Is.Not.Null);
            Assert.That(updatedPost.Title, Is.EqualTo("New Title"));
            Assert.That(updatedPost.Content, Is.EqualTo("New Content"));
        }
        
        [Test]
        public async Task DeletePostAsync_DeletesPost_WhenUserIsAuthorOrAdmin()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                FullName = "Jane Doe",
                Email = "jane@example.com",
                UserName = "janedoe",
                ProfilePictureUrl = "/images/jane.png",
                RegistrationDate = DateTime.UtcNow
            };
            var forumTopic = new Data.Entities.Forum.ForumTopic { TopicId = 1, Title = "General" };
            var post = new Data.Entities.Forum.ForumPost
            {
                ForumPostId = postId,
                Title = "Post to Delete",
                Content = "Content to Delete",
                AuthorId = userId,
                TopicId = 1
            };

            _context.Users.Add(user);
            _context.ForumTopics.Add(forumTopic);
            _context.ForumPosts.Add(post);
            await _context.SaveChangesAsync();

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()), new Claim(ClaimTypes.Role, "Admin")
            }));

            // Act
            var result = await _forumService.DeletePostAsync(postId, claimsPrincipal);

            // Assert
            var deletedPost = await _context.ForumPosts.FindAsync(postId);
            Assert.That(result, Is.True);
            Assert.That(deletedPost, Is.Null);
        }

        [Test]
        public async Task GetDetailsViewModelAsync_ReturnsModelWithCommentsAndReplies()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                FullName = "Jane Doe",
                Email = "jane@example.com",
                UserName = "janedoe",
                ProfilePictureUrl = "/images/jane.png",
                RegistrationDate = DateTime.UtcNow
            };
            var forumTopic = new Data.Entities.Forum.ForumTopic { TopicId = 1, Title = "General" };
            var post = new Data.Entities.Forum.ForumPost
            {
                ForumPostId = postId,
                Title = "Test Post",
                Content = "Test Content",
                AuthorId = userId,
                TopicId = 1
            };
            var comment = new Comment
            {
                CommentId = Guid.NewGuid(),
                Content = "Test Comment",
                AuthorId = userId,
                ForumPostId = postId,
                CreatedDate = DateTime.UtcNow
            };
            var reply = new Comment
            {
                CommentId = Guid.NewGuid(),
                Content = "Test Reply",
                AuthorId = userId,
                ForumPostId = postId,
                ParentCommentId = comment.CommentId,
                CreatedDate = DateTime.UtcNow
            };

            _context.Users.Add(user);
            _context.ForumTopics.Add(forumTopic);
            _context.ForumPosts.Add(post);
            _context.Comments.Add(comment);
            _context.Comments.Add(reply);
            await _context.SaveChangesAsync();

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

            // Act
            var result = await _forumService.GetDetailsViewModelAsync(postId, claimsPrincipal);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Comments.Count, Is.EqualTo(1));
            Assert.That(result.Comments.First().Content, Is.EqualTo("Test Comment"));
            Assert.That(result.Comments.First().Replies.Count, Is.EqualTo(1));
            Assert.That(result.Comments.First().Replies.First().Content, Is.EqualTo("Test Reply"));
        }
        
        [Test]
        public async Task AddCommentAsync_AddsComment_WhenValidRequestAndUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                FullName = "Jane Doe",
                Email = "jane@example.com",
                UserName = "janedoe",
                ProfilePictureUrl = "/images/jane.png",
                RegistrationDate = DateTime.UtcNow
            };
            var post = new ForumPost
            {
                ForumPostId = postId,
                Title = "Test Post",
                Content = "Test Content",
                AuthorId = userId,
                TopicId = 1,
                Comments = new List<Comment>()
            };
            _context.Users.Add(user);
            _context.ForumPosts.Add(post);
            await _context.SaveChangesAsync();

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

            _userManagerMock
                .Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(user);

            var request = new AddCommentRequest { PostId = postId, Content = "This is a test comment." };

            // Act
            var result = await _forumService.AddCommentAsync(request, claimsPrincipal);

            // Assert
            Assert.That(result.success, Is.True);
            Assert.That(result.commentId, Is.Not.Null);
            var comment = await _context.Comments.FindAsync(result.commentId);
            Assert.That(comment.Content, Is.EqualTo("This is a test comment."));
            Assert.That(comment.AuthorId, Is.EqualTo(userId));
        }

        [Test]
        public async Task DeleteCommentAsync_DeletesComment_WhenUserIsAuthorOrAdmin()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var commentId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                FullName = "Jane Doe",
                Email = "jane@example.com",
                UserName = "janedoe",
                ProfilePictureUrl = "/images/jane.png",
                RegistrationDate = DateTime.UtcNow
            };
            var post = new ForumPost
            {
                ForumPostId = Guid.NewGuid(),
                Title = "Test Post",
                Content = "Test Content",
                AuthorId = userId,
                TopicId = 1,
                Comments = new List<Comment>()
            };
            var comment = new Comment
            {
                CommentId = commentId,
                Content = "Comment to Delete",
                AuthorId = userId,
                ForumPostId = post.ForumPostId,
                CreatedDate = DateTime.UtcNow
            };

            _context.Users.Add(user);
            _context.ForumPosts.Add(post);
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()), new Claim(ClaimTypes.Role, "Admin")
            }));

            _userManagerMock
                .Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(user);

            var request = new DeleteCommentRequest { CommentId = commentId };

            // Act
            var result = await _forumService.DeleteCommentAsync(request, claimsPrincipal);

            // Assert
            var deletedComment = await _context.Comments.FindAsync(commentId);
            Assert.That(result.success, Is.True);
            Assert.That(deletedComment, Is.Null);
        }
        
        [Test]
        public async Task UpvotePostAsync_UpdatesUpvotes_WhenUserUpvotesPost()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                FullName = "Jane Doe",
                Email = "jane@example.com",
                UserName = "janedoe",
                ProfilePictureUrl = "/images/jane.png",
                RegistrationDate = DateTime.UtcNow
            };
            var post = new ForumPost
            {
                ForumPostId = postId,
                Title = "Test Post",
                Content = "Test Content",
                AuthorId = userId,
                TopicId = 1
            };
            _context.Users.Add(user);
            _context.ForumPosts.Add(post);
            await _context.SaveChangesAsync();
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));
            // Act
            var result = await _forumService.UpvotePostAsync(postId, claimsPrincipal);
            // Assert
            Assert.That(result.success, Is.True);
            Assert.That(result.upvotes, Is.EqualTo(1));
            Assert.That(result.downvotes, Is.EqualTo(0));
        }

        [Test]
        public async Task DownvotePostAsync_UpdatesDownvotes_WhenUserDownvotesPost()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                FullName = "Jane Doe",
                Email = "jane@example.com",
                UserName = "janedoe",
                ProfilePictureUrl = "/images/jane.png",
                RegistrationDate = DateTime.UtcNow
            };
            var post = new ForumPost
            {
                ForumPostId = postId,
                Title = "Test Post",
                Content = "Test Content",
                AuthorId = userId,
                TopicId = 1
            };
            _context.Users.Add(user);
            _context.ForumPosts.Add(post);
            await _context.SaveChangesAsync();
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));
            // Act
            var result = await _forumService.DownvotePostAsync(postId, claimsPrincipal);
            // Assert
            Assert.That(result.success, Is.True);
            Assert.That(result.upvotes, Is.EqualTo(0));
            Assert.That(result.downvotes, Is.EqualTo(1));
        }

        [Test]
        public async Task ReplyToCommentAsync_AddsReply_WhenValidRequestAndUser()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var commentId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                FullName = "Jane Doe",
                Email = "jane@example.com",
                UserName = "janedoe",
                ProfilePictureUrl = "/images/jane.png",
                RegistrationDate = DateTime.UtcNow
            };
            var post = new ForumPost
            {
                ForumPostId = Guid.NewGuid(),
                Title = "Test Post",
                Content = "Test Content",
                AuthorId = userId,
                TopicId = 1
            };
            var comment = new Comment
            {
                CommentId = commentId,
                Content = "Test Comment",
                AuthorId = userId,
                ForumPostId = post.ForumPostId,
                CreatedDate = DateTime.UtcNow
            };
            _context.Users.Add(user);
            _context.ForumPosts.Add(post);
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));
            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            var request = new ReplyCommentRequest { ParentCommentId = commentId, Content = "This is a test reply." };
            //Act
            var result = await _forumService.ReplyToCommentAsync(request, claimsPrincipal);
            //Assert
            Assert.That(result.success, Is.True);
            Assert.That(result.replyId, Is.Not.Null);
            var reply = await _context.Comments.FindAsync(result.replyId);
            Assert.That(reply.Content, Is.EqualTo("This is a test reply."));
            Assert.That(reply.AuthorId, Is.EqualTo(userId));
        }
        
        [Test]
        public async Task GetDeleteConfirmationAsync_ReturnsModel_WhenUserIsAuthorOrAdmin()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                FullName = "Jane Doe",
                Email = "jane@example.com",
                UserName = "janedoe",
                ProfilePictureUrl = "/images/jane.png",
                RegistrationDate = DateTime.UtcNow
            };
            var forumTopic = new Data.Entities.Forum.ForumTopic { TopicId = 1, Title = "General" };
            var post = new Data.Entities.Forum.ForumPost
            {
                ForumPostId = postId,
                Title = "Post to Delete",
                Content = "Content to Delete",
                AuthorId = userId,
                TopicId = 1
            };

            _context.Users.Add(user);
            _context.ForumTopics.Add(forumTopic);
            _context.ForumPosts.Add(post);
            await _context.SaveChangesAsync();

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()), new Claim(ClaimTypes.Role, "Admin")
            }));

            // Act
            var result = await _forumService.GetDeleteConfirmationAsync(postId, claimsPrincipal);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo("Post to Delete"));
            Assert.That(result.Content, Is.EqualTo("Content to Delete"));
        }
        
        [Test]
        public async Task GetDeleteConfirmationAsync_ReturnsNull_WhenPostDoesNotExist()
        {
            var result = await _forumService.GetDeleteConfirmationAsync(Guid.NewGuid(), null);
            Assert.That(result, Is.Null);
        }
        
        [Test]
        public async Task GetDeleteConfirmationAsync_ReturnsNull_WhenUserIsNotAuthorOrAdmin()
        {
            var userId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                FullName = "Jane Doe",
                Email = "jane@example.com",
                UserName = "janedoe",
                ProfilePictureUrl = "/images/jane.png",
                RegistrationDate = DateTime.UtcNow
            };
            var forumTopic = new Data.Entities.Forum.ForumTopic { TopicId = 1, Title = "General" };
            var post = new Data.Entities.Forum.ForumPost
            {
                ForumPostId = postId,
                Title = "Post to Delete",
                Content = "Content to Delete",
                AuthorId = Guid.NewGuid(),
                TopicId = 1
            };

            _context.Users.Add(user);
            _context.ForumTopics.Add(forumTopic);
            _context.ForumPosts.Add(post);
            await _context.SaveChangesAsync();

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

            var result = await _forumService.GetDeleteConfirmationAsync(postId, claimsPrincipal);

            Assert.That(result, Is.Null);
        }
        
        [Test]
        public async Task DeletePostAsync_ReturnsFalse_WhenPostDoesNotExist()
        {
            var result = await _forumService.DeletePostAsync(Guid.NewGuid(), null);
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeletePostAsync_ReturnsFalse_WhenUserIsNotAuthorOrAdmin()
        {
            var userId = Guid.NewGuid();
            var postId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                FullName = "Jane Doe",
                Email = "jane@example.com",
                UserName = "janedoe",
                ProfilePictureUrl = "/images/jane.png",
                RegistrationDate = DateTime.UtcNow
            };
            var forumTopic = new Data.Entities.Forum.ForumTopic { TopicId = 1, Title = "General" };
            var post = new Data.Entities.Forum.ForumPost
            {
                ForumPostId = postId,
                Title = "Post to Delete",
                Content = "Content to Delete",
                AuthorId = Guid.NewGuid(),
                TopicId = 1
            };

            _context.Users.Add(user);
            _context.ForumTopics.Add(forumTopic);
            _context.ForumPosts.Add(post);
            await _context.SaveChangesAsync();

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

            var result = await _forumService.DeletePostAsync(postId, claimsPrincipal);

            Assert.That(result, Is.False);
        }
        
        
    }
}
