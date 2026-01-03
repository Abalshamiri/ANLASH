using ANLASH.LanguageCenters;
using ANLASH.LanguageCenters.Dto;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Abp.Application.Services.Dto;

namespace ANLASH.Tests.LanguageCenters
{
    /// <summary>
    /// اختبارات تكامل شاملة لخدمة تسعير الدورات
    /// Comprehensive Integration Tests for Course Pricing App Service
    /// </summary>
    public class CoursePricingAppService_Tests : ANLASHTestBase
    {
        private readonly ICoursePricingAppService _coursePricingAppService;
        private readonly ILanguageCourseAppService _languageCourseAppService;
        private readonly ILanguageCenterAppService _languageCenterAppService;

        public CoursePricingAppService_Tests()
        {
            _coursePricingAppService = Resolve<ICoursePricingAppService>();
            _languageCourseAppService = Resolve<ILanguageCourseAppService>();
            _languageCenterAppService = Resolve<ILanguageCenterAppService>();
        }

        private async Task<long> CreateTestCourseAsync()
        {
            // Create center first
            var centerDto = new CreateLanguageCenterDto
            {
                Name = "Test Center for Pricing",
                NameAr = "معهد اختبار للتسعير",
                CountryId = 1,
                CityId = 1,
                Slug = "test-center-pricing",
                SlugAr = "معهد-اختبار-تسعير",
                Rating = 4.0m
            };
            var center = await _languageCenterAppService.CreateAsync(centerDto);

            // Create course
            var courseDto = new CreateLanguageCourseDto
            {
                LanguageCenterId = center.Id,
                CourseName = "Test Course for Pricing",
                CourseNameAr = "دورة اختبار للتسعير",
                CourseType = CourseType.General,
                Level = CourseLevel.Intermediate,
                MinDurationWeeks = 12,
                HoursPerWeek = 15
            };
            var course = await _languageCourseAppService.CreateAsync(courseDto);
            return course.Id;
        }

        [Fact]
        public async Task Should_Create_Valid_CoursePricing()
        {
            // Arrange
            var courseId = await CreateTestCourseAsync();
            var createDto = new CreateCoursePricingDto
            {
                LanguageCourseId = courseId,
                DurationWeeks = 12,
                Fee = 5000m,
                CurrencyId = 1, // SAR
                RegistrationFee = 500m,
                MaterialsFee = 200m,
                IsActive = true
            };

            // Act
            var result = await _coursePricingAppService.CreateAsync(createDto);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBeGreaterThan(0);
            result.DurationWeeks.ShouldBe(12);
            result.Fee.ShouldBe(5000m);
            result.FeePerWeek.ShouldBe(5000m / 12);
            result.FinalPrice.ShouldBe(5000m); // No discount
        }

        [Fact]
        public async Task Should_Calculate_FeePerWeek_Correctly()
        {
            // Arrange
            var courseId = await CreateTestCourseAsync();
            var createDto = new CreateCoursePricingDto
            {
                LanguageCourseId = courseId,
                DurationWeeks = 8,
                Fee = 4000m,
                CurrencyId = 1
            };

            // Act
            var result = await _coursePricingAppService.CreateAsync(createDto);

            // Assert
            result.FeePerWeek.ShouldBe(500m); // 4000 / 8
        }

        [Fact]
        public async Task Should_Calculate_FinalPrice_With_Discount()
        {
            // Arrange
            var courseId = await CreateTestCourseAsync();
            var createDto = new CreateCoursePricingDto
            {
                LanguageCourseId = courseId,
                DurationWeeks = 12,
                Fee = 6000m,
                CurrencyId = 1,
                HasDiscount = true,
                DiscountPercentage = 20m // 20% discount
            };

            // Act
            var result = await _coursePricingAppService.CreateAsync(createDto);

            // Assert
            result.FinalPrice.ShouldBe(4800m); // 6000 - (6000 * 0.20)
        }

        [Fact]
        public async Task Should_Get_Pricing_By_Course()
        {
            // Arrange
            var courseId = await CreateTestCourseAsync();
            
            // Create multiple pricing options
            var durations = new[] { 4, 8, 12, 24 };
            foreach (var duration in durations)
            {
                var createDto = new CreateCoursePricingDto
                {
                    LanguageCourseId = courseId,
                    DurationWeeks = duration,
                    Fee = duration * 400m,
                    CurrencyId = 1
                };
                await _coursePricingAppService.CreateAsync(createDto);
            }

            // Act
            var result = await _coursePricingAppService.GetByCourseAsync(courseId);

            // Assert
            result.ShouldNotBeNull();
            result.Items.Count.ShouldBe(4);
        }

