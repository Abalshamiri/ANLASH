using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ANLASH.Lookups.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ANLASH.Lookups
{
    /// <summary>
    /// واجهة خدمة البيانات المرجعية - Lookup Application Service Interface
    /// <para>توفر جميع البيانات المرجعية (العملات، الدول، المدن)</para>
    /// <para>Provides all lookup data (currencies, countries, cities)</para>
    /// </summary>
    public interface ILookupAppService : IApplicationService
    {
        /// <summary>
        /// جلب جميع العملات النشطة - Get all active currencies
        /// <para>يُستخدم في القوائم المنسدلة للعملات</para>
        /// </summary>
        Task<ListResultDto<CurrencyDto>> GetAllCurrenciesAsync();

        /// <summary>
        /// جلب جميع الدول النشطة - Get all active countries
        /// <para>يُستخدم في القوائم المنسدلة للدول</para>
        /// </summary>
        Task<ListResultDto<CountryDto>> GetAllCountriesAsync();

        /// <summary>
        /// جلب جميع المدن - Get all cities
        /// </summary>
        Task<ListResultDto<CityDto>> GetAllCitiesAsync();

        /// <summary>
        /// جلب المدن حسب الدولة - Get cities by country
        /// <para>يُستخدم في القوائم المنسدلة المتتالية (Country → Cities)</para>
        /// </summary>
        /// <param name="countryId">معرّف الدولة - Country ID</param>
        Task<ListResultDto<CityDto>> GetCitiesByCountryAsync(int countryId);
    }
}
