using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using MotorcycleForum.Data;
using MotorcycleForum.Data.Entities;
using MotorcycleForum.Data.Entities.Marketplace;
using MotorcycleForum.Services;
using MotorcycleForum.Services.Marketplace;
using MotorcycleForum.Services.Models.Marketplace;

namespace MotorcycleForum.Tests.Services.Marketplace
{
    [TestFixture]
    public class MarketplaceServiceTests
    {
        private MotorcycleForumDbContext _context;
        private Mock<UserManager<User>> _userManagerMock;
        private Mock<IS3Service> _s3ServiceMock;
        private MarketplaceService _marketplaceService;


        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<MotorcycleForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new MotorcycleForumDbContext(options);

            var store = new Mock<IUserStore<User>>();
            _userManagerMock =
                new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            _s3ServiceMock = new Mock<IS3Service>(); // ✅ No constructor args here!

            _marketplaceService = new MarketplaceService(_context, _s3ServiceMock.Object);
        }


        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetFilteredListingsAsync_ReturnsEmpty_WhenNoListingsExist()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var claims = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }));

            // Act
            var result = await _marketplaceService.GetFilteredListingsAsync(
                null, null, null, null, null, 1, claims);

            // Assert
            Assert.That(result.Listings, Is.Empty);
            Assert.That(result.TotalPages, Is.EqualTo(0));
        }

        [Test]
        public async Task GetListingDetailsAsync_ReturnsListing_WhenListingExists()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var sellerId = Guid.NewGuid();

            var category = new Category { CategoryId = Guid.NewGuid(), Name = "Gear" };
            await _context.Categories.AddAsync(category);

            var user = new User
            {
                Id = sellerId,
                FullName = "Test Seller",
                Email = "seller@example.com",
                UserName = "selleruser",
                ProfilePictureUrl = "/images/seller.png",
                RegistrationDate = DateTime.UtcNow
            };
            await _context.Users.AddAsync(user);

            var listing = new MarketplaceListing(sellerId)
            {
                ListingId = listingId,
                Title = "Motorcycle Jacket",
                Description = "Protective and stylish",
                CategoryId = category.CategoryId,
                Price = 120,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                SellerPhoneNumber = "123456",
                Location = "Testville"
            };


            await _context.MarketplaceListings.AddAsync(listing);
            await _context.SaveChangesAsync();

            // Act
            var result = await _marketplaceService.GetListingDetailsAsync(listingId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo("Motorcycle Jacket"));
            Assert.That(result.Description, Does.Contain("Protective"));
        }


        [Test]
        public async Task GetUserListingsAsync_ReturnsEmpty_WhenUserHasNoListings()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var claims =
                new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, userId) }));

            // Act
            var result = await _marketplaceService.GetUserListingsAsync(claims);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetUserListingsAsync_ReturnsListings_WhenUserHasListings()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var user = new User
            {
                Id = userId,
                FullName = "Test User",
                Email = "test@example.com",
                UserName = "testuser",
                ProfilePictureUrl = "/images/test.png",
                RegistrationDate = DateTime.UtcNow
            };

            var categoryId = Guid.NewGuid();
            _context.Users.Add(user);
            _context.Categories.Add(new Category { CategoryId = categoryId, Name = "Parts" });

            var listing = new MarketplaceListing(userId)
            {
                ListingId = Guid.NewGuid(),
                Title = "Helmet",
                Description = "Black helmet, great condition",
                Location = "Berlin",
                Price = 99,
                CategoryId = categoryId,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                SellerPhoneNumber = "123456789"
            };

            _context.MarketplaceListings.Add(listing);
            await _context.SaveChangesAsync();

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

            // Act
            var result = await _marketplaceService.GetUserListingsAsync(claimsPrincipal);

            // Assert
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.First().Title, Is.EqualTo("Helmet"));
        }

        [Test]
        public async Task CreateListingAsync_CreatesListing_WhenValidModelIsProvided()
        {
            var userId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();

            var user = new User
            {
                Id = userId,
                FullName = "Test User",
                Email = "test@example.com",
                UserName = "testuser",
                ProfilePictureUrl = "/images/test.png",
                RegistrationDate = DateTime.UtcNow
            };

            _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _s3ServiceMock.Setup(s => s.UploadFileAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync("https://example.com/fake-image.jpg");

            _context.Users.Add(user);
            _context.Categories.Add(new Category { CategoryId = categoryId, Name = "Test Category" });
            await _context.SaveChangesAsync();

            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1);
            fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(new byte[1]));
            fileMock.Setup(f => f.FileName).Returns("test.jpg");

            var model = new MarketplaceListingViewModel
            {
                Title = "New Motorcycle",
                Description = "Powerful and reliable motorcycle",
                Price = 5000,
                Location = "New York",
                CategoryId = categoryId,
                ImageFiles = new List<IFormFile> { fileMock.Object }
            };

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

            var result = await _marketplaceService.CreateListingAsync(model, claimsPrincipal);

            Assert.That(result, Is.Not.Null);
            Assert.That(await _context.MarketplaceListings.AnyAsync(l => l.ListingId == result), Is.True);
        }

        [Test]
        public async Task UpdateListingAsync_UpdatesListing_WhenValidModelIsProvided()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var listingId = Guid.NewGuid();

            var user = new User
            {
                Id = userId,
                FullName = "Test User",
                ProfilePictureUrl = "/images/test.jpg",
                Email = "test@example.com",
                UserName = "testuser",
                RegistrationDate = DateTime.UtcNow
            };

            var category = new Category
            {
                CategoryId = categoryId,
                Name = "Test Category"
            };

            var listing = new MarketplaceListing(userId)
            {
                ListingId = listingId,
                Title = "Old Motorcycle",
                Description = "Unreliable motorcycle",
                Price = 1000,
                Location = "London",
                CategoryId = categoryId,
                SellerPhoneNumber = "123456789"
            };

            _context.Users.Add(user);
            _context.Categories.Add(category);
            _context.MarketplaceListings.Add(listing);
            await _context.SaveChangesAsync();

            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(1);
            mockFile.Setup(f => f.FileName).Returns("test.jpg");
            mockFile.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(new byte[10]));

            var updatedModel = new MarketplaceListingViewModel
            {
                ListingId = listingId,
                Title = "Updated Motorcycle",
                Description = "Updated and reliable motorcycle",
                Price = 6000,
                Location = "New York",
                CategoryId = categoryId,
                PhoneNumber = "987654321",
                ImageFiles = new List<IFormFile> { mockFile.Object }
            };

            var claimsPrincipal = new ClaimsPrincipal(
                new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                })
            );

            _s3ServiceMock.Setup(s => s.UploadFileAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync("https://bucket.fake/test.jpg");

            // Act
            var result = await _marketplaceService.UpdateListingAsync(updatedModel, claimsPrincipal);

            // Assert
            Assert.That(result, Is.True);
            var updatedListing = await _context.MarketplaceListings.FindAsync(listingId);
            Assert.That(updatedListing, Is.Not.Null);
            Assert.That(updatedListing.Title, Is.EqualTo("Updated Motorcycle"));
            Assert.That(updatedListing.Price, Is.EqualTo(6000));
            Assert.That(updatedListing.Location, Is.EqualTo("New York"));
            Assert.That(updatedListing.SellerPhoneNumber, Is.EqualTo("987654321"));
        }

        [Test]
        public async Task DeleteListingAsync_DeletesListing_WhenListingExists()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var user = new User
            {
                Id = userId,
                FullName = "Seller",
                Email = "seller@example.com",
                UserName = "selleruser",
                ProfilePictureUrl = "/images/seller.jpg",
                RegistrationDate = DateTime.UtcNow
            };

            var categoryId = Guid.NewGuid();
            var category = new Category { CategoryId = categoryId, Name = "Test Category" };

            var listing = new MarketplaceListing(userId)
            {
                ListingId = listingId,
                Title = "Bike to delete",
                Description = "This is a test bike listing to be deleted",
                Price = 1234,
                Location = "Berlin",
                CategoryId = categoryId,
                SellerPhoneNumber = "123456789"
            };

            _context.Users.Add(user);
            _context.Categories.Add(category);
            _context.MarketplaceListings.Add(listing);
            await _context.SaveChangesAsync();

            // Act
            var result = await _marketplaceService.DeleteListingAsync(listingId);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(await _context.MarketplaceListings.AnyAsync(l => l.ListingId == listingId), Is.False);
        }

        [Test]
        public async Task UploadImagesAsync_UploadsImages_WhenFilesAreProvided()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var listingId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();

            _context.Users.Add(new User
            {
                Id = userId,
                FullName = "Jane Doe",
                UserName = "janedoe",
                Email = "jane@example.com",
                ProfilePictureUrl = "/images/jane.png",
                RegistrationDate = DateTime.UtcNow
            });

            _context.Categories.Add(new Category
            {
                CategoryId = categoryId,
                Name = "Parts"
            });

            _context.MarketplaceListings.Add(new MarketplaceListing(userId)
            {
                ListingId = listingId,
                Title = "Listing with Images",
                Description = "A listing meant for image uploads",
                Location = "Test City",
                Price = 1000,
                CategoryId = categoryId,
                SellerPhoneNumber = "000111222"
            });

            await _context.SaveChangesAsync();

            var fileMock1 = new Mock<IFormFile>();
            fileMock1.Setup(f => f.FileName).Returns("image1.jpg");
            fileMock1.Setup(f => f.Length).Returns(1024);
            fileMock1.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(new byte[1024]));

            var fileMock2 = new Mock<IFormFile>();
            fileMock2.Setup(f => f.FileName).Returns("image2.png");
            fileMock2.Setup(f => f.Length).Returns(2048);
            fileMock2.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(new byte[2048]));

            var files = new FormFileCollection { fileMock1.Object, fileMock2.Object };

            _s3ServiceMock.Setup(s => s.UploadFileAsync(It.IsAny<Stream>(), It.IsAny<string>()))
                .ReturnsAsync("https://test.com/image.jpg");

            // Act
            var result = await _marketplaceService.UploadImagesAsync(files, listingId);

            // Assert
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task DeleteImageAsync_DeletesImage_WhenImageExists()
        {
            // Arrange
            var imageId = Guid.NewGuid();
            var listingId = Guid.NewGuid();

            var image = new MarketplaceListingImage
            {
                ImageId = imageId, ImageUrl = "https://test.com/image.jpg", ListingId = listingId
            };

            _context.MarketplaceListingImages.Add(image);
            await _context.SaveChangesAsync();

            // Act
            var result = await _marketplaceService.DeleteImageAsync(imageId);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(await _context.MarketplaceListingImages.AnyAsync(i => i.ImageId == imageId), Is.False);
        }

        [Test]
        public async Task GetFilteredListingsAsync_ReturnsListings_WhenListingsExist()
        {
            // Arrange  
            var listingOwnerId = Guid.NewGuid();
            var viewerId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();

            var user = new User
            {
                Id = listingOwnerId,
                FullName = "Test User",
                Email = "test@example.com",
                UserName = "testuser",
                ProfilePictureUrl = "/images/test.png",
                RegistrationDate = DateTime.UtcNow
            };

            var category = new Category
            {
                CategoryId = categoryId,
                Name = "Accessories"
            };

            var listing = new MarketplaceListing(listingOwnerId)
            {
                ListingId = Guid.NewGuid(),
                Title = "Motorcycle Gloves",
                Description = "Comfortable and durable gloves",
                Price = 50,
                Location = "New York",
                CategoryId = categoryId,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                SellerPhoneNumber = "123456789"
            };

            _context.Users.Add(user);
            _context.Categories.Add(category);
            _context.MarketplaceListings.Add(listing);
            await _context.SaveChangesAsync();

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, viewerId.ToString())
            }));

            // Act  
            var result = await _marketplaceService.GetFilteredListingsAsync(
                "Gloves", categoryId, null, null, null, 1, claimsPrincipal);

            // Assert  
            Assert.That(result.Listings, Is.Not.Empty);
            Assert.That(result.Listings.First().Title, Is.EqualTo("Motorcycle Gloves"));
            Assert.That(result.TotalPages, Is.EqualTo(1));
        }
        
        [Test]
        public async Task GetCreateViewModelAsync_ReturnsViewModel_WithCategories()
        {
            // Arrange
            var category1 = new Category { CategoryId = Guid.NewGuid(), Name = "Bikes" };
            var category2 = new Category { CategoryId = Guid.NewGuid(), Name = "Gear" };
            _context.Categories.Add(category1);
            _context.Categories.Add(category2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _marketplaceService.GetCreateViewModelAsync();

            // Assert
            Assert.That(result.Categories.Items.Cast<object>().Count(), Is.GreaterThanOrEqualTo(2));
        }
        
        [Test]
        public async Task GetEditViewModelAsync_ReturnsViewModel_WithListingData()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();
            var listingId = Guid.NewGuid();

            var user = new User
            {
                Id = userId,
                FullName = "Test User",
                Email = "test@example.com",
                UserName = "testuser",
                ProfilePictureUrl = "/images/test.png",
                RegistrationDate = DateTime.UtcNow
            };

            var category = new Category { CategoryId = categoryId, Name = "Test Category" };
            var listing = new MarketplaceListing(userId)
            {
                ListingId = listingId,
                Title = "Motorcycle",
                Description = "A test motorcycle listing",
                Price = 1000,
                Location = "London",
                CategoryId = categoryId,
                SellerPhoneNumber = "123456789"
            };
            _context.Users.Add(user);
            _context.Categories.Add(category);
            _context.MarketplaceListings.Add(listing);
            await _context.SaveChangesAsync();

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));
            // Act
            var result = await _marketplaceService.GetEditViewModelAsync(listingId, claimsPrincipal);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo("Motorcycle"));
            Assert.That(result.Price, Is.EqualTo(1000));
            Assert.That(result.Categories.SelectedValue.ToString(), Is.EqualTo(categoryId.ToString()));
        }

        [Test]
        public async Task GetDeleteConfirmationAsync_ReturnsListing_WhenListingExists()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var user = new User
            {
                Id = userId,
                FullName = "Test User",
                Email = "test@example.com",
                UserName = "testuser",
                ProfilePictureUrl = "/images/test.png",
                RegistrationDate = DateTime.UtcNow
            };

            var category = new Category { CategoryId = Guid.NewGuid(), Name = "Test Category" };
            var listing = new MarketplaceListing(userId)
            {
                ListingId = listingId,
                Title = "Motorcycle to delete",
                Description = "This is a test motorcycle listing to be deleted",
                Price = 1000,
                Location = "London",
                CategoryId = category.CategoryId,
                SellerPhoneNumber = "123456789"
            };

            _context.Users.Add(user);
            _context.Categories.Add(category);
            _context.MarketplaceListings.Add(listing);
            await _context.SaveChangesAsync();

            var claimsPrincipal =
                new ClaimsPrincipal(
                    new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) }));

            // Act
            var result = await _marketplaceService.GetDeleteConfirmationAsync(listingId, claimsPrincipal);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo("Motorcycle to delete"));
        }
        
        [Test]
        public async Task GetDeleteConfirmationAsync_ReturnsNull_WhenListingDoesntExist()
        {
            // Arrange
            var listingId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var claimsPrincipal =
                new ClaimsPrincipal(new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                }));
            // Act
            var result = await _marketplaceService.GetDeleteConfirmationAsync(listingId, claimsPrincipal);
            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetFilteredListingsAsync_ReturnsEmpty_WhenCategoryDoesNotExist()
        {
            // Arrange  
            var nonExistentCategoryId = Guid.NewGuid();
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
               new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
           }));

            // Act  
            var result = await _marketplaceService.GetFilteredListingsAsync(
                null, nonExistentCategoryId, null, null, null, 1, claimsPrincipal);

            // Assert  
            Assert.That(result.Listings, Is.Empty);
            Assert.That(result.TotalPages, Is.EqualTo(0));
        }

        [Test]
        public async Task DeleteListingAsync_ReturnsFalse_WhenListingDoesNotExist()
        {
            // Arrange  
            var nonExistentListingId = Guid.NewGuid();

            // Act  
            var result = await _marketplaceService.DeleteListingAsync(nonExistentListingId);

            // Assert  
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task UpdateListingAsync_ReturnsFalse_WhenListingDoesNotExist()
        {
            // Arrange  
            var userId = Guid.NewGuid();
            var nonExistentListingId = Guid.NewGuid();

            var updatedModel = new MarketplaceListingViewModel
            {
                ListingId = nonExistentListingId,
                Title = "Non-existent Listing",
                Description = "This should fail",
                Price = 1000,
                Location = "Nowhere",
                CategoryId = Guid.NewGuid()
            };

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
               new Claim(ClaimTypes.NameIdentifier, userId.ToString())
           }));

            // Act  
            var result = await _marketplaceService.UpdateListingAsync(updatedModel, claimsPrincipal);

            // Assert  
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task GetListingDetailsAsync_ReturnsNull_WhenListingNotFound()
        {
            // Arrange
            var listingId = Guid.NewGuid();

            // Act
            var result = await _marketplaceService.GetListingDetailsAsync(listingId);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task DeleteListingAsync_ReturnsFalse_WhenListingNotFound()
        {
            // Arrange
            var nonexistentId = Guid.NewGuid();

            // Act
            var result = await _marketplaceService.DeleteListingAsync(nonexistentId);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task UpdateListingAsync_ReturnsFalse_WhenUserIsNotOwner()
        {
            // Arrange
            var listingOwnerId = Guid.NewGuid();
            var anotherUserId = Guid.NewGuid();

            var categoryId = Guid.NewGuid();
            _context.Categories.Add(new Category { CategoryId = categoryId, Name = "TestCat" });

            var listing = new MarketplaceListing(listingOwnerId)
            {
                ListingId = Guid.NewGuid(),
                Title = "Bike",
                Description = "Fast",
                Price = 200,
                Location = "Berlin",
                CategoryId = categoryId
            };

            _context.MarketplaceListings.Add(listing);
            await _context.SaveChangesAsync();

            var model = new MarketplaceListingViewModel
            {
                ListingId = listing.ListingId,
                Title = "Updated",
                Description = "Updated desc",
                Price = 1000,
                Location = "Paris",
                CategoryId = categoryId
            };

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, anotherUserId.ToString())
            }));

            // Act
            var result = await _marketplaceService.UpdateListingAsync(model, claimsPrincipal);

            // Assert
            Assert.That(result, Is.False);
        }

    }

}

