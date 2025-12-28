using AutoMapper;
using ANLASH.Storage.Dto;

namespace ANLASH.Storage
{
    /// <summary>
    /// AutoMapper configuration for Storage module
    /// </summary>
    public class StorageMapProfile : Profile
    {
        public StorageMapProfile()
        {
            // AppBinaryObject <-> FileDto
            CreateMap<AppBinaryObject, FileDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
                .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => src.ContentType))
                .ForMember(dest => dest.FileSize, opt => opt.MapFrom(src => src.FileSize))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.EntityType, opt => opt.MapFrom(src => src.EntityType))
                .ForMember(dest => dest.EntityId, opt => opt.MapFrom(src => src.EntityId))
                .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Width))
                .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
                .ForMember(dest => dest.CreationTime, opt => opt.MapFrom(src => src.CreationTime));
        }
    }
}
