using ANLASH.LanguageCenters;
using ANLASH.LanguageCenters.Dto;
using Shouldly;
using System.Threading.Tasks;
using Xunit;
using Abp.Application.Services.Dto;

namespace ANLASH.Tests.LanguageCenters
{
    /// <summary>
    /// اختبارات تكامل شاملة لخدمة معاهد اللغة
    /// Comprehensive Integration Tests for Language Center App Service
    /// </summary>
    public class LanguageCenterAppService_Tests : ANLASHTestBase
    {
        private readonly ILanguageCenterAppService _languageCenterAppService;

        public LanguageCenterAppService_Tests()
        {
            _languageCenterAppService = Resolve<ILanguageCenterAppService>();
        }

        #region CRUD Tests

        [Fact]
        public async Task Should_Get_All_LanguageCenters()
        {
            // Arrange
            var input = new PagedLanguageCenterRequestDto
            {
                MaxResultCount = 10
            };

            // Act
            var result = await _languageCenterAppService.GetAllAsync(input);

            // Assert
            result.ShouldNotBeNull();
            result.Items.ShouldNotBeNull();
        }

        [Fact]
        public async Task Should_Create_Valid_LanguageCenter()
        {
            // Arrange
            var createDto = new CreateLanguageCenterDto
            {
                Name = "British Council Test",
                NameAr = "المجلس الثقافي البريطاني - اختبار",
                Description = "Leading language center",
                DescriptionAr = "معهد لغة رائد",
                CountryId = 1, // Assuming Saudi Arabia
                CityId = 1,    // Assuming Riyadh
                Slug = "british-council-test",
                SlugAr = "المجلس-الثقافي-البريطاني-اختبار",
                Rating = 4.5m,
                IsActive = true,
                IsAccredited = true
            };

            // Act
            var result = await _languageCenterAppService.CreateAsync(createDto);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBeGreaterThan(0);
            result.Name.ShouldBe("British Council Test");
            result.NameAr.ShouldBe("المجلس الثقافي البريطاني - اختبار");
            result.Rating.ShouldBe(4.5m);
            result.IsActive.ShouldBeTrue();
            result.IsAccredited.ShouldBeTrue();
        }

        [Fact]
        public async Task Should_Update_Existing_LanguageCenter()
        {
            // Arrange - Create first
            var createDto = new CreateLanguageCenterDto
            {
                Name = "Original Center",
                NameAr = "المعهد الأصلي",
                CountryId = 1,
                CityId = 1,
                Slug = "original-center",
                SlugAr = "المعهد-الأصلي",
                Rating = 3.5m,
                IsActive = true
            };
            var created = await _languageCenterAppService.CreateAsync(createDto);

            // Act - Update
            var updateDto = new UpdateLanguageCenterDto
            {
                Id = created.Id,
                Name = "Updated Center",
                NameAr = "المعهد المحدث",
                CountryId = 1,
                CityId = 1,
                Slug = "updated-center",
                SlugAr = "المعهد-المحدث",
                Rating = 4.8m,
                IsActive = true,
                IsFeatured = true
            };
            var updated = await _languageCenterAppService.UpdateAsync(updateDto);

            // Assert
            updated.ShouldNotBeNull();
            updated.Id.ShouldBe(created.Id);
            updated.Name.ShouldBe("Updated Center");
            updated.NameAr.ShouldBe("المعهد المحدث");
            updated.Rating.ShouldBe(4.8m);
            updated.IsFeatured.ShouldBeTrue();
        }

        [Fact]
        public async Task Should_Delete_LanguageCenter()
        {
            // Arrange - Create first
            var createDto = new CreateLanguageCenterDto
            {
                Name = "To Be Deleted",
                NameAr = "للحذف",
                CountryId = 1,
                CityId = 1,
                Slug = "to-be-deleted-unique",
                SlugAr = "للحذف-فريد",
                Rating = 3.0m
            };
            var created = await _languageCenterAppService.CreateAsync(createDto);

            // Act
            await _languageCenterAppService.DeleteAsync(new EntityDto<long>(created.Id));

            // Assert - Entity should be soft deleted (not found in normal queries)
            var allCenters = await _languageCenterAppService.GetAllAsync(new PagedLanguageCenterRequestDto
            {
                MaxResultCount = 1000
            });
            
            allCenters.Items.ShouldNotContain(c => c.Id == created.Id);
        }

