using ANLASH.Universities;
using ANLASH.Universities.Dto;
using Shouldly;
using System.Threading.Tasks;
using Xunit;
using Abp.Application.Services.Dto;

namespace ANLASH.Tests.Universities
{
    /// <summary>
    /// اختبارات تكامل لخدمة الجامعات - University App Service Integration Tests
    /// </summary>
    public class UniversityAppService_Tests : ANLASHTestBase
    {
        private readonly IUniversityAppService _universityAppService;

        public UniversityAppService_Tests()
        {
            _universityAppService = Resolve<IUniversityAppService>();
        }

        [Fact]
        public async Task Should_Get_All_Universities()
        {
            // Arrange
            var input = new PagedUniversityRequestDto
            {
                MaxResultCount = 10
            };

            // Act
            var result = await _universityAppService.GetAllAsync(input);

            // Assert
            result.ShouldNotBeNull();
            result.Items.ShouldNotBeNull();
            result.Items.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Should_Create_Valid_University()
        {
            // Arrange
            var createDto = new CreateUniversityDto
            {
                Name = "Test University",
                NameAr = "جامعة اختبار",
                Description = "Test Description",
                DescriptionAr = "وصف اختبار",
                Country = "Saudi Arabia",
                City = "Riyadh",
                Type = UniversityType.Public,
                Rating = 4.0m,
                IsActive = true
            };

            // Act
            var result = await _universityAppService.CreateAsync(createDto);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBeGreaterThan(0);
            result.Name.ShouldBe("Test University");
            result.NameAr.ShouldBe("جامعة اختبار");
            result.IsActive.ShouldBeTrue();
        }

        [Fact]
        public async Task Should_Update_Existing_University()
        {
            // Arrange - Create first
            var createDto = new CreateUniversityDto
            {
                Name = "Original Name",
                NameAr = "الاسم الأصلي",
                Country = "Saudi Arabia",
                City = "Jeddah",
                Type = UniversityType.Public,
                Rating = 3.5m
            };
            var created = await _universityAppService.CreateAsync(createDto);

            // Act - Update
            var updateDto = new UpdateUniversityDto
            {
                Id = created.Id, // Set the Id
                Name = "Updated Name",
                NameAr = "الاسم المحدث",
                Country = "Saudi Arabia",
                City = "Jeddah",
                Type = UniversityType.Private,
                Rating = 4.5m,
                IsActive = true
            };

            var updated = await _universityAppService.UpdateAsync(updateDto);

            // Assert
            updated.ShouldNotBeNull();
            updated.Id.ShouldBe(created.Id);
            updated.Name.ShouldBe("Updated Name");
            updated.NameAr.ShouldBe("الاسم المحدث");
            updated.Type.ShouldBe(UniversityType.Private);
            updated.Rating.ShouldBe(4.5m);
        }

        [Fact]
        public async Task Should_Delete_University()
        {
            // Arrange - Create first
            var createDto = new CreateUniversityDto
            {
                Name = "To Be Deleted",
                NameAr = "للحذف",
                Country = "Saudi Arabia",
                City = "Dammam",
                Type = UniversityType.Public,
                Rating = 3.0m
            };
            var created = await _universityAppService.CreateAsync(createDto);

            // Act
            await _universityAppService.DeleteAsync(new EntityDto<int>(created.Id));

            // Assert - Try to get deleted item
            await Should.ThrowAsync<Abp.Domain.Entities.EntityNotFoundException>(async () =>
            {
                await _universityAppService.GetAsync(new EntityDto<int>(created.Id));
            });
        }

        [Fact]
        public async Task Should_Get_University_By_Id()
        {
            // Arrange - Use existing seed data (King Saud University should be ID 1)
            int universityId = 1;

            // Act
            var result = await _universityAppService.GetAsync(new EntityDto<int>(universityId));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(universityId);
            result.Name.ShouldNotBeNullOrEmpty();
            result.NameAr.ShouldNotBeNullOrEmpty();
        }

        [Fact]
        public async Task Should_Get_Featured_Universities()
        {
            // Act
            var result = await _universityAppService.GetFeaturedAsync(5);

            // Assert
            result.ShouldNotBeNull();
            result.Items.ShouldNotBeNull();
            result.Items.Count.ShouldBeGreaterThan(0);
            
            // All should be featured
            foreach (var university in result.Items)
            {
                university.IsFeatured.ShouldBeTrue();
            }
        }

        [Fact]
        public async Task Should_Get_University_By_Slug()
        {
            // Arrange - Use existing seed data slug
            var slug = "king-saud-university";

            // Act
            var result = await _universityAppService.GetBySlugAsync(slug);

            // Assert
            result.ShouldNotBeNull();
            result.Slug.ShouldBe(slug);
            result.Name.ShouldContain("Saud");
        }

        [Fact]
        public async Task Should_Toggle_University_Active_Status()
        {
            // Arrange - Create a university
            var createDto = new CreateUniversityDto
            {
                Name = "Toggle Test",
                NameAr = "اختبار التبديل",
                Country = "Saudi Arabia",
                City = "Riyadh",
                Type = UniversityType.Public,
                Rating = 4.0m,
                IsActive = true
            };
            var created = await _universityAppService.CreateAsync(createDto);
            created.IsActive.ShouldBeTrue();

            // Act
            await _universityAppService.ToggleActiveAsync(created.Id);

            // Assert
            var updated = await _universityAppService.GetAsync(new EntityDto<int>(created.Id));
            updated.IsActive.ShouldBeFalse();
        }

        [Fact]
        public async Task Should_Filter_By_Country()
        {
            // Arrange
            var input = new PagedUniversityRequestDto
            {
                Country = "Saudi Arabia",
                MaxResultCount = 100
            };

            // Act
            var result = await _universityAppService.GetAllAsync(input);

            // Assert
            result.ShouldNotBeNull();
            result.Items.ShouldAllBe(u => u.Country == "Saudi Arabia");
        }

        [Fact]
        public async Task Should_Search_By_SearchTerm()
        {
            // Arrange
            var input = new PagedUniversityRequestDto
            {
                SearchTerm = "King",
                MaxResultCount = 100
            };

            // Act
            var result = await _universityAppService.GetAllAsync(input);

            // Assert
            result.ShouldNotBeNull();
            // Results should contain universities with "King" in name
            result.Items.Count.ShouldBeGreaterThan(0);
        }
    }
}
