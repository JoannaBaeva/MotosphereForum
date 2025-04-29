using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using MotorcycleForum.Data;
using MotorcycleForum.Data.Entities;
using MotorcycleForum.Services.Admin;
using MotorcycleForum.Services;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;
using MotorcycleForum.Data.Entities.Event_Tracker;
using MotorcycleForum.Data.Entities.Forum;

namespace MotorcycleForum.Tests.Services.Admin
{
    [TestFixture]
    public class AdminServiceTests
    {
        private MotorcycleForumDbContext _context;
        private Mock<UserManager<User>> _userManagerMock;
        private Mock<RoleManager<IdentityRole<Guid>>> _roleManagerMock;
        private Mock<IS3Service> _s3ServiceMock;
        private AdminService _adminService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<MotorcycleForumDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new MotorcycleForumDbContext(options);

            var userStore = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(
                userStore.Object, null, null, null, null, null, null, null, null
            );

            var roleStore = new Mock<IRoleStore<IdentityRole<Guid>>>();
            _roleManagerMock = new Mock<RoleManager<IdentityRole<Guid>>>(
                roleStore.Object, null, null, null, null
            );

            _s3ServiceMock = new Mock<IS3Service>();

            _adminService = new AdminService(
                _context,
                _userManagerMock.Object,
                _roleManagerMock.Object,
                _s3ServiceMock.Object
            );
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetDashboardStatsAsync_ReturnsCorrectCounts()
        {
            // Arrange
            _context.Users.Add(new User
            {
                Id = Guid.NewGuid(),
                FullName = "User A",
                ProfilePictureUrl = "/img.jpg",
                RegistrationDate = DateTime.UtcNow
            });

            _context.ForumPosts.Add(new Data.Entities.Forum.ForumPost
            {
                ForumPostId = Guid.NewGuid(),
                Title = "Post",
                Content = "Test content",
                AuthorId = Guid.NewGuid(),
                TopicId = 1,
                CreatedDate = DateTime.UtcNow
            });

            _context.MarketplaceListings.Add(new Data.Entities.Marketplace.MarketplaceListing(Guid.NewGuid())
            {
                Title = "Bike",
                Description = "Fast",
                Price = 1000,
                Location = "Sofia",
                CategoryId = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                SellerPhoneNumber = "123456789"
            });

            _context.Events.Add(new Data.Entities.Event_Tracker.Event
            {
                EventId = Guid.NewGuid(),
                Title = "Ride",
                Description = "Event description",
                EventDate = DateTime.UtcNow,
                Location = "Sofia",
                OrganizerId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid()
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _adminService.GetDashboardStatsAsync();

            // Assert
            Assert.That(result.TotalUsers, Is.EqualTo(1));
            Assert.That(result.ForumPostsCount, Is.EqualTo(1));
            Assert.That(result.MarketplaceListingsCount, Is.EqualTo(1));
            Assert.That(result.EventsCount, Is.EqualTo(1));
        }


        [Test]
        public async Task CreateMarketplaceCategoryAsync_CreatesCategory()
        {
            // Act
            var result = await _adminService.CreateMarketplaceCategoryAsync("Gear");

            // Assert
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "Gear");
            Assert.That(result, Is.True);
            Assert.That(category, Is.Not.Null);
        }

        [Test]
        public async Task DeleteMarketplaceCategoryAsync_ReturnsFalse_WhenNotFound()
        {
            // Act
            var result = await _adminService.DeleteMarketplaceCategoryAsync(Guid.NewGuid());

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeleteMarketplaceCategoryAsync_ReturnsTrue_WhenFound()
        {
            // Arrange
            var name = "TestCategory";
            await _adminService.CreateMarketplaceCategoryAsync(name);
            var createdCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);

            // Act
            var result = await _adminService.DeleteMarketplaceCategoryAsync(createdCategory.CategoryId);

            // Assert
            Assert.That(result, Is.True);
        }


        [Test]
        public async Task DeleteForumTopicAsync_ReturnsTrue_WhenFound()
        {
            // Arrange
            await _adminService.CreateForumTopicAsync("TestTopic");

            // Act
            var result = await _adminService.DeleteForumTopicAsync(1);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task DeleteForumTopicAsync_ReturnsFalse_WhenNotFound()
        {
            // Act
            var result = await _adminService.DeleteForumTopicAsync(1);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeletePostDataAsync_DeletesPostData()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var topicId = await _adminService.CreateForumTopicAsync("Test Topic");
            var post = new ForumPost
            {
                ForumPostId = postId,
                Title = "Test Post",
                Content = "Test Content",
                AuthorId = Guid.NewGuid(),
                TopicId = 1
            };

            await _context.ForumPosts.AddAsync(post);
            await _context.SaveChangesAsync();

            // Act
            await _adminService.DeletePostDataAsync(postId);

            // Assert
            var postAfterDelete = await _context.ForumPosts.FirstOrDefaultAsync(p => p.ForumPostId == postId);
            Assert.That(postAfterDelete, Is.Null);
        }

        [Test]
        public async Task DeletePostDataAsync_DeletesPostData_WithImages()
        {
            // Arrange
            var postId = Guid.NewGuid();
            await _adminService.CreateForumTopicAsync("Test Topic");

            var topicId = 1;

            var post = new ForumPost
            {
                ForumPostId = postId,
                Title = "Test Post",
                Content = "Test Content",
                AuthorId = Guid.NewGuid(),
                TopicId = topicId,
                Images = new List<ForumPostImage>
                {
                    new ForumPostImage { ImageUrl = "https://s3.fake.com/test1.jpg" },
                    new ForumPostImage { ImageUrl = "https://s3.fake.com/test2.jpg" }
                }
            };

            await _context.ForumPosts.AddAsync(post);
            await _context.SaveChangesAsync();

            _s3ServiceMock.Setup(s => s.DeleteFileAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            await _adminService.DeletePostDataAsync(postId);

            // Assert
            var postAfterDelete = await _context.ForumPosts.FirstOrDefaultAsync(p => p.ForumPostId == postId);
            Assert.That(postAfterDelete, Is.Null);
        }

        [Test]
        public async Task DeletePostDataAsync_DeletesPostData_WithoutImages()
        {
            // Arrange
            var postId = Guid.NewGuid();
            await _adminService.CreateForumTopicAsync("Test Topic");
            var topicId = 1;
            var post = new ForumPost
            {
                ForumPostId = postId,
                Title = "Test Post",
                Content = "Test Content",
                AuthorId = Guid.NewGuid(),
                TopicId = topicId
            };

            await _context.ForumPosts.AddAsync(post);
            await _context.SaveChangesAsync();

            // Act
            await _adminService.DeletePostDataAsync(postId);

            // Assert
            var postAfterDelete = await _context.ForumPosts.FirstOrDefaultAsync(p => p.ForumPostId == postId);
            Assert.That(postAfterDelete, Is.Null);
        }

        [Test]
        public async Task BanUserAsync_BansUser()
        {
            // Arrange
            var email = "testuser@example.com";
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                FullName = "Test User",
                UserName = "testuser",
                ProfilePictureUrl = "/images/default.png",
                RegistrationDate = DateTime.UtcNow
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _adminService.BanUserAsync(email);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(await _context.BannedEmails.AnyAsync(b => b.Email == email), Is.True);
        }

        [Test]
        public async Task UnbanUserAsync_UnbansUser()
        {
            // Arrange
            var email = "testuser@example.com";
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                FullName = "Test User",
                UserName = "testuser",
                ProfilePictureUrl = "/images/default.png",
                RegistrationDate = DateTime.UtcNow
            };

            await _context.Users.AddAsync(user);
            _context.BannedEmails.Add(new BannedEmail
            {
                Id = Guid.NewGuid(),
                Email = email
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _adminService.UnbanUserAsync(email);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(await _context.BannedEmails.AnyAsync(b => b.Email == email), Is.False);
        }

        [Test]
        public async Task GetPendingEventsAsync_ReturnsPendingEvents()
        {
            // Arrange
            var organizerId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();

            var organizer = new User
            {
                Id = organizerId,
                FullName = "Organizer Test",
                Email = "organizer@test.com",
                UserName = "organizer",
                ProfilePictureUrl = "/images/organizer.png",
                RegistrationDate = DateTime.UtcNow
            };

            var category = new EventCategory
            {
                CategoryId = categoryId,
                Name = "Test Category"
            };

            await _context.Users.AddAsync(organizer);
            await _context.EventCategories.AddAsync(category);
            await _context.SaveChangesAsync();

            var pendingEvent = new Event
            {
                EventId = Guid.NewGuid(),
                Title = "Pending Event",
                Description = "This is a pending event.",
                EventDate = DateTime.UtcNow.AddDays(10),
                Location = "Sofia",
                OrganizerId = organizerId,
                CategoryId = categoryId,
                IsApproved = false // Important
            };

            await _context.Events.AddAsync(pendingEvent);
            await _context.SaveChangesAsync();

            // Act
            var result = await _adminService.GetPendingEventsAsync();

            // Assert
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(1));
            Assert.That(result.Any(e => e.Title == "Pending Event"), Is.True);
        }

        [Test]
        public async Task ApproveEventAsync_ApprovesEvent()
        {
            //Arrange
            var newEvent = new Event
            {
                EventId = Guid.NewGuid(),
                Title = "New Event",
                Description = "This is a new event.",
                EventDate = DateTime.UtcNow.AddDays(10),
                Location = "Sofia",
                OrganizerId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid()
            };

            await _context.Events.AddAsync(newEvent);
            await _context.SaveChangesAsync();

            //Act
            var result = await _adminService.ApproveEventAsync(newEvent.EventId);

            //Assert
            Assert.That(result, Is.True);
            var updatedEvent = await _context.Events.FindAsync(newEvent.EventId);
            Assert.That(updatedEvent.IsApproved, Is.True);
        }

        [Test]
        public async Task RejectEventAsync_RejectsEvent()
        {
            //Arrange
            var newEvent = new Event
            {
                EventId = Guid.NewGuid(),
                Title = "New Event",
                Description = "This is a new event.",
                EventDate = DateTime.UtcNow.AddDays(10),
                Location = "Sofia",
                OrganizerId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid()
            };

            await _context.Events.AddAsync(newEvent);
            await _context.SaveChangesAsync();

            //Act
            var result = await _adminService.RejectEventAsync(newEvent.EventId);

            //Assert
            Assert.That(result, Is.True);
            var updatedEvent = await _context.Events.FindAsync(newEvent.EventId);
            Assert.That(updatedEvent, Is.Null);
        }
        
        [Test]
        public async Task ApproveEventAsync_ApprovesEvent_WhenNotFound()
        {
            //Arrange
            var newEvent = Guid.NewGuid();

            //Act
            var result = await _adminService.ApproveEventAsync(newEvent);

            //Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task RejectEventAsync_RejectsEvent_WhenNotFound()
        {
            //Arrange
            var newEvent = Guid.NewGuid();

            //Act
            var result = await _adminService.RejectEventAsync(newEvent);

            //Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task PromoteToModeratorAsync_PromotesUser()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = "Test User",
                Email = "testuser@example.com",
                UserName = "testuser",
                ProfilePictureUrl = "/images/default.png",
                RegistrationDate = DateTime.UtcNow
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            _userManagerMock
                .Setup(um => um.FindByIdAsync(user.Id.ToString()))
                .ReturnsAsync(user);

            _roleManagerMock
                .Setup(rm => rm.RoleExistsAsync("Moderator"))
                .ReturnsAsync(true);

            _userManagerMock
                .Setup(um => um.AddToRoleAsync(user, "Moderator"))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(um => um.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Moderator" });

            // Recreate service with updated mocks
            _adminService = new AdminService(_context, _userManagerMock.Object, _roleManagerMock.Object, _s3ServiceMock.Object);

            // Act
            var result = await _adminService.PromoteToModeratorAsync(user.Id);

            // Assert
            Assert.That(result, Is.True);
            var roles = await _userManagerMock.Object.GetRolesAsync(user);
            Assert.That(roles.Contains("Moderator"), Is.True);
        }


        [Test]
        public async Task DemoteFromModeratorAsync_DemotesUser()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = "Test User",
                Email = "testuser@example.com",
                UserName = "testuser",
                ProfilePictureUrl = "/images/default.png",
                RegistrationDate = DateTime.UtcNow
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            _userManagerMock
                .Setup(um => um.FindByIdAsync(user.Id.ToString()))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(um => um.RemoveFromRoleAsync(user, "Moderator"))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(um => um.GetRolesAsync(user))
                .ReturnsAsync(new List<string>()); // simulate empty role list after demotion

            // recreate AdminService with proper mocks
            _adminService = new AdminService(_context, _userManagerMock.Object, _roleManagerMock.Object, _s3ServiceMock.Object);

            // Act
            var result = await _adminService.DemoteFromModeratorAsync(user.Id);

            // Assert
            Assert.That(result, Is.True);
            _userManagerMock.Verify(um => um.RemoveFromRoleAsync(user, "Moderator"), Times.Once);
        }


        [Test]
        public async Task DemoteFromModeratorAsync_DemotesUser_WhenNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var result = await _adminService.DemoteFromModeratorAsync(userId);

            // Assert
            Assert.That(result, Is.False);
        }
        
        [Test]
        public async Task PromoteToModeratorAsync_PromotesUser_WhenNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var result = await _adminService.PromoteToModeratorAsync(userId);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task PromoteToModeratorAsync_PromotesUser_WhenUserExists()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = "Test User",
                Email = "testuser@example.com",
                UserName = "testuser",
                ProfilePictureUrl = "/images/default.png",
                RegistrationDate = DateTime.UtcNow
            };

            // Add user into in-memory database
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Mock UserManager behavior
            _userManagerMock
                .Setup(um => um.FindByIdAsync(user.Id.ToString()))
                .ReturnsAsync(user);

            _userManagerMock
                .Setup(um => um.GetRolesAsync(user))
                .ReturnsAsync(new List<string>());

            _userManagerMock
                .Setup(um => um.AddToRoleAsync(user, "Moderator"))
                .ReturnsAsync(IdentityResult.Success);

            // Mock RoleManager behavior
            _roleManagerMock
                .Setup(rm => rm.RoleExistsAsync("Moderator"))
                .ReturnsAsync(true);

            // Recreate AdminService with mocks
            _adminService = new AdminService(_context, _userManagerMock.Object, _roleManagerMock.Object, _s3ServiceMock.Object);

            // Act
            var result = await _adminService.PromoteToModeratorAsync(user.Id);

            // Assert
            Assert.That(result, Is.True);

            // Verify AddToRoleAsync was called
            _userManagerMock.Verify(um => um.AddToRoleAsync(user, "Moderator"), Times.Once);
        }

        [Test]
        public async Task GetMarketplaceCategoriesAsync_ReturnsMarketplaceCategories()
        {
            // Arrange
            await _adminService.CreateMarketplaceCategoryAsync("Category 1");
            await _adminService.CreateMarketplaceCategoryAsync("Category 2");

            // Act
            var result = await _adminService.GetMarketplaceCategoriesAsync();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(c => c.Name == "Category 1"), Is.True);
            Assert.That(result.Any(c => c.Name == "Category 2"), Is.True);
        }
        
