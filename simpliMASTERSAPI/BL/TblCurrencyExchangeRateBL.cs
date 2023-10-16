using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL
{
    public class TblCurrencyExchangeRateBL : ITblCurrencyExchangeRateBL
    {
        private readonly ITblCurrencyExchangeRateDAO _iTblCurrencyExchangeRateDAO;
        private readonly ICommon _iCommon;
        private readonly IConnectionString _iConnectionString;
        private readonly IDimensionDAO _iDimensionDAO;
        public TblCurrencyExchangeRateBL(ITblCurrencyExchangeRateDAO iTblCurrencyExchangeRateDAO, IConnectionString iConnectionString, ICommon iCommon, IDimensionDAO iDimensionDAO)
        {
            _iCommon = iCommon;
            _iConnectionString = iConnectionString;
            _iTblCurrencyExchangeRateDAO = iTblCurrencyExchangeRateDAO;
            _iDimensionDAO = iDimensionDAO;
        }
        #region Selection
        public List<CurrencyChangeRateTO> SelectExchangeCurrencyRates(DateTime date, int currencyId, string currencyCode)
        {

            List<CurrencyChangeRateTO> list = _iTblCurrencyExchangeRateDAO.GetExchangeCurrencyRates(date);
            List<CurrencyChangeRateTO> currencyExchangeRate = new List<CurrencyChangeRateTO>();
            if (list != null && list.Count > 0)
            {
                dynamic obj = JObject.Parse(list[0].ResponseJsonData);

                CurrencyChangeRateTO currencyChangeTO = new CurrencyChangeRateTO();
                if (String.IsNullOrEmpty(currencyCode))
                {
                    if (currencyId != 0)
                    {
                        DropDownTO currencyTO = _iDimensionDAO.SelectCurrencyById(currencyId);
                        String currCode = currencyTO.Code;

                        Decimal decNo = System.Convert.ToDecimal(obj.rates[currCode]);
                        currencyChangeTO.CurrencyRate = 1 / decNo;
                    }
                    else
                        return null;
                }
                else
                {
                    Decimal decNo = System.Convert.ToDecimal(obj.rates[currencyCode]);
                    currencyChangeTO.CurrencyRate = 1 / decNo;

                }



                currencyChangeTO.ExecutedOn = list[0].ExecutedOn;
                currencyChangeTO.BaseCurrencyCode = list[0].BaseCurrencyCode;
                currencyChangeTO.ExchangeRateDate = list[0].ExchangeRateDate;
                currencyChangeTO.IdCurExchangeRate = list[0].IdCurExchangeRate;
                currencyExchangeRate.Add(currencyChangeTO);
            }
            return currencyExchangeRate;
            
        }

        public List<CurrencyChangeRateTO> SelectLatestCurrencyRatesForHeader(DateTime date)
        {

            List<DropDownTO> currencyList = _iDimensionDAO.SelectCurrencyForDropDown();
            List<CurrencyChangeRateTO> currencyRateList = _iTblCurrencyExchangeRateDAO.GetExchangeCurrencyRates(date);

            List<CurrencyChangeRateTO> latestCurrencyRates = new List<CurrencyChangeRateTO>();
            if (currencyRateList != null && currencyRateList.Count > 0)
            {
                dynamic obj = JObject.Parse(currencyRateList[0].ResponseJsonData);

                for(int i=0;i< currencyList.Count;i++)
                {
                    CurrencyChangeRateTO latestCurrencyTO = new CurrencyChangeRateTO();
                    latestCurrencyTO.BaseCurrencyCode = currencyList[i].Text;
                    String currCode = currencyList[i].Code;
                    Decimal decNo = System.Convert.ToDecimal(obj.rates[currCode]);
                    latestCurrencyTO.CurrencyRate = 1 / decNo;
                    latestCurrencyTO.ExecutedOn = currencyRateList[0].ExecutedOn;
                    latestCurrencyTO.IdCurExchangeRate= currencyRateList[0].IdCurExchangeRate;
                    latestCurrencyRates.Add(latestCurrencyTO);
                }


            }
            return latestCurrencyRates;

        }
        #endregion


        #region Insertion


        public int PostCurrencyExchangeRate(CurrencyChangeRateTO currencyChangeRateTO)
        {                 
            try
            {
               int result=0;
                if (currencyChangeRateTO != null)
                {
                    result = _iTblCurrencyExchangeRateDAO.InsertCurrencyExchangeRate(currencyChangeRateTO);
                    if (result != 1)
                    {
                        return 0;
                    }
                }            
             
                return 1;
            }
            catch (Exception e)
            {            
                return 0;
            }         

        }

        #endregion

    }
}

