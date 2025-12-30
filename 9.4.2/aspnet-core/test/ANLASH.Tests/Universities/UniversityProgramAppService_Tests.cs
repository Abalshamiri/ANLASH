using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using ANLASH.Universities;
using ANLASH.Universities.Dto;
using Shouldly;
using Xunit;

namespace ANLASH.Tests.Universities
{
    /// <summary>
    /// Unit tests for UniversityProgramAppService
    /// اختبارات وحدة لخدمة برامج الجامعة
    /// </summary>
    public class UniversityProgramAppService_Tests : ANLASHTestBase
    {
        private readonly IUniversityProgramAppService _programAppService;

        public UniversityProgramAppService_Tests()
        {
            _programAppService = Resolve<IUniversityProgramAppService>();
        }

        #region GetAsync Tests

        [Fact]
        public async Task GetAsync_Should_Return_Program_When_Exists()
        {
            // Arrange - Create a test program
            var createDto = new CreateUniversityProgramDto
            {
                UniversityId = 1, // Assuming university with ID 1 exists
                Name = "Test Program",
                NameAr = "برنامج تجريبي",
                Level = ProgramLevel.Bachelor,
                Mode = StudyMode.FullTime,
                DurationYears = 4,
                TuitionFee = 10000,
                CurrencyId = 1,
                IsActive = true
            };

            var created = await _programAppService.CreateAsync(createDto);

            // Act
            var result = await _programAppService.GetAsync(new EntityDto<long>(created.Id));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(created.Id);
            result.Name.ShouldBe("Test Program");
            result.NameAr.ShouldBe("برنامج تجريبي");
            result.Level.ShouldBe(ProgramLevel.Bachelor);
        }

        #endregion

        #region GetByUniversityId Tests

        [Fact]
        public async Task GetByUniversityId_Should_Return_Programs_For_University()
        {
            // Arrange - Create multiple programs for same university
            var universityId = 1L;
            
            await _programAppService.CreateAsync(new CreateUniversityProgramDto
            {
                UniversityId = universityId,
                Name = "Program 1",
                NameAr = "برنامج 1",
                Level = ProgramLevel.Bachelor,
                Mode = StudyMode.FullTime,
                DurationYears = 4,
                TuitionFee = 10000,
                CurrencyId = 1
            });

            await _programAppService.CreateAsync(new CreateUniversityProgramDto
            {
                UniversityId = universityId,
                Name = "Program 2",
                NameAr = "برنامج 2",
                Level = ProgramLevel.Master,
                Mode = StudyMode.PartTime,
                DurationYears = 2,
                TuitionFee = 15000,
                CurrencyId = 1
            });

            // Act
            var result = await _programAppService.GetByUniversityIdAsync(universityId);

            // Assert
            result.ShouldNotBeNull();
            result.Items.Count.ShouldBeGreaterThanOrEqualTo(2);
        }

        #endregion

        #region GetByLevel Tests

        [Fact]
        public async Task GetByLevel_Should_Filter_By_Level()
        {
            // Arrange
            await _programAppService.CreateAsync(new CreateUniversityProgramDto
            {
                UniversityId = 1,
                Name = "Bachelor Program",
                NameAr = "برنامج بكالوريوس",
                Level = ProgramLevel.Bachelor,
                Mode = StudyMode.FullTime,
                DurationYears = 4,
                TuitionFee = 10000,
                CurrencyId = 1
            });

            // Act
            var result = await _programAppService.GetByLevelAsync(
                ProgramLevel.Bachelor,
                new PagedAndSortedResultRequestDto { MaxResultCount = 10 });

            // Assert
            result.ShouldNotBeNull();
            result.TotalCount.ShouldBeGreaterThan(0);
            result.Items.ShouldAllBe(p => p.Level == ProgramLevel.Bachelor);
        }

        #endregion

        #region Create Tests

        [Fact]
        public async Task CreateAsync_Should_Create_Program_With_Valid_Data()
        {
            // Arrange
            var createDto = new CreateUniversityProgramDto
            {
                UniversityId = 1,
                Name = "New Program",
                NameAr = "برنامج جديد",
                Description = "Description",
                DescriptionAr = "وصف",
                Level = ProgramLevel.Bachelor,
                Mode = StudyMode.FullTime,
                Field = "Engineering",
                FieldAr = "هندسة",
                DurationYears = 4,
                TuitionFee = 12000,
                CurrencyId = 1,
                IsActive = true
            };

            // Act
            var result = await _programAppService.CreateAsync(createDto);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBeGreaterThan(0);
            result.Name.ShouldBe(createDto.Name);
            result.NameAr.ShouldBe(createDto.NameAr);
            result.Level.ShouldBe(createDto.Level);
        }