        [Fact]
        public async Task Should_Get_LanguageCenter_By_Id()
        {
            // Arrange - Create a center
            var createDto = new CreateLanguageCenterDto
            {
                Name = "Get By ID Test",
                NameAr = "اختبار الحصول بالمعرف",
                CountryId = 1,
                CityId = 1,
                Slug = "get-by-id-test",
                SlugAr = "اختبار-الحصول-بالمعرف",
                Rating = 4.0m
            };
            var created = await _languageCenterAppService.CreateAsync(createDto);

            // Act
            var result = await _languageCenterAppService.GetAsync(new EntityDto<long>(created.Id));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(created.Id);
            result.Name.ShouldBe("Get By ID Test");
        }

        #endregion

        #region Business Logic Tests

        [Fact]
        public async Task Should_Get_Featured_LanguageCenters()
        {
            // Arrange - Create featured centers
            for (int i = 0; i < 3; i++)
            {
                var createDto = new CreateLanguageCenterDto
                {
                    Name = $"Featured Center {i}",
                    NameAr = $"معهد مميز {i}",
                    CountryId = 1,
                    CityId = 1,
                    Slug = $"featured-center-{i}",
                    SlugAr = $"معهد-مميز-{i}",
                    Rating = 4.5m,
                    IsActive = true,
                    IsFeatured = true
                };
                await _languageCenterAppService.CreateAsync(createDto);
            }

            // Act
            var result = await _languageCenterAppService.GetFeaturedAsync(5);

            // Assert
            result.ShouldNotBeNull();
            result.Items.ShouldNotBeNull();
            result.Items.Count.ShouldBeGreaterThan(0);
            
            // All should be featured and active
            foreach (var center in result.Items)
            {
                center.IsFeatured.ShouldBeTrue();
                center.IsActive.ShouldBeTrue();
            }
        }

        [Fact]
        public async Task Should_Get_LanguageCenter_By_Slug()
        {
            // Arrange
            var createDto = new CreateLanguageCenterDto
            {
                Name = "Slug Test Center",
                NameAr = "معهد اختبار الرابط",
                CountryId = 1,
                CityId = 1,
                Slug = "slug-test-center",
                SlugAr = "معهد-اختبار-الرابط",
                Rating = 4.0m,
                IsActive = true
            };
            await _languageCenterAppService.CreateAsync(createDto);

            // Act
            var result = await _languageCenterAppService.GetBySlugAsync("slug-test-center");

            // Assert
            result.ShouldNotBeNull();
            result.Slug.ShouldBe("slug-test-center");
            result.Name.ShouldBe("Slug Test Center");
        }

