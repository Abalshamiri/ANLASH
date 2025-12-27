using System.Collections.Generic;
using System.Linq;
using ANLASH.Lookups;

namespace ANLASH.EntityFrameworkCore.Seed.Host
{
    /// <summary>
    /// منشئ البيانات الأولية للعملات والدول والمدن
    /// Default Lookups Data Creator
    /// </summary>
    public class DefaultLookupsCreator
    {
        private readonly ANLASHDbContext _context;

        public DefaultLookupsCreator(ANLASHDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateCurrencies();
            CreateCountries();
            CreateCities();
        }

        #region Currencies - العملات

        private void CreateCurrencies()
        {
            var currencies = new List<Currency>
            {
                // ✅ Middle East Currencies - عملات الشرق الأوسط
                new Currency { Code = "SAR", Name = "Saudi Riyal", NameAr = "ريال سعودي", Symbol = "﷼", IsActive = true },
                new Currency { Code = "AED", Name = "UAE Dirham", NameAr = "درهم إماراتي", Symbol = "د.إ", IsActive = true },
                new Currency { Code = "KWD", Name = "Kuwaiti Dinar", NameAr = "دينار كويتي", Symbol = "د.ك", IsActive = true },
                new Currency { Code = "QAR", Name = "Qatari Riyal", NameAr = "ريال قطري", Symbol = "ر.ق", IsActive = true },
                new Currency { Code = "OMR", Name = "Omani Rial", NameAr = "ريال عماني", Symbol = "ر.ع", IsActive = true },
                new Currency { Code = "BHD", Name = "Bahraini Dinar", NameAr = "دينار بحريني", Symbol = "د.ب", IsActive = true },
                new Currency { Code = "JOD", Name = "Jordanian Dinar", NameAr = "دينار أردني", Symbol = "د.أ", IsActive = true },
                new Currency { Code = "EGP", Name = "Egyptian Pound", NameAr = "جنيه مصري", Symbol = "ج.م", IsActive = true },
                
                // ✅ Asia Currencies - عملات آسيا
                new Currency { Code = "MYR", Name = "Malaysian Ringgit", NameAr = "رينغيت ماليزي", Symbol = "RM", IsActive = true },
                new Currency { Code = "IDR", Name = "Indonesian Rupiah", NameAr = "روبية إندونيسية", Symbol = "Rp", IsActive = true },
                new Currency { Code = "TRY", Name = "Turkish Lira", NameAr = "ليرة تركية", Symbol = "₺", IsActive = true },
                
                // ✅ Major World Currencies - العملات العالمية الرئيسية
                new Currency { Code = "USD", Name = "US Dollar", NameAr = "دولار أمريكي", Symbol = "$", IsActive = true },
                new Currency { Code = "EUR", Name = "Euro", NameAr = "يورو", Symbol = "€", IsActive = true },
                new Currency { Code = "GBP", Name = "British Pound", NameAr = "جنيه إسترليني", Symbol = "£", IsActive = true },
                new Currency { Code = "CAD", Name = "Canadian Dollar", NameAr = "دولار كندي", Symbol = "C$", IsActive = true },
                new Currency { Code = "AUD", Name = "Australian Dollar", NameAr = "دولار أسترالي", Symbol = "A$", IsActive = true }
            };

            // إضافة فقط العملات غير الموجودة
            foreach (var currency in currencies)
            {
                if (!_context.Currencies.Any(c => c.Code == currency.Code))
                {
                    _context.Currencies.Add(currency);
                }
            }

            _context.SaveChanges();
        }

        #endregion

        #region Countries - الدول

