using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{
    public class DimStateTO
    {
        #region Declarations
        Int32 idState;
        String stateCode;
        String stateName;
        String country;
        String stateOrUTCode;
        Int32 countryId;
        string countryCode;
        String mappedTxnId;
        #endregion

        #region Constructor
        public DimStateTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdState
        {
            get { return idState; }
            set { idState = value; }
        }
        public String StateCode
        {
            get { return stateCode; }
            set { stateCode = value; }
        }
        public String StateName
        {
            get { return stateName; }
            set { stateName = value; }
        }
        public String CountryName
        {
            get { return country; }
            set { country = value; }
        }
        public String StateOrUTCode
        {
            get { return stateOrUTCode; }
            set { stateOrUTCode = value; }
        }

        public int CountryId { get => countryId; set => countryId = value; }
        public string CountryCode { get => countryCode; set => countryCode = value; }
        public string MappedTxnId { get => mappedTxnId; set => mappedTxnId = value; }
        #endregion
    }
}
