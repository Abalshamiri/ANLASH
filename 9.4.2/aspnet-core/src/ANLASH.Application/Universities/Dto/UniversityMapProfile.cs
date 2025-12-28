using AutoMapper;
using ANLASH.Universities;

namespace ANLASH.Universities.Dto
{
    public class UniversityMapProfile : Profile
    {
        public UniversityMapProfile()
        {
            // University -> UniversityDto
            CreateMap<University, UniversityDto>();
            
            // CreateUniversityDto -> University
            CreateMap<CreateUniversityDto, University>()
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.City, opt => opt.Ignore());
            
            // UpdateUniversityDto -> University
            CreateMap<UpdateUniversityDto, University>()
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.City, opt => opt.Ignore());
        }
    }
}
