using AutoMapper;
using ANLASH.LanguageCenters;

namespace ANLASH.LanguageCenters.Dto
{
    /// <summary>
    /// AutoMapper Profile for Language Centers Module
    /// Maps all DTOs to entities and vice versa
    /// </summary>
    public class LanguageCenterMapProfile : Profile
    {
        public LanguageCenterMapProfile()
        {
            // LanguageCenter Mappings
            CreateMap<LanguageCenter, LanguageCenterDto>()
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country != null ? src.Country.Name : null))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City != null ? src.City.Name : null));

            CreateMap<CreateLanguageCenterDto, LanguageCenter>()
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.City, opt => opt.Ignore());

            CreateMap<UpdateLanguageCenterDto, LanguageCenter>()
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.City, opt => opt.Ignore());

            CreateMap<LanguageCenter, LanguageCenterDetailDto>()
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country != null ? src.Country.Name : null))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City != null ? src.City.Name : null))
                .ForMember(dest => dest.Courses, opt => opt.Ignore())
                .ForMember(dest => dest.FAQs, opt => opt.Ignore())
                .ForMember(dest => dest.TotalCourses, opt => opt.Ignore())
                .ForMember(dest => dest.TotalPublishedFAQs, opt => opt.Ignore());

            // LanguageCourse Mappings
            CreateMap<LanguageCourse, LanguageCourseDto>()
                .ForMember(dest => dest.LanguageCenterName, opt => opt.Ignore());

            CreateMap<CreateLanguageCourseDto, LanguageCourse>();
            CreateMap<UpdateLanguageCourseDto, LanguageCourse>();

            // CoursePricing Mappings
            CreateMap<CoursePricing, CoursePricingDto>()
                .ForMember(dest => dest.CourseName, opt => opt.Ignore())
                .ForMember(dest => dest.CurrencyCode, opt => opt.Ignore())
                .ForMember(dest => dest.CurrencySymbol, opt => opt.Ignore());

            CreateMap<CreateCoursePricingDto, CoursePricing>()
                .ForMember(dest => dest.Currency, opt => opt.Ignore());

            CreateMap<UpdateCoursePricingDto, CoursePricing>()
                .ForMember(dest => dest.Currency, opt => opt.Ignore());

            // LanguageCenterFAQ Mappings
            CreateMap<LanguageCenterFAQ, LanguageCenterFAQDto>()
                .ForMember(dest => dest.LanguageCenterName, opt => opt.Ignore());

            CreateMap<CreateLanguageCenterFAQDto, LanguageCenterFAQ>();
            CreateMap<UpdateLanguageCenterFAQDto, LanguageCenterFAQ>();
        }
    }
}
