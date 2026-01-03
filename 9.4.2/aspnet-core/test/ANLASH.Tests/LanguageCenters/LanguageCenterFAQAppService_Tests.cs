using ANLASH.LanguageCenters;
using ANLASH.LanguageCenters.Dto;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Abp.Application.Services.Dto;

namespace ANLASH.Tests.LanguageCenters
{
    /// <summary>
    /// اختبارات تكامل شاملة لخدمة الأسئلة الشائعة للمعاهد
    /// Comprehensive Integration Tests for Language Center FAQ App Service
    /// </summary>
    public class LanguageCenterFAQAppService_Tests : ANLASHTestBase
    {
        private readonly ILanguageCenterFAQAppService _faqAppService;
        private readonly ILanguageCenterAppService _languageCenterAppService;

        public LanguageCenterFAQAppService_Tests()
        {
            _faqAppService = Resolve<ILanguageCenterFAQAppService>();
            _languageCenterAppService = Resolve<ILanguageCenterAppService>();
        }

        private async Task<long> CreateTestLanguageCenterAsync()
        {
            var createDto = new CreateLanguageCenterDto
            {
                Name = "Test Center for FAQs",
                NameAr = "معهد اختبار للأسئلة",
                CountryId = 1,
                CityId = 1,
                Slug = "test-center-faqs",
                SlugAr = "معهد-اختبار-أسئلة",
                Rating = 4.0m
            };
            var result = await _languageCenterAppService.CreateAsync(createDto);
            return result.Id;
        }

        [Fact]
        public async Task Should_Create_Valid_FAQ()
        {
            // Arrange
            var centerId = await CreateTestLanguageCenterAsync();
            var createDto = new CreateLanguageCenterFAQDto
            {
                LanguageCenterId = centerId,
                Question = "What are the course timings?",
                QuestionAr = "ما هي مواعيد الدورات؟",
                Answer = "Courses run from 9 AM to 3 PM",
                AnswerAr = "تبدأ الدورات من 9 صباحاً حتى 3 عصراً",
                Category = "General",
                CategoryAr = "عام",
                IsPublished = true,
                DisplayOrder = 1
            };

            // Act
            var result = await _faqAppService.CreateAsync(createDto);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBeGreaterThan(0);
            result.Question.ShouldBe("What are the course timings?");
            result.IsPublished.ShouldBeTrue();
            result.ViewCount.ShouldBe(0);
            result.HelpfulCount.ShouldBe(0);
        }

        [Fact]
        public async Task Should_Get_FAQs_By_LanguageCenter()
        {
            // Arrange
            var centerId = await CreateTestLanguageCenterAsync();
            
            // Create multiple FAQs
            for (int i = 0; i < 5; i++)
            {
                var createDto = new CreateLanguageCenterFAQDto
                {
                    LanguageCenterId = centerId,
                    Question = $"Question {i}?",
                    QuestionAr = $"سؤال {i}؟",
                    Answer = $"Answer {i}",
                    AnswerAr = $"جواب {i}",
                    Category = "General",
                    CategoryAr = "عام",
                    DisplayOrder = i
                };
                await _faqAppService.CreateAsync(createDto);
            }

            // Act
            var result = await _faqAppService.GetByLanguageCenterAsync(centerId);

            // Assert
            result.ShouldNotBeNull();
            result.Items.Count.ShouldBe(5);
        }

        [Fact]
        public async Task Should_Get_Published_FAQs_Only()
        {
            // Arrange
            var centerId = await CreateTestLanguageCenterAsync();
            
            // Create published FAQ
            var publishedDto = new CreateLanguageCenterFAQDto
            {
                LanguageCenterId = centerId,
                Question = "Published Question?",
                QuestionAr = "سؤال منشور؟",
                Answer = "Published Answer",
                AnswerAr = "جواب منشور",
                Category = "General",
                CategoryAr = "عام",
                IsPublished = true
            };
            await _faqAppService.CreateAsync(publishedDto);

            // Create unpublished FAQ
            var unpublishedDto = new CreateLanguageCenterFAQDto
            {
                LanguageCenterId = centerId,
                Question = "Unpublished Question?",
                QuestionAr = "سؤال غير منشور؟",
                Answer = "Unpublished Answer",
                AnswerAr = "جواب غير منشور",
                Category = "General",
                CategoryAr = "عام",
                IsPublished = false
            };
            await _faqAppService.CreateAsync(unpublishedDto);

            // Act
            var result = await _faqAppService.GetPublishedByLanguageCenterAsync(centerId);

            // Assert
            result.ShouldNotBeNull();
            result.Items.ShouldAllBe(f => f.IsPublished == true);
        }