        [Fact]
        public async Task Should_Get_Active_Pricing_Only()
        {
            // Arrange
            var courseId = await CreateTestCourseAsync();
            
            // Create active pricing
            var activeDto = new CreateCoursePricingDto
            {
                LanguageCourseId = courseId,
                DurationWeeks = 12,
                Fee = 5000m,
                CurrencyId = 1,
                IsActive = true
            };
            await _coursePricingAppService.CreateAsync(activeDto);

            // Create inactive pricing
            var inactiveDto = new CreateCoursePricingDto
            {
                LanguageCourseId = courseId,
                DurationWeeks = 24,
                Fee = 9000m,
                CurrencyId = 1,
                IsActive = false
            };
            await _coursePricingAppService.CreateAsync(inactiveDto);

            // Act
            var result = await _coursePricingAppService.GetActiveByCourseAsync(courseId);

            // Assert
            result.ShouldNotBeNull();
            result.Items.ShouldAllBe(p => p.IsActive == true);
        }

        [Fact]
        public async Task Should_Create_Bulk_Pricing()
        {
            // Arrange
            var courseId = await CreateTestCourseAsync();
            var bulkList = new List<CreateCoursePricingDto>
            {
                new CreateCoursePricingDto
                {
                    LanguageCourseId = courseId,
                    DurationWeeks = 4,
                    Fee = 2000m,
                    CurrencyId = 1
                },
                new CreateCoursePricingDto
                {
                    LanguageCourseId = courseId,
                    DurationWeeks = 8,
                    Fee = 3500m,
                    CurrencyId = 1
                },
                new CreateCoursePricingDto
                {
                    LanguageCourseId = courseId,
                    DurationWeeks = 12,
                    Fee = 5000m,
                    CurrencyId = 1
                }
            };

            // Act
            await _coursePricingAppService.CreateBulkAsync(bulkList);

            // Assert
            var result = await _coursePricingAppService.GetByCourseAsync(courseId);
            result.Items.Count.ShouldBe(3);
        }

        [Fact]
        public async Task Should_Apply_Discount_To_Course()
        {
            // Arrange
            var courseId = await CreateTestCourseAsync();
            
            // Create pricing without discount
            var createDto = new CreateCoursePricingDto
            {
                LanguageCourseId = courseId,
                DurationWeeks = 12,
                Fee = 6000m,
                CurrencyId = 1,
                HasDiscount = false
            };
            var created = await _coursePricingAppService.CreateAsync(createDto);
            created.HasDiscount.ShouldBeFalse();

            // Act - Apply 15% discount
            await _coursePricingAppService.ApplyDiscountToCourseAsync(courseId, 15m);

            // Assert
            var result = await _coursePricingAppService.GetByCourseAsync(courseId);
            result.Items.ShouldAllBe(p => p.HasDiscount == true);
            result.Items.ShouldAllBe(p => p.DiscountPercentage == 15m);
        }

        [Fact]
        public async Task Should_Not_Allow_Duplicate_Duration()
        {
            // Arrange
            var courseId = await CreateTestCourseAsync();
            
            // Create first pricing
            var createDto1 = new CreateCoursePricingDto
            {
                LanguageCourseId = courseId,
                DurationWeeks = 12,
                Fee = 5000m,
                CurrencyId = 1
            };
            await _coursePricingAppService.CreateAsync(createDto1);

            // Act & Assert - Try to create with same duration
            var createDto2 = new CreateCoursePricingDto
            {
                LanguageCourseId = courseId,
                DurationWeeks = 12, // Same duration
                Fee = 5500m,
                CurrencyId = 1
            };

            await Should.ThrowAsync<Abp.UI.UserFriendlyException>(async () =>
            {
                await _coursePricingAppService.CreateAsync(createDto2);
            });
        }

        [Fact]
        public async Task Should_Delete_All_Pricing_For_Course()
        {
            // Arrange
            var courseId = await CreateTestCourseAsync();
            
            // Create multiple pricing
            for (int i = 1; i <= 3; i++)
            {
                var createDto = new CreateCoursePricingDto
                {
                    LanguageCourseId = courseId,
                    DurationWeeks = i * 4,
                    Fee = i * 2000m,
                    CurrencyId = 1
                };
                await _coursePricingAppService.CreateAsync(createDto);
            }

            // Act
            await _coursePricingAppService.DeleteByCourseAsync(courseId);

            // Assert
            var result = await _coursePricingAppService.GetByCourseAsync(courseId);
            result.Items.Count.ShouldBe(0);
        }
    }
}