        [Test]
        public async Task GetEventCategoriesAsync_ReturnsEventCategories()
        {
            // Arrange
            await _adminService.CreateEventCategoryAsync("Category 1");
            await _adminService.CreateEventCategoryAsync("Category 2");

            // Act
            var result = await _adminService.GetEventCategoriesAsync();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Any(c => c.Name == "Category 1"), Is.True);
            Assert.That(result.Any(c => c.Name == "Category 2"), Is.True);
        }
        
        [Test]
        public async Task DeleteEventCategoryAsync_DeletesEventCategory()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            await _adminService.CreateEventCategoryAsync("Test Category");
            var category = await _context.EventCategories.FirstOrDefaultAsync(c => c.Name == "Test Category");

            // Act
            var result = await _adminService.DeleteEventCategoryAsync(category.CategoryId);

            // Assert
            Assert.That(result, Is.True);
            var categoryAfterDelete =
                await _context.EventCategories.FirstOrDefaultAsync(c => c.CategoryId == categoryId);
            Assert.That(categoryAfterDelete, Is.Null);
        }

        [Test]
        public async Task DeleteEventCategoryAsync_ReturnsFalse_WhenNotFound()
        {
            // Act
            var result = await _adminService.DeleteEventCategoryAsync(Guid.NewGuid());

            // Assert
            Assert.That(result, Is.False);
        }
    }
}