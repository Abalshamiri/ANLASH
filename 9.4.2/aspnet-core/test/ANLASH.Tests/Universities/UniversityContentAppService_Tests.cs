using System.Linq;
using System.Threading.Tasks;
using ANLASH.Universities;
using ANLASH.Universities.Dto;
using Shouldly;
using Xunit;

namespace ANLASH.Tests.Universities
{
    /// <summary>
    /// Unit tests for UniversityContentAppService
    /// </summary>
    public class UniversityContentAppService_Tests : ANLASHTestBase
    {
        private readonly IUniversityContentAppService _contentAppService;

        public UniversityContentAppService_Tests()
        {
            _contentAppService = Resolve<IUniversityContentAppService>();
        }

        [Fact]
        public async Task GetByUniversityIdAsync_Should_Return_Contents()
        {
            // Arrange
            var universityId = 1L;
            await _contentAppService.CreateAsync(new CreateUniversityContentDto
            {
                UniversityId = universityId,
                ContentType = UniversityContentType.Overview,
                Title = "Overview",
                TitleAr = "نظرة عامة",
                Content = "Content",
                ContentAr = "محتوى"
            });

            // Act
            var result = await _contentAppService.GetByUniversityIdAsync(universityId);

            // Assert
            result.Items.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task GetByTypeAsync_Should_Return_Content_By_Type()
        {
            // Arrange
            var universityId = 1L;
            await _contentAppService.CreateAsync(new CreateUniversityContentDto
            {
                UniversityId = universityId,
                ContentType = UniversityContentType.Admissions,
                Title = "Admissions",
                TitleAr = "القبول",
                Content = "Admission Content",
                ContentAr = "محتوى القبول"
            });

            // Act
            var result = await _contentAppService.GetByTypeAsync(universityId, UniversityContentType.Admissions);

            // Assert
            result.ShouldNotBeNull();
            result.ContentType.ShouldBe(UniversityContentType.Admissions);
        }

        [Fact]
        public async Task CreateAsync_Should_Create_Content()
        {
            // Arrange
            var createDto = new CreateUniversityContentDto
            {
                UniversityId = 1,
                ContentType = UniversityContentType.About,
                Title = "About University",
                TitleAr = "عن الجامعة",
                Content = "University information",
                ContentAr = "معلومات عن الجامعة"
            };

            // Act
            var result = await _contentAppService.CreateAsync(createDto);

            // Assert
            result.ShouldNotBeNull();
            result.Title.ShouldBe("About University");
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Content()
        {
            // Arrange
            var created = await _contentAppService.CreateAsync(new CreateUniversityContentDto
            {
                UniversityId = 1,
                ContentType = UniversityContentType.StudentLife,
                Title = "Original",
                TitleAr = "أصلي",
                Content = "Original Content",
                ContentAr = "محتوى أصلي"
            });

            // Act
            var updateDto = new UpdateUniversityContentDto
            {
                Id = created.Id,
                ContentType = UniversityContentType.StudentLife,
                Title = "Updated",
                TitleAr = "محدث",
                Content = "Updated Content",
                ContentAr = "محتوى محدث"
            };
            var updated = await _contentAppService.UpdateAsync(updateDto);

            // Assert
            updated.Title.ShouldBe("Updated");
            updated.TitleAr.ShouldBe("محدث");
        }
    }
}
