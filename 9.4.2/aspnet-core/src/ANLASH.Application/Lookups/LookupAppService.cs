using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using ANLASH.Lookups.Dto;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ANLASH.Lookups
{
    /// <summary>
    /// خدمة البيانات المرجعية - Lookup Application Service
    /// <para>توفر الوصول للعملات والدول والمدن</para>
    /// <para>Provides access to currencies, countries, and cities</para>
    /// </summary>
    public class LookupAppService : ANLASHAppServiceBase, ILookupAppService
    {
        private readonly IRepository<Currency, int> _currencyRepository;
        private readonly IRepository<Country, int> _countryRepository;
        private readonly IRepository<City, int> _cityRepository;

        public LookupAppService(
            IRepository<Currency, int> currencyRepository,
            IRepository<Country, int> countryRepository,
            IRepository<City, int> cityRepository)
        {
            _currencyRepository = currencyRepository;
            _countryRepository = countryRepository;
            _cityRepository = cityRepository;
        }

        /// <summary>
        /// جلب جميع العملات النشطة - Get all active currencies
        /// </summary>
        public async Task<ListResultDto<CurrencyDto>> GetAllCurrenciesAsync()
        {
            var currencies = await _currencyRepository
                .GetAll()
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();

            return new ListResultDto<CurrencyDto>(
                ObjectMapper.Map<List<CurrencyDto>>(currencies)
            );
        }

        /// <summary>
        /// جلب جميع الدول النشطة - Get all active countries
        /// </summary>
        public async Task<ListResultDto<CountryDto>> GetAllCountriesAsync()
        {
            var countries = await _countryRepository
                .GetAll()
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();

            return new ListResultDto<CountryDto>(
                ObjectMapper.Map<List<CountryDto>>(countries)
            );
        }

        /// <summary>
        /// جلب جميع المدن - Get all cities
        /// </summary>
        public async Task<ListResultDto<CityDto>> GetAllCitiesAsync()
        {
            var cities = await _cityRepository
                .GetAll()
                .Include(c => c.Country)
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();

            var cityDtos = cities.Select(city => new CityDto
            {
                Id = city.Id,
                Name = city.Name,
                NameAr = city.NameAr,
                CountryId = city.CountryId,
                CountryName = city.Country?.Name,
                CountryNameAr = city.Country?.NameAr,
                IsActive = city.IsActive
            }).ToList();

            return new ListResultDto<CityDto>(cityDtos);
        }

        /// <summary>
        /// جلب المدن حسب الدولة - Get cities by country
        /// <para>يُستخدم في القوائم المنسدلة المتتالية</para>
        /// </summary>
        public async Task<ListResultDto<CityDto>> GetCitiesByCountryAsync(int countryId)
        {
            var cities = await _cityRepository
                .GetAll()
                .Include(c => c.Country)
                .Where(c => c.CountryId == countryId && c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();

            var cityDtos = cities.Select(city => new CityDto
            {
                Id = city.Id,
                Name = city.Name,
                NameAr = city.NameAr,
                CountryId = city.CountryId,
                CountryName = city.Country?.Name,
                CountryNameAr = city.Country?.NameAr,
                IsActive = city.IsActive
            }).ToList();

            return new ListResultDto<CityDto>(cityDtos);
        }
    }
}