        [Fact]
        public async Task Should_Get_FAQs_By_Category()
        {
            // Arrange
            var centerId = await CreateTestLanguageCenterAsync();
            
            // Create FAQs in different categories
            var admissionDto = new CreateLanguageCenterFAQDto
            {
                LanguageCenterId = centerId,
                Question = "How to apply?",
                QuestionAr = "كيف أتقدم؟",
                Answer = "Fill the online form",
                AnswerAr = "املأ النموذج الإلكتروني",
                Category = "Admission",
                CategoryAr = "القبول",
                IsPublished = true
            };
            await _faqAppService.CreateAsync(admissionDto);

            var feesDto = new CreateLanguageCenterFAQDto
            {
                LanguageCenterId = centerId,
                Question = "What are the fees?",
                QuestionAr = "ما هي الرسوم؟",
                Answer = "Fees vary by course",
                AnswerAr = "تختلف الرسوم حسب الدورة",
                Category = "Fees",
                CategoryAr = "الرسوم",
                IsPublished = true
            };
            await _faqAppService.CreateAsync(feesDto);

            // Act
            var result = await _faqAppService.GetByCategoryAsync(centerId, "Admission");

            // Assert
            result.ShouldNotBeNull();
            result.Items.ShouldAllBe(f => f.Category == "Admission");
        }

        [Fact]
        public async Task Should_Get_Featured_FAQs()
        {
            // Arrange
            var centerId = await CreateTestLanguageCenterAsync();
            
            // Create featured FAQs
            for (int i = 0; i < 3; i++)
            {
                var createDto = new CreateLanguageCenterFAQDto
                {
                    LanguageCenterId = centerId,
                    Question = $"Featured Question {i}?",
                    QuestionAr = $"سؤال مميز {i}؟",
                    Answer = $"Featured Answer {i}",
                    AnswerAr = $"جواب مميز {i}",
                    Category = "General",
                    CategoryAr = "عام",
                    IsPublished = true,
                    IsFeatured = true,
                    DisplayOrder = i
                };
                await _faqAppService.CreateAsync(createDto);
            }

            // Create non-featured FAQ
            var normalDto = new CreateLanguageCenterFAQDto
            {
                LanguageCenterId = centerId,
                Question = "Normal Question?",
                QuestionAr = "سؤال عادي؟",
                Answer = "Normal Answer",
                AnswerAr = "جواب عادي",
                Category = "General",
                CategoryAr = "عام",
                IsPublished = true,
                IsFeatured = false
            };
            await _faqAppService.CreateAsync(normalDto);

            // Act
            var result = await _faqAppService.GetFeaturedAsync(centerId);

            // Assert
            result.ShouldNotBeNull();
            result.Items.ShouldAllBe(f => f.IsFeatured == true);
        }

        [Fact]
        public async Task Should_Toggle_Publish_Status()
        {
            // Arrange
            var centerId = await CreateTestLanguageCenterAsync();
            var createDto = new CreateLanguageCenterFAQDto
            {
                LanguageCenterId = centerId,
                Question = "Toggle Test?",
                QuestionAr = "اختبار التبديل؟",
                Answer = "Toggle Answer",
                AnswerAr = "جواب التبديل",
                Category = "General",
                CategoryAr = "عام",
                IsPublished = true
            };
            var created = await _faqAppService.CreateAsync(createDto);

            // Act
            await _faqAppService.TogglePublishAsync(created.Id);

            // Assert
            var updated = await _faqAppService.GetAsync(new EntityDto<long>(created.Id));
            updated.IsPublished.ShouldBeFalse();
        }

        [Fact]
        public async Task Should_Toggle_Featured_Status()
        {
            // Arrange
            var centerId = await CreateTestLanguageCenterAsync();
            var createDto = new CreateLanguageCenterFAQDto
            {
                LanguageCenterId = centerId,
                Question = "Featured Toggle?",
                QuestionAr = "تبديل المميز؟",
                Answer = "Featured Answer",
                AnswerAr = "جواب مميز",
                Category = "General",
                CategoryAr = "عام",
                IsFeatured = false
            };
            var created = await _faqAppService.CreateAsync(createDto);

            // Act
            await _faqAppService.ToggleFeaturedAsync(created.Id);

            // Assert
            var updated = await _faqAppService.GetAsync(new EntityDto<long>(created.Id));
            updated.IsFeatured.ShouldBeTrue();
        }

