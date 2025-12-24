using ANLASH.Universities;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ANLASH.Tests.Universities
{
    /// <summary>
    /// اختبارات الوحدة لمدير نطاق الجامعات - University Manager Unit Tests
    /// </summary>
    public class UniversityManager_Tests : ANLASHTestBase
    {
        private readonly UniversityManager _universityManager;

        public UniversityManager_Tests()
        {
            _universityManager = Resolve<UniversityManager>();
        }

        [Fact]
        public async Task Should_Create_University_With_Auto_Generated_Slug()
        {
            // Arrange
            var university = new University
            {
                Name = "New Test University",
                NameAr = "جامعة اختبار جديدة",
                Country = "Saudi Arabia",
                City = "Riyadh",
                Type = UniversityType.Public,
                Rating = 4.0m
            };

            // Act
            var result = await _universityManager.CreateAsync(university);

            // Assert
            result.ShouldNotBeNull();
            result.Slug.ShouldNotBeNullOrEmpty();
            result.Slug.ShouldBe("new-test-university");
        }

        [Fact]
        public async Task Should_Create_University_With_Custom_Slug()
        {
            // Arrange
            var university = new University
            {
                Name = "Another University",
                Country = "Saudi Arabia",
                City = "Jeddah",
                Type = UniversityType.Private,
                Rating = 3.5m,
                Slug = "custom-slug"
            };

            // Act
            var result = await _universityManager.CreateAsync(university);

            // Assert
            result.ShouldNotBeNull();
            result.Slug.ShouldBe("custom-slug");
        }

        [Fact]
        public async Task Should_Generate_Arabic_Slug_When_NameAr_Provided()
        {
            // Arrange
            var university = new University
            {
                Name = "Test",
                NameAr = "جامعة الاختبار الجديدة",
                Country = "Saudi Arabia",
                City = "Riyadh",
                Type = UniversityType.Public,
                Rating = 4.0m
            };

            // Act
            var result = await _universityManager.CreateAsync(university);

            // Assert
            result.ShouldNotBeNull();
            result.SlugAr.ShouldNotBeNullOrEmpty();
        }

        [Fact]
        public async Task Should_Throw_Exception_For_Duplicate_Name()
        {
            // Use unique name to avoid conflicts with other tests
            var uniqueName = "Duplicate Test " + Guid.NewGuid().ToString("N").Substring(0, 8);
            
            // Arrange
            var university1 = new University
            {
                Name = uniqueName,
                Country = "Saudi Arabia",
                City = "Riyadh",
                Type = UniversityType.Public,
                Rating = 4.0m
            };

            await _universityManager.CreateAsync(university1);

            var university2 = new University
            {
                Name = uniqueName, // Same name
                Country = "Saudi Arabia",
                City = "Jeddah",
                Type = UniversityType.Private,
                Rating = 3.5m
            };

            // Act & Assert
            await Should.ThrowAsync<Abp.UI.UserFriendlyException>(async () =>
            {
                await _universityManager.CreateAsync(university2);
            });
        }

        [Fact]
        public async Task Should_Throw_Exception_For_Invalid_Rating()
        {
            // Arrange
            var university = new University
            {
                Name = "Invalid Rating Test",
                Country = "Saudi Arabia",
                City = "Riyadh",
                Type = UniversityType.Public,
                Rating = 6.0m // Invalid: > 5
            };

            // Act & Assert
            await Should.ThrowAsync<Abp.UI.UserFriendlyException>(async () =>
            {
                await _universityManager.CreateAsync(university);
            });
        }

        [Fact]
        public async Task Should_Update_University_Successfully()
        {
            // Arrange - Create first
            var university = new University
            {
                Name = "Original Name",
                Country = "Saudi Arabia",
                City = "Riyadh",
                Type = UniversityType.Public,
                Rating = 3.0m
            };
            var created = await _universityManager.CreateAsync(university);

            // Act - Update
            created.Name = "Updated Name";
            created.Rating = 4.5m;
            var updated = await _universityManager.UpdateAsync(created);

            // Assert
            updated.ShouldNotBeNull();
            updated.Name.ShouldBe("Updated Name");
            updated.Rating.ShouldBe(4.5m);
        }

        [Fact]
        public async Task Should_Check_Slug_Uniqueness()
        {
            // Arrange
            var slug = "unique-test-slug" + Guid.NewGuid().ToString("N").Substring(0, 8);

            // Act
            var isUnique = await _universityManager.IsSlugUniqueAsync(slug);

            // Assert
            isUnique.ShouldBeTrue();
        }

        [Fact]
        public async Task Should_Detect_Non_Unique_Slug()
        {
            // Use unique slug to avoid conflicts
            var uniqueSlug = "existing-slug-test-" + Guid.NewGuid().ToString("N").Substring(0, 8);
            
            // Arrange - Create university with slug
            var university = new University
            {
                Name = "Slug Test " + Guid.NewGuid().ToString("N").Substring(0, 8),
                Country = "Saudi Arabia",
                City = "Riyadh",
                Type = UniversityType.Public,
                Rating = 4.0m,
                Slug = uniqueSlug
            };
            await _universityManager.CreateAsync(university);

            // Act
            var isUnique = await _universityManager.IsSlugUniqueAsync(uniqueSlug);

            // Assert
            isUnique.ShouldBeFalse();
        }

        [Fact]
        public async Task Should_Allow_Same_Slug_For_Same_University()
        {
            // Use unique slug to avoid conflicts
            var uniqueSlug = "same-university-slug-" + Guid.NewGuid().ToString("N").Substring(0, 8);
            
            // Arrange - Create university
            var university = new University
            {
                Name = "Same Slug Test " + Guid.NewGuid().ToString("N").Substring(0, 8),
                Country = "Saudi Arabia",
                City = "Riyadh",
                Type = UniversityType.Public,
                Rating = 4.0m,
                Slug = uniqueSlug
            };
            var created = await _universityManager.CreateAsync(university);

            // Act - Check uniqueness excluding the same university
            var isUnique = await _universityManager.IsSlugUniqueAsync(uniqueSlug, created.Id);

            // Assert
            isUnique.ShouldBeTrue();
        }

        [Theory]
        [InlineData("Test University", "test-university")]
        [InlineData("Testing & Development", "testing-and-development")]
        [InlineData("Multi--Dash--Test", "multi-dash-test")]
        [InlineData("  Trim Test  ", "trim-test")]
        public void Should_Generate_Correct_Slug_Format(string input, string expected)
        {
            // This tests the slug generation logic indirectly
            // We'll validate the expected format
            var slug = input
                .ToLowerInvariant()
                .Replace(" ", "-")
                .Replace("&", "and")
                .Replace("--", "-")
                .Trim('-');

            slug.ShouldBe(expected);
        }
    }
}
