
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblCurrencyExchangeRateDAO
    {

        int InsertCurrencyExchangeRate(CurrencyChangeRateTO currencyChangeRateTO);
        List<CurrencyChangeRateTO> GetExchangeCurrencyRates(DateTime date);

    }
}