        [Fact]
        public async Task Should_Record_View()
        {
            // Arrange
            var centerId = await CreateTestLanguageCenterAsync();
            var createDto = new CreateLanguageCenterFAQDto
            {
                LanguageCenterId = centerId,
                Question = "View Test?",
                QuestionAr = "اختبار المشاهدة؟",
                Answer = "View Answer",
                AnswerAr = "جواب المشاهدة",
                Category = "General",
                CategoryAr = "عام"
            };
            var created = await _faqAppService.CreateAsync(createDto);
            created.ViewCount.ShouldBe(0);

            // Act
            await _faqAppService.RecordViewAsync(created.Id);
            await _faqAppService.RecordViewAsync(created.Id);

            // Assert
            var updated = await _faqAppService.GetAsync(new EntityDto<long>(created.Id));
            updated.ViewCount.ShouldBe(2);
        }

        [Fact]
        public async Task Should_Record_Helpful_Rating()
        {
            // Arrange
            var centerId = await CreateTestLanguageCenterAsync();
            var createDto = new CreateLanguageCenterFAQDto
            {
                LanguageCenterId = centerId,
                Question = "Rating Test?",
                QuestionAr = "اختبار التقييم؟",
                Answer = "Rating Answer",
                AnswerAr = "جواب التقييم",
                Category = "General",
                CategoryAr = "عام"
            };
            var created = await _faqAppService.CreateAsync(createDto);

            // Act - Record helpful
            await _faqAppService.RecordRatingAsync(created.Id, true);
            await _faqAppService.RecordRatingAsync(created.Id, true);

            // Assert
            var updated = await _faqAppService.GetAsync(new EntityDto<long>(created.Id));
            updated.HelpfulCount.ShouldBe(2);
            updated.NotHelpfulCount.ShouldBe(0);
        }

        [Fact]
        public async Task Should_Record_NotHelpful_Rating()
        {
            // Arrange
            var centerId = await CreateTestLanguageCenterAsync();
            var createDto = new CreateLanguageCenterFAQDto
            {
                LanguageCenterId = centerId,
                Question = "Not Helpful Test?",
                QuestionAr = "اختبار غير مفيد؟",
                Answer = "Not Helpful Answer",
                AnswerAr = "جواب غير مفيد",
                Category = "General",
                CategoryAr = "عام"
            };
            var created = await _faqAppService.CreateAsync(createDto);

            // Act - Record not helpful
            await _faqAppService.RecordRatingAsync(created.Id, false);

            // Assert
            var updated = await _faqAppService.GetAsync(new EntityDto<long>(created.Id));
            updated.HelpfulCount.ShouldBe(0);
            updated.NotHelpfulCount.ShouldBe(1);
        }

        [Fact]
        public async Task Should_Reorder_FAQs()
        {
            // Arrange
            var centerId = await CreateTestLanguageCenterAsync();
            
            // Create FAQs
            var faq1 = await _faqAppService.CreateAsync(new CreateLanguageCenterFAQDto
            {
                LanguageCenterId = centerId,
                Question = "First?",
                QuestionAr = "الأول؟",
                Answer = "First",
                AnswerAr = "الأول",
                Category = "General",
                CategoryAr = "عام",
                DisplayOrder = 1
            });

            var faq2 = await _faqAppService.CreateAsync(new CreateLanguageCenterFAQDto
            {
                LanguageCenterId = centerId,
                Question = "Second?",
                QuestionAr = "الثاني؟",
                Answer = "Second",
                AnswerAr = "الثاني",
                Category = "General",
                CategoryAr = "عام",
                DisplayOrder = 2
            });

            // Act - Swap order
            var orders = new[]
            {
                new FAQOrderDto { Id = faq1.Id, DisplayOrder = 2 },
                new FAQOrderDto { Id = faq2.Id, DisplayOrder = 1 }
            };
            await _faqAppService.ReorderAsync(orders);

            // Assert
            var updated1 = await _faqAppService.GetAsync(new EntityDto<long>(faq1.Id));
            var updated2 = await _faqAppService.GetAsync(new EntityDto<long>(faq2.Id));
            
            updated1.DisplayOrder.ShouldBe(2);
            updated2.DisplayOrder.ShouldBe(1);
        }
    }
}
