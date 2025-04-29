using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Moq;
using MotorcycleForum.Data;
using MotorcycleForum.Data.Entities;
using MotorcycleForum.Data.Entities.Event_Tracker;
using MotorcycleForum.Services.Events;
using MotorcycleForum.Services.Models.Events;

namespace MotorcycleForum.Tests.Services.Events
{
    [TestFixture]
    public class EventsServiceTests
    {
        private MotorcycleForumDbContext _context;
        private EventsService _eventsService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<MotorcycleForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new MotorcycleForumDbContext(options);
            _eventsService = new EventsService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetEventsIndexViewModelAsync_ShouldReturnEventsIndexViewModel()
        {
            // Arrange
            var categories = new List<EventCategory>()
            {
                new EventCategory() { CategoryId = Guid.NewGuid(), Name = "Category1" },
                new EventCategory() { CategoryId = Guid.NewGuid(), Name = "Category2" }
            };

            await _context.EventCategories.AddRangeAsync(categories);
            await _context.SaveChangesAsync();

            // Act
            var viewModel = await _eventsService.GetEventsIndexViewModelAsync();

            // Assert
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.Events, Is.Not.Null);
            Assert.That(viewModel.Categories, Is.Not.Null);
            Assert.That(viewModel.Categories.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetCreateViewModelAsync_ShouldReturnEventFormViewModel()
        {
            // Arrange
            var categories = new List<EventCategory>
            {
                new EventCategory { CategoryId = Guid.NewGuid(), Name = "Category1" },
                new EventCategory { CategoryId = Guid.NewGuid(), Name = "Category2" }
            };

            await _context.EventCategories.AddRangeAsync(categories);
            await _context.SaveChangesAsync();

            // Act
            var viewModel = await _eventsService.GetCreateViewModelAsync();

            // Assert
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.Categories, Is.Not.Null);
            Assert.That(viewModel.Categories.Count(), Is.EqualTo(2));
        }
        
        [Test]
        public async Task DeleteEventAsync_ShouldDeleteEvent()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var @event = new Event
            {
                EventId = eventId,
                Title = "Test Event",
                Description = "Test Description",
                EventDate = DateTime.Now,
                Location = "Test Location",
                CategoryId = Guid.NewGuid(),
                OrganizerId = Guid.Parse("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8")
            };
            await _context.Events.AddAsync(@event);
            await _context.SaveChangesAsync();

            // Act
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8")
            }));
            var result = await _eventsService.DeleteEventAsync(eventId, user);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(await _context.Events.FindAsync(eventId), Is.Null);
        }

        [Test]
        public async Task GetEditViewModelAsync_ShouldReturnEventFormViewModel()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var @event = new Event
            {
                EventId = eventId,
                Title = "Test Event",
                Description = "Test Description",
                EventDate = DateTime.Now,
                Location = "Test Location",
                CategoryId = Guid.NewGuid(),
                OrganizerId = Guid.Parse("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8")
            };
            await _context.Events.AddAsync(@event);
            await _context.SaveChangesAsync();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8")
            }));

            // Act
            var viewModel = await _eventsService.GetEditViewModelAsync(eventId, user);

            // Assert
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.Categories, Is.Not.Null);
            Assert.That(viewModel.Title, Is.EqualTo("Test Event"));
        }

        [Test]
        public async Task UpdateEventAsync_ShouldUpdateEvent()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var originalCategoryId = Guid.NewGuid();
            var newCategoryId = Guid.NewGuid();
            var futureDate = DateTime.Now.AddDays(1);

            var @event = new Event
            {
                EventId = eventId,
                Title = "Test Event",
                Description = "Test Description",
                EventDate = DateTime.Now,
                Location = "Test Location",
                CategoryId = originalCategoryId,
                OrganizerId = Guid.Parse("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8")
            };
            await _context.Events.AddAsync(@event);
            await _context.SaveChangesAsync();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8")
            }));

            var updatedModel = new EventFormViewModel
            {
                EventId = eventId,
                Title = "Updated Test Event",
                Description = "Updated Test Description",
                EventDate = futureDate,
                Location = "Updated Test Location",
                CategoryId = newCategoryId
            };

            // Act
            var result = await _eventsService.UpdateEventAsync(eventId, updatedModel, user);

            // Assert
            Assert.That(result, Is.True);
            var updatedEvent = await _context.Events.FindAsync(eventId);
            Assert.That(updatedEvent!.Title, Is.EqualTo("Updated Test Event"));
            Assert.That(updatedEvent.Description, Is.EqualTo("Updated Test Description"));
            Assert.That(updatedEvent.Location, Is.EqualTo("Updated Test Location"));
            Assert.That(updatedEvent.EventDate, Is.EqualTo(futureDate));
            Assert.That(updatedEvent.CategoryId, Is.EqualTo(newCategoryId));
        }

        [Test]
        public async Task CreateEventAsync_ShouldCreateEvent()
        {
            // Arrange
            var userId = Guid.Parse("f23a5f6d-1c7b-4a5b-97eb-08dbf6a6c3f8");

            // Make sure the user exists
            await _context.Users.AddAsync(new User
            {
                Id = userId,
                FullName = "Test User",
                Email = "test@example.com",
                UserName = "testuser",
                ProfilePictureUrl = "/images/default.png",
                RegistrationDate = DateTime.UtcNow
            });

            // Create a category and save it
            var category = new EventCategory { CategoryId = Guid.NewGuid(), Name = "Test Category" };
            await _context.EventCategories.AddAsync(category);
            await _context.SaveChangesAsync();

            var model = new EventFormViewModel
            {
                Title = "Test Event",
                Description = "Test Description",
                EventDate = DateTime.Now.AddDays(1),
                Location = "Test Location",
                CategoryId = category.CategoryId
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }, authenticationType: "TestAuthType")); // Marks the identity as authenticated

            // Act
            var result = await _eventsService.CreateEventAsync(model, user);

            // Assert
            Assert.That(result, Is.True);
            var createdEvent = await _context.Events.FirstOrDefaultAsync(e => e.Title == "Test Event");
            Assert.That(createdEvent, Is.Not.Null);
            Assert.That(createdEvent.Description, Is.EqualTo("Test Description"));
            Assert.That(createdEvent.Location, Is.EqualTo("Test Location"));
            Assert.That(createdEvent.CategoryId, Is.EqualTo(category.CategoryId));
            Assert.That(createdEvent.OrganizerId, Is.EqualTo(userId));
            Assert.That(createdEvent.IsApproved, Is.False); // since your service sets it to false
        }

    }
}