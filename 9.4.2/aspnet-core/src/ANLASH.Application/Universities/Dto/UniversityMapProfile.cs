using AutoMapper;
using ANLASH.Universities;

namespace ANLASH.Universities.Dto
{
    public class UniversityMapProfile : Profile
    {
        public UniversityMapProfile()
        {
            // University -> UniversityDto
            CreateMap<University, UniversityDto>()
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.CountryName))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.CityName));
            
            // CreateUniversityDto -> University
            CreateMap<CreateUniversityDto, University>()
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.City, opt => opt.Ignore());
            
            // UpdateUniversityDto -> University
            CreateMap<UpdateUniversityDto, University>()
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.City, opt => opt.Ignore());

            // UniversityProgram Mappings
            CreateMap<UniversityProgram, UniversityProgramDto>();
            CreateMap<CreateUniversityProgramDto, UniversityProgram>();
            CreateMap<UpdateUniversityProgramDto, UniversityProgram>();

            // UniversityContent Mappings
            CreateMap<UniversityContent, UniversityContentDto>();
            CreateMap<CreateUniversityContentDto, UniversityContent>();
            CreateMap<UpdateUniversityContentDto, UniversityContent>();

            // UniversityFAQ Mappings
            CreateMap<UniversityFAQ, UniversityFAQDto>()
                .ForMember(dest => dest.UniversityName, opt => opt.MapFrom(src => src.University != null ? src.University.Name : null))
                .ForMember(dest => dest.UniversityNameAr, opt => opt.MapFrom(src => src.University != null ? src.University.NameAr : null));
            CreateMap<CreateUniversityFAQDto, UniversityFAQ>();
            CreateMap<UpdateUniversityFAQDto, UniversityFAQ>();
        }
    }
}
