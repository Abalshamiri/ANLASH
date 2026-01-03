using ANLASH.LanguageCenters;
using ANLASH.LanguageCenters.Dto;
using Shouldly;
using System.Threading.Tasks;
using Xunit;
using Abp.Application.Services.Dto;

namespace ANLASH.Tests.LanguageCenters
{
    /// <summary>
    /// اختبارات تكامل شاملة لخدمة الدورات اللغوية
    /// Comprehensive Integration Tests for Language Course App Service
    /// </summary>
    public class LanguageCourseAppService_Tests : ANLASHTestBase
    {
        private readonly ILanguageCourseAppService _languageCourseAppService;
        private readonly ILanguageCenterAppService _languageCenterAppService;

        public LanguageCourseAppService_Tests()
        {
            _languageCourseAppService = Resolve<ILanguageCourseAppService>();
            _languageCenterAppService = Resolve<ILanguageCenterAppService>();
        }

        private async Task<long> CreateTestLanguageCenterAsync()
        {
            var createDto = new CreateLanguageCenterDto
            {
                Name = "Test Center for Courses",
                NameAr = "معهد اختبار للدورات",
                CountryId = 1,
                CityId = 1,
                Slug = "test-center-courses",
                SlugAr = "معهد-اختبار-دورات",
                Rating = 4.0m,
                IsActive = true
            };
            var result = await _languageCenterAppService.CreateAsync(createDto);
            return result.Id;
        }

        [Fact]
        public async Task Should_Create_Valid_LanguageCourse()
        {
            // Arrange
            var centerId = await CreateTestLanguageCenterAsync();
            var createDto = new CreateLanguageCourseDto
            {
                LanguageCenterId = centerId,
                CourseName = "General English",
                CourseNameAr = "اللغة الإنجليزية العامة",
                CourseType = CourseType.General,
                Level = CourseLevel.Intermediate,
                Description = "Comprehensive English course",
                DescriptionAr = "دورة إنجليزية شاملة",
                MinDurationWeeks = 12,
                HoursPerWeek = 15,
                IsActive = true
            };

            // Act
            var result = await _languageCourseAppService.CreateAsync(createDto);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBeGreaterThan(0);
            result.CourseName.ShouldBe("General English");
            result.CourseType.ShouldBe(CourseType.General);
            result.Level.ShouldBe(CourseLevel.Intermediate);
        }

        [Fact]
        public async Task Should_Get_Courses_By_LanguageCenter()
        {
            // Arrange
            var centerId = await CreateTestLanguageCenterAsync();
            
            // Create multiple courses
            for (int i = 0; i < 3; i++)
            {
                var createDto = new CreateLanguageCourseDto
                {
                    LanguageCenterId = centerId,
                    CourseName = $"Course {i}",
                    CourseNameAr = $"دورة {i}",
                    CourseType = CourseType.General,
                    Level = CourseLevel.Beginner,
                    MinDurationWeeks = 8,
                    HoursPerWeek = 10,
                    DisplayOrder = i
                };
                await _languageCourseAppService.CreateAsync(createDto);
            }

            // Act
            var result = await _languageCourseAppService.GetByLanguageCenterAsync(centerId);

            // Assert
            result.ShouldNotBeNull();
            result.Items.Count.ShouldBe(3);
        }

        [Fact]
        public async Task Should_Get_Active_Courses_Only()
        {
            // Arrange
            var centerId = await CreateTestLanguageCenterAsync();
            
            // Create active course
            var activeDto = new CreateLanguageCourseDto
            {
                LanguageCenterId = centerId,
                CourseName = "Active Course",
                CourseNameAr = "دورة نشطة",
                CourseType = CourseType.TestPreparation,
                Level = CourseLevel.UpperIntermediate,
                MinDurationWeeks = 10,
                HoursPerWeek = 12,
                IsActive = true
            };
            await _languageCourseAppService.CreateAsync(activeDto);

            // Create inactive course
            var inactiveDto = new CreateLanguageCourseDto
            {
                LanguageCenterId = centerId,
                CourseName = "Inactive Course",
                CourseNameAr = "دورة غير نشطة",
                CourseType = CourseType.Academic,
                Level = CourseLevel.Advanced,
                MinDurationWeeks = 8,
                HoursPerWeek = 10,
                IsActive = false
            };
            await _languageCourseAppService.CreateAsync(inactiveDto);

            // Act
            var result = await _languageCourseAppService.GetActiveByLanguageCenterAsync(centerId);

            // Assert
            result.ShouldNotBeNull();
            result.Items.ShouldAllBe(c => c.IsActive == true);
        }

