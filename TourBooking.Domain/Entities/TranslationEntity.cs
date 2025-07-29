using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourBooking.Domain.Entities
{
    public class TranslationEntity : IBaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Guid LanguageId { get; set; }
        public LanguageEntity Language { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class VehicleTypeTranslation : TranslationEntity { }

    public class VehicleBrandTranslation : TranslationEntity { }

    public class TourTypeTranslation : TranslationEntity
    {       
     }

    public class TourDifficultyTranslation : TranslationEntity { }

    public class LanguageTranslation : TranslationEntity { }

    public class TourPointTranslation : TranslationEntity
    {
         public Guid TourPointId { get; set; }
        public TourPointEntity TourPoint { get; set; }

     }

    public class RegionTranslation : TranslationEntity
    {
        public Guid RegionId { get; set; }
        public RegionEntity Region { get; set; }
    }

    public class DistrictTranslation : TranslationEntity
    {
        public Guid DistrictId { get; set; }
        public DistrictEntity District { get; set; }
    }

    public class CityTranslation : TranslationEntity
    {
        public Guid CityId { get; set; }
        public CityEntity City { get; set; }
    }

    public class CountryTranslation : TranslationEntity
    {
        public Guid CountryId { get; set; }
        public CountryEntity Country { get; set; }
    }
}
