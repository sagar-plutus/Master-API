using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblCurrencyExchangeRateBL
    {
        int PostCurrencyExchangeRate(CurrencyChangeRateTO currencyChangeRateTO);
        List<CurrencyChangeRateTO> SelectExchangeCurrencyRates(DateTime date, int currencyId, string currencyCode="");
        List<CurrencyChangeRateTO> SelectLatestCurrencyRatesForHeader(DateTime date);

    }
}