        [Fact]
        public async Task Should_Filter_By_CourseType()
        {
            // Arrange
            var centerId = await CreateTestLanguageCenterAsync();
            var createDto = new CreateLanguageCourseDto
            {
                LanguageCenterId = centerId,
                CourseName = "Test Preparation",
                CourseNameAr = "تحضير اختبار",
                CourseType = CourseType.TestPreparation,
                Level = CourseLevel.UpperIntermediate,
                MinDurationWeeks = 8,
                HoursPerWeek = 15,
                IsActive = true
            };
            await _languageCourseAppService.CreateAsync(createDto);

            // Act
            var input = new PagedLanguageCourseRequestDto
            {
                CourseType = CourseType.TestPreparation,
                MaxResultCount = 10
            };
            var result = await _languageCourseAppService.GetByCourseTypeAsync(CourseType.TestPreparation, input);

            // Assert
            result.ShouldNotBeNull();
            result.Items.ShouldAllBe(c => c.CourseType == CourseType.TestPreparation);
        }

        [Fact]
        public async Task Should_Filter_By_Level()
        {
            // Arrange
            var centerId = await CreateTestLanguageCenterAsync();
            var createDto = new CreateLanguageCourseDto
            {
                LanguageCenterId = centerId,
                CourseName = "Advanced English",
                CourseNameAr = "إنجليزية متقدمة",
                CourseType = CourseType.General,
                Level = CourseLevel.Advanced,
                MinDurationWeeks = 12,
                HoursPerWeek = 15,
                IsActive = true
            };
            await _languageCourseAppService.CreateAsync(createDto);

            // Act
            var input = new PagedLanguageCourseRequestDto
            {
                Level = CourseLevel.Advanced,
                MaxResultCount = 10
            };
            var result = await _languageCourseAppService.GetByLevelAsync(CourseLevel.Advanced, input);

            // Assert
            result.ShouldNotBeNull();
            result.Items.ShouldAllBe(c => c.Level == CourseLevel.Advanced);
        }

        [Fact]
        public async Task Should_Toggle_Course_Active_Status()
        {
            // Arrange
            var centerId = await CreateTestLanguageCenterAsync();
            var createDto = new CreateLanguageCourseDto
            {
                LanguageCenterId = centerId,
                CourseName = "Toggle Test Course",
                CourseNameAr = "دورة اختبار التبديل",
                CourseType = CourseType.General,
                Level = CourseLevel.Elementary,
                MinDurationWeeks = 8,
                HoursPerWeek = 10,
                IsActive = true
            };
            var created = await _languageCourseAppService.CreateAsync(createDto);

            // Act
            await _languageCourseAppService.ToggleActiveAsync(created.Id);

            // Assert
            var updated = await _languageCourseAppService.GetAsync(new EntityDto<long>(created.Id));
            updated.IsActive.ShouldBeFalse();
        }

        [Fact]
        public async Task Should_Get_Featured_Courses()
        {
            // Arrange
            var centerId = await CreateTestLanguageCenterAsync();
            
            for (int i = 0; i < 3; i++)
            {
                var createDto = new CreateLanguageCourseDto
                {
                    LanguageCenterId = centerId,
                    CourseName = $"Featured Course {i}",
                    CourseNameAr = $"دورة مميزة {i}",
                    CourseType = CourseType.General,
                    Level = CourseLevel.Intermediate,
                    MinDurationWeeks = 10,
                    HoursPerWeek = 12,
                    IsActive = true,
                    IsFeatured = true,
                    DisplayOrder = i
                };
                await _languageCourseAppService.CreateAsync(createDto);
            }

            // Act
            var result = await _languageCourseAppService.GetFeaturedAsync(5);

            // Assert
            result.ShouldNotBeNull();
            result.Items.ShouldAllBe(c => c.IsFeatured == true && c.IsActive == true);
        }
    }
}
