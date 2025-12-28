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
        }
    }
}
