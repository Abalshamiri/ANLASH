using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ANLASH.Universities;
using ANLASH.Universities.Dto;
using Shouldly;
using Xunit;

namespace ANLASH.Tests.Universities
{
    /// <summary>
    /// Unit tests for UniversityFAQAppService
    /// </summary>
    public class UniversityFAQAppService_Tests : ANLASHTestBase
    {
        private readonly IUniversityFAQAppService _faqAppService;

        public UniversityFAQAppService_Tests()
        {
            _faqAppService = Resolve<IUniversityFAQAppService>();
        }

        [Fact]
        public async Task GetAsync_Should_Return_FAQ()
        {
            // Arrange
            var created = await _faqAppService.CreateAsync(new CreateUniversityFAQDto
            {
                UniversityId = 1,
                Question = "Test Question",
                QuestionAr = "سؤال تجريبي",
                Answer = "Test Answer",
                AnswerAr = "جواب تجريبي"
            });

            // Act
            var result = await _faqAppService.GetAsync(created.Id);

            // Assert
            result.ShouldNotBeNull();
            result.Question.ShouldBe("Test Question");
            result.QuestionAr.ShouldBe("سؤال تجريبي");
        }

        [Fact]
        public async Task GetByUniversityAsync_Should_Return_All_FAQs()
        {
            // Arrange
            var universityId = 1L;
            await _faqAppService.CreateAsync(new CreateUniversityFAQDto
            {
                UniversityId = universityId,
                Question = "Q1",
                QuestionAr = "س1",
                Answer = "A1",
                AnswerAr = "ج1"
            });

            await _faqAppService.CreateAsync(new CreateUniversityFAQDto
            {
                UniversityId = universityId,
                Question = "Q2",
                QuestionAr = "س2",
                Answer = "A2",
                AnswerAr = "ج2"
            });

            // Act
            var result = await _faqAppService.GetByUniversityAsync(universityId);

            // Assert
            result.Items.Count.ShouldBeGreaterThanOrEqualTo(2);
        }

        [Fact]
        public async Task GetPublishedByUniversityAsync_Should_Return_Only_Published()
        {
            // Arrange
            var universityId = 1L;
            await _faqAppService.CreateAsync(new CreateUniversityFAQDto
            {
                UniversityId = universityId,
                Question = "Published",
                QuestionAr = "منشور",
                Answer = "Ans",
                AnswerAr = "جواب",
                IsPublished = true
            });

            await _faqAppService.CreateAsync(new CreateUniversityFAQDto
            {
                UniversityId = universityId,
                Question = "Not Published",
                QuestionAr = "غير منشور",
                Answer = "Ans",
                AnswerAr = "جواب",
                IsPublished = false
            });

            // Act
            var result = await _faqAppService.GetPublishedByUniversityAsync(universityId);

            // Assert
            result.Items.ShouldAllBe(f => f.IsPublished);
        }

        [Fact]
        public async Task TogglePublishAsync_Should_Toggle_Status()
        {
            // Arrange
            var created = await _faqAppService.CreateAsync(new CreateUniversityFAQDto
            {
                UniversityId = 1,
                Question = "Toggle Test",
                QuestionAr = "اختبار",
                Answer = "Answer",
                AnswerAr = "جواب",
                IsPublished = false
            });

            // Act
            await _faqAppService.TogglePublishAsync(created.Id);

            // Assert
            var result = await _faqAppService.GetAsync(created.Id);
            result.IsPublished.ShouldBeTrue();
        }

        [Fact]
        public async Task ReorderAsync_Should_Update_DisplayOrder()
        {
            // Arrange
            var faq1 = await _faqAppService.CreateAsync(new CreateUniversityFAQDto
            {
                UniversityId = 1,
                Question = "Q1",
                QuestionAr = "س1",
                Answer = "A1",
                AnswerAr = "ج1",
                DisplayOrder = 1
            });

            var faq2 = await _faqAppService.CreateAsync(new CreateUniversityFAQDto
            {
                UniversityId = 1,
                Question = "Q2",
                QuestionAr = "س2",
                Answer = "A2",
                AnswerAr = "ج2",
                DisplayOrder = 2
            });

            // Act - Swap order
            await _faqAppService.ReorderAsync(new List<FAQOrderDto>
            {
                new FAQOrderDto { Id = faq2.Id, DisplayOrder = 1 },
                new FAQOrderDto { Id = faq1.Id, DisplayOrder = 2 }
            });

            // Assert
            var result1 = await _faqAppService.GetAsync(faq1.Id);
            var result2 = await _faqAppService.GetAsync(faq2.Id);
            result1.DisplayOrder.ShouldBe(2);
            result2.DisplayOrder.ShouldBe(1);
        }
    }
}