        private void CreateCountries()
        {
            var countries = new List<Country>
            {
                // ✅ Gulf Countries - دول الخليج
                new Country { Code = "SA", Name = "Saudi Arabia", NameAr = "المملكة العربية السعودية", IsActive = true },
                new Country { Code = "AE", Name = "United Arab Emirates", NameAr = "الإمارات العربية المتحدة", IsActive = true },
                new Country { Code = "KW", Name = "Kuwait", NameAr = "الكويت", IsActive = true },
                new Country { Code = "QA", Name = "Qatar", NameAr = "قطر", IsActive = true },
                new Country { Code = "OM", Name = "Oman", NameAr = "سلطنة عمان", IsActive = true },
                new Country { Code = "BH", Name = "Bahrain", NameAr = "البحرين", IsActive = true },
                
                // ✅ Arab Countries - الدول العربية
                new Country { Code = "EG", Name = "Egypt", NameAr = "مصر", IsActive = true },
                new Country { Code = "JO", Name = "Jordan", NameAr = "الأردن", IsActive = true },
                new Country { Code = "LB", Name = "Lebanon", NameAr = "لبنان", IsActive = true },
                new Country { Code = "SY", Name = "Syria", NameAr = "سوريا", IsActive = true },
                new Country { Code = "IQ", Name = "Iraq", NameAr = "العراق", IsActive = true },
                new Country { Code = "YE", Name = "Yemen", NameAr = "اليمن", IsActive = true },
                new Country { Code = "PS", Name = "Palestine", NameAr = "فلسطين", IsActive = true },
                
                // ✅ North African Countries - دول شمال أفريقيا
                new Country { Code = "MA", Name = "Morocco", NameAr = "المغرب", IsActive = true },
                new Country { Code = "DZ", Name = "Algeria", NameAr = "الجزائر", IsActive = true },
                new Country { Code = "TN", Name = "Tunisia", NameAr = "تونس", IsActive = true },
                new Country { Code = "LY", Name = "Libya", NameAr = "ليبيا", IsActive = true },
                new Country { Code = "SD", Name = "Sudan", NameAr = "السودان", IsActive = true },
                
                // ✅ Asian Countries - دول آسيا
                new Country { Code = "MY", Name = "Malaysia", NameAr = "ماليزيا", IsActive = true },
                new Country { Code = "ID", Name = "Indonesia", NameAr = "إندونيسيا", IsActive = true },
                new Country { Code = "TR", Name = "Turkey", NameAr = "تركيا", IsActive = true },
                new Country { Code = "PK", Name = "Pakistan", NameAr = "باكستان", IsActive = true },
                new Country { Code = "BD", Name = "Bangladesh", NameAr = "بنجلاديش", IsActive = true },
                
                // ✅ Western Countries - الدول الغربية
                new Country { Code = "US", Name = "United States", NameAr = "الولايات المتحدة", IsActive = true },
                new Country { Code = "GB", Name = "United Kingdom", NameAr = "المملكة المتحدة", IsActive = true },
                new Country { Code = "CA", Name = "Canada", NameAr = "كندا", IsActive = true },
                new Country { Code = "AU", Name = "Australia", NameAr = "أستراليا", IsActive = true },
                new Country { Code = "DE", Name = "Germany", NameAr = "ألمانيا", IsActive = true },
                new Country { Code = "FR", Name = "France", NameAr = "فرنسا", IsActive = true }
            };

            foreach (var country in countries)
            {
                if (!_context.Countries.Any(c => c.Code == country.Code))
                {
                    _context.Countries.Add(country);
                }
            }

            _context.SaveChanges();
        }

        #endregion

        #region Cities - المدن