        [Fact]
        public async Task Should_Get_LanguageCenters_By_Country()
        {
            // Arrange - Create centers in specific country
            for (int i = 0; i < 2; i++)
            {
                var createDto = new CreateLanguageCenterDto
                {
                    Name = $"Country Test {i}",
                    NameAr = $"اختبار الدولة {i}",
                    CountryId = 1, // Saudi Arabia
                    CityId = 1,
                    Slug = $"country-test-{i}",
                    SlugAr = $"اختبار-الدولة-{i}",
                    Rating = 4.0m,
                    IsActive = true
                };
                await _languageCenterAppService.CreateAsync(createDto);
            }

            // Act
            var result = await _languageCenterAppService.GetByCountryAsync(1);

            // Assert
            result.ShouldNotBeNull();
            result.Items.ShouldNotBeNull();
            result.Items.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Should_Get_Accredited_LanguageCenters()
        {
            // Arrange - Create accredited center
            var createDto = new CreateLanguageCenterDto
            {
                Name = "Accredited Center",
                NameAr = "معهد معتمد",
                CountryId = 1,
                CityId = 1,
                Slug = "accredited-center",
                SlugAr = "معهد-معتمد",
                Rating = 4.5m,
                IsActive = true,
                IsAccredited = true
            };
            await _languageCenterAppService.CreateAsync(createDto);

            // Act
            var input = new PagedLanguageCenterRequestDto
            {
                IsAccredited = true,
                MaxResultCount = 10
            };
            var result = await _languageCenterAppService.GetAccreditedAsync(input);

            // Assert
            result.ShouldNotBeNull();
            result.Items.ShouldAllBe(c => c.IsAccredited == true);
        }

        [Fact]
        public async Task Should_Toggle_Active_Status()
        {
            // Arrange
            var createDto = new CreateLanguageCenterDto
            {
                Name = "Toggle Test",
                NameAr = "اختبار التبديل",
                CountryId = 1,
                CityId = 1,
                Slug = "toggle-test",
                SlugAr = "اختبار-التبديل",
                Rating = 4.0m,
                IsActive = true
            };
            var created = await _languageCenterAppService.CreateAsync(createDto);
            created.IsActive.ShouldBeTrue();

            // Act
            await _languageCenterAppService.ToggleActiveAsync(created.Id);

            // Assert
            var updated = await _languageCenterAppService.GetAsync(new EntityDto<long>(created.Id));
            updated.IsActive.ShouldBeFalse();
        }

        [Fact]
        public async Task Should_Toggle_Featured_Status()
        {
            // Arrange
            var createDto = new CreateLanguageCenterDto
            {
                Name = "Featured Toggle Test",
                NameAr = "اختبار تبديل المميز",
                CountryId = 1,
                CityId = 1,
                Slug = "featured-toggle-test",
                SlugAr = "اختبار-تبديل-المميز",
                Rating = 4.0m,
                IsFeatured = false
            };
            var created = await _languageCenterAppService.CreateAsync(createDto);
            created.IsFeatured.ShouldBeFalse();

            // Act
            await _languageCenterAppService.ToggleFeaturedAsync(created.Id);

            // Assert
            var updated = await _languageCenterAppService.GetAsync(new EntityDto<long>(created.Id));
            updated.IsFeatured.ShouldBeTrue();
        }

        #endregion

        #region Validation Tests

        [Fact]
        public async Task Should_Not_Create_LanguageCenter_With_Duplicate_Slug()
        {
            // Arrange - Create first center
            var createDto1 = new CreateLanguageCenterDto
            {
                Name = "First Center",
                NameAr = "المعهد الأول",
                CountryId = 1,
                CityId = 1,
                Slug = "duplicate-slug-test",
                SlugAr = "اختبار-رابط-مكرر",
                Rating = 4.0m
            };
            await _languageCenterAppService.CreateAsync(createDto1);

            // Act & Assert - Try to create with same slug
            var createDto2 = new CreateLanguageCenterDto
            {
                Name = "Second Center",
                NameAr = "المعهد الثاني",
                CountryId = 1,
                CityId = 1,
                Slug = "duplicate-slug-test", // Same slug
                SlugAr = "اختبار-رابط-مكرر-2",
                Rating = 4.0m
            };

            await Should.ThrowAsync<Abp.UI.UserFriendlyException>(async () =>
            {
                await _languageCenterAppService.CreateAsync(createDto2);
            });
        }

        [Fact]
        public async Task Should_Filter_By_Keyword()
        {
            // Arrange
            var createDto = new CreateLanguageCenterDto
            {
                Name = "British Council Keyword Test",
                NameAr = "المجلس الثقافي البريطاني - كلمة مفتاحية",
                CountryId = 1,
                CityId = 1,
                Slug = "british-council-keyword",
                SlugAr = "المجلس-الثقافي-كلمة",
                Rating = 4.0m,
                IsActive = true
            };
            await _languageCenterAppService.CreateAsync(createDto);

            // Act
            var input = new PagedLanguageCenterRequestDto
            {
                Keyword = "British",
                MaxResultCount = 10
            };
            var result = await _languageCenterAppService.GetAllAsync(input);

            // Assert
            result.ShouldNotBeNull();
            result.Items.Count.ShouldBeGreaterThan(0);
        }

        #endregion
    }
}
