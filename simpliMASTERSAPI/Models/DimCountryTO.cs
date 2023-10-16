using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{
    public class DimCountryTO
    {
        #region Declarations
        Int32 idCountry;
        String countryCode;
        String countryName;
        Int32 isActive;
        Int32 countryMobileCode;
        Int32 mobNoDigitLength;
        #endregion

        #region Constructor
        public DimCountryTO()
        {
        }



        #endregion

        #region GetSet
        public int IdCountry { get => idCountry; set => idCountry = value; }
        public string CountryCode { get => countryCode; set => countryCode = value; }
        public string CountryName { get => countryName; set => countryName = value; }
        public int IsActive { get => isActive; set => isActive = value; }
        public int CountryMobileCode { get => countryMobileCode; set => countryMobileCode = value; }
        public int MobNoDigitLength { get => mobNoDigitLength; set => mobNoDigitLength = value; }
        #endregion
    }
}
