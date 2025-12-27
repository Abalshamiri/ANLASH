using AutoMapper;
using ANLASH.Lookups;
using ANLASH.Lookups.Dto;

namespace ANLASH.Application
{
    /// <summary>
    /// AutoMapper Configuration for Lookup Entities
    /// <para>تكوين AutoMapper للكيانات المرجعية</para>
    /// </summary>
    public class LookupDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            // Currency mappings
            configuration.CreateMap<Currency, CurrencyDto>();

            // Country mappings
            configuration.CreateMap<Country, CountryDto>();

            // City mappings
            configuration.CreateMap<City, CityDto>()
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country.Name))
                .ForMember(dest => dest.CountryNameAr, opt => opt.MapFrom(src => src.Country.NameAr));
        }
    }
}