        #endregion

        #region Update Tests

        [Fact]
        public async Task UpdateAsync_Should_Update_Program()
        {
            // Arrange - Create
            var created = await _programAppService.CreateAsync(new CreateUniversityProgramDto
            {
                UniversityId = 1,
                Name = "Original Name",
                NameAr = "اسم أصلي",
                Level = ProgramLevel.Bachelor,
                Mode = StudyMode.FullTime,
                DurationYears = 4,
                TuitionFee = 10000,
                CurrencyId = 1
            });

            // Act - Update
            var updateDto = new UpdateUniversityProgramDto
            {
                Id = created.Id,
                Name = "Updated Name",
                NameAr = "اسم محدث",
                Level = ProgramLevel.Bachelor,
                Mode = StudyMode.FullTime,
                DurationYears = 4,
                TuitionFee = 12000,
                CurrencyId = 1
            };

            var updated = await _programAppService.UpdateAsync(updateDto);

            // Assert
            updated.Name.ShouldBe("Updated Name");
            updated.NameAr.ShouldBe("اسم محدث");
            updated.TuitionFee.ShouldBe(12000);
        }

        #endregion

        #region Delete Tests

        [Fact]
        public async Task DeleteAsync_Should_Soft_Delete_Program()
        {
            // Arrange
            var created = await _programAppService.CreateAsync(new CreateUniversityProgramDto
            {
                UniversityId = 1,
                Name = "To Delete",
                NameAr = "للحذف",
                Level = ProgramLevel.Bachelor,
                Mode = StudyMode.FullTime,
                DurationYears = 4,
                TuitionFee = 10000,
                CurrencyId = 1
            });

            // Act
            await _programAppService.DeleteAsync(new EntityDto<long>(created.Id));

            // Assert - Should throw EntityNotFoundException when trying to get deleted item
            await Should.ThrowAsync<Abp.Domain.Entities.EntityNotFoundException>(async () =>
            {
                await _programAppService.GetAsync(new EntityDto<long>(created.Id));
            });
        }

        #endregion

        #region GetFeatured Tests

        [Fact]
        public async Task GetFeaturedAsync_Should_Return_Only_Featured_Programs()
        {
            // Arrange - Create featured program
            var created = await _programAppService.CreateAsync(new CreateUniversityProgramDto
            {
                UniversityId = 1,
                Name = "Featured Program",
                NameAr = "برنامج مميز",
                Level = ProgramLevel.Bachelor,
                Mode = StudyMode.FullTime,
                DurationYears = 4,
                TuitionFee = 10000,
                CurrencyId = 1,
                IsFeatured = true,
                IsActive = true
            });

            // Act
            var result = await _programAppService.GetFeaturedAsync(10);

            // Assert
            result.ShouldNotBeNull();
            result.Items.ShouldContain(p => p.Id == created.Id);
            result.Items.ShouldAllBe(p => p.IsFeatured);
        }

        #endregion

        #region ToggleFeatured Tests

        [Fact]
        public async Task ToggleFeaturedAsync_Should_Toggle_Featured_Status()
        {
            // Arrange
            var created = await _programAppService.CreateAsync(new CreateUniversityProgramDto
            {
                UniversityId = 1,
                Name = "Toggle Test",
                NameAr = "اختبار تبديل",
                Level = ProgramLevel.Bachelor,
                Mode = StudyMode.FullTime,
                DurationYears = 4,
                TuitionFee = 10000,
                CurrencyId = 1,
                IsFeatured = false
            });

            // Act
            await _programAppService.ToggleFeaturedAsync(created.Id);

            // Assert
            var result = await _programAppService.GetAsync(new EntityDto<long>(created.Id));
            result.IsFeatured.ShouldBeTrue();

            // Toggle again
            await _programAppService.ToggleFeaturedAsync(created.Id);
            result = await _programAppService.GetAsync(new EntityDto<long>(created.Id));
            result.IsFeatured.ShouldBeFalse();
        }

        #endregion
    }
}