        private void CreateCities()
        {
            // جلب الدول من قاعدة البيانات
            var saudiArabia = _context.Countries.FirstOrDefault(c => c.Code == "SA");
            var uae = _context.Countries.FirstOrDefault(c => c.Code == "AE");
            var egypt = _context.Countries.FirstOrDefault(c => c.Code == "EG");
            var malaysia = _context.Countries.FirstOrDefault(c => c.Code == "MY");
            var turkey = _context.Countries.FirstOrDefault(c => c.Code == "TR");
            var jordan = _context.Countries.FirstOrDefault(c => c.Code == "JO");

            var cities = new List<City>();

            // ✅ Saudi Arabia Cities - مدن السعودية
            if (saudiArabia != null)
            {
                cities.AddRange(new[]
                {
                    new City { Name = "Riyadh", NameAr = "الرياض", CountryId = saudiArabia.Id, IsActive = true },
                    new City { Name = "Jeddah", NameAr = "جدة", CountryId = saudiArabia.Id, IsActive = true },
                    new City { Name = "Mecca", NameAr = "مكة المكرمة", CountryId = saudiArabia.Id, IsActive = true },
                    new City { Name = "Medina", NameAr = "المدينة المنورة", CountryId = saudiArabia.Id, IsActive = true },
                    new City { Name = "Dammam", NameAr = "الدمام", CountryId = saudiArabia.Id, IsActive = true },
                    new City { Name = "Khobar", NameAr = "الخبر", CountryId = saudiArabia.Id, IsActive = true },
                    new City { Name = "Dhahran", NameAr = "الظهران", CountryId = saudiArabia.Id, IsActive = true },
                    new City { Name = "Tabuk", NameAr = "تبوك", CountryId = saudiArabia.Id, IsActive = true },
                    new City { Name = "Abha", NameAr = "أبها", CountryId = saudiArabia.Id, IsActive = true },
                    new City { Name = "Khamis Mushait", NameAr = "خميس مشيط", CountryId = saudiArabia.Id, IsActive = true }
                });
            }

            // ✅ UAE Cities - مدن الإمارات
            if (uae != null)
            {
                cities.AddRange(new[]
                {
                    new City { Name = "Dubai", NameAr = "دبي", CountryId = uae.Id, IsActive = true },
                    new City { Name = "Abu Dhabi", NameAr = "أبوظبي", CountryId = uae.Id, IsActive = true },
                    new City { Name = "Sharjah", NameAr = "الشارقة", CountryId = uae.Id, IsActive = true },
                    new City { Name = "Ajman", NameAr = "عجمان", CountryId = uae.Id, IsActive = true },
                    new City { Name = "Ras Al Khaimah", NameAr = "رأس الخيمة", CountryId = uae.Id, IsActive = true },
                    new City { Name = "Fujairah", NameAr = "الفجيرة", CountryId = uae.Id, IsActive = true }
                });
            }

            // ✅ Egypt Cities - مدن مصر
            if (egypt != null)
            {
                cities.AddRange(new[]
                {
                    new City { Name = "Cairo", NameAr = "القاهرة", CountryId = egypt.Id, IsActive = true },
                    new City { Name = "Alexandria", NameAr = "الإسكندرية", CountryId = egypt.Id, IsActive = true },
                    new City { Name = "Giza", NameAr = "الجيزة", CountryId = egypt.Id, IsActive = true },
                    new City { Name = "Mansoura", NameAr = "المنصورة", CountryId = egypt.Id, IsActive = true },
                    new City { Name = "Tanta", NameAr = "طنطا", CountryId = egypt.Id, IsActive = true }
                });
            }

            // ✅ Malaysia Cities - مدن ماليزيا
            if (malaysia != null)
            {
                cities.AddRange(new[]
                {
                    new City { Name = "Kuala Lumpur", NameAr = "كوالالمبور", CountryId = malaysia.Id, IsActive = true },
                    new City { Name = "Johor Bahru", NameAr = "جوهور بارو", CountryId = malaysia.Id, IsActive = true },
                    new City { Name = "Penang", NameAr = "بينانج", CountryId = malaysia.Id, IsActive = true },
                    new City { Name = "Malacca", NameAr = "ملقا", CountryId = malaysia.Id, IsActive = true }
                });
            }

            // ✅ Turkey Cities - مدن تركيا
            if (turkey != null)
            {
                cities.AddRange(new[]
                {
                    new City { Name = "Istanbul", NameAr = "إسطنبول", CountryId = turkey.Id, IsActive = true },
                    new City { Name = "Ankara", NameAr = "أنقرة", CountryId = turkey.Id, IsActive = true },
                    new City { Name = "Izmir", NameAr = "إزمير", CountryId = turkey.Id, IsActive = true },
                    new City { Name = "Bursa", NameAr = "بورصة", CountryId = turkey.Id, IsActive = true }
                });
            }

            // ✅ Jordan Cities - مدن الأردن
            if (jordan != null)
            {
                cities.AddRange(new[]
                {
                    new City { Name = "Amman", NameAr = "عمّان", CountryId = jordan.Id, IsActive = true },
                    new City { Name = "Zarqa", NameAr = "الزرقاء", CountryId = jordan.Id, IsActive = true },
                    new City { Name = "Irbid", NameAr = "إربد", CountryId = jordan.Id, IsActive = true },
                    new City { Name = "Aqaba", NameAr = "العقبة", CountryId = jordan.Id, IsActive = true }
                });
            }

            // إضافة المدن غير الموجودة
            foreach (var city in cities)
            {
                if (!_context.Cities.Any(c => c.Name == city.Name && c.CountryId == city.CountryId))
                {
                    _context.Cities.Add(city);
                }
            }

            _context.SaveChanges();
        }

        #endregion
    }
}
