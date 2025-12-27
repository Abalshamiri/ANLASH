using ANLASH.Universities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ANLASH.EntityFrameworkCore.Seed.Host
{
    /// <summary>
    /// مُنشئ بيانات الجامعات الأولية - Initial Universities Data Creator
    /// </summary>
    public class InitialUniversitiesCreator
    {
        private readonly ANLASHDbContext _context;

        public InitialUniversitiesCreator(ANLASHDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateUniversities();
        }

        private void CreateUniversities()
        {
            // Check if universities already exist
            if (_context.Universities.IgnoreQueryFilters().Any())
            {
                return; // Universities already seeded
            }

            // ✅ Get Country and City IDs from lookup tables
            var saudiArabia = _context.Countries.FirstOrDefault(c => c.Code == "SA");
            if (saudiArabia == null) return; // Lookups not seeded yet

            var riyadh = _context.Cities.FirstOrDefault(c => c.CountryId == saudiArabia.Id && c.Name == "Riyadh");
            var jeddah = _context.Cities.FirstOrDefault(c => c.CountryId == saudiArabia.Id && c.Name == "Jeddah");
            var dhahran = _context.Cities.FirstOrDefault(c => c.CountryId == saudiArabia.Id && c.Name == "Dhahran");
            var dammam = _context.Cities.FirstOrDefault(c => c.CountryId == saudiArabia.Id && c.Name == "Dammam");

            // Sample Saudi Universities
            var universities = new[]
            {
                new University
                {
                    Name = "King Saud University",
                    NameAr = "جامعة الملك سعود",
                    Description = "The premier university in Saudi Arabia, established in 1957",
                    DescriptionAr = "الجامعة الرائدة في المملكة العربية السعودية، تأسست عام 1957",
                    CountryId = saudiArabia.Id,  // ✅ Using CountryId
                    CityId = riyadh?.Id,  // ✅ Using CityId
                    Address = "King Saud University, Riyadh 11451",
                    Type = UniversityType.Public,
                    Email = "info@ksu.edu.sa",
                    Phone = "+966 11 467 6000",
                    WebsiteUrl = "https://ksu.edu.sa",
                    Rating = 4.5m,
                    WorldRanking = 200,
                    EstablishmentYear = 1957,
                    Slug = "king-saud-university",
                    SlugAr = "جامعة-الملك-سعود",
                    IsActive = true,
                    IsFeatured = true,
                    DisplayOrder = 1
                },
                new University
                {
                    Name = "King Abdulaziz University",
                    NameAr = "جامعة الملك عبدالعزيز",
                    Description = "A leading research university in Jeddah",
                    DescriptionAr = "جامعة بحثية رائدة في جدة",
                    CountryId = saudiArabia.Id,
                    CityId = jeddah?.Id,
                    Address = "King Abdulaziz University, Jeddah 21589",
                    Type = UniversityType.Public,
                    Email = "info@kau.edu.sa",
                    Phone = "+966 12 695 2000",
                    WebsiteUrl = "https://kau.edu.sa",
                    Rating = 4.3m,
                    WorldRanking = 250,
                    EstablishmentYear = 1967,
                    Slug = "king-abdulaziz-university",
                    SlugAr = "جامعة-الملك-عبدالعزيز",
                    IsActive = true,
                    IsFeatured = true,
                    DisplayOrder = 2
                },
                new University
                {
                    Name = "King Fahd University of Petroleum and Minerals",
                    NameAr = "جامعة الملك فهد للبترول والمعادن",
                    Description = "Premier institution for science and engineering",
                    DescriptionAr = "مؤسسة رائدة في العلوم والهندسة",
                    CountryId = saudiArabia.Id,
                    CityId = dhahran?.Id,
                    Address = "KFUPM, Dhahran 31261",
                    Type = UniversityType.Public,
                    Email = "info@kfupm.edu.sa",
                    Phone = "+966 13 860 0000",
                    WebsiteUrl = "https://kfupm.edu.sa",
                    Rating = 4.7m,
                    WorldRanking = 180,
                    EstablishmentYear = 1963,
                    Slug = "kfupm",
                    SlugAr = "جامعة-الملك-فهد-للبترول-والمعادن",
                    IsActive = true,
                    IsFeatured = true,
                    DisplayOrder = 3
                },
                new University
                {
                    Name = "Imam Abdulrahman Bin Faisal University",
                    NameAr = "جامعة الإمام عبدالرحمن بن فيصل",
                    Description = "Comprehensive university in the Eastern Province",
                    DescriptionAr = "جامعة شاملة في المنطقة الشرقية",
                    CountryId = saudiArabia.Id,
                    CityId = dammam?.Id,
                    Address = "IAU, Dammam 31441",
                    Type = UniversityType.Public,
                    Email = "info@iau.edu.sa",
                    Phone = "+966 13 333 0000",
                    WebsiteUrl = "https://iau.edu.sa",
                    Rating = 4.2m,
                    WorldRanking = 450,
                    EstablishmentYear = 1975,
                    Slug = "iau",
                    SlugAr = "جامعة-الإمام-عبدالرحمن-بن-فيصل",
                    IsActive = true,
                    IsFeatured = false,
                    DisplayOrder = 4
                },
                new University
                {
                    Name = "Princess Nourah Bint Abdulrahman University",
                    NameAr = "جامعة الأميرة نورة بنت عبدالرحمن",
                    Description = "The largest women's university in the world",
                    DescriptionAr = "أكبر جامعة للبنات في العالم",
                    CountryId = saudiArabia.Id,
                    CityId = riyadh?.Id,
                    Address = "PNU, Riyadh 11671",
                    Type = UniversityType.Public,
                    Email = "info@pnu.edu.sa",
                    Phone = "+966 11 823 0000",
                    WebsiteUrl = "https://pnu.edu.sa",
                    Rating = 4.4m,
                    WorldRanking = 350,
                    EstablishmentYear = 1970,
                    Slug = "pnu",
                    SlugAr = "جامعة-الأميرة-نورة",
                    IsActive = true,
                    IsFeatured = true,
                    DisplayOrder = 5
                },
                new University
                {
                    Name = "Alfaisal University",
                    NameAr = "جامعة الفيصل",
                    Description = "Private research university in Riyadh",
                    DescriptionAr = "جامعة بحثية خاصة في الرياض",
                    CountryId = saudiArabia.Id,
                    CityId = riyadh?.Id,
                    Address = "Alfaisal University, Riyadh 11533",
                    Type = UniversityType.Private,
                    Email = "info@alfaisal.edu",
                    Phone = "+966 11 215 7777",
                    WebsiteUrl = "https://alfaisal.edu",
                    Rating = 4.3m,
                    WorldRanking = 500,
                    EstablishmentYear = 2002,
                    Slug = "alfaisal-university",
                    SlugAr = "جامعة-الفيصل",
                    IsActive = true,
                    IsFeatured = false,
                    DisplayOrder = 6
                }
            };

            _context.Universities.AddRange(universities);
            _context.SaveChanges();
        }
    }
}
