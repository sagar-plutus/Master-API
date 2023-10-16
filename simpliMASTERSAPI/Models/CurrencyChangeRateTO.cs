using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class CurrencyChangeRateTO
    {
        #region Declarations
        Int32 idCurExchangeRate;
        String baseCurrencyCode;
        String responseJsonData;
        DateTime exchangeRateDate;
        DateTime executedOn;
        Decimal currencyRate;
        #endregion

        #region Constructor
        public CurrencyChangeRateTO()
        {
        }
        #endregion

        #region GetSet
        public int IdCurExchangeRate { get => idCurExchangeRate; set => idCurExchangeRate = value; }
        public string BaseCurrencyCode { get => baseCurrencyCode; set => baseCurrencyCode = value; }
        public string ResponseJsonData { get => responseJsonData; set => responseJsonData = value; }
        public DateTime ExchangeRateDate { get => exchangeRateDate; set => exchangeRateDate = value; }
        public DateTime ExecutedOn { get => executedOn; set => executedOn = value; }
        public decimal CurrencyRate { get => currencyRate; set => currencyRate = value; }
        #endregion
    }
}
