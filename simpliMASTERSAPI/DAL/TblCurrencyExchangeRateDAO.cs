
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class TblCurrencyExchangeRateDAO : ITblCurrencyExchangeRateDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblCurrencyExchangeRateDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlQuery = "SELECT TOP 1 * FROM tblCurrencyExchangeRates";
            return sqlQuery;
        }


        #endregion

        #region Selection

        public List<CurrencyChangeRateTO> GetExchangeCurrencyRates(DateTime date)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                //cmdSelect.CommandText = SqlSelectQuery() + " WHERE DAY(executedOn) <="+ date.Day+ " and MONTH(executedOn) <="+ date.Month + " and YEAR(executedOn) <= "+ date.Year + "  order by executedOn desc ";
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE cast(executedOn as date) <= @fromDate order by executedOn desc ";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = date;
                //cmdSelect.Parameters.Add("@date", System.Data.SqlDbType.DateTime).Value = date;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(rdr);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<CurrencyChangeRateTO> ConvertDTToList(SqlDataReader CurrencyChangeRateTODT)
        {
            List<CurrencyChangeRateTO> currencyExchangeRateList = new List<CurrencyChangeRateTO>();
            if(CurrencyChangeRateTODT!=null)
            {
                while(CurrencyChangeRateTODT.Read())
                {
                    CurrencyChangeRateTO currencyChangeRateTONew = new CurrencyChangeRateTO();
                    if (CurrencyChangeRateTODT["idCurExchangeRate"] != DBNull.Value)
                        currencyChangeRateTONew.IdCurExchangeRate = Convert.ToInt32(CurrencyChangeRateTODT["idCurExchangeRate"].ToString());

                    if (CurrencyChangeRateTODT["exchangeRateDate"] != DBNull.Value)
                        currencyChangeRateTONew.ExchangeRateDate = Convert.ToDateTime(CurrencyChangeRateTODT["exchangeRateDate"].ToString());

                    if (CurrencyChangeRateTODT["baseCurrencyCode"] != DBNull.Value)
                        currencyChangeRateTONew.BaseCurrencyCode = Convert.ToString(CurrencyChangeRateTODT["baseCurrencyCode"].ToString());

                    if (CurrencyChangeRateTODT["responseJsonData"] != DBNull.Value)
                        currencyChangeRateTONew.ResponseJsonData = Convert.ToString(CurrencyChangeRateTODT["responseJsonData"].ToString());

                    if (CurrencyChangeRateTODT["executedOn"] != DBNull.Value)
                        currencyChangeRateTONew.ExecutedOn = Convert.ToDateTime(CurrencyChangeRateTODT["executedOn"].ToString());

                    currencyExchangeRateList.Add(currencyChangeRateTONew);
                }
            }
            return currencyExchangeRateList;
        }

        #endregion


        #region Insertion


        public int InsertCurrencyExchangeRate(CurrencyChangeRateTO currencyChangeRateTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlCommand cmdInsert = new SqlCommand();
            SqlConnection conn = new SqlConnection(sqlConnStr);
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(currencyChangeRateTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public  int ExecuteInsertionCommand(CurrencyChangeRateTO currencyChangeRateTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblCurrencyExchangeRates]( " +
          
            " [exchangeRateDate]" +
              ",[baseCurrencyCode]"+
            " ,[responseJsonData]" +
            " ,[executedOn]" +
           
            " )" +
" VALUES (" +

            " @exchangeRateDate " +
             " ,@baseCurrencyCode " +
            " ,@responseJsonData " +
            " ,@executedOn " +              
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

         //   cmdInsert.Parameters.Add("@IdAttributesStatus", System.Data.SqlDbType.Int).Value = tblAttributeStatusTO.IdAttributesStatus;
            cmdInsert.Parameters.Add("@exchangeRateDate", System.Data.SqlDbType.DateTime).Value = currencyChangeRateTO.ExchangeRateDate;
            cmdInsert.Parameters.Add("@baseCurrencyCode", System.Data.SqlDbType.NVarChar).Value = currencyChangeRateTO.BaseCurrencyCode;
            cmdInsert.Parameters.Add("@responseJsonData", System.Data.SqlDbType.NVarChar).Value = currencyChangeRateTO.ResponseJsonData;
            cmdInsert.Parameters.Add("@executedOn", System.Data.SqlDbType.DateTime).Value = currencyChangeRateTO.ExecutedOn;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                currencyChangeRateTO.IdCurExchangeRate = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion


    }
}